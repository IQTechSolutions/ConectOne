using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.RequestFeatures;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Domain.Interfaces
{
    /// <summary>
    /// Implements the vacation interval service, providing methods to manage vacation interval data.
    /// </summary>
    public interface IVacationIntervalService
    {
        /// <summary>
        /// Retrieves a paginated list of vacation intervals based on the specified parameters.
        /// </summary>
        /// <param name="args">The parameters used to filter and paginate the vacation intervals. Cannot be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// with a collection of <see cref="VacationIntervalDto"/> objects representing the vacation intervals.</returns>
        Task<IBaseResult<IEnumerable<VacationIntervalDto>>> VacationIntervalListAsync(VacationIntervalPageParameters args, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a vacation interval
        /// </summary>
        /// <param name="vacationPricingId">The identity of the vacation interval to retrieve</param>
        /// <returns>A result indicating the success of the operation.</returns>
        Task<IBaseResult<VacationIntervalDto>> VacationIntervalAsync(string vacationPricingId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new vacation interval.
        /// </summary>
        /// <param name="dto">The data transfer object containing the vacation interval data.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success of the operation.</returns>
        Task<IBaseResult> CreateVacationIntervalAsync(VacationIntervalDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the vacation interval information for a specific vacation.
        /// </summary>
        /// <param name="dto">The data transfer object containing the vacation data, including interval information.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success of the operation.</returns>
        Task<IBaseResult> UpdateVacationIntervalAsync(VacationIntervalDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a vacation interval.
        /// </summary>
        /// <param name="vacationIntervalId">The ID of the vacation price to remove.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success of the operation.</returns>
        Task<IBaseResult> RemoveVacationIntervalAsync(string vacationIntervalId, CancellationToken cancellationToken = default);
    }
}