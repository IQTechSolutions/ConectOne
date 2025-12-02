using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Maui.Storage;

namespace IdentityModule.Blazor.StateManagers
{
    /// <summary>
    /// Provides an authentication state provider for .NET MAUI applications that manages user authentication state
    /// using secure storage and JWT tokens.
    /// </summary>
    /// <remarks>This provider enables authentication and logout functionality by storing and retrieving
    /// authentication tokens from secure storage. It integrates with Blazor's authentication system to notify
    /// subscribers of authentication state changes. Use this class to support authentication scenarios in MAUI
    /// applications that require persistent user sessions.</remarks>
    public class MauiAuthenticationStateProvider : AuthenticationStateProvider, ICustomAuthenticationStateProvider
    {
        /// <summary>
        /// Logs in the user by storing the provided authentication token and updating the authentication state.
        /// </summary>
        /// <param name="token">The authentication token to be stored for the current user. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous login operation.</returns>
        public async Task Login(string token)
        {
            await SecureStorage.SetAsync("accounttoken", token);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        /// <summary>
        /// Logs out the current user and updates the authentication state asynchronously.
        /// </summary>
        /// <remarks>This method removes the user's authentication token from secure storage and notifies
        /// subscribers of the authentication state change. After calling this method, the user will be considered
        /// unauthenticated.</remarks>
        /// <returns>A task that represents the asynchronous logout operation.</returns>
        public async Task Logout()
        {
            SecureStorage.Remove("accounttoken");
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        /// <summary>
        /// Asynchronously retrieves the current user's authentication state based on a stored account token.
        /// </summary>
        /// <remarks>This method attempts to read an account token from secure storage and parse the
        /// user's claims from it. If the token is missing or invalid, the returned authentication state will indicate
        /// an unauthenticated user. If an error occurs while accessing secure storage, the stored token is removed and
        /// the user is treated as unauthenticated.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="AuthenticationState"/> object representing the current user's authentication state. If no valid
        /// account token is found, the user is considered unauthenticated.</returns>
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            try
            {
                var userInfo = await SecureStorage.GetAsync("accounttoken");
                if (userInfo != null)
                {
                    var claims = JwtParser.ParseClaimsFromJwt(userInfo);
                    identity = new ClaimsIdentity(claims, "Server authentication");
                }
            }
            catch (HttpRequestException ex)
            {
                SecureStorage.Remove("accounttoken");
                Console.WriteLine("Request failed:" + ex.ToString());
            }
            catch (Exception e)
            {
                SecureStorage.Remove("accounttoken");
                Console.WriteLine("Request failed:" + e.ToString());
            }

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        /// <summary>
        /// Notifies the authentication state provider of a user authentication event.
        /// </summary>
        /// <param name="token">The authentication token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task NotifyUserAuthentication(string token)
        {
            var authState = Task.FromResult(await GetAuthenticationStateAsync());
            NotifyAuthenticationStateChanged(authState);
        }

        /// <summary>
        /// Notifies the authentication state provider of a user logout event.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task NotifyUserLogout()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(anonymousUser));
            NotifyAuthenticationStateChanged(authState);
        }

        /// <summary>
        /// Gets the current authenticated user.
        /// </summary>
        /// <returns>The current <see cref="ClaimsPrincipal"/> representing the authenticated user.</returns>
        public async Task<ClaimsPrincipal> CurrentUser()
        {
            var state = await this.GetAuthenticationStateAsync();
            return state.User;
        }
    }
}
