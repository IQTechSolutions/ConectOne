using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using GoogleCalendar.Base;

namespace GoogleCalendar.Auth;

/// <summary>
/// Provides authentication and token management services for Google APIs using OAuth 2.0.
/// </summary>
/// <remarks>This service facilitates user authentication and token refresh operations for Google APIs,
/// specifically supporting the Google Calendar API. It manages credential storage and retrieval, and is intended to be
/// used in applications that require secure access to Google services on behalf of a user.</remarks>
public class GoogleAuthService : BaseService
{
    private readonly string[] _scopes = { "https://www.googleapis.com/auth/calendar" };

    /// <summary>
    /// Initializes a new instance of the GoogleAuthService class using the specified HTTP client.
    /// </summary>
    /// <param name="httpClient">The HTTP client instance used to send requests to Google authentication endpoints. Cannot be null.</param>
    public GoogleAuthService(HttpClient httpClient) : base(httpClient) { }

    /// <summary>
    /// Authenticates a user asynchronously using the specified credentials file and token storage path.
    /// </summary>
    /// <remarks>If a valid token exists at the specified token path for the given user, it will be reused;
    /// otherwise, the user may be prompted to authorize access. The method requires network access to complete the
    /// authentication process.</remarks>
    /// <param name="credentialsFilePath">The file path to the client credentials JSON file. This file must exist and contain valid Google client secrets.</param>
    /// <param name="tokenPath">The directory path where the authentication token will be stored or retrieved. Must be a valid, writable
    /// directory path.</param>
    /// <param name="userName">The user name to associate with the authentication session. Used to identify the user for token storage and
    /// retrieval.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a UserCredential object representing
    /// the authenticated user.</returns>
    public async Task<UserCredential> AuthenticateAsync(string credentialsFilePath, string tokenPath, string userName)
    {
        return await HandleErrorAsync(async () =>
        {
            using var stream = new FileStream(credentialsFilePath, FileMode.Open, FileAccess.Read);
            var credPath = Path.Combine(tokenPath, "GoogleAuthToken");

            var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.FromStream(stream).Secrets,
                _scopes,
                userName,
                CancellationToken.None,
                new FileDataStore(credPath, true));

            Log("Authentication successful.");
            return credential;
        });
    }

    /// <summary>
    /// Attempts to refresh the OAuth token for the specified user credential if the current token has expired.
    /// </summary>
    /// <param name="credential">The user credential containing the OAuth token to be checked and refreshed. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the token was
    /// refreshed; otherwise, <see langword="false"/> if the token was still valid.</returns>
    public async Task<bool> RefreshTokenAsync(UserCredential credential)
    {
        Log("Refreshing Google OAuth token...");
        return await HandleErrorAsync(async () =>
        {
            if (credential.Token.IsExpired(credential.Flow.Clock))
            {
                await credential.RefreshTokenAsync(CancellationToken.None);
                Log("Token refreshed successfully.");
                return true; 
            }

            Log("Token is still valid.");
            return false;
        });
    }
}