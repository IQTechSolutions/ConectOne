using ConectOne.Domain.Comparers;
using ConectOne.Domain.Extensions;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using FilingModule.Domain.Interfaces;
using GroupingModule.Domain.Entities;
using MessagingModule.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.Interfaces.SchoolEvents;
using SchoolsModule.Domain.RequestFeatures;
using SchoolsModule.Domain.Specifications;

namespace SchoolsModule.Infrastructure.Implementation.SchoolEvents
{
    /// <summary>
    /// Service for exporting school event data and learner consent statuses into Excel.
    /// </summary>
    public class SchoolEventExportService(ISchoolsModuleRepoManager schoolsModuleRepoManager, IExcelService excelService) : ISchoolEventExportService
    {
        /// <summary>
        /// Exports the consent status for each learner in an event, grouped by activity group.
        /// </summary>
        /// <param name="eventId">The unique ID of the event.</param>
        /// <param name="consentType">The type of consent to evaluate (e.g., Attendance, Transport).</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Excel file path or reference containing grouped learner consent data.</returns>
        public async Task<IBaseResult<string>> ExportEventConsents(string eventId, ConsentTypes consentType, CancellationToken cancellationToken = default)
        {
            var exportData = new Dictionary<string, List<EventConsentDto>>();
            var activityGroupResult = await schoolsModuleRepoManager.ParticipatingActivityGroups.ListAsync(new ParticipatingActivityGroupsByEventIdSpec(eventId), false, cancellationToken);

            if (!activityGroupResult.Succeeded) return await Result<string>.FailAsync(activityGroupResult.Messages);

            foreach (var group in activityGroupResult.Data.OrderBy(c => c.ActivityGroup.Name, new NaturalStringComparer()))
            {
                var learnerConsents = new Dictionary<string, EventConsentDto>();

                var categoryName = group?.ActivityGroup?.Categories?.FirstOrDefault()?.Category?.Name ?? "Uncategorized";
                var groupName = group?.ActivityGroup?.Name ?? "Unnamed Group";
                var groupLabel = $"{categoryName} {groupName}";

                foreach (var learner in group.ParticipatingTeamMembers)
                {
                    var learnerParentsSpec = new LambdaSpec<LearnerParent>(c => c.LearnerId == learner.TeamMemberId);
                    learnerParentsSpec.AddInclude(q => q.Include(c => c.Parent).ThenInclude(c => c.EmailAddresses));

                    var learnerParentsResult = await schoolsModuleRepoManager.LearnerParents.ListAsync(learnerParentsSpec, false, cancellationToken);
                    if (!learnerParentsResult.Succeeded) continue;

                    var consentSpec = new LambdaSpec<ParentPermission>(c => c.LearnerId == learner.TeamMemberId && c.ConsentType == consentType && c.EventId == eventId && c.ParticipatingActivityGroupId == group.Id);
                    var consentResult = await schoolsModuleRepoManager.ParentPermissions.ListAsync(consentSpec, false, cancellationToken);

                    if (learner.TeamMember.RequireConsentFromAllParents)
                    {
                        learnerConsents[learner.TeamMemberId] = new EventConsentDto(learner.TeamMember, consentResult.Data.Any() ? consentResult.Data.Count() == learner.TeamMember.Parents.Count() ? "true" : "false" : null, groupLabel);
                    }

                    else if (learner.TeamMember.Parents.Any(c => c.ParentConsentRequired))
                    {
                        if (consentType == ConsentTypes.Attendance)
                        {
                            learnerConsents[learner.TeamMemberId] = new EventConsentDto(
                                learner.TeamMember,
                                consentResult.Data.Any() ? consentResult.Data.Any(c => c.Granted) ? "Yes" : "No" : null,
                                groupLabel);
                        }
                        else if(consentType == ConsentTypes.Transport)
                        {
                            learnerConsents[learner.TeamMemberId] = new EventConsentDto(
                                learner.TeamMember,
                                GetTransportConsent(consentResult.Data),
                                groupLabel);
                        }
                    }
                }

                exportData[groupLabel] = learnerConsents.Values.ToList();
            }

            var exportResult = await excelService.ExportMultipleSheetsAsync(exportData, new Dictionary<string, Func<EventConsentDto, object>>
                {
                    { "Learner", item => item.Learner },
                    { "Grade", item => item.Grade },
                    { "Team", item => item.Team },
                    { "Parent Consent", item => !string.IsNullOrEmpty(item.Consent) ? item.Consent  : "TBC" }
                });

            return await Result<string>.SuccessAsync(data: exportResult.Data);
        }

        /// <summary>
        /// Determines the transport consent status based on the provided list of parent permissions.
        /// </summary>
        /// <remarks>The method evaluates the list of consents to determine the transport direction based
        /// on the <see cref="ConsentDirection"/> values. If multiple directions are granted, the method prioritizes
        /// "ToAndFrom" over individual "To" or "From" directions.</remarks>
        /// <param name="consents">A list of <see cref="ParentPermission"/> objects representing the consents granted by parents.</param>
        /// <returns>A string describing the transport consent status: <list type="bullet"> <item><description><c>null</c> if the
        /// list is empty or no relevant consent is found.</description></item> <item><description>"No" if no consents
        /// are granted.</description></item> <item><description>A string indicating the transport direction (e.g.,
        /// "Transport ToAndFrom", "Transport To", or "Transport From") based on the granted
        /// consents.</description></item> </list></returns>
        public string? GetTransportConsent(List<ParentPermission> consents)
        {
            if (!consents.Any()) return null;
            if(!consents.Any(c => c.Granted)) return "No";
            if (consents.Any(c => c.ConsentDirection == ConsentDirection.ToAndFrom)) return "Transport " + ConsentDirection.ToAndFrom.GetDescription();
            if (consents.Any(c => c.ConsentDirection == ConsentDirection.To) && consents.Any(c => c.ConsentDirection == ConsentDirection.From)) return "Transport " + ConsentDirection.ToAndFrom.GetDescription();
            if (consents.Any(c => c.ConsentDirection == ConsentDirection.To)) return "Transport " + ConsentDirection.To.GetDescription();
            if (consents.Any(c => c.ConsentDirection == ConsentDirection.From)) return "Transport " + ConsentDirection.From.GetDescription();
            return null;
        }

        /// <summary>
        /// Exports a list of upcoming school events to an Excel file.
        /// </summary>
        /// <param name="pageParameters">Filtering and paging parameters (currently unused).</param>
        /// <param name="trackChanges">Whether to enable change tracking for queried entities.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Excel file path or reference containing event data.</returns>
        public async Task<IBaseResult<string>> ExportEvents(SchoolEventPageParameters pageParameters, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var result = await schoolsModuleRepoManager.SchoolEvents.ListAsync(
                new LambdaSpec<SchoolEvent<Category<ActivityGroup>>>(c => c.StartDate.Date >= DateTime.Now.Date),
                trackChanges,
                cancellationToken);

            if (!result.Succeeded|| result.Data == null)
                return await Result<string>.FailAsync(result.Messages);

            var events = result.Data.Where(e => e != null).ToList();
            if (!events.Any())
                return await Result<string>.SuccessAsync();

            var data = await excelService.ExportAsync(result.Data, new Dictionary<string, Func<SchoolEvent<Category<ActivityGroup>>, object>>
            {
                { "Id", item => item.Id },
                { "Name", item => item.Heading },
                { "Description", item => string.IsNullOrEmpty(item.Description) ? "" : item.Description.HtmlToPlainText() },
                { "Address", item => item.Address },
                { "Start Date", item => item.StartDate.ToLongDateString() },
                { "End Date", item => item.EndDate.ToLongDateString() },
                { "Links", item => item.DocumentLinks ?? "" }
            }, sheetName: "Events");

            if (data == null || !data.Succeeded || data.Data == null)
                return await Result<string>.FailAsync(data?.Messages ?? ["Excel export failed."]);

            return await Result<string>.SuccessAsync(data : data.Data);
        }
    }
}
