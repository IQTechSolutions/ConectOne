using ConectOne.Domain.Entities;
using ConectOne.Domain.Enums;
using FilingModule.Domain.Entities;

namespace IdentityModule.Domain.Entities
{
    /// <summary>
    /// Represents a user's profile information, including personal details, contact information, preferences, and system settings.
    /// Inherits from <see cref="ImageFileCollection{TEntity, TKey}"/> to support associated image management.
    /// </summary>
    public class UserInfo : FileCollection<UserInfo, string>
    {
        #region Properties

        #region Personal Details

        /// <summary>
        /// A unique URL slug for identifying the user, typically used for profile links.
        /// </summary>
        public string? UniqueUrl { get; set; }

        /// <summary>
        /// The user's title (e.g., Mr, Ms, Dr).
        /// </summary>
        public Title Title { get; set; } = Title.Me;

        /// <summary>
        /// First name of the user.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Middle or other names of the user.
        /// </summary>
        public string? OtherNames { get; set; }

        /// <summary>
        /// Last name (surname) of the user.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// The user's full name, composed of first, other, and last names.
        /// </summary>
        public string FullName => $"{FirstName} {OtherNames} {LastName}";

        /// <summary>
        /// Identity or national registration number.
        /// </summary>
        public string? IdentityNr { get; set; }

        /// <summary>
        /// Gender of the user.
        /// </summary>
        public Gender Gender { get; set; } = Gender.Unknown;

        /// <summary>
        /// Marital status of the user.
        /// </summary>
        public MaritalStatus MaritalStatus { get; set; } = MaritalStatus.Single;

        /// <summary>
        /// User's current mood status, for personal or social context display.
        /// </summary>
        public string? MoodStatus { get; set; }

        /// <summary>
        /// A short biography or self-description provided by the user.
        /// </summary>
        public string? Bio { get; set; }

        /// <summary>
        /// Name of the user's affiliated company (if applicable).
        /// </summary>
        public string? CompanyName { get; set; } = null!;

        /// <summary>
        /// Value-added tax registration number for business purposes.
        /// </summary>
        public string? VatNr { get; set; } = null!;

        #endregion

        /// <summary>
        /// Application-specific settings associated with this user.
        /// </summary>
        public UserAppSettings? UserAppSettings { get; set; }

        #endregion

        #region Collections

        /// <summary>
        /// List of contact numbers associated with the user.
        /// </summary>
        public virtual List<ContactNumber<UserInfo>> ContactNumbers { get; set; } = [];

        /// <summary>
        /// List of email addresses associated with the user.
        /// </summary>
        public virtual List<EmailAddress<UserInfo>> EmailAddresses { get; set; } = [];

        /// <summary>
        /// List of physical or mailing addresses linked to the user.
        /// </summary>
        public virtual List<Address<UserInfo>> Addresses { get; set; } = [];

        #endregion

        #region Overrides

        /// <summary>
        /// Returns a string representation of the user entity.
        /// </summary>
        public override string ToString()
        {
            return $"User Info";
        }

        #endregion
    }
}
