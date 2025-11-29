using System.ComponentModel.DataAnnotations;
using ConectOne.Domain.RequestFeatures;
using NeuralTech.Base.Enums;

namespace MessagingModule.Domain.RequestFeatures
{
    /// <summary>
    /// Defines paging and filtering parameters for message retrieval. 
    /// This class inherits from <see cref="RequestParameters"/>, 
    /// providing additional properties to filter messages by date ranges, 
    /// search text, message type, sender, or receiver.
    /// </summary>
    public class MessagePageParameters : RequestParameters
    {
        /// <summary>
        /// Used for identifying a specific notification ID, if relevant.
        /// </summary>
        public string? NotificationId { get; set; }

        /// <summary>
        /// Optional text to filter messages (e.g., by subject or content).
        /// </summary>
        public string? SearchText { get; set; }

        /// <summary>
        /// If set, filters messages to those whose receiver matches this user ID.
        /// </summary>
        public string? ReceiverId { get; set; }

        /// <summary>
        /// If set, filters messages to those whose sender matches this user ID.
        /// </summary>
        public string? SenderId { get; set; }

        /// <summary>
        /// An optional entity ID to further filter messages 
        /// (e.g., tied to a particular event, product, or domain entity).
        /// </summary>
        public string? EntityId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is publicly accessible.
        /// </summary>
        public bool Public { get; set; }

        /// <summary>
        /// A message type (e.g., Global, Learner, Parent) for additional filtering.
        /// </summary>
        public MessageType? MessageType { get; set; }

        /// <summary>
        /// Start date for filtering messages (only messages on or after this date).
        /// </summary>
        [DataType(DataType.Date)] public DateTime? StartDateFilter { get; set; }

        /// <summary>
        /// End date for filtering messages (only messages on or before this date).
        /// </summary>
        [DataType(DataType.Date)] public DateTime? EndDateFilter { get; set; }
    }
}
