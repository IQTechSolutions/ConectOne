using System.ComponentModel.DataAnnotations;

namespace IdentityModule.Application.ViewModels
{
    /// <summary>
    /// Represents the data required to reset a user's password.
    /// </summary>
    /// <remarks>This view model is typically used in scenarios where a user needs to reset their password,
    /// such as during a password recovery process. It includes fields for the user's identifier, the new password, and
    /// a confirmation of the new password.</remarks>
    public class ResetPasswordViewModel
    {
        public string UserId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the password for the user.
        /// </summary>
        /// <remarks>The password is required and must meet the specified length constraints. It is
        /// validated using the <see cref="DataType.Password"/> data type.</remarks>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        /// <summary>
        /// Gets or sets the confirmation password entered by the user.
        /// </summary>
        /// <remarks>This property is validated to ensure it matches the <see cref="Password"/> property. 
        /// If the values do not match, an error message is displayed.</remarks>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmedPassword { get; set; } = null!;
    }
}
