using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace IdentityModule.Blazor.StateManagers
{
    /// <summary>
    /// Defines a contract for providing and managing authentication state within an application.
    /// </summary>
    /// <remarks>Implementations of this interface enable components to retrieve the current authentication
    /// state, notify the system of authentication or logout events, and access the current authenticated user. This is
    /// typically used in applications that require custom authentication logic or integration with external
    /// authentication providers.</remarks>
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
