using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using IdentityModule.Domain.DataTransferObjects;
using MessagingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.Interfaces;
using MessagingModule.Domain.RequestFeatures;

namespace MessagingModule.Application.RestServices
{
    /// <summary>
    /// Provides functionality for sending and managing push notifications to recipients using Firebase Cloud Messaging
    /// (FCM) and web push protocols.
    /// </summary>
    /// <remarks>This service supports batch sending of notifications and background queuing for large
    /// recipient lists. It integrates with Firebase Cloud Messaging for device notifications and web push for
    /// browser-based notifications. The service enforces batch size limits according to FCM constraints and handles
    /// error aggregation for failed deliveries.</remarks>
    /// <param name="provider">The HTTP provider used to perform network operations required for sending notifications.</param>
    public class PushNotificationRestService(IBaseHttpProvider provider) : IPushNotificationService
    {
        /// <summary>
        /// Sends a push notification to a collection of recipients asynchronously.
        /// </summary>
        /// <param name="notificationList">A collection of recipients who will receive the push notification. Cannot be null or empty.</param>
        /// <param name="pushNotification">The notification details to send to the specified recipients. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the notification send operation.</returns>
        public async Task<IBaseResult> SendNotifications(IEnumerable<RecipientDto> notificationList, NotificationDto pushNotification)
        {
            var result = await provider.PostAsync("pushnotifications/v2", new CreatePushNotificationRequest()
            {
                Users = notificationList.ToList(),
                Notification = pushNotification
            });
            return result;
        }

        /// <summary>
        /// Asynchronously enqueues push notifications to be sent to the specified recipients.
        /// </summary>
        /// <param name="recipients">A collection of recipients who will receive the push notification. Cannot be null or empty.</param>
        /// <param name="pushNotification">The notification to be sent to each recipient. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the enqueue operation.</returns>
        /// <exception cref="NotImplementedException">The method is not implemented.</exception>
        public Task<IBaseResult> EnqueueNotificationsAsync(IEnumerable<RecipientDto> recipients, NotificationDto pushNotification)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Registers a new web push notification subscription using the specified subscription details.
        /// </summary>
        /// <param name="token">An object containing the subscription information required to enable web push notifications for a user.
        /// Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the subscription request.</returns>
        public async Task<IBaseResult> AddNotificationSubscription(NotificationSubscriptionDto token)
        {
            var result = await provider.PostAsync("notifications/webPush/subscribe", token);
            return result;
        }
    }
}
