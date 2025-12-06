using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;

namespace AccomodationModule.Infrastructure.Implimentation
{
    /// <summary>
    /// Implements the golfer package service, providing methods to manage golfer package data.
    /// </summary>
    public class GolferPackageService(IAccomodationRepositoryManager accomodationRepositoryManager) : IGolferPackageService
    {
        /// <summary>
        /// Retrieves a list of golfer packages associated with the specified vacation.
        /// </summary>
        /// <remarks>This method queries the repository for golfer packages associated with the specified
        /// vacation ID. The result includes a collection of data transfer objects (<see cref="GolferPackageDto"/>)
        /// representing the golfer packages.</remarks>
        /// <param name="vacationId">The unique identifier of the vacation for which golfer packages are to be retrieved.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>An asynchronous operation that returns a result containing a collection of <see cref="GolferPackageDto"/>
        /// objects. If the operation succeeds, the result contains the list of golfer packages; otherwise, it contains
        /// error messages.</returns>
        public async Task<IBaseResult<IEnumerable<GolferPackageDto>>> GolferPackageListAsync(string vacationId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<GolferPackage>(c => c.VacationId! == vacationId);
            var golferPackageResult = await accomodationRepositoryManager.GolferPackages.ListAsync(spec, false, cancellationToken);

            if (golferPackageResult.Succeeded)
                return await Result<IEnumerable<GolferPackageDto>>.SuccessAsync(golferPackageResult.Data.Select(c => new GolferPackageDto(c)));
            return await Result<IEnumerable<GolferPackageDto>>.FailAsync(golferPackageResult.Messages);
        }

        /// <summary>
        /// Get a vacation golfer packages
        /// </summary>
        /// <param name="golferPackageId">The identity of the vacation golfer packages to retrieve</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success of the operation.</returns>
        public async Task<IBaseResult<GolferPackageDto>> GolferPackageAsync(string golferPackageId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<GolferPackage>(c => c.Id == golferPackageId);
            var golferPackageResult = await accomodationRepositoryManager.GolferPackages.FirstOrDefaultAsync(spec, false, cancellationToken);

            if (golferPackageResult.Succeeded)
                return await Result<GolferPackageDto>.SuccessAsync(new GolferPackageDto(golferPackageResult.Data!));
            return await Result<GolferPackageDto>.FailAsync(golferPackageResult.Messages);
        }

        /// <summary>
        /// Creates a new golfer packages.
        /// </summary>
        /// <param name="dto">The data transfer object containing the golfer packages data.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success of the operation.</returns>
        public async Task<IBaseResult> CreateGolferPackageAsync(GolferPackageDto dto, CancellationToken cancellationToken = default)
        {
            var golferPackage = dto.ToGolferPackage();
            await accomodationRepositoryManager.GolferPackages.CreateAsync(golferPackage, cancellationToken);

            // Save the changes
            var saveResult = await accomodationRepositoryManager.GolferPackages.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            // Return success message
            return await Result.SuccessAsync($"Golfer Package was created successfully");
        }

        /// <summary>
        /// Updates the golfer packages information for a specific vacation.
        /// </summary>
        /// <param name="dto">The data transfer object containing the vacation data, including golfer packages information.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success of the operation.</returns>
        public async Task<IBaseResult> UpdateGolferPackageAsync(GolferPackageDto dto, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<GolferPackage>(c => c.Id == dto.GolferPackageId);
            var golferPackageResult = await accomodationRepositoryManager.GolferPackages.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!golferPackageResult.Succeeded) return await Result.FailAsync(golferPackageResult.Messages);

            dto.UpdateGolferPackageValues(golferPackageResult.Data!);

            var updateResult = accomodationRepositoryManager.GolferPackages.Update(golferPackageResult.Data!);
            if (!updateResult.Succeeded) return await Result.FailAsync(updateResult.Messages);

            var saveResult = await accomodationRepositoryManager.GolferPackages.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync($"Golfer Package updated successfully");
        }

        /// <summary>
        /// Removes a golfer packages.
        /// </summary>
        /// <param name="golferPackageId">The ID of the golfer packages to remove.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success of the operation.</returns>
        public async Task<IBaseResult> RemoveGolferPackageAsync(string golferPackageId, CancellationToken cancellationToken = default)
        {
            // Delete the vacation price
            var result = await accomodationRepositoryManager.GolferPackages.DeleteAsync(golferPackageId, cancellationToken);
            if (!result.Succeeded) await Result.FailAsync(result.Messages);

            // Return success message
            return await Result.SuccessAsync($"Golfer Package with id '{golferPackageId}' was successfully removed");
        }
    }
}
