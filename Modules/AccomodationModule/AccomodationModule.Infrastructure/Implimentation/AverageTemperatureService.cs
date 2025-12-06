using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;

namespace AccomodationModule.Infrastructure.Implimentation
{
    /// <summary>
    /// Provides functionality for managing average temperature data, including retrieval, creation,  updating, and
    /// deletion operations.
    /// </summary>
    /// <remarks>This service interacts with a repository to perform CRUD operations on average temperature
    /// records.  It supports asynchronous operations and includes methods for retrieving all average temperatures  for
    /// a specific area, retrieving a single average temperature by its identifier, creating new records,  updating
    /// existing records, and removing records.</remarks>
    /// <param name="averageTempRepo"></param>
    public class AverageTemperatureService(IRepository<AverageTemperature, string> averageTempRepo) : IAverageTemperatureService
    {
        /// <summary>
        /// Retrieves the average temperatures for a specified area.
        /// </summary>
        /// <remarks>This method queries the repository for average temperature data associated with the
        /// specified area ID. The result includes a collection of DTOs that encapsulate the temperature data.</remarks>
        /// <param name="areaId">The identifier of the area for which average temperatures are requested. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/> if not
        /// provided.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// with a collection of <see cref="AverageTemperatureDto"/> objects representing the average temperatures for
        /// the specified area. If the operation fails, the result will include error messages.</returns>
        public async Task<IBaseResult<IEnumerable<AverageTemperatureDto>>> AllAverageTempraturesAsync(string areaId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<AverageTemperature>(c => c.AreaId == areaId);

            var result = await averageTempRepo.ListAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<IEnumerable<AverageTemperatureDto>>.FailAsync(result.Messages);

            return await Result<IEnumerable<AverageTemperatureDto>>.SuccessAsync(result.Data.Select(c => new AverageTemperatureDto(c)));
        }

        /// <summary>
        /// Retrieves the average temperature data associated with the specified identifier.
        /// </summary>
        /// <remarks>This method queries the repository for the average temperature record matching the
        /// specified identifier. If the record is found, it returns a success result containing the corresponding data
        /// transfer object. If the record is not found or the operation fails, it returns a failure result with error
        /// messages.</remarks>
        /// <param name="averageTempId">The unique identifier of the average temperature record to retrieve. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional; defaults to <see langword="default"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping an <see cref="AverageTemperatureDto"/> instance if the operation succeeds, or error messages
        /// if it fails.</returns>
        public async Task<IBaseResult<AverageTemperatureDto>> AverageTempratureAsync(string averageTempId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<AverageTemperature>(c => c.Id == averageTempId);

            var result = await averageTempRepo.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<AverageTemperatureDto>.FailAsync(result.Messages);
            return await Result<AverageTemperatureDto>.SuccessAsync(new AverageTemperatureDto(result.Data!));
        }

        /// <summary>
        /// Creates a new average temperature record asynchronously.
        /// </summary>
        /// <param name="model">The data transfer object containing the details of the average temperature to be created.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the created <see cref="AverageTemperatureDto"/> if the operation succeeds, or error messages if
        /// it fails.</returns>
        public async Task<IBaseResult<AverageTemperatureDto>> CreateAverageTempratureAsync(AverageTemperatureDto model, CancellationToken cancellationToken = default)
        {
            var result = await averageTempRepo.CreateAsync(new AverageTemperature(model), cancellationToken);
            if (!result.Succeeded) return await Result<AverageTemperatureDto>.FailAsync(result.Messages);

            var saveResult = await averageTempRepo.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result<AverageTemperatureDto>.FailAsync(saveResult.Messages);

            return await Result<AverageTemperatureDto>.SuccessAsync(new AverageTemperatureDto(result.Data));
        }

        /// <summary>
        /// Updates the average temperature record in the database with the provided data.
        /// </summary>
        /// <remarks>This method updates the average temperature record identified by the <c>Id</c>
        /// property of the <paramref name="package"/> parameter. If the record does not exist, the operation fails and
        /// an appropriate error message is returned. The method performs validation and ensures that the database is
        /// updated only if the record exists.</remarks>
        /// <param name="package">The <see cref="AverageTemperatureDto"/> containing the updated average temperature data. The <c>Id</c>
        /// property must match an existing record in the database.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object, which indicates the success or failure of the operation. If successful, the result contains the
        /// updated <see cref="AverageTemperatureDto"/> object. If no matching record is found, or if the update fails,
        /// the result contains error messages.</returns>
        public async Task<IBaseResult<AverageTemperatureDto>> UpdateAverageTempratureAsync(AverageTemperatureDto package, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<AverageTemperature>(c => c.Id == package.Id);
            var result = await averageTempRepo.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<AverageTemperatureDto>.FailAsync(result.Messages);

            var response = result.Data;
            if (response == null)
                return Result<AverageTemperatureDto>.Fail($"No average temprature with id matching '{package.Id}' was found in the database");

            response.Month = package.Month;
            response.AvgLow = package.AvgLow;
            response.AvgHigh = package.AvgHigh;

            averageTempRepo.Update(response);
            var saveResult = await averageTempRepo.SaveAsync(cancellationToken);
            if (saveResult.Succeeded)
                return await Result<AverageTemperatureDto>.SuccessAsync(new AverageTemperatureDto());
            return await Result<AverageTemperatureDto>.FailAsync(saveResult.Messages);
        }

        /// <summary>
        /// Removes the average temperature record associated with the specified identifier.
        /// </summary>
        /// <remarks>This method attempts to delete the specified average temperature record. If the
        /// operation fails, the result will include error messages.</remarks>
        /// <param name="id">The unique identifier of the average temperature record to remove. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
        public async Task<IBaseResult> RemoveAverageTempratureAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await averageTempRepo.DeleteAsync(id, cancellationToken);
            if (!result.Succeeded) return await Result.FailAsync(result.Messages);
            return await Result.SuccessAsync($"Package was successfully removed");
        }
    }
}
