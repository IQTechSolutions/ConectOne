using AccomodationModule.Domain.Arguments.Requests;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using AccomodationModule.Domain.RequestFeatures;
using ConectOne.Domain.Extensions;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.RequestFeatures;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Application.RestServices
{
    /// <summary>
    /// Provides a set of methods for managing vouchers and their associated data.
    /// </summary>
    /// <remarks>This service acts as an abstraction layer for interacting with voucher-related data,
    /// including retrieving, creating, updating, and deleting vouchers, as well as managing their associations with
    /// rooms and users. It communicates with an underlying HTTP provider to perform these operations.</remarks>
    /// <param name="provider"></param>
    public class VoucherRestService(IBaseHttpProvider provider) : IVoucherService
    {
        /// <summary>
        /// Asynchronously retrieves the total count of vouchers based on the specified query parameters.
        /// </summary>
        /// <remarks>This method sends a request to the underlying provider to retrieve the count of
        /// vouchers. The query parameters specified in <paramref name="pageParameters"/> are used to construct the
        /// query string for the request.</remarks>
        /// <param name="pageParameters">The parameters used to filter and paginate the voucher count query. Must not be <c>null</c>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// where <c>T</c> is an <see cref="int"/> representing the total count of vouchers matching the specified
        /// parameters.</returns>
        public async Task<IBaseResult<int>> VoucherCountAsync(VoucherPageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<int>($"vouchers/count?{pageParameters.GetQueryString()}");
            return result;
        }

        /// <summary>
        /// Retrieves a paginated list of vouchers based on the specified page parameters.
        /// </summary>
        /// <remarks>This method queries the underlying data source to retrieve vouchers in a paginated
        /// format. The <paramref name="pageParameters"/> parameter allows specifying the page size, page number, and
        /// any additional filtering criteria. If no vouchers match the criteria, the result will contain an empty
        /// list.</remarks>
        /// <param name="pageParameters">The parameters that define the pagination and filtering options for the vouchers.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{VoucherDto}"/> containing the paginated list of vouchers.</returns>
        public async Task<PaginatedResult<VoucherDto>> PagedVouchersAsync(VoucherPageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetPagedAsync<VoucherDto, VoucherPageParameters>($"vouchers", pageParameters);
            return result;
        }

        /// <summary>
        /// Retrieves a list of all available vouchers asynchronously.
        /// </summary>
        /// <remarks>This method sends a request to the underlying provider to fetch all vouchers. The
        /// returned result encapsulates the operation's outcome, including any potential errors.</remarks>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with a list of <see cref="VoucherDto"/> instances representing the available vouchers.</returns>
        public async Task<IBaseResult<List<VoucherDto>>> AllVouchersAsync(CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<List<VoucherDto>>("vouchers/all");
            return result;
        }

        /// <summary>
        /// Retrieves a voucher by its unique identifier.
        /// </summary>
        /// <remarks>This method sends a request to retrieve a voucher with the specified identifier. If
        /// the voucher does not exist, the result may indicate an error or an empty response, depending on the
        /// implementation of the underlying provider.</remarks>
        /// <param name="voucherId">The unique identifier of the voucher to retrieve.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// of type <see cref="VoucherDto"/> representing the retrieved voucher.</returns>
        public async Task<IBaseResult<VoucherDto>> VoucherAsync(int voucherId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<VoucherDto>($"vouchers/{voucherId}");
            return result;
        }

        /// <summary>
        /// Creates a new voucher asynchronously.
        /// </summary>
        /// <remarks>This method sends the provided voucher details to the underlying provider for
        /// creation. Ensure that the <paramref name="model"/> contains all required fields before calling this
        /// method.</remarks>
        /// <param name="model">The <see cref="VoucherDto"/> object containing the details of the voucher to be created.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the created <see cref="VoucherDto"/> if the operation is successful.</returns>
        public async Task<IBaseResult<VoucherDto>> CreateVoucherAsync(VoucherDto model, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<VoucherDto, VoucherDto>($"vouchers", model);
            return result;
        }

        /// <summary>
        /// Updates an existing voucher asynchronously.
        /// </summary>
        /// <remarks>This method sends the updated voucher details to the server using an HTTP POST
        /// request.  Ensure that the provided <paramref name="model"/> contains valid data before calling this
        /// method.</remarks>
        /// <param name="model">The <see cref="VoucherDto"/> object containing the updated voucher details.  This parameter cannot be <see
        /// langword="null"/>.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.  The default value is <see
        /// cref="CancellationToken.None"/>.</param>
        /// <returns>An <see cref="IBaseResult"/> representing the result of the update operation.  The result contains the
        /// updated voucher details if the operation is successful.</returns>
        public async Task<IBaseResult> UpdateVoucherAsync(VoucherDto model, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<VoucherDto, VoucherDto>($"vouchers", model);
            return result;
        }

        /// <summary>
        /// Deletes a voucher with the specified identifier.
        /// </summary>
        /// <remarks>This method sends a delete request to the underlying provider to remove the specified
        /// voucher.  Ensure the <paramref name="voucherId"/> corresponds to an existing voucher.</remarks>
        /// <param name="voucherId">The unique identifier of the voucher to delete.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the delete operation.</returns>
        public async Task<IBaseResult> DeleteVoucherAsync(int voucherId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"vouchers/{voucherId}", "");
            return result;
        }

        /// <summary>
        /// Retrieves the collection of rooms associated with a specific voucher.
        /// </summary>
        /// <remarks>This method sends a request to the underlying provider to retrieve the rooms
        /// associated with the specified voucher. Ensure that the <paramref name="voucherId"/> is valid and corresponds
        /// to an existing voucher.</remarks>
        /// <param name="voucherId">The unique identifier of the voucher whose associated rooms are to be retrieved.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// containing a collection of <see cref="RoomDto"/> objects representing the rooms associated with the
        /// specified voucher.</returns>
        public async Task<IBaseResult<ICollection<RoomDto>>> VoucherRoomsAsync(int voucherId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<ICollection<RoomDto>>($"vouchers/{voucherId}/voucherRooms");
            return result;
        }

        /// <summary>
        /// Retrieves the details of a room associated with a specific voucher.
        /// </summary>
        /// <remarks>This method sends a request to retrieve the room details associated with the
        /// specified voucher. Ensure that the provided identifiers are valid and that the cancellation token is used
        /// appropriately  to handle operation cancellations.</remarks>
        /// <param name="voucherId">The unique identifier of the voucher.</param>
        /// <param name="roomId">The unique identifier of the room to retrieve.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the details of the room as a <see cref="RoomDto"/>.</returns>
        public async Task<IBaseResult<RoomDto>> VoucherRoomAsync(int voucherId, int roomId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<RoomDto>($"vouchers/{voucherId}/voucherRooms/{roomId}");
            return result;
        }

        /// <summary>
        /// Creates a voucher room using the specified data transfer object.
        /// </summary>
        /// <remarks>This method sends a request to create a voucher room using the provided data. Ensure
        /// that the <paramref name="dto"/> contains all required fields before calling this method.</remarks>
        /// <param name="dto">The data transfer object containing the details of the voucher room to be created.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> CreateVoucherRoomAsync(VoucherRoomDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync($"vouchers/createvoucherRoom", dto);
            return result;
        }

        /// <summary>
        /// Maps a voucher to a room using the provided mapping details.
        /// </summary>
        /// <remarks>This method sends the mapping details to the provider for processing. Ensure that the
        /// <paramref name="dto"/> parameter contains valid data before calling this method.</remarks>
        /// <param name="dto">The data transfer object containing the voucher and room mapping details.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the outcome of the mapping
        /// operation.</returns>
        public async Task<IBaseResult> MapVoucherRoomAsync(VoucherMappingDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync($"vouchers/maproom", dto);
            return result;
        }

        /// <summary>
        /// Removes the association between a voucher and a room asynchronously.
        /// </summary>
        /// <remarks>This method sends a request to remove the specified room from the voucher. Ensure
        /// that the provided <paramref name="voucherId"/> and <paramref name="roomId"/> are valid and exist in the
        /// system.</remarks>
        /// <param name="voucherId">The unique identifier of the voucher whose room association is to be removed.</param>
        /// <param name="roomId">The unique identifier of the room to be disassociated from the voucher.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveVoucherRoomsAsync(int voucherId, int roomId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"{voucherId}/voucherRooms/{roomId}/delete", "");
            return result;
        }

        /// <summary>
        /// Retrieves a paginated list of user vouchers for the specified user.
        /// </summary>
        /// <remarks>This method fetches user vouchers from the underlying data provider using the
        /// specified pagination parameters. The result includes both the data and metadata about the pagination
        /// state.</remarks>
        /// <param name="userId">The unique identifier of the user whose vouchers are to be retrieved. Cannot be null or empty.</param>
        /// <param name="pageParameters">The pagination parameters, including page size and page number, used to control the paginated result set.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a collection of <see cref="UserVoucherDto"/> objects
        /// representing the user's vouchers, along with pagination metadata.</returns>
        public async Task<PaginatedResult<UserVoucherDto>> PagedUserVouchersAsync(string userId, VoucherPageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetPagedAsync<UserVoucherDto, RequestParameters>($"vouchers/user/{userId}", pageParameters);
            return result;
        }

        /// <summary>
        /// Retrieves the details of a user voucher based on the specified voucher ID.
        /// </summary>
        /// <remarks>This method communicates with an external provider to retrieve the user voucher
        /// details. Ensure that the <paramref name="userVoucherId"/> is valid and corresponds to an existing
        /// voucher.</remarks>
        /// <param name="userVoucherId">The unique identifier of the user voucher to retrieve. This value cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the details of the user voucher as a <see cref="UserVoucherDto"/>. If the voucher is not found,
        /// the result may indicate an error or an empty response, depending on the implementation.</returns>
        public async Task<IBaseResult<UserVoucherDto>> UserVoucherAsync(string userVoucherId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<UserVoucherDto>($"vouchers/user/voucher/{userVoucherId}");
            return result;
        }

        /// <summary>
        /// Creates a user voucher asynchronously.
        /// </summary>
        /// <remarks>This method sends a request to create a user voucher and returns the result of the
        /// operation. Ensure that the <paramref name="request"/> parameter is properly populated before calling this
        /// method.</remarks>
        /// <param name="request">The request object containing the details required to create the user voucher.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// representing the outcome of the operation.</returns>
        public async Task<IBaseResult> CreateUserVoucherAsync(CreateUserVoucherRequest request, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync($"vouchers/user/voucher", request);
            return result;
        }

        /// <summary>
        /// Removes a user voucher asynchronously.
        /// </summary>
        /// <remarks>This method sends a request to delete the specified user voucher. Ensure the
        /// <paramref name="userVoucherId"/> corresponds to a valid voucher before calling this method.</remarks>
        /// <param name="userVoucherId">The unique identifier of the user voucher to be removed. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>An <see cref="IBaseResult"/> representing the result of the operation.</returns>
        public async Task<IBaseResult> RemoveUserVoucherAsync(string userVoucherId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"vouchers/user/voucher", userVoucherId);
            return result;
        }
    }
}
