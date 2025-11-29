namespace IdentityModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents a request to initiate the forgot password process.
    /// </summary>
    public record ForgotPasswordRequest
    {
        /// <summary>
        /// Gets or initializes the email address associated with the account.
        /// </summary>
        public string EmailAddress { get; init; } = null!;

        /// <summary>
        /// Gets or initializes the URL to which the user should be redirected after resetting the password.
        /// </summary>
        public string? ReturnUrl { get; init; } = null!;
    }
}