using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using Microsoft.EntityFrameworkCore;

namespace AccomodationModule.Infrastructure.Implimentation
{
    /// <summary>
    /// Provides functionality for managing packages associated with accommodations.
    /// </summary>
    /// <remarks>The <see cref="PackageService"/> class offers methods to retrieve, create, update, and delete
    /// packages. It interacts with the accommodation repository to perform operations on package data.</remarks>
    /// <param name="accomodationRepo">The injected accommodation repository manager</param>
    public class LodgingPackageService(IAccomodationRepositoryManager accomodationRepo) : ILodgingPackageService
    {
        /// <summary>
        /// Retrieves all package account types associated with the specified lodging.
        /// </summary>
        /// <remarks>This method filters packages by the specified lodging ID and excludes deleted
        /// packages. It includes related entities such as rooms, meal plans, bed types, and lodging details in the
        /// result.</remarks>
        /// <param name="lodgingId">The unique identifier of the lodging for which package account types are to be retrieved.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>An asynchronous operation that returns a result containing a collection of <see cref="PackageDto"/> objects.
        /// If the operation fails, the result will include error messages.</returns>
		public async Task<IBaseResult<IEnumerable<LodgingPackageDto>>> AllPackageAccountTypesAsync(string lodgingId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<LodgingPackage>(c => c.LodgingId == lodgingId && !c.IsDeleted);
            spec.AddInclude(c => c.Include(c => c.Rooms).ThenInclude(c => c.MealPlans));
            spec.AddInclude(c => c.Include(c => c.Rooms).ThenInclude(c => c.BedTypes));
            spec.AddInclude(c => c.Include(c => c.Lodging));

            var result = await accomodationRepo.Packages.ListAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<IEnumerable<LodgingPackageDto>>.FailAsync(result.Messages);

            return await Result<IEnumerable<LodgingPackageDto>>.SuccessAsync(result.Data.Select(c => new LodgingPackageDto(c)));
        }

        /// <summary>
        /// Retrieves a package by its unique identifier and returns detailed information about the package.
        /// </summary>
        /// <remarks>This method retrieves a package along with its associated rooms, meal plans, bed
        /// types, and lodging details. The operation may fail if the package does not exist or if there are issues
        /// accessing the data source.</remarks>
        /// <param name="packageId">The unique identifier of the package to retrieve.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>An asynchronous operation that returns an <see cref="IBaseResult{T}"/> containing a <see cref="PackageDto"/>
        /// with detailed information about the package if the operation succeeds, or error messages if it fails.</returns>
        public async Task<IBaseResult<LodgingPackageDto>> ProductPackageAsync(int packageId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<LodgingPackage>(c => c.Id == packageId);
            spec.AddInclude(c => c.Include(c => c.Rooms).ThenInclude(c => c.MealPlans));
            spec.AddInclude(c => c.Include(c => c.Rooms).ThenInclude(c => c.BedTypes));
            spec.AddInclude(c => c.Include(c => c.Lodging));

            var result = await accomodationRepo.Packages.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<LodgingPackageDto>.FailAsync(result.Messages);
            return await Result<LodgingPackageDto>.SuccessAsync(new LodgingPackageDto(result.Data));
        }

        /// <summary>
        /// Creates a new product package asynchronously.
        /// </summary>
        /// <remarks>This method attempts to create a new product package using the provided <paramref
        /// name="model"/>.  If the creation fails, the result will include failure messages.</remarks>
        /// <param name="model">The data transfer object representing the package to be created.  This must contain valid package details.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// object with the created package data if the operation succeeds, or error messages if it fails.</returns>
        public async Task<IBaseResult<LodgingPackageDto>> CreateProductPackageAsync(LodgingPackageDto model, CancellationToken cancellationToken = default)
        {
			var result = await accomodationRepo.Packages.CreateAsync(new LodgingPackage(model), cancellationToken);
            if(!result.Succeeded) return await Result<LodgingPackageDto>.FailAsync(result.Messages);

            return await Result<LodgingPackageDto>.SuccessAsync(new LodgingPackageDto(result.Data));
        }

        /// <summary>
        /// Updates an existing product package in the database with the provided details.
        /// </summary>
        /// <remarks>This method updates the short description, long description, and special rate ID of
        /// the specified package. If no package with the given ID exists, the operation fails and returns an error
        /// message.</remarks>
        /// <param name="package">The <see cref="PackageDto"/> containing the updated details for the product package. The <see
        /// cref="PackageDto.PackageId"/> must correspond to an existing package in the database.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// of type <see cref="PackageDto"/>. If the update is successful, the result contains the updated package
        /// details. If the operation fails, the result contains error messages describing the failure.</returns>
        public async Task<IBaseResult<LodgingPackageDto>> UpdateProductPackageAsync(LodgingPackageDto package, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<LodgingPackage>(c => c.Id == package.PackageId);
            var result = await accomodationRepo.Packages.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return Result<LodgingPackageDto>.Fail(result.Messages);

            var response = result.Data;
            if (response == null)
                return Result<LodgingPackageDto>.Fail($"No package with id matching '{package.PackageId}' was found in the database");

            response.ShortDescription = package.ShortDescription;
            response.LongDescription = package.LongDescription;
            response.SpecialRateId = package.SpecialRateId;

            accomodationRepo.Packages.Update(response);
            var saveResult = await accomodationRepo.Packages.SaveAsync(cancellationToken);
            if (saveResult.Succeeded)
                return await Result<LodgingPackageDto>.SuccessAsync(new LodgingPackageDto(response));
            return await Result<LodgingPackageDto>.FailAsync(saveResult.Messages);
        }

        /// <summary>
        /// Removes a package with the specified identifier asynchronously.
        /// </summary>
        /// <remarks>This method attempts to remove a package by its identifier. If the operation fails, 
        /// the returned result will include failure messages detailing the issue.</remarks>
        /// <param name="id">The unique identifier of the package to be removed.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>An <see cref="IBaseResult"/> indicating the outcome of the operation.  If successful, the result contains a
        /// success message; otherwise, it contains error messages.</returns>
        public async Task<IBaseResult> RemovePackageAsync(int id, CancellationToken cancellationToken = default)
        {
            var result = await accomodationRepo.Packages.DeleteAsync(id, cancellationToken);
            if (!result.Succeeded) return await Result.FailAsync(result.Messages);
            return await Result.SuccessAsync($"Package was successfully removed");
        }
	}
}
