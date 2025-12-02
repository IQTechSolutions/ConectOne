using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using ConectOne.Domain.Constants;
using ConectOne.Domain.Extensions;
using ConectOne.Domain.ResultWrappers;
using IdentityModule.Blazor.StateManagers;
using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Interfaces;
using IdentityModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Maui.Storage;

namespace IdentityModule.Blazor.Implimentation
{
    /// <summary>
    /// Provides high-level account-related services for a Maui-based application. 
    /// Implements user registration, login, logout, token management, and user/role data 
    /// retrieval via an underlying <see cref="HttpClient"/>.
    /// </summary>
    public class MauiAccountsClient : IAccountsProvider
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;
        private readonly AuthenticationStateProvider _authStateProvider;

        /// <summary>
        /// Constructs a new instance of <see cref="MauiAccountsClient"/>, configuring 
        /// <paramref name="client"/> for account/identity operations and capturing
        /// an <paramref name="authStateProvider"/> for updating application authentication state.
        /// </summary>
        /// <param name="client">
        /// The <see cref="HttpClient"/> instance used to communicate with identity endpoints.
        /// </param>
        /// <param name="authStateProvider">
        /// Provides the current authentication state for the application, typically 
        /// updated on successful login/logout.
        /// </param>
        public MauiAccountsClient(HttpClient client, AuthenticationStateProvider authStateProvider)
        {
            _client = client;

            // Configure the HTTP client with a default 30s timeout and empty headers.
            _client.Timeout = new TimeSpan(0, 0, 30);
            _client.DefaultRequestHeaders.Clear();
            _authStateProvider = authStateProvider;

            // Set up case-insensitive JSON deserialization.
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        #region Authentication

        /// <summary>
        /// Registers a new user account by sending the appropriate request data
        /// to the identity server's registration endpoint.
        /// </summary>
        /// <param name="request">Contains the user's signup details (e.g., email, password).</param>
        /// <returns>
        /// A <see cref="RegistrationResponse"/> indicating success/failure of the operation 
        /// and potential error messages.
        /// </returns>
        public async Task<RegistrationResponse> RegisterUserAsync(RegistrationRequest request)
        {
            // Send the registration data to the server.
            var response = await _client.PostAsJsonAsync("account/register", request);

            // If there's an error, deserialize the server response to obtain info.
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<RegistrationResponse>(content, _options);
                return result!;
            }

            // On success, return a default success response with IsSuccessfulRegistration = true.
            return new RegistrationResponse() { IsSuccessfulRegistration = true };
        }

        /// <summary>
        /// Attempts to log in the user by sending credentials to the server,
        /// storing tokens locally upon success, and updating the auth state.
        /// </summary>
        /// <param name="user">Contains login credentials (e.g., username/password).</param>
        /// <returns>
        /// An <see cref="AuthResponse"/> containing an access token, refresh token, 
        /// and any error details.
        /// </returns>
        public async Task<AuthResponse> Login(AuthRequest user)
        {
            try
            {
                var url = "account/login";

                // Serialize the AuthRequest into JSON payload.
                var json = JsonSerializer.Serialize(user);
                var content2 = new StringContent(json, Encoding.UTF8, "application/json");

                // POST to the login endpoint.
                var response = await _client.PostAsync(url, content2);
                var content = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<AuthResponse>(content, _options);

                // If login request failed, return the server's result as error information.
                if (!response.IsSuccessStatusCode)
                    return result!;

                // Otherwise, capture the token and place it in the authentication provider state.
                ((MauiAuthenticationStateProvider)_authStateProvider).Login(result!.Token);

                // Set the bearer token for subsequent calls.
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);

                return result;
            }
            catch (Exception)
            {
                // If something unexpected occurs, bubble up the exception.
                throw;
            }
        }

        /// <summary>
        /// Retrieves a list of device tokens linked to specified user IDs, 
        /// typically for push notification use.
        /// </summary>
        /// <param name="userIds">
        /// A list of user IDs to check for associated device tokens.
        /// </param>
        /// <returns>
        /// A result containing a list of <see cref="DeviceTokenDto"/> objects.
        /// </returns>
        public async Task<IBaseResult<List<DeviceTokenDto>>> GetDeviceTokens(List<string> userIds)
        {
            // Make a POST request with user IDs to retrieve their token info.
            var result = await _client.PostAsJsonAsync($"account/deviceTokens", userIds);
            return await result.ToResultAsync<List<DeviceTokenDto>>();
        }

        /// <summary>
        /// Assigns or updates a device token for the current user in the system, 
        /// enabling push notifications.
        /// </summary>
        /// <param name="token">
        /// A <see cref="DeviceTokenDto"/> carrying the device token string 
        /// and the user association.
        /// </param>
        /// <returns>
        /// An <see cref="IBaseResult"/> indicating whether the operation succeeded or failed.
        /// </returns>
        public async Task<IBaseResult> SetDeviceToken(DeviceTokenDto token)
        {
            var result = await _client.PutAsJsonAsync("account/createDeviceToken", token);
            return await result.ToResultAsync();
        }

        /// <summary>
        /// Removes a specific device token from a given user, 
        /// preventing further notifications to that token.
        /// </summary>
        /// <param name="userId">The unique ID of the user.</param>
        /// <param name="deviceTokenId">The token identifier or content to remove.</param>
        /// <returns>An <see cref="IBaseResult"/> signifying success or failure.</returns>
        public async Task<IBaseResult> RemoveDeviceToken(string userId, string deviceTokenId)
        {
            var result = await _client.DeleteAsync($"account/removeDeviceToken/{userId}/{deviceTokenId}");
            return await result.ToResultAsync();
        }

        /// <summary>
        /// Initiates a forgot-password flow for a given email address, 
        /// possibly sending an email with a reset link.
        /// </summary>
        /// <param name="forgotPasswordDto">
        /// The request containing the user's email address.
        /// </param>
        /// <returns>
        /// A result containing an optional success message or 
        /// error detail.
        /// </returns>
        public async Task<IBaseResult<string>> ForgotPassword(ForgotPasswordRequest forgotPasswordDto)
        {
            var response = await _client.GetAsync($"account/forgot/{forgotPasswordDto.EmailAddress}");

            return await response.ToResultAsync<string>();
        }

        /// <summary>
        /// Retrieves a special token required for resetting a user's password, 
        /// typically to be included in the password reset flow.
        /// </summary>
        /// <param name="forgotPasswordDto">
        /// The user/email detail to generate a reset token for.
        /// </param>
        /// <returns>
        /// A result containing the reset token or error information.
        /// </returns>
        public async Task<IBaseResult<ResetPasswordTokenResponse>> GetResetPasswordTokenAsync(ForgotPasswordRequest forgotPasswordDto)
        {
            var response = await _client.PostAsJsonAsync("account/resetPasswordToken", forgotPasswordDto);
            return await response.ToResultAsync<ResetPasswordTokenResponse>();
        }

        /// <summary>
        /// Resets the user's password using a token and new password data.
        /// </summary>
        /// <param name="resetPasswordDto">Contains token, email, and new password fields.</param>
        /// <returns>
        /// An <see cref="IBaseResult"/> indicating success or failure of the reset.
        /// </returns>
        public async Task<IBaseResult> ResetPassword(ResetPasswordRequest resetPasswordDto)
        {
            var response = await _client.PostAsJsonAsync("account/reset", resetPasswordDto);
            return await response.ToResultAsync<UserInfoDto>();
        }

        /// <summary>
        /// Logs out the current user by clearing their authentication and
        /// removing tokens from secure storage, then clearing the Authorization header.
        /// </summary>
        public async Task Logout()
        {
            // Clear local secure storage for tokens
            SecureStorage.Remove(StorageConstants.Local.AuthToken);
            SecureStorage.Remove(StorageConstants.Local.RefreshToken);
            SecureStorage.Remove(StorageConstants.Local.UserImageURL);

            // Notify the provider that the user has logged out
            await ((MauiAuthenticationStateProvider)_authStateProvider).Logout();

            // Remove the bearer token header
            _client.DefaultRequestHeaders.Authorization = null;
        }

        /// <summary>
        /// Obtains a new JWT access token from the server using the refresh token,
        /// storing the updated tokens in secure storage.
        /// </summary>
        /// <returns>The newly refreshed token as a string.</returns>
        /// <exception cref="ApplicationException">If the token refresh fails on the server side.</exception>
        public async Task<string> RefreshToken()
        {
            // Retrieve the existing access and refresh tokens
            var token = await SecureStorage.GetAsync(StorageConstants.Local.AuthToken);
            var refreshToken = await SecureStorage.GetAsync(StorageConstants.Local.RefreshToken);

            // Issue a token refresh request
            var response = await _client.PostAsJsonAsync("account/refresh",
                new RefreshTokenRequest { Token = token, RefreshToken = refreshToken }
            );

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<AuthResponse>(content);

            if (!result!.IsSuccessfulAuth)
            {
                throw new ApplicationException("There was an error refressing this token");
            }

            // Update tokens in secure storage
            token = result.Token;
            refreshToken = result.RefreshToken;
            await SecureStorage.SetAsync(StorageConstants.Local.AuthToken, token);
            await SecureStorage.SetAsync(StorageConstants.Local.RefreshToken, refreshToken);

            // Update the default authorization header
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            return token;
        }

        /// <summary>
        /// Attempts to refresh the access token if it is close to expiry (less than 1 minute).
        /// If no refresh token is in storage, or not close to expiry, returns an empty string.
        /// </summary>
        /// <returns>The new token if a refresh occurred, or an empty string if no refresh needed.</returns>
        public async Task<string> TryRefreshToken()
        {
            // Check if refresh token is available
            var availableToken = await SecureStorage.GetAsync(StorageConstants.Local.RefreshToken);
            if (string.IsNullOrEmpty(availableToken)) return string.Empty;

            // Evaluate expiration on the current token
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var exp = user.FindFirst(c => c.Type.Equals("exp"))?.Value;
            var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));
            var timeUTC = DateTime.UtcNow;
            var diff = expTime - timeUTC;

            // If token is nearly expired, do a refresh
            if (diff.TotalMinutes <= 1)
                return await RefreshToken();

            return string.Empty;
        }

        /// <summary>
        /// Forces a token refresh, ignoring current expiry conditions.
        /// Usually needed if the app specifically requires the freshest credentials.
        /// </summary>
        /// <returns>A newly obtained token from the server.</returns>
        public async Task<string> TryForceRefreshToken()
        {
            return await RefreshToken();
        }

        #endregion
        
    }
}