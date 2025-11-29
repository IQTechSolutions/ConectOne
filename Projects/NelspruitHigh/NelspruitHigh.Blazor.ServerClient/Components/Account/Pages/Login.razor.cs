using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace NelspruitHigh.Blazor.ServerClient.Components.Account.Pages
{
    /// <summary>
    /// Provides functionality for handling user login, including authentication via password or passkey, and managing
    /// redirection after successful or failed sign-in attempts.
    /// </summary>
    /// <remarks>The Login component coordinates user authentication workflows, supporting both traditional
    /// password-based and passkey-based sign-in methods. It manages form input, validation, and user feedback, and
    /// interacts with the HTTP context and redirection logic to ensure a secure and user-friendly login experience.
    /// This class is typically used as part of an authentication flow in web applications that require user
    /// sign-in.</remarks>
    public partial class Login
    {
        private string? errorMessage;
        private EditContext editContext = default!;

        /// <summary>
        /// Gets or sets the current HTTP context for the associated request.
        /// </summary>
        /// <remarks>This property provides access to HTTP-specific information about the current request,
        /// such as user identity, request headers, and response details. It is typically supplied by the framework and
        /// should not be set manually in most scenarios.</remarks>
        [CascadingParameter] private HttpContext HttpContext { get; set; } = default!;

        /// <summary>
        /// Gets or sets the input data submitted from the form.
        /// </summary>
        [SupplyParameterFromForm] private InputModel Input { get; set; } = default!;

        /// <summary>
        /// Gets or sets the URL to which the user is redirected after the current operation completes.
        /// </summary>
        /// <remarks>This property is typically used to preserve the user's intended destination when
        /// authentication or other intermediate steps are required. If not specified, the default redirect behavior
        /// applies.</remarks>
        [SupplyParameterFromQuery] private string? ReturnUrl { get; set; }

        /// <summary>
        /// Asynchronously initializes the component and prepares the input model and edit context for user interaction.
        /// </summary>
        /// <remarks>If the current HTTP request uses the GET method, any existing external authentication
        /// cookies are cleared to ensure a clean login process.</remarks>
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
        /// Attempts to sign in a user using either passkey or password authentication, and redirects the user based on
        /// the outcome.
        /// </summary>
        /// <remarks>If passkey credentials are provided, passkey authentication is attempted; otherwise,
        /// password authentication is used. The method redirects the user upon successful login, two-factor
        /// authentication requirement, or account lockout. If authentication fails, an error message is set. This
        /// method does not perform form validation for passkey sign-in, but does validate the form for password
        /// sign-in.</remarks>
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
        /// Represents the input data required for user authentication, including credentials and optional passkey
        /// information.
        /// </summary>
        /// <remarks>This model is typically used to capture user input during a login process. It
        /// includes properties for email, password, a 'remember me' option, and an optional passkey for multi-factor
        /// authentication scenarios.</remarks>
        private sealed class InputModel
        {
            /// <summary>
            /// Gets or sets the email address associated with the user.
            /// </summary>
            [Required, EmailAddress] public string Email { get; set; } = "";

            /// <summary>
            /// Gets or sets the password associated with the user or account.
            /// </summary>
            [Required, DataType(DataType.Password)] public string Password { get; set; } = "";

            /// <summary>
            /// Gets or sets a value indicating whether the user should remain signed in after closing the browser.
            /// </summary>
            [Display(Name = "Remember me?")] public bool RememberMe { get; set; }

            /// <summary>
            /// Gets or sets the passkey input model containing user-provided passkey information.
            /// </summary>
            public PasskeyInputModel? Passkey { get; set; }
        }
    }
}
