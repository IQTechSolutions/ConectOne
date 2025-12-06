using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AccomodationModule.Infrastructure.Implimentation
{
    /// <summary>
    /// Provides functionality for managing areas and their associated data, including retrieval, creation, updating,
    /// and deletion of areas.
    /// </summary>
    /// <remarks>This service interacts with repositories to perform operations on areas and their related
    /// average temperature data. It supports asynchronous operations and includes methods for retrieving all areas,
    /// retrieving a specific area by ID, creating a new area, updating an existing area, and removing an
    /// area.</remarks>
    /// <param name="areaRepo">The injected area repository</param>
    /// <param name="averageTempRepo">The injected average temperature repository</param>
    public class AreaService(IRepository<Area, string> areaRepo, IRepository<AverageTemperature, string> averageTempRepo) : IAreaService
    {
        /// <summary>
        /// Retrieves all areas along with their associated average temperature data.
        /// </summary>
        /// <remarks>This method queries the repository for all areas and includes their average
        /// temperature data. The result is returned as a collection of <see cref="AreaDto"/> objects. If the operation
        /// is unsuccessful, the result will contain failure messages.</remarks>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// where <c>T</c> is an <see cref="IEnumerable{T}"/> of <see cref="AreaDto"/> objects. If the operation fails,
        /// the result includes error messages.</returns>
        public async Task<IBaseResult<IEnumerable<AreaDto>>> AllAreasAsync(CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Area>(c => true);
            spec.AddInclude(c => c.Include(c => c.AverageTemperatures));

            var result = await areaRepo.ListAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<IEnumerable<AreaDto>>.FailAsync(result.Messages);

            return await Result<IEnumerable<AreaDto>>.SuccessAsync(result.Data.Select(c => new AreaDto(c)));
        }

        /// <summary>
        /// Retrieves the details of an area based on the specified area identifier.
        /// </summary>
        /// <remarks>This method queries the repository for the specified area and includes related data,
        /// such as average temperatures. If the area is not found or the operation fails, the result will indicate
        /// failure with appropriate error messages.</remarks>
        /// <param name="areaId">The unique identifier of the area to retrieve. This parameter cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see langword="default"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object, where <c>T</c> is <see cref="AreaDto"/>. If the operation succeeds, the result contains the area
        /// details; otherwise, it contains error messages.</returns>
        public async Task<IBaseResult<AreaDto>> AreaAsync(string areaId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Area>(c => c.Id == areaId);
            spec.AddInclude(c => c.Include(c => c.AverageTemperatures));

            var result = await areaRepo.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<AreaDto>.FailAsync(result.Messages);
            return await Result<AreaDto>.SuccessAsync(new AreaDto(result.Data));
        }

        /// <summary>
        /// Creates a new area asynchronously based on the provided model.
        /// </summary>
        /// <remarks>This method attempts to create a new area using the provided model. If the creation
        /// fails, the result will include failure messages. Ensure that the <paramref name="model"/> contains valid
        /// data before calling this method.</remarks>
        /// <param name="model">The data transfer object containing the details of the area to be created.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the created <see cref="AreaDto"/> if the operation succeeds, or error messages if it fails.</returns>
        public async Task<IBaseResult<AreaDto>> CreateAreaAsync(AreaDto model, CancellationToken cancellationToken = default)
        {
            var result = await areaRepo.CreateAsync(new Area(model), cancellationToken);
            if (!result.Succeeded) return await Result<AreaDto>.FailAsync(result.Messages);

            var saveResult = await areaRepo.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result<AreaDto>.FailAsync(result.Messages);

            return await Result<AreaDto>.SuccessAsync(new AreaDto(result.Data));
        }

        /// <summary>
        /// Updates an existing area in the database with the provided data.
        /// </summary>
        /// <remarks>This method updates the area's name, description, and associated average
        /// temperatures.  Any new average temperatures provided in <paramref name="areaDto"/> are added to the
        /// database,  while any existing average temperatures not included in <paramref name="areaDto"/> are
        /// removed.</remarks>
        /// <param name="areaDto">The data transfer object containing the updated information for the area. The <see cref="AreaDto.Id"/>
        /// property must match the ID of an existing area.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// indicating the success or failure of the operation. If successful, the result contains the updated <see
        /// cref="AreaDto"/>.</returns>
        public async Task<IBaseResult<AreaDto>> UpdateAreaAsync(AreaDto areaDto, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Area>(c => c.Id == areaDto.Id);
            spec.AddInclude(c => c.Include(g => g.AverageTemperatures));

            var result = await areaRepo.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<AreaDto>.FailAsync(result.Messages);

            var response = result.Data;
            if (response == null)
                return await Result<AreaDto>.FailAsync($"No package with id matching '{areaDto.Id}' was found in the database");

            response.Name = areaDto.Name;
            response.Description = areaDto.Description;

            areaRepo.Update(response);
            var saveResult = await areaRepo.SaveAsync(cancellationToken);
            if (saveResult.Succeeded)
                return await Result<AreaDto>.SuccessAsync(new AreaDto(response));
            return await Result<AreaDto>.FailAsync(saveResult.Messages);
        }

        /// <summary>
        /// Removes an area identified by the specified ID asynchronously.
        /// </summary>
        /// <remarks>This method attempts to remove the specified area from the repository. If the
        /// operation fails, the returned result will include the failure messages.</remarks>
        /// <param name="areaId">The unique identifier of the area to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation. If successful, the result includes a success message;
        /// otherwise, it contains error messages.</returns>
        public async Task<IBaseResult> RemoveAreaAsync(string areaId, CancellationToken cancellationToken = default)
        {
            var result = await areaRepo.DeleteAsync(areaId, cancellationToken);
            if (!result.Succeeded) return await Result.FailAsync(result.Messages);
            return await Result.SuccessAsync($"Package was successfully removed");
        }
    }
}
