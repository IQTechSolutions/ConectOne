using ConectOne.Domain.ResultWrappers;
using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.RequestFeatures;

namespace IdentityModule.Domain.Interfaces
{
    /// <summary>
    /// Defines the contract for account-related operations within the application,
    /// including authentication flows (login, logout, refresh tokens),
    /// user registration and profile management, role assignments, and exporting data.
    /// This interface outlines the essential actions that any account provider should implement.
    /// </summary>
    public interface IAccountsProvider 
    {
        #region Authentication

        /// <summary>
        /// Registers a new user in the system using the provided registration data.
        /// If registration succeeds, the server returns a <see cref="RegistrationResponse"/> 
        /// with an indication of success; otherwise, it includes error details.
        /// </summary>
        /// <param name="userRegistrationDto">The user's sign-up information (email, password, etc.).</param>
        /// <returns>
        /// A <see cref="RegistrationResponse"/> indicating success/failure 
        /// and any relevant server messages.
        /// </returns>
        Task<RegistrationResponse> RegisterUserAsync(RegistrationRequest userRegistrationDto);

        /// <summary>
        /// Authenticates a user with the given credentials. 
        /// On success, returns an <see cref="AuthResponse"/> containing a JWT access token 
        /// and potentially a refresh token, enabling further authenticated requests.
        /// </summary>
        /// <param name="user">Contains the login credentials (email/username, password).</param>
        /// <returns>An <see cref="AuthResponse"/> specifying token(s) or error details.</returns>
        Task<AuthResponse> Login(AuthRequest user);

        /// <summary>
        /// Initiates a "forgot password" process for the specified email address. 
        /// Depending on server implementation, an email containing reset steps or tokens 
        /// might be sent.
        /// </summary>
        /// <param name="forgotPasswordDto">Contains the user's email address for password reset.</param>
        /// <returns>
        /// A result containing a <see cref="string"/> message or error details.
        /// </returns>
        Task<IBaseResult<string>> ForgotPassword(ForgotPasswordRequest forgotPasswordDto);

        /// <summary>
        /// Generates or retrieves a special token used to reset the user's password, 
        /// typically sent to the user via email.
        /// </summary>
        /// <param name="forgotPasswordDto">The user's email or additional data for generating the token.</param>
        /// <returns>A result containing the <see cref="ResetPasswordTokenResponse"/> or an error.</returns>
        Task<IBaseResult<ResetPasswordTokenResponse>> GetResetPasswordTokenAsync(ForgotPasswordRequest forgotPasswordDto);

        /// <summary>
        /// Completes the password reset process, applying the new password to the user account
        /// if the provided token is valid.
        /// </summary>
        /// <param name="resetPasswordDto">
        /// The reset request containing token, email, and the new desired password.
        /// </param>
        /// <returns>An <see cref="IBaseResult"/> indicating success/failure of the reset.</returns>
        Task<IBaseResult> ResetPassword(ResetPasswordRequest resetPasswordDto);

        /// <summary>
        /// Retrieves all device tokens associated with the given user IDs,
        /// typically for notification or messaging purposes.
        /// </summary>
        /// <param name="userIds">A list of user identifiers to look up tokens for.</param>
        /// <returns>A result containing the matching <see cref="DeviceTokenDto"/> objects.</returns>
        Task<IBaseResult<List<DeviceTokenDto>>> GetDeviceTokens(List<string> userIds);

        /// <summary>
        /// Associates a new or existing device token to a user, enabling push notifications 
        /// for that device.
        /// </summary>
        /// <param name="token">A <see cref="DeviceTokenDto"/> with token data and user reference.</param>
        /// <returns>An <see cref="IBaseResult"/> specifying success or failure.</returns>
        Task<IBaseResult> SetDeviceToken(DeviceTokenDto token);

        /// <summary>
        /// Removes a specific device token from a user's profile, preventing further notifications 
        /// to that token.
        /// </summary>
        /// <param name="userId">The user's unique identifier.</param>
        /// <param name="deviceTokenId">The token or token ID to be removed.</param>
        /// <returns>An <see cref="IBaseResult"/> specifying success or failure.</returns>
        Task<IBaseResult> RemoveDeviceToken(string userId, string deviceTokenId);

        /// <summary>
        /// Logs out the currently authenticated user by clearing their token(s) 
        /// and updating any internal authentication state.
        /// </summary>
        /// <returns>A task representing the logout operation.</returns>
        Task Logout();

        /// <summary>
        /// Requests a new JWT access token using an existing refresh token. 
        /// Updates stored tokens upon success.
        /// </summary>
        /// <returns>The updated access token, or an exception on failure.</returns>
        Task<string> RefreshToken();

        /// <summary>
        /// Attempts to refresh the token only if it is close to expiration. 
        /// If no refresh is necessary, returns an empty string.
        /// </summary>
        /// <returns>A <see cref="string"/> containing the new token if refreshed, 
        /// otherwise empty.</returns>
        Task<string> TryRefreshToken();

        /// <summary>
        /// Forces a token refresh, regardless of expiration state, 
        /// returning the newly obtained token from the server.
        /// </summary>
        /// <returns>The updated token as a <see cref="string"/>.</returns>
        Task<string> TryForceRefreshToken();

        #endregion
    }
}
