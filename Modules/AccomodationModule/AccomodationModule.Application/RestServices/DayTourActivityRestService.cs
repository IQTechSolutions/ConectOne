using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Application.RestServices
{
    /// <summary>
    /// Provides methods for managing day tour activities, including retrieving, creating, updating, and deleting
    /// activities.
    /// </summary>
    /// <remarks>This service interacts with a RESTful API to perform operations related to day tour
    /// activities. It is designed to handle asynchronous operations and supports cancellation tokens for request
    /// management. The service requires an implementation of <see cref="IBaseHttpProvider"/> to communicate with the
    /// underlying API.</remarks>
    /// <param name="provider"></param>
    public class DayTourActivityRestService(IBaseHttpProvider provider) : IDayTourActivityService
    {
        /// <summary>
        /// Retrieves a list of day tour activities associated with the specified vacation.
        /// </summary>
        /// <remarks>This method fetches day tour activities for a specific vacation by its identifier.
        /// The caller can use the <paramref name="cancellationToken"/> to cancel the operation if needed.</remarks>
        /// <param name="vacationId">The unique identifier of the vacation for which to retrieve day tour activities.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// containing an enumerable collection of <see cref="DayTourActivityDto"/> objects representing the day tour
        /// activities.</returns>
        public async Task<IBaseResult<IEnumerable<DayTourActivityDto>>> DayTourActivityListAsync(string vacationId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<DayTourActivityDto>>($"vacations/dayToursActivities/{vacationId}");
            return result;
        }

        /// <summary>
        /// Retrieves the details of a day tour activity based on the specified activity ID.
        /// </summary>
        /// <remarks>This method fetches the details of a day tour activity from the provider's API.
        /// Ensure that the <paramref name="activityId"/> corresponds to a valid activity in the system.</remarks>
        /// <param name="activityId">The unique identifier of the day tour activity to retrieve. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the details of the day tour activity as a <see cref="DayTourActivityDto"/>.</returns>
        public async Task<IBaseResult<DayTourActivityDto>> DayTourActivityAsync(string activityId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<DayTourActivityDto>($"vacations/dayToursActivities/details/{activityId}");
            return result;
        }

        /// <summary>
        /// Creates or updates a day tour activity asynchronously.
        /// </summary>
        /// <remarks>This method sends a PUT request to the "vacations/dayToursActivities" endpoint with
        /// the provided data. Ensure that the <paramref name="dto"/> parameter contains valid data before calling this
        /// method.</remarks>
        /// <param name="dto">The data transfer object containing the details of the day tour activity to be created or updated.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> CreateDayTourActivityAsync(DayTourActivityDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync($"vacations/dayToursActivities", dto);
            return result;
        }

        /// <summary>
        /// Updates an existing day tour activity with the specified details.
        /// </summary>
        /// <remarks>This method sends an HTTP PUT request to update the day tour activity. Ensure that
        /// the <paramref name="dto"/> contains valid and complete data for the update.</remarks>
        /// <param name="dto">The data transfer object containing the updated details of the day tour activity.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateDayTourActivityAsync(DayTourActivityDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync($"vacations/dayToursActivities", dto);
            return result;
        }

        /// <summary>
        /// Removes a day tour activity identified by the specified activity ID.
        /// </summary>
        /// <remarks>This method sends a request to delete the specified day tour activity. Ensure the
        /// <paramref name="activityId"/>  corresponds to an existing activity before calling this method.</remarks>
        /// <param name="activityId">The unique identifier of the day tour activity to be removed. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the removal operation.</returns>
        public async Task<IBaseResult> RemoveDayTourActivityAsync(string activityId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"vacations/dayToursActivities", activityId);
            return result;
        }
    }
}
