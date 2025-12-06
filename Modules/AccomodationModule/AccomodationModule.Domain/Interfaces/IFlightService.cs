using AccomodationModule.Domain.DataTransferObjects;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Domain.Interfaces
{
    /// <summary>
    /// Implements the flight service, providing methods to manage vacation flight data.
    /// </summary>
    public interface IFlightService
    {
        /// <summary>
        /// Retrieves a list of flights associated with the specified vacation package.
        /// </summary>
        /// <remarks>The returned flights are specific to the provided vacation package identifier. If no
        /// flights are found, the result may contain an empty collection. Ensure to check the result's status or data
        /// as appropriate.</remarks>
        /// <param name="vacationId">The unique identifier of the vacation package for which to retrieve flights. Cannot be <see
        /// langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// containing an enumerable collection of <see cref="FlightDto"/> objects representing the flights.</returns>
        Task<IBaseResult<IEnumerable<FlightDto>>> FlightListAsync(string vacationId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves flight details for the specified flight ID.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch flight details. Ensure that
        /// the provided <paramref name="flightId"/> is valid and corresponds to an existing flight. The operation may
        /// be canceled by passing a cancellation token.</remarks>
        /// <param name="flightId">The unique identifier of the flight to retrieve. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping the flight details as a <see cref="FlightDto"/>. If the flight is not found, the result may
        /// indicate an error.</returns>
        Task<IBaseResult<FlightDto>> FlightAsync(string flightId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new flight based on the provided flight details.
        /// </summary>
        /// <remarks>The method performs validation on the provided flight details before attempting to
        /// create the flight. Ensure that all required fields in <paramref name="dto"/> are populated to avoid
        /// validation errors.</remarks>
        /// <param name="dto">The data transfer object containing the details of the flight to be created.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation, including success or failure details.</returns>
        Task<IBaseResult> CreateFlightAsync(FlightDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the details of an existing flight asynchronously.
        /// </summary>
        /// <remarks>This method updates the flight details based on the information provided in the
        /// <paramref name="dto"/> parameter. Ensure that the flight exists before calling this method. The operation
        /// may fail if the provided data is invalid or if the flight cannot be found.</remarks>
        /// <param name="dto">The data transfer object containing the updated flight details. Cannot be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        Task<IBaseResult> UpdateFlightAsync(FlightDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a flight with the specified identifier asynchronously.
        /// </summary>
        /// <remarks>The operation will fail if the specified flight does not exist. Ensure the <paramref
        /// name="flightId"/>  corresponds to a valid flight before calling this method.</remarks>
        /// <param name="flightId">The unique identifier of the flight to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        Task<IBaseResult> RemoveFlightAsync(string flightId, CancellationToken cancellationToken = default);
    }
}
