using ConectOne.Domain.Entities;
using FilingModule.Domain.Entities;

namespace SchoolsModule.Domain.Entities
{
    /// <summary>
    /// Represents a Parent entity within the school module. Inherits from ImageFileCollection to manage associated images.
    /// </summary>
    public class Parent : FileCollection<Parent, string>
    {
        /// <summary>
        /// Gets or sets the ID number of the parent.
        /// </summary>
        public string? ParentIdNumber { get; set; }

        /// <summary>
        /// Gets or sets the first name of the parent.
        /// </summary>
        public string FirstName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the middle name of the parent.
        /// </summary>
        public string? MiddleName { get; set; }

        /// <summary>
        /// Gets or sets the display name of the parent.
        /// </summary>
        public string? DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the parent.
        /// </summary>
        public string LastName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the description of the parent.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the wallet ID associated with the parent.
        /// </summary>
        public int? WalletId { get; set; }

        /// <summary>
        /// Gets or sets the wallet user ID associated with the parent.
        /// </summary>
        public int? WalletUserId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether consent is required from the parent.
        /// </summary>
        public bool RequireConsent { get; set; } = true;

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

        /// <summary>
        /// Gets or sets general emergency medical information supplied by the parent.
        /// </summary>
        public string? EmergencyMedicalInfo { get; set; }

        /// <summary>
        /// Gets or sets the parent's medical aid provider.
        /// </summary>
        public string? MedicalAidProvider { get; set; }

        /// <summary>
        /// Gets or sets the medical aid plan the parent belongs to.
        /// </summary>
        public string? MedicalAidPlan { get; set; }

        /// <summary>
        /// Gets or sets the parent's medical aid number.
        /// </summary>
        public string? MedicalAidNumber { get; set; }

        /// <summary>
        /// Gets or sets the main member name for the medical aid.
        /// </summary>
        public string? MedicalAidMainMember { get; set; }

        /// <summary>
        /// Gets or sets the identification number of the medical aid's main member.
        /// </summary>
        public string? MedicalAidMainMemberIdNumber { get; set; }

        /// <summary>
        /// Gets or sets the primary physician's name for this parent's medical aid details.
        /// </summary>
        public string? PrimaryPhysicianName { get; set; }

        /// <summary>
        /// Gets or sets the contact number for the parent's primary physician.
        /// </summary>
        public string? PrimaryPhysicianContactNumber { get; set; }

        #region Many to One Relationships

        /// <summary>
        /// Gets or sets the collection of addresses associated with the parent.
        /// </summary>
        public virtual ICollection<Address<Parent>> Addresses { get; set; } = null!;

        /// <summary>
        /// Gets or sets the collection of contact numbers associated with the parent.
        /// </summary>
        public virtual ICollection<ContactNumber<Parent>> ContactNumbers { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of email addresses associated with the parent.
        /// </summary>
        public virtual ICollection<EmailAddress<Parent>> EmailAddresses { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of emergency contacts associated with the parent.
        /// </summary>
        public virtual ICollection<ParentEmergencyContact> EmergencyContacts { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of learners associated with the parent.
        /// </summary>
        public virtual ICollection<LearnerParent> Learners { get; set; } = [];

        /// <summary>
        /// Gets or sets the learners that rely on this parent's medical aid information.
        /// </summary>
        public virtual ICollection<Learner> MedicalAidLearners { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of event consents associated with the parent.
        /// </summary>
        public virtual ICollection<ParentPermission> EventConsents { get; set; } = [];

        #endregion
    }
}
