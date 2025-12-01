using System.Net.Http.Headers;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace IdentityModule.Blazor.StateManagers
{
    /// <summary>
    /// Custom authentication state provider for managing user authentication state.
    /// </summary>
    public class AuthStateProvider : AuthenticationStateProvider, ICustomAuthenticationStateProvider
    {
        private readonly HttpClient _client;
        private readonly ILocalStorageService protectedSessionStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthStateProvider"/> class.
        /// </summary>
        /// <param name="client">The HTTP client used for making requests.</param>
        /// <param name="localStorageService">The local storage service for storing authentication tokens.</param>
        public AuthStateProvider(HttpClient client, ILocalStorageService localStorageService)
        {
            _client = client;
            protectedSessionStore = localStorageService;
        }

        /// <summary>
        /// Gets the current authentication state asynchronously.
        /// </summary>
        /// <returns>The current <see cref="AuthenticationState"/>.</returns>
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var result = await protectedSessionStore.GetItemAsync<string>("authToken");
            if (string.IsNullOrEmpty(result))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result);
            var claims = JwtParser.ParseClaimsFromJwt(result);
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(result), "auth")));
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
