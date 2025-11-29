using IdentityModule.Domain.RequestFeatures;

namespace IdentityModule.Domain.Interfaces
{
    /// <summary>
    /// Represents a request to manage permissions for a specific role.
    /// This class is used to transfer role and claim data between layers or systems.
    /// </summary>
    public class PermissionRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier of the role for which permissions are being managed.
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// Gets or sets the list of role claims associated with the role.
        /// </summary>
        public List<RoleClaimRequest> RoleClaims { get; set; }
    }
}
