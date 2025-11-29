using System.ComponentModel.DataAnnotations;
using ConectOne.Domain.Entities;
using ConectOne.Domain.Interfaces;
using IdentityModule.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace IdentityModule.Domain.Entities
{
    /// <summary>
    /// Represents a user within the Identity system, extending <see cref="IdentityUser"/> 
    /// with additional fields such as company details, licensing info, refresh tokens, 
    /// and auditable entity data (e.g., CreatedOn, CreatedBy).
    /// </summary>
    public class ApplicationUser : IdentityUser<string>, IAuditableEntity<string>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationUser"/> class 
        /// with default values. Primarily used by EF Core or when manually constructing 
        /// an object and setting properties afterward.
        /// </summary>
        public ApplicationUser() { }

        /// <summary>
        /// Initializes a new instance of <see cref="ApplicationUser"/> with required fields.
        /// If provided, constructor sets up basic user data such as phone number, email, 
        /// and a <see cref="UserInfo"/> object that includes contact info.
        /// </summary>
        /// <param name="id">The unique string ID (GUID) for this user.</param>
        /// <param name="username">The username for this user.</param>
        /// <param name="firstName">The user's first name.</param>
        /// <param name="lastName">The user's last name.</param>
        /// <param name="phoneNr">A primary phone number for the user.</param>
        /// <param name="email">The user's email address.</param>
        /// <param name="companyName">The user's company name or organization.</param>
        /// <param name="uniqueUrl">
        /// An optional unique URL that might represent a user-specific profile or site address.
        /// </param>
        public ApplicationUser(string id, string username, string firstName, string lastName, string phoneNr, string email, string companyName, string uniqueUrl = "")
        {
            Id = id;
            UserName = username;
            PhoneNumber = phoneNr;
            Email = email;
            CompanyName = companyName;

            // Create and configure the UserInfo sub-object
            UserInfo = new UserInfo
            {
                UniqueUrl = uniqueUrl,
                FirstName = firstName,
                LastName = lastName
            };

            // Populate the contact lists with initial default phone number/email
            UserInfo.ContactNumbers.Add(new ContactNumber<UserInfo>
            {
                InternationalCode = "+27",
                Number = phoneNr,
                Default = true
            });

            UserInfo.EmailAddresses.Add(new EmailAddress<UserInfo>
            {
                Email = email
            });
        }

        #endregion

        /// <summary>
        /// Holds additional user information beyond standard ASP.NET Core Identity fields, 
        /// such as names, addresses, social details, etc.
        /// </summary>
        public UserInfo UserInfo { get; set; } = new UserInfo();

        /// <summary>
        /// A unique URL that may represent a user-specific profile or site address.
        /// </summary>
        public DateTime? PrivacyAndUsageTermsAcceptedTimeStamp { get; set; }

        #region Company Details

        /// <summary>
        /// The job title or position of the user within a company, if applicable.
        /// </summary>
        public string? JobTitle { get; set; }

        /// <summary>
        /// The name of the company or organization to which the user belongs.
        /// </summary>
        public string? CompanyName { get; set; } = "";

        #endregion

        #region License Information

        /// <summary>
        /// The total count of licenses allocated or available to this user. 
        /// Defaults to 10000 for demonstration or testing.
        /// </summary>
        public int Licenses { get; set; } = 10000;

        #endregion

        #region Tokens

        /// <summary>
        /// Indicates whether this user is currently connected to the system 
        /// (e.g., for real-time updates, chat connections, or presence info).
        /// </summary>
        public bool IsConnected { get; set; }

        /// <summary>
        /// An optional security refresh token used for certain authentication/authorization flows.
        /// </summary>
        public string? RefreshToken { get; set; }

        /// <summary>
        /// DateTime indicating when the <see cref="RefreshToken"/> expires and 
        /// is no longer valid for obtaining new access tokens.
        /// </summary>
        public DateTime RefreshTokenExpiryTime { get; set; }

        #endregion

        #region Registration Status

        /// <summary>
        /// Indicates if the user is currently active within the system 
        /// (e.g., not disabled or locked).
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Represents the current registration status of the user 
        /// (e.g., Pending, Accepted, Rejected, etc.).
        /// </summary>
        public RegistrationStatus RegistrationStatus { get; set; } = RegistrationStatus.Pending;

        /// <summary>
        /// If <see cref="RegistrationStatus"/> is set to Rejected, 
        /// this contains a textual explanation for the decision.
        /// </summary>
        public string? ReasonForRejection { get; set; }

        #endregion

        #region IAuditableEntity Members

        /// <summary>
        /// The ID of the user or system that created this record.
        /// </summary>
        public string? CreatedBy { get; set; } = string.Empty;

        /// <summary>
        /// The timestamp when this record was created.
        /// </summary>
        public DateTime? CreatedOn { get; set; }

        /// <summary>
        /// The ID of the user or system that last modified this record.
        /// </summary>
        public string? LastModifiedBy { get; set; }

        /// <summary>
        /// The timestamp when this record was last modified.
        /// </summary>
        public DateTime? LastModifiedOn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the role has been soft-deleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the role was soft-deleted.
        /// </summary>
        public DateTime? DeletedOn { get; set; }

        /// <summary>
        /// Gets or sets the row version for concurrency checks.
        /// </summary>
        [Timestamp] public byte[] RowVersion { get; set; } = null!;

        #endregion

        #region Navigation Properties (Related Data)

        /// <summary>
        /// A collection of <see cref="Follower"/> entities indicating 
        /// who follows this user in a social or organizational sense.
        /// </summary>
        public virtual ICollection<Follower> Followers { get; set; } = new HashSet<Follower>();

        #endregion

        /// <summary>
        /// Returns a textual representation of the user object. 
        /// Primarily for debugging or logging purposes.
        /// </summary>
        public override string ToString() => "User";
    }

    /// <summary>
    /// Provides extension methods for the <see cref="ApplicationUser"/> class, 
    /// including logic to determine if user login is approved based on a 
    /// configuration setting.
    /// </summary>
    public static class ApplicationUserExtensions
    {
        /// <summary>
        /// Determines whether the user's login is approved by checking a configuration 
        /// value named "AproveRegistrations" and ensuring <see cref="RegistrationStatus"/> 
        /// is set to <see cref="RegistrationStatus.Accepted"/>.
        /// </summary>
        /// <param name="user">The <see cref="ApplicationUser"/> being evaluated.</param>
        /// <param name="configuration">
        /// The application's <see cref="IConfiguration"/> instance, used to read 
        /// "AproveRegistrations" setting.
        /// </param>
        /// <returns>
        /// <c>true</c> if the user is allowed to log in (either the setting doesn't exist, 
        /// is set to <c>false</c>, or the user is accepted). Otherwise, <c>false</c>.
        /// </returns>
        public static bool UserLoginAproved(this ApplicationUser user, IConfiguration configuration)
        {
            // Retrieve the 'AproveRegistrations' config setting
            var approveSection = configuration.GetSection("AproveRegistrations");
            if (!approveSection.Exists()) return true;

            // Convert the config value to a bool to see if user registrations must be approved
            var approveUserRegistrations = !string.IsNullOrEmpty(approveSection.Value) && bool.Parse(approveSection.Value);
            if (!approveUserRegistrations) return true;

            // If approval is required, the user must have RegistrationStatus==Accepted
            return user.RegistrationStatus == RegistrationStatus.Accepted;
        }
    }
}