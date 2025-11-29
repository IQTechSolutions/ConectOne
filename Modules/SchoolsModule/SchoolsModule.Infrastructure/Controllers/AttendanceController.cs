using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Infrastructure.Controllers
{
    /// <summary>
    /// Provides endpoints for managing attendance records and groups.
    /// </summary>
    /// <remarks>The <see cref="AttendanceController"/> class exposes HTTP API methods for retrieving and
    /// creating attendance-related data. It is secured using the "Bearer" authentication scheme and is accessible via
    /// the "api/attendance" route.</remarks>
    /// <param name="attendanceService"></param>
    [Route("api/attendance"), Authorize(AuthenticationSchemes = "Bearer")]
    public class AttendanceController(IAttendanceService attendanceService) : ControllerBase
    {
        /// <summary>
        /// Retrieves a list of attendance records that require completion based on the specified criteria.
        /// </summary>
        /// <remarks>This endpoint is accessed via an HTTP GET request to the "required" route. Ensure
        /// that the query parameters in <paramref name="args"/> are properly populated to retrieve the desired
        /// results.</remarks>
        /// <param name="args">The request parameters used to filter and retrieve the attendance records.  This parameter must not be null
        /// and should include valid query values.</param>
        /// <returns>An <see cref="IActionResult"/> containing the filtered list of attendance records that need completion. The
        /// response is serialized as JSON.</returns>
        [HttpGet("required")] public async Task<IActionResult> GetAttendanceListToComplete([FromQuery] AttendanceListRequest args)
        {
            var result = await attendanceService.GetAttendanceListToCompleteAsync(args);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new attendance group based on the specified parameters.
        /// </summary>
        /// <remarks>This method invokes the underlying service to create an attendance group
        /// asynchronously. Ensure that the <paramref name="args"/> object contains valid data before calling this
        /// method.</remarks>
        /// <param name="args">The request object containing the parameters for creating the attendance group. This must include all
        /// required fields for the operation.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation. Typically, this will be an HTTP 200
        /// response with the created attendance group details.</returns>
        [HttpPut] public async Task<IActionResult> CreateAttendanceGroup([FromBody] AttendanceResultListRequest args)
        {
            var result = await attendanceService.CreateAttendanceGroupAsync(args);
            return Ok(result);
        }

        #region Import/Export

        /// <summary>
        /// Exports all parent records into a file and returns a reference (e.g., Base64 string) to that file.
        /// This allows clients to download a list of parents for external analysis or archiving.
        /// </summary>
        /// <returns>A reference to the exported file, or an error if the export fails.</returns>
        [HttpPost("exportAttendance")] public async Task<IActionResult> ExportAttendance([FromBody] ExportAttendanceRequest request)
        {
            var result = await attendanceService.ExportAttendanceGroup(request);
            return Ok(result);
        }

        #endregion
    }
}
