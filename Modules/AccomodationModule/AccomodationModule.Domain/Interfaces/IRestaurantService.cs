using AccomodationModule.Domain.DataTransferObjects;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Domain.Interfaces
{
    /// <summary>
    /// Defines a contract for managing restaurant-related operations.
    /// </summary>
    /// <remarks>This interface provides methods for retrieving, creating, updating, and deleting restaurant
    /// data. It is designed to work asynchronously and supports cancellation through <see
    /// cref="CancellationToken"/>.</remarks>
    public interface IRestaurantService
    {
        /// <summary>
        /// Retrieves a collection of restaurants located in the specified area.
        /// </summary>
        /// <remarks>Use this method to retrieve restaurant data for a specific area. The returned result
        /// may include additional metadata or status information encapsulated in the <see cref="IBaseResult"/>
        /// object.</remarks>
        /// <param name="areaId">The identifier of the area for which to retrieve restaurants. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with a collection of <see cref="RestaurantDto"/> objects representing the restaurants in the specified area.</returns>
        Task<IBaseResult<IEnumerable<RestaurantDto>>> AllRestaurantsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves detailed information about a restaurant based on its unique identifier.
        /// </summary>
        /// <remarks>Use this method to fetch detailed information about a specific restaurant, such as
        /// its name, location, and other attributes. Ensure that the <paramref name="restaurantId"/> is valid and
        /// corresponds to an existing restaurant.</remarks>
        /// <param name="restaurantId">The unique identifier of the restaurant to retrieve. This value cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/> if not
        /// provided.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping the <see cref="RestaurantDto"/> with the restaurant's details.</returns>
        Task<IBaseResult<RestaurantDto>> RestaurantAsync(string restaurantId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new restaurant asynchronously.
        /// </summary>
        /// <remarks>Use this method to create a new restaurant in the system. Ensure that the <paramref
        /// name="model"/> parameter contains valid data before calling this method. The operation may be canceled by
        /// passing a cancellation token.</remarks>
        /// <param name="model">The data transfer object containing the details of the restaurant to be created. Cannot be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional; defaults to <see langword="default"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the created restaurant's details if the operation succeeds.</returns>
        Task<IBaseResult<RestaurantDto>> CreateRestaurantAsync(RestaurantDto model, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the details of an existing restaurant.
        /// </summary>
        /// <remarks>Use this method to update the details of a restaurant in the system. Ensure that the
        /// provided  <see cref="RestaurantDto"/> contains valid and complete information for the update.</remarks>
        /// <param name="package">The <see cref="RestaurantDto"/> containing the updated restaurant information. This parameter must not be
        /// <see langword="null"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// object wrapping the updated <see cref="RestaurantDto"/>. If the update fails, the result will indicate the
        /// failure.</returns>
        Task<IBaseResult<RestaurantDto>> UpdateRestaurantAsync(RestaurantDto package, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a restaurant from the system asynchronously.
        /// </summary>
        /// <remarks>This method performs the removal operation asynchronously. Ensure that the provided
        /// <paramref name="restaurantId"/> corresponds to an existing restaurant in the system.</remarks>
        /// <param name="restaurantId">The unique identifier of the restaurant to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional; defaults to <see langword="default"/>.</param>
        /// <returns>An <see cref="IBaseResult"/> indicating the outcome of the operation. The result contains information about
        /// whether the removal was successful and any associated messages or errors.</returns>
        Task<IBaseResult> RemoveRestaurantAsync(string restaurantId, CancellationToken cancellationToken = default);
    }
}