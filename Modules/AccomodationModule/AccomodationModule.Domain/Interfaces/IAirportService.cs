using AccomodationModule.Domain.DataTransferObjects;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Domain.Interfaces
{
    /// <summary>
    /// Service contract for managing airports.
    /// </summary>
    public interface IAirportService
    {
        /// <summary>
        /// Retrieves all airports.
        /// </summary>
        Task<IBaseResult<IEnumerable<AirportDto>>> AllAirportsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a specific airport by id.
        /// </summary>
        Task<IBaseResult<AirportDto>> AirportAsync(string airportId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new airport.
        /// </summary>
        Task<IBaseResult<AirportDto>> CreateAirportAsync(AirportDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing airport.
        /// </summary>
        Task<IBaseResult> UpdateAirportAsync(AirportDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes an airport by id.
        /// </summary>
        Task<IBaseResult> RemoveAirportAsync(string airportId, CancellationToken cancellationToken = default);
    }
}
