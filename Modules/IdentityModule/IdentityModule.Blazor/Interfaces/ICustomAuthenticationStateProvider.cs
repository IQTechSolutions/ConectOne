using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace IdentityModule.Blazor.Interfaces
{
    /// <summary>
    /// Defines a contract for providing and managing authentication state in an application.
    /// </summary>
    /// <remarks>Implement this interface to supply custom authentication state logic, such as integrating
    /// with external authentication providers or managing user sessions. Methods on this interface enable components to
    /// retrieve the current authentication state and notify the provider of authentication or logout events. This
    /// interface is typically used in Blazor or similar frameworks to support authentication flows.</remarks>
    public interface ICustomAuthenticationStateProvider
    {
        /// <summary>
        /// Gets the current authentication state asynchronously.
        /// </summary>
        /// <returns>The current <see cref="AuthenticationState"/>.</returns>
        Task<AuthenticationState> GetAuthenticationStateAsync();

        /// <summary>
        /// Notifies the authentication state provider of a user authentication event.
        /// </summary>
        /// <param name="token">The authentication token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task NotifyUserAuthentication(string token);

        /// <summary>
        /// Notifies the authentication state provider of a user logout event.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task NotifyUserLogout();

        /// <summary>
        /// Gets the current authenticated user.
        /// </summary>
        /// <returns>The current <see cref="ClaimsPrincipal"/> representing the authenticated user.</returns>
        Task<ClaimsPrincipal> CurrentUser();
    }
}
