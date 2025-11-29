using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using ConectOne.Domain.Constants;
using ConectOne.Domain.Extensions;
using ConectOne.Domain.ResultWrappers;
using IdentityModule.Blazor.Interfaces;
using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Interfaces;
using IdentityModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;

namespace IdentityModule.Blazor.Implimentation
{
    /// <summary>
    /// Provides methods for managing user accounts, authentication, and roles.
    /// </summary>
    public class AccountsClient : IAccountsProvider
    {
        private readonly HttpClient _client;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _protectedLocalStorage;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountsClient"/> class.
        /// </summary>
        /// <param name="client">The HTTP client.</param>
        /// <param name="authStateProvider">The authentication state provider.</param>
        /// <param name="protectedLocalStorage">The local storage service.</param>
        public AccountsClient(HttpClient client, AuthenticationStateProvider authStateProvider, ILocalStorageService protectedLocalStorage)
        {
            _client = client;
            _client.Timeout = new TimeSpan(0, 0, 30);
            _client.DefaultRequestHeaders.Clear();
            _authStateProvider = authStateProvider;
            _protectedLocalStorage = protectedLocalStorage;
        }

        #region Authentication

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="request">The registration request.</param>
        /// <returns>The registration response.</returns>
        public async Task<RegistrationResponse> RegisterUserAsync(RegistrationRequest request)
        {
            var response = await _client.PostAsJsonAsync("account/register", request);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<RegistrationResponse>(content);
                return result!;
            }
            return new RegistrationResponse() { IsSuccessfulRegistration = true };
        }

        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="user">The authentication request.</param>
        /// <returns>The authentication response.</returns>
        public async Task<AuthResponse> Login(AuthRequest user)
        {
            var response = await _client.PostAsJsonAsync("account/login", user);
            var content = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<AuthResponse>(content);

            if (!response.IsSuccessStatusCode)
                return result!;

            await _protectedLocalStorage.SetItemAsync(StorageConstants.Local.AuthToken, result!.Token);
            await _protectedLocalStorage.SetItemAsync(StorageConstants.Local.RefreshToken, result.RefreshToken);

            if (!string.IsNullOrEmpty(result.UserImageURL))
            {
                await _protectedLocalStorage.SetItemAsync(StorageConstants.Local.UserImageURL, result.UserImageURL);
            }

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);
            await ((ICustomAuthenticationStateProvider)_authStateProvider).NotifyUserAuthentication(result.Token);

            return result;
        }

        /// <summary>
        /// Sends a forgot password request.
        /// </summary>
        /// <param name="forgotPasswordDto">The forgot password request.</param>
        /// <returns>The result of the operation.</returns>
        public async Task<IBaseResult<string>> ForgotPassword(ForgotPasswordRequest forgotPasswordDto)
        {
             var response = await _client.PostAsJsonAsync("account/forgot", forgotPasswordDto);
             return await response.ToResultAsync<string>();
        }

        /// <summary>
        /// Gets a reset password token.
        /// </summary>
        /// <param name="forgotPasswordDto">The forgot password request.</param>
        /// <returns>The reset password token response.</returns>
        public async Task<IBaseResult<ResetPasswordTokenResponse>> GetResetPasswordTokenAsync(ForgotPasswordRequest forgotPasswordDto)
        {
            var response = await _client.PostAsJsonAsync("account/reset", forgotPasswordDto);

            return await response.ToResultAsync<ResetPasswordTokenResponse>();
        }

        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <param name="resetPasswordDto">The reset password request.</param>
        /// <returns>The result of the operation.</returns>
        public async Task<IBaseResult> ResetPassword(ResetPasswordRequest resetPasswordDto)
        {
            var response = await _client.PostAsJsonAsync("account/reset", resetPasswordDto);

            return await response.ToResultAsync<UserInfoDto>();
        }

        /// <summary>
        /// Logs out the user.
        /// </summary>
        public async Task Logout()
        {
            await _protectedLocalStorage.RemoveItemAsync(StorageConstants.Local.AuthToken);
            await _protectedLocalStorage.RemoveItemAsync(StorageConstants.Local.RefreshToken);
            await _protectedLocalStorage.RemoveItemAsync(StorageConstants.Local.UserImageURL);
            await ((ICustomAuthenticationStateProvider)_authStateProvider).NotifyUserLogout();
            _client.DefaultRequestHeaders.Authorization = null;
        }

        /// <summary>
        /// Refreshes the authentication token.
        /// </summary>
        /// <returns>The new authentication token.</returns>
        public async Task<string> RefreshToken()
        {
            var token = await _protectedLocalStorage.GetItemAsync<string>(StorageConstants.Local.AuthToken);
            var refreshToken = await _protectedLocalStorage.GetItemAsync<string>(StorageConstants.Local.RefreshToken);

            var response = await _client.PostAsJsonAsync("account/refresh", new RefreshTokenRequest { Token = token!, RefreshToken = refreshToken! });

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<AuthResponse>(content);

            if (!result!.IsSuccessfulAuth)
            {
                throw new ApplicationException("There was an error refreshing this token");
            }

            token = result.Token;
            refreshToken = result.RefreshToken;
            await _protectedLocalStorage.SetItemAsync(StorageConstants.Local.AuthToken, token);
            await _protectedLocalStorage.SetItemAsync(StorageConstants.Local.RefreshToken, refreshToken);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            return token;
        }

        /// <summary>
        /// Tries to refresh the authentication token if it is about to expire.
        /// </summary>
        /// <returns>The new authentication token if refreshed, otherwise an empty string.</returns>
        public async Task<string> TryRefreshToken()
        {
            var availableToken = await _protectedLocalStorage.GetItemAsync<string>(StorageConstants.Local.RefreshToken);
            if (string.IsNullOrEmpty(availableToken)) return string.Empty;
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var exp = user.FindFirst(c => c.Type.Equals("exp"))?.Value;
            var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));
            var timeUtc = DateTime.UtcNow;
            var diff = expTime - timeUtc;
            if (diff.TotalMinutes <= 1)
                return await RefreshToken();
            return string.Empty;
        }

        /// <summary>
        /// Forces a refresh of the authentication token.
        /// </summary>
        /// <returns>The new authentication token.</returns>
        public async Task<string> TryForceRefreshToken()
        {
            return await RefreshToken();
        }

        /// <summary>
        /// Gets device tokens for a list of user IDs.
        /// </summary>
        /// <param name="userIds">The list of user IDs.</param>
        /// <returns>The list of device tokens.</returns>
        public async Task<IBaseResult<List<DeviceTokenDto>>> GetDeviceTokens(List<string> userIds)
        {
            var webRequestResult = await _client.PostAsJsonAsync($"account/deviceTokens", userIds);
            return await webRequestResult.ToResultAsync<List<DeviceTokenDto>>();
        }

        /// <summary>
        /// Sets a device token for a user.
        /// </summary>
        /// <param name="deviceToken">The device token.</param>
        /// <returns>The result of the operation.</returns>
        public async Task<IBaseResult> SetDeviceToken(DeviceTokenDto deviceToken)
        {
            var webRequestResult = await _client.PutAsJsonAsync("account/createDeviceToken", deviceToken);
            return await webRequestResult.ToResultAsync();
        }

        /// <summary>
        /// Removes a device token for a user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="deviceTokenId">The device token ID.</param>
        /// <returns>The result of the operation.</returns>
        public async Task<IBaseResult> RemoveDeviceToken(string userId, string deviceTokenId)
        {
            var webRequestResult = await _client.DeleteAsync($"account/removeDeviceToken/{userId}/{deviceTokenId}");
            return await webRequestResult.ToResultAsync();
        }

        #endregion
    }
}
