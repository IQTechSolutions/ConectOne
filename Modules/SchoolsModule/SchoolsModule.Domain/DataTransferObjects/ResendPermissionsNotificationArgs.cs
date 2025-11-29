using MessagingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.Enums;

namespace SchoolsModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents the arguments required to resend a permissions notification for a specific event or activity.
    /// </summary>
    /// <remarks>This class encapsulates the details of a learner, event, and associated entities required to
    /// generate and send a permissions notification. It includes optional fields for specifying the event date, URL,
    /// and consent type.</remarks>
    public class ResendPermissionsNotificationArgs
    {
        /// <summary>
        /// Gets or sets the unique identifier for the learner.
        /// </summary>
        public string? LearnerId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the name of the learner.
        /// </summary>
        public string? LearnerName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the participating team member.
        /// </summary>
        public string? ParticipatingTeamMemberId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for the event.
        /// </summary>
        public string? EventId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the name of the event.
        /// </summary>
        public string? EventName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the participating activity group.
        /// </summary>
        public string? ParticipatingActivityGroupId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the date and time of the event.
        /// </summary>
        public DateTime? EventDate { get; set; }

        /// <summary>
        /// Gets or sets the URL associated with the event.
        /// </summary>
        public string? EventUrl { get; set; } = null!;

        /// <summary>
        /// Gets or sets the type of consent provided by the user.
        /// </summary>
        public ConsentTypes? ConsentType { get; set; }

        /// <summary>
        /// Gets or sets the message associated with the current operation.
        /// </summary>
        public MessageDto? Message { get; set; } = null!;
    }
}
