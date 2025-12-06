using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using LocationModule.Domain.DataTransferObjects;
using LocationModule.Domain.Interfaces;
using LocationModule.Domain.RequestFeatures;

namespace LocationModule.Application.RestServices
{
    /// <summary>
    /// Provides RESTful operations for managing city data, including retrieving, creating, updating, and deleting
    /// cities.
    /// </summary>
    /// <remarks>This service acts as an abstraction layer for interacting with city-related data through
    /// HTTP-based APIs.  It supports paginated retrieval of cities, fetching all cities, retrieving a specific city by
    /// its identifier,  and performing CRUD operations such as creating, updating, and removing cities.</remarks>
    /// <param name="provider"></param>
    public class CityRestService(IBaseHttpProvider provider) : ICityService
    {
        /// <summary>
        /// Retrieves a paginated list of cities based on the specified paging parameters.
        /// </summary>
        /// <remarks>This method fetches city data from the underlying data provider using the specified
        /// pagination settings. Ensure that the <paramref name="pageParameters"/> object contains valid values for page
        /// number and page size.</remarks>
        /// <param name="pageParameters">The parameters that define the pagination settings, such as page number and page size.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a collection of <see cref="CityDto"/> objects representing the
        /// cities for the specified page. If no cities are found, the collection will be empty.</returns>
        public async Task<PaginatedResult<CityDto>> PagedCitiesAsync(CityPageParameters pageParameters)
        {
            var result = await provider.GetPagedAsync<CityDto, CityPageParameters>("cities", pageParameters);
            return result;
        }

        /// <summary>
        /// Retrieves a collection of all cities.
        /// </summary>
        /// <remarks>This method asynchronously fetches a list of cities from the underlying data
        /// provider. The returned collection may be empty if no cities are available.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult{T}"/>
        /// where <c>T</c> is an <see cref="IEnumerable{CityDto}"/>  representing the collection of cities.</returns>
        public async Task<IBaseResult<IEnumerable<CityDto>>> AllCitiesAsync()
        {
            var result = await provider.GetAsync<IEnumerable<CityDto>>("cities/all");
            return result;
        }

        /// <summary>
        /// Retrieves information about a city based on the specified city identifier.
        /// </summary>
        /// <remarks>This method performs an asynchronous HTTP GET request to fetch city details. Ensure
        /// that the <paramref name="cityId"/> corresponds to a valid city in the data source.</remarks>
        /// <param name="cityId">The unique identifier of the city to retrieve. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// of type <see cref="CityDto"/> with the city's details, or an error result if the operation fails.</returns>
        public async Task<IBaseResult<CityDto>> CityAsync(string cityId)
        {
            var result = await provider.GetAsync<CityDto>($"cities/{cityId}");
            return result;
        }

        /// <summary>
        /// Creates a new city using the provided data transfer object (DTO).
        /// </summary>
        /// <remarks>This method sends the city data to the underlying provider for creation. Ensure that
        /// the <paramref name="dto"/> contains all required fields.</remarks>
        /// <param name="dto">The data transfer object containing the details of the city to be created. Cannot be <see langword="null"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with the ID of the created city as a string.</returns>
        public async Task<IBaseResult<string>> CreateCityAsync(CityDto dto)
        {
            var result = await provider.PutAsync<string, CityDto>($"cities", dto);
            return result;
        }

        /// <summary>
        /// Updates the details of a city asynchronously.
        /// </summary>
        /// <param name="dto">The data transfer object containing the updated city details. Cannot be <see langword="null"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateCityAsync(CityDto dto)
        {
            var result = await provider.PostAsync($"cities", dto);
            return result;
        }

        /// <summary>
        /// Removes a city with the specified identifier asynchronously.
        /// </summary>
        /// <remarks>This method sends a request to delete the city identified by <paramref
        /// name="cityId"/>. Ensure the identifier  corresponds to an existing city in the system.</remarks>
        /// <param name="cityId">The unique identifier of the city to be removed. Cannot be <see langword="null"/> or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveCityAsync(string cityId)
        {
            var result = await provider.DeleteAsync($"cities", cityId);
            return result;
        }
    }
}
