using IdentityModule.Domain.Interfaces;
using IdentityModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Mvc;

namespace IdentityModule.Infrastructure.Controllers
{
    /// <summary>
    /// API controller for managing audit trails.
    /// </summary>
    [Route("api/[controller]"), ApiController]
    public class AuditTrailsController : ControllerBase
    {
        private readonly IAuditTrailsService _auditService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditTrailsController"/> class.
        /// </summary>
        /// <param name="currentUserService">The current user service.</param>
        /// <param name="auditService">The audit trails service.</param>
        public AuditTrailsController(IAuditTrailsService auditService)
        {
            _auditService = auditService;
        }

        /// <summary>
        /// Retrieves the audit trails for the specified user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>An <see cref="IActionResult"/> containing the audit trails.</returns>
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserTrailsAsync(string userId)
        {
            var result = await _auditService.GetCurrentUserTrailsAsync(userId);
            return Ok(result);
        }

        /// <summary>
        /// Exports the audit trails to an Excel file.
        /// </summary>
        /// <param name="searchString">The search string to filter audit trails.</param>
        /// <param name="searchInOldValues">Indicates whether to search in old values.</param>
        /// <param name="searchInNewValues">Indicates whether to search in new values.</param>
        /// <returns>An <see cref="IActionResult"/> containing the exported Excel file.</returns>
        [HttpGet("export")]
        public async Task<IActionResult> ExportExcel([FromQuery]ExportToExcelRequest request)
        {
            var data = await _auditService.ExportToExcelAsync(request.UserId, request.SearchString, request.SearchInOldValues, request.SearchInNewValues);
            return Ok(data);
        }
    }
}

