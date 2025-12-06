using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;

namespace AccomodationModule.Infrastructure.Implimentation
{
    /// <summary>
    /// Implements the meal addition service, providing methods to manage meal addition data.
    /// </summary>
    public class MealAdditionService(IAccomodationRepositoryManager accomodationRepositoryManager) : IMealAdditionService
    {
        /// <summary>
        /// Retrieves a collection of meal additions associated with the specified vacation.
        /// </summary>
        /// <remarks>This method queries the underlying data source to retrieve meal additions associated
        /// with the specified vacation. The result indicates whether the operation was successful and provides either
        /// the data or error messages.</remarks>
        /// <param name="vacationId">The unique identifier of the vacation for which meal additions are to be retrieved.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>An asynchronous operation that returns a result containing a collection of <see cref="MealAdditionDto"/>
        /// objects if the operation succeeds, or failure messages if the operation fails.</returns>
        public async Task<IBaseResult<IEnumerable<MealAdditionDto>>> MealAdditionsAsync(string vacationId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<MealAddition>(c => c.VacationId == vacationId);

            var mealAdditionResult = await accomodationRepositoryManager.MealAdditions.ListAsync(spec, false, cancellationToken);

            // If successful, return the vacation pricing items; otherwise, return failure
            if (mealAdditionResult.Succeeded)
                return await Result<IEnumerable<MealAdditionDto>>.SuccessAsync(mealAdditionResult.Data.Select(c => new MealAdditionDto(c)));
            return await Result<IEnumerable<MealAdditionDto>>.FailAsync(mealAdditionResult.Messages);
        }

        /// <summary>
        /// Retrieves a meal addition by its unique identifier.
        /// </summary>
        /// <remarks>This method queries the repository for a meal addition matching the specified
        /// identifier. If the operation succeeds, the meal addition details are returned as a <see
        /// cref="MealAdditionDto"/>. If the operation fails, the result will include error messages describing the
        /// failure.</remarks>
        /// <param name="mealAdditionId">The unique identifier of the meal addition to retrieve. This value cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="IBaseResult{MealAdditionDto}"/> indicating the success or failure of the operation. If successful, the
        /// result includes the meal addition details; otherwise, it contains error messages.</returns>
        public async Task<IBaseResult<MealAdditionDto>> MealAdditionAsync(string mealAdditionId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<MealAddition>(c => c.Id == mealAdditionId);
            var mealAdditionResult = await accomodationRepositoryManager.MealAdditions.FirstOrDefaultAsync(spec, false, cancellationToken);

            // If successful, return the meal addition items; otherwise, return failure
            if (mealAdditionResult.Succeeded)
                return await Result<MealAdditionDto>.SuccessAsync(new MealAdditionDto(mealAdditionResult.Data!));
            return await Result<MealAdditionDto>.FailAsync(mealAdditionResult.Messages);
        }

        /// <summary>
        /// Creates a new meal addition asynchronously.
        /// </summary>
        /// <remarks>This method converts the provided <paramref name="dto"/> into a meal addition entity,
        /// saves it to the repository, and returns the result of the operation. Ensure that the <paramref name="dto"/>
        /// contains valid data before calling this method.</remarks>
        /// <param name="dto">The data transfer object containing the details of the meal addition to be created.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. Defaults to <see cref="CancellationToken.None"/> if not
        /// provided.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation. If successful, the result includes a success message; otherwise, it
        /// contains error messages.</returns>
        public async Task<IBaseResult> CreateMealAdditionAsync(MealAdditionDto dto, CancellationToken cancellationToken = default)
        {
            // Convert the DTO to a Meal Addition entity
            var mealAddition = dto.ToMealAddition();

            // Create the meal addition
            await accomodationRepositoryManager.MealAdditions.CreateAsync(mealAddition, cancellationToken);

            // Save the changes
            var saveResult = await accomodationRepositoryManager.MealAdditions.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            // Return success message
            return await Result.SuccessAsync($"Meal Addition was created successfully");
        }

        /// <summary>
        /// Updates an existing meal addition with the provided data.
        /// </summary>
        /// <remarks>This method performs the following steps: <list type="number"> <item>Validates that
        /// the meal addition specified by <paramref name="dto"/> exists.</item> <item>Updates the meal addition with
        /// the values provided in <paramref name="dto"/>.</item> <item>Saves the changes to the repository.</item>
        /// </list> If any step fails, the method returns a failure result with the corresponding error
        /// messages.</remarks>
        /// <param name="dto">The data transfer object containing the updated values for the meal addition. The <see
        /// cref="MealAdditionDto.MealAdditionId"/> property must correspond to an existing meal addition.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the update. If successful, the result includes a success message. If
        /// unsuccessful, the result contains error messages.</returns>
        public async Task<IBaseResult> UpdateMealAdditionAsync(MealAdditionDto dto, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<MealAddition>(c => c.Id == dto.MealAdditionId);

            var mealAdditionResult = await accomodationRepositoryManager.MealAdditions.FirstOrDefaultAsync(spec, true, cancellationToken);
            if (!mealAdditionResult.Succeeded) return await Result.FailAsync(mealAdditionResult.Messages);

            dto.UpdateMealAdditionValues(mealAdditionResult.Data!);

            var updateResult = accomodationRepositoryManager.MealAdditions.Update(mealAdditionResult.Data);
            if (!updateResult.Succeeded) return await Result.FailAsync(updateResult.Messages);

            var saveResult = await accomodationRepositoryManager.MealAdditions.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync($"Meal Addition updated successfully");
        }

        /// <summary>
        /// Removes a meal addition identified by the specified ID.
        /// </summary>
        /// <remarks>This method attempts to remove the specified meal addition from the repository. If
        /// the operation fails, the returned result will include error messages detailing the issue.</remarks>
        /// <param name="mealAdditionId">The unique identifier of the meal addition to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>An <see cref="IBaseResult"/> indicating the outcome of the operation. If successful, the result contains a
        /// success message; otherwise, it contains error messages describing the failure.</returns>
        public async Task<IBaseResult> RemoveMealAdditionAsync(string mealAdditionId, CancellationToken cancellationToken = default)
        {
            var result = await accomodationRepositoryManager.MealAdditions.DeleteAsync(mealAdditionId, cancellationToken);
            if (!result.Succeeded) await Result.FailAsync(result.Messages);

            var saveResult = await accomodationRepositoryManager.MealAdditions.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            // Return success message
            return await Result.SuccessAsync($"Meal Addition with id '{mealAdditionId}' was successfully removed");
        }
    }
}
