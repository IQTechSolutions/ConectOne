using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.RequestFeatures;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AccomodationModule.Infrastructure.Implimentation
{
    /// <summary>
    /// Service for managing amenities in the accommodation module.
    /// </summary>
    /// <remarks>
    /// This service provides methods to create, update, delete, and retrieve amenities,
    /// as well as manage their association with lodgings and rooms.
    /// </remarks>
    /// <param name="amenityRepo">The injected amenity item repository</param>
    /// <param name="lodgingAmenityItemRepo">The injected lodging amenity item repository</param>
    /// <param name="roomAmenityItemRepo">The injected room amenity repository</param>
    public class AmenityService(IRepository<Amenity, int> amenityRepo, IRepository<AmenityItem<Lodging, string>, string> lodgingAmenityItemRepo, IRepository<AmenityItem<Room, int>, int> roomAmenityItemRepo) : IAmenityService
    {
		/// <summary>
		/// Retrieves a collection of amenities associated with a specific lodging.
		/// </summary>
		/// <remarks>This method queries the repository for amenities associated with the specified lodging ID and
		/// maps the results to <see cref="AmenityDto"/> objects. If the operation is unsuccessful, the result will include
		/// error messages detailing the failure.</remarks>
		/// <param name="lodgingId">The unique identifier of the lodging for which amenities are to be retrieved.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> with a
		/// collection of <see cref="AmenityDto"/> objects representing the amenities of the specified lodging. If the
		/// operation fails, the result contains error messages.</returns>
		public async Task<IBaseResult<IEnumerable<AmenityDto>>> LodgingAmenitiesAsync(string lodgingId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<AmenityItem<Lodging, string>>(c => c.LodgingId == lodgingId);
			spec.AddInclude(c => c.Include(g => g.Amenity));

			var result = await lodgingAmenityItemRepo.ListAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<IEnumerable<AmenityDto>>.FailAsync(result.Messages);

            return await Result<IEnumerable<AmenityDto>>.SuccessAsync(result.Data.Select(c => new AmenityDto(c.Amenity)));
        }

		/// <summary>
		/// Retrieves a paginated list of amenities based on the specified request parameters.
		/// </summary>
		/// <remarks>The method filters amenities by name or description if <see cref="RequestParameters.SearchText"/>
		/// is provided. Pagination is applied using the <see cref="RequestParameters.PageNr"/> and <see
		/// cref="RequestParameters.PageSize"/> properties.</remarks>
		/// <param name="parameters">The parameters used to control pagination and filtering of the amenities. Includes properties such as <see
		/// cref="RequestParameters.PageNr"/>, <see cref="RequestParameters.PageSize"/>,  and <see
		/// cref="RequestParameters.SearchText"/> for filtering by name or description.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="PaginatedResult{T}"/> containing a list of <see cref="AmenityDto"/> objects that match the  specified
		/// search criteria and pagination settings. If no amenities are found, the result will contain an  empty list. If the
		/// operation fails, the result will include error messages.</returns>
        public async Task<PaginatedResult<AmenityDto>> PagedAmenitiesAsync(RequestParameters parameters, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Amenity>(c => true);
			if (!string.IsNullOrEmpty(parameters.SearchText))
				spec = new LambdaSpec<Amenity>(c => c.Name.Contains(parameters.SearchText) || c.Description != null && c.Description.Contains(parameters.SearchText));
            
			var result = await amenityRepo.ListAsync(spec, false, cancellationToken);
			return !result.Succeeded 
                ? PaginatedResult<AmenityDto>.Failure(result.Messages) 
                : PaginatedResult<AmenityDto>.Success(result.Data.Select(c => new AmenityDto(c)).ToList(), result.Data.Count(), parameters.PageNr, parameters.PageSize);
        }

		/// <summary>
		/// Retrieves all available amenities.
		/// </summary>
		/// <param name="cancellationToken">A token that can be used to cancel the operation. The default value is <see langword="default"/>.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> with a
		/// collection of <see cref="AmenityDto"/> objects representing the amenities.</returns>
        public async Task<IBaseResult<IEnumerable<AmenityDto>>> AllAmenitiesAsync(CancellationToken cancellationToken = default)
        {
            var result = await amenityRepo.ListAsync(false, cancellationToken);
            if (!result.Succeeded) return await Result<IEnumerable<AmenityDto>>.FailAsync(result.Messages);
            return await Result<IEnumerable<AmenityDto>>.SuccessAsync(result.Data.Select(c => new AmenityDto(c)));
        }

		/// <summary>
		/// Retrieves an amenity by its unique identifier.
		/// </summary>
		/// <remarks>This method queries the database for an amenity matching the specified <paramref
		/// name="amenityId"/>. If the amenity exists, it is returned as a <see cref="AmenityDto"/>. If no matching amenity is
		/// found, the result will indicate failure with a descriptive error message.</remarks>
		/// <param name="amenityId">The unique identifier of the amenity to retrieve. Must be a valid, existing amenity ID.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
		/// <returns>An <see cref="IBaseResult{T}"/> containing the <see cref="AmenityDto"/> if the amenity is found. If no amenity
		/// matches the specified <paramref name="amenityId"/>, the result will indicate failure with an appropriate error
		/// message.</returns>
        public async Task<IBaseResult<AmenityDto>> AmenityAsync(int amenityId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Amenity>(c => c.Id == amenityId);

			var result = await amenityRepo.FirstOrDefaultAsync(spec, false, cancellationToken);
			if(!result.Succeeded) return await Result<AmenityDto>.FailAsync(result.Messages);

            return await Result<AmenityDto>.SuccessAsync(new AmenityDto(result.Data));
        }

		/// <summary>
		/// Creates a new amenity based on the provided data transfer object (DTO).
		/// </summary>
		/// <remarks>This method attempts to create a new amenity in the underlying repository. If the creation is
		/// successful, the returned result includes the newly created amenity. If the creation fails, the returned result
		/// includes the failure messages.</remarks>
		/// <param name="dto">The data transfer object containing the details of the amenity to be created. Must not be <see langword="null"/>.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
		/// <returns>A result object containing the created amenity as a <see cref="AmenityDto"/> if the operation succeeds. If the
		/// operation fails, the result contains error messages describing the failure.</returns>
		public async Task<IBaseResult<AmenityDto>> CreateAmenity(AmenityDto dto, CancellationToken cancellationToken = default)
		{
			var amenity = dto.ToAmenity();
			var result = await amenityRepo.CreateAsync(amenity, cancellationToken);
			if(!result.Succeeded) return await Result<AmenityDto>.FailAsync(result.Messages);

            return await Result<AmenityDto>.SuccessAsync(new AmenityDto(amenity));
        }

		/// <summary>
		/// Updates an existing amenity in the database with the provided details.
		/// </summary>
		/// <remarks>This method updates the name and icon class of an existing amenity based on the provided
		/// <paramref name="amenity"/> object. If no amenity matching the specified ID is found, the method returns a failure
		/// result with an appropriate error message.</remarks>
		/// <param name="amenity">The data transfer object containing the updated details of the amenity. The <see cref="AmenityDto.AmenityId"/>
		/// property must match the ID of an existing amenity.</param>
		/// <param name="cancellationToken">An optional token to monitor for cancellation requests.</param>
		/// <returns>A result indicating the success or failure of the operation. If successful, the result contains a success message.
		/// If the operation fails, the result includes error messages describing the issue.</returns>
		public async Task<IBaseResult<AmenityDto>> UpdateAmenity(AmenityDto amenity, CancellationToken cancellationToken = default)
		{
			var spec = new LambdaSpec<Amenity>(c => c.Id.ToString() == amenity.AmenityId);

            var result = await amenityRepo.FirstOrDefaultAsync(spec, true, cancellationToken);
			if(!result.Succeeded) return await Result<AmenityDto>.FailAsync(result.Messages);

            var response = result.Data;
            if (response is null)
                return await Result<AmenityDto>.FailAsync($"No amenity with id matching '{amenity.AmenityId}' was found in the database");

            response.Name = amenity.Name;
            response.IconClass = amenity.Icon;

            amenityRepo.Update(response);

            var saveResult = await amenityRepo.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result<AmenityDto>.FailAsync(saveResult.Messages);
            {
                return await Result<AmenityDto>.SuccessAsync($"{response.Name} successfully updated");
            }
        }

		/// <summary>
		/// Removes an amenity by its unique identifier.
		/// </summary>
		/// <remarks>This method attempts to delete the specified amenity and save the changes to the repository. If
		/// the save operation fails, the result will indicate failure with the associated error messages.</remarks>
		/// <param name="amenityId">The unique identifier of the amenity to be removed.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation. Defaults to <see langword="default"/> if not provided.</param>
		/// <returns>An <see cref="IBaseResult"/> indicating the outcome of the operation.  If successful, the result contains a
		/// success message; otherwise, it contains error messages.</returns>
		public async Task<IBaseResult> RemoveAmentity(int amenityId, CancellationToken cancellationToken = default)
        {
            var result = await amenityRepo.DeleteAsync(amenityId, cancellationToken);
			var saveResult = await amenityRepo.SaveAsync(cancellationToken);
			if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync($"{result.Data} successfully deleted");
        }
        
        /// <summary>
		/// Adds an amenity item to a lodging entity.
		/// </summary>
		/// <remarks>This method associates an amenity with a lodging entity by creating a new amenity item and saving
		/// it to the repository. Ensure that both the amenity and lodging identifiers are valid before calling this
		/// method.</remarks>
		/// <param name="amentityId">The unique identifier of the amenity to be added.</param>
		/// <param name="lodgingId">The unique identifier of the lodging to which the amenity will be added.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation. Defaults to <see cref="CancellationToken.None"/>.</param>
		/// <returns>A task that represents the asynchronous operation. The result contains an <see cref="IBaseResult"/> indicating
		/// whether the operation succeeded or failed. If successful, the result includes a success message.</returns>
		public async Task<IBaseResult> AddLodgingAmenityItem(int amentityId, string lodgingId, CancellationToken cancellationToken = default)
		{
            var amenityItem = new AmenityItem<Lodging, string>(lodgingId, amentityId);
            await lodgingAmenityItemRepo.CreateAsync(amenityItem, cancellationToken);
            var saveResult = await lodgingAmenityItemRepo.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync($"{amenityItem.Amenity} was successfully added to lodging");
        }

		/// <summary>
		/// Removes a lodging amenity item associated with the specified amenity ID and lodging ID.
		/// </summary>
		/// <remarks>This method performs the removal of a lodging amenity item and saves the changes to the
		/// repository. Ensure that the provided IDs are valid and correspond to existing entities.</remarks>
		/// <param name="amentityId">The unique identifier of the amenity to be removed.</param>
		/// <param name="lodgingId">The unique identifier of the lodging associated with the amenity.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
		/// <returns>An <see cref="IBaseResult"/> indicating the outcome of the operation. Returns a success result if the amenity was
		/// successfully removed, or a failure result with error messages if the operation did not succeed.</returns>
		public async Task<IBaseResult> RemoveLodgingAmenityItem(int amentityId, string lodgingId, CancellationToken cancellationToken = default)
		{
            await lodgingAmenityItemRepo.DeleteAsync(amentityId.ToString(), cancellationToken);
            var saveResult = await amenityRepo.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync($"{amentityId} successfully updated");
        }
		
		/// <summary>
		/// Adds an amenity to a specified room.
		/// </summary>
		/// <remarks>This method associates an amenity with a room by creating a new amenity item and saving the
		/// changes. If the operation fails, the result will include error messages indicating the reason for
		/// failure.</remarks>
		/// <param name="amentityId">The unique identifier of the amenity to be added.</param>
		/// <param name="roomId">The unique identifier of the room to which the amenity will be added.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation. Defaults to <see langword="default"/> if not provided.</param>
		/// <returns>A task that represents the asynchronous operation. The result contains a success message if the amenity was added
		/// successfully, or failure messages if the operation did not succeed.</returns>
        public async Task<IBaseResult> AddRoomAmenityItem(int amentityId, int roomId, CancellationToken cancellationToken = default)
		{
			var amenityItem = new AmenityItem<Room, int>(roomId, amentityId);
			await roomAmenityItemRepo.CreateAsync(amenityItem, cancellationToken);

			var saveResult = await amenityRepo.SaveAsync(cancellationToken);
			if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync($"{amenityItem.Amenity} was successfully added to room");
        }
		
		/// <summary>
		/// Removes a specific amenity item from a room.
		/// </summary>
		/// <remarks>This method performs the removal of an amenity item from a room and saves the changes to the
		/// repository. Ensure that both <paramref name="amentityId"/> and <paramref name="roomId"/> correspond to valid
		/// entities.</remarks>
		/// <param name="amentityId">The unique identifier of the amenity item to be removed.</param>
		/// <param name="roomId">The unique identifier of the room from which the amenity item will be removed.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation. Defaults to <see langword="default"/> if not provided.</param>
		/// <returns>An <see cref="IBaseResult"/> indicating the outcome of the operation. Returns a success result if the amenity item
		/// was successfully removed, or a failure result with error messages if the operation did not succeed.</returns>
        public async Task<IBaseResult> RemoveRoomAmenityItem(int amentityId, int roomId, CancellationToken cancellationToken = default)
		{
            await lodgingAmenityItemRepo.DeleteAsync(amentityId.ToString(), cancellationToken);

            var saveResult = await amenityRepo.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync($"Amenity Item was successfully added to room");
        }
    }
}
