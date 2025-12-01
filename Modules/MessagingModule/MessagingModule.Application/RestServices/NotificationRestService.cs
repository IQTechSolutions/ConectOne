using ConectOne.Domain.Extensions;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using MessagingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.Interfaces;
using MessagingModule.Domain.RequestFeatures;

namespace MessagingModule.Application.RestServices
{
    /// <summary>
    /// Provides REST-based operations for retrieving, managing, and updating user notifications.
    /// </summary>
    /// <remarks>This service implements notification-related functionality by communicating with a remote
    /// REST API. All methods are asynchronous and return results that reflect the outcome of the corresponding HTTP
    /// operations. Thread safety depends on the underlying HTTP provider implementation.</remarks>
    /// <param name="provider">The HTTP provider used to perform REST API requests for notification operations.</param>
    public sealed class NotificationRestService(IBaseHttpProvider provider) : INotificationService
    {
        /// <summary>
        /// Retrieves a paginated list of notifications based on the specified paging and filter parameters.
        /// </summary>
        /// <param name="args">The parameters that define paging, sorting, and filtering options for the notifications query. Cannot be
        /// null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a paginated result of
        /// notification data transfer objects that match the specified criteria.</returns>
        public async Task<PaginatedResult<NotificationDto>> PagedNotificationsAsync(NotificationPageParameters args)
        {
            var result = await provider.GetPagedAsync<NotificationDto, NotificationPageParameters>($"notifications/paged", args);
            return result;
        }

        /// <summary>
        /// Asynchronously retrieves a collection of unread notifications for the current user, using the specified
        /// paging and filtering parameters.
        /// </summary>
        /// <param name="args">The parameters that define paging and filtering options for the unread notifications query. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with a collection of <see cref="NotificationDto"/> objects representing the unread notifications. The
        /// collection will be empty if there are no unread notifications.</returns>
        public async Task<IBaseResult<IEnumerable<NotificationDto>>> UnreadNotificationsAsync(NotificationPageParameters args)
        {
            var result = await provider.GetAsync<IEnumerable<NotificationDto>>($"notifications/unread/count/{args.GetQueryString()}");
            return result;
        }

        /// <summary>
        /// Asynchronously retrieves the count of unread notifications based on the specified filter parameters.
        /// </summary>
        /// <param name="args">The parameters used to filter and paginate the notifications. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with the number of unread notifications matching the specified criteria.</returns>
        public async Task<IBaseResult<int>> UnreadNotificationCountAsync(NotificationPageParameters args)
        {
            var result = await provider.GetAsync<int>($"notifications/{args.GetQueryString()}");
            return result;
        }

        /// <summary>
        /// Retrieves the notification details for the specified notification identifier asynchronously.
        /// </summary>
        /// <param name="notificationId">The unique identifier of the notification to retrieve. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="IBaseResult{NotificationDto}"/> with the notification details if found; otherwise, the result may
        /// indicate an error or that the notification does not exist.</returns>
        public async Task<IBaseResult<NotificationDto>> NotificationAsync(string notificationId)
        {
            var result = await provider.GetAsync<NotificationDto>("");
            return result;
        }

        /// <summary>
        /// Marks the specified notification as read asynchronously.
        /// </summary>
        /// <param name="notificationId">The unique identifier of the notification to mark as read. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="IBaseResult{NotificationDto}"/> with the updated notification details.</returns>
        public async Task<IBaseResult<NotificationDto>> MarkAsReadAsync(string notificationId)
        {
            var result = await provider.GetAsync<NotificationDto>($"notifications/{notificationId}/markasread");
            return result;
        }

        /// <summary>
        /// Adds a new notification asynchronously.
        /// </summary>
        /// <param name="message">The notification to add. May be null if no notification data is provided.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="IBaseResult{NotificationDto}"/> representing the outcome of the add operation, including the added
        /// notification data if successful.</returns>
        public async Task<IBaseResult<NotificationDto>> AddNotificationAsync(NotificationDto? message)
        {
            var result = await provider.PutAsync<NotificationDto, NotificationDto>("notifications", message);
            return result;
        }

        /// <summary>
        /// Asynchronously removes a notification with the specified identifier.
        /// </summary>
        /// <param name="notificationId">The unique identifier of the notification to remove. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the removal operation.</returns>
        public async Task<IBaseResult> RemoveNotificationAsync(string notificationId)
        {
            var result = await provider.DeleteAsync("notifications", notificationId);
            return result;
        }
    }
}
