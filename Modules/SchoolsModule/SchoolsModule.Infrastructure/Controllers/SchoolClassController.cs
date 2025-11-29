using ConectOne.Domain.ResultWrappers;
using IdentityModule.Domain.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Infrastructure.Controllers
{
    /// <summary>
    /// The SchoolClassController handles HTTP requests related to school classes.
    /// </summary>
    [Route("api/schoolClasses"), ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class SchoolClassController(ISchoolClassService schoolClassService) : ControllerBase
    {
        /// <summary>
        /// Gets a paginated list of school classes.
        /// </summary>
        /// <param name="pageParameters">The pagination parameters.</param>
        /// <returns>An <see cref="IActionResult"/> containing the paginated list of school classes.</returns>
        [HttpGet("pagedschoolClasses")] public async Task<IActionResult> PagedSchoolClassesAsync([FromQuery] SchoolClassPageParameters pageParameters)
        {
            var result = await schoolClassService.PagedSchoolClassesAsync(pageParameters);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a list of all school classes.
        /// </summary>
        /// <remarks>This method is an HTTP GET endpoint that fetches all school classes from the
        /// underlying service.</remarks>
        /// <returns>An <see cref="IActionResult"/> containing a collection of school classes. The response is serialized as JSON
        /// and returned with an HTTP 200 status code.</returns>
        [HttpGet("all")]
        public async Task<IActionResult> AllSchoolClassesAsync()
        {
            var result = await schoolClassService.AllSchoolClassesAsync();
            return Ok(result);
        }

        /// <summary>
        /// Gets a specific school class by its ID.
        /// </summary>
        /// <param name="schoolClassId">The ID of the school class.</param>
        /// <returns>An <see cref="IActionResult"/> containing the school class details.</returns>
        [HttpGet("{schoolClassId}")] public async Task<IActionResult> SchoolClassAsync(string schoolClassId)
        {
            var result = await schoolClassService.SchoolClassAsync(schoolClassId);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new school class.
        /// </summary>
        /// <param name="schoolClass">The school class data transfer object.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the creation operation.</returns>
        [HttpPut] public async Task<IActionResult> CreateAsync([FromBody] SchoolClassDto schoolClass)
        {
            var result = await schoolClassService.CreateAsync(schoolClass);
            return Ok(result);
        }

        /// <summary>
        /// Updates an existing school class.
        /// </summary>
        /// <param name="schoolClass">The school class data transfer object.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the update operation.</returns>
        [HttpPost] public async Task<IActionResult> UpdateAsync([FromBody] SchoolClassDto schoolClass)
        {
            var result = await schoolClassService.UpdateAsync(schoolClass);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a school class by its ID.
        /// </summary>
        /// <param name="schoolClassId">The ID of the school class to delete.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the delete operation.</returns>
        [HttpDelete("{schoolClassId}")] public async Task<IActionResult> DeleteAsync(string schoolClassId)
        {
            var result = await schoolClassService.DeleteAsync(schoolClassId);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a list of recipients (learners and their related contacts) for a given school class, 
        /// based on the provided filtering criteria such as class ID, grade, gender, and age range.
        /// </summary>
        /// <param name="pageParameters">Filter parameters including class ID, grade ID, gender, parent ID, and paging info.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> containing either a list of <see cref="RecipientDto"/> objects 
        /// on success or error messages on failure.
        /// </returns>
        /// <response code="200">
        /// Returns a successful result wrapping the list of recipients (even if the list is empty).
        /// </response>
        [HttpGet("notificationList")] public async Task<IActionResult> SchoolClassNotificationList([FromQuery] LearnerPageParameters pageParameters)
        {
            var result = await schoolClassService.SchoolClassNotificationList(pageParameters);
            if (result.Succeeded)
            {
                return Ok(await Result<IEnumerable<RecipientDto>>.SuccessAsync(result.Data));
            }
            return Ok(result);
        }

        #region Chats

        /// <summary>
        /// Creates a chat group for a parent with the specified ID.
        /// </summary>
        /// <param name="parentId">The identity of the parent the group is created for</param>
        /// <param name="userId">The identity of the current user creating the chat group</param>
        [HttpPut("chats/{classId}")]
        public async Task<IActionResult> CreateSchoolClassChatGroupAsync(string classId, [FromBody] string userId)
        {
            var result = await schoolClassService.CreateSchoolClassChatGroupAsync(classId, userId);
            return Ok(result);
        }

        #endregion

        #region Import/Export

        /// <summary>
        /// Exports all parent records into a file and returns a reference (e.g., Base64 string) to that file.
        /// This allows clients to download a list of parents for external analysis or archiving.
        /// </summary>
        /// <returns>A reference to the exported file, or an error if the export fails.</returns>
        [HttpPost("exportAttendance/toBeCompleted")]
        public async Task<IActionResult> ExportAttendanceToBeCompleted([FromBody] ExportAttendanceRequest request)
        {
            var result = await schoolClassService.ExportAttendanceGroupToBeCompleted(request);
            return Ok(result);
        }

        #endregion
    }
}
