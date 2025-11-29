using ConectOne.Domain.ResultWrappers;
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
    /// API controller for managing <see cref="SchoolGrade"/> entities.
    /// Provides endpoints for CRUD operations and notification recipient queries.
    /// </summary>
    [Route("api/schoolGrades"), ApiController, Authorize(AuthenticationSchemes = "Bearer")]
    public class SchoolGradeController(ISchoolGradeService schoolGradeService) : ControllerBase
    {
        /// <summary>
        /// Retrieves a paginated list of school grades.
        /// </summary>
        [HttpGet("pagedschoolGrades")] public async Task<IActionResult> PagedSchoolGradesAsync([FromQuery] SchoolGradePageParameters pageParameters)
        {
            var result = await schoolGradeService.PagedSchoolGradesAsync(pageParameters);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves all school grades.
        /// </summary>
        /// <remarks>This method returns a collection of all school grades available in the system. The
        /// result is returned as an HTTP 200 OK response containing the data.</remarks>
        /// <returns>An <see cref="IActionResult"/> containing the collection of school grades. The response body includes the
        /// data in the format provided by the service.</returns>
        [HttpGet("all")] public async Task<IActionResult> AllSchoolGradesAsync()
        {
            var result = await schoolGradeService.AllSchoolGradesAsync();
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a specific school grade by ID.
        /// </summary>
        /// <param name="schoolGradeId">The unique identifier of the school grade.</param>
        [HttpGet("{schoolGradeId}")] public async Task<IActionResult> SchoolGradeAsync(string schoolGradeId)
        {
            var result = await schoolGradeService.SchoolGradeAsync(schoolGradeId);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new school grade.
        /// </summary>
        /// <param name="schoolGrade">The data for the school grade to create.</param>
        [HttpPut] public async Task<IActionResult> CreateAsync([FromBody] SchoolGradeDto schoolGrade)
        {
            var result = await schoolGradeService.CreateAsync(schoolGrade);
            return Ok(result);
        }

        /// <summary>
        /// Updates an existing school grade.
        /// </summary>
        /// <param name="schoolGrade">The data for the school grade to update.</param>
        [HttpPost] public async Task<IActionResult> UpdateAsync([FromBody] SchoolGradeDto schoolGrade)
        {
            var result = await schoolGradeService.UpdateAsync(schoolGrade);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a school grade by ID.
        /// </summary>
        /// <param name="schoolGradeId">The ID of the school grade to delete.</param>
        [HttpDelete("{schoolGradeId}")] public async Task<IActionResult> DeleteAsync(string schoolGradeId)
        {
            var result = await schoolGradeService.DeleteAsync(schoolGradeId);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a list of recipients (learners, parents, teachers) for school grade notifications.
        /// </summary>
        /// <param name="pageParameters">Filter criteria for learners and grade.</param>
        [HttpGet("notificationList")] public async Task<IActionResult> SchoolGradeNotificationList([FromQuery] LearnerPageParameters pageParameters)
        {
            var result = await schoolGradeService.SchoolGradeNotificationList(pageParameters);
            return Ok(result.Succeeded
                ? await Result<IEnumerable<RecipientDto>>.SuccessAsync(result.Data)
                : result);
        }
    }
}
