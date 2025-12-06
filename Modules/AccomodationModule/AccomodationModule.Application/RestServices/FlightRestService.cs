using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Application.RestServices
{
    /// <summary>
    /// Provides methods for managing flight-related operations, including retrieving, creating, updating, and deleting
    /// flights.
    /// </summary>
    /// <remarks>This service communicates with an external HTTP provider to perform flight-related
    /// operations. It is designed to handle asynchronous operations and supports cancellation tokens for managing
    /// long-running tasks. Ensure that the provided identifiers and data transfer objects are valid and correspond to
    /// existing entities in the system where applicable.</remarks>
    /// <param name="provider"></param>
    public class FlightRestService(IBaseHttpProvider provider) : IFlightService
    {
        /// <summary>
        /// Retrieves a list of flights associated with the specified vacation identifier.
        /// </summary>
        /// <remarks>This method communicates with an external provider to retrieve flight data. Ensure
        /// that the <paramref name="vacationId"/>  corresponds to a valid vacation in the system. The operation may be
        /// canceled by passing a cancellation token.</remarks>
        /// <param name="vacationId">The unique identifier of the vacation for which to retrieve flights. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// containing an enumerable collection of <see cref="FlightDto"/> objects representing the flights for the
        /// specified vacation.</returns>
        public async Task<IBaseResult<IEnumerable<FlightDto>>> FlightListAsync(string vacationId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<FlightDto>>($"vacations/flights/{vacationId}");
            return result;
        }

        /// <summary>
        /// Retrieves flight details for the specified flight ID.
        /// </summary>
        /// <remarks>This method uses the underlying provider to fetch flight details. Ensure that the
        /// <paramref name="flightId"/> corresponds to a valid flight in the system. The operation may be canceled by
        /// passing a cancellation token.</remarks>
        /// <param name="flightId">The unique identifier of the flight to retrieve details for. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the flight details as a <see cref="FlightDto"/>. If the flight is not found, the result may
        /// indicate an error.</returns>
        public async Task<IBaseResult<FlightDto>> FlightAsync(string flightId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<FlightDto>($"vacations/flights/details/{flightId}");
            return result;
        }

        /// <summary>
        /// Creates a new flight using the provided flight details.
        /// </summary>
        /// <remarks>This method sends the flight details to the underlying provider to create a new
        /// flight. Ensure that the  <paramref name="dto"/> contains valid data before calling this method.</remarks>
        /// <param name="dto">The flight details to be created, encapsulated in a <see cref="FlightDto"/> object.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> CreateFlightAsync(FlightDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync($"vacations/flights", dto);
            return result;
        }

        /// <summary>
        /// Updates the flight information asynchronously.
        /// </summary>
        /// <remarks>This method sends the flight update request to the underlying provider. Ensure that
        /// the <paramref name="dto"/> contains valid flight details before calling this method.</remarks>
        /// <param name="dto">The data transfer object containing the flight details to be updated.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateFlightAsync(FlightDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"vacations/flights", dto);
            return result;
        }

        /// <summary>
        /// Removes a flight with the specified identifier asynchronously.
        /// </summary>
        /// <remarks>This method sends a request to delete the flight identified by <paramref
        /// name="flightId"/>. Ensure the provided identifier corresponds to an existing flight.</remarks>
        /// <param name="flightId">The unique identifier of the flight to be removed. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveFlightAsync(string flightId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"vacations/flights", flightId);
            return result;
        }
    }
}
