using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Application.RestServices
{
    /// <summary>
    /// Provides RESTful operations for managing restaurants, including retrieving, creating, updating, and deleting
    /// restaurant data.
    /// </summary>
    /// <remarks>This service acts as an abstraction over HTTP operations for interacting with a restaurant
    /// API.  It supports asynchronous operations for retrieving all restaurants, retrieving a specific restaurant by
    /// ID,  creating a new restaurant, updating an existing restaurant, and removing a restaurant.</remarks>
    /// <param name="provider"></param>
    public class RestaurantRestService(IBaseHttpProvider provider) : IRestaurantService
    {
        /// <summary>
        /// Retrieves a collection of all restaurants.
        /// </summary>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// with an enumerable collection of <see cref="RestaurantDto"/> objects representing the restaurants.</returns>
        public Task<IBaseResult<IEnumerable<RestaurantDto>>> AllRestaurantsAsync(CancellationToken cancellationToken = default)
        {
            var result = provider.GetAsync<IEnumerable<RestaurantDto>>("restaurants");
            return result;
        }

        /// <summary>
        /// Retrieves information about a restaurant based on the specified restaurant ID.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch restaurant details from the
        /// underlying data provider. Ensure that the <paramref name="restaurantId"/> is valid and corresponds to an
        /// existing restaurant.</remarks>
        /// <param name="restaurantId">The unique identifier of the restaurant to retrieve. This value cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the restaurant details as a <see cref="RestaurantDto"/>. If the restaurant is not found, the
        /// result may indicate an error or an empty response.</returns>
        public Task<IBaseResult<RestaurantDto>> RestaurantAsync(string restaurantId, CancellationToken cancellationToken = default)
        {
            var result = provider.GetAsync<RestaurantDto>($"restaurants/{restaurantId}");
            return result;
        }

        /// <summary>
        /// Creates a new restaurant using the provided restaurant details.
        /// </summary>
        /// <remarks>This method sends an asynchronous request to create a new restaurant. Ensure that the
        /// <paramref name="model"/> contains valid data before calling this method. The operation may be canceled by
        /// passing a cancellation token.</remarks>
        /// <param name="model">The <see cref="RestaurantDto"/> object containing the details of the restaurant to be created.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation. The default value is <see
        /// cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// where <c>T</c> is <see cref="RestaurantDto"/>, representing the created restaurant details.</returns>
        public Task<IBaseResult<RestaurantDto>> CreateRestaurantAsync(RestaurantDto model, CancellationToken cancellationToken = default)
        {
            var result = provider.PutAsync<RestaurantDto, RestaurantDto>($"restaurants", model);
            return result;
        }

        /// <summary>
        /// Updates the details of an existing restaurant asynchronously.
        /// </summary>
        /// <remarks>This method sends the updated restaurant details to the server and returns the
        /// updated  information as part of the result. Ensure that the provided <paramref name="package"/>  contains
        /// valid data before calling this method.</remarks>
        /// <param name="package">The <see cref="RestaurantDto"/> containing the updated restaurant details.  This parameter cannot be <see
        /// langword="null"/>.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult{T}"/>
        /// of type <see cref="RestaurantDto"/> representing the updated restaurant details.</returns>
        public Task<IBaseResult<RestaurantDto>> UpdateRestaurantAsync(RestaurantDto package, CancellationToken cancellationToken = default)
        {
            var result = provider.PostAsync<RestaurantDto, RestaurantDto>($"restaurants", package);
            return result;
        }

        /// <summary>
        /// Removes a restaurant with the specified identifier asynchronously.
        /// </summary>
        /// <remarks>This method sends a delete request to the underlying provider to remove the specified
        /// restaurant. Ensure that the <paramref name="restaurantId"/> corresponds to an existing restaurant.</remarks>
        /// <param name="restaurantId">The unique identifier of the restaurant to be removed. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public Task<IBaseResult> RemoveRestaurantAsync(string restaurantId, CancellationToken cancellationToken = default)
        {
            var result = provider.DeleteAsync($"restaurants", restaurantId);
            return result;
        }
    }
}
