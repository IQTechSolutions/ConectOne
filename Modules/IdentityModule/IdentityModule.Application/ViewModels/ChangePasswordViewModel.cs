using System.ComponentModel.DataAnnotations;

namespace IdentityModule.Application.ViewModels
{
    /// <summary>
    /// Represents the data required to change a user's password, including the current password, the new password, and
    /// a confirmation of the new password.
    /// </summary>
    /// <remarks>This view model is typically used in scenarios where a user is updating their password. All
    /// properties are required and should be validated before processing the password change.</remarks>
    public class ChangePasswordViewModel
    {
        /// <summary>
        /// Gets or sets the current password of the user.
        /// </summary>
        [Required] public string CurrentPassword { get; set; }

        /// <summary>
        /// Gets or sets the new password to be set for the user.
        /// </summary>
        /// <remarks>Ensure the password meets the application's security requirements, such as minimum
        /// length and complexity rules, before assigning it to this property.</remarks>
        [Required] public string NewPassword { get; set; }

        /// <summary>
        /// Gets or sets the confirmation for the new password.  This value must match the new password to ensure
        /// consistency.
        /// </summary>
        /// <remarks>This property is required and should be validated to ensure it matches the new
        /// password.</remarks>
        [Required] public string ConfirmNewPassword { get; set; }
    }
}