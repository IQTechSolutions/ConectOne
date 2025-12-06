using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccomodationModule.Infrastructure.Controllers
{
    /// <summary>
    /// Provides endpoints for managing average temperature data.
    /// </summary>
    /// <remarks>This controller exposes API methods for retrieving, creating, updating, and deleting average
    /// temperature records. It interacts with the <see cref="IAverageTemperatureService"/> to perform operations on
    /// average temperature data.</remarks>
    /// <param name="averageTemperatureService"></param>
    [Route("api/averagetemperatures"),ApiController]
    public class AverageTemperaturesController(IAverageTemperatureService averageTemperatureService) : ControllerBase
    {
        /// <summary>
        /// Retrieves the average temperatures for all locations within the specified area.
        /// </summary>
        /// <remarks>This method calls the underlying service to fetch the average temperatures for all
        /// locations within the specified area. Ensure that <paramref name="areaId"/> corresponds to a valid
        /// area.</remarks>
        /// <param name="areaId">The unique identifier of the area for which average temperatures are requested. Must not be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> containing the average temperatures for the specified area. The result is
        /// returned as an HTTP 200 response with the data, or an appropriate error response if the request fails.</returns>
        [HttpGet("areas/{areaId}")] public async Task<IActionResult> AllAverageTemperatures(string areaId)
        {
            var result = await averageTemperatureService.AllAverageTempraturesAsync(areaId);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves the average temperature data associated with the specified identifier.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch the average temperature data 
        /// using the provided identifier. The result is returned as an HTTP response.</remarks>
        /// <param name="averageTemperatureId">The unique identifier for the average temperature data to retrieve.  This parameter cannot be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> containing the average temperature data if found,  or an appropriate HTTP
        /// response if the data is not available.</returns>
        [HttpGet("{averageTemperatureId}")] public async Task<IActionResult> AverageTemperature(string averageTemperatureId)
        {
            var result = await averageTemperatureService.AverageTempratureAsync(averageTemperatureId);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new average temperature record.
        /// </summary>
        /// <remarks>This method processes the provided average temperature data and stores it using the 
        /// underlying service. The response includes the created record.</remarks>
        /// <param name="averageTemperature">The data transfer object containing the average temperature details to be created. This parameter must not
        /// be null.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.  Typically, this will be an HTTP 200
        /// response containing the created record.</returns>
        [HttpPut] public async Task<IActionResult> CreateAverageTemperature([FromBody] AverageTemperatureDto averageTemperature)
        {
            var result = await averageTemperatureService.CreateAverageTempratureAsync(averageTemperature);
            return Ok(result);
        }

        /// <summary>
        /// Updates the average temperature data using the provided information.
        /// </summary>
        /// <remarks>This method processes a POST request to update average temperature data. Ensure that
        /// the <paramref name="averageTemperature"/> object contains valid data before calling this method.</remarks>
        /// <param name="averageTemperature">The data transfer object containing the updated average temperature information. This parameter must not be
        /// <see langword="null"/>.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the update operation. Typically, this will be an
        /// HTTP 200 response containing the result of the update.</returns>
        [HttpPost] public async Task<IActionResult> EditAverageTemperature([FromBody] AverageTemperatureDto averageTemperature)
        {
            var result = await averageTemperatureService.UpdateAverageTempratureAsync(averageTemperature);
            return Ok(result);
        }

        /// <summary>
        /// Deletes the average temperature record associated with the specified identifier.
        /// </summary>
        /// <remarks>This method performs an HTTP DELETE operation to remove the specified average
        /// temperature record. Ensure that the identifier provided corresponds to an existing record.</remarks>
        /// <param name="averageTemperatureId">The unique identifier of the average temperature record to delete.  This parameter cannot be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.  Typically, a success response is
        /// returned if the record is deleted successfully.</returns>
        [HttpDelete("{packageId}")] public async Task<IActionResult> DeleteAverageTemperature(string averageTemperatureId)
        {
            var result = await averageTemperatureService.RemoveAverageTempratureAsync(averageTemperatureId);
            return Ok(result);
        }
    }
}
