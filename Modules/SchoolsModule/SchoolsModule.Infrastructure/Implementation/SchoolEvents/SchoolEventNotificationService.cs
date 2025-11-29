using ConectOne.Domain.ResultWrappers;
using IdentityModule.Domain.DataTransferObjects;
using MessagingModule.Domain.Enums;
using NeuralTech.Base.Enums;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.Interfaces.SchoolEvents;
using SchoolsModule.Domain.Specifications;

namespace SchoolsModule.Infrastructure.Implementation.SchoolEvents
{
    /// <summary>
    ///     Builds recipient lists for notifications related to <c>SchoolEvent</c> entities.
    ///     <para>
    ///         Two public entry points are exposed:
    ///         <list type="bullet">
    ///             <item>
    ///                 <see cref="EventNotificationList"/>—who should be told that an event exists?
    ///             </item>
    ///             <item>
    ///                 <see cref="EventPermissionNotificationList"/>—who still needs to supply a specific consent (e.g. attendance)?
    ///             </item>
    ///         </list>
    ///     </para>
    ///     The service is intentionally query‑only; it never updates the database.  
    ///     All persistence responsibilities remain inside the repository layer.
    /// </summary>
    public class SchoolEventNotificationService(ISchoolsModuleRepoManager schoolsModuleRepoManager) : ISchoolEventNotificationService
    {
        /// <summary>
        ///     Retrieves **all unique parents and teachers** that should receive a generic
        ///     “there’s an upcoming event” notification.
        /// </summary>
        /// <param name="schoolEventId">
        ///     The primary‑key of the event whose audience must be calculated.
        /// </param>
        /// <param name="cancellationToken">Standard token for cooperative cancellation.</param>
        /// <returns>
        ///     <see cref="SuccessResult{T}"/>—collection of <see cref="RecipientDto"/>  
        ///     when the operation succeeds; otherwise <see cref="FailResult{T}"/>.
        /// </returns>
        public async Task<IBaseResult<IEnumerable<RecipientDto>>> EventNotificationList(string schoolEventId, CancellationToken cancellationToken = default)
        {
            var result = await schoolsModuleRepoManager.SchoolEvents.FirstOrDefaultAsync(new SingleEventNotificationSpecification(schoolEventId), false, cancellationToken);
            if (!result.Succeeded) return await Result<IEnumerable<RecipientDto>>.FailAsync(result.Messages);

            if (result.Data == null) return await Result<IEnumerable<RecipientDto>>.FailAsync($"No event found with id matching {schoolEventId}, in the database");

            var currentUserNotificationList = new List<RecipientDto>();
            
            foreach (var team in result.Data.ParticipatingActivityGroups)
            {
                foreach (var teamMember in team.ParticipatingTeamMembers)
                {
                    foreach (var parent in teamMember.TeamMember.Parents)
                    {
                        if (currentUserNotificationList.All(c => c.Id != parent.ParentId))
                            currentUserNotificationList.Add(new RecipientDto(parent.ParentId!, parent.Parent!.FirstName, parent.Parent.LastName, parent.Parent.EmailAddresses.Select(c => c.Email).ToList(), parent.Parent.ReceiveNotifications, parent.Parent.RecieveEmails));
                    }
                }

                if (team.ActivityGroup?.Teacher is not null && currentUserNotificationList.All(c => c.Id != team.ActivityGroup?.Teacher?.Id))
                    currentUserNotificationList.Add(new RecipientDto(team.ActivityGroup.Teacher.Id, team.ActivityGroup.Teacher.Name, team.ActivityGroup.Teacher.Surname, team.ActivityGroup.Teacher.EmailAddresses.Select(c => c.Email).ToList(), true, false));
            }
            return await Result<IEnumerable<RecipientDto>>.SuccessAsync(currentUserNotificationList);
        }

        /// <summary>
        ///     Retrieves parents (and teachers for visibility) who have **not yet supplied** the requested
        ///     consent for an event—optionally scoped to a single team or learner.
        /// </summary>
        /// <param name="consentType">Type of consent still required (attendance, transport, etc.).</param>
        /// <param name="schoolEventId">Target event ID.</param>
        /// <param name="activityGroupId">
        ///     Optional team ID. When supplied, only learners in that team are evaluated.
        /// </param>
        /// <param name="learnerId">
        ///     Optional learner ID. When supplied, only that learner’s parents are evaluated.
        /// </param>
        /// <param name="cancellationToken">Standard token for cooperative cancellation.</param>
        /// <returns>
        ///     Parents and teachers that should be reminded to provide a missing consent.
        /// </returns>
        public async Task<IBaseResult<IEnumerable<RecipientDto>>> EventPermissionNotificationList(ConsentTypes consentType, string schoolEventId, string? activityGroupId = null, string? learnerId = null, CancellationToken cancellationToken = default)
        {
            var result = await schoolsModuleRepoManager.SchoolEvents.FirstOrDefaultAsync(new SingleEventNotificationSpecification(schoolEventId), false, cancellationToken);
            if (!result.Succeeded) return await Result<IEnumerable<RecipientDto>>.FailAsync(result.Messages);
            if(result.Data == null) return await Result<IEnumerable<RecipientDto>>.FailAsync($"No event found with id matching {schoolEventId}, in the database");

            var currentUserNotificationList = new List<RecipientDto>();

            var participatingTeams = string.IsNullOrEmpty(activityGroupId)
                ? result.Data.ParticipatingActivityGroups
                : result.Data.ParticipatingActivityGroups.Where(c => c.ActivityGroupId == activityGroupId);

            foreach (var team in participatingTeams)
            {
                var teamMembers = !string.IsNullOrEmpty(learnerId)
                    ? team.ParticipatingTeamMembers.Where(c => c.TeamMemberId == learnerId)
                    : team.ParticipatingTeamMembers;
                foreach (var teamMember in teamMembers)
                {
                    foreach (var parent in teamMember.TeamMember.Parents.Where(c => c.ParentConsentRequired))
                    {
                        if (consentType == ConsentTypes.Attendance && result.Data.AttendanceConsentRequired && !teamMember.TeamMember.EventConsents.Any(c => c.ConsentType == ConsentTypes.Attendance && c.EventId == schoolEventId))
                        {
                            if (currentUserNotificationList.All(c => c.Id != parent.ParentId))
                                currentUserNotificationList.Add(new RecipientDto(parent.ParentId!, parent.Parent!.FirstName, parent.Parent.LastName, parent.Parent.EmailAddresses.Select(c => c.Email).ToList(), parent.Parent.ReceiveNotifications, parent.Parent.RecieveEmails, null, MessageType.Parent));
                        }
                    }
                }

                if (team.ActivityGroup?.Teacher is not null && currentUserNotificationList.All(c => c.Id != team.ActivityGroup?.Teacher?.Id))
                    currentUserNotificationList.Add(new RecipientDto(team.ActivityGroup.Teacher.Id, team.ActivityGroup.Teacher.Name, team.ActivityGroup.Teacher.Surname, team.ActivityGroup.Teacher.EmailAddresses.Select(c => c.Email).ToList(), true, false, null, MessageType.None));
            }
            return await Result<IEnumerable<RecipientDto>>.SuccessAsync(currentUserNotificationList);
        }
    }
}
