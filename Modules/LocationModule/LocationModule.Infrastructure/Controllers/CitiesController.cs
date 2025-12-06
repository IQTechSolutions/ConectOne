using LocationModule.Domain.DataTransferObjects;
using LocationModule.Domain.Interfaces;
using LocationModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Mvc;

namespace LocationModule.Infrastructure.Controllers
{
    /// <summary>
    /// Provides endpoints for managing cities.
    /// </summary>
    [Route("api/cities"), ApiController]
    public class CitiesController(ICityService cityService) : ControllerBase
    {
        /// <summary>
        /// Retrieves a paginated list of cities.
        /// </summary>
        [HttpGet] public async Task<IActionResult> PagedCities([FromQuery] CityPageParameters pageParameters)
        {
            var result = await cityService.PagedCitiesAsync(pageParameters);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a list of all cities.
        /// </summary>
        /// <remarks>This method returns all cities available in the system. The result is returned as an
        /// HTTP 200 OK response  containing the list of cities in the response body.</remarks>
        /// <returns>An <see cref="IActionResult"/> containing an HTTP 200 OK response with the list of cities.</returns>
        [HttpGet("all")] public async Task<IActionResult> AllCities()
        {
            var result = await cityService.AllCitiesAsync();
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a specific city by its id.
        /// </summary>
        [HttpGet("{cityId}")] public async Task<IActionResult> City(string cityId)
        {
            var result = await cityService.CityAsync(cityId);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new city.
        /// </summary>
        [HttpPut] public async Task<IActionResult> CreateCity([FromBody] CityDto city)
        {
            var result = await cityService.CreateCityAsync(city);
            return Ok(result);
        }

        /// <summary>
        /// Updates an existing city.
        /// </summary>
        [HttpPost] public async Task<IActionResult> EditCity([FromBody] CityDto city)
        {
            var result = await cityService.UpdateCityAsync(city);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a city by id.
        /// </summary>
        [HttpDelete("{cityId}")] public async Task<IActionResult> DeleteCity(string cityId)
        {
            var result = await cityService.RemoveCityAsync(cityId);
            return Ok(result);
        }
    }
}
