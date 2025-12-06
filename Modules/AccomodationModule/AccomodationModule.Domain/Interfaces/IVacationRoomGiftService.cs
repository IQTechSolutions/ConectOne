using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.RequestFeatures;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Domain.Interfaces
{
    /// <summary>
    /// Implements the vacation interval service, providing methods to manage vacation interval data.
    /// </summary>
    public interface IVacationRoomGiftService
    {
        /// <summary>
        /// Retrieves a paginated list of room gifts available during a specified vacation interval.
        /// </summary>
        /// <remarks>This method is designed to support scenarios where users need to query room gifts for
        /// a specific vacation interval with pagination support. Ensure that the <paramref name="args"/> parameter is
        /// properly populated with valid interval and page information.</remarks>
        /// <param name="args">The parameters specifying the vacation interval and pagination details. This must not be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// with an enumerable collection of <see cref="RoomGiftDto"/> objects representing the room gifts.</returns>
        Task<IBaseResult<IEnumerable<RoomGiftDto>>> VacationRoomGiftListAsync(VacationIntervalPageParameters args, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a vacation interval
        /// </summary>
        /// <param name="vacationRoomGiftId">The identity of the vacation interval to retrieve</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success of the operation.</returns>
        Task<IBaseResult<RoomGiftDto>> VacationRoomGiftAsync(string vacationRoomGiftId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new vacation interval.
        /// </summary>
        /// <param name="dto">The data transfer object containing the vacation interval data.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success of the operation.</returns>
        Task<IBaseResult> CreateVacationRoomGiftAsync(RoomGiftDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the vacation interval information for a specific vacation.
        /// </summary>
        /// <param name="dto">The data transfer object containing the vacation data, including interval information.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success of the operation.</returns>
        Task<IBaseResult> UpdateVacationRoomGiftAsync(RoomGiftDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a vacation interval.
        /// </summary>
        /// <param name="vacationRoomGiftId">The ID of the vacation price to remove.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success of the operation.</returns>
        Task<IBaseResult> RemoveVacationRoomGiftAsync(string vacationRoomGiftId, CancellationToken cancellationToken = default);
    }
}
