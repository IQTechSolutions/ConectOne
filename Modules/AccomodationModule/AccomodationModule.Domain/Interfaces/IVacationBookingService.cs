using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.RequestFeatures;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Domain.Interfaces
{
    /// <summary>
    /// Defines the contract for managing vacation bookings, including operations for retrieving, creating, updating, 
    /// and deleting vacation bookings, as well as marking bookings as active.
    /// </summary>
    /// <remarks>This service provides methods to interact with vacation booking data, supporting both
    /// paginated and full  retrieval of bookings. It also includes functionality for managing the lifecycle of vacation
    /// bookings, such as  adding, editing, and deleting bookings, as well as marking specific bookings as
    /// active.</remarks>
    public interface IVacationBookingService 
    {
        /// <summary>
        /// Retrieves a paginated list of vacation bookings based on the specified parameters.
        /// </summary>
        /// <remarks>The method supports pagination and filtering based on the provided <paramref
        /// name="parameters"/>. Use the <paramref name="cancellationToken"/> to cancel the operation if
        /// needed.</remarks>
        /// <param name="parameters">The pagination and filtering parameters used to retrieve the vacation bookings.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing the paginated list of vacation bookings as <see
        /// cref="VacationBookingDto"/> objects.</returns>
        Task<PaginatedResult<VacationBookingDto>> PagedAsync(VacationBookingPageParams parameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Marks the specified vacation booking as active.
        /// </summary>
        /// <remarks>This method is used to update the status of a vacation booking to active. Ensure that
        /// the <paramref name="vacationBookingId"/> corresponds to a valid booking before calling this
        /// method.</remarks>
        /// <param name="vacationBookingId">The unique identifier of the vacation booking to be marked as active. Cannot be <c>null</c> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        Task<IBaseResult> MarkAsActiveAsync(string vacationBookingId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all vacation bookings asynchronously.
        /// </summary>
        /// <remarks>The returned result may include an empty list if no vacation bookings are available.
        /// Ensure to check the result's status or error information as appropriate.</remarks>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// containing a list of <see cref="VacationBookingDto"/> objects representing the vacation bookings.</returns>
        Task<IBaseResult<List<VacationBookingDto>>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a vacation booking by its unique identifier.
        /// </summary>
        /// <remarks>Use this method to retrieve detailed information about a specific vacation booking.
        /// Ensure the provided <paramref name="id"/> corresponds to an existing booking. The operation may return a
        /// failure result if the booking does not exist or if an error occurs during retrieval.</remarks>
        /// <param name="id">The unique identifier of the vacation booking to retrieve. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping the <see cref="VacationBookingDto"/> if the booking is found, or an appropriate result
        /// indicating failure.</returns>
        Task<IBaseResult<VacationBookingDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously adds a new vacation booking.
        /// </summary>
        /// <param name="dto">The data transfer object containing the details of the vacation booking to add.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that includes the added vacation booking details.</returns>
        Task<IBaseResult<VacationBookingDto>> AddAsync(VacationBookingDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing vacation booking with the provided details.
        /// </summary>
        /// <param name="dto">The data transfer object containing the updated vacation booking details.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  IBaseResult{T} object with
        /// the updated VacationBookingDto  if the operation is successful.</returns>
        Task<IBaseResult<VacationBookingDto>> EditAsync(VacationBookingDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the resource identified by the specified ID asynchronously.
        /// </summary>
        /// <remarks>This method performs the delete operation asynchronously. Ensure that the provided
        /// <paramref name="id"/> corresponds to an existing resource. The operation may fail if the resource does not
        /// exist or if it is locked or in use.</remarks>
        /// <param name="id">The unique identifier of the resource to delete. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the delete operation.</returns>
        Task<IBaseResult> DeleteAsync(string id, CancellationToken cancellationToken = default);
    }
}
