using System.ComponentModel.DataAnnotations;
using ConectOne.Application.ViewModels;
using SchoolsModule.Domain.DataTransferObjects;

namespace SchoolsModule.Application.ViewModels
{
    /// <summary>
    /// Represents a parent entity in a user interface-friendly format. This view model aggregates 
    /// personal details, contact information, address details, and various preferences into a single structure 
    /// that can be easily rendered by a UI or manipulated within client-side logic.
    /// 
    /// Key Characteristics:
    /// - Contains identity and profile-related fields (Name, CoverImageUrl, DisplayName).
    /// - Holds communication preferences (ReceiveNotifications, ReceiveMessages, ReceiveEmails).
    /// - Integrates related view models for address, contact numbers, and email addresses for seamless UI binding.
    /// - Reflects business logic requirements such as needing consent or being registered as a user.
    /// 
    /// Usage Scenarios:
    /// - Displaying parent details in a web or mobile application’s profile page.
    /// - Using the integrated address, contact number, and email address view models 
    ///   to build a comprehensive editing form for a parent’s personal data.
    /// - Passing data back to the server, as this ViewModel can be reversed into a DTO or entity to update records.
    /// 
    /// By consolidating parent-related data into one view model, this class simplifies both data retrieval 
    /// and state management in the UI layer.
    /// </summary>
    public class ParentViewModel
    {
        #region Constructors

        /// <summary>
        /// Default constructor for ParentViewModel.
        /// </summary>
        public ParentViewModel() { }

        /// <summary>
        /// Constructs a ParentViewModel from a ParentDto.
        /// Maps the properties from the DTO to the view model.
        /// </summary>
        /// <param name="dto">The data transfer object containing parent information.</param>
        public ParentViewModel(ParentDto dto)
        {
            ParentId = dto.ParentId;
            CoverImageUrl = dto.CoverImageUrl;
            FirstName = dto.FirstName;
            LastName = dto.LastName;
            MiddleName = dto.MiddleName;
            DisplayName = dto.DisplayName;
            LastName = dto.LastName;
            Description = dto.Description;
            WalletId = dto.WalletId;
            WalletUserId = dto.WalletUserId;
            RequireConsent = dto.RequireConsent;
            ReceiveNotifications = dto.ReceiveNotifications;
            ReceiveMessages = dto.ReceiveMessages;
            ReceiveEmails = dto.RecieveEmails;

            EmergencyMedicalInfo = dto.EmergencyMedicalInfo;
            MedicalAidProvider = dto.MedicalAidProvider;
            MedicalAidPlan = dto.MedicalAidPlan;
            MedicalAidNumber = dto.MedicalAidNumber;
            MedicalAidMainMember = dto.MedicalAidMainMember;
            MedicalAidMainMemberIdNumber = dto.MedicalAidMainMemberIdNumber;
            PrimaryPhysicianName = dto.PrimaryPhysicianName;
            PrimaryPhysicianContactNumber = dto.PrimaryPhysicianContactNumber;

            ContactNumbers = dto.ContactNumbers.Select(c => new ContactNumberViewModel(c)).ToList();
            EmailAddresses = dto.EmailAddresses.Select(c => new EmailAddressViewModel(c)).ToList();
            EmergencyContacts = dto.EmergencyContacts.Select(c => new ParentEmergencyContactViewModel(c)).ToList();
        }

        #endregion

        /// <summary>
        /// The unique identifier for the parent.
        /// If null, it may indicate a newly created parent record not yet persisted in the database.
        /// </summary>
        public string? ParentId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// A URL to a cover image representing the parent.
        /// Can be displayed in UI as a profile header or background.
        /// </summary>
        public string? CoverImageUrl { get; set; }

        /// <summary>
        /// The parent's first name. This field is required (non-nullable) to ensure proper identification.
        /// </summary>
        public string FirstName { get; set; } = null!;

        /// <summary>
        /// An optional middle name for the parent, if applicable.
        /// </summary>
        public string? MiddleName { get; set; }

        /// <summary>
        /// An optional display name that the parent might prefer, 
        /// useful for informal or nickname-based identification.
        /// </summary>
        public string? DisplayName { get; set; }

        /// <summary>
        /// The parent's last name. Required to ensure complete identification details.
        /// </summary>
        public string LastName { get; set; } = null!;

        /// <summary>
        /// A textual description or bio for the parent, providing additional context or information.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The parent's associated wallet ID, if the system supports a wallet or payment system.
        /// </summary>
        public int? WalletId { get; set; }

        /// <summary>
        /// The parent's associated wallet user ID, linking them to a payment 
        /// or financial profile in the system.
        /// </summary>
        public int? WalletUserId { get; set; }

        /// <summary>
        /// Indicates if the parent is required to provide consent for certain actions,
        /// such as approving a child's participation in activities.
        /// </summary>
        public bool RequireConsent { get; set; } = true;

        /// <summary>
        /// Indicates if the parent should receive notifications (e.g., SMS, push notifications)
        /// from the system.
        /// </summary>
        public bool ReceiveNotifications { get; set; } = true;

        /// <summary>
        /// Indicates if the parent should receive internal messages from the system,
        /// such as inbox messages or system alerts.
        /// </summary>
        public bool ReceiveMessages { get; set; } = true;

        /// <summary>
        /// Indicates if the parent should receive emails from the system,
        /// such as newsletters, event updates, or notifications.
        /// </summary>
        public bool ReceiveEmails { get; set; } = true;

        /// <summary>
        /// General emergency medical information provided by the parent.
        /// </summary>
        public string? EmergencyMedicalInfo { get; set; }

        /// <summary>
        /// The parent's medical aid provider.
        /// </summary>
        public string? MedicalAidProvider { get; set; }

        /// <summary>
        /// The plan or option registered with the medical aid provider.
        /// </summary>
        public string? MedicalAidPlan { get; set; }

        /// <summary>
        /// The parent's medical aid membership or policy number.
        /// </summary>
        public string? MedicalAidNumber { get; set; }

        /// <summary>
        /// The main member associated with the parent's medical aid.
        /// </summary>
        public string? MedicalAidMainMember { get; set; }

        /// <summary>
        /// The identification number for the medical aid main member.
        /// </summary>
        [MinLength(13)] public string? MedicalAidMainMemberIdNumber { get; set; }

        /// <summary>
        /// The parent's preferred primary physician.
        /// </summary>
        public string? PrimaryPhysicianName { get; set; }

        /// <summary>
        /// Contact number for the parent's primary physician.
        /// </summary>
        public string? PrimaryPhysicianContactNumber { get; set; }

        /// <summary>
        /// A collection of <see cref="ContactNumberViewModel"/> objects 
        /// representing the parent's phone numbers. 
        /// Allows multiple entries for diverse contact preferences (e.g., home, work, mobile).
        /// </summary>
        public List<ContactNumberViewModel> ContactNumbers { get; set; } = [];

        /// <summary>
        /// A collection of <see cref="EmailAddressViewModel"/> objects
        /// representing the parent's email addresses.
        /// Supports multiple addresses and indicates if one is default.
        /// </summary>
        public List<EmailAddressViewModel> EmailAddresses { get; set; } = [];

        /// <summary>
        /// A collection of emergency contacts associated with the parent.
        /// </summary>
        public List<ParentEmergencyContactViewModel> EmergencyContacts { get; set; } = [];

        /// <summary>
        /// A list of <see cref="LearnerDto"/> objects referencing all 
        /// learners (or dependents) for which this parent might have responsibility.
        /// </summary>
        public List<LearnerDto> Dependants { get; set; } = new();

        #region Methods

        /// <summary>
        /// Converts the current instance of the parent entity to a <see cref="ParentDto"/> object.
        /// </summary>
        /// <remarks>This method maps the properties of the parent entity to their corresponding
        /// properties in the  <see cref="ParentDto"/> object. Collections such as contact numbers and email addresses
        /// are  also converted to their DTO equivalents.</remarks>
        /// <returns>A <see cref="ParentDto"/> object containing the data from the current parent entity.</returns>
        public ParentDto ToDto()
        {
            return new ParentDto
            {
                ParentId = this.ParentId!,
                CoverImageUrl = this.CoverImageUrl,
                FirstName = this.FirstName,
                MiddleName = this.MiddleName,
                LastName = this.LastName,
                DisplayName = this.DisplayName,
                Description = this.Description,
                WalletId = this.WalletId,
                WalletUserId = this.WalletUserId,
                RequireConsent = this.RequireConsent,
                ReceiveNotifications = this.ReceiveNotifications,
                ReceiveMessages = this.ReceiveMessages,
                RecieveEmails = this.ReceiveEmails,
                EmergencyMedicalInfo = this.EmergencyMedicalInfo,
                MedicalAidProvider = this.MedicalAidProvider,
                MedicalAidPlan = this.MedicalAidPlan,
                MedicalAidNumber = this.MedicalAidNumber,
                MedicalAidMainMember = this.MedicalAidMainMember,
                MedicalAidMainMemberIdNumber = this.MedicalAidMainMemberIdNumber,
                PrimaryPhysicianName = this.PrimaryPhysicianName,
                PrimaryPhysicianContactNumber = this.PrimaryPhysicianContactNumber,
                ContactNumbers = this.ContactNumbers.Select(c => c.ToDto()).ToList(),
                EmailAddresses = this.EmailAddresses.Select(e => e.ToDto()).ToList(),
                Learners = this.Dependants
            };
        }

        #endregion
    }
}
