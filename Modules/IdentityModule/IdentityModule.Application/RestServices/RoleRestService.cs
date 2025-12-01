using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Interfaces;
using IdentityModule.Domain.RequestFeatures;

namespace IdentityModule.Application.RestServices
{
    /// <summary>
    /// Provides methods for managing roles and their associations with users and other roles through RESTful API calls.
    /// </summary>
    /// <remarks>This service acts as an abstraction layer for interacting with role-related endpoints,
    /// enabling operations such as retrieving roles, managing role-user associations, and creating or updating roles.
    /// It relies on an <see cref="IBaseHttpProvider"/> to perform HTTP requests and handle responses.</remarks>
    /// <param name="provider"></param>
    public class RoleRestService(IBaseHttpProvider provider) : IRoleService
    {
        /// <summary>
        /// Retrieves all roles available in the system.
        /// </summary>
        /// <remarks>This method asynchronously fetches a collection of roles from the underlying data
        /// provider. The roles are returned as a collection of RoleDto objects.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  IBaseResult{T} object
        /// wrapping an IEnumerable{T} of RoleDto  instances representing the roles.</returns>
        public async Task<IBaseResult<IEnumerable<RoleDto>>> AllRoles()
        {
            var result = await provider.GetAsync<IEnumerable<RoleDto>>("account/roles");
            return result;
        }

        /// <summary>
        /// Retrieves the roles associated with a specific user.
        /// </summary>
        /// <remarks>The method fetches the roles from the "account/roles" endpoint. Ensure that the user
        /// ID provided  is valid and corresponds to an existing user in the system.</remarks>
        /// <param name="userId">The unique identifier of the user whose roles are to be retrieved.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  IBaseResult{T} of
        /// IEnumerable{RoleDto} representing the user's roles.</returns>
        public async Task<IBaseResult<IEnumerable<RoleDto>>> UserRolesAsync(string userId)
        {
            var result = await provider.GetAsync<IEnumerable<RoleDto>>("account/roles");
            return result;
        }

        /// <summary>
        /// Retrieves a collection of roles that are designated as product managers.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with an enumerable collection of <see cref="RoleDto"/> objects representing product manager roles. The
        /// collection is empty if no product manager roles are found.</returns>
        public async Task<IBaseResult<IEnumerable<RoleDto>>> ProductManagers()
        {
            var result = await provider.GetAsync<IEnumerable<RoleDto>>("account/roles/productManagers");
            return result;
        }

        /// <summary>
        /// Retrieves the count of users associated with the specified role.
        /// </summary>
        /// <remarks>This method sends an asynchronous request to retrieve the user count for the
        /// specified role. Ensure that the role name provided is valid and exists in the system.</remarks>
        /// <param name="roleName">The name of the role for which to retrieve the user count. Cannot be null or empty.</param>
        /// <returns>An <see cref="IBaseResult{T}"/> containing the total number of users assigned to the specified role. The
        /// result will include the count as an integer.</returns>
        public async Task<IBaseResult<int>> RoleUserCount(string roleName)
        {
            var result = await provider.GetAsync<int>($"account/roles/user/count/{roleName}");
            return result;
        }

        /// <summary>
        /// Retrieves a list of users who are recipients of notifications for the specified role.
        /// </summary>
        /// <remarks>This method sends a request to the underlying provider to fetch the notification
        /// recipients for the given role. Ensure that the <paramref name="roleName"/> corresponds to a valid role in
        /// the system.</remarks>
        /// <param name="roleName">The name of the role for which to retrieve notification recipients. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// of <see cref="IEnumerable{RecipientDto}"/> representing the users associated with the specified role.</returns>
        public async Task<IBaseResult<IEnumerable<RecipientDto>>> RoleNotificationsUserList(string roleName)
        {
            var result = await provider.GetAsync<IEnumerable<RecipientDto>>($"account/users/notifications/byRole/{roleName}");
            return result;
        }

        /// <summary>
        /// Creates a new application role based on the provided role data transfer object (DTO).
        /// </summary>
        /// <remarks>This method sends a request to the "account/roles/create" endpoint to create the
        /// specified role. Ensure that the provided <see cref="RoleDto"/> contains all required fields for the role
        /// creation process.</remarks>
        /// <param name="dto">The <see cref="RoleDto"/> containing the details of the role to be created. This parameter cannot be <see
        /// langword="null"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the created role details if the operation is successful.</returns>
        public async Task<IBaseResult<RoleDto>> CreateApplicationRole(RoleDto dto)
        {
            var result = await provider.PutAsync<RoleDto, RoleDto>("account/roles/create", dto);
            return result;
        }

        /// <summary>
        /// Updates an existing application role with the specified details.
        /// </summary>
        /// <remarks>This method sends the updated role details to the "account/roles/update" endpoint.
        /// Ensure that the provided <see cref="RoleDto"/> contains valid data to avoid errors during the update
        /// process.</remarks>
        /// <param name="dto">The <see cref="RoleDto"/> object containing the updated role details. This parameter cannot be <c>null</c>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// of type <see cref="RoleDto"/> representing the updated role details.</returns>
        public async Task<IBaseResult<RoleDto>> UpdateApplicationRole(RoleDto dto)
        {
            var result = await provider.PostAsync<RoleDto, RoleDto>("account/roles/update", dto);
            return result;
        }

        /// <summary>
        /// Deletes an application role with the specified identifier.
        /// </summary>
        /// <remarks>This method sends a delete request to the underlying provider to remove the
        /// application role. Ensure the <paramref name="id"/> corresponds to a valid role before calling this
        /// method.</remarks>
        /// <param name="id">The unique identifier of the application role to delete. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the delete operation.</returns>
        public async Task<IBaseResult> DeleteApplicationRole(string id)
        {
            var result = await provider.DeleteAsync("account/roles/delete", id);
            return result;
        }

        /// <summary>
        /// Retrieves the details of a role based on the specified role name.
        /// </summary>
        /// <param name="roleName">The name of the role to retrieve. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  IBaseResult{T} of type
        /// RoleDto with the details of the role.</returns>
        public async Task<IBaseResult<RoleDto>> RoleAsync(string roleName)
        {
            var result = await provider.GetAsync<RoleDto>($"account/roles/{roleName}");
            return result;
        }

        /// <summary>
        /// Retrieves the child roles of a specified role asynchronously.
        /// </summary>
        /// <param name="roleId">The unique identifier of the role whose child roles are to be retrieved. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// containing an <see cref="IEnumerable{T}"/> of <see cref="RoleDto"/> objects representing the child roles.</returns>
        public async Task<IBaseResult<IEnumerable<RoleDto>>> ChildrenAsync(string roleId)
        {
            var result = await provider.GetAsync<IEnumerable<RoleDto>>($"account/roles/children/{roleId}");
            return result;
        }

        /// <summary>
        /// Assigns a specified role to a user asynchronously.
        /// </summary>
        /// <remarks>This method sends a request to the underlying provider to assign the specified role
        /// to the user. Ensure that the user ID and role name are valid and exist in the system before calling this
        /// method.</remarks>
        /// <param name="userId">The unique identifier of the user to whom the role will be assigned. Cannot be null or empty.</param>
        /// <param name="role">The name of the role to assign to the user. Cannot be null or empty.</param>
        /// <returns>An <see cref="IBaseResult"/> representing the outcome of the operation.</returns>
        public async Task<IBaseResult> AddUserToRoleAsync(string userId, string role)
        {
            var result = await provider.PutAsync("account/roles/create", new UserRoleRequest(){UserId = userId, RoleName = role});
            return result;
        }

        /// <summary>
        /// Adds a role to be managed by a specified parent role asynchronously.
        /// </summary>
        /// <remarks>This method sends a request to associate the specified role with the parent role,
        /// enabling the parent role to manage the specified role. Ensure that both role identifiers are valid and that
        /// the caller has the necessary permissions to perform this operation.</remarks>
        /// <param name="parentRoleId">The unique identifier of the parent role that will manage the specified role.</param>
        /// <param name="roleToBeManagedId">The unique identifier of the role to be managed by the parent role.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> AddRoleToBeManagedToParentAsync(string parentRoleId, string roleToBeManagedId)
        {
            var result = await provider.PutAsync("account/roles/managedroles/add",
                new RoleManagementRequest() { ParentRoleId = parentRoleId, RoleToBeManagedId = roleToBeManagedId });
            return result;
        }

        /// <summary>
        /// Removes the specified role from the user asynchronously.
        /// </summary>
        /// <remarks>This method sends a request to the underlying provider to remove the specified role
        /// from the user. Ensure that the user and role exist before calling this method.</remarks>
        /// <param name="userId">The unique identifier of the user from whom the role will be removed. Cannot be null or empty.</param>
        /// <param name="role">The name of the role to be removed. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveRoleAsync(string userId, string role)
        {
            var result = await provider.DeleteAsync($"account/roles/delete/{userId}/{role}","");
            return result;
        }
    }
}
