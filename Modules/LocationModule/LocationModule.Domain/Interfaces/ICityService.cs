using ConectOne.Domain.ResultWrappers;
using LocationModule.Domain.DataTransferObjects;
using LocationModule.Domain.RequestFeatures;

namespace LocationModule.Domain.Interfaces
{
    /// <summary>
    /// Defines a contract for managing city-related operations.
    /// </summary>
    public interface ICityService
    {
        /// <summary>
        /// Retrieves a paginated list of cities.
        /// </summary>
        Task<PaginatedResult<CityDto>> PagedCitiesAsync(CityPageParameters pageParameters);

        /// <summary>
        /// Retrieves all cities.
        /// </summary>
        Task<IBaseResult<IEnumerable<CityDto>>> AllCitiesAsync();

        /// <summary>
        /// Retrieves a specific city by id.
        /// </summary>
        Task<IBaseResult<CityDto>> CityAsync(string cityId);

        /// <summary>
        /// Creates a new city.
        /// </summary>
        Task<IBaseResult<string>> CreateCityAsync(CityDto dto);

        /// <summary>
        /// Updates an existing city.
        /// </summary>
        Task<IBaseResult> UpdateCityAsync(CityDto dto);

        /// <summary>
        /// Removes a city by id.
        /// </summary>
        Task<IBaseResult> RemoveCityAsync(string cityId);
    }
}
