using System.ComponentModel.DataAnnotations;

namespace IdentityModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents a request to change a user's password.
    /// </summary>
    public class ChangePasswordRequest
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangePasswordRequest"/> class with default values.
        /// </summary>
        public ChangePasswordRequest() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangePasswordRequest"/> class with specified user ID, current password, and new password.
        /// </summary>
        /// <param name="userId">The unique identifier of the user requesting the password change.</param>
        /// <param name="currentPassword">The user's current password.</param>
        /// <param name="newPassword">The new password to be set for the user.</param>
        public ChangePasswordRequest(string userId, string currentPassword, string newPassword)
        {
            UserId = userId;
            CurrentPassword = currentPassword;
            NewPassword = newPassword;
        }

        #endregion

        /// <summary>
        /// Gets or sets the unique identifier of the user requesting the password change.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the user's current password.
        /// </summary>
        [Required] public string CurrentPassword { get; set; }

        /// <summary>
        /// Gets or sets the new password to be set for the user.
        /// </summary>
        [Required] public string NewPassword { get; set; }
    }
}