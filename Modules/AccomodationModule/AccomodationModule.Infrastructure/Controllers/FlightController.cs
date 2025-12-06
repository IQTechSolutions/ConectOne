using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccomodationModule.Infrastructure.Controllers
{
    /// <summary>
    /// API controller for managing vacation entities.
    /// </summary>
    [Route("api/vacations/flights"), ApiController]
    public class FlightController(IFlightService flightService) : ControllerBase
    {
        /// <summary>
        /// Retrieves all flights for a specific vacation.
        /// </summary>
        /// <param name="vacationId">The ID of the vacation to retrieve flight for.</param>
        /// <returns>An IActionResult containing the vacation flight.</returns>
        [HttpGet("{vacationId}")] public async Task<IActionResult> FlightsAsync(string vacationId)
        {
            var result = await flightService.FlightListAsync(vacationId, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Get a vacation flight.
        /// </summary>
        /// <param name="flightId">The ID of the vacation interval to delete.</param>
        [HttpGet("details/{flightId}")] public async Task<IActionResult> FlightAsync(string flightId)
        {
            var result = await flightService.FlightAsync(flightId, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new vacation flight.
        /// </summary>
        /// <param name="dto">The DTO containing the vacation flight data.</param>
        /// <returns>An IActionResult indicating the result of the create operation.</returns>
        [HttpPut] public async Task<IActionResult> CreateFlightAsync([FromBody] FlightDto dto)
        {
            var newFlight = await flightService.CreateFlightAsync(dto, HttpContext.RequestAborted);
            return Ok(newFlight);
        }

        /// <summary>
        /// Update a new vacation flight.
        /// </summary>
        /// <param name="dto">The DTO containing the vacation flight data.</param>
        /// <returns>An IActionResult indicating the result of the update operation.</returns>
        [HttpPost] public async Task<IActionResult> UpdateFlightAsync([FromBody] FlightDto dto)
        {
            var newFlight = await flightService.UpdateFlightAsync(dto, HttpContext.RequestAborted);
            return Ok(newFlight);
        }

        /// <summary>
        /// Deletes a vacation flight by its unique identifier.
        /// </summary>
        /// <param name="flightId">The ID of the vacation flight to delete.</param>
        /// <returns>An IActionResult indicating the result of the delete operation.</returns>
        [HttpDelete("{flightId}")] public async Task<IActionResult> RemoveFlightAsync(string flightId)
        {
            var result = await flightService.RemoveFlightAsync(flightId, HttpContext.RequestAborted);
            return Ok(result);
        }
    }
}
