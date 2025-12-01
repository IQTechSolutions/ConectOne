using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using IdentityModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.ActivityGroups;

namespace SchoolsModule.Application.RestServices.ActivityGroups
{
    /// <summary>
    /// Provides methods for retrieving notification-related data for activity groups and categories.
    /// </summary>
    /// <remarks>This service interacts with a REST API to fetch notification recipients for specific activity
    /// groups, activity group categories, or participating activity groups. It is designed to be used in scenarios
    /// where notification data is required for managing or displaying activity-related information.</remarks>
    /// <param name="provider"></param>
    public class ActivityGroupNotificationRestService(IBaseHttpProvider provider) : IActivityGroupNotificationService
    {
        /// <summary>
        /// Retrieves a list of recipients associated with the specified activity group.
        /// </summary>
        /// <remarks>This method communicates with an external provider to fetch the notification list for
        /// the specified activity group. Ensure that the <paramref name="activityGroupId"/> is valid and corresponds to
        /// an existing activity group.</remarks>
        /// <param name="activityGroupId">The unique identifier of the activity group for which to retrieve the notification list.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// containing an enumerable collection of <see cref="RecipientDto"/> objects representing the recipients.</returns>
        public async Task<IBaseResult<IEnumerable<RecipientDto>>> ActivityGroupNotificationList(string activityGroupId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<RecipientDto>>($"activitygroups/notificationList/{activityGroupId}");
            return result;
        }
        
        /// <summary>
        /// Retrieves a list of recipients associated with the specified activity group category.
        /// </summary>
        /// <remarks>This method sends a request to retrieve the notification list for the specified
        /// activity group category. Ensure that the <paramref name="activityGroupCategoryId"/> is valid and not empty
        /// before calling this method.</remarks>
        /// <param name="activityGroupCategoryId">The unique identifier of the activity group category for which to retrieve the notification list.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// of <see cref="IEnumerable{T}"/> containing <see cref="RecipientDto"/> objects representing the recipients.</returns>
        public async Task<IBaseResult<IEnumerable<RecipientDto>>> ActivityGroupCategoryNotificationList(string activityGroupCategoryId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<RecipientDto>>($"activities/categories/notificationList/{activityGroupCategoryId}");
            return result;
        }

        /// <summary>
        /// Retrieves a list of recipients participating in the notification list for the specified activity group.
        /// </summary>
        /// <remarks>This method asynchronously retrieves the notification list for the specified activity
        /// group. The caller can use the <paramref name="cancellationToken"/> to cancel the operation if
        /// needed.</remarks>
        /// <param name="activityGroupId">The unique identifier of the activity group for which to retrieve the notification list.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// of <see cref="IEnumerable{T}"/> containing <see cref="RecipientDto"/> objects representing the recipients.</returns>
        public async Task<IBaseResult<IEnumerable<RecipientDto>>> ParticipatingActivityGroupNotificationList(string activityGroupId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<RecipientDto>>($"activitygroups/participating/notificationList/{activityGroupId}");
            return result;
        }
    }
}
