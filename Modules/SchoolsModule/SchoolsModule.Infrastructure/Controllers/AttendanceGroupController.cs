using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Infrastructure.Controllers;

/// <summary>
/// API controller for managing <see cref="AttendanceGroup"/> entities.
/// </summary>
[Route("api/attendancegroups"), ApiController, Authorize(AuthenticationSchemes = "Bearer")]
public class AttendanceGroupController(IAttendanceGroupService service) : ControllerBase
{
    /// <summary>
    /// Retrieves a paginated list of attendance groups based on the specified parameters.
    /// </summary>
    /// <remarks>This method handles HTTP GET requests to the "paged" endpoint and returns the result in an
    /// HTTP 200 OK response.</remarks>
    /// <param name="parameters">The parameters used to define the pagination and filtering criteria for the attendance groups.</param>
    /// <returns>An <see cref="IActionResult"/> containing the paginated list of attendance groups.</returns>
    [HttpGet("paged")] public async Task<IActionResult> PagedAsync([FromQuery] AttendanceGroupPageParameters parameters)
    {
        var result = await service.PagedAttendanceGroupsAsync(parameters);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves all attendance groups associated with the specified parent group identifier.
    /// </summary>
    /// <param name="parentGroupId">The identifier of the parent group for which to retrieve attendance groups. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> containing the list of attendance groups associated with the specified parent
    /// group.</returns>
    [HttpGet("all/{parentGroupId}")] public async Task<IActionResult> AllAsync(string parentGroupId)
    {
        var result = await service.AllAttendanceGroupsAsync(parentGroupId);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves the attendance group details for the specified group identifier.
    /// </summary>
    /// <remarks>This method is an HTTP GET endpoint that returns the details of an attendance group based on
    /// the provided identifier.</remarks>
    /// <param name="attendanceGroupId">The unique identifier of the attendance group to retrieve.</param>
    /// <returns>An <see cref="IActionResult"/> containing the attendance group details if found; otherwise, an appropriate error
    /// response.</returns>
    [HttpGet("{attendanceGroupId}")] public async Task<IActionResult> AttendanceGroupAsync(string attendanceGroupId)
    {
        var result = await service.AttendanceGroupAsync(attendanceGroupId);
        return Ok(result);
    }

    /// <summary>
    /// Asynchronously creates a new attendance group.
    /// </summary>
    /// <param name="attendanceGroup">The attendance group data transfer object containing the details of the group to be created. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IActionResult"/>
    /// indicating the outcome of the operation.</returns>
    [HttpPut] public async Task<IActionResult> CreateAsync([FromBody] AttendanceGroupDto attendanceGroup)
    {
        var result = await service.CreateAsync(attendanceGroup);
        return Ok(result);
    }

    /// <summary>
    /// Updates the specified attendance group asynchronously.
    /// </summary>
    /// <param name="attendanceGroup">The attendance group data transfer object containing the updated information. Cannot be null.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the update operation.</returns>
    [HttpPost] public async Task<IActionResult> UpdateAsync([FromBody] AttendanceGroupDto attendanceGroup)
    {
        var result = await service.UpdateAsync(attendanceGroup);
        return Ok(result);
    }

    /// <summary>
    /// Deletes the attendance group with the specified identifier.
    /// </summary>
    /// <param name="attendanceGroupId">The unique identifier of the attendance group to delete. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the delete operation.</returns>
    [HttpDelete("{attendanceGroupId}")] public async Task<IActionResult> DeleteAsync(string attendanceGroupId)
    {
        var result = await service.DeleteAsync(attendanceGroupId);
        return Ok(result);
    }
}

