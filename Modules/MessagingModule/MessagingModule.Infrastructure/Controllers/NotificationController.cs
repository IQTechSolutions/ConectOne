using MessagingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.Interfaces;
using MessagingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessagingModule.Infrastructure.Controllers
{
    /// <summary>
    /// Provides API endpoints for managing push notification subscriptions for authenticated users.
    /// </summary>
    /// <remarks>Requires authentication using the Bearer scheme. All endpoints are accessible under the
    /// 'api/notifications' route.</remarks>
    /// <param name="pushNotificationService">The service used to handle push notification subscription operations for users. Cannot be null.</param>
    [Route("api/notifications"), ApiController, Authorize(AuthenticationSchemes = "Bearer")]
    public class NotificationController(IPushNotificationService pushNotificationService, INotificationService notificationService) : ControllerBase
    {
        /// <summary>
        /// Registers a new web push notification subscription for the current user.
        /// </summary>
        /// <param name="dto">The subscription details to register, including endpoint and keys. Cannot be null.</param>
        /// <returns>An <see cref="IActionResult"/> containing the updated user information after the subscription is added.</returns>
        [HttpPost("webPush/subscribe")]
        public async Task<IActionResult> CreateNotificationSubscription([FromBody] NotificationSubscriptionDto dto)
        {
            var user = await pushNotificationService.AddNotificationSubscription(dto);
            return Ok(user);
        }

        /// <summary>
        /// Retrieves a paginated list of notifications based on the specified parameters.
        /// </summary>
        /// <remarks>This endpoint supports pagination to efficiently retrieve large sets of
        /// notifications. Ensure that the <paramref name="args"/> parameter is properly populated to avoid invalid
        /// requests.</remarks>
        /// <param name="args">The parameters used to define the pagination and filtering of notifications. This includes page size, page
        /// number, and any additional filters.</param>
        /// <returns>An <see cref="IActionResult"/> containing the paginated list of notifications. The result is returned as an
        /// HTTP 200 response with the notifications in the response body.</returns>
        [HttpGet("paged")]
        public async Task<IActionResult> PagedNotifications([FromQuery] NotificationPageParameters args)
        {
            var notifications = await notificationService.PagedNotificationsAsync(args);
            return Ok(notifications);
        }

        /// <summary>
        /// Retrieves a paginated list of unread notifications for the current user.
        /// </summary>
        /// <remarks>The response includes only notifications that are marked as unread. Use the <paramref
        /// name="args"/> parameter to specify pagination details such as page number and page size.</remarks>
        /// <param name="args">The parameters for pagination and filtering of unread notifications.</param>
        /// <returns>An <see cref="IActionResult"/> containing the paginated list of unread notifications.</returns>
        [HttpGet("unread")]
        public async Task<IActionResult> GetUnreadNotifications([FromQuery] NotificationPageParameters args)
        {
            var notifications = await notificationService.UnreadNotificationsAsync(args);
            return Ok(notifications);
        }

        /// <summary>
        /// Retrieves the count of unread notifications based on the specified query parameters.
        /// </summary>
        /// <remarks>The method processes the provided query parameters to determine the count of unread
        /// notifications and returns the result in an HTTP 200 OK response. Ensure that the <paramref name="args"/>
        /// parameter is properly populated to filter the notifications as needed.</remarks>
        /// <param name="args">The query parameters used to filter and paginate the unread notifications.</param>
        /// <returns>An <see cref="IActionResult"/> containing the count of unread notifications as an integer.</returns>
        [HttpGet("unread/count")]
        public async Task<IActionResult> GetUnreadNotificationCount([FromQuery] NotificationPageParameters args)
        {
            var notificationCount = await notificationService.UnreadNotificationCountAsync(args);
            return Ok(notificationCount);
        }

        /// <summary>
        /// Creates a new notification based on the provided data.
        /// </summary>
        /// <remarks>This method processes the provided notification data and adds it to the system
        /// asynchronously.  Ensure that the <paramref name="notification"/> object contains valid and complete data
        /// before calling this method.</remarks>
        /// <param name="notification">The notification data to be created. This must include all required fields for a valid notification.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.  Returns an HTTP 200 OK response with
        /// the created notification details if successful.</returns>
        [HttpPut]
        public async Task<IActionResult> CreateNewNotification([FromBody] NotificationDto notification)
        {
            var response = await notificationService.AddNotificationAsync(notification);
            return Ok(response);
        }

        /// <summary>
        /// Deletes a notification with the specified identifier.
        /// </summary>
        /// <remarks>This method invokes the underlying service to remove the notification and returns the
        /// result as an HTTP response. Ensure that the <paramref name="notificationId"/> corresponds to an existing
        /// notification.</remarks>
        /// <param name="notificationId">The unique identifier of the notification to be removed. Cannot be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Typically returns an HTTP 200
        /// response with the operation result.</returns>
        [HttpDelete("{notificationId}")]
        public async Task<IActionResult> RemoveNotification(string notificationId)
        {
            var response = await notificationService.RemoveNotificationAsync(notificationId);
            return Ok(response);
        }

        /// <summary>
        /// Retrieves a notification by its unique identifier.
        /// </summary>
        /// <remarks>This method returns an HTTP 200 response with the notification data if the
        /// notification exists. If the notification is not found, an appropriate HTTP error response is
        /// returned.</remarks>
        /// <param name="notificationId">The unique identifier of the notification to retrieve. Cannot be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> containing the notification data if found, or an appropriate HTTP response if
        /// not.</returns>
        [HttpGet("{notificationId}")]
        public async Task<IActionResult> GetNotification(string notificationId)
        {
            var notification = await notificationService.NotificationAsync(notificationId);
            return Ok(notification);
        }

        /// <summary>
        /// Marks the specified notification as read.
        /// </summary>
        /// <remarks>This action is invoked via an HTTP GET request to the endpoint specified by the route
        /// template.</remarks>
        /// <param name="notificationId">The unique identifier of the notification to mark as read. Cannot be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation. Typically, this will include a
        /// success message if the operation completes successfully.</returns>
		[HttpGet("{notificationId}/markasread")]
        public async Task<IActionResult> MarkAsReadNotification(string notificationId)
        {
            var message = await notificationService.MarkAsReadAsync(notificationId);
            return Ok(message);
        }
    }
}
