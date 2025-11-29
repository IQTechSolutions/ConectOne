using ConectOne.Domain.DataTransferObjects;
using ConectOne.Domain.Entities;
using FilingModule.Domain.Enums;
using IdentityModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for a Parent entity within the school system.
    /// This DTO is used to transfer parent-related data between different layers of the application,
    /// including personal information, contact details, and associated learners.
    /// 
    /// Key properties:
    /// - Identification and Personal Info: Id, ParentIdNumber, FirstName, MiddleName, DisplayName, LastName, Description
    /// - Additional Info: CoverImageUrl for profile images, optional Wallet details (WalletId, WalletUserId)
    /// - Communication and Consent Preferences: RequireConsent, ReceiveNotifications, ReceiveMessages, RecieveEmails
    /// - Related Data: Lists of Addresses, ContactNumbers, EmailAddresses, Learners (via LearnerParentDto), and EventConsents (ParentPermissionDto)
    /// 
    /// Use this DTO to present or manage parent information along with their associated learners and 
    /// their preferences for communication and event participation consent.
    /// </summary>
    public record ParentDto
    {
        #region Constructors

        /// <summary>
        /// Parameterless constructor, primarily for serialization or manual instantiation.
        /// </summary>
        public ParentDto() { }

        /// <summary>
        /// Creates a new <see cref="ParentDto"/> by mapping values from an existing <see cref="Parent"/> entity.
        /// This constructor is typically used when reading a Parent entity from the database
        /// and converting it into a DTO for network transmission or UI display.
        /// </summary>
        /// <param name="parent">The parent entity to map from.</param>
        public ParentDto(Parent parent, bool loadLearners = false)
        {
            if (parent is not null)
            {
                try
                {
                    ParentId = parent.Id;
                    FirstName = parent.FirstName;
                    MiddleName = parent.MiddleName;
                    DisplayName = parent.DisplayName;
                    LastName = parent.LastName;

                    Description = parent.Description;
                    WalletId = parent.WalletId;
                    WalletUserId = parent.WalletUserId;

                    EmergencyMedicalInfo = parent.EmergencyMedicalInfo;
                    MedicalAidProvider = parent.MedicalAidProvider;
                    MedicalAidPlan = parent.MedicalAidPlan;
                    MedicalAidNumber = parent.MedicalAidNumber;
                    MedicalAidMainMember = parent.MedicalAidMainMember;
                    MedicalAidMainMemberIdNumber = parent.MedicalAidMainMemberIdNumber;
                    PrimaryPhysicianName = parent.PrimaryPhysicianName;
                    PrimaryPhysicianContactNumber = parent.PrimaryPhysicianContactNumber;

                    CoverImageUrl = parent.Images?.FirstOrDefault(c => c.Image.ImageType == UploadType.Cover)?.Image.RelativePath ?? "_content/FilingModule.Blazor/images/profileImage128x128.png";
                    RequireConsent = parent.RequireConsent;

                    RecieveEmails = parent.RecieveEmails;
                    ReceiveNotifications = parent.ReceiveNotifications;
                    ReceiveMessages = parent.ReceiveMessages;

                    Addresses = parent.Addresses?.Select(c => new AddressDto(c)).ToList();
                    ContactNumbers = parent.ContactNumbers?.Select(c => new ContactNumberDto(c)).ToList();
                    EmailAddresses = parent.EmailAddresses?.Select(c => new EmailAddressDto(c)).ToList();
                    EmergencyContacts = parent.EmergencyContacts?.Select(c => new ParentEmergencyContactDto(c)).ToList();

                    if (loadLearners)
                        Learners = parent.Learners?.Select(c => new LearnerDto(c.Learner, false)).ToList() ?? new List<LearnerDto>();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        /// <summary>
        /// Creates a new <see cref="ParentDto"/> by mapping values from a <see cref="UserInfoViewModel"/>.
        /// This is primarily used when a parent record is cross-linked with an existing user account
        /// (for instance, an authenticated user filling parent fields).
        /// </summary>
        /// <param name="parent">The user info view model that includes personal details.</param>
        public ParentDto(UserInfoDto parent)
        {
            ParentId = parent.UserId;
            CoverImageUrl = parent.CoverImageUrl;
            FirstName = parent.FirstName;
            MiddleName = parent.MiddleName;
            DisplayName = parent.DisplayName;
            LastName = parent.LastName;

            Description = parent.Description;
            RequireConsent = parent.RequireConsent; // Typically, a parent would require consent for Learners.

            RecieveEmails = parent.ReceiveEmails;
            ReceiveNotifications = parent.ReceiveNotifications;
            ReceiveMessages = parent.ReceiveMessages;
        }

        #endregion

        /// <summary>
        /// The unique identifier of this parent record (often a Guid).
        /// If null or empty, the parent might not be persisted yet.
        /// </summary>
        public string? ParentId { get; set; }

        /// <summary>
        /// An optional parent ID number, which could be a national ID or other unique identifier.
        /// </summary>
        public string? ParentIdNumber { get; init; }

        /// <summary>
        /// URL to an image or avatar representing the parent.
        /// </summary>
        public string? CoverImageUrl { get; init; }

        /// <summary>
        /// The parent's first name.
        /// </summary>
        public string? FirstName { get; init; } = null!;

        /// <summary>
        /// The parent's middle name (if any).
        /// </summary>
        public string? MiddleName { get; init; }

        /// <summary>
        /// A display or preferred name for the parent, which may differ from their legal name.
        /// </summary>
        public string? DisplayName { get; init; }

        /// <summary>
        /// The parent's last name.
        /// </summary>
        public string? LastName { get; init; } = null!;

        /// <summary>
        /// A description or additional notes about the parent (e.g., profession, relation details).
        /// </summary>
        public string? Description { get; init; }

        /// <summary>
        /// The parent's wallet ID, if integrated with a payment or finance system.
        /// </summary>
        public int? WalletId { get; init; }

        /// <summary>
        /// The wallet user ID associated with the parent's account for financial transactions.
        /// </summary>
        public int? WalletUserId { get; init; }

        /// <summary>
        /// General emergency medical information provided by the parent.
        /// </summary>
        public string? EmergencyMedicalInfo { get; init; }

        /// <summary>
        /// The parent's medical aid provider.
        /// </summary>
        public string? MedicalAidProvider { get; init; }

        /// <summary>
        /// The plan or option registered with the medical aid provider.
        /// </summary>
        public string? MedicalAidPlan { get; init; }

        /// <summary>
        /// The parent's medical aid membership or policy number.
        /// </summary>
        public string? MedicalAidNumber { get; init; }

        /// <summary>
        /// The main member for the parent's medical aid policy.
        /// </summary>
        public string? MedicalAidMainMember { get; init; }

        /// <summary>
        /// The identification number for the medical aid main member.
        /// </summary>
        public string? MedicalAidMainMemberIdNumber { get; init; }

        /// <summary>
        /// The parent's preferred primary physician.
        /// </summary>
        public string? PrimaryPhysicianName { get; init; }

        /// <summary>
        /// Contact number for the parent's primary physician.
        /// </summary>
        public string? PrimaryPhysicianContactNumber { get; init; }

        /// <summary>
        /// Indicates if the parent's consent is required for certain events or activities involving their learners.
        /// </summary>
        public bool RequireConsent { get; init; }

        /// <summary>
        /// Indicates whether the parent should receive notifications (push, SMS, or other types).
        /// </summary>
        public bool ReceiveNotifications { get; init; }

        /// <summary>
        /// Indicates whether the parent should receive messages related to school activities, announcements, etc.
        /// </summary>
        public bool ReceiveMessages { get; init; }

        /// <summary>
        /// Indicates whether the parent should receive emails for communication from the school.
        /// </summary>
        public bool RecieveEmails { get; init; }

        /// <summary>
        /// A collection of addresses associated with the parent, e.g., home or billing address.
        /// </summary>
        public List<AddressDto>? Addresses { get; init; } = new();

        /// <summary>
        /// A collection of contact numbers (e.g., mobile, home phone) associated with the parent.
        /// </summary>
        public List<ContactNumberDto> ContactNumbers { get; init; } = new();

        /// <summary>
        /// A collection of email addresses associated with the parent.
        /// </summary>
        public List<EmailAddressDto> EmailAddresses { get; init; } = new();

        /// <summary>
        /// A collection of emergency contacts associated with the parent.
        /// </summary>
        public List<ParentEmergencyContactDto> EmergencyContacts { get; init; } = new();

        /// <summary>
        /// A collection of learners linked to this parent.
        /// </summary>
        public List<LearnerDto> Learners { get; init; } = [];

        /// <summary>
        /// A collection of parent permissions (consents) related to events that the parent may need to approve for their learners.
        /// </summary>
        public List<ParentPermissionDto> EventConsents { get; init; } = new();

        #region Entity Conversion Methods

        /// <summary>
        /// Creates a new <see cref="Parent"/> entity from the current DTO data. 
        /// This method is typically used when the parent data has just been created on the client 
        /// and needs to be transformed into a domain entity for persistence.
        /// </summary>
        /// <returns>A new <see cref="Parent"/> domain entity with fields mapped from this DTO.</returns>
        public Parent CreateParent()
        {
            return new Parent
            {
                Id = ParentId,
                FirstName = FirstName,
                MiddleName = MiddleName,
                DisplayName = DisplayName,
                LastName = LastName,
                Description = Description,
                WalletId = WalletId,
                WalletUserId = WalletUserId,
                EmergencyMedicalInfo = EmergencyMedicalInfo,
                MedicalAidProvider = MedicalAidProvider,
                MedicalAidPlan = MedicalAidPlan,
                MedicalAidNumber = MedicalAidNumber,
                MedicalAidMainMember = MedicalAidMainMember,
                MedicalAidMainMemberIdNumber = MedicalAidMainMemberIdNumber,
                PrimaryPhysicianName = PrimaryPhysicianName,
                PrimaryPhysicianContactNumber = PrimaryPhysicianContactNumber,
                ReceiveNotifications = ReceiveNotifications,
                ReceiveMessages = ReceiveMessages,
                RecieveEmails = RecieveEmails,

                Addresses = Addresses.Select(c => c.ToAddress<Parent>()).ToList(),
                ContactNumbers = ContactNumbers.Select(c => new ContactNumber<Parent>
                {
                    InternationalCode = "+27",
                    Number = c.Number,
                    Default = c.Default
                }).ToList(),
                EmailAddresses = EmailAddresses.Select(c => new EmailAddress<Parent>
                {
                    Email = c.EmailAddress,
                    Default = c.Default
                }).ToList(),
                EmergencyContacts = EmergencyContacts.Select(c => c.ToEntity()).ToList()
            };
        }

        /// <summary>
        /// Updates an existing <see cref="Parent"/> entity with fields from this DTO. 
        /// This method is commonly used when an existing Parent record in the database 
        /// needs to be updated with new data from the client.
        /// </summary>
        /// <param name="parent">The existing Parent entity to update.</param>
        /// <returns>The same Parent entity, after modifications.</returns>
        public Parent UpdateParent(Parent parent)
        {
            parent.ParentIdNumber = ParentId;
            parent.FirstName = FirstName;
            parent.MiddleName = MiddleName;
            parent.DisplayName = DisplayName;
            parent.LastName = LastName;
            parent.Description = Description;
            parent.WalletId = WalletId;
            parent.WalletUserId = WalletUserId;

            parent.EmergencyMedicalInfo = EmergencyMedicalInfo;
            parent.MedicalAidProvider = MedicalAidProvider;
            parent.MedicalAidPlan = MedicalAidPlan;
            parent.MedicalAidNumber = MedicalAidNumber;
            parent.MedicalAidMainMember = MedicalAidMainMember;
            parent.MedicalAidMainMemberIdNumber = MedicalAidMainMemberIdNumber;
            parent.PrimaryPhysicianName = PrimaryPhysicianName;
            parent.PrimaryPhysicianContactNumber = PrimaryPhysicianContactNumber;

            parent.RequireConsent = RequireConsent;
            parent.ReceiveNotifications = ReceiveNotifications;
            parent.ReceiveMessages = ReceiveMessages;
            parent.RecieveEmails = RecieveEmails;

            return parent;
        }

        #endregion

        /// <summary>
        /// Creates and returns a <see cref="UserInfoDto"/> object populated with the current user's information.
        /// </summary>
        /// <remarks>The returned <see cref="UserInfoDto"/> contains details such as the user's name,
        /// contact information,  notification preferences, and other profile-related data. If multiple email addresses
        /// or phone numbers  are available, the default one is selected; otherwise, the first available entry is
        /// used.</remarks>
        /// <returns>A <see cref="UserInfoDto"/> object containing the user's profile information.</returns>
        public UserInfoDto GetUserInfoDto()
        {
            return new UserInfoDto()
            {
                UserId = ParentId,
                CoverImageUrl = CoverImageUrl,
                FirstName = FirstName,
                MiddleName = MiddleName,
                DisplayName = DisplayName,
                LastName = LastName,
                Description = Description,
                ReceiveNotifications = ReceiveNotifications,
                ReceiveMessages = ReceiveMessages,
                ReceiveEmails = RecieveEmails,
                RequireConsent = RequireConsent,
                EmailAddress = EmailAddresses.FirstOrDefault(c => c.Default) == null ? EmailAddresses.FirstOrDefault()?.EmailAddress : EmailAddresses.FirstOrDefault(c => c.Default)?.EmailAddress,
                PhoneNr = ContactNumbers.FirstOrDefault(c => c.Default) == null ? ContactNumbers.FirstOrDefault()?.Number : ContactNumbers.FirstOrDefault(c => c.Default)?.Number
            };
        }
    }
}
