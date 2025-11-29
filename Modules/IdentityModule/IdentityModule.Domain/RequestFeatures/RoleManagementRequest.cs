namespace IdentityModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents a request to manage a role within a hierarchical role structure, including the parent role and the
    /// role to be managed.
    /// </summary>
    /// <remarks>This record is used to encapsulate the details required for managing roles, such as assigning
    /// or modifying roles within a system. It includes the unique identifiers for the parent role and the role to be
    /// managed.</remarks>
    public record RoleManagementRequest
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleManagementRequest"/> class.
        /// </summary>
        public RoleManagementRequest() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleManagementRequest"/> class with the specified parent role
        /// ID and role to be managed ID.
        /// </summary>
        /// <param name="parentRoleId">The unique identifier of the parent role associated with this request. Cannot be <see langword="null"/> or
        /// empty.</param>
        /// <param name="roleToBeManagedId">The unique identifier of the role to be managed. Cannot be <see langword="null"/> or empty.</param>
        public RoleManagementRequest(string parentRoleId, string roleToBeManagedId)
        {
            ParentRoleId = parentRoleId;
            RoleToBeManagedId = roleToBeManagedId;
        }

        #endregion

        /// <summary>
        /// Gets the identifier of the parent role associated with this role.
        /// </summary>
        public string ParentRoleId { get; init; }

        /// <summary>
        /// Gets the identifier of the role to be managed.
        /// </summary>
        public string RoleToBeManagedId { get; init; }
    }
}