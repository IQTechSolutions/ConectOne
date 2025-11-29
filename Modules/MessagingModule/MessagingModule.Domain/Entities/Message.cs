using ConectOne.Domain.Extensions;
using FilingModule.Domain.Entities;
using NeuralTech.Base.Enums;

namespace MessagingModule.Domain.Entities
{
    /// <summary>
    /// Represents a message or communication entity within the system, 
    /// potentially including references to sender/receiver, 
    /// attachments (files/documents), and message content/metadata.
    /// Inherits from <see cref="FileCollection{TEntity,TId}"/> to 
    /// support images and files attached to messages.
    /// </summary>
    public class Message : FileCollection<Message, string>
    {
        #region Properties
        
        /// <summary>
        /// The date/time when the recipient read the message, if any.
        /// </summary>
        public DateTime? ReadTime { get; set; }

        /// <summary>
        /// The date/time when the message was marked as delivered to the recipient, if any.
        /// </summary>
        public DateTime? DeliveredTime { get; set; }

        /// <summary>
        /// The subject or title of this message (cannot be null).
        /// </summary>
        public string Subject { get; set; } = null!;

        /// <summary>
        /// A shortened description or summary, often used for previews.
        /// </summary>
        public string? ShortDescription { get; set; }

        /// <summary>
        /// The main body or content of the message (cannot be null).
        /// </summary>
        public string Description { get; set; } = null!;

        #endregion

        #region Read Only

        /// <summary>
        /// A truncated version of <see cref="Description"/>, 
        /// limiting its length for UI previews (max 50 characters).
        /// </summary>
        public string TruncatedMessage => Description.TruncateLongString(50);

        /// <summary>
        /// Returns true if <see cref="ReadTime"/> is set, indicating the message has been read.
        /// </summary>
        public bool Read => ReadTime != null;

        /// <summary>
        /// Returns true if <see cref="DeliveredTime"/> is set, indicating the message has been delivered.
        /// </summary>
        public bool Delivered => DeliveredTime != null;

        /// <summary>
        /// Stores comma-separated links pointing to documents or external resources.
        /// </summary>
        public string? DocumentLinks { get; set; }

        /// <summary>
        /// Optional entity ID that this message may refer to (e.g., a product, event, or user).
        /// </summary>
        public string? EntityId { get; set; }

        /// <summary>
        /// Categorizes the message, e.g., Global, Learner, Event, etc.
        /// </summary>
        public MessageType MessageType { get; set; } = MessageType.Global;

        public bool Public { get; set; } = true;

        #endregion

        #region One-to-Many Relationships

        /// <summary>
        /// Gets or sets the identifier of the sender.
        /// </summary>
        public string? SenderId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier of the receiver.
        /// </summary>
        public string? ReceiverId { get; set; } = null!;

        #endregion
    }
}
