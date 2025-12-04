using ConectOne.Domain.Enums;
using ConectOne.Domain.Extensions;
using FilingModule.Domain.DataTransferObjects;
using IdentityModule.Domain.DataTransferObjects;
using MessagingModule.Domain.Entities;
using NeuralTech.Base.Enums;

namespace MessagingModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a Notification Data Transfer Object (DTO) that can be constructed
    /// from various sources (Notification entity, NotificationViewModel, MessageViewModel).
    /// This DTO provides properties such as Title, Message, Receiver, and Timestamps,
    /// along with methods to convert back to the underlying Notification entity.
    /// </summary>
    public record NotificationDto
    {
        #region Constructors

        /// <summary>
        /// Parameterless constructor to allow default instantiation
        /// and manual property assignment.
        /// </summary>
        public NotificationDto() { }

        /// <summary>
        /// Constructs a NotificationDto by projecting a <see cref="Notification"/> entity.
        /// </summary>
        /// <param name="notification">The source entity from the database.</param>
        public NotificationDto(Notification notification)
        {
            NotificationId = notification.Id;

            ReceiverId = notification.ReceiverId;
            ReceiverName = notification.ReceiverName;
            ReceiverImageUrl = notification.ReceiverImageUrl;

            Title = notification.Title;
            ShortDescription = notification.ShortDescription;
            Message = notification.Description;
            NotificationUrl = notification.NotificationUrl;

            EntityId = notification.EntityId;
            MessageId = notification.MessageId;
            MessageType = notification.MessageType;

            Created = notification.CreatedOn;
            ReadTime = notification.OpenedDate;
            SendTime = notification.SentDate;

            DeviceTokensNotified = !string.IsNullOrEmpty(notification.DeviceTokensNotified) ? notification.DeviceTokensNotified.Split(",").ToList() : [];
            NotificationSubscriptionsNotified = !string.IsNullOrEmpty(notification.NotificationSubscriptionsNotified) ? notification.NotificationSubscriptionsNotified.Split(",").ToList() : [];
        }

        /// <summary>
        /// Reinitializes a NotificationDto while assigning a new <paramref name="notificationId"/> 
        /// and optionally truncating the <see cref="ShortDescription"/>.
        /// </summary>
        /// <param name="notificationId">A custom notification ID to use.</param>
        /// <param name="notification">The original NotificationDto to copy from.</param>
        public NotificationDto(string notificationId, NotificationDto notification)
        {
            NotificationId = notificationId;
            EntityId = notification.EntityId;
            MessageType = notification.MessageType;
            Title = notification.Title;
            NotificationUrl = notification.NotificationUrl;
            ShortDescription = notification.ShortDescription.TruncateLongString(55);
            Message = notification.Message;
            Created = DateTime.Now;
        }

        /// <summary>
        /// Constructs a NotificationDto for a user-based scenario
        /// (e.g., a learner notification), assigning basic user info.
        /// </summary>
        /// <param name="notificationId">A new unique ID for this notification.</param>
        /// <param name="user">A user object (e.g., from Identity) used for naming/ID.</param>
        /// <param name="title">The title or subject of the notification.</param>
        /// <param name="caption">A short description or caption to show in previews.</param>
        /// <param name="message">The main message content.</param>
        /// <param name="notificationUrl">A URL for further details if needed.</param>
        public NotificationDto(string notificationId, UserInfoDto user, string title, string caption, string message, string notificationUrl)
        {
            NotificationId = notificationId;
            EntityId = user.UserId;
            MessageType = MessageType.Learner;
            ReceiverId = user.UserId;
            ReceiverName = user.FirstName + " " + user.LastName;
            ReceiverImageUrl = "";
            Title = title;
            ShortDescription = caption;
            Message = message;
            Created = DateTime.Now;
            NotificationUrl = notificationUrl;
        }

        /// <summary>
        /// Specialized constructor for a scenario where a team member is added/removed,
        /// establishing the basics for a notification around that event.
        /// </summary>
        /// <param name="notificationId">New ID for the notification.</param>
        /// <param name="user">Contains relevant info such as receiver ID and name.</param>
        /// <param name="title">Notification title.</param>
        /// <param name="caption">Short descriptive text/caption.</param>
        /// <param name="message">The main text message or description of the change.</param>
        /// <param name="notificationUrl">A link to more info if needed.</param>
        public NotificationDto(string notificationId, TeamMemberChangeNotificationItem user, string title, string caption, string message, string notificationUrl)
        {
            NotificationId = notificationId;
            EntityId = user.ReceiverId;
            MessageType = user.NotificationItemType;
            ReceiverId = user.ReceiverId;
            ReceiverName = user.MemberName;
            ReceiverImageUrl = "";
            Title = title;
            ShortDescription = caption;
            Message = message;
            Created = DateTime.Now;
            NotificationUrl = notificationUrl;
        }

        #endregion

        /// <summary>
        /// Indicates the notification's message type, e.g. Global, Learner, etc.
        /// </summary>
        public MessageType MessageType { get; init; } = MessageType.Global;

        /// <summary>
        /// An optional entity ID (e.g., referencing a specific resource).
        /// </summary>
        public string? EntityId { get; init; }

        /// <summary>
        /// The category of the entity that the notification is being sent to
        /// </summary>
        public string? Category { get; init; }

        /// <summary>
        /// An optional message ID, if this notification relates to a specific message.
        /// </summary>
        public string? MessageId { get; init; }

        /// <summary>
        /// The unique identifier for the notification.
        /// </summary>
        public string? NotificationId { get; init; }

        /// <summary>
        /// The receiver's user ID (the user who gets this notification).
        /// </summary>
        public string? ReceiverId { get; init; }

        /// <summary>
        /// The receiver's display name.
        /// </summary>
        public string? ReceiverName { get; init; }

        /// <summary>
        /// The receiver's image URL, if available (e.g., for avatars).
        /// </summary>
        public string? ReceiverImageUrl { get; init; }

        /// <summary>
        /// The title or main heading of this notification.
        /// </summary>
        public string? Title { get; init; }

        /// <summary>
        /// A concise description or preview for UI display.
        /// </summary>
        public string? ShortDescription { get; init; }

        /// <summary>
        /// The core message or content of the notification.
        /// </summary>
        public string? Message { get; init; }

        /// <summary>
        /// An optional gender filter, defaults to All.
        /// </summary>
        public Gender Gender { get; init; } = Gender.All;

        /// <summary>
        /// A URL linking to more details about this notification.
        /// </summary>
        public string? NotificationUrl { get; init; }

        /// <summary>
        /// Timestamp for when the notification was created.
        /// </summary>
        public DateTime? Created { get; init; } = DateTime.Now;

        /// <summary>
        /// Timestamp for when the user viewed/opened this notification, if any.
        /// </summary>
        public DateTime? ReadTime { get; init; }

        /// <summary>
        /// Timestamp for when the notification was sent, if any.
        /// </summary>
        public DateTime? SendTime { get; init; }

        /// <summary>
        /// A list of device tokens that were notified.
        /// </summary>
        public List<string>? DeviceTokensNotified { get; init; } = [];

        /// <summary>
        /// A list of notification subscriptions that were notified.
        /// </summary>
        public List<string>? NotificationSubscriptionsNotified { get; init; } = [];

        /// <summary>
        /// Indicates whether this notification is read (based on <see cref="ReadTime"/>).
        /// </summary>
        public bool Read => ReadTime != null;

        /// <summary>
        /// Indicates whether this notification is sent (based on <see cref="SendTime"/>).
        /// </summary>
        public bool Sent => SendTime != null;

        /// <summary>
        /// Gets or sets the collection of document links associated with the current instance.
        /// </summary>
        /// <remarks>This property can be used to store and retrieve links to related documents. 
        /// Modifications to the collection will directly affect the stored links.</remarks>
        public ICollection<string> DocumentLinks { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the collection of document files associated with this instance.
        /// </summary>
        public ICollection<DocumentDto> Documents { get; set; } = new List<DocumentDto>();

        /// <summary>
        /// Converts this DTO to its underlying <see cref="Notification"/> entity
        /// for persistence or other logic.
        /// </summary>
        /// <returns>A fully constructed <see cref="Notification"/> entity.</returns>
        public Notification ToNotification()
        {
            return new Notification
            {
                Id = NotificationId!,
                EntityId = EntityId,
                MessageId = MessageId!,
                Title = Title!,
                ShortDescription = ShortDescription,
                Description = Message!,
                ReceiverId = ReceiverId!,
                ReceiverName = string.IsNullOrEmpty(ReceiverName) ? "" : ReceiverName,
                ReceiverImageUrl = ReceiverImageUrl,
                NotificationUrl = NotificationUrl!,
                SentDate = SendTime,
                CreatedOn = Created,
                MessageType = MessageType,
                DeviceTokensNotified = string.Join(",", DeviceTokensNotified!),
                NotificationSubscriptionsNotified = string.Join(",", NotificationSubscriptionsNotified!)
            };
        }
    }
}
