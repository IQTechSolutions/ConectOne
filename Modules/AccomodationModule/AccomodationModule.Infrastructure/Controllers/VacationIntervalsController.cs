using AccomodationModule.Domain.Arguments;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using AccomodationModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Mvc;

namespace AccomodationModule.Infrastructure.Controllers
{
    /// <summary>
    /// API controller for managing vacation entities.
    /// </summary>
    [Route("api/vacations/vacationIntervals"), ApiController]
    public class VacationIntervalsController(IVacationIntervalService vacationIntervalService) : ControllerBase
    {
        /// <summary>
        /// Retrieves all vacation interval for a specific vacation.
        /// </summary>
        /// <param name="args">The result filter params.</param>
        /// <returns>An IActionResult containing the vacation interval.</returns>
        [HttpGet] public async Task<IActionResult> VacationIntervalsAsync([FromBody] VacationIntervalPageParameters args)
        {
            var result = await vacationIntervalService.VacationIntervalListAsync(args, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Get a vacation price
        /// </summary>
        /// <param name="vacationIntervalId">The identity of the vacation interval to retrieve</param>
        /// <returns>A result indicating the success of the operation.</returns>
        [HttpGet("{vacationIntervalId}")] public async Task<IActionResult> VacationIntervalAsync(string vacationIntervalId)
        {
            var result = await vacationIntervalService.VacationIntervalAsync(vacationIntervalId, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new vacation interval.
        /// </summary>
        /// <param name="dto">The DTO containing the vacation interval data.</param>
        /// <returns>An IActionResult indicating the result of the create operation.</returns>
        [HttpPut] public async Task<IActionResult> CreateVacationIntervalAsync([FromBody] VacationIntervalDto dto)
        {
            var newPackage = await vacationIntervalService.CreateVacationIntervalAsync(dto, HttpContext.RequestAborted);
            return Ok(newPackage);
        }

        /// <summary>
        /// Update a new vacation interval.
        /// </summary>
        /// <param name="dto">The DTO containing the vacation interval data.</param>
        /// <returns>An IActionResult indicating the result of the update operation.</returns>
        [HttpPost] public async Task<IActionResult> UpdateVacationIntervalAsync([FromBody] VacationIntervalDto dto)
        {
            var newPackage = await vacationIntervalService.UpdateVacationIntervalAsync(dto, HttpContext.RequestAborted);
            return Ok(newPackage);
        }

        /// <summary>
        /// Deletes a vacation interval by its unique identifier.
        /// </summary>
        /// <param name="vacationIntervalId">The ID of the vacation interval to delete.</param>
        /// <returns>An IActionResult indicating the result of the delete operation.</returns>
        [HttpDelete("{vacationIntervalId}")] public async Task<IActionResult> RemoveVacationIntervalAsync(string vacationIntervalId)
        {
            var result = await vacationIntervalService.RemoveVacationIntervalAsync(vacationIntervalId, HttpContext.RequestAborted);
            return Ok(result);
        }
    }
}
