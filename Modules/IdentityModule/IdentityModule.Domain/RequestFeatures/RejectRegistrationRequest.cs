namespace IdentityModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents a request to reject a user registration.
    /// This class is used to capture the reason for rejection and the user ID of the rejected registration.
    /// </summary>
    public record RejectRegistrationRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user whose registration is being rejected.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the reason for rejecting the user's registration.
        /// </summary>
        public string? ReasonForRejection { get; set; }

        /// <summary>
        /// Converts the current <see cref="RejectRegistrationRequest"/> instance to a dictionary.
        /// </summary>
        /// <returns>
        /// A dictionary containing the <see cref="UserId"/> and <see cref="ReasonForRejection"/> as key-value pairs.
        /// </returns>
        public Dictionary<string, string?> ToDictionary()
        {
            return new Dictionary<string, string?>
            {
                [nameof(UserId)] = UserId,
                [nameof(ReasonForRejection)] = ReasonForRejection
            };
        }
    }
}
