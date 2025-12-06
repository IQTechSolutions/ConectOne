using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;

namespace AccomodationModule.Infrastructure.Implimentation
{
    /// <summary>
    /// Provides operations for managing restaurants, including retrieving, creating, updating, and deleting restaurant
    /// data.
    /// </summary>
    /// <remarks>This service interacts with repositories to perform CRUD operations on restaurant-related
    /// data. It supports asynchronous operations and includes methods for retrieving all restaurants in a specific
    /// area, fetching details of a single restaurant, creating new restaurants, updating existing ones, and removing
    /// restaurants.</remarks>
    /// <param name="areaRepo"></param>
    public class RestaurantService(IRepository<Restaurant, string> restaurantRepo) : IRestaurantService
    {
        /// <summary>
        /// Retrieves a collection of all restaurants.
        /// </summary>
        /// <remarks>This method retrieves all restaurants from the repository and maps them to <see
        /// cref="RestaurantDto"/> objects. If the operation fails, the result will include the failure
        /// messages.</remarks>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// where <c>T</c> is an <see cref="IEnumerable{T}"/> of <see cref="RestaurantDto"/> objects. The result
        /// indicates whether the operation succeeded and, if successful, includes the collection of restaurants.</returns>
        public async Task<IBaseResult<IEnumerable<RestaurantDto>>> AllRestaurantsAsync(CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Restaurant>(c => true);

            var result = await restaurantRepo.ListAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<IEnumerable<RestaurantDto>>.FailAsync(result.Messages);

            return await Result<IEnumerable<RestaurantDto>>.SuccessAsync(result.Data.Select(c => new RestaurantDto(c)));
        }

        /// <summary>
        /// Retrieves information about a restaurant based on its unique identifier.
        /// </summary>
        /// <remarks>This method queries the repository for a restaurant matching the specified
        /// identifier. If the restaurant is found, its details are returned as a <see cref="RestaurantDto"/>. If the
        /// restaurant is not found or an error occurs, the result will contain failure messages.</remarks>
        /// <param name="restaurantId">The unique identifier of the restaurant to retrieve. This value cannot be null or empty.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. Defaults to <see cref="CancellationToken.None"/> if not
        /// provided.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the restaurant details encapsulated in a <see cref="RestaurantDto"/> if the operation succeeds,
        /// or error messages if the operation fails.</returns>
        public async Task<IBaseResult<RestaurantDto>> RestaurantAsync(string restaurantId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Restaurant>(c => c.Id == restaurantId);

            var result = await restaurantRepo.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<RestaurantDto>.FailAsync(result.Messages);
            return await Result<RestaurantDto>.SuccessAsync(new RestaurantDto(result.Data));
        }

        /// <summary>
        /// Creates a new restaurant asynchronously based on the provided model.
        /// </summary>
        /// <param name="model">The data transfer object containing the details of the restaurant to be created.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object, where <c>T</c> is <see cref="RestaurantDto"/>. If the operation succeeds, the result contains the
        /// created restaurant's details; otherwise, it contains error messages.</returns>
        public async Task<IBaseResult<RestaurantDto>> CreateRestaurantAsync(RestaurantDto model, CancellationToken cancellationToken = default)
        {
			var result = await restaurantRepo.CreateAsync(new Restaurant(model), cancellationToken);
            if(!result.Succeeded) return await Result<RestaurantDto>.FailAsync(result.Messages);

            var saveResult = await restaurantRepo.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result<RestaurantDto>.FailAsync(saveResult.Messages);

            return await Result<RestaurantDto>.SuccessAsync(new RestaurantDto(result.Data));
        }

        /// <summary>
        /// Updates the details of an existing restaurant in the database.
        /// </summary>
        /// <remarks>This method performs the following operations: <list type="number"> <item>
        /// <description>Validates that a restaurant with the specified <c>Id</c> exists in the database.</description>
        /// </item> <item> <description>Updates the restaurant's properties with the values provided in the <paramref
        /// name="package"/>.</description> </item> <item> <description>Saves the changes to the database.</description>
        /// </item> </list> If no restaurant with the specified <c>Id</c> is found, the method returns a failure
        /// result.</remarks>
        /// <param name="package">The <see cref="RestaurantDto"/> containing the updated restaurant information. The <c>Id</c> property must
        /// match the ID of an existing restaurant.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="IBaseResult{RestaurantDto}"/>: <list type="bullet"> <item> <description> If the update is successful,
        /// the result contains the updated <see cref="RestaurantDto"/>. </description> </item> <item> <description> If
        /// the update fails, the result contains error messages describing the failure. </description> </item> </list></returns>
        public async Task<IBaseResult<RestaurantDto>> UpdateRestaurantAsync(RestaurantDto package, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Restaurant>(c => c.Id == package.Id);
            var result = await restaurantRepo.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<RestaurantDto>.FailAsync(result.Messages);

            var response = result.Data;
            if (response == null)
                return await Result<RestaurantDto>.FailAsync($"No restaurant with id matching '{package.Id}' was found in the database");

            response.Name = package.Name;
            response.Comments = package.Comments;

            restaurantRepo.Update(response);
            var saveResult = await restaurantRepo.SaveAsync(cancellationToken);
            if (saveResult.Succeeded)
                return await Result<RestaurantDto>.SuccessAsync(new RestaurantDto(response));
            return await Result<RestaurantDto>.FailAsync(saveResult.Messages);
        }

        /// <summary>
        /// Removes a restaurant from the system asynchronously.
        /// </summary>
        /// <remarks>This method attempts to remove the specified restaurant by its identifier. If the
        /// operation fails, the returned result will include error messages describing the failure.</remarks>
        /// <param name="restaurantId">The unique identifier of the restaurant to be removed. Must not be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation. If successful, the result includes a success message.</returns>
        public async Task<IBaseResult> RemoveRestaurantAsync(string restaurantId, CancellationToken cancellationToken = default)
        {
            var result = await restaurantRepo.DeleteAsync(restaurantId, cancellationToken);
            if (!result.Succeeded) return await Result.FailAsync(result.Messages);
            return await Result.SuccessAsync($"Package was successfully removed");
        }
	}
}
