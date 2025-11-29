using IdentityModule.Domain.RequestFeatures;

namespace IdentityModule.Domain.Interfaces
{
    /// <summary>
    /// Represents a response containing details about a role and its associated claims.
    /// This class is used to transfer permission-related data between layers or systems.
    /// </summary>
    public class PermissionResponse
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionResponse"/> class with default values.
        /// </summary>
        public PermissionResponse() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionResponse"/> class with a specified message.
        /// </summary>
        /// <param name="message">A message providing additional context or information about the response.</param>
        public PermissionResponse(string message) { }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier of the role.
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// Gets or sets the list of claims associated with the role.
        /// </summary>
        public List<RoleClaimResponse> RoleClaims { get; set; }

        #endregion
    }
}