namespace IdentityModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents a request to refresh an authentication token.
    /// </summary>
    /// <remarks>This class is typically used in scenarios where an expired or soon-to-expire token needs to
    /// be replaced with a new one using a valid refresh token.</remarks>
    public class RefreshTokenRequest
    {
        /// <summary>
        /// Gets or sets the authentication token used to access secured resources.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the refresh token used to obtain a new access token when the current token expires.
        /// </summary>
        /// <remarks>Ensure that the refresh token is securely stored and transmitted, as it grants access
        /// to sensitive resources. The token may need to be updated periodically based on the authentication provider's
        /// policies.</remarks>
        public string RefreshToken { get; set; }
    }
}
