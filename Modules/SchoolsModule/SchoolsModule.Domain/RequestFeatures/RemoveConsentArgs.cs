using MessagingModule.Domain.Enums;

namespace SchoolsModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents the arguments required to remove a consent for a specific learner and event.
    /// </summary>
    /// <remarks>This class encapsulates the details necessary to identify and remove a consent, including the
    /// learner, the event, the type of consent, and an optional parent identifier.</remarks>
    public class RemoveConsentArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveConsentArgs"/> class.
        /// </summary>
        public RemoveConsentArgs() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveConsentArgs"/> class with the specified learner ID, event
        /// ID, consent type, and optional parent ID.
        /// </summary>
        /// <param name="learnerId">The unique identifier of the learner for whom the consent is being removed. This value cannot be null or
        /// empty.</param>
        /// <param name="eventId">The unique identifier of the event associated with the consent. This value cannot be null or empty.</param>
        /// <param name="consentType">The type of consent being removed. This value must be a valid <see cref="ConsentTypes"/> enumeration value.</param>
        /// <param name="parentId">The optional identifier of the parent entity associated with the consent. This value can be null if no
        /// parent entity is applicable.</param>
        public RemoveConsentArgs(string learnerId, string eventId, ConsentTypes consentType, string? parentId = null)
        {
            ParentId = parentId;
            LearnerId = learnerId;
            EventId = eventId;
            ConsentType = consentType;
        }

        #endregion

        /// <summary>
        /// Gets or sets the identifier of the parent entity.
        /// </summary>
        public string? ParentId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for the event.
        /// </summary>
        public string EventId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for a learner.
        /// </summary>
        public string LearnerId { get; set; }

        /// <summary>
        /// Gets or sets the type of consent associated with the current operation.
        /// </summary>
        public ConsentTypes? ConsentType { get; set; }  
    }
}
