namespace IdentityModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents a request to assign a role to a user.
    /// </summary>
    public record UserRoleRequest
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRoleRequest"/> class with default values.
        /// </summary>
        public UserRoleRequest() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRoleRequest"/> class with specified user ID and role name.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="roleName">The name of the role to assign to the user.</param>
        public UserRoleRequest(string userId, string roleName)
        {
            UserId = userId;
            RoleName = roleName;
        }

        #endregion

        /// <summary>
        /// Gets the unique identifier of the user.
        /// </summary>
        public string UserId { get; init; }

        /// <summary>
        /// Gets the name of the role to assign to the user.
        /// </summary>
        public string RoleName { get; init; }
    }
}