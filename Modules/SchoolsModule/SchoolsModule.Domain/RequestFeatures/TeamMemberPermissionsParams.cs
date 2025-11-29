using MessagingModule.Domain.Enums;

namespace SchoolsModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents the parameters required to manage permissions for a team member in the context of an activity or
    /// event.
    /// </summary>
    /// <remarks>This record encapsulates information about a team member's participation in an activity or
    /// event,  including their consent status and related metadata. It is typically used to track and manage 
    /// permissions or consent for specific actions within a team-based activity.</remarks>
    public record TeamMemberPermissionsParams
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamMemberPermissionsParams"/> class.
        /// </summary>
        public TeamMemberPermissionsParams() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamMemberPermissionsParams"/> class with the specified
        /// parameters.
        /// </summary>
        /// <param name="eventId">The unique identifier of the event associated with the permissions.</param>
        /// <param name="parentId">The unique identifier of the parent entity related to the permissions.</param>
        /// <param name="learnerId">The unique identifier of the learner for whom the permissions are being set.</param>
        /// <param name="participatingActivityGroupId">The unique identifier of the activity group in which the learner is participating.</param>
        /// <param name="participatingTeamMemberId">The unique identifier of the team member associated with the permissions.</param>
        /// <param name="consent">A value indicating whether consent has been granted. <see langword="true"/> if consent is granted;
        /// otherwise, <see langword="false"/>.</param>
        /// <param name="consentType">The type of consent being granted, represented by the <see cref="ConsentTypes"/> enumeration.</param>
        /// <param name="consentDirection">An optional value indicating the direction of consent, represented by the <see cref="ConsentDirection"/>
        /// enumeration. This can be <see langword="null"/> if no specific direction is applicable.</param>
        public TeamMemberPermissionsParams(string eventId, string parentId, string learnerId, string participatingActivityGroupId, string participatingTeamMemberId, bool consent, 
            ConsentTypes consentType, ConsentDirection? consentDirection = null)
        {
            EventId = eventId;
            ParentId = parentId;
            ParticipatingActivityGroupId = participatingActivityGroupId;
            LearnerId = learnerId;
            ParticipatingTeamMemberId = participatingTeamMemberId;
            Consent = consent;
            ConsentType = consentType;
            ConsentDirection = consentDirection;
        }

        #endregion

        /// <summary>
        /// Gets the unique identifier for the event.
        /// </summary>
        public string? EventId { get; init; }

        /// <summary>
        /// Gets the identifier of the parent entity, if one exists.
        /// </summary>
        public string? ParentId { get; init; }

        /// <summary>
        /// Gets the identifier of the participating activity group.
        /// </summary>
        public string? ParticipatingActivityGroupId { get; init; }

        /// <summary>
        /// Gets the unique identifier for the learner.
        /// </summary>
        public string? LearnerId { get; init; }

        /// <summary>
        /// Gets the unique identifier of the participating team member.
        /// </summary>
        public string? ParticipatingTeamMemberId { get; init; }

        /// <summary>
        /// Gets a value indicating whether the user has provided consent.
        /// </summary>
        public bool Consent { get; init; }

        /// <summary>
        /// Gets the type of consent associated with the current operation.
        /// </summary>
        public ConsentTypes ConsentType { get; init; }

        /// <summary>
        /// Gets the direction of consent associated with the current operation.
        /// </summary>
        public ConsentDirection? ConsentDirection { get; init; }
    }
}
