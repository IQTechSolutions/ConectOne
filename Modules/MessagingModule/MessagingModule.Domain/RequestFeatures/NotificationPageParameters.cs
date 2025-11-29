using ConectOne.Domain.RequestFeatures;
using NeuralTech.Base.Enums;

namespace MessagingModule.Domain.RequestFeatures
{
    /// <summary>
    /// Defines paging and filtering parameters for notifications. Inherits from <see cref="RequestParameters"/>,
    /// adding properties to filter notifications by message type, receiver, or entity.
    /// </summary>
    public class NotificationPageParameters : RequestParameters
    {
        /// <summary>
        /// An optional message type (e.g., Global, Learner, etc.) to filter notifications.
        /// </summary>
        public MessageType? MessageType { get; set; }

        /// <summary>
        /// If set, restricts results to notifications intended for a specific user (ReceiverId).
        /// </summary>
        public string? ReceiverId { get; set; }

        /// <summary>
        /// If set, restricts results to notifications tied to a specific entity (e.g., event or resource).
        /// </summary>
        public string? EntityId { get; set; }
    }
}
