using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;
using ConectOne.Domain.Enums;
using FilingModule.Domain.Entities;
using MessagingModule.Domain.Entities;

namespace SchoolsModule.Domain.Entities
{
    /// <summary>
    /// Represents a Learner entity within a school. This class extends a base entity class and provides
    /// a variety of navigation properties and data fields pertinent to a student (learner) in a school system.
    /// </summary>
    public class Learner : FileCollection<Learner, string>
    {
        /// <summary>
        /// A unique identifier (GUID) representing this learner as a child in the system. Possibly used 
        /// externally or for integration with another system.
        /// </summary>
        public string ChildGuid { get; set; } = null!;

        /// <summary>
        /// The learner's first name.
        /// </summary>
        public string FirstName { get; set; } = null!;

        /// <summary>
        /// The learner's middle name (optional).
        /// </summary>
        public string? MiddleName { get; set; }

        /// <summary>
        /// A display name for the learner, which could be a preferred or nickname.
        /// </summary>
        public string? DisplayName { get; set; }

        /// <summary>
        /// The learner's last name.
        /// </summary>
        public string LastName { get; set; } = null!;

        /// <summary>
        /// A general description or additional notes about the learner.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Additional medical notes or information that is specific to the learner.
        /// </summary>
        public string? MedicalNotes { get; set; }

        /// <summary>
        /// Identifier of the parent whose medical aid should be used for this learner.
        /// </summary>
        [ForeignKey(nameof(MedicalAidParent))] public string? MedicalAidParentId { get; set; }

        /// <summary>
        /// Navigation property for the parent whose medical aid is associated with the learner.
        /// </summary>
        public Parent? MedicalAidParent { get; set; }

        /// <summary>
        /// An identifier number for the learner, possibly a student ID or national ID.
        /// </summary>
        public string IdNumber { get; set; } = null!;

        /// <summary>
        /// The learner's gender as defined in the Gender enum (Male, Female, Other).
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// A unique identifier representing the school this learner attends.
        /// </summary>
        public string SchoolUid { get; set; } = null!;

        /// <summary>
        /// Indicates if at least one of the learner's parents requires consent before certain actions or events.
        /// This property is derived by checking if any parent has ParentConsentRequired set to true.
        /// </summary>
        public bool RequireParentConsent => Parents.Any(c => c.ParentConsentRequired);

        /// <summary>
        /// Indicates whether consent must be obtained from all parents (if there are multiple parents) 
        /// before proceeding with certain actions related to this learner.
        /// </summary>
        public bool RequireConsentFromAllParents { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the parent receives notifications.
        /// </summary>
        public bool ReceiveNotifications { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the parent receives messages.
        /// </summary>
        public bool ReceiveMessages { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the parent receives emails.
        /// </summary>
        public bool RecieveEmails { get; set; } = true;

        #region One-to-Many Relationships

        /// <summary>
        /// Foreign key linking to the SchoolGrade entity. Represents the grade the learner is currently in.
        /// </summary>
        [ForeignKey(nameof(SchoolGrade))] public string? SchoolGradeId { get; set; }

        /// <summary>
        /// Navigation property for the SchoolGrade the learner is associated with. 
        /// A learner belongs to a single grade.
        /// </summary>
        public SchoolGrade? SchoolGrade { get; set; }

        /// <summary>
        /// Foreign key linking to the SchoolClass entity, indicating which class the learner belongs to.
        /// </summary>
        [ForeignKey(nameof(SchoolClass))] public string? SchoolClassId { get; set; }

        /// <summary>
        /// Navigation property for the SchoolClass the learner is currently enrolled in.
        /// A learner belongs to a single class.
        /// </summary>
        public SchoolClass? SchoolClass { get; set; }

        #endregion

        #region Many-to-One Relationships

        /// <summary>
        /// A collection of parent-learner relationships, linking the learner to zero, one, or multiple parents.
        /// These records can indicate custody, contact details, and consent requirements.
        /// </summary>
        public virtual ICollection<LearnerParent> Parents { get; set; } = [];

        /// <summary>
        /// A collection of contact numbers associated with the learner. This might include primary, secondary, or emergency numbers.
        /// Each ContactNumber is typed to Learner, ensuring the relationship is specific.
        /// </summary>
        public virtual ICollection<ContactNumber<Learner>> ContactNumbers { get; set; } = [];

        /// <summary>
        /// A collection of email addresses associated with the learner. This might include a school email or a personal email.
        /// Each EmailAddress is typed to Learner, ensuring the relationship is specific.
        /// </summary>
        public virtual ICollection<EmailAddress<Learner>> EmailAddresses { get; set; } = [];

        /// <summary>
        /// A collection of activity groups or teams that the learner is a member of. 
        /// For example, sports teams, clubs, or extracurricular groups.
        /// </summary>
        public virtual ICollection<ActivityGroupTeamMember> ActivityGroups { get; set; } = [];

        /// <summary>
        /// A collection of messages related to this learner, possibly messages addressed to them or related to their activities.
        /// </summary>
        public virtual ICollection<Message> Messages { get; set; } = [];

        /// <summary>
        /// A collection of consents (ParentPermission) related to events. This can represent
        /// whether parents have granted or withheld permission for the learner to participate in certain events.
        /// </summary>
        public virtual ICollection<ParentPermission> EventConsents { get; set; } = [];

        /// <summary>
        /// Collection of disciplinary incidents associated with this learner.
        /// </summary>
        public virtual ICollection<DisciplinaryIncident> DisciplinaryIncidents { get; set; } = [];

        #endregion
    }
}
