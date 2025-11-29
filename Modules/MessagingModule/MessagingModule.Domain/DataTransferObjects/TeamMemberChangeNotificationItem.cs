using NeuralTech.Base.Enums;

namespace MessagingModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents data needed to notify a particular user about a team member's addition 
    /// or removal in an activity group or event context. This class specifies the 
    /// receiving user, the team member’s name, and the type of message/notification.
    /// </summary>
    public class TeamMemberChangeNotificationItem
    {
        /// <summary>
        /// The ID of the user set to receive this notification.
        /// </summary>
        public string ReceiverId { get; set; } = null!;

        /// <summary>
        /// The display name of the team member whose membership changed.
        /// </summary>
        public string MemberName { get; set; } = null!;

        /// <summary>
        /// The type/category of notification (e.g., 
        /// <see cref="MessageType.ActivityGroup"/> or <see cref="MessageType.ParticipatingActivityGroup"/>).
        /// </summary>
        public MessageType NotificationItemType { get; set; }
    }
}
