namespace IdentityModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents the response returned after a user registration attempt.
    /// </summary>
    public record RegistrationResponse
    {
        /// <summary>
        /// Gets a value indicating whether the registration was successful.
        /// </summary>
        public bool IsSuccessfulRegistration { get; init; }

        /// <summary>
        /// Gets a collection of error messages, if any, encountered during the registration process.
        /// </summary>
        public IEnumerable<string> Errors { get; init; }
    }
}