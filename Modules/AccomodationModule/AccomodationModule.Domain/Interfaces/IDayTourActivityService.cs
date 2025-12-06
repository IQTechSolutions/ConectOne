using AccomodationModule.Domain.DataTransferObjects;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Domain.Interfaces
{
    /// <summary>
    /// Implements the day tour activity service, providing methods to manage day tour activity data.
    /// </summary>
    public interface IDayTourActivityService
    {
        /// <summary>
        /// Retrieves a list of day tour activities associated with the specified vacation.
        /// </summary>
        /// <param name="vacationId">The unique identifier of the vacation for which day tour activities are to be retrieved. Must not be null or
        /// empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// with an enumerable collection of <see cref="DayTourActivityDto"/> objects representing the day tour
        /// activities. If no activities are found, the collection will be empty.</returns>
        Task<IBaseResult<IEnumerable<DayTourActivityDto>>> DayTourActivityListAsync(string vacationId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a vacation day tour activity
        /// </summary>
        /// <param name="activityId">The identity of the day tour activities to retrieve</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success of the operation.</returns>
        Task<IBaseResult<DayTourActivityDto>> DayTourActivityAsync(string activityId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new day tour activity.
        /// </summary>
        /// <param name="dto">The data transfer object containing the day tour activity.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success of the operation.</returns>
        Task<IBaseResult> CreateDayTourActivityAsync(DayTourActivityDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the day tour activity information for a specific vacation.
        /// </summary>
        /// <param name="dto">The data transfer object containing the vacation data, including day tour activity information.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success of the operation.</returns>
        Task<IBaseResult> UpdateDayTourActivityAsync(DayTourActivityDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a day tour activity.
        /// </summary>
        /// <param name="activityId">The ID of the day tour activity to remove.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success of the operation.</returns>
        Task<IBaseResult> RemoveDayTourActivityAsync(string activityId, CancellationToken cancellationToken = default);
    }
}
