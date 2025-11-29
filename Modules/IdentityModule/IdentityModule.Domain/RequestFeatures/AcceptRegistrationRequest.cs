using IdentityModule.Domain.DataTransferObjects;

namespace IdentityModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents a request to accept a user registration.
    /// </summary>
    public class AcceptRegistrationRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AcceptRegistrationRequest"/> class with default values.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        public IEnumerable<RoleDto>? SelectedRoles { get; set; }
    }
}
