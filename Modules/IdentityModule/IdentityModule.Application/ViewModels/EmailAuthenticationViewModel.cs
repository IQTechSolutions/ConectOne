using System.ComponentModel.DataAnnotations;
using IdentityModule.Domain.Enums;
using IdentityModule.Domain.RequestFeatures;

namespace IdentityModule.Application.ViewModels
{
    /// <summary>
    /// Represents the data required for email-based authentication.
    /// This view model is used to capture and validate user input during the login process.
    /// </summary>
    public class EmailAuthenticationViewModel
    {
        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        [EmailAddress, Required(ErrorMessage = "User name is required")] public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password for the user account.
        /// </summary>
        [Required(ErrorMessage = "Password name is required"), DataType(DataType.Password)] public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user wants to remain logged in.
        /// </summary>
        [Display(Name = "Remember me?")] public bool RememberMe { get; set; }

        /// <summary>
        /// Converts the current user credentials into an <see cref="AuthRequest"/> object.
        /// </summary>
        /// <remarks>The returned <see cref="AuthRequest"/> object is configured to use email-based
        /// authentication.</remarks>
        /// <returns>An <see cref="AuthRequest"/> object containing the user's email, password, and authentication type.</returns>
        public AuthRequest ToAuthRequest()
        {
            return new AuthRequest
            {
                UserName = this.Email,
                Password = this.Password,
                AuthenticationType = AuthenticationType.Email
            };
        }
    }
}