using System.ComponentModel.DataAnnotations;
using IdentityModule.Domain.Enums;

namespace IdentityModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents an authentication request containing user credentials and authentication type.
    /// </summary>
    public record class AuthRequest
    {
        /// <summary>
        /// Gets the username or email used for authentication.
        /// </summary>
        [Required(ErrorMessage = "User name is required")] public string UserName { get; init; }

        /// <summary>
        /// Gets the password used for authentication.
        /// </summary>
        [Required(ErrorMessage = "Password name is required")] public string Password { get; init; }

        /// <summary>
        /// Gets the type of authentication (e.g., username or email).
        /// </summary>
        public AuthenticationType AuthenticationType { get; init; }
    }
}
