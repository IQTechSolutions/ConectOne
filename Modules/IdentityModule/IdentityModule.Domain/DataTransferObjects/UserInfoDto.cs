using ConectOne.Domain.DataTransferObjects;
using ConectOne.Domain.Enums;
using FilingModule.Domain.DataTransferObjects;
using FilingModule.Domain.Enums;
using IdentityModule.Domain.Entities;
using IdentityModule.Domain.Enums;

namespace IdentityModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for user information.
    /// This class is used to transfer user-related data between layers or systems.
    /// </summary>
    public record UserInfoDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInfoDto"/> class with default values.
        /// </summary>
        public UserInfoDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInfoDto"/> class using an <see cref="ApplicationUser"/> entity.
        /// </summary>
        /// <param name="user">The <see cref="ApplicationUser"/> entity containing user details.</param>
        public UserInfoDto(ApplicationUser user)
        {
            var imageFile = user.UserInfo.Images.FirstOrDefault(c => c.Image.ImageType == UploadType.Cover);
            if (!string.IsNullOrEmpty(imageFile?.Image.RelativePath))
                CoverImageUrl = $"/{imageFile.Image.RelativePath}";

            UserId = user.Id;
            CompanyName = user.CompanyName;
            VatNr = user.UserInfo.VatNr;
            FirstName = user.UserInfo.FirstName!;
            MiddleName = user.UserInfo.OtherNames;
            DisplayName = user.UserInfo.FullName;
            LastName = user.UserInfo.LastName!;
            JobTitle = user.JobTitle;
            UserName = user.UserName!;
            Description = user.UserInfo.Bio;
            PhoneNr = user.PhoneNumber!;
            EmailAddress = user.Email!;
            EmailConfirmed = user.EmailConfirmed;
            RegistrationStatus = user.RegistrationStatus;
            ReasonForRejection = user.ReasonForRejection;
            AllocatedLicences = user.Licenses;

            if (user.UserInfo.UserAppSettings is not null)
            {
                ShowJobTitle = user.UserInfo.UserAppSettings.ShowJobTitle;
                ShowPhoneNr = user.UserInfo.UserAppSettings.ShowPhoneNr;
                ShowEmailAddress = user.UserInfo.UserAppSettings.ShowEmailAddress;
                ReceiveMessages = user.UserInfo.UserAppSettings.ReceiveMessages;
                ReceiveNotifications = user.UserInfo.UserAppSettings.ReceiveNotifications;
                ReceiveEmails = user.UserInfo.UserAppSettings.ReceiveEmails;
            }

            if (user.UserInfo.Addresses != null && user.UserInfo.Addresses.Any())
            {
                var physicalAddress = user.UserInfo.Addresses.Select(c => new AddressDto(c)).FirstOrDefault(c => c.AddressType == AddressType.Physical);
                Address = physicalAddress ?? user.UserInfo.Addresses.Select(c => new AddressDto(c)).FirstOrDefault();

                var companyAddress = user.UserInfo.Addresses.Select(c => new AddressDto(c)).FirstOrDefault(c => c.AddressType == AddressType.Billing);
                CompanyAddress = companyAddress;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInfoDto"/> class using a <see cref="UserInfo"/> entity.
        /// </summary>
        /// <param name="user">The <see cref="UserInfo"/> entity containing user details.</param>
        public UserInfoDto(UserInfo user)
        {
            var imageFile = user.Images?.FirstOrDefault(c => c.Image.ImageType == UploadType.Cover);
            CoverImageUrl = !string.IsNullOrEmpty(imageFile?.Image.RelativePath) ? $"/{imageFile.Image.RelativePath}" : "_content/FilingModule.Blazor/images/profileImage128x128.png";

            UserId = user.Id;
            FirstName = user.FirstName!;
            MiddleName = user.OtherNames;
            DisplayName = user.FullName;
            LastName = user.LastName!;
            Description = user.Bio;
            PhoneNr = user.ContactNumbers is null ? "" : user.ContactNumbers.FirstOrDefault(c => c.Default)?.Number;
            EmailAddress = user.EmailAddresses is null || !user.EmailAddresses.Any() ? "" : user.EmailAddresses.FirstOrDefault(c => c.Default) == null ? user.EmailAddresses.FirstOrDefault().Email : user.EmailAddresses.FirstOrDefault(c => c.Default).Email;

            CompanyName = user.CompanyName;
            VatNr = user.VatNr;

            if (user.UserAppSettings != null)
            {
                ShowJobTitle = user.UserAppSettings.ShowJobTitle;
                ShowPhoneNr = user.UserAppSettings.ShowPhoneNr;
                ShowEmailAddress = user.UserAppSettings.ShowEmailAddress;
                ReceiveMessages = user.UserAppSettings.ReceiveMessages;
                ReceiveNotifications = user.UserAppSettings.ReceiveNotifications;
                ReceiveEmails = user.UserAppSettings.ReceiveEmails;
            }

            if (user.Addresses is not null && user.Addresses.Any())
            {
                var physicalAddress = user.Addresses.Select(c => new AddressDto(c)).FirstOrDefault(c => c.AddressType == AddressType.Physical);
                Address = physicalAddress ?? user.Addresses.Select(c => new AddressDto(c)).FirstOrDefault();

                var companyAddress = user.Addresses.Select(c => new AddressDto(c)).FirstOrDefault(c => c.AddressType == AddressType.Billing);
                CompanyAddress = companyAddress ?? user.Addresses.Select(c => new AddressDto(c)).FirstOrDefault();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the unique identifier of the user.
        /// </summary>
        public string? UserId { get; init; }

        /// <summary>
        /// Gets the URL of the user's cover image.
        /// </summary>
        public string? CoverImageUrl { get; init; }

        /// <summary>
        /// Gets the name of the company associated with the user.
        /// </summary>
        public string? CompanyName { get; init; }

        /// <summary>
        /// Gets or sets the VAT number of the user or company.
        /// </summary>
        public string? VatNr { get; set; }

        /// <summary>
        /// Gets the first name of the user.
        /// </summary>
        public string? FirstName { get; init; } = null!;

        /// <summary>
        /// Gets the middle name of the user.
        /// </summary>
        public string? MiddleName { get; init; }

        /// <summary>
        /// Gets the display name of the user.
        /// </summary>
        public string? DisplayName { get; init; }

        /// <summary>
        /// Gets the last name of the user.
        /// </summary>
        public string? LastName { get; init; } = null!;

        /// <summary>
        /// Gets the professional title of the user.
        /// </summary>
        public string? ProfessionalTitle { get; init; }

        /// <summary>
        /// Gets the job title of the user.
        /// </summary>
        public string? JobTitle { get; init; }

        /// <summary>
        /// Gets the username of the user.
        /// </summary>
        public string UserName { get; init; } = string.Empty;

        /// <summary>
        /// Gets the number of licenses allocated to the user.
        /// </summary>
        public int AllocatedLicences { get; init; }

        /// <summary>
        /// Gets the unique URL associated with the user.
        /// </summary>
        public string? UniqueUrl { get; init; }

        /// <summary>
        /// Gets the description or bio of the user.
        /// </summary>
        public string? Description { get; init; }

        /// <summary>
        /// Gets the phone number of the user.
        /// </summary>
        public string? PhoneNr { get; init; }

        /// <summary>
        /// Gets the email address of the user.
        /// </summary>
        public string? EmailAddress { get; init; }

        /// <summary>
        /// Gets a value indicating whether the user's email is confirmed.
        /// </summary>
        public bool EmailConfirmed { get; init; }

        /// <summary>
        /// Gets a value indicating whether the user is active.
        /// </summary>
        public bool Active { get; init; }

        /// <summary>
        /// Gets the registration status of the user.
        /// </summary>
        public RegistrationStatus RegistrationStatus { get; init; }

        /// <summary>
        /// Gets the reason for rejection, if the user's registration was rejected.
        /// </summary>
        public string? ReasonForRejection { get; init; }

        /// <summary>
        /// Gets a value indicating whether the user's job title should be displayed.
        /// </summary>
        public bool ShowJobTitle { get; init; } = true;

        /// <summary>
        /// Gets a value indicating whether the user's phone number should be displayed.
        /// </summary>
        public bool ShowPhoneNr { get; init; } = true;

        /// <summary>
        /// Gets a value indicating whether the user's email address should be displayed.
        /// </summary>
        public bool ShowEmailAddress { get; init; } = true;

        /// <summary>
        /// Gets a value indicating whether the user should receive notifications.
        /// </summary>
        public bool ReceiveNotifications { get; init; } = true;

        /// <summary>
        /// Gets a value indicating whether the user should receive messages.
        /// </summary>
        public bool ReceiveMessages { get; init; } = true;

        /// <summary>
        /// Gets a value indicating whether the user should receive emails.
        /// </summary>
        public bool ReceiveEmails { get; init; } = true;

        /// <summary>
        /// Gets a value indicating whether the user requires consent for certain actions.
        /// </summary>
        public bool RequireConsent { get; init; } = true;

        /// <summary>
        /// Gets the physical address of the user.
        /// </summary>
        public AddressDto? Address { get; init; }

        /// <summary>
        /// Gets the company address of the user.
        /// </summary>
        public AddressDto? CompanyAddress { get; init; }

        /// <summary>
        /// Gets or sets the collection of images associated with the entity.
        /// </summary>
        public ICollection<ImageDto> Images { get; set; } = [];

        #endregion
    }
}
