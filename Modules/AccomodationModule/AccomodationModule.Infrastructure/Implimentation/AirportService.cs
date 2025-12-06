using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using Microsoft.EntityFrameworkCore;

namespace AccomodationModule.Infrastructure.Implimentation
{
    /// <summary>
    /// Implements airport related operations.
    /// </summary>
    public class AirportService(IAccomodationRepositoryManager accomodationRepo) : IAirportService
    {
        /// <summary>
        /// Retrieves a list of all airports, including their associated cities and countries.
        /// </summary>
        /// <remarks>The method fetches all airports from the repository and includes related city and
        /// country data. The result indicates whether the operation succeeded and provides the corresponding data or
        /// error messages.</remarks>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <IBaseResult{T}/> of
        /// <IEnumerable{T}/> containing <AirportDto/> objects. If the operation fails, the result will include error
        /// messages.</returns>
        public async Task<IBaseResult<IEnumerable<AirportDto>>> AllAirportsAsync(CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Airport>(c => true);
            spec.AddInclude(c => c.Include(g => g.City).ThenInclude(c => c.Country));

            var result = await accomodationRepo.Airports.ListAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<IEnumerable<AirportDto>>.FailAsync(result.Messages);
            return await Result<IEnumerable<AirportDto>>.SuccessAsync(result.Data.Select(a => new AirportDto(a)));
        }

        /// <summary>
        /// Retrieves information about an airport based on its unique identifier.
        /// </summary>
        /// <remarks>This method queries the database for an airport with the specified <paramref
        /// name="airportId"/>. If no matching airport is found, the result will indicate failure with an appropriate
        /// error message.</remarks>
        /// <param name="airportId">The unique identifier of the airport to retrieve. This value cannot be null or empty.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// where <c>T</c> is <see cref="AirportDto"/>. If the operation succeeds, the result contains the airport data;
        /// otherwise, it contains error messages.</returns>
        public async Task<IBaseResult<AirportDto>> AirportAsync(string airportId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Airport>(c => c.Id == airportId);
            spec.AddInclude(c => c.Include(g => g.City).ThenInclude(c => c.Country));

            var result = await accomodationRepo.Airports.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<AirportDto>.FailAsync(result.Messages);
            if (result.Data == null) return await Result<AirportDto>.FailAsync($"No airport with id matching '{airportId}' was found in the database");
            return await Result<AirportDto>.SuccessAsync(new AirportDto(result.Data));
        }

        /// <summary>
        /// Creates a new airport record asynchronously.
        /// </summary>
        /// <remarks>This method performs the following steps: <list type="number"> <item>Attempts to
        /// create a new airport record using the provided <paramref name="dto"/>.</item> <item>If the creation
        /// succeeds, it saves the changes to the repository.</item> <item>Returns a success result containing the
        /// created airport details, or a failure result with error messages if any step fails.</item> </list></remarks>
        /// <param name="dto">The data transfer object containing the details of the airport to be created.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with the created <see cref="AirportDto"/> if the operation succeeds, or error messages if it fails.</returns>
        public async Task<IBaseResult<AirportDto>> CreateAirportAsync(AirportDto dto, CancellationToken cancellationToken = default)
        {
            var createResult = await accomodationRepo.Airports.CreateAsync(new Airport(dto), cancellationToken);
            if (!createResult.Succeeded) return await Result<AirportDto>.FailAsync(createResult.Messages);
            var saveResult = await accomodationRepo.Airports.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result<AirportDto>.FailAsync(saveResult.Messages);
            return await Result<AirportDto>.SuccessAsync(new AirportDto(createResult.Data));
        }

        /// <summary>
        /// Updates an existing airport record in the database with the provided details.
        /// </summary>
        /// <remarks>This method retrieves the airport record matching the ID specified in the <paramref
        /// name="dto"/> parameter. If no matching airport is found, the operation fails with an appropriate error
        /// message. The method updates the airport's name, code, city, and description based on the provided <paramref
        /// name="dto"/> values. If the update is successful, the changes are saved to the database.</remarks>
        /// <param name="dto">An <see cref="AirportDto"/> object containing the updated details of the airport. The <see
        /// cref="AirportDto.Id"/> property must match the ID of an existing airport.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation. This parameter is optional.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation. If successful, the result contains a success message;
        /// otherwise, it contains error messages.</returns>
        public async Task<IBaseResult> UpdateAirportAsync(AirportDto dto, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Airport>(c => c.Id == dto.Id);

            var result = await accomodationRepo.Airports.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result.FailAsync(result.Messages);
            var airport = result.Data;
            if (airport == null) return await Result.FailAsync($"No airport with id matching '{dto.Id}' was found in the database");
            
            
            airport.Name = dto.Name;
            airport.Code = dto.Code;
            airport.CityId = dto.City.CityId;
            airport.Description = dto.Description ?? string.Empty;
            
            accomodationRepo.Airports.Update(airport);
            
            var saveResult = await accomodationRepo.Airports.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);
            return await Result.SuccessAsync($"Airport '{airport.Name}' was updated successfully");
        }

        /// <summary>
        /// Removes an airport from the database based on its unique identifier.
        /// </summary>
        /// <param name="airportId">The unique identifier of the airport to retrieve. This value cannot be null or empty.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation. This parameter is optional.</param>
        /// <returns></returns>
        public async Task<IBaseResult> RemoveAirportAsync(string airportId, CancellationToken cancellationToken = default)
        {
            var result = await accomodationRepo.Airports.DeleteAsync(airportId, cancellationToken);
            if (!result.Succeeded) return await Result.FailAsync(result.Messages);
            return await Result.SuccessAsync($"Airport was successfully removed");
        }
    }
}
