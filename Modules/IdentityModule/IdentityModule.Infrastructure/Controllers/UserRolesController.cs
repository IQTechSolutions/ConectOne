using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Interfaces;
using IdentityModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityModule.Infrastructure.Controllers
{
    /// <summary>
    /// AccountController handles user registration, authentication, password resets, user and role management, 
    /// as well as permissions. It integrates with Identity and a custom IAuthService for token generation and 
    /// authentication logic.
    /// </summary>
    [Route("api/account/roles"), ApiController, Authorize(AuthenticationSchemes = "Bearer")]
    public class UserRolesController(IRoleService roleService, IPermissionService permissionService) : ControllerBase
    {
        #region Roles

        /// <summary>
        /// Retrieves all available roles.
        /// </summary>
        [HttpGet, AllowAnonymous] public async Task<IActionResult> GetAllRoles()
        {
            var roles = await roleService.AllRoles();
            return Ok(roles);
        }

        /// <summary>
        /// Retrieves a list of all users who have the Product Manager role.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing a collection of Product Manager users. The response has a status
        /// code of 200 (OK) with the list of users, or an empty list if no Product Managers are found.</returns>
        [HttpGet("productManagers")] public async Task<IActionResult> GetProductManagers()
        {
            var roles = await roleService.ProductManagers();
            return Ok(roles);
        }

        /// <summary>
        /// Gets all roles assigned to a specific user.
        /// </summary>
        [HttpGet("user/{userId}")] public async Task<IActionResult> GetAllUserRoles(string userId)
        {
            var roles = await roleService.UserRolesAsync(userId);
            return Ok(roles);
        }

        /// <summary>
        /// Retrieves the count of users associated with a specified role.
        /// </summary>
        /// <remarks>This method calls the authentication service to fetch the user count for the given
        /// role. The result is returned as an HTTP 200 OK response with the user count in the response body.</remarks>
        /// <param name="roleName">The name of the role for which to retrieve the user count. Cannot be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> containing the count of users in the specified role.</returns>
        [HttpGet("user/count/{roleName}")] public async Task<IActionResult> GetRoleUserCount(string roleName)
        {
            var roles = await roleService.RoleUserCount(roleName);
            return Ok(roles);
        }

        /// <summary>
        /// Creates a new application role.
        /// </summary>
        [HttpPost("create")] public async Task<IActionResult> CreateApplicationRole([FromBody] RoleDto request)
        {
            var result = await roleService.CreateApplicationRole(request);
            return Ok(result);
        }

        /// <summary>
        /// Updates an application role with the specified details.
        /// </summary>
        /// <remarks>This method processes the update request by delegating the operation to the 
        /// <c>authenticationService</c>. Ensure that the <paramref name="request"/> contains  valid role data before
        /// invoking this method.</remarks>
        /// <param name="request">The role details to update, provided as a <see cref="RoleDto"/> object.  This parameter must not be <see
        /// langword="null"/>.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the update operation. Typically, this will include
        /// the updated role details or a status indicating the outcome.</returns>
        [HttpPost("update")] public async Task<IActionResult> UpdateApplicationRole([FromBody] RoleDto request)
        {
            var result = await roleService.UpdateApplicationRole(request);
            return Ok(result);
        }

        /// <summary>
        /// Deletes an application role by roleId.
        /// </summary>
        [HttpDelete("delete/{roleId}")] public async Task<IActionResult> DeleteApplicationRole(string roleId)
        {
            var result = await roleService.DeleteApplicationRole(roleId);
            return Ok(result);
        }

        /// <summary>
        /// Gets details of a role by roleName.
        /// </summary>
        [HttpGet("{roleName}")] public async Task<IActionResult> GetRoleAsync(string roleName)
        {
            var roles = await roleService.RoleAsync(roleName);
            return Ok(roles);
        }

        /// <summary>
        /// Retrieves the child roles associated with the specified role ID.
        /// </summary>
        /// <remarks>This method calls the authentication service to fetch the child roles for the given
        /// role ID. The result is returned as an HTTP 200 OK response containing the child roles.</remarks>
        /// <param name="roleId">The unique identifier of the role whose child roles are to be retrieved. Cannot be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> containing a collection of child roles if the operation is successful.</returns>
        [HttpGet("children/{roleId}")] public async Task<IActionResult> GetChildrenAsync(string roleId)
        {
            var roles = await roleService.ChildrenAsync(roleId);
            return Ok(roles);
        }

        /// <summary>
        /// Adds a user to a specified role.
        /// </summary>
        [HttpPut] public async Task<IActionResult> AddUserToRoleAsync([FromBody] UserRoleRequest request)
        {
            await roleService.AddUserToRoleAsync(request.UserId, request.RoleName);
            return Ok();
        }

        /// <summary>
        /// Adds a user to a specified role.
        /// </summary>
        [HttpPut("managedroles/add")]
        public async Task<IActionResult> AddRoleToBeManagedToParentAsync([FromBody] RoleManagementRequest request)
        {
            var result = await roleService.AddRoleToBeManagedToParentAsync(request.ParentRoleId, request.RoleToBeManagedId);
            return Ok(result);
        }

        /// <summary>
        /// Removes a role from a user.
        /// </summary>
        [HttpDelete("delete/{userId}/{roleName}")] public async Task<IActionResult> RemoveRoleFromUserAsync(string userId, string roleName)
        {
            var result = await roleService.RemoveRoleAsync(userId, roleName);
            return Ok(result);
        }

        #endregion

        #region Permissions

        /// <summary>
        /// Gets all permissions (claims) associated with a particular role by roleId.
        /// </summary>
        [HttpGet("role/permissions/{roleId}")] public async Task<IActionResult> GetAllPermissionsByRoleId(string roleId)
        {
            var response = await permissionService.ClaimsByRoleIdAsync(roleId);
            return Ok(response);
        }

        /// <summary>
        /// Creates role claims (permissions) for a role.
        /// </summary>
        [HttpPost("permissions/save")] public async Task<IActionResult> CreateRoleClaim([FromBody]RoleClaimRequest request)
        {
            await permissionService.SaveRoleClaimsAsync(request);
            return Ok();
        }

        /// <summary>
        /// Updates permissions for a role based on a permission request.
        /// </summary>
        [HttpPost("permissions/update")] public async Task<IActionResult> UpdatePermissions([FromBody] PermissionRequest request)
        {
            var result = await permissionService.UpdateRoleClaimsAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a role claim by its ID.
        /// </summary>
        [HttpDelete("permissions/{id}")] public async Task<IActionResult> Delete(int id)
        {
            await permissionService.DeleteRoleClaimAsync(id);
            return Ok();
        }

        #endregion
    }
}
