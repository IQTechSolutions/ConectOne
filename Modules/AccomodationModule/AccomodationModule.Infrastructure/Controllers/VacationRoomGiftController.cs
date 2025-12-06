using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using AccomodationModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Mvc;

namespace AccomodationModule.Infrastructure.Controllers
{
    /// <summary>
    /// API controller for managing vacation entities.
    /// </summary>
    [Route("api/vacations/vacationRoomGifts"), ApiController]
    public class VacationRoomGiftController(IVacationRoomGiftService vacationIntervalService) : ControllerBase
    {
        /// <summary>
        /// Retrieves all vacation room gifts for a specific vacation.
        /// </summary>
        /// <param name="args">The result filter params.</param>
        /// <returns>An IActionResult containing the vacation room gifts.</returns>
        [HttpGet] public async Task<IActionResult> VacationRoomGiftsAsync([FromBody] VacationIntervalPageParameters args)
        {
            var result = await vacationIntervalService.VacationRoomGiftListAsync(args, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Get a vacation price
        /// </summary>
        /// <param name="vacationRoomGiftId">The identity of the vacation interval to retrieve</param>
        /// <returns>A result indicating the success of the operation.</returns>
        [HttpGet("{vacationRoomGiftId}")] public async Task<IActionResult> VacationRoomGiftsAsync(string vacationRoomGiftId)
        {
            var result = await vacationIntervalService.VacationRoomGiftAsync(vacationRoomGiftId, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new vacation interval.
        /// </summary>
        /// <param name="dto">The DTO containing the vacation interval data.</param>
        /// <returns>An IActionResult indicating the result of the create operation.</returns>
        [HttpPut] public async Task<IActionResult> CreateVacationRoomGiftsAsync([FromBody] RoomGiftDto dto)
        {
            var newPackage = await vacationIntervalService.CreateVacationRoomGiftAsync(dto, HttpContext.RequestAborted);
            return Ok(newPackage);
        }

        /// <summary>
        /// Update a new vacation interval.
        /// </summary>
        /// <param name="dto">The DTO containing the vacation interval data.</param>
        /// <returns>An IActionResult indicating the result of the update operation.</returns>
        [HttpPost] public async Task<IActionResult> UpdateVacationRoomGiftsAsync([FromBody] RoomGiftDto dto)
        {
            var newPackage = await vacationIntervalService.UpdateVacationRoomGiftAsync(dto, HttpContext.RequestAborted);
            return Ok(newPackage);
        }

        /// <summary>
        /// Deletes a vacation interval by its unique identifier.
        /// </summary>
        /// <param name="vacationRoomGiftId">The ID of the vacation interval to delete.</param>
        /// <returns>An IActionResult indicating the result of the delete operation.</returns>
        [HttpDelete("{vacationRoomGiftId}")] public async Task<IActionResult> RemoveVacationRoomGiftsAsync(string vacationRoomGiftId)
        {
            var result = await vacationIntervalService.RemoveVacationRoomGiftAsync(vacationRoomGiftId, HttpContext.RequestAborted);
            return Ok(result);
        }
    }
}
