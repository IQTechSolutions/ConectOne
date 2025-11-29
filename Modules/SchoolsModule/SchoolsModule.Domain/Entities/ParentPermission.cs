using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;
using GroupingModule.Domain.Entities;
using MessagingModule.Domain.Enums;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Domain.Entities
{
    /// <summary>
    /// Represents a parent's consent or permission for a specific activity, event, or learner-related action.
    /// </summary>
    /// <remarks>This class is used to track and manage parental permissions, including the type of consent, 
    /// whether it has been granted, and its association with specific entities such as a parent, learner,  event, or
    /// activity group. It supports scenarios where parental consent is required for participation  in school-related
    /// activities or events.</remarks>
    public class ParentPermission : EntityBase<string>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ParentPermission"/> class.
        /// </summary>
        public ParentPermission() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParentPermission"/> class with the specified parameters.
        /// </summary>
        /// <remarks>The <paramref name="parameters"/> object must provide all required values for the
        /// properties of the <see cref="ParentPermission"/> instance. The <see cref="Id"/> property is automatically
        /// generated as a new GUID string during initialization.</remarks>
        /// <param name="parameters">An object containing the parameters required to initialize the <see cref="ParentPermission"/> instance. This
        /// includes consent type, parent ID, learner ID, event ID, participating activity group ID, consent direction,
        /// and consent status.</param>
        public ParentPermission(TeamMemberPermissionsParams parameters)
        {
            Id = Guid.NewGuid().ToString();
            ConsentType = parameters.ConsentType;
            ParentId = parameters.ParentId;
            Granted = parameters.Consent;
            LearnerId = parameters.LearnerId;
            EventId = parameters.EventId;
            ParticipatingActivityGroupId = parameters.ParticipatingActivityGroupId;
            ConsentDirection = parameters.ConsentDirection;
        }

        #endregion

        /// <summary>
        /// Gets or sets the type of consent associated with the current operation.
        /// </summary>
        public ConsentTypes ConsentType { get; set; }

        /// <summary>
        /// Gets or sets the direction of consent for the current operation.
        /// </summary>
        public ConsentDirection? ConsentDirection { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the requested permission has been granted.
        /// </summary>
        public bool Granted { get; set; } = false;

        /// <summary>
        /// Gets or sets the identifier of the parent entity.
        /// </summary>
        [ForeignKey(nameof(Parent))] public string? ParentId { get; set; }

        /// <summary>
        /// Gets or sets the parent object associated with this instance.
        /// </summary>
        public Parent? Parent { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the learner associated with this entity.
        /// </summary>
        [ForeignKey(nameof(Learner))] public string? LearnerId { get; set; }

        /// <summary>
        /// Gets or sets the learner associated with the current context.
        /// </summary>
        public Learner? Learner { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the associated event.
        /// </summary>
        [ForeignKey(nameof(Event))] public string? EventId { get; set; }

        /// <summary>
        /// Gets or sets the school event associated with a specific category of activity groups.
        /// </summary>
        public SchoolEvent<Category<ActivityGroup>>? Event { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the participating activity group.
        /// </summary>
        [ForeignKey(nameof(ParticipatingActivityGroup))] public string? ParticipatingActivityGroupId { get; set; }

        /// <summary>
        /// Gets or sets the participating activity group associated with the current context.
        /// </summary>
        public ParticipatingActivityGroup? ParticipatingActivityGroup { get; set; }
    }
}
