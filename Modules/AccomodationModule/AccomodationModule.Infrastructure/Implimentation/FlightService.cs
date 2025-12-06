using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;

namespace AccomodationModule.Infrastructure.Implimentation
{
    /// <summary>
    /// Implements the flight service, providing methods to manage vacation flight data.
    /// </summary>
    public class FlightService(IAccomodationRepositoryManager accomodationRepositoryManager) : IFlightService
    {
        /// <summary>
        /// Retrieves a list of flights associated with the specified vacation identifier.
        /// </summary>
        /// <remarks>This method queries the flight repository to retrieve flights associated with the
        /// specified vacation identifier. If the operation is successful, the flights are returned as a collection of
        /// <see cref="FlightDto"/> objects. If the operation fails, the result will include the error messages
        /// describing the failure.</remarks>
        /// <param name="vacationId">The unique identifier of the vacation for which to retrieve the associated flights.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// where <c>T</c> is an <see cref="IEnumerable{T}"/> of <see cref="FlightDto"/> objects. If the operation
        /// succeeds, the result contains the list of flights; otherwise, it contains error messages.</returns>
        public async Task<IBaseResult<IEnumerable<FlightDto>>> FlightListAsync(string vacationId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Flight>(c => c.VacationId! == vacationId);
            var vacationFlightResult = await accomodationRepositoryManager.Flights.ListAsync(spec, false, cancellationToken);

            if (vacationFlightResult.Succeeded)
                return await Result<IEnumerable<FlightDto>>.SuccessAsync(vacationFlightResult.Data.Select(c => new FlightDto(c)));
            return await Result<IEnumerable<FlightDto>>.FailAsync(vacationFlightResult.Messages);
        }

        /// <summary>
        /// Retrieves flight details based on the specified flight identifier.
        /// </summary>
        /// <remarks>This method queries the flight repository to retrieve flight details. The result
        /// indicates whether the operation succeeded and provides the corresponding flight data or error
        /// messages.</remarks>
        /// <param name="flightId">The unique identifier of the flight to retrieve. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// of type <see cref="FlightDto"/>. If the operation succeeds, the result contains the flight details;
        /// otherwise, it contains error messages.</returns>
        public async Task<IBaseResult<FlightDto>> FlightAsync(string flightId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Flight>(c => c.Id == flightId);
            var vacationFlightResult = await accomodationRepositoryManager.Flights.FirstOrDefaultAsync(spec, false, cancellationToken);

            if (vacationFlightResult.Succeeded)
                return await Result<FlightDto>.SuccessAsync(new FlightDto(vacationFlightResult.Data!));
            return await Result<FlightDto>.FailAsync(vacationFlightResult.Messages);
        }

        /// <summary>
        /// Creates a new flight record asynchronously.
        /// </summary>
        /// <remarks>This method creates a new flight record based on the provided <paramref name="dto"/>
        /// and saves it to the underlying data store. If the save operation fails, the result will include the failure
        /// messages.</remarks>
        /// <param name="dto">The data transfer object containing the details of the flight to be created.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation. If the operation succeeds, the result contains a success message;
        /// otherwise, it contains error messages.</returns>
        public async Task<IBaseResult> CreateFlightAsync(FlightDto dto, CancellationToken cancellationToken = default)
        {
            var flight = dto.ToFlight();
            await accomodationRepositoryManager.Flights.CreateAsync(flight, cancellationToken);

            // Save the changes
            var saveResult = await accomodationRepositoryManager.Flights.SaveAsync();
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            // Return success message
            return await Result.SuccessAsync($"Flight was created successfully");
        }

        /// <summary>
        /// Updates the details of an existing flight based on the provided data transfer object (DTO).
        /// </summary>
        /// <remarks>This method performs the following steps: <list type="number"> <item>Retrieves the
        /// flight entity corresponding to the <c>FlightId</c> in the provided DTO.</item> <item>Updates the flight
        /// entity with the values from the DTO.</item> <item>Persists the changes to the data store.</item> </list> If
        /// any step fails, the method returns a failure result with the associated error messages.</remarks>
        /// <param name="dto">The <see cref="FlightDto"/> containing the updated flight details. The <c>FlightId</c> property must
        /// correspond to an existing flight.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation. If the operation succeeds, the result contains a success
        /// message. If it fails, the result contains error messages describing the failure.</returns>
        public async Task<IBaseResult> UpdateFlightAsync(FlightDto dto, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Flight>(c => c.Id == dto.FlightId);

            var vacationFlightResult = await accomodationRepositoryManager.Flights.FirstOrDefaultAsync(spec, true, cancellationToken);
            if (!vacationFlightResult.Succeeded) return await Result.FailAsync(vacationFlightResult.Messages);

            var flight = vacationFlightResult.Data;
            dto.UpdateFlightValues(flight!);

            var updateResult = accomodationRepositoryManager.Flights.Update(flight!);
            if (!updateResult.Succeeded) return await Result.FailAsync(updateResult.Messages);

            var saveResult = await accomodationRepositoryManager.Flights.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync($"Images updated successfully");
        }

        /// <summary>
        /// Removes a flight with the specified identifier asynchronously.
        /// </summary>
        /// <param name="flightId">The unique identifier of the flight to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation. If successful, the result includes a success message.</returns>
        public async Task<IBaseResult> RemoveFlightAsync(string flightId, CancellationToken cancellationToken = default)
        {
            // Delete the vacation price
            var result = await accomodationRepositoryManager.Flights.DeleteAsync(flightId, cancellationToken);
            if (!result.Succeeded) await Result.FailAsync(result.Messages);

            // Return success message
            return await Result.SuccessAsync($"Vacation flight with id '{flightId}' was successfully removed");
        }
    }
}
