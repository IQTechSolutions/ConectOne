using CalendarModule.Domain.DataTransferObjects;
using CalendarModule.Domain.Entities;
using CalendarModule.Domain.RequestFeatures;
using ConectOne.Domain.ResultWrappers;

namespace CalendarModule.Domain.Interfaces
{
    /// <summary>
    /// Defines the contract for managing appointments and their associated calendar entries.
    /// </summary>
    /// <remarks>This interface extends <see cref="IBaseService{TEntity, TDto, TKey}"/> to provide 
    /// functionality specific to handling <see cref="Appointment"/> entities and their  corresponding data transfer
    /// objects (<see cref="CalendarEntryDto"/>).</remarks>
    public interface IAppointmentService
    {
        /// <summary>
        /// Asynchronously retrieves all calendar entries.
        /// </summary>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// object that includes a list of <see cref="CalendarEntryDto"/> objects representing the calendar entries.</returns>
        Task<IBaseResult<List<CalendarEntryDto>>> GetAllAsync(CalendarPageParameters requestParameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a calendar entry by its unique identifier.
        /// </summary>
        /// <remarks>Use this method to retrieve detailed information about a specific calendar entry.
        /// Ensure the provided <paramref name="id"/> corresponds to a valid calendar entry in the system.</remarks>
        /// <param name="id">The unique identifier of the calendar entry to retrieve. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping the <see cref="CalendarEntryDto"/> for the specified identifier, or an appropriate error
        /// result if the entry is not found or an error occurs.</returns>
        Task<IBaseResult<CalendarEntryDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously adds a new calendar entry.
        /// </summary>
        /// <remarks>The operation may fail if the provided calendar entry data is invalid or if the
        /// operation  is canceled via the cancellationToken.</remarks>
        /// <param name="dto">The data transfer object containing the details of the calendar entry to add.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  IBaseResult{T} object
        /// wrapping the added CalendarEntryDto.</returns>
        Task<IBaseResult<CalendarEntryDto>> AddAsync(CalendarEntryDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing calendar entry with the provided data.
        /// </summary>
        /// <param name="dto">The data transfer object containing the updated details of the calendar entry.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  IBaseResult{T} object with
        /// the updated CalendarEntryDto  if the operation is successful.</returns>
        Task<IBaseResult<CalendarEntryDto>> EditAsync(CalendarEntryDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the resource with the specified identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the resource to delete. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the delete operation.</returns>
        Task<IBaseResult> DeleteAsync(string id, CancellationToken cancellationToken = default);
    }
}
