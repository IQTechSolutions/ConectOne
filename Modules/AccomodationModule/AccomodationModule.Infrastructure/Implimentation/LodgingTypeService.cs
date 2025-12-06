using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;

namespace AccomodationModule.Infrastructure.Implimentation
{
    /// <summary>
    /// Provides operations for managing lodging types, including retrieval, creation, updating, and deletion.
    /// </summary>
    /// <remarks>This service interacts with the accommodation repository to perform CRUD operations on
    /// lodging types. It is designed to handle business logic related to lodging types and return results in a
    /// standardized format.</remarks>
    /// <param name="accomodationRepo">the injected repository manager</param>
    public class LodgingTypeService(IAccomodationRepositoryManager accomodationRepo) : ILodgingTypeService
    {
        /// <summary>
        /// Retrieves all lodging types available in the system.
        /// </summary>
        /// <remarks>This method retrieves all lodging types without applying any filters. The result
        /// includes  a collection of lodging type data transfer objects (DTOs) that contain the ID, name, and 
        /// description of each lodging type.</remarks>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> containing
        /// a collection of <LodgingTypeDto/> objects  representing the lodging types. If the operation fails, the
        /// result includes error messages.</returns>
        public async Task<IBaseResult<IEnumerable<LodgingTypeDto>>> AllLodgingTypesAsync(CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<LodgingType>(c => true);

            var result = await accomodationRepo.LodgingTypes.ListAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<IEnumerable<LodgingTypeDto>>.FailAsync(result.Messages);

            return await Result<IEnumerable<LodgingTypeDto>>.SuccessAsync(result.Data.Select(c => new LodgingTypeDto(c.Id, c.Name, c.Description)));
        }

        /// <summary>
        /// Retrieves a lodging type by its unique identifier.
        /// </summary>
        /// <remarks>This method queries the lodging type repository to find a lodging type that matches
        /// the specified identifier. If the lodging type is found, it is returned as a <see cref="LodgingTypeDto"/>. If
        /// not found or if an error occurs, the result will indicate failure with appropriate error messages.</remarks>
        /// <param name="lodgingTypeId">The unique identifier of the lodging type to retrieve. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation. Defaults to <see
        /// langword="default"/> if not provided.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with a <see cref="LodgingTypeDto"/> representing the lodging type if found, or an error message if the
        /// operation fails.</returns>
        public async Task<IBaseResult<LodgingTypeDto>> LodgingTypeAsync(string lodgingTypeId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<LodgingType>(c => c.Id == lodgingTypeId);

            var result = await accomodationRepo.LodgingTypes.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<LodgingTypeDto>.FailAsync(result.Messages);

            if (result.Data == null)
                return await Result<LodgingTypeDto>.FailAsync($"No lodging type with id matching '{lodgingTypeId}' was found in the database");

            return await Result<LodgingTypeDto>.SuccessAsync(new LodgingTypeDto(result.Data.Id, result.Data.Name, result.Data.Description));
        }

        /// <summary>
        /// Creates a new lodging type asynchronously.
        /// </summary>
        /// <remarks>The method attempts to create a new lodging type in the system. If the operation is
        /// successful, the result will include the newly created lodging type's details. If the operation fails, the
        /// result will include error messages describing the failure.</remarks>
        /// <param name="lodgingType">The lodging type data to create, including its name and description.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with the created lodging type data if the operation succeeds, or error messages if it fails.</returns>
        public async Task<IBaseResult<LodgingTypeDto>> CreateLodgingTypeAsync(LodgingTypeDto lodgingType, CancellationToken cancellationToken = default)
        {
			var result = await accomodationRepo.LodgingTypes.CreateAsync(new LodgingType(lodgingType), cancellationToken);
            if(!result.Succeeded) return await Result<LodgingTypeDto>.FailAsync(result.Messages);

            var saveResult = await accomodationRepo.LodgingTypes.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result<LodgingTypeDto>.FailAsync(saveResult.Messages);

            return await Result<LodgingTypeDto>.SuccessAsync(new LodgingTypeDto(result.Data.Id, result.Data.Name, result.Data.Description));
        }

        /// <summary>
        /// Updates an existing lodging type with the specified details.
        /// </summary>
        /// <remarks>This method updates the name and description of an existing lodging type in the
        /// database. If no lodging type with the specified ID is found, the operation fails.</remarks>
        /// <param name="lodgingType">The lodging type data transfer object containing the updated details. The <see cref="LodgingTypeDto.Id"/>
        /// must match an existing lodging type in the database.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. This parameter is optional and defaults to <see
        /// cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation. If successful, the result contains the updated <see
        /// cref="LodgingTypeDto"/>. If unsuccessful, the result contains error messages.</returns>
        public async Task<IBaseResult> UpdateLodgingTypeAsync(LodgingTypeDto lodgingType, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<LodgingType>(c => c.Id == lodgingType.Id);
            var result = await accomodationRepo.LodgingTypes.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<LodgingTypeDto>.FailAsync(result.Messages);

            var response = result.Data;
            if (response == null)
                return await Result<LodgingTypeDto>.FailAsync($"No package with id matching '{lodgingType.Id}' was found in the database");

            response.Name = lodgingType.Name;
            response.Description = lodgingType.Description;

            accomodationRepo.LodgingTypes.Update(response);
            var saveResult = await accomodationRepo.Packages.SaveAsync(cancellationToken);
            if (saveResult.Succeeded)
                return await Result<LodgingTypeDto>.SuccessAsync(new LodgingTypeDto(response.Id, response.Name, response.Description));
            return await Result<LodgingTypeDto>.FailAsync(saveResult.Messages);
        }

        /// <summary>
        /// Removes a lodging type identified by the specified ID.
        /// </summary>
        /// <remarks>This method attempts to delete the specified lodging type from the repository. If the
        /// operation fails, the returned result will include the failure messages.</remarks>
        /// <param name="lodgingTypeId">The unique identifier of the lodging type to be removed. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>An <see cref="IBaseResult"/> indicating the success or failure of the operation. If successful, the result
        /// contains a success message; otherwise, it contains error messages.</returns>
        public async Task<IBaseResult> RemoveLodgingTypeAsync(string lodgingTypeId, CancellationToken cancellationToken = default)
        {
            var result = await accomodationRepo.LodgingTypes.DeleteAsync(lodgingTypeId, cancellationToken);
            if (!result.Succeeded) return Result.Fail(result.Messages);
            return Result.Success($"Package was successfully removed");
        }
	}
}
