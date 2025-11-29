using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;

namespace NelspruitHigh.Blazor.ServerClient.Components.Account.Pages
{
    /// <summary>
    /// Represents the page model for the 'Forgot Password' workflow, allowing users to request a password reset link by
    /// providing their email address.
    /// </summary>
    /// <remarks>This class is typically used in authentication flows to initiate the password reset process.
    /// It handles user input, validates the email address, and sends a password reset link if the email is associated
    /// with a confirmed user account. For security reasons, the response does not reveal whether the email exists or is
    /// confirmed. This class is intended to be used within a Blazor or ASP.NET Core application as part of the identity
    /// management system.</remarks>
    public partial class ForgotPassword
    {
        /// <summary>
        /// Gets or sets the input data submitted from the form.
        /// </summary>
        [SupplyParameterFromForm] private InputModel Input { get; set; } = default!;

        /// <summary>
        /// Initializes the component and sets up any required state before rendering.
        /// </summary>
        protected override void OnInitialized()
        {
            Input ??= new();
        }

        /// <summary>
        /// Handles a valid password reset request by generating a password reset link and sending it to the user's
        /// email address.
        /// </summary>
        /// <remarks>If the user does not exist or the email address is not confirmed, the method
        /// redirects to the password reset confirmation page without revealing the existence or status of the account.
        /// This helps protect user privacy and security.</remarks>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task OnValidSubmitAsync()
        {
            var user = await UserManager.FindByEmailAsync(Input.Email);
            if (user is null || !(await UserManager.IsEmailConfirmedAsync(user)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                RedirectManager.RedirectTo("Account/ForgotPasswordConfirmation");
                return;
            }

            // For more information on how to enable account confirmation and password reset please
            // visit https://go.microsoft.com/fwlink/?LinkID=532713
            var code = await UserManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = NavigationManager.GetUriWithQueryParameters(
                NavigationManager.ToAbsoluteUri("Account/ResetPassword").AbsoluteUri,
                new Dictionary<string, object?> { ["code"] = code });

            await EmailSender.SendPasswordResetLinkAsync(user, Input.Email, HtmlEncoder.Default.Encode(callbackUrl));

            RedirectManager.RedirectTo("Account/ForgotPasswordConfirmation");
        }

        /// <summary>
        /// Gets or sets the email address provided by the user.
        /// </summary>
        /// <remarks>The value must be a valid email address format. This property is typically used to
        /// capture user input in forms that require an email address.</remarks>
        private sealed class InputModel
        {
            /// <summary>
            /// Gets or sets the email address associated with the user.
            /// </summary>
            [Required, EmailAddress] public string Email { get; set; } = "";
        }
    }
}
