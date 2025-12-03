using MessagingModule.Domain.Interfaces;
using MessagingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessagingModule.Infrastructure.Controllers
{
    /// <summary>
    /// API controller for managing chat groups and their members.
    /// Supports creation, updates, retrieval, and deletion of chat groups.
    /// </summary>
    [Route("api/chats/groups"), ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ChatGroupController(IChatGroupService chatGroupService) : ControllerBase
    {
        /// <summary>
        /// Retrieves all chat groups for the currently authenticated user.
        /// </summary>
        [HttpGet] public async Task<IActionResult> UserGroups()
        {
            var result = await chatGroupService.ChatGroups(HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves all chat groups associated with a specific user ID.
        /// </summary>
        /// <param name="userId">The user ID to filter chat groups by.</param>
        [HttpGet("{userId}")] public async Task<IActionResult> UserGroups(string userId)
        {
            var result = await chatGroupService.ChatGroups(userId, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new chat group with the specified name and user memberships.
        /// </summary>
        /// <param name="request">The group creation request, including group name and member user IDs.</param>
        [HttpPut] public async Task<IActionResult> CreateChatGroup([FromBody] AddUpdateChatGroupRequest request)
        {
            var result = await chatGroupService.CreateGroupAsync(request, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Updates the name or metadata of an existing chat group.
        /// </summary>
        /// <param name="request">The group DTO containing updated information.</param>
        [HttpPost] public async Task<IActionResult> UpdateChatGroup([FromBody] AddUpdateChatGroupRequest request)
        {
            var result = await chatGroupService.UpdateGroupAsync(request, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a chat group by its unique identifier.
        /// </summary>
        /// <param name="groupId">The ID of the group to delete.</param>
        [HttpDelete("{groupId}")] public async Task<IActionResult> DeleteChatGroup(string groupId)
        {
            var result = await chatGroupService.DeleteGroupAsync(groupId, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves the list of members in a specified chat group.
        /// </summary>
        /// <param name="groupId">The ID of the chat group.</param>
        [HttpGet("members/{groupId}")] public async Task<IActionResult> GroupMembers(string groupId)
        {
            var result = await chatGroupService.GetGroupMembersAsync(groupId, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Updates the members of a specified chat group.
        /// </summary>
        /// <param name="request">The request containing the group ID and list of user IDs to set as members.</param>
        [HttpPost("members")] public async Task<IActionResult> UpdateGroupMembers([FromBody] AddUpdateChatGroupRequest request)
        {
            var result = await chatGroupService.UpdateGroupMembersAsync(request, HttpContext.RequestAborted);
            return Ok(result);
        }
    }

}
