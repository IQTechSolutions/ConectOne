using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using LocationModule.Domain.DataTransferObjects;
using LocationModule.Domain.Interfaces;
using LocationModule.Domain.RequestFeatures;

namespace LocationModule.Application.RestServices
{
    /// <summary>
    /// Provides methods for interacting with country-related data through a RESTful API.
    /// </summary>
    /// <remarks>This service allows clients to retrieve, create, update, and delete country data, as well as
    /// fetch paginated or complete lists of countries. It relies on an HTTP provider to communicate with the underlying
    /// API.</remarks>
    /// <param name="provider"></param>
    public class CountryRestService(IBaseHttpProvider provider) : ICountryService
    {
        /// <summary>
        /// Retrieves a paginated list of countries based on the specified page parameters.
        /// </summary>
        /// <remarks>This method fetches data from the "countries" endpoint and applies the specified
        /// pagination settings. Ensure that <paramref name="pageParameters"/> contains valid values to avoid unexpected
        /// results.</remarks>
        /// <param name="pageParameters">The parameters that define the pagination settings, such as page number and page size.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a collection of <see cref="CountryDto"/> objects representing
        /// the countries in the requested page, along with pagination metadata.</returns>
        public async Task<PaginatedResult<CountryDto>> PagedCountriesAsync(CountryPageParameters pageParameters)
        {
            var result = await provider.GetPagedAsync<CountryDto, CountryPageParameters>("countries", pageParameters);
            return result;
        }

        /// <summary>
        /// Retrieves a collection of all countries.
        /// </summary>
        /// <remarks>This method asynchronously fetches a list of countries from the underlying data
        /// provider. The returned collection contains country details represented as <see cref="CountryDto"/>
        /// objects.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult{T}"/>
        /// wrapping an <see cref="IEnumerable{T}"/> of <see cref="CountryDto"/>  objects, representing the list of all
        /// countries.</returns>
        public async Task<IBaseResult<IEnumerable<CountryDto>>> AllCountriesAsync()
        {
            var result = await provider.GetAsync<IEnumerable<CountryDto>>("countries/all");
            return result;
        }

        /// <summary>
        /// Retrieves information about a specific country based on the provided country identifier.
        /// </summary>
        /// <param name="countryId">The unique identifier of the country to retrieve. Cannot be <see langword="null"/> or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// object with the country details as a <see cref="CountryDto"/>. If the country is not found, the result may
        /// indicate an error.</returns>
        public async Task<IBaseResult<CountryDto>> CountryAsync(string countryId)
        {
            var result = await provider.GetAsync<CountryDto>($"countries/{countryId}");
            return result;
        }

        /// <summary>
        /// Creates a new country using the provided data transfer object (DTO).
        /// </summary>
        /// <param name="dto">The <see cref="CountryDto"/> containing the details of the country to be created. Cannot be <see
        /// langword="null"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// with the ID of the newly created country.</returns>
        public async Task<IBaseResult<int>> CreateCountryAsync(CountryDto dto)
        {
            var result = await provider.PutAsync<int, CountryDto>($"countries", dto);
            return result;
        }

        /// <summary>
        /// Updates the details of a country asynchronously.
        /// </summary>
        /// <remarks>This method sends the updated country details to the underlying provider for
        /// processing. Ensure that the <paramref name="dto"/> contains valid data before calling this method.</remarks>
        /// <param name="dto">The data transfer object containing the updated country details.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateCountryAsync(CountryDto dto)
        {
            var result = await provider.PostAsync($"countries", dto);
            return result;
        }

        /// <summary>
        /// Removes a country with the specified identifier asynchronously.
        /// </summary>
        /// <remarks>This method sends a delete request to the underlying data provider to remove the
        /// specified country. Ensure that the <paramref name="countryId"/> corresponds to an existing
        /// country.</remarks>
        /// <param name="countryId">The unique identifier of the country to be removed. Cannot be <see langword="null"/> or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveCountryAsync(string countryId)
        {
            var result = await provider.DeleteAsync($"countries", countryId);
            return result;
        }
    }
}
