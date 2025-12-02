using ConectOne.Domain.ResultWrappers;
using ConectOne.Infrastructure.Controllers;
using ConectOne.Infrastructure.Interfaces;
using IdentityModule.Domain.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Infrastructure.Controllers
{
    /// <summary>
    /// Controller handling parent-related operations, including retrieval, creation, update, deletion, 
    /// and exporting of parent data. It uses IParentService for business logic and returns results as HTTP responses.
    /// </summary>
    [Route("api/parents"), Authorize(AuthenticationSchemes = "Bearer")]
    public class ParentController(IParentService parentService, IContactInfoService<Parent> service) : BaseContactInfoApiController<Parent>(service)
    {
        /// <summary>
        /// Retrieves a paged list of parent users based on the given page parameters.
        /// Useful if distinguishing between parent records and parent users in the system.
        /// </summary>
        /// <param name="pageParameters">Parameters like page number, page size, and filters for the query.</param>
        /// <returns>A paginated list of ParentDto objects.</returns>
        [HttpGet("pagedparentusers")] public async Task<IActionResult> PagedParentUsersAsync([FromQuery] ParentPageParameters pageParameters)
        {
            var result = await parentService.PagedParentsAsync(pageParameters);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a paged list of parents based on given parameters. 
        /// This might return all parents, not only those considered "users."
        /// </summary>
        /// <param name="pageParameters">Paging and filtering parameters.</param>
        /// <returns>A PaginatedResult containing ParentDto data.</returns>
        [HttpGet("pagedparents")] public async Task<IActionResult> PagedParentsAsync([FromQuery] ParentPageParameters pageParameters)
        {
            var result = await parentService.PagedParentsAsync(pageParameters);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a list of all parents.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch all parent records  using the
        /// underlying query service. The result is returned with an HTTP 200 OK status.</remarks>
        /// <returns>An <see cref="IActionResult"/> containing a collection of parent records.  The response is serialized as
        /// JSON.</returns>
        [HttpGet("all")] public async Task<IActionResult> AllParents()
        {
            var result = await parentService.AllParentsAsync();
            return Ok(result);
        }

        /// <summary>
        /// Returns the total number of parents in the system, helpful for UI dashboards or pagination.
        /// </summary>
        /// <returns>A count of all parent records.</returns>
        [HttpGet("count")] public async Task<IActionResult> ParentCount()
        {
            var result = await parentService.ParentCount(HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a parent by their unique identifier.
        /// </summary>
        /// <param name="parentId">The ID of the parent to retrieve.</param>
        /// <returns>A ParentDto if found, otherwise an error.</returns>
        [HttpGet("{parentId}")] public async Task<IActionResult> ParentAsync(string parentId)
        {
            var result = await parentService.ParentAsync(parentId, false, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a parent by their email address.
        /// Useful for scenarios where you need to find parents using their contact details.
        /// </summary>
        /// <param name="emailAddress">The parent's email address.</param>
        /// <returns>A ParentDto if found.</returns>
        [HttpGet("byemail/{emailAddress}")] public async Task<IActionResult> ParentByEmailAsync(string emailAddress)
        {
            var result = await parentService.ParentByEmailAsync(emailAddress);
            return Ok(result);
        }

        /// <summary>
        /// Determines whether a parent with the specified email address exists.
        /// </summary>
        /// <remarks>This method performs an asynchronous check to determine if a parent record exists for
        /// the given email address. It returns an HTTP 200 response with the result as a boolean value.</remarks>
        /// <param name="emailAddress">The email address to check for an existing parent. Cannot be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> containing a boolean value:  <see langword="true"/> if a parent with the
        /// specified email address exists; otherwise, <see langword="false"/>.</returns>
        [HttpGet("exist/{emailAddress}"), AllowAnonymous] public async Task<IActionResult> ParentExits(string emailAddress)
        {
            var result = await parentService.ParentByEmailAsync(emailAddress);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new parent record using the provided ParentDto.
        /// </summary>
        /// <param name="parentDto">The data of the parent to create.</param>
        /// <returns>The newly created ParentDto, or an error message if creation fails.</returns>
        [HttpPut] public async Task<IActionResult> CreateParentAsync([FromBody] ParentDto parentDto)
        {
            var result = await parentService.CreateAsync(parentDto);
            return Ok(result);
        }

        /// <summary>
        /// Updates an existing parent's data.
        /// </summary>
        /// <param name="parentDto">The updated parent information.</param>
        /// <returns>A success or failure result.</returns>
        [HttpPost] public async Task<IActionResult> UpdateParentAsync([FromBody] ParentDto parentDto)
        {
            var result = await parentService.UpdateAsync(parentDto);
            return Ok(result);
        }

        /// <summary>
        /// Updates an existing parent's data.
        /// </summary>
        /// <param name="parentDto">The updated parent information.</param>
        /// <returns>A success or failure result.</returns>
        [HttpPost("updateprofile")] public async Task<IActionResult> UpdateParenProfiletAsync([FromBody] ParentDto parentDto)
        {
            var result = await parentService.UpdateProfileAsync(parentDto);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a parent by their ID.
        /// </summary>
        /// <param name="parentId">The ID of the parent to remove.</param>
        /// <returns>A success or failure result.</returns>
        [HttpDelete("{parentId}")] public async Task<IActionResult> DeleteAsync(string parentId)
        {
            var result = await parentService.RemoveAsync(parentId);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves all learners associated with a specific parent by parentId.
        /// Returns a list of LearnerDto with learner details.
        /// </summary>
        [HttpPost("parentlearners/{parentId}/create/{learnerId}")] public async Task<IActionResult> CreateParentLearnerAsync(string parentId, string learnerId)
        {
            var result = await parentService.CreateParentLearnerAsync(parentId, learnerId);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves all learners associated with a specific parent by parentId.
        /// Returns a list of LearnerDto with learner details.
        /// </summary>
        [HttpGet("parentlearners/{parentId}")] public async Task<IActionResult> ParentLearnersAsync(string parentId)
        {
            var result = await parentService.ParentLearnersAsync(parentId);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves all learners associated with a specific parent by parentId.
        /// Returns a list of LearnerDto with learner details.
        /// </summary>
        [HttpDelete("parentlearners/{parentId}/{learnerId}")] public async Task<IActionResult> DeleteParentLearnerAsync(string parentId, string learnerId)
        {
            var result = await parentService.RemoveParentLearnerAsync(parentId, learnerId);
            return Ok(result);
        }

        /// <summary>
        /// Exports all parent records into a file and returns a reference (e.g., Base64 string) to that file.
        /// This allows clients to download a list of parents for external analysis or archiving.
        /// </summary>
        /// <returns>A reference to the exported file, or an error if the export fails.</returns>
        [HttpGet("export")] public async Task<IActionResult> Export()
        {
            var result = await parentService.ExportParents();
            return Ok(result);
        }

        #region Notifications

        /// <summary>
        /// Retrieves a list of parent user records suitable for notifications,
        /// optionally filtered by certain <see cref="ParentPageParameters"/> (e.g., ParentId, search criteria).
        /// </summary>
        [HttpGet("notificationList")] public async Task<IActionResult> ParentsNotificationList([FromQuery] ParentPageParameters pageParameters)
        {
            var result = await parentService.ParentsNotificationList(pageParameters);
            if (result.Succeeded)
            {
                return Ok(await Result<IEnumerable<RecipientDto>>.SuccessAsync(result.Data));
            }
            return Ok(result);
        }

        #endregion

        #region Chats

        /// <summary>
        /// Creates a chat group for a parent with the specified ID.
        /// </summary>
        /// <param name="parentId">The identity of the parent the group is created for</param>
        /// <param name="userId">The identity of the current user creating the chat group</param>
        [HttpPut("chats/{parentId}")] public async Task<IActionResult> CreateParentChatGroupAsync(string parentId, [FromBody]string userId)
        {
            var result = await parentService.CreateParentChatGroupAsync(parentId, userId);
            return Ok(result);
        }

        #endregion
    }
}
