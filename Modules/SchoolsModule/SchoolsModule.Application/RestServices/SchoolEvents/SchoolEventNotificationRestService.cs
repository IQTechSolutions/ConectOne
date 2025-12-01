using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using IdentityModule.Domain.DataTransferObjects;
using MessagingModule.Domain.Enums;
using SchoolsModule.Domain.Interfaces.SchoolEvents;

namespace SchoolsModule.Application.RestServices.SchoolEvents
{
    /// <summary>
    /// Provides REST-based services for managing school event notifications.
    /// </summary>
    /// <remarks>This service allows clients to retrieve notification recipient lists for specific school
    /// events, including those filtered by permissions or consent types. It interacts with a REST API to fetch the
    /// required data.</remarks>
    /// <param name="provider"></param>
    public class SchoolEventNotificationRestService(IBaseHttpProvider provider) : ISchoolEventNotificationService
    {
        /// <summary>
        /// Retrieves a list of recipients for a specified school event.
        /// </summary>
        /// <remarks>This method communicates with an external provider to fetch the notification list for
        /// the specified school event. Ensure that the <paramref name="schoolEventId"/> is valid and corresponds to an
        /// existing event.</remarks>
        /// <param name="schoolEventId">The unique identifier of the school event for which to retrieve the notification list.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// containing an enumerable collection of <see cref="RecipientDto"/> objects representing the recipients of the
        /// event notifications.</returns>
        public async Task<IBaseResult<IEnumerable<RecipientDto>>> EventNotificationList(string schoolEventId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<RecipientDto>>($"schoolevents/notificationList/{schoolEventId}");
            return result;
        }

        /// <summary>
        /// Retrieves a list of recipients for event permission notifications based on the specified consent type and
        /// event details.
        /// </summary>
        /// <remarks>This method queries the notification list for a specific school event and consent
        /// type. Optional parameters allow further filtering by activity group or learner.</remarks>
        /// <param name="consentType">The type of consent used to filter the recipients.</param>
        /// <param name="schoolEventId">The unique identifier of the school event for which the notification list is being retrieved.</param>
        /// <param name="activityGroupId">An optional identifier for the activity group associated with the event. If null, the notification list is
        /// not filtered by activity group.</param>
        /// <param name="learnerId">An optional identifier for the learner associated with the event. If null, the notification list is not
        /// filtered by learner.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// containing an enumerable collection of <see cref="RecipientDto"/> objects representing the recipients of the
        /// event permission notifications.</returns>
        public async Task<IBaseResult<IEnumerable<RecipientDto>>> EventPermissionNotificationList(ConsentTypes consentType, string schoolEventId, string? activityGroupId = null, string? learnerId = null, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<RecipientDto>>($"schoolevents/notificationList//permissions/{schoolEventId}/{consentType}");
            return result;
        }
    }
}
