using ConectOne.Domain.ResultWrappers;
using IdentityModule.Domain.DataTransferObjects;
using MessagingModule.Domain.DataTransferObjects;

namespace MessagingModule.Domain.Interfaces
{
    /// <summary>
    /// Defines an interface for push notification operations within the application.
    /// This service is responsible for constructing and sending push notifications 
    /// to a group of recipient users. Implementations typically involve:
    /// <list type="bullet">
    ///   <item><description>Gathering device tokens for each intended recipient.</description></item>
    ///   <item><description>Packaging the notification payload (title, body, data) in a format understood by a push service (e.g., Firebase Cloud Messaging).</description></item>
    ///   <item><description>Handling edge cases (e.g., no valid device tokens, large batches exceeding single request limits).</description></item>
    /// </list>
    /// </summary>
    public interface IPushNotificationService
    {
        /// <summary>
        /// Sends push notifications to a list of recipients.
        /// </summary>
        /// <remarks>This method sends the specified notification to all recipients in the provided list.
        /// Ensure that the notification list is not empty and that each recipient is valid.</remarks>
        /// <param name="notificationList">A collection of recipients to whom the notification will be sent. Each recipient must be represented as a
        /// <see cref="RecipientDto"/> object.</param>
        /// <param name="pushNotification">The notification details to be sent, represented as a <see cref="NotificationDto"/> object. This parameter
        /// cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// object indicating the success or failure of the operation.</returns>
        Task<IBaseResult> SendNotifications(IEnumerable<RecipientDto> notificationList, NotificationDto pushNotification);

        /// <summary>
        /// Asynchronously enqueues push notifications to be sent to the specified recipients.
        /// </summary>
        /// <param name="recipients">A collection of recipients who will receive the push notification. Cannot be null or empty.</param>
        /// <param name="pushNotification">The notification to be sent to each recipient. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the enqueue operation.</returns>
        Task<IBaseResult> EnqueueNotificationsAsync(IEnumerable<RecipientDto> recipients, NotificationDto pushNotification);

        /// <summary>
        /// Adds a new notification subscription asynchronously.
        /// </summary>
        /// <param name="token">An object containing the details of the notification subscription to add. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the subscription addition.</returns>
        Task<IBaseResult> AddNotificationSubscription(NotificationSubscriptionDto token);
    }
}