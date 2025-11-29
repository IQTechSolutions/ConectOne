using ConectOne.Domain.ResultWrappers;
using IdentityModule.Domain.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.ActivityGroups;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Infrastructure.Controllers
{
    /// <summary>
    /// The ActivityGroupController provides RESTful endpoints to manage activity groups
    /// (e.g., sports teams, clubs, or extra-curricular groups). Endpoints include creating,
    /// updating, deleting, listing, and managing notifications/permissions 
    /// (such as removing consent or linking categories).
    /// </summary>
    [Route("api/activitygroups"), ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ActivityGroupController(IActivityGroupQueryService activityGroupQueryService, 
        IActivityGroupCommandService activityGroupCommandService, IActivityGroupNotificationService activityGroupNotificationService, IActivityGroupExportService activityGroupExportService) : ControllerBase
    {
        /// <summary>
        /// Returns all activity groups matching the specified filtering or paging criteria.
        /// </summary>
        /// <param name="pageParameters">Contains query params like page size, filters, etc.</param>
        /// <returns>An <see cref="IBaseResult"/> with a list of <see cref="ActivityGroupDto"/> objects.</returns>
        [HttpGet("all")] public async Task<IActionResult> AllActivityGroupsAsync([FromQuery] ActivityGroupPageParameters pageParameters)
        {
            var result = await activityGroupQueryService.AllActivityGroupsAsync(pageParameters);
            return Ok(result);
        }

        /// <summary>
        /// Returns a paginated list of activity groups, based on the provided <see cref="ActivityGroupPageParameters"/>.
        /// </summary>
        [HttpGet("pagedactivitygroups")] public async Task<IActionResult> PagedActivityGroupsAsync([FromQuery] ActivityGroupPageParameters pageParameters)
        {
            var result = await activityGroupQueryService.PagedActivityGroupsAsync(pageParameters);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a paginated list of activity groups specifically associated with an event,
        /// using <see cref="ActivityGroupPageParameters"/> for further filtering or paging.
        /// </summary>
        [HttpGet("pagedactivitygroupsforevent")] public async Task<IActionResult> PagedActivityGroupsForEventAsync([FromQuery] ActivityGroupPageParameters pageParameters)
        {
            var result = await activityGroupQueryService.PagedActivityGroupsForEventAsync(pageParameters);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a paginated list of activity groups that belong to participating event categories.
        /// This is a synchronous method returning an already-computed result.
        /// </summary>
        [HttpGet("pagedActivityGroupsForParticipatingEventCategories")]
        public async Task<IActionResult> PagedActivityGroupsForParticipatingEventCategories([FromQuery] ActivityGroupPageParameters pageParameters)
        {
            var result = await activityGroupQueryService.PagedActivityGroupsForParticipatingEventCategories(pageParameters);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a single activity group by its unique ID.
        /// </summary>
        /// <param name="activityGroupId">ID of the activity group to fetch.</param>
        /// <returns>An <see cref="ActivityGroupDto"/> if found, or an error result if not.</returns>
        [HttpGet("{activityGroupId}")] public async Task<IActionResult> ActivityGroupAsync(string activityGroupId)
        {
            var result = await activityGroupQueryService.ActivityGroupAsync(activityGroupId, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new activity group record using the data from <paramref name="activityGroup"/>.
        /// </summary>
        /// <param name="activityGroup">DTO describing the new group.</param>
        /// <returns>
        /// A success or failure result, containing the newly created activity group if successful.
        /// </returns>
        [HttpPut] public async Task<IActionResult> CreateAsync([FromBody] ActivityGroupDto activityGroup)
        {
            var result = await activityGroupCommandService.CreateAsync(activityGroup, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Updates an existing activity group with the data from <paramref name="activityGroup"/>.
        /// </summary>
        /// <param name="activityGroup">DTO describing the updated group fields.</param>
        /// <returns>A success or failure result from the update operation.</returns>
        [HttpPost] public async Task<IActionResult> UpdateAsync([FromBody] ActivityGroupDto activityGroup)
        {
            var result = await activityGroupCommandService.UpdateAsync(activityGroup, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Deletes an activity group identified by <paramref name="activityGroupId"/>.
        /// </summary>
        /// <param name="activityGroupId">The unique ID of the group to remove.</param>
        /// <returns>A success or failure result indicating the outcome.</returns>
        [HttpDelete("{activityGroupId}")] public async Task<IActionResult> DeleteAsync(string activityGroupId)
        {
            var result = await activityGroupCommandService.DeleteAsync(activityGroupId, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Removes consent (attendance, transport, etc.) from a user in this activity group context
        /// by processing the <see cref="RemoveConsentArgs"/>.
        /// </summary>
        [HttpPost("removeConsent")] public async Task<IActionResult> RemoveConsentAsync([FromBody] RemoveConsentArgs args)
        {
            var result = await activityGroupCommandService.RemoveConsent(args, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Links an activity group to a category, enabling further classification or grouping.
        /// </summary>
        /// <param name="categoryId">The category ID to link.</param>
        /// <param name="activityGroupId">The activity group ID being linked.</param>
        /// <returns>A success or failure result.</returns>
        [HttpPut("categories/add/{categoryId}/{activityGroupId}")] public async Task<IActionResult> CreateActivityGroupCategoryAsync(string categoryId, string activityGroupId)
        {
            var result = await activityGroupCommandService.CreateActivityGroupCategoryAsync(categoryId, activityGroupId, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves learners or members that form a team's participants 
        /// in the specified activity group.
        /// </summary>
        /// <param name="activityGroupId">ID of the group whose members to fetch.</param>
        /// <returns>A list of <see cref="LearnerDto"/> representing the group’s team members.</returns>
        [HttpGet("teamMembers")] public async Task<IActionResult> ActivityGroupTeamMembers([FromQuery] LearnerPageParameters args)
        {
            var result = await activityGroupQueryService.ActivityGroupTeamMembersAsync(args, HttpContext.RequestAborted);
            return Ok(result);
        }

        [HttpPut("teamMembers/add/{activityGroupId}/{learnerId}")] public async Task<IActionResult> CreateActivityGroupTeamMemberAsync(string activityGroupId, string learnerId)
        {
            var result = await activityGroupCommandService.CreateActivityGroupTeamMemberAsync(activityGroupId, learnerId, HttpContext.RequestAborted);
            return Ok(result);
        }

        [HttpDelete("teamMembers/remove/{categoryId}/{activityGroupId}")] public async Task<IActionResult> RemoveActivityGroupTeamMemberAsync(string activityGroupId, string learnerId)
        {
            var result = await activityGroupCommandService.RemoveActivityGroupTeamMemberAsync(activityGroupId, learnerId, HttpContext.RequestAborted);
            return Ok(result);
        }

        #region Notifications

        /// <summary>
        /// Retrieves a list of users (in <see cref="UserInfoDto"/>) who should be notified 
        /// for the given activity group ID (standard membership or subscription).
        /// </summary>
        /// <param name="activityGroupId">ID of the group for which the notification list is requested.</param>
        /// <returns>A success or failure result containing user info objects.</returns>
        [HttpGet("notificationList/{activityGroupId}")] public async Task<IActionResult> ActivityGroupNotificationList(string activityGroupId)
        {
            var result = await activityGroupNotificationService.ActivityGroupNotificationList(activityGroupId, HttpContext.RequestAborted);
            return result.Succeeded ? Ok(await Result<IEnumerable<RecipientDto>>.SuccessAsync(result.Data)) : Ok(result);
        }

        /// <summary>
        /// Retrieves a list of users who are actively participating in a specific activity group,
        /// for notification purposes (potentially a subset of the entire group).
        /// </summary>
        /// <param name="activityGroupId">The group’s ID used to filter participants.</param>
        /// <returns>
        /// A success or failure result with <see cref="UserInfoDto"/> objects for those who need notifications.
        /// </returns>
        [HttpGet("participating/notificationList/{activityGroupId}")] public async Task<IActionResult> ParticipatingActivityGroupNotificationList(string activityGroupId)
        {
            var result = await activityGroupNotificationService.ParticipatingActivityGroupNotificationList(activityGroupId, HttpContext.RequestAborted);
            if (result.Succeeded)
            {
                return Ok(await Result<IEnumerable<RecipientDto>>.SuccessAsync(result.Data));
            }
            return Ok(result);
        }

        #endregion

        #region Import/Export

        /// <summary>
        /// Exports event data (likely in a certain file format or structure).
        /// </summary>
        [HttpGet("exportactivitygroup/{activityGroupId}")] public async Task<IActionResult> Export(string activityGroupId)
        {
            var result = await activityGroupExportService.ExportActivityGroup(activityGroupId, HttpContext.RequestAborted);
            return Ok(result);
        }

        #endregion
    }
}
