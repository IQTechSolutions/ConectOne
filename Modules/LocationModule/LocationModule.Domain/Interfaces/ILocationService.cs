using ConectOne.Domain.ResultWrappers;
using LocationModule.Domain.DataTransferObjects;
using LocationModule.Domain.RequestFeatures;

namespace LocationModule.Domain.Interfaces
{
    /// <summary>
    /// Provides methods for retrieving location data, including paginated and full lists of locations.
    /// </summary>
    /// <remarks>This interface defines operations for accessing location information, such as retrieving all
    /// locations or fetching a paginated subset of locations based on specified parameters. Implementations of this
    /// interface are expected to handle data retrieval and any necessary transformations into DTOs.</remarks>
    public interface ILocationService
    {
        /// <summary>
        /// Retrieves a paginated list of locations based on the specified page parameters.
        /// </summary>
        /// <param name="pageParameters">The parameters that define the pagination settings, such as page number and page size.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a collection of <see cref="LocationDto"/> objects and
        /// pagination metadata, such as total count and current page information.</returns>
        Task<PaginatedResult<LocationDto>> PagedLocationsAsync(LocationPageParameters pageParameters);

        /// <summary>
        /// Retrieves a list of all available locations asynchronously.
        /// </summary>
        /// <remarks>The returned result may include metadata or status information as part of the <see
        /// cref="IBaseResult{T}"/>. Callers should check the result's status or error information to ensure the
        /// operation was successful.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// with a list of <see cref="LocationDto"/> objects representing the available locations.</returns>
        Task<IBaseResult<List<LocationDto>>> AllLocationsAsync();
    }
}
