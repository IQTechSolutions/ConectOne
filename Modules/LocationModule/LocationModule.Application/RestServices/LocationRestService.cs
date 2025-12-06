using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using LocationModule.Domain.DataTransferObjects;
using LocationModule.Domain.Interfaces;
using LocationModule.Domain.RequestFeatures;

namespace LocationModule.Application.RestServices
{
    /// <summary>
    /// Provides REST-based operations for retrieving location data using an HTTP provider.
    /// </summary>
    /// <param name="provider">The HTTP provider used to perform REST requests for location data. Cannot be null.</param>
    public class LocationRestService(IBaseHttpProvider provider) : ILocationService
    {
        /// <summary>
        /// Retrieves a paginated list of locations based on the specified paging and filtering parameters.
        /// </summary>
        /// <param name="pageParameters">The parameters that define paging, sorting, and filtering options for the location query. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a paginated result of location
        /// data transfer objects matching the specified criteria.</returns>
        /// <exception cref="NotImplementedException">Thrown in all cases, as this method is not yet implemented.</exception>
        public Task<PaginatedResult<LocationDto>> PagedLocationsAsync(LocationPageParameters pageParameters)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Asynchronously retrieves all available locations.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// holding a list of <see cref="LocationDto"/> objects representing all locations. If no locations are found,
        /// the list will be empty.</returns>
        /// <exception cref="NotImplementedException">Thrown if the method is not implemented.</exception>
        public Task<IBaseResult<List<LocationDto>>> AllLocationsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
