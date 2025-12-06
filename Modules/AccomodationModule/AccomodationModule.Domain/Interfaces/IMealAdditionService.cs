using AccomodationModule.Domain.DataTransferObjects;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Domain.Interfaces
{
    /// <summary>
    /// Implements the meal addition service, providing methods to manage meal addition data.
    /// </summary>
    public interface IMealAdditionService
    {
        /// <summary>
        /// Retrieves a collection of meal additions associated with the specified vacation.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch meal additions for a given
        /// vacation. The returned collection may be empty if no meal additions are associated with the specified
        /// vacation.</remarks>
        /// <param name="vacationId">The unique identifier of the vacation for which meal additions are requested. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional; defaults to <see langword="default"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// with a collection of <see cref="MealAdditionDto"/> objects representing the meal additions for the specified
        /// vacation.</returns>
        Task<IBaseResult<IEnumerable<MealAdditionDto>>> MealAdditionsAsync(string vacationId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves detailed information about a meal addition based on its unique identifier.
        /// </summary>
        /// <remarks>Use this method to fetch information about a specific meal addition, such as its
        /// name, description, or associated metadata. Ensure that the <paramref name="mealAdditionId"/> is valid and
        /// corresponds to an existing meal addition.</remarks>
        /// <param name="mealAdditionId">The unique identifier of the meal addition to retrieve. This value cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see langword="default"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="IBaseResult{MealAdditionDto}"/> object with the details of the requested meal addition.</returns>
        Task<IBaseResult<MealAdditionDto>> MealAdditionAsync(string mealAdditionId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new meal addition asynchronously.
        /// </summary>
        /// <remarks>Use this method to add a new meal addition to the system. Ensure that the <paramref
        /// name="dto"/>  contains valid data before calling this method.</remarks>
        /// <param name="dto">The data transfer object containing the details of the meal addition to be created. Must not be <see
        /// langword="null"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        Task<IBaseResult> CreateMealAdditionAsync(MealAdditionDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the details of a meal addition asynchronously.
        /// </summary>
        /// <remarks>Use this method to update the properties of an existing meal addition. Ensure that
        /// the provided <paramref name="dto"/> contains valid data for the update operation.</remarks>
        /// <param name="dto">The data transfer object containing the updated meal addition details. Cannot be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/> if not
        /// provided.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the update operation.</returns>
        Task<IBaseResult> UpdateMealAdditionAsync(MealAdditionDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a meal addition identified by the specified ID.
        /// </summary>
        /// <remarks>Use this method to remove a specific meal addition from the system. Ensure that the
        /// provided <paramref name="mealAdditionId"/> corresponds to a valid meal addition.</remarks>
        /// <param name="mealAdditionId">The unique identifier of the meal addition to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional; defaults to <see langword="default"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
        Task<IBaseResult> RemoveMealAdditionAsync(string mealAdditionId, CancellationToken cancellationToken = default);
    }
}
