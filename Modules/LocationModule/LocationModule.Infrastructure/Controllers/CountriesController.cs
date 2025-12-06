using LocationModule.Domain.DataTransferObjects;
using LocationModule.Domain.Interfaces;
using LocationModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Mvc;

namespace LocationModule.Infrastructure.Controllers
{
    /// <summary>
    /// Provides endpoints for managing countries.
    /// </summary>
    [Route("api/countries"), ApiController]
    public class CountriesController(ICountryService countryService) : ControllerBase
    {
        /// <summary>
        /// Retrieves a paginated list of countries.
        /// </summary>
        [HttpGet] public async Task<IActionResult> PagedCountries([FromQuery] CountryPageParameters pageParameters)
        {
            var result = await countryService.PagedCountriesAsync(pageParameters);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a list of all countries.
        /// </summary>
        /// <remarks>This method returns a collection of countries as provided by the underlying data
        /// source. The result is returned in an HTTP 200 OK response.</remarks>
        /// <returns>An <see cref="IActionResult"/> containing the list of all countries.</returns>
        [HttpGet("all")] public async Task<IActionResult> AllCountries()
        {
            var result = await countryService.AllCountriesAsync();
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a specific country by its id.
        /// </summary>
        [HttpGet("{countryId}")] public async Task<IActionResult> Country(string countryId)
        {
            var result = await countryService.CountryAsync(countryId);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new country.
        /// </summary>
        [HttpPut] public async Task<IActionResult> CreateCountry([FromBody] CountryDto country)
        {
            var result = await countryService.CreateCountryAsync(country);
            return Ok(result);
        }

        /// <summary>
        /// Updates an existing country.
        /// </summary>
        [HttpPost] public async Task<IActionResult> EditCountry([FromBody] CountryDto country)
        {
            var result = await countryService.UpdateCountryAsync(country);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a country by id.
        /// </summary>
        [HttpDelete("{countryId}")] public async Task<IActionResult> DeleteCountry(string countryId)
        {
            var result = await countryService.RemoveCountryAsync(countryId);
            return Ok(result);
        }
    }
}

