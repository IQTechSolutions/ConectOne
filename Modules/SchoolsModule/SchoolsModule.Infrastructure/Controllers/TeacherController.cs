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
    /// The TeacherController provides RESTful endpoints for managing teacher records,
    /// including listing, creating, updating, and deleting teachers. 
    /// It also includes a specialized endpoint for teacher notifications.
    /// </summary>
    [Route("api/teachers"), ApiController, Authorize(AuthenticationSchemes = "Bearer")]
    public class TeacherController(ITeacherService teacherService, IContactInfoService<Teacher> service) : BaseContactInfoApiController<Teacher>(service)
    {
        // Although constructor injection is often used, this simplified record-like syntax 
        // in the class definition achieves the same effect. 
        private readonly ITeacherService _teacherService = teacherService;

        /// <summary>
        /// Retrieves a list of all teachers asynchronously.
        /// </summary>
        /// <remarks>This method handles HTTP GET requests to the "all" endpoint and returns a paginated
        /// list of teachers. The operation is cancellable through the HTTP request's cancellation token.</remarks>
        /// <param name="pageParameters">The parameters for pagination and filtering of the teacher list.</param>
        /// <returns>An <see cref="IActionResult"/> containing the list of teachers.</returns>
        [HttpGet("all")] public async Task<IActionResult> AllTeachersAsync()
        {
            var result = await _teacherService.AllTeachersAsync(HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Returns a paginated list of teachers, based on the provided <see cref="TeacherPageParameters"/>.
        /// </summary>
        /// <param name="pageParameters">Parameters controlling paging, filtering, etc.</param>
        /// <returns>A paged list of <see cref="TeacherDto"/> objects.</returns>
        [HttpGet("pagedteachers")] public async Task<IActionResult> PagedTeachersAsync([FromQuery] TeacherPageParameters pageParameters)
        {
            var result = await _teacherService.PagedTeachersAsync(pageParameters, false, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a list of teacher accounts for sending notifications,
        /// optionally filtered by the provided <paramref name="pageParameters"/>.
        /// </summary>
        /// <param name="pageParameters">Contains possible teacher-specific filters, ID, etc.</param>
        /// <returns>An enumeration of <see cref="UserInfoDto"/> who should receive notifications.</returns>
        [HttpGet("notificationList")] public async Task<IActionResult> TeacherNotificationList([FromQuery] TeacherPageParameters pageParameters)
        {
            var result = await _teacherService.TeachersNotificationList(pageParameters, HttpContext.RequestAborted);
            if (result.Succeeded)
            {
                // Wrap the data in a success result for uniform response structure
                return Ok(await Result<IEnumerable<RecipientDto>>.SuccessAsync(result.Data));
            }
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a single teacher’s record by their unique ID.
        /// </summary>
        /// <param name="teacherId">The teacher's ID to look up.</param>
        /// <returns>An <see cref="IBaseResult"/> containing a <see cref="TeacherDto"/> if found.</returns>
        [HttpGet("{teacherId}")] public async Task<IActionResult> TeacherAsync(string teacherId)
        {
            var result = await _teacherService.TeacherAsync(teacherId, false, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a teacher record by their email address, often used in linking accounts.
        /// </summary>
        /// <param name="emailAddress">The email to search for.</param>
        /// <returns>An <see cref="IBaseResult{TeacherDto}"/> if a matching record is found.</returns>
        [HttpGet("byemail/{emailAddress}")] public async Task<IActionResult> ParentByEmailAsync(string emailAddress)
        {
            var result = await _teacherService.TeacherByEmailAsync(emailAddress, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Determines whether a teacher with the specified email address exists.
        /// </summary>
        /// <remarks>This endpoint is accessible anonymously and can be used to verify the existence of a
        /// teacher by their email address.</remarks>
        /// <param name="emailAddress">The email address of the teacher to check for existence. Cannot be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> containing a boolean value.  <see langword="true"/> if a teacher with the
        /// specified email address exists; otherwise, <see langword="false"/>.</returns>
        [HttpGet("exist/{emailAddress}"), AllowAnonymous] public async Task<IActionResult> TeacherExits(string emailAddress)
        {
            var result = await _teacherService.TeacherExist(emailAddress, HttpContext.RequestAborted);
            return Ok(result);
        }
        
        /// <summary>
        /// Creates a new teacher record in the system, based on the <paramref name="teacher"/> DTO.
        /// </summary>
        /// <param name="teacher">The teacher data to create.</param>
        /// <returns>A success or failure result, including the newly created teacher if successful.</returns>
        [HttpPut] public async Task<IActionResult> CreateAsync([FromBody] TeacherDto teacher)
        {
            var result = await _teacherService.CreateAsync(teacher, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Updates an existing teacher record in the system, applying the changes from <paramref name="teacher"/>.
        /// </summary>
        /// <param name="teacher">The teacher data to update, including the ID referencing the record.</param>
        /// <returns>A success or failure result indicating outcome of the update.</returns>
        [HttpPost] public async Task<IActionResult> UpdateAsync([FromBody] TeacherDto teacher)
        {
            var result = await _teacherService.UpdateAsync(teacher, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a specific teacher from the system, referencing the <paramref name="teacherId"/>.
        /// </summary>
        /// <param name="teacherId">The unique ID of the teacher to remove.</param>
        /// <returns>An <see cref="IBaseResult"/> indicating the success or failure of the deletion.</returns>
        [HttpDelete("{teacherId}")] public async Task<IActionResult> DeleteAsync(string teacherId)
        {
            var result = await _teacherService.RemoveAsync(teacherId, HttpContext.RequestAborted);
            return Ok(result);
        }
    }
}
