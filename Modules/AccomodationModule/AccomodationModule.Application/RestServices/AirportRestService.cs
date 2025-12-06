using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Application.RestServices
{
    /// <summary>
    /// Provides a REST-based implementation of the <see cref="IAirportService"/> interface for managing airport data.
    /// </summary>
    /// <remarks>This service interacts with an underlying HTTP provider to perform CRUD operations on airport
    /// data. It includes methods to retrieve all airports, fetch details of a specific airport, create new airports,
    /// update existing ones, and remove airports. Each operation is asynchronous and supports cancellation through a
    /// <see cref="CancellationToken"/>.</remarks>
    /// <param name="provider"></param>
    public class AirportRestService(IBaseHttpProvider provider) : IAirportService
    {
        /// <summary>
        /// Retrieves a collection of all available airports.
        /// </summary>
        /// <remarks>This method fetches the list of airports from the underlying data provider. The
        /// caller can use the <paramref name="cancellationToken"/> to cancel the operation if needed.</remarks>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// containing an enumerable collection of <see cref="AirportDto"/> objects representing the airports.</returns>
        public async Task<IBaseResult<IEnumerable<AirportDto>>> AllAirportsAsync(CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<AirportDto>>("airports");
            return result;
        }

        /// <summary>
        /// Retrieves information about a specific airport asynchronously.
        /// </summary>
        /// <remarks>This method uses the underlying provider to fetch airport details based on the
        /// specified <paramref name="airportId"/>. Ensure that the provided <paramref name="airportId"/> corresponds to
        /// a valid airport.</remarks>
        /// <param name="airportId">The unique identifier of the airport to retrieve. This value cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that wraps the <see cref="AirportDto"/> containing the airport details.</returns>
        public async Task<IBaseResult<AirportDto>> AirportAsync(string airportId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<AirportDto>($"airports/{airportId}");
            return result;
        }

        /// <summary>
        /// Creates a new airport using the provided data transfer object (DTO).
        /// </summary>
        /// <remarks>This method sends a PUT request to the "airports" endpoint to create a new airport.
        /// Ensure that the provided <paramref name="dto"/> contains valid data before calling this method.</remarks>
        /// <param name="dto">The <see cref="AirportDto"/> containing the details of the airport to be created.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation. The default value is <see
        /// cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// with the created <see cref="AirportDto"/> if the operation is successful.</returns>
        public async Task<IBaseResult<AirportDto>> CreateAirportAsync(AirportDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<AirportDto, AirportDto>($"airports", dto);
            return result;
        }

        /// <summary>
        /// Updates the details of an airport asynchronously.
        /// </summary>
        /// <remarks>This method sends the updated airport details to the underlying provider for
        /// processing. Ensure that the <paramref name="dto"/> parameter contains valid data before calling this
        /// method.</remarks>
        /// <param name="dto">The data transfer object containing the updated airport details.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateAirportAsync(AirportDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<AirportDto, AirportDto>($"airports", dto);
            return result;
        }

        /// <summary>
        /// Removes an airport with the specified identifier.
        /// </summary>
        /// <remarks>This method sends a delete request to the underlying provider to remove the specified
        /// airport. Ensure that the <paramref name="airportId"/> corresponds to a valid airport.</remarks>
        /// <param name="airportId">The unique identifier of the airport to remove. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveAirportAsync(string airportId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"airports", airportId);
            return result;
        }
    }
}
