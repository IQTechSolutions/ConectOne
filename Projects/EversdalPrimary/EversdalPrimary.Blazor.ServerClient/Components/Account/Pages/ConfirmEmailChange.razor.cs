using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace EversdalPrimary.Blazor.ServerClient.Components.Account.Pages
{
    /// <summary>
    /// Represents a Razor component that handles confirmation of a user's email address change via a confirmation link.
    /// </summary>
    /// <remarks>This component processes query parameters from an email confirmation link to validate and
    /// complete an email address change for a user. It displays a status message indicating the result of the
    /// confirmation process. Typically, this component is used as the target of an email confirmation link sent to the
    /// user after they request to change their email address.</remarks>
    public partial class ConfirmEmailChange
    {
        private string? message;

        /// <summary>
        /// Gets or sets the current HTTP context for the component.
        /// </summary>
        /// <remarks>This property provides access to the HTTP context associated with the current
        /// request. It is typically supplied as a cascading parameter in Blazor applications to enable components to
        /// access request-specific information such as user identity, request headers, and session data.</remarks>
        [CascadingParameter] private HttpContext HttpContext { get; set; } = default!;

        /// <summary>
        /// Gets or sets the user identifier provided in the query string.
        /// </summary>
        [SupplyParameterFromQuery] private string? UserId { get; set; }

        /// <summary>
        /// Gets or sets the email address associated with the current request.
        /// </summary>
        [SupplyParameterFromQuery] private string? Email { get; set; }

        /// <summary>
        /// Gets or sets the code value supplied from the query string.
        /// </summary>
        [SupplyParameterFromQuery] private string? Code { get; set; }

        /// <summary>
        /// Asynchronously initializes the component and processes the email change confirmation workflow.
        /// </summary>
        /// <remarks>This method validates the email change confirmation link, updates the user's email
        /// and user name if valid, and refreshes the user's sign-in session. If the confirmation link is invalid or the
        /// operation fails, the user is redirected or an error message is set. This method is typically called by the
        /// Blazor framework during component initialization.</remarks>
        /// <returns>A task that represents the asynchronous initialization operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            if (UserId is null || Email is null || Code is null)
            {
                RedirectManager.RedirectToWithStatus(
                    "Account/Login", "Error: Invalid email change confirmation link.", HttpContext);
                return;
            }

            var user = await UserManager.FindByIdAsync(UserId);
            if (user is null)
            {
                message = "Unable to find user with Id '{userId}'";
                return;
            }

            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(Code));
            var result = await UserManager.ChangeEmailAsync(user, Email, code);
            if (!result.Succeeded)
            {
                message = "Error changing email.";
                return;
            }

            // In our UI email and user name are one and the same, so when we update the email
            // we need to update the user name.
            var setUserNameResult = await UserManager.SetUserNameAsync(user, Email);
            if (!setUserNameResult.Succeeded)
            {
                message = "Error changing user name.";
                return;
            }

            await SignInManager.RefreshSignInAsync(user);
            message = "Thank you for confirming your email change.";
        }
    }
}
