using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Progolf.Blazor.ServerClient.Components.Account.Pages
{
    /// <summary>
    /// Represents the login page component responsible for handling user authentication via password or passkey
    /// methods.
    /// </summary>
    /// <remarks>This component manages the login workflow, including form validation, credential processing,
    /// and redirecting users based on authentication outcomes. It supports both traditional password-based sign-in and
    /// passkey authentication, and integrates with external authentication schemes when necessary. The component also
    /// handles scenarios such as two-factor authentication requirements and account lockouts, providing appropriate
    /// user feedback and navigation.</remarks>
    public partial class Login
    {
        private string? errorMessage;
        private EditContext editContext = default!;

        /// <summary>
        /// Gets or sets the current HTTP context for the component.
        /// </summary>
        /// <remarks>This property is typically provided as a cascading parameter in Blazor components to
        /// access information about the current HTTP request, such as user identity, request headers, and session data.
        /// The availability and contents of the HTTP context may vary depending on the hosting environment and
        /// middleware configuration.</remarks>
        [CascadingParameter] private HttpContext HttpContext { get; set; } = default!;

        /// <summary>
        /// Gets or sets the input model containing form data supplied by the user.
        /// </summary>
        [SupplyParameterFromForm] private InputModel Input { get; set; } = default!;

        /// <summary>
        /// Gets or sets the URL to which the user should be redirected after completing the current operation.
        /// </summary>
        /// <remarks>If not specified, the default redirect behavior will be used. This property is
        /// typically set to preserve navigation context or to return the user to their previous location.</remarks>
        [SupplyParameterFromQuery] private string? ReturnUrl { get; set; }

        /// <summary>
        /// Asynchronously initializes the component and prepares the input model and edit context for user interaction.
        /// </summary>
        /// <remarks>If the current HTTP request uses the GET method, any existing external authentication
        /// cookie is cleared to ensure a clean login process. This method is typically called by the Blazor framework
        /// during component initialization.</remarks>
        /// <returns>A task that represents the asynchronous initialization operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            Input ??= new();

            editContext = new EditContext(Input);

            if (HttpMethods.IsGet(HttpContext.Request.Method))
            {
                // Clear the existing external cookie to ensure a clean login process
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            }
        }

        /// <summary>
        /// Attempts to sign in the user using either passkey or password authentication, and redirects based on the
        /// result.
        /// </summary>
        /// <remarks>If passkey credentials are provided, passkey authentication is attempted without form
        /// validation. Otherwise, password authentication is performed after validating the input form. The method
        /// handles two-factor authentication and account lockout scenarios by redirecting the user to the appropriate
        /// pages. If authentication fails, an error message is set.</remarks>
        /// <returns>A task that represents the asynchronous login operation.</returns>
        public async Task LoginUser()
        {
            if (!string.IsNullOrEmpty(Input.Passkey?.Error))
            {
                errorMessage = $"Error: {Input.Passkey.Error}";
                return;
            }

            SignInResult result;
            if (!string.IsNullOrEmpty(Input.Passkey?.CredentialJson))
            {
                // When performing passkey sign-in, don't perform form validation.
                result = await SignInManager.PasskeySignInAsync(Input.Passkey.CredentialJson);
            }
            else
            {
                // If doing a password sign-in, validate the form.
                if (!editContext.Validate())
                {
                    return;
                }

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                result = await SignInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
            }

            if (result.Succeeded)
            {
                Logger.LogInformation("User logged in.");
                RedirectManager.RedirectTo(ReturnUrl);
            }
            else if (result.RequiresTwoFactor)
            {
                RedirectManager.RedirectTo(
                    "Account/LoginWith2fa",
                    new() { ["returnUrl"] = ReturnUrl, ["rememberMe"] = Input.RememberMe });
            }
            else if (result.IsLockedOut)
            {
                Logger.LogWarning("User account locked out.");
                RedirectManager.RedirectTo("Account/Lockout");
            }
            else
            {
                errorMessage = "Error: Invalid login attempt.";
            }
        }

        /// <summary>
        /// Represents the input data required for a user sign-in operation, including credentials and optional
        /// authentication options.
        /// </summary>
        /// <remarks>This model is typically used to bind user input from a login form. It includes
        /// properties for email, password, a persistent login option, and optional passkey-based authentication.
        /// Validation attributes are applied to ensure required fields and proper formatting. The class is intended for
        /// use in authentication workflows and should not be instantiated directly outside of such contexts.</remarks>
        private sealed class InputModel
        {
            /// <summary>
            /// Gets or sets the email address associated with the user.
            /// </summary>
            /// <remarks>The value must be a valid email address format and cannot be null or empty.
            /// This property is typically required for user identification and communication purposes.</remarks>
            [Required, EmailAddress] public string Email { get; set; } = "";

            /// <summary>
            /// Gets or sets the password associated with the user or account.
            /// </summary>
            /// <remarks>The password value is required and should be provided in a secure manner.
            /// When setting this property, ensure that sensitive data is handled according to security best practices,
            /// such as using secure input controls and avoiding logging or exposing the password value.</remarks>
            [Required, DataType(DataType.Password)] public string Password { get; set; } = "";

            /// <summary>
            /// Gets or sets a value indicating whether the user should remain signed in after closing the browser.
            /// </summary>
            [Display(Name = "Remember me?")] public bool RememberMe { get; set; }

            /// <summary>
            /// Gets or sets the passkey input model used for authentication or verification purposes.
            /// </summary>
            public PasskeyInputModel? Passkey { get; set; }
        }
    }
}
