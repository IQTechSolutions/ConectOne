using MessagingModule.Domain.Enums;

namespace SchoolsModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents the permissions granted by a parent for a team member in the context of a specific event.
    /// </summary>
    /// <remarks>This data transfer object encapsulates information about a parent's consent for a learner's
    /// participation in an event, including the type of consent, the associated event, and the identifiers for the
    /// learner and parent.</remarks>
    public record TeamMemberParentPermissionDto
    {
        /// <summary>
        /// Gets the type of consent associated with this instance.
        /// </summary>
        public ConsentTypes ConsentType { get; init; }

        /// <summary>
        /// Gets the unique identifier for the learner.
        /// </summary>
        public string? LearnerId { get; init; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the parent entity.
        /// </summary>
        public string? ParentId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for the event.
        /// </summary>
        public string EventId { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether the user has provided consent.
        /// </summary>
        public bool Consent { get; set; }

    }
}
