using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Application.RestServices
{
    /// <summary>
    /// Provides methods for managing meal additions associated with vacations.
    /// </summary>
    /// <remarks>This service offers functionality to retrieve, create, update, and delete meal additions for
    /// vacations. It communicates with a RESTful API through the provided <see cref="IBaseHttpProvider"/>.</remarks>
    /// <param name="provider"></param>
    public class MealAdditionRestService(IBaseHttpProvider provider) : IMealAdditionService
    {
        /// <summary>
        /// Retrieves a collection of meal additions associated with the specified vacation.
        /// </summary>
        /// <remarks>This method fetches meal additions for a given vacation by its identifier. The caller
        /// can use the <paramref name="cancellationToken"/> to cancel the operation if needed.</remarks>
        /// <param name="vacationId">The unique identifier of the vacation for which meal additions are requested.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// containing an enumerable collection of <see cref="MealAdditionDto"/> objects representing the meal
        /// additions.</returns>
        public async Task<IBaseResult<IEnumerable<MealAdditionDto>>> MealAdditionsAsync(string vacationId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<MealAdditionDto>>("vacations/mealAdditions");
            return result;
        }

        /// <summary>
        /// Retrieves the details of a meal addition by its unique identifier.
        /// </summary>
        /// <remarks>This method sends a request to retrieve the meal addition details from the provider.
        /// Ensure that the <paramref name="mealAdditionId"/> corresponds to a valid meal addition in the
        /// system.</remarks>
        /// <param name="mealAdditionId">The unique identifier of the meal addition to retrieve. This value cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping the <see cref="MealAdditionDto"/> for the specified meal addition.</returns>
        public async Task<IBaseResult<MealAdditionDto>> MealAdditionAsync(string mealAdditionId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<MealAdditionDto>($"vacations/mealAdditions/{mealAdditionId}");
            return result;
        }

        /// <summary>
        /// Creates or updates a meal addition asynchronously.
        /// </summary>
        /// <remarks>This method sends a PUT request to the "vacations/mealAdditions" endpoint with the
        /// provided data. Ensure that the <paramref name="dto"/> parameter contains valid data before calling this
        /// method.</remarks>
        /// <param name="dto">The data transfer object containing the details of the meal addition to be created or updated.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> CreateMealAdditionAsync(MealAdditionDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync($"vacations/mealAdditions", dto);
            return result;
        }

        /// <summary>
        /// Updates a meal addition asynchronously using the provided data transfer object.
        /// </summary>
        /// <remarks>This method sends a POST request to the "vacations/mealAdditions" endpoint with the
        /// provided data. Ensure that the <paramref name="dto"/> parameter is properly populated before calling this
        /// method.</remarks>
        /// <param name="dto">The <see cref="MealAdditionDto"/> containing the details of the meal addition to be updated.</param>
        /// <param name="cancellationToken">An optional <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateMealAdditionAsync(MealAdditionDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"vacations/mealAdditions", dto);
            return result;
        }

        /// <summary>
        /// Removes a meal addition identified by the specified ID.
        /// </summary>
        /// <remarks>This method sends a request to delete the specified meal addition. Ensure the
        /// provided ID is valid  and corresponds to an existing meal addition.</remarks>
        /// <param name="mealAdditionId">The unique identifier of the meal addition to be removed. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the removal operation.</returns>
        public async Task<IBaseResult> RemoveMealAdditionAsync(string mealAdditionId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"vacations/mealAdditions", mealAdditionId);
            return result;
        }
    }
}
