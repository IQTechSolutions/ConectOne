namespace IdentityModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents the response returned after an authentication attempt.
    /// </summary>
    public record AuthResponse
    {
        /// <summary>
        /// Gets or sets the timestamp indicating when the user accepted the privacy and usage terms.
        /// </summary>
        public DateTime? PrivacyAndUsageTermsAcceptedTimeStamp { get; set; }

        /// <summary>
        /// Indicates whether the authentication attempt was successful.
        /// </summary>
        public bool IsSuccessfulAuth { get; init; }

        /// <summary>
        /// Contains the error message if the authentication attempt failed.
        /// </summary>
        public string ErrorMessage { get; init; } = string.Empty;

        /// <summary>
        /// The JWT token issued upon successful authentication.
        /// </summary>
        public string Token { get; init; } = string.Empty;

        /// <summary>
        /// The refresh token issued to allow obtaining a new JWT token without re-authenticating.
        /// </summary>
        public string RefreshToken { get; init; }

        /// <summary>
        /// The URL of the authenticated user's profile image.
        /// </summary>
        public string UserImageURL { get; init; }

        /// <summary>
        /// The unique identifier of the authenticated user.
        /// </summary>
        public string UserId { get; init; } = string.Empty;

        /// <summary>
        /// The expiration time of the refresh token.
        /// </summary>
        public DateTime RefreshTokenExpiryTime { get; init; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is accessing the application for the first time.
        /// </summary>
        public bool FirstTimeUser { get; set; }
    }
}