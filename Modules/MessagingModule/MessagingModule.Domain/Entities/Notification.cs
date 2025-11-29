using ConectOne.Domain.Entities;
using NeuralTech.Base.Enums;

namespace MessagingModule.Domain.Entities
{
    /// <summary>
    /// Represents a notification record in the system. Stores information about 
    /// who receives it, when it was sent or read, and details such as a title, 
    /// body content, and a link to more information.
    /// </summary>
    public class Notification : EntityBase<string>
    {
        /// <summary>
        /// Identifies the type of message or notification 
        /// (e.g., Global, Learner, Parent, Teacher, etc.).
        /// </summary>
        public MessageType MessageType { get; set; } = MessageType.None;

        /// <summary>
        /// Optional entity ID, referencing a particular item/event in the system that this notification is tied to.
        /// </summary>
        public string? EntityId { get; set; }
        
        /// <summary>
        /// The unique ID of the user who is meant to receive this notification.
        /// </summary>
        public string ReceiverId { get; set; } = null!;

        /// <summary>
        /// The display name of the user receiving the notification, if available.
        /// </summary>
        public string ReceiverName { get; set; } = null!;

        /// <summary>
        /// A URL or path to the receiver’s image/avatar, if any.
        /// </summary>
        public string? ReceiverImageUrl { get; set; } = null!;

        /// <summary>
        /// The main title or heading for the notification.
        /// </summary>
        public string Title { get; set; } = null!;

        /// <summary>
        /// A short preview or snippet of the notification content.
        /// </summary>
        public string? ShortDescription { get; set; } = null!;

        /// <summary>
        /// The body or detailed description of the notification.
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// A link providing more context or details about the notification 
        /// (e.g., a deep link to a page).
        /// </summary>
        public string? NotificationUrl { get; set; } 

        /// <summary>
        /// The timestamp when the notification was sent or published.
        /// </summary>
        public DateTime? SentDate { get; set; }

        /// <summary>
        /// The timestamp when the user opened/read this notification. 
        /// Null if the user has not opened it yet.
        /// </summary>
        public DateTime? OpenedDate { get; set; }

        /// <summary>
        /// A comma-separated string of device tokens that have been notified (e.g., for push notifications).
        /// </summary>
        public string? DeviceTokensNotified { get; set; }

        /// <summary>
        /// A comma-separated string of notification subscriptions that have been notified (e.g., for push notifications).
        /// </summary>
        public string? NotificationSubscriptionsNotified { get; set; }

        #region Relationships

        /// <summary>
        /// Optional message ID if this notification is associated with a specific message.
        /// </summary>
        public string? MessageId { get; set; }

        #endregion

        #region Read-only Members 

        /// <summary>
        /// Indicates whether this notification has been read (true if <see cref="OpenedDate"/> is set).
        /// </summary>
        public bool Read => OpenedDate != null;

        /// <summary>
        /// Indicates whether this notification has been sent (true if <see cref="SentDate"/> is set).
        /// </summary>
        public bool Sent => SentDate != null;

        #endregion
    }
}
