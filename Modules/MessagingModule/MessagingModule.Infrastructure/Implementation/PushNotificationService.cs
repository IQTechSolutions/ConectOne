using ConectOne.Domain.Extensions;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using FirebaseAdmin.Messaging;
using Hangfire;
using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Entities;
using IdentityModule.Domain.Interfaces;
using IdentityModule.Domain.RequestFeatures;
using MessagingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using NeuralTech.Base.Enums;
using Message = FirebaseAdmin.Messaging.Message;
using Notification = MessagingModule.Domain.Entities.Notification;

namespace MessagingModule.Infrastructure.Implementation
{
    /// <summary>
    /// A service responsible for sending push notifications to a list of users via Firebase Cloud Messaging (FCM).
    /// This includes:
    /// <list type="bullet">
    ///     <item><description>Fetching and batching device tokens per user.</description></item>
    ///     <item><description>Constructing <see cref="Domain.Entities.Message"/> objects appropriate for FCM.</description></item>
    ///     <item><description>Splitting large sends into sub-batches of at most 500 messages each.</description></item>
    ///     <item><description>Storing sent notifications into a repository for record-keeping.</description></item>
    /// </list>
    /// </summary>
    public class PushNotificationService(IRepository<DeviceToken, string> deviceTokenRepository, IRepository<Notification, string> notificationRepository,IUserService userRepository,
        IBackgroundJobClient jobs, ILogger<PushNotificationService> logger) : IPushNotificationService
    {
        private const int MaxBatchSize = 500;

        private static NotificationDto EnsureNotificationUrl(NotificationDto notification)
        {
            if (!string.IsNullOrWhiteSpace(notification.NotificationUrl))
                return notification;

            var entityId = notification.MessageId ?? notification.EntityId;
            if (string.IsNullOrWhiteSpace(entityId))
                return notification;

            var navigationUrl = $"/messages/bytype/{(int)notification.MessageType}/{entityId}";
            return notification with { NotificationUrl = navigationUrl };
        }

        /// <summary>
        /// Sends push notifications to a list of users. If more than 500 messages are required,
        /// the send operation is broken into multiple sub-batches to respect Firebase's limit.
        /// </summary>
        /// <param name="notificationList">
        /// A collection of <see cref="UserInfoDto"/> objects representing the recipients.
        /// Each recipient may have multiple device tokens.
        /// </param>
        /// <param name="pushNotification">
        /// The <see cref="NotificationDto"/> containing the title, body, and other
        /// metadata to send. Also includes a short description and an optional URL to navigate to.
        /// </param>
        /// <returns>
        /// An <see cref="IBaseResult"/> indicating success or failure, along with
        /// potential errors from FCM or the database.
        /// </returns>
        public async Task<IBaseResult> SendNotifications(IEnumerable<RecipientDto> notificationList, NotificationDto pushNotification)
        {
            var errorList = new List<string>();
            var notificationWithNavigation = EnsureNotificationUrl(pushNotification);

            try
            {
                // 1) Build the list of FCM messages based on users and tokens.
                var messageListResult = await CreateMessageList(notificationList, notificationWithNavigation);

                // If message creation encountered an error, return early.
                if (!messageListResult.Succeeded)
                    errorList.AddRange(messageListResult.Messages);
                else
                {
                    var messageList = messageListResult.Data.messageList;
                    if (messageList.Any())
                    {
                        if (messageList.Count <= 500)
                        {
                            var instance = FirebaseMessaging.DefaultInstance;
                            var response = await instance.SendEachAsync(messageList);
                            BatchResponse aaa = response;
                            if (aaa.SuccessCount == 0)
                                errorList.AddRange(aaa.Responses.Select(c => c.Exception.Message));
                        }
                        else
                        {
                            var sentCount = 0;
                            // If more than 500 messages, break into sub-batches of size 500.
                            var count = Math.Ceiling((double)messageList.Count() / 500);
                            // total sub-batches already processed
                            var skipCount = 0;

                            for (int i = 0; i < count; i++)
                            {
                                // Take the next 500 messages (or fewer if near the end).
                                var batchList = messageList.Skip(skipCount).Take(500);

                                // Send the current batch to Firebase.
                                var batchResponse = await FirebaseMessaging.DefaultInstance.SendEachAsync(batchList);
                                sentCount += batchResponse.SuccessCount;
                                skipCount += 500;
                            }

                            if (sentCount == 0) errorList.Add("No Notifications was sent");
                        }
                    }
                }

                if (errorList.Any())
                    return await Result.FailAsync(errorList);
                return await Result.SuccessAsync("Batch Notifications successfully sent");
            }
            catch (Exception e)
            {
                // Log or handle the exception. Return a fail result with the error message.
                return await Result.FailAsync(e.Message);
            }
        }

        /// <summary>
        /// Enqueues a batch of notifications for background processing.
        /// </summary>
        /// <param name="recipients">this is the list of the users that needs to receive a notification</param>
        /// <param name="pushNotification">this is the notification that needs to be sent</param>
        /// <returns></returns>
        public async Task<IBaseResult> EnqueueNotificationsAsync(IEnumerable<RecipientDto> recipients, NotificationDto pushNotification)
        {
            const int shardSize = 5_000;
            var notificationWithNavigation = EnsureNotificationUrl(pushNotification);

            foreach (var shard in recipients.Chunk(shardSize))
            {
                // Take a hard copy so the IEnumerable isn't lazily re-evaluated
                var shardList = shard.ToList();

                // Never capture the variable in a closure – pass it as a method arg
                jobs.Enqueue<PushNotificationService>(svc =>
                    svc.ProcessNotificationsAsync(shardList, notificationWithNavigation));
            }

            return await Result.SuccessAsync("Notifications queued for background delivery");
        }

        /// <summary>
        /// Processes a list of notifications by sending push notifications to recipients via Firebase Cloud Messaging
        /// (FCM)  and web push services.
        /// </summary>
        /// <remarks>This method processes notifications by creating message payloads for the specified
        /// recipients and sending them  through the appropriate channels (e.g., FCM or web push). If errors occur
        /// during the process, they are logged  for further investigation. The method is designed to be retried
        /// automatically in case of transient failures.</remarks>
        /// <param name="notificationList">A list of recipients to whom the notifications will be sent. Cannot be null or empty.</param>
        /// <param name="pushNotification">The notification details, including title, body, and other metadata. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [Queue("pushnotifications"), AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 30, 120, 300 })]
        public async Task ProcessNotificationsAsync(List<RecipientDto> notificationList, NotificationDto pushNotification)
        {
            var errorList = new List<string>();

            try
            {
                var result = await CreateMessageList(notificationList, pushNotification);

                if (!result.Succeeded)
                {
                    errorList.AddRange(result.Messages);
                }
                else
                {
                    // -------- FCM ----------
                    if (result.Data.messageList.Any())
                        await SendFirebaseMessages(result.Data.messageList, errorList);
                        
                }

                if (errorList.Any())
                    logger.LogWarning("FCM/WebPush errors: {Errors}", string.Join(" | ", errorList));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled exception in ProcessNotificationsAsync");
                throw;                       // let Hangfire capture & retry
            }
        }

        #region Helpers

        /// <summary>
        /// Sends Firebase Cloud Messages in batches respecting FCM's limit.
        /// </summary>
        /// <param name="messageList">The list of messages to send.</param>
        /// <param name="errorList">A list to collect any error messages encountered during sending.</param>
        private async Task SendFirebaseMessages(List<Message> messageList, List<string> errorList)
        {
            var firebase = FirebaseMessaging.DefaultInstance;

            if (messageList.Count <= MaxBatchSize)
            {
                var response = await firebase.SendEachAsync(messageList);
                if (response.SuccessCount == 0)
                    errorList.AddRange(response.Responses.Select(r => r.Exception?.Message ?? "Unknown error"));
            }
            else
            {
                int skip = 0;
                while (skip < messageList.Count)
                {
                    var batch = messageList.Skip(skip).Take(MaxBatchSize).ToList();
                    var response = await firebase.SendEachAsync(batch);
                    if (response.SuccessCount == 0)
                        errorList.AddRange(response.Responses.Select(r => r.Exception?.Message ?? "Unknown error"));
                    skip += MaxBatchSize;
                }
            }
        }

        /// <summary>
        /// Resolves valid notification recipients by matching their email addresses to known users in the database.
        /// Filters out recipients with no matching user or duplicates.
        /// </summary>
        /// <param name="recipients">
        /// A list of <see cref="RecipientDto"/> instances, each potentially containing multiple email addresses.
        /// </param>
        /// <returns>
        /// A list of <see cref="NotificationRecipient"/> objects, each including user info and optional message type, for users found in the system.
        /// </returns>
        /// <remarks>
        /// - This method performs a lookup for each email address against the <see cref="ApplicationUser"/> set.
        /// - The first matched user is used; duplicates are avoided based on <c>UserId</c>.
        /// </remarks>
        private async Task<List<NotificationRecipient>> ResolveValidUsers(IEnumerable<RecipientDto> recipients)
        {
            var usersDbSet = await userRepository.AllUsers(new UserPageParameters());
            var validRecipients = new List<NotificationRecipient>();

            foreach (var r in recipients)
            {
                if (r.EmailAddresses is null || !r.EmailAddresses.Any()) continue;

                UserInfoDto? matchedUser = null;
                foreach (var email in r.EmailAddresses)
                {
                    matchedUser = usersDbSet.Data.FirstOrDefault(u => u.EmailAddress == email);
                    if (matchedUser is not null) break;
                }

                if (matchedUser is not null && validRecipients.All(x => x.User.UserId != matchedUser.UserId))
                {
                    validRecipients.Add(new NotificationRecipient
                    {
                        MessageType = r.MessageType,
                        User = matchedUser
                    });
                }
            }

            return validRecipients;
        }

        /// <summary>
        /// Constructs a new <see cref="Notification"/> domain entity for persistence,
        /// based on the user and notification DTO provided.
        /// </summary>
        /// <param name="user">The target user who will receive the notification.</param>
        /// <param name="dto">The source <see cref="NotificationDto"/> containing message data.</param>
        /// <param name="type">Optional message type override; if <c>null</c>, retains the original type.</param>
        /// <returns>
        /// A populated <see cref="Notification"/> entity with audit and user fields filled in.
        /// </returns>
        /// <remarks>
        /// - Assigns a new unique ID and maps user-related metadata such as name and profile image.
        /// - The method ensures traceability by storing <c>ReceiverId</c> and notification context.
        /// </remarks>
        private Notification BuildNotificationEntity(UserInfoDto user, NotificationDto dto, MessageType? type)
        {
            var entity = dto.ToNotification();
            entity.Id = Guid.NewGuid().ToString();
            entity.ReceiverId = user.UserId!;
            entity.ReceiverName = $"{user.FirstName} {user.LastName}";
            entity.ReceiverImageUrl = user.CoverImageUrl;
            entity.MessageType = type ?? entity.MessageType;
            return entity;
        }

        /// <summary>
        /// Constructs Firebase Cloud Messaging (FCM) messages from a set of device tokens and shared notification content.
        /// </summary>
        /// <param name="tokens">The device tokens associated with a user.</param>
        /// <param name="dto">The <see cref="NotificationDto"/> containing the title, message, and other content fields.</param>
        /// <param name="navigationUrl">The navigation URL to attach to each message's data payload.</param>
        /// <returns>
        /// A list of <see cref="Message"/> objects ready for dispatch via Firebase.
        /// </returns>
        /// <remarks>
        /// - Each message includes a unique Android collapse key to prevent overwriting.
        /// - The message body truncates long content for mobile readability.
        /// </remarks>
        private List<Message> BuildFirebaseMessages(IEnumerable<DeviceToken> tokens, NotificationDto dto, string navigationUrl)
        {
            return tokens.Select(token => new Message
            {
                Notification = new FirebaseAdmin.Messaging.Notification
                {
                    Title = dto.Title,
                    Body = !string.IsNullOrEmpty(dto.Message)
                        ? dto.ShortDescription.TruncateLongString(55)
                        : dto.Message.HtmlToPlainText().TruncateLongString(55)
                },
                Data = new Dictionary<string, string>
                {
                    { "NavigationID", navigationUrl }
                },
                Token = token.DeviceTokenContent,
                Android = new AndroidConfig
                {
                    CollapseKey = Guid.NewGuid().ToString()
                }
            }).ToList();
        }

        #endregion

        /// <summary>
        /// Creates a list of Firebase messages from the input user list and the provided
        /// <see cref="NotificationDto"/>. It also records each notification in the
        /// <paramref name="notificationRepository"/> if the user has tokens.
        /// </summary>
        /// <param name="notificationList">
        /// The set of users to notify.
        /// </param>
        /// <param name="pushNotification">
        /// The shared push notification data (title, body, URL, etc.).
        /// </param>
        /// <returns>
        /// An <see cref="IBaseResult{T}"/> with a list of <see cref="Message"/> objects
        /// ready to send to Firebase, or errors if an issue occurs storing notifications or
        /// mapping them to messages.
        /// </returns>
        private async Task<IBaseResult<(List<Message> messageList, List<NotificationSubscriptionMessage> webMessages)>> CreateMessageList(IEnumerable<RecipientDto> notificationList, NotificationDto pushNotification)
        {
            var messageList = new List<Message>();
            var webMessageList = new List<NotificationSubscriptionMessage>();
            var validRecipients = await ResolveValidUsers(notificationList);

            foreach (var recipient in validRecipients)
            {
                var user = recipient.User;
                if (!user.ReceiveNotifications) continue;

                var notification = BuildNotificationEntity(user, pushNotification, pushNotification.MessageType);

                var tokensResult = deviceTokenRepository.FindByCondition(c => c.UserId == user.UserId);
                if (tokensResult.Succeeded && tokensResult.Data.Any())
                {
                    var tokens = tokensResult.Data.ToList();
                    var tokenStrings = tokens.Select(t => t.DeviceTokenContent).ToList();

                    // Create FCM messages and track notified tokens
                    var firebaseMessages = BuildFirebaseMessages(tokens, pushNotification, notification.NotificationUrl);
                    messageList.AddRange(firebaseMessages);
                    notification.DeviceTokensNotified = string.Join(",", tokenStrings);
                }

                var createResult = await notificationRepository.CreateAsync(notification);
                if (!createResult.Succeeded)
                    return await Result<(List<Message>, List<NotificationSubscriptionMessage>)>.FailAsync(createResult.Messages);

                var saveResult = await notificationRepository.SaveAsync();
                if (!saveResult.Succeeded)
                    return await Result<(List<Message>, List<NotificationSubscriptionMessage>)>.FailAsync(saveResult.Messages);
            }

            return await Result<(List<Message>, List<NotificationSubscriptionMessage>)>.SuccessAsync((messageList, webMessageList));
        }
    }

    /// <summary>
    /// Represents a recipient of a notification, including the message type and user information.
    /// </summary>
    public class NotificationRecipient
    {
        /// <summary>
        /// Gets or sets the type of message to be sent to the recipient.
        /// </summary>
        public MessageType? MessageType { get; set; }

        /// <summary>
        /// Gets or sets the user information for the recipient.
        /// </summary>
        public UserInfoDto User { get; set; }
    }

    /// <summary>
    /// Represents a message to be sent to a web push subscription.
    /// </summary>
    public class NotificationSubscriptionMessage
    {
        /// <summary>
        /// Gets or sets the title of the notification.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the body of the notification.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the URL to navigate to when the notification is clicked.
        /// </summary>
        public string Url { get; set; }
    }

}