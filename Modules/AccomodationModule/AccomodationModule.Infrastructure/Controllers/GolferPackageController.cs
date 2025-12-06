using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccomodationModule.Infrastructure.Controllers
{
    /// <summary>
    /// API controller for managing vacation entities.
    /// </summary>
    [Route("api/vacations/golferPackages"), ApiController]
    public class GolferPackageController(IGolferPackageService golferPackageService) : ControllerBase
    {
        /// <summary>
        /// Retrieves all golfer packages for a specific vacation.
        /// </summary>
        /// <param name="vacationId">The ID of the vacation to retrieve golfer packages for.</param>
        /// <returns>An IActionResult containing the golfer packages.</returns>
        [HttpGet("{vacationId}")]
        public async Task<IActionResult> GolferPackagesAsync(string vacationId)
        {
            var result = await golferPackageService.GolferPackageListAsync(vacationId, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Get a golfer packages
        /// </summary>
        /// <param name="golferPackageId">The identity of the golfer packages to retrieve</param>
        /// <returns>A result indicating the success of the operation.</returns>
        [HttpGet("details/{golferPackageId}")] public async Task<IActionResult> GolferPackageAsync(string golferPackageId)
        {
            var result = await golferPackageService.GolferPackageAsync(golferPackageId, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new golfer packages.
        /// </summary>
        /// <param name="dto">The DTO containing the golfer packages data.</param>
        /// <returns>An IActionResult indicating the result of the create operation.</returns>
        [HttpPut]
        public async Task<IActionResult> CreateGolferPackageAsync([FromBody] GolferPackageDto dto)
        {
            var newPackage = await golferPackageService.CreateGolferPackageAsync(dto, HttpContext.RequestAborted);
            return Ok(newPackage);
        }

        /// <summary>
        /// Update a new golfer packages.
        /// </summary>
        /// <param name="dto">The DTO containing the golfer packages data.</param>
        /// <returns>An IActionResult indicating the result of the update operation.</returns>
        [HttpPost]
        public async Task<IActionResult> UpdateGolferPackageAsync([FromBody] GolferPackageDto dto)
        {
            var newPackage = await golferPackageService.UpdateGolferPackageAsync(dto, HttpContext.RequestAborted);
            return Ok(newPackage);
        }

        /// <summary>
        /// Deletes a golfer packages by its unique identifier.
        /// </summary>
        /// <param name="golferPackageId">The ID of the golfer packages to delete.</param>
        /// <returns>An IActionResult indicating the result of the delete operation.</returns>
        [HttpDelete("{golferPackageId}")]
        public async Task<IActionResult> RemoveGolferPackageAsync(string golferPackageId)
        {
            var result = await golferPackageService.RemoveGolferPackageAsync(golferPackageId, HttpContext.RequestAborted);
            return Ok(result);
        }
    }
}
