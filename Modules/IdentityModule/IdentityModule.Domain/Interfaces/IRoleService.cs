using ConectOne.Domain.ResultWrappers;
using IdentityModule.Domain.DataTransferObjects;

namespace IdentityModule.Domain.Interfaces
{
    /// <summary>
    /// Interface for role management operations.
    /// </summary>
    public interface IRoleService
    {
        /// <summary>
        /// Retrieves all application roles.
        /// </summary>
        Task<IBaseResult<IEnumerable<RoleDto>>> AllRoles();

        /// <summary>
        /// Gets roles assigned to a user.
        /// </summary>
        Task<IBaseResult<IEnumerable<RoleDto>>> UserRolesAsync(string userId);

        /// <summary>
        /// Retrieves a collection of roles representing all product managers.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with an enumerable collection of <see cref="RoleDto"/> objects for each product manager. The collection is
        /// empty if no product managers are found.</returns>
        Task<IBaseResult<IEnumerable<RoleDto>>> ProductManagers();

        /// <summary>
        /// Asynchronously retrieves the number of users assigned to the specified role.
        /// </summary>
        /// <param name="roleName">The name of the role for which to count the users. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the total number of users
        /// associated with the specified role.</returns>
        Task<IBaseResult<int>> RoleUserCount(string roleName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        Task<IBaseResult<IEnumerable<RecipientDto>>> RoleNotificationsUserList(string roleName);

        /// <summary>
        /// Creates a new application role.
        /// </summary>
        Task<IBaseResult<RoleDto>> CreateApplicationRole(RoleDto dto);

        /// <summary>
        /// Updates an existing application role with the specified details.
        /// </summary>
        /// <remarks>This method updates the role based on the information provided in the <paramref
        /// name="dto"/> parameter. Ensure that the role ID in the DTO corresponds to an existing role in the
        /// system.</remarks>
        /// <param name="dto">The data transfer object containing the updated role details. Must not be <see langword="null"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the updated role details if the operation is successful.</returns>
        Task<IBaseResult<RoleDto>> UpdateApplicationRole(RoleDto dto);

        /// <summary>
        /// Deletes an application role.
        /// </summary>
        Task<IBaseResult> DeleteApplicationRole(string id);

        /// <summary>
        /// Retrieves role details by name.
        /// </summary>
        Task<IBaseResult<RoleDto>> RoleAsync(string roleName);

        /// <summary>
        /// Retrieves the child roles associated with the specified role name.
        /// </summary>
        /// <remarks>Use this method to retrieve hierarchical role information in scenarios where roles
        /// have parent-child relationships.</remarks>
        /// <param name="roleName">The name of the role for which to retrieve child roles. Cannot be <see langword="null"/> or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with a collection of <see cref="RoleDto"/> representing the child roles.</returns>
        Task<IBaseResult<IEnumerable<RoleDto>>> ChildrenAsync(string roleId);

        /// <summary>
        /// Adds a user to a role.
        /// </summary>
        Task<IBaseResult> AddUserToRoleAsync(string userId, string role);

        /// <summary>
        /// Associates a role with a parent role, designating the specified role to be managed by the parent role.
        /// </summary>
        /// <param name="parentRoleId">The unique identifier of the parent role that will manage the specified role. Cannot be null or empty.</param>
        /// <param name="roleToBeManagedId">The unique identifier of the role to be managed by the parent role. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<IBaseResult> AddRoleToBeManagedToParentAsync(string parentRoleId, string roleToBeManagedId);

        /// <summary>
        /// Removes a role from a user.
        /// </summary>
        Task<IBaseResult> RemoveRoleAsync(string userId, string role);
    }
}
