using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NelspruitHigh.Blazor.ServerClient.Components.Account.Pages
{
    /// <summary>
    /// Represents a page that enables users to reset their password using a password reset code.
    /// </summary>
    /// <remarks>This class is typically used as part of an account recovery workflow. It processes password
    /// reset requests by validating the provided reset code and updating the user's password if the request is valid.
    /// If the reset code is invalid or the operation fails, appropriate error messages are displayed or the user is
    /// redirected to a confirmation or error page.</remarks>
    public partial class ResetPassword
    {
        private IEnumerable<IdentityError>? identityErrors;

        /// <summary>
        /// Gets or sets the input data submitted from the form.
        /// </summary>
        [SupplyParameterFromForm] private InputModel Input { get; set; } = default!;

        /// <summary>
        /// Gets or sets the code value supplied from the query string.
        /// </summary>
        [SupplyParameterFromQuery] private string? Code { get; set; }

        /// <summary>
        /// Gets a formatted error message that summarizes the identity errors, if any.
        /// </summary>
        private string? Message => identityErrors is null ? null : $"Error: {string.Join(", ", identityErrors.Select(error => error.Description))}";

        /// <summary>
        /// Initializes the component and processes the password reset code if present.
        /// </summary>
        /// <remarks>This method decodes the password reset code and prepares the input model for use. If
        /// the code is missing, the user is redirected to an error page. This method is typically called by the Blazor
        /// framework during component initialization and should not be called directly.</remarks>
        protected override void OnInitialized()
        {
            Input ??= new();

            if (Code is null)
            {
                RedirectManager.RedirectTo("Account/InvalidPasswordReset");
                return;
            }

            Input.Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(Code));
        }

        /// <summary>
        /// Handles the password reset form submission when the input is valid.
        /// </summary>
        /// <remarks>This method attempts to reset the user's password using the provided email, code, and
        /// new password. If the operation succeeds or the user does not exist, the user is redirected to the password
        /// reset confirmation page. If the reset fails, any resulting errors are stored for display. No indication is
        /// given as to whether the user exists, to protect user privacy.</remarks>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task OnValidSubmitAsync()
        {
            var user = await UserManager.FindByEmailAsync(Input.Email);
            if (user is null)
            {
                // Don't reveal that the user does not exist
                RedirectManager.RedirectTo("Account/ResetPasswordConfirmation");
                return;
            }

            var result = await UserManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            if (result.Succeeded)
            {
                RedirectManager.RedirectTo("Account/ResetPasswordConfirmation");
                return;
            }

            identityErrors = result.Errors;
        }

        /// <summary>
        /// Represents the input data required for a user to reset their password.
        /// </summary>
        /// <remarks>This model is typically used in password reset workflows to collect and validate user
        /// input, including the email address, new password, confirmation of the new password, and the reset code. All
        /// properties are required except for the password confirmation, which is validated against the new
        /// password.</remarks>
        private sealed class InputModel
        {
            /// <summary>
            /// Gets or sets the email address associated with the user.
            /// </summary>
            [Required, EmailAddress] public string Email { get; set; } = "";

            /// <summary>
            /// Gets or sets the password for the user account.
            /// </summary>
            /// <remarks>The password must be at least 6 and at most 100 characters in length. This
            /// property is typically used for user registration or authentication scenarios. The value is not stored in
            /// plain text and should be handled securely to protect user credentials.</remarks>
            [Required, StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6), DataType(DataType.Password)]
            public string Password { get; set; } = "";

            /// <summary>
            /// Gets or sets the confirmation password entered by the user.
            /// </summary>
            /// <remarks>This property is typically used to verify that the user has entered their
            /// intended password correctly by requiring the same value as the main password field. The value should
            /// match the value of the Password property for validation to succeed.</remarks>
            [DataType(DataType.Password), Display(Name = "Confirm password"), Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; } = "";

            /// <summary>
            /// Gets or sets the code associated with the entity.
            /// </summary>
            /// <remarks>This property is required and must be set to a non-empty value before saving
            /// the entity. The code typically serves as a unique identifier or reference within the application
            /// domain.</remarks>
            [Required] public string Code { get; set; } = "";
        }
    }
}
