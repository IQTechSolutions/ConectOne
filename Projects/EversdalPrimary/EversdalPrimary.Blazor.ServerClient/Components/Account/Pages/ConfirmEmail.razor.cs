using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace EversdalPrimary.Blazor.ServerClient.Components.Account.Pages
{
    /// <summary>
    /// Represents a Razor component that handles user email confirmation as part of the authentication process.
    /// </summary>
    /// <remarks>This component processes email confirmation requests by validating the user ID and
    /// confirmation code supplied via query parameters. It displays a status message indicating whether the email
    /// confirmation was successful or if an error occurred. Typically, this component is used as part of an account
    /// management or registration workflow.</remarks>
    public partial class ConfirmEmail
    {
        private string? statusMessage;

        /// <summary>
        /// Gets or sets the current HTTP context for the component.
        /// </summary>
        /// <remarks>This property provides access to HTTP-specific information about the current request,
        /// such as user identity, request headers, and response details. It is typically supplied as a cascading
        /// parameter in Blazor applications to enable components to interact with the underlying HTTP
        /// context.</remarks>
        [CascadingParameter] private HttpContext HttpContext { get; set; } = default!;

        /// <summary>
        /// Gets or sets the user identifier provided in the query string.
        /// </summary>
        [SupplyParameterFromQuery] private string? UserId { get; set; }

        /// <summary>
        /// Gets or sets the code value supplied from the query string.
        /// </summary>
        [SupplyParameterFromQuery] private string? Code { get; set; }

        /// <summary>
        /// Asynchronously initializes the component and attempts to confirm the user's email address based on the
        /// provided user ID and confirmation code.
        /// </summary>
        /// <remarks>If the user ID or confirmation code is missing, the user is redirected. If the user
        /// cannot be found, a 404 status code is set and an error message is displayed. Otherwise, the method attempts
        /// to confirm the user's email and sets a status message indicating the result.</remarks>
        /// <returns>A task that represents the asynchronous initialization operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            if (UserId is null || Code is null)
            {
                RedirectManager.RedirectTo("");
                return;
            }

            var user = await UserManager.FindByIdAsync(UserId);
            if (user is null)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                statusMessage = $"Error loading user with ID {UserId}";
            }
            else
            {
                var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(Code));
                var result = await UserManager.ConfirmEmailAsync(user, code);
                statusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
            }
        }
    }
}
