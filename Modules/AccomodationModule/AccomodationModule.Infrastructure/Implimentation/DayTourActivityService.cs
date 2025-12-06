using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;

namespace AccomodationModule.Infrastructure.Implimentation
{
    /// <summary>
    /// Implements the day tour activity service, providing methods to manage day tour activity data.
    /// </summary>
    public class DayTourActivityService(IAccomodationRepositoryManager accomodationRepositoryManager) : IDayTourActivityService
    {
        /// <summary>
        /// Retrieves a list of day tour activities associated with the specified vacation identifier.
        /// </summary>
        /// <remarks>This method queries the data source for day tour activities associated with the
        /// specified vacation  identifier. The result includes only activities that match the provided identifier. If
        /// no activities  are found, the result will indicate failure with appropriate messages.</remarks>
        /// <param name="vacationId">The unique identifier of the vacation for which to retrieve day tour activities.  This parameter cannot be
        /// null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult"/>
        /// where <c>T</c> is an <see cref="IEnumerable{T}"/> of  <see cref="DayTourActivityDto"/> objects. If the
        /// operation succeeds, the result contains the list of  day tour activities; otherwise, it contains error
        /// messages.</returns>
        public async Task<IBaseResult<IEnumerable<DayTourActivityDto>>> DayTourActivityListAsync(string vacationId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<DayTourActivity>(c => c.VacationId! == vacationId);
            var dayTourActivityResult = await accomodationRepositoryManager.DayTourActivities.ListAsync(spec, false, cancellationToken);

            if (dayTourActivityResult.Succeeded)
                return await Result<IEnumerable<DayTourActivityDto>>.SuccessAsync(dayTourActivityResult.Data.Select(c => new DayTourActivityDto(c)));
            return await Result<IEnumerable<DayTourActivityDto>>.FailAsync(dayTourActivityResult.Messages);
        }

        /// <summary>
        /// Retrieves the details of a day tour activity based on the specified activity ID.
        /// </summary>
        /// <remarks>This method queries the repository for a day tour activity matching the specified
        /// <paramref name="activityId"/>. If the activity is found, the result will include a DTO representation of the
        /// activity. If the activity is not found or an error occurs, the result will indicate failure and include
        /// relevant error messages.</remarks>
        /// <param name="activityId">The unique identifier of the day tour activity to retrieve. This value cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that includes the details of the day tour activity as a <see cref="DayTourActivityDto"/> if the
        /// operation succeeds, or error messages if it fails.</returns>
        public async Task<IBaseResult<DayTourActivityDto>> DayTourActivityAsync(string activityId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<DayTourActivity>(c => c.Id == activityId);
            var dayTourActivityResult = await accomodationRepositoryManager.DayTourActivities.FirstOrDefaultAsync(spec, false, cancellationToken);

            if (dayTourActivityResult.Succeeded)
                return await Result<DayTourActivityDto>.SuccessAsync(new DayTourActivityDto(dayTourActivityResult.Data!));
            return await Result<DayTourActivityDto>.FailAsync(dayTourActivityResult.Messages);
        }

        /// <summary>
        /// Creates a new day tour activity asynchronously.
        /// </summary>
        /// <remarks>This method creates a new day tour activity based on the provided <paramref
        /// name="dto"/> and saves it to the underlying data store. If the operation fails, the returned result will
        /// include the relevant error messages.</remarks>
        /// <param name="dto">The data transfer object containing the details of the day tour activity to be created.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation. If successful, the result includes a success message; otherwise, it
        /// contains error messages describing the failure.</returns>
        public async Task<IBaseResult> CreateDayTourActivityAsync(DayTourActivityDto dto, CancellationToken cancellationToken = default)
        {
            var dayTourActivity = dto.ToDayTourActivity();
            await accomodationRepositoryManager.DayTourActivities.CreateAsync(dayTourActivity, cancellationToken);

            // Save the changes
            var saveResult = await accomodationRepositoryManager.DayTourActivities.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            // Return success message
            return await Result.SuccessAsync($"Day Tour Activity was created successfully");
        }

        /// <summary>
        /// Updates the details of a day tour activity asynchronously.
        /// </summary>
        /// <remarks>This method updates the day tour activity identified by the <c>DayTourActivityId</c>
        /// property in the provided <paramref name="dto"/>. If the specified activity does not exist, or if any part of
        /// the update process fails, the method returns a failure result with appropriate error messages.</remarks>
        /// <param name="dto">The data transfer object containing the updated values for the day tour activity.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation, along with any relevant messages.</returns>
        public async Task<IBaseResult> UpdateDayTourActivityAsync(DayTourActivityDto dto, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<DayTourActivity>(c => c.Id == dto.DayTourActivityId);
            var dayTourActivityResult = await accomodationRepositoryManager.DayTourActivities.FirstOrDefaultAsync(spec, true, cancellationToken);
            if (!dayTourActivityResult.Succeeded) return await Result.FailAsync(dayTourActivityResult.Messages);

            dto.UpdateDayTourActivityValues(dayTourActivityResult.Data!);

            var updateResult = accomodationRepositoryManager.DayTourActivities.Update(dayTourActivityResult.Data!);
            if (!updateResult.Succeeded) return await Result.FailAsync(updateResult.Messages);

            var saveResult = await accomodationRepositoryManager.GolferPackages.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync($"Golfer Package updated successfully");
        }

        /// <summary>
        /// Removes a day tour activity with the specified identifier asynchronously.
        /// </summary>
        /// <remarks>This method removes the specified day tour activity and saves the changes to the
        /// underlying data store. If the operation fails at any step, the returned result will contain the failure
        /// messages.</remarks>
        /// <param name="activityId">The unique identifier of the day tour activity to be removed. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation. If successful, the result includes a success message.</returns>
        public async Task<IBaseResult> RemoveDayTourActivityAsync(string activityId, CancellationToken cancellationToken = default)
        {
            // Delete the vacation price
            var result = await accomodationRepositoryManager.DayTourActivities.DeleteAsync(activityId, cancellationToken);
            if (!result.Succeeded) await Result.FailAsync(result.Messages);

            var saveResult = await accomodationRepositoryManager.GolferPackages.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            // Return success message
            return await Result.SuccessAsync($"Day Tour Activity with id '{activityId}' was successfully removed");
        }
    }
}
