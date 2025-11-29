using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
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
    /// Service for managing consent-related operations for school events.
    /// </summary>
    public class SchoolEventPermissionService(ISchoolsModuleRepoManager schoolsModuleRepoManager) : ISchoolEventPermissionService
    {
        /// <summary>
        /// Retrieves a collection of parent permissions based on the specified criteria.
        /// </summary>
        /// <remarks>This method queries parent permissions based on the provided filtering criteria and
        /// includes related learner information in the result. The returned data is mapped to <see
        /// cref="ParentPermissionDto"/> objects for easier consumption.</remarks>
        /// <param name="parameters">The request parameters containing the criteria for filtering parent permissions, such as the participating
        /// activity group ID, consent type, and consent direction.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// where <c>T</c> is an <see cref="IEnumerable{T}"/> of <see cref="ParentPermissionDto"/> objects. If the
        /// operation fails, the result includes error messages.</returns>
        public async Task<IBaseResult<IEnumerable<ParentPermissionDto>>> GetAllParentPermissions(string participatingActivityGroupId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<ParentPermission>(c => c.ParticipatingActivityGroupId == participatingActivityGroupId);
            spec.AddInclude(c => c.Include(c => c.Learner));

            var result = await schoolsModuleRepoManager.ParentPermissions.ListAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<IEnumerable<ParentPermissionDto>>.FailAsync(result.Messages);

            return await Result<IEnumerable<ParentPermissionDto>>.SuccessAsync(result.Data.Select(c => new ParentPermissionDto(c)));
        }

        /// <summary>
        /// Grants consent for a given event and learner from a specified parent.
        /// </summary>
        public async Task<IBaseResult<string>> GiveConsent(TeamMemberPermissionsParams parameters, CancellationToken cancellationToken = default)
        {
            // Check if a consent already exists for the parent/learner/event combination
            var existingResult = await schoolsModuleRepoManager.ParentPermissions.FirstOrDefaultAsync(
                new LambdaSpec<ParentPermission>(c =>
                    c.EventId == parameters.EventId &&
                    c.ParentId == parameters.ParentId &&
                    c.LearnerId == parameters.LearnerId &&
                    c.ConsentType == parameters.ConsentType &&
                    c.ParticipatingActivityGroupId == parameters.ParticipatingActivityGroupId),
                true,
                cancellationToken);

            if (!existingResult.Succeeded)
                return await Result<string>.FailAsync(existingResult.Messages);

            if (existingResult.Data is null)
            {
                // No existing record found, create a new one
                var parentConsent = new ParentPermission(parameters);
                var createResult = await schoolsModuleRepoManager.ParentPermissions.CreateAsync(parentConsent, cancellationToken);
                if (!createResult.Succeeded) return await Result<string>.FailAsync(createResult.Messages);
            }
            else
            {
                // Update the existing consent
                var consent = existingResult.Data;
                consent.Granted = parameters.Consent;
                consent.ConsentDirection = parameters.ConsentDirection;
                schoolsModuleRepoManager.ParentPermissions.Update(consent);
            }

            var saveResult = await schoolsModuleRepoManager.ParentPermissions.SaveAsync(cancellationToken);
            return saveResult.Succeeded
                ? await Result<string>.SuccessAsync("Consent granted")
                : await Result<string>.FailAsync(saveResult.Messages);
        }

        /// <summary>
        /// Retracts previously granted consent for a specific event, learner, and parent.
        /// </summary>
        public async Task<IBaseResult> RetractConsent(TeamMemberPermissionsParams parameters, CancellationToken cancellationToken = default)
        {
            var consentResult = await schoolsModuleRepoManager.ParentPermissions.FirstOrDefaultAsync(
                new LambdaSpec<ParentPermission>(c =>
                    c.EventId == parameters.EventId &&
                    c.ParentId == parameters.ParentId &&
                    c.ConsentType == parameters.ConsentType &&
                    c.LearnerId == parameters.LearnerId && 
                    c.ParticipatingActivityGroupId == parameters.ParticipatingActivityGroupId),
                false,
                cancellationToken);

            if (!consentResult.Succeeded) return await Result.FailAsync(consentResult.Messages);
            if (consentResult.Data == null) return await Result.FailAsync("No matching consent record found.");

            await schoolsModuleRepoManager.ParentPermissions.DeleteAsync(consentResult.Data.Id, cancellationToken);
            var saveResult = await schoolsModuleRepoManager.ParentPermissions.SaveAsync(cancellationToken);

            return saveResult.Succeeded
                ? await Result.SuccessAsync("Consent was successfully retracted")
                : await Result.FailAsync(saveResult.Messages);
        }

        /// <summary>
        /// Retrieves all consent statuses for a parent and event.
        /// </summary>
        public async Task<IBaseResult<List<SchoolEventPermissionsDto>>> SchoolEventPermissionsListAsync(SchoolEventPermissionsRequestArgs args, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(args.ParentEmail) || string.IsNullOrWhiteSpace(args.EventId))
                return await Result<List<SchoolEventPermissionsDto>>.FailAsync("ParentEmail and EventId are required.");

            var parentResult = await schoolsModuleRepoManager.Parents.ListAsync(new EventPermissionParentListSpecification(args), false, cancellationToken);
            var parent = parentResult.Data.FirstOrDefault();
            if (parent == null) return await Result<List<SchoolEventPermissionsDto>>.SuccessAsync([]);

            var teamSpec = new LambdaSpec<ParticipatingActivityGroup>(pag => pag.EventId == args.EventId);
            teamSpec.AddInclude(q => q.Include(p => p.ActivityGroup));
            teamSpec.AddInclude(q => q.Include(p => p.ParticipatingTeamMembers).ThenInclude(ptm => ptm.TeamMember));

            var teamResult = await schoolsModuleRepoManager.ParticipatingActivityGroups.ListAsync(teamSpec, false, cancellationToken);
            if (!teamResult.Succeeded) return await Result<List<SchoolEventPermissionsDto>>.FailAsync(teamResult.Messages);

            var resultList = new List<SchoolEventPermissionsDto>();
            foreach (var team in teamResult.Data)
            {
                foreach (var link in parent.Learners)
                {
                    if (team.ParticipatingTeamMembers.All(c => c.TeamMemberId != link.LearnerId)) continue;

                    var permissionItem = new SchoolEventPermissionsDto() { ParticipatingActivityGroupId = team.Id, Learner = new LearnerDto(link.Learner!, true), ActivityGroup = new ActivityGroupDto(team.ActivityGroup) };

                    var consents = parent.EventConsents.Where(c => c.EventId == args.EventId && c.LearnerId == link.LearnerId && c.ParticipatingActivityGroupId == team.Id).ToList();

                    permissionItem.AttendanceConsentGiven = consents.FirstOrDefault(c => c.ConsentType == ConsentTypes.Attendance)?.Granted;
                    if (permissionItem.AttendanceConsent) // If required, check transport too
                    {
                        var c = consents.FirstOrDefault(c => c.ConsentType == ConsentTypes.Transport);
                        permissionItem.TransportConsentGiven = c?.Granted;
                        permissionItem.ConsentDirection = c?.ConsentDirection;
                    }

                    resultList.Add(permissionItem);
                }
            }
            return await Result<List<SchoolEventPermissionsDto>>.SuccessAsync(resultList);
        }

        /// <summary>
        /// Retrieves consent status for a specific learner and event.
        /// </summary>
        public async Task<IBaseResult<SchoolEventPermissionsDto>> SchoolEventPermissionsAsync(SchoolEventPermissionsRequestArgs args, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(args.LearnerId) || string.IsNullOrWhiteSpace(args.EventId))
                return await Result<SchoolEventPermissionsDto>.FailAsync("LearnerId and EventId are required.");

            var learnerSpec = new LambdaSpec<Learner>(l => l.Id == args.LearnerId);
            var learnerResult = await schoolsModuleRepoManager.Learners.ListAsync(learnerSpec, true, cancellationToken);
            var learner = learnerResult.Data.FirstOrDefault();
            if (learner == null) return await Result<SchoolEventPermissionsDto>.FailAsync("Learner not found.");

            var dto = new SchoolEventPermissionsDto { Learner = new LearnerDto(learner, true) };

            var parentSpec = new LambdaSpec<LearnerParent>(lp => lp.LearnerId == args.LearnerId && lp.ParentConsentRequired);
            parentSpec.AddInclude(q => q.Include(lp => lp.Parent).ThenInclude(p => p.EventConsents));
            parentSpec.AddInclude(q => q.Include(lp => lp.Parent).ThenInclude(p => p.EmailAddresses));

            var parentResult = await schoolsModuleRepoManager.LearnerParents.ListAsync(parentSpec, false, cancellationToken);
            if (!parentResult.Succeeded) return await Result<SchoolEventPermissionsDto>.FailAsync(parentResult.Messages);

            foreach (var relation in parentResult.Data)
            {
                var parent = relation.Parent;

                var attendance = parent.EventConsents.FirstOrDefault(ec =>
                    ec.EventId == args.EventId &&
                    ec.ConsentType == ConsentTypes.Attendance &&
                    ec.LearnerId == args.LearnerId && ec.ParticipatingActivityGroupId == args.ActivityGroupId);

                if (attendance != null)
                    dto.AttendanceConsentGiven = attendance.Granted;

                var transport = parent.EventConsents.FirstOrDefault(ec =>
                    ec.EventId == args.EventId &&
                    ec.ConsentType == ConsentTypes.Transport &&
                    ec.LearnerId == args.LearnerId && ec.ParticipatingActivityGroupId == args.ActivityGroupId);

                if (transport != null)
                    dto.TransportConsentGiven = transport.Granted;
            }

            return await Result<SchoolEventPermissionsDto>.SuccessAsync(dto);
        }
    }
}