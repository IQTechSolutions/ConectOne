using IdentityModule.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace KiburiOnline.Blazor.ServerClient.Components.Account
{
    /// <summary>
    /// Provides access to the current authenticated user within the context of an HTTP request, enforcing that a valid
    /// user is present.
    /// </summary>
    /// <param name="userManager">The user manager used to retrieve user information from the current HTTP context.</param>
    /// <param name="redirectManager">The redirect manager used to handle redirection when a valid user cannot be found.</param>
    internal sealed class IdentityUserAccessor(UserManager<ApplicationUser> userManager, IdentityRedirectManager redirectManager)
    {
        /// <summary>
        /// Retrieves the current authenticated user associated with the specified HTTP context. Throws an error and
        /// redirects if the user cannot be loaded.
        /// </summary>
        /// <remarks>If the user cannot be loaded, the method redirects the response to an error page and
        /// does not return a user object. This method is typically used to ensure that a valid user is present before
        /// proceeding with further operations.</remarks>
        /// <param name="context">The HTTP context containing the user principal from which to retrieve the application user. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the authenticated <see
        /// cref="ApplicationUser"/> associated with the context.</returns>
        public async Task<ApplicationUser> GetRequiredUserAsync(HttpContext context)
        {
            var user = await userManager.GetUserAsync(context.User);

            if (user is null)
            {
                redirectManager.RedirectToWithStatus("Account/InvalidUser", $"Error: Unable to load user with ID '{userManager.GetUserId(context.User)}'.", context);
            }

            return user;
        }
    }
}
