

using AccomodationModule.Domain.Arguments.Requests;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Interfaces;
using AccomodationModule.Domain.RequestFeatures;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AccomodationModule.Infrastructure.Implimentation
{
    /// <summary>
    /// Provides functionality for managing vouchers, including operations such as retrieval, creation,  updating,
    /// deletion, and mapping vouchers to rooms or users.
    /// </summary>
    /// <remarks>This service interacts with multiple repositories to perform operations on vouchers,
    /// lodgings, rooms,  and user vouchers. It supports asynchronous methods for efficient data handling and includes
    /// features  such as pagination, filtering, and mapping entities.</remarks>
    /// <param name="voucherService">The injected voucher repository</param>
    /// <param name="lodgingService">The injected lodging repository</param>
    /// <param name="voucherRoomService">The injected voucher room repository</param>
    /// <param name="userVoucherService">The injected user voucher repository</param>
	public class VoucherService(IRepository<Voucher, int> voucherService, IRepository<Lodging, string> lodgingService, IRepository<Room, int> voucherRoomService, IRepository<UserVoucher, string> userVoucherService) : IVoucherService
	{
        /// <summary>
        /// Asynchronously retrieves the count of vouchers based on the specified page parameters.
        /// </summary>
        /// <remarks>This method queries the voucher service to retrieve a filtered list of vouchers and
        /// calculates the count. The operation may fail if the underlying service encounters an error.</remarks>
        /// <param name="pageParameters">The parameters used to filter and paginate the voucher list.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>An <see cref="IBaseResult"/> containing the count of vouchers that match the specified criteria. If the
        /// operation fails, the result will include error messages.</returns>
        public async Task<IBaseResult<int>> VoucherCountAsync(VoucherPageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var result = await voucherService.ListAsync(false, cancellationToken);
            if (result.Succeeded)
                return await Result<int>.SuccessAsync(result.Data.Count());
            return await Result<int>.FailAsync(result.Messages);
        }

        /// <summary>
        /// Retrieves a paginated list of vouchers based on the specified page parameters.
        /// </summary>
        /// <remarks>This method retrieves vouchers along with their associated lodging details, images,
        /// and rooms. The returned data is transformed into <see cref="VoucherDto"/> objects for easier
        /// consumption.</remarks>
        /// <param name="pageParameters">The parameters that define the pagination settings, including page number and page size.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A <see cref="PaginatedResult{VoucherDto}"/> containing the paginated list of vouchers. If the operation
        /// succeeds, the result includes the vouchers and pagination metadata. If the operation fails, the result
        /// contains error messages.</returns>
        public async Task<PaginatedResult<VoucherDto>> PagedVouchersAsync(VoucherPageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Voucher>(c => true);
            spec.AddInclude(c => c.Include(c => c.Lodging).ThenInclude(c => c!.Images));
            spec.AddInclude(c => c.Include(c => c.Rooms));

            var result = await voucherService.ListAsync(spec, false, cancellationToken);
			if(result.Succeeded)
                return PaginatedResult<VoucherDto>.Success(result.Data.Select(voucher => new VoucherDto(voucher)).ToList(), result.Data.Count, pageParameters.PageNr, pageParameters.PageSize);
            return PaginatedResult<VoucherDto>.Failure(result.Messages);
		}

        /// <summary>
        /// Retrieves all vouchers asynchronously, including related lodging and image data.
        /// </summary>
        /// <remarks>This method retrieves all vouchers from the underlying data source and maps them to
        /// <see cref="VoucherDto"/> objects. Lodging and associated images are included in the response. Ensure that
        /// the cancellation token is properly handled to avoid unnecessary resource usage.</remarks>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see langword="default"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with a list of <see cref="VoucherDto"/> objects representing the vouchers. If the operation fails, the
        /// result includes error messages.</returns>
		public async Task<IBaseResult<List<VoucherDto>>> AllVouchersAsync(CancellationToken cancellationToken = default)
		{
			var result = voucherService.FindAll(false);
			if(!result.Succeeded) return await Result<List<VoucherDto>>.FailAsync(result.Messages);

            var response = await result.Data.Include(c => c.Lodging).ThenInclude(c => c!.Images).Select(c => new VoucherDto(c)).ToListAsync();
            return await Result<List<VoucherDto>>.SuccessAsync(response);
        }

        /// <summary>
        /// Retrieves a voucher by its unique identifier and returns detailed information about the voucher.
        /// </summary>
        /// <remarks>This method retrieves detailed information about a voucher, including its associated
        /// lodging, images, settings, amenities, and rooms. The method uses a specification pattern to include related
        /// entities in the query.</remarks>
        /// <param name="voucherId">The unique identifier of the voucher to retrieve. Must be a valid, existing voucher ID.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>An asynchronous task that returns an <see cref="IBaseResult{VoucherDto}"/> containing the voucher details.
        /// If the operation succeeds, the result contains a <see cref="VoucherDto"/> with detailed information about
        /// the voucher. If the operation fails, the result contains error messages describing the failure.</returns>
		public async Task<IBaseResult<VoucherDto>> VoucherAsync(int voucherId, CancellationToken cancellationToken = default)
		{
            var spec = new LambdaSpec<Voucher>(c => c.Id == voucherId);
            spec.AddInclude(c => c.Include(c => c.Lodging).ThenInclude(c => c!.Images));
            spec.AddInclude(c => c.Include(c => c.Lodging).ThenInclude(c => c!.Settings));
            spec.AddInclude(c => c.Include(c => c.Lodging).ThenInclude(c => c!.Amneties).ThenInclude(c => c.Amenity));
            spec.AddInclude(c => c.Include(c => c.Rooms).ThenInclude(c => c.Images));


            var result = await voucherService.FirstOrDefaultAsync(spec, cancellationToken: cancellationToken);
			if(!result.Succeeded) return await Result<VoucherDto>.FailAsync(result.Messages);
            return await Result<VoucherDto>.SuccessAsync(new VoucherDto(result.Data!));

        }

        /// <summary>
        /// Creates a new voucher asynchronously based on the provided model.
        /// </summary>
        /// <remarks>This method validates the lodging specified in the <paramref name="model"/> before
        /// creating the voucher. If the lodging does not exist or the creation fails, the result will indicate failure
        /// with appropriate error messages.</remarks>
        /// <param name="model">The <see cref="VoucherDto"/> containing the details of the voucher to be created. The <see
        /// cref="VoucherDto.LodgingId"/> property must correspond to an existing lodging.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// where <c>T</c> is <see cref="VoucherDto"/>. If successful, the result contains the created voucher; 
        /// otherwise, it contains error messages indicating the failure.</returns>
		public async Task<IBaseResult<VoucherDto>> CreateVoucherAsync(VoucherDto model, CancellationToken cancellationToken = default)
		{
            var spec = new LambdaSpec<Lodging>(c => c.Id == model.LodgingId);
            spec.AddInclude(c => c.Include(c => c.Settings));

            var result = await lodgingService.FirstOrDefaultAsync(spec, false, cancellationToken);
			if(!result.Succeeded) return await Result<VoucherDto>.FailAsync(result.Messages);

            var voucher = model.ToVoucher();
            voucher.LodgingId = model.LodgingId;

            var newVoucher = await voucherService.CreateAsync(voucher, cancellationToken);
            if (!newVoucher.Succeeded) return await Result<VoucherDto>.FailAsync(newVoucher.Messages);

            newVoucher.Data.Lodging = result.Data;
            return await Result<VoucherDto>.SuccessAsync(new VoucherDto(newVoucher.Data));
        }

        /// <summary>
        /// Updates an existing voucher with the provided details.
        /// </summary>
        /// <remarks>This method updates the voucher's details, including its title, descriptions, rate,
        /// markup percentage, commission, features, terms, active status, and lodging ID. Ensure that the <paramref
        /// name="model"/> contains valid data and that the voucher exists before calling this method.</remarks>
        /// <param name="model">The <see cref="VoucherDto"/> containing the updated voucher information. The <see
        /// cref="VoucherDto.VoucherId"/> must correspond to an existing voucher.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>:
        /// <list type="bullet"> <item> <description>Returns a success result if the voucher is updated successfully,
        /// including a message with the updated voucher's title.</description> </item> <item> <description>Returns a
        /// failure result if the update operation fails, including error messages describing the failure.</description>
        /// </item> </list></returns>
		public async Task<IBaseResult> UpdateVoucherAsync(VoucherDto model, CancellationToken cancellationToken = default)
		{
            var spec = new LambdaSpec<Voucher>(c => c.Id == model.VoucherId);

            var result = await voucherService.FirstOrDefaultAsync(spec, true, cancellationToken);
            if (!result.Succeeded) return await Result.FailAsync(result.Messages);

            var response = result.Data;
            response!.Title = model.Title;
            response.ShortDescription = model.ShortDescription;
            response.LongDescription = model.LongDescription;
            response.Rate = model.Rate;
            response.MarkupPercentage = model.MarkupPercentage;
            response.Commission = model.Commission;
            response.Features = model.Features;
            response.Terms = model.Terms;
            response.Active = model.Active;
            response.LodgingId = model.LodgingId;

            voucherService.Update(response);
            var saveResult = await voucherService.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync($"{response.Title} successfully updated");
        }

        /// <summary>
        /// Deletes a voucher with the specified ID and attempts to save the changes.
        /// </summary>
        /// <remarks>This method deletes the specified voucher and saves the changes to the underlying
        /// data store. If the save operation fails, the method returns a failure result containing the error
        /// messages.</remarks>
        /// <param name="voucherId">The unique identifier of the voucher to delete.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success or failure of the operation. If successful, the result contains a success
        /// message. Otherwise, the result contains error messages describing the failure.</returns>
		public async Task<IBaseResult> DeleteVoucherAsync(int voucherId, CancellationToken cancellationToken = default)
		{
            await voucherService.DeleteAsync(voucherId, cancellationToken);
            var saveResult = await voucherService.SaveAsync(cancellationToken);
            if (saveResult.Succeeded)
            {
                return await Result.SuccessAsync($"Voucher successfully updated");
            }
            return Result.Fail(saveResult.Messages);
		}

        /// <summary>
        /// Retrieves a collection of rooms associated with the specified voucher.
        /// </summary>
        /// <remarks>This method queries the underlying data source to find rooms linked to the specified
        /// voucher. The returned result indicates whether the operation succeeded or failed, along with the associated
        /// data or error messages.</remarks>
        /// <param name="voucherId">The unique identifier of the voucher for which the associated rooms are to be retrieved.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with a collection of <see cref="RoomDto"/> objects representing the rooms associated with the voucher. If
        /// the operation fails, the result contains error messages.</returns>
		public async Task<IBaseResult<ICollection<RoomDto>>> VoucherRoomsAsync(int voucherId, CancellationToken cancellationToken = default)
		{
            var spec = new LambdaSpec<Room>(c => c.VoucherId == voucherId);
            var result = await voucherRoomService.ListAsync(spec, false, cancellationToken);
			if (!result.Succeeded) return await Result<ICollection<RoomDto>>.FailAsync(result.Messages);

            var rooms = result.Data.Select(c => new RoomDto(c)).ToList();
            return await Result<ICollection<RoomDto>>.SuccessAsync(rooms);
        }

        /// <summary>
        /// Creates a mapping between a voucher and a room asynchronously.
        /// </summary>
        /// <remarks>This method attempts to associate a voucher with a room based on the identifiers
        /// provided in the <paramref name="dto"/>. It performs validation to ensure that both the voucher and the room
        /// exist in the database before creating the mapping. If either the voucher or the room cannot be found, or if
        /// the mapping operation fails, the method returns a failure result with appropriate error messages.</remarks>
        /// <param name="dto">The data transfer object containing the voucher and room identifiers.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A result indicating the success or failure of the operation. If successful, the result contains a success
        /// message. If unsuccessful, the result contains error messages describing the failure.</returns>
		public async Task<IBaseResult> CreateVoucherRoomAsync(VoucherRoomDto dto, CancellationToken cancellationToken = default)
		{
            var spec = new LambdaSpec<Voucher>(c => c.Id == dto.VoucherId);

            var voucherResult = await voucherService.FirstOrDefaultAsync(spec, false, cancellationToken);
			if (!voucherResult.Succeeded) return await Result.FailAsync(voucherResult.Messages);

            if (voucherResult.Data == null)
                return await Result.FailAsync($"No voucher matching id '{dto.VoucherId}' found in the database");

            var roomspec = new LambdaSpec<Room>(c => c.Id == dto.RoomId);

            var roomresult = await voucherRoomService.FirstOrDefaultAsync(roomspec, false, cancellationToken);
            if (!roomresult.Succeeded) return await Result.FailAsync(roomresult.Messages);

            if (roomresult.Data == null)
                return await Result.FailAsync($"No room matching id '{dto.RoomId}' found in the database");

            var voucherRoom = new Room(roomresult.Data) { VoucherId = voucherResult.Data.Id };
            var creationResult = await voucherRoomService.CreateAsync(voucherRoom, cancellationToken);
            if (creationResult.Succeeded)
            {
                return await Result<ICollection<RoomDto>>.SuccessAsync($"{voucherRoom.Name} successfully mapped to {voucherResult.Data.Title}");
            }
            return await Result.FailAsync(creationResult.Messages);
        }

        /// <summary>
        /// Maps a room to a voucher within a specified lodging.
        /// </summary>
        /// <remarks>This method validates the existence of the lodging and room specified in the
        /// <paramref name="dto"/>. If either the lodging or room cannot be found, or if the mapping operation fails,
        /// the method returns a failure result.</remarks>
        /// <param name="dto">The data transfer object containing the voucher ID, lodging ID, and room ID to be mapped.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>An asynchronous operation that returns a result indicating the success or failure of the mapping process. If
        /// successful, the result contains a success message.</returns>
        public async Task<IBaseResult> MapVoucherRoomAsync(VoucherMappingDto dto, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Lodging>(c => c.Id == dto.LodgingId);
            var lodgingResult = await lodgingService.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!lodgingResult.Succeeded) return await Result.FailAsync(lodgingResult.Messages);

            var lodging = lodgingResult.Data;
            if (lodging == null) return await Result.FailAsync($"No lodging matching id '{dto.VoucherId}' found in the database");

            var roomSpec = new LambdaSpec<Room>(c => c.Id.ToString().Equals(dto.RoomId));
            var roomResult = await voucherRoomService.FirstOrDefaultAsync(roomSpec, false, cancellationToken);
            if (!roomResult.Succeeded) return await Result.FailAsync(roomResult.Messages);

            var room = roomResult.Data;
            if (room == null) return await Result.FailAsync($"No room matching id '{dto.RoomId}' found in the database");

            var voucherRoom = new Room(room) { VoucherId = dto.VoucherId };
            var creationResult = await voucherRoomService.CreateAsync(voucherRoom, cancellationToken);
            if (!creationResult.Succeeded) return await Result.FailAsync(creationResult.Messages);

            return await Result<ICollection<RoomDto>>.SuccessAsync($"{voucherRoom.Name} succesfully mapped from {lodging.Name}");
        }

        /// <summary>
        /// Retrieves a room associated with a specific voucher and room ID.
        /// </summary>
        /// <remarks>This method queries the database for a room associated with the specified voucher and
        /// room ID. If the room is found, its details are returned as a <see cref="RoomDto"/> object. If no matching
        /// room is found, or if the operation fails, an error result is returned.</remarks>
        /// <param name="voucherId">The unique identifier of the voucher.</param>
        /// <param name="roomId">The unique identifier of the room.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>An <see cref="IBaseResult{RoomDto}"/> containing the room details if found, or an error result if the room
        /// does not exist or the operation fails.</returns>
        public async Task<IBaseResult<RoomDto>> VoucherRoomAsync(int voucherId, int roomId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Room>(c => c.VoucherId == voucherId && c.Id == roomId);
            spec.AddInclude(c => c.Include(c => c.Images));

            var result = await voucherRoomService.FirstOrDefaultAsync(spec, false, cancellationToken);
			if(!result.Succeeded) return await Result<RoomDto>.FailAsync(result.Messages);

            if (result.Data == null)
                return await Result<RoomDto>.FailAsync($"No room matching id '{voucherId}' found in the database");

            return await Result<RoomDto>.SuccessAsync(new RoomDto(result.Data));
        }

        /// <summary>
        /// Removes a room associated with a specific voucher.
        /// </summary>
        /// <remarks>This method removes the association between a voucher and a room.  It validates the
        /// existence of the specified voucher and room before performing the removal.</remarks>
        /// <param name="voucherId">The unique identifier of the voucher.</param>
        /// <param name="roomId">The unique identifier of the room to be removed.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>An <see cref="IBaseResult"/> indicating the outcome of the operation.  If successful, the result contains a
        /// success message; otherwise, it contains error messages.</returns>
        public async Task<IBaseResult> RemoveVoucherRoomsAsync(int voucherId, int roomId, CancellationToken cancellationToken = default)
		{
            var spec = new LambdaSpec<Room>(c => c.VoucherId == voucherId && c.Id == roomId);

            var result = await voucherRoomService.FirstOrDefaultAsync(spec, false, cancellationToken);
			if(!result.Succeeded) return await Result.FailAsync(result.Messages);

            voucherRoomService.Delete(result.Data!);

            var saveResult = await voucherService.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync($"{result.Data!.Name} removed successfully");
        }


        #region User Vouchers

        /// <summary>
        /// Retrieves a paginated list of user vouchers based on the specified user ID and pagination parameters.
        /// </summary>
        /// <remarks>This method queries the user's vouchers and includes related data such as voucher
        /// details, lodging information, associated images, rooms, and addresses. If the operation fails, the returned
        /// result will indicate failure along with error messages.</remarks>
        /// <param name="userId">The unique identifier of the user whose vouchers are to be retrieved.</param>
        /// <param name="pageParameters">The pagination parameters, including page number and page size, used to control the paginated result.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a collection of <see cref="UserVoucherDto"/> objects
        /// representing the user's vouchers, along with pagination metadata.</returns>
        public async Task<PaginatedResult<UserVoucherDto>> PagedUserVouchersAsync(string userId, VoucherPageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<UserVoucher>(c => c.UserId == userId);
            spec.AddInclude(c => c.Include(c => c.Voucher).ThenInclude(c => c.Lodging).ThenInclude(c => c.Images));
            spec.AddInclude(c => c.Include(c => c.Voucher).ThenInclude(c => c.Rooms));
            spec.AddInclude(c => c.Include(c => c.Voucher).ThenInclude(c => c.Lodging).ThenInclude(c => c.Address));

            var result = await userVoucherService.ListAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return PaginatedResult<UserVoucherDto>.Failure(result.Messages);

            var pagedData = result.Data
                .Select(c => new UserVoucherDto(userId, new VoucherDto(c.Voucher), new RoomDto(c.Voucher.Rooms.FirstOrDefault(r => r.Id == c.RoomId)))).ToList();
            return PaginatedResult<UserVoucherDto>.Success(pagedData, pagedData.Count, pageParameters.PageNr, pageParameters.PageSize);
        }

        /// <summary>
        /// Retrieves a user voucher based on the specified voucher ID.
        /// </summary>
        /// <remarks>This method queries the user voucher service to find a voucher matching the specified
        /// ID. If a matching voucher is found, it constructs a <see cref="UserVoucherDto"/> containing the user ID,
        /// voucher details, and associated room details. If no matching voucher is found or the operation fails, the
        /// method returns a failure result with relevant error messages.</remarks>
        /// <param name="userVoucherId">The unique identifier of the user voucher to retrieve. This parameter cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>An asynchronous task that returns an <see cref="IBaseResult{T}"/> containing a <see cref="UserVoucherDto"/>
        /// if the operation succeeds, or error messages if the operation fails.</returns>
        public async Task<IBaseResult<UserVoucherDto>> UserVoucherAsync(string userVoucherId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<UserVoucher>(c => c.Id == userVoucherId);

            var result = await userVoucherService.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (result.Succeeded)
            {
                return await Result<UserVoucherDto>.SuccessAsync(new UserVoucherDto(result.Data.UserId, new VoucherDto(result.Data.Voucher), new RoomDto(result.Data.Voucher.Rooms.FirstOrDefault(r => r.Id == result.Data.RoomId))));
            }
            return await Result<UserVoucherDto>.FailAsync(result.Messages);
        }

        /// <summary>
        /// Creates a user voucher based on the provided request.
        /// </summary>
        /// <remarks>This method creates a user voucher by associating a user, voucher, and room based on
        /// the provided request. Ensure that the request contains valid IDs for the user, voucher, and room.</remarks>
        /// <param name="request">The request containing the details of the user voucher to be created, including the user ID, voucher ID, and
        /// room ID.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>An <see cref="IBaseResult"/> representing the outcome of the operation. Returns a success result if the user
        /// voucher is created successfully, or a failure result with error messages if the operation fails.</returns>
        public async Task<IBaseResult> CreateUserVoucherAsync(CreateUserVoucherRequest request, CancellationToken cancellationToken = default)
        {
            var userVoucher = new UserVoucher() { RoomId = request.RoomId, VoucherId = request.VoucherId, UserId = request.UserId };

            var result = await userVoucherService.CreateAsync(userVoucher, cancellationToken);
            if (!result.Succeeded) return await Result.FailAsync(result.Messages);
            return await Result.SuccessAsync("User Voucher successfully created");
        }

        /// <summary>
        /// Removes a user voucher identified by the specified ID.
        /// </summary>
        /// <remarks>This method attempts to delete the specified user voucher. If the operation fails,
        /// the result will include the failure messages.</remarks>
        /// <param name="userVoucherId">The unique identifier of the user voucher to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional; defaults to <see langword="default"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation. If successful, the result includes a success message;
        /// otherwise, it contains error messages.</returns>
        public async Task<IBaseResult> RemoveUserVoucherAsync(string userVoucherId, CancellationToken cancellationToken = default)
        {
            var result = await userVoucherService.DeleteAsync(userVoucherId, cancellationToken);
            if(!result.Succeeded) return await Result.FailAsync(result.Messages);

            return await Result.SuccessAsync("User Voucher successfully removed");
        }

        #endregion
    }
}
