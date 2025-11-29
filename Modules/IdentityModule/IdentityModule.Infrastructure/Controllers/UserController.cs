using ConectOne.Domain.DataTransferObjects;
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
    [Route("api/account/users"), ApiController, Authorize(AuthenticationSchemes = "Bearer")]
    public class UserController(IUserService authenticationService, IRoleService roleService) : ControllerBase
    {
        #region Users

        /// <summary>
        /// Retrieves all users with optional paging and filtering.
        /// Returns pagination metadata in X-Pagination header.
        /// </summary>
        [HttpGet] public async Task<IActionResult> AllUsers([FromQuery] UserPageParameters pageParameters)
        {
            var userList = await authenticationService.GetAllUsersAsync(pageParameters);
            return Ok(userList);
        }

        /// <summary>
        /// Retrieves all users with optional paging and filtering.
        /// Returns pagination metadata in X-Pagination header.
        /// </summary>
        [HttpGet("notifications/byRole/{roleName}")] public async Task<IActionResult> AllUsersForNotifications(string roleName)
        {
            var userList = await roleService.RoleNotificationsUserList(roleName);
            return Ok(userList);
        }

        /// <summary>
        /// Retrieves a list of all users eligible to receive global notifications.
        /// </summary>
        /// <remarks>This method returns a collection of users who are configured to receive global
        /// notifications.  The list is fetched asynchronously from the authentication service.</remarks>
        /// <returns>An <see cref="IActionResult"/> containing the list of users eligible for global notifications.  The result
        /// is returned as an HTTP 200 OK response with the user list in the response body.</returns>
        [HttpGet("notifications/global")] public async Task<IActionResult> AllUsersForNotifications()
        {
            var userList = await authenticationService.GlobalNotificationsUserList();
            return Ok(userList);
        }
        
        /// <summary>
        /// Retrieves all users with optional paging and filtering.
        /// Returns pagination metadata in X-Pagination header.
        /// </summary>
        [HttpGet("paged")] public async Task<IActionResult> PagedUsers([FromQuery] UserPageParameters pageParameters)
        {
            var userList = await authenticationService.PagedUsers(pageParameters);
            return Ok(userList);
        }

        /// <summary>
        /// Returns the total count of users in the system.
        /// </summary>
        [HttpGet("count")] public async Task<IActionResult> UserCount()
        {
            var result = await authenticationService.UserCount();
            return Ok(result);
        }

        /// <summary>
        /// Retrieves detailed user info by userId.
        /// </summary>
        [HttpGet("{userId}")] public async Task<IActionResult> GetUserInfo(string userId)
        {
            var user = await authenticationService.GetUserInfoAsync(userId);
            return Ok(user);
        }

        /// <summary>
        /// Mark user as connected or disconnected.
        /// </summary>
        [HttpPost("changeStatus/{userId}/{status}")] public async Task<IActionResult> ChangeConnectionStatus(string userId, bool status)
        {
            var user = await authenticationService.ChangeConnectionStatus(userId, status);
            return Ok(user);
        }

        /// <summary>
        /// Retrieves user info by email.
        /// </summary>
        [HttpGet("email/{email}")] public async Task<IActionResult> GetUserInfoByEmail(string email)
        {
            var user = await authenticationService.GetUserInfoByEmailAsync(email);
            return Ok(user);
        }

        /// <summary>
        /// Updates user information using the provided UserInfoDto.
        /// </summary>
        [HttpPut("userInfo/create")] public async Task<IActionResult> CreateUserInfo([FromBody] UserInfoDto userInfo)
        {
            var result = await authenticationService.CreateUserInfoAsync(userInfo);
            return Ok(result);
        }

        /// <summary>
        /// Updates user information using the provided UserInfoDto.
        /// </summary>
        [HttpPost("update")] public async Task<IActionResult> UpdateUserInfo([FromBody] UserInfoDto userInfo)
        {
            var result = await authenticationService.UpdateUserInfoAsync(userInfo);
            return Ok(result);
        }

        /// <summary>
        /// Adds or updates an address for a user.
        /// </summary>
        [HttpPost("{userId}/address/addupdate")] public async Task<IActionResult> AddUpdateUserInfoAddress(string userId, [FromBody] AddressDto address)
        {
            await authenticationService.AddUpdateUserInfoAddress(userId, address);
            return Ok();
        }

        /// <summary>
        /// Changes a user's status (e.g., active/inactive).
        /// </summary>
        [HttpPost("changestatus")] public async Task<IActionResult> ChangeUserStatus([FromBody] string userId)
        {
            await authenticationService.ChangeUserStatusAsync(userId);
            return Ok();
        }

        /// <summary>
        /// Changes a user's password.
        /// </summary>
        [HttpPost("changepassword")] public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest changePasswordRequest)
        {
            await authenticationService.ChangeUserPasswordAsync(changePasswordRequest);
            return Ok();
        }

        /// <summary>
        /// Accepts a user registration request.
        /// </summary>
        [HttpPost("registrations/accept")] public async Task<IActionResult> AcceptRegistration([FromBody] AcceptRegistrationRequest registrationRequest)
        {
            await authenticationService.AcceptRegistrationAsync(registrationRequest);
            return Ok();
        }

        /// <summary>
        /// Rejects a user registration request (this example returns a user list, might be incomplete logic).
        /// </summary>
        [HttpPost("registrations/reject")] public async Task<IActionResult> RejectRegistration([FromBody] UserPageParameters pageParameters)
        {
            var userList = await authenticationService.GetAllUsersAsync(pageParameters);
            return Ok(userList);
        }

        /// <summary>
        /// Deletes a user from the system by their unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to be removed.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the success or failure of the operation.</returns>
        [HttpDelete("removeUser/{userId}")] public async Task<IActionResult> DeleteUser(string userId)
        {
            var result = await authenticationService.RemoveUserAsync(userId);
            return Ok(result);
        }

        #endregion
    }
}
