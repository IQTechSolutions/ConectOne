using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;

namespace EversdalPrimary.Blazor.ServerClient.Components.Account.Pages
{
    /// <summary>
    /// Represents a component that allows users to request a new email confirmation link by submitting their email
    /// address.
    /// </summary>
    /// <remarks>This component is typically used in scenarios where a user has not received or has lost their
    /// original email confirmation message. It handles form input, validates the email address, and sends a new
    /// confirmation link if the email is associated with an existing user account. No indication is given to the user
    /// about whether the email exists in the system, to protect user privacy.</remarks>
    public partial class ResendEmailConfirmation
    {
        private string? message;

        /// <summary>
        /// Gets or sets the input data submitted from the form.
        /// </summary>
        [SupplyParameterFromForm] private InputModel Input { get; set; } = default!;

        /// <summary>
        /// Performs additional initialization when the component is first initialized.
        /// </summary>
        /// <remarks>Override this method to set up component state or perform operations that should
        /// occur once during the component's lifetime. This method is called by the framework before rendering the
        /// component for the first time.</remarks>
        protected override void OnInitialized()
        {
            Input ??= new();
        }

        /// <summary>
        /// Handles the submission of a valid email verification form and sends a confirmation link to the specified
        /// email address.
        /// </summary>
        /// <remarks>If the specified email address is not associated with an existing user, a
        /// verification message is still sent to avoid disclosing user existence. The confirmation link is sent to the
        /// provided email address regardless of user registration status.</remarks>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task OnValidSubmitAsync()
        {
            var user = await UserManager.FindByEmailAsync(Input.Email!);
            if (user is null)
            {
                message = "Verification email sent. Please check your email.";
                return;
            }

            var userId = await UserManager.GetUserIdAsync(user);
            var code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = NavigationManager.GetUriWithQueryParameters(
                NavigationManager.ToAbsoluteUri("Account/ConfirmEmail").AbsoluteUri,
                new Dictionary<string, object?> { ["userId"] = userId, ["code"] = code });
            await EmailSender.SendConfirmationLinkAsync(user, Input.Email, HtmlEncoder.Default.Encode(callbackUrl));

            message = "Verification email sent. Please check your email.";
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
