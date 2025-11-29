namespace IdentityModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents a request to reset a user's password.
    /// </summary>
    public record ResetPasswordRequest
    {
        /// <summary>
        /// Gets or initializes the email address associated with the account.
        /// </summary>
        public string UserId { get; init; } = null!;

        /// <summary>
        /// Gets or initializes the new password for the account.
        /// </summary>
        public string Password { get; init; } = null!;

        /// <summary>
        /// Gets or initializes the reset code used to verify the password reset request.
        /// </summary>
        public string ResetCode { get; init; } = null!;
    }
}