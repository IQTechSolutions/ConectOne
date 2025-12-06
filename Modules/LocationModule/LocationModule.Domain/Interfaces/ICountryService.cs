using ConectOne.Domain.ResultWrappers;
using LocationModule.Domain.DataTransferObjects;
using LocationModule.Domain.RequestFeatures;

namespace LocationModule.Domain.Interfaces
{
    /// <summary>
    /// Defines a contract for managing country related operations.
    /// </summary>
    public interface ICountryService
    {
        /// <summary>
        /// Retrieves a paginated list of countries based on the specified parameters.
        /// </summary>
        Task<PaginatedResult<CountryDto>> PagedCountriesAsync(CountryPageParameters pageParameters);

        /// <summary>
        /// Retrieves all countries.
        /// </summary>
        Task<IBaseResult<IEnumerable<CountryDto>>> AllCountriesAsync();

        /// <summary>
        /// Retrieves a specific country by id.
        /// </summary>
        Task<IBaseResult<CountryDto>> CountryAsync(string countryId);

        /// <summary>
        /// Creates a new country.
        /// </summary>
        Task<IBaseResult<int>> CreateCountryAsync(CountryDto dto);

        /// <summary>
        /// Updates an existing country.
        /// </summary>
        Task<IBaseResult> UpdateCountryAsync(CountryDto dto);

        /// <summary>
        /// Removes a country by id.
        /// </summary>
        Task<IBaseResult> RemoveCountryAsync(string countryId);
    }
}

