using System.ComponentModel.DataAnnotations;
using ConectOne.Domain.DataTransferObjects;
using FilingModule.Domain.Enums;
using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Entities;
using IdentityModule.Domain.Enums;

namespace IdentityModule.Application.ViewModels
{
    /// <summary>
    /// Represents a user profile view model in the Identity subsystem.
    /// Provides a strongly typed way for UI components and services 
    /// to create, display, and edit user details such as name, 
    /// contact info, company information, and social media settings.
    /// </summary>
    public class UserInfoViewModel
    {
        private string _displayName = null!;

        #region Constructors

        /// <summary>
        /// Parameterless constructor, typically used for serialization 
        /// or when you intend to set properties manually after construction.
        /// </summary>
        public UserInfoViewModel() { }

        /// <summary>
        /// Constructs a <see cref="UserInfoViewModel"/> from a <see cref="UserInfoDto"/>. 
        /// This is often used to map server-side data (DTO) 
        /// to a view model suitable for data binding on the client/UI.
        /// </summary>
        /// <param name="userInfo">
        /// A user information Data Transfer Object containing all relevant data 
        /// for the user (personal details, contact info, etc.).
        /// </param>
        public UserInfoViewModel(UserInfoDto userInfo)
        {
            UserId         = userInfo.UserId;
            CoverImageUrl  = userInfo.CoverImageUrl;
            FirstName      = userInfo.FirstName;
            MiddleName     = userInfo.MiddleName;
            DisplayName    = userInfo.DisplayName;
            LastName       = userInfo.LastName;
            JobTitle       = userInfo.JobTitle;
            UserName       = userInfo.UserName;
            Description    = userInfo.Description;
            PhoneNr        = userInfo.PhoneNr;
            EmailAddress   = userInfo.EmailAddress;
            EmailConfirmed = userInfo.EmailConfirmed;
            Active         = userInfo.Active;
            RegistrationStatus  = userInfo.RegistrationStatus;
            ReasonForRejection  = userInfo.ReasonForRejection;
            AllocatedLisences   = userInfo.AllocatedLicences;

            ShowJobTitle         = userInfo.ShowJobTitle;
            ShowPhoneNr          = userInfo.ShowPhoneNr;
            ShowEmailAddress     = userInfo.ShowEmailAddress;
            ReceiveMessages      = userInfo.ReceiveMessages;
            ReceiveNotifications = userInfo.ReceiveNotifications;
            RecieveEmails        = userInfo.ReceiveEmails;
            RequireConsent       = userInfo.RequireConsent;

            CompanyName = userInfo.CompanyName ?? string.Empty;  
            VatNr       = userInfo.VatNr;

            Address = userInfo.Address is not null ? userInfo.Address : new AddressDto();

            CompanyAddress = userInfo.CompanyAddress is not null ? userInfo.CompanyAddress : new AddressDto();
        }

        /// <summary>
        /// Constructs a <see cref="UserInfoViewModel"/> by mapping data 
        /// from an <see cref="ApplicationUser"/> domain entity and its related roles. 
        /// This overload is typically used in server-side Blazor 
        /// when constructing user details directly from EF Core entities.
        /// </summary>
        /// <param name="userInfo">
        /// The user entity from which to map data (includes personal details, 
        /// phone, email, etc.).
        /// </param>
        /// <param name="roles">
        /// A collection of role names assigned to this user (e.g., "Admin", "User").
        /// </param>
        public UserInfoViewModel(ApplicationUser userInfo, ICollection<string> roles)
        {
            // Basic identity fields
            UserId         = userInfo.Id;
            CoverImageUrl  = userInfo.UserInfo.Images.FirstOrDefault(c => c.Image.ImageType == UploadType.Cover)?.Image.RelativePath;

            CompanyName    = userInfo.CompanyName ?? string.Empty;
            FirstName      = userInfo.UserInfo.FirstName!;
            MiddleName     = userInfo.UserInfo.OtherNames;
            LastName       = userInfo.UserInfo.LastName!;
            JobTitle       = userInfo.JobTitle;
            UserName       = userInfo.UserName!;
            Description    = userInfo.UserInfo.Bio;
            PhoneNr        = userInfo.PhoneNumber!;
            EmailAddress   = userInfo.Email!;
            EmailConfirmed = userInfo.EmailConfirmed;
            RegistrationStatus = userInfo.RegistrationStatus;
            ReasonForRejection  = userInfo.ReasonForRejection;

            // UI-related flags (from user app settings)
            ShowJobTitle         = userInfo.UserInfo.UserAppSettings!.ShowJobTitle;
            ShowPhoneNr          = userInfo.UserInfo.UserAppSettings.ShowPhoneNr;
            ShowEmailAddress     = userInfo.UserInfo.UserAppSettings.ShowEmailAddress;
            ReceiveMessages      = userInfo.UserInfo.UserAppSettings.ReceiveMessages;
            ReceiveNotifications = userInfo.UserInfo.UserAppSettings.ReceiveNotifications;
            RecieveEmails        = userInfo.UserInfo.UserAppSettings.ReceiveEmails;

            // Roles (e.g., user might be in multiple roles)
            UserRoles = roles;
        }

        #endregion

        /// <summary>
        /// The primary key identifier for this user 
        /// (used to correlate with Identity or EFCore).
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// A cover image or profile image path/URL for the user.
        /// Typically used on profile pages or dashboards.
        /// </summary>
        public string? CoverImageUrl { get; set; }

        /// <summary>
        /// User's first name. (Required for identification purposes.)
        /// </summary>
        [Display(Name = "First Name"), MaxLength(255, ErrorMessage = "The maximum length for first name is 255 character")]
        public string FirstName { get; set; } = null!;

        /// <summary>
        /// User's middle name or any additional names 
        /// beyond their primary first name.
        /// </summary>
        [Display(Name = "Middle Name")]
        public string? MiddleName { get; set; }

        /// <summary>
        /// A short name used to display the user in UI. 
        /// If empty, defaults to "FirstName LastName".
        /// </summary>
        [Display(Name = "Display Name")]
        public string? DisplayName
        {
            get => string.IsNullOrEmpty(_displayName) ? $"{FirstName} {LastName}" : _displayName;
            set => _displayName = value!;
        }

        /// <summary>
        /// User's last name or surname. (Required.)
        /// </summary>
        [Display(Name = "Last Name"), MaxLength(255, ErrorMessage = "The maximum length for last name is 255 character")]
        public string LastName { get; set; } = null!;

        /// <summary>
        /// The company that this user is associated with (if any).
        /// Could be used to display or store an organization's name.
        /// </summary>
        [Display(Name = "Company Name"), MaxLength(255, ErrorMessage = "The maximum length for company name is 255 character")]
        public string CompanyName { get; set; } = null!;

        /// <summary>
        /// A VAT (Value-Added Tax) number for the user's company, if relevant.
        /// Also ensures compliance with certain business requirements.
        /// </summary>
        [Display(Name = "VAT Nr"), MaxLength(255, ErrorMessage = "The maximum length for company name is 255 character")]
        public string? VatNr { get; set; }

        /// <summary>
        /// The user's job title or position within a company (if applicable).
        /// </summary>
        [Display(Name = "Job Title")]
        public string? JobTitle { get; set; }

        /// <summary>
        /// A boolean indicating whether the user chooses to show their job title publicly or not.
        /// </summary>
        public bool ShowJobTitle { get; set; } = true;

        /// <summary>
        /// The Identity username (often an email, but can be unique user strings as well).
        /// </summary>
        [Display(Name = "User Name")]
        public string UserName { get; set; } = null!;

        /// <summary>
        /// A short biography or personal description. 
        /// Could be used for "About Me" sections in the UI.
        /// </summary>
        [Display(Name = "About Me"), DataType(DataType.MultilineText)]
        [MaxLength(5000, ErrorMessage = "The maximum length for the description is 5000 character")]
        public string? Description { get; set; }

        /// <summary>
        /// The user's phone number in a text format. 
        /// Could be validated or normalized externally.
        /// </summary>
        public string PhoneNr { get; set; } = null!;

        /// <summary>
        /// Indicates if the user consents to showing their phone number in public or shared UI contexts.
        /// </summary>
        public bool ShowPhoneNr { get; set; } = true;

        /// <summary>
        /// The user's primary email address. 
        /// Often also used as the login username in many identity systems.
        /// </summary>
        [EmailAddress] public string EmailAddress { get; set; } = null!;

        /// <summary>
        /// Indicates if the user allows their email address to be displayed 
        /// (e.g., on a public-facing profile page).
        /// </summary>
        public bool ShowEmailAddress { get; set; } = true;

        /// <summary>
        /// The count of licenses or seats allocated to this user, if applicable in a licensing system.
        /// </summary>
        public int AllocatedLisences { get; set; }

        /// <summary>
        /// Whether the user wants to receive push or in-app notifications.
        /// </summary>
        public bool ReceiveNotifications { get; set; } = true;

        public bool RequireConsent { get; set; } = true;

        /// <summary>
        /// Whether the user wants to receive internal system messages (inbox, chat, etc.).
        /// </summary>
        public bool ReceiveMessages { get; set; } = true;

        /// <summary>
        /// Whether the user wants to receive emails from the system 
        /// (e.g., marketing, transaction notifications, updates).
        /// </summary>
        public bool RecieveEmails { get; set; } = true;

        /// <summary>
        /// Indicates if the user has confirmed their email address. 
        /// Typically set by Identity flows upon user verification.
        /// </summary>
        public bool EmailConfirmed { get; set; }

        /// <summary>
        /// Represents if the user is active and allowed to log in, or 
        /// if the system has deactivated them for some reason.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Indicates if the user is online in real-time context (not always used).
        /// Could be used in chat or presence features.
        /// </summary>
        public bool IsOnline { get; set; }

        /// <summary>
        /// Status indicating if the user has fully completed registration, 
        /// is pending approval, or is locked out, etc.
        /// </summary>
        public RegistrationStatus RegistrationStatus { get; set; }

        /// <summary>
        /// If the user was rejected (e.g., on a moderated sign-up process),
        /// this string captures the reason for that rejection (displayed in admin UI, etc.).
        /// </summary>
        public string? ReasonForRejection { get; set; }

        /// <summary>
        /// The user's residential or primary address data (street, city, state, etc.).
        /// </summary>
        public AddressDto? Address { get; set; }

        /// <summary>
        /// A separate address used for the company if the user 
        /// is tied to an organization or has a different business address.
        /// </summary>
        public AddressDto? CompanyAddress { get; set; }

        /// <summary>
        /// A list of roles that this user belongs to, 
        /// e.g., "Admin", "User", "SuperUser", for system permissions.
        /// </summary>
        public ICollection<string> UserRoles { get; set; } = new List<string>();

        #region Method

        /// <summary>
        /// Converts the current user information into a <see cref="UserInfoDto"/> object.
        /// </summary>
        /// <remarks>This method maps the properties of the current user entity to a new <see
        /// cref="UserInfoDto"/> instance. Nested objects, such as <see cref="Address"/> and <see
        /// cref="CompanyAddress"/>, are also converted to their respective DTOs.</remarks>
        /// <returns>A <see cref="UserInfoDto"/> object containing the mapped data from the current user entity.</returns>
        public UserInfoDto ToDto()
        {
            return new UserInfoDto()
            {
                CoverImageUrl = this.CoverImageUrl,
                UserId = this.UserId,
                FirstName = this.FirstName,
                MiddleName = this.MiddleName,
                DisplayName = this.DisplayName,
                LastName = this.LastName,
                JobTitle = this.JobTitle,
                UserName = this.UserName,
                Description = this.Description,
                PhoneNr = this.PhoneNr,
                EmailAddress = this.EmailAddress,
                EmailConfirmed = this.EmailConfirmed,
                Active = this.Active,
                RegistrationStatus = this.RegistrationStatus,
                ReasonForRejection = this.ReasonForRejection,
                AllocatedLicences = this.AllocatedLisences,

                ShowJobTitle = this.ShowJobTitle,
                ShowPhoneNr = this.ShowPhoneNr,
                ShowEmailAddress = this.ShowEmailAddress,
                ReceiveMessages = this.ReceiveMessages,
                ReceiveNotifications = this.ReceiveNotifications,
                ReceiveEmails = this.RecieveEmails,

                CompanyName = this.CompanyName,
                VatNr = this.VatNr,

                Address = this.Address != null ? this.Address : new AddressDto(),
                CompanyAddress = this.CompanyAddress != null ? this.CompanyAddress : new AddressDto()
        };
        }

        #endregion
    }
}
