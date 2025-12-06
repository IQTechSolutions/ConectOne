using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccomodationModule.Infrastructure.Controllers
{
    /// <summary>
    /// API controller for managing vacation entities.
    /// </summary>
    [Route("api/vacations/dayToursActivities"), ApiController]
    public class DayTourActivityController(IDayTourActivityService dayTourActivityService) : ControllerBase
    {
        /// <summary>
        /// Retrieves all vacation prices for a specific vacation.
        /// </summary>
        /// <param name="vacationId">The ID of the vacation to retrieve prices for.</param>
        /// <returns>An IActionResult containing the vacation prices.</returns>
        [HttpGet("{vacationId}")] public async Task<IActionResult> DayTourActivitiesAsync(string vacationId)
        {
            var result = await dayTourActivityService.DayTourActivityListAsync(vacationId, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Get a day tour activity by its unique identifier.
        /// </summary>
        /// <param name="dayTourActivityId">The identity of the day tour activity to retrieve</param>
        /// <returns>A result indicating the success of the operation.</returns>
        [HttpGet("details/{dayTourActivityId}")] public async Task<IActionResult> DayTourActivityAsync(string dayTourActivityId)
        {
            var result = await dayTourActivityService.DayTourActivityAsync(dayTourActivityId, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Creates a day tour activity.
        /// </summary>
        /// <param name="dto">The DTO containing the day tour activity data.</param>
        /// <returns>An IActionResult indicating the result of the create operation.</returns>
        [HttpPut] public async Task<IActionResult> CreateDayTourActivityAsync([FromBody] DayTourActivityDto dto)
        {
            var newPackage = await dayTourActivityService.CreateDayTourActivityAsync(dto, HttpContext.RequestAborted);
            return Ok(newPackage);
        }

        /// <summary>
        /// Update a new day tour activity.
        /// </summary>
        /// <param name="dto">The DTO containing the day tour activity data.</param>
        /// <returns>An IActionResult indicating the result of the update operation.</returns>
        [HttpPost] public async Task<IActionResult> UpdateDayTourActivityAsync([FromBody] DayTourActivityDto dto)
        {
            var newPackage = await dayTourActivityService.UpdateDayTourActivityAsync(dto, HttpContext.RequestAborted);
            return Ok(newPackage);
        }

        /// <summary>
        /// Deletes a day tour activity by its unique identifier.
        /// </summary>
        /// <param name="dayTourActivityId">The ID of the day tour activity to delete.</param>
        /// <returns>An IActionResult indicating the result of the delete operation.</returns>
        [HttpDelete("{dayTourActivityId}")] public async Task<IActionResult> RemoveDayTourActivityAsync(string dayTourActivityId)
        {
            var result = await dayTourActivityService.RemoveDayTourActivityAsync(dayTourActivityId, HttpContext.RequestAborted);
            return Ok(result);
        }
    }
}
