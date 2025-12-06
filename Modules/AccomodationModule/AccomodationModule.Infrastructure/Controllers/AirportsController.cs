using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccomodationModule.Infrastructure.Controllers
{
    /// <summary>
    /// Provides endpoints for managing airports.
    /// </summary>
    [Route("api/airports"), ApiController]
    public class AirportsController(IAirportService airportService) : ControllerBase
    {
        /// <summary>
        /// Retrieves all airports.
        /// </summary>
        [HttpGet] public async Task<IActionResult> AllAirports()
        {
            var result = await airportService.AllAirportsAsync();
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a specific airport by its id.
        /// </summary>
        [HttpGet("{airportId}")] public async Task<IActionResult> Airport(string airportId)
        {
            var result = await airportService.AirportAsync(airportId);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new airport.
        /// </summary>
        [HttpPut] public async Task<IActionResult> CreateAirport([FromBody] AirportDto airport)
        {
            var result = await airportService.CreateAirportAsync(airport);
            return Ok(result);
        }

        /// <summary>
        /// Updates an existing airport.
        /// </summary>
        [HttpPost] public async Task<IActionResult> EditAirport([FromBody] AirportDto airport)
        {
            var result = await airportService.UpdateAirportAsync(airport);
            return Ok(result);
        }

        /// <summary>
        /// Deletes an airport by id.
        /// </summary>
        [HttpDelete("{airportId}")] public async Task<IActionResult> DeleteAirport(string airportId)
        {
            var result = await airportService.RemoveAirportAsync(airportId);
            return Ok(result);
        }
    }
}
