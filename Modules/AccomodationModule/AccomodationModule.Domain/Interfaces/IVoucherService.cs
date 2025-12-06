using AccomodationModule.Domain.Arguments.Requests;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.RequestFeatures;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Domain.Interfaces
{
    /// <summary>
    /// Defines a contract for managing vouchers and their associated data, including operations for  retrieving,
    /// creating, updating, and deleting vouchers, as well as managing voucher-room mappings  and user-specific
    /// vouchers.
    /// </summary>
    /// <remarks>This interface provides methods for working with vouchers and their related entities, such as
    /// rooms  and user vouchers. It supports asynchronous operations for retrieving paginated results, individual 
    /// voucher details, and performing CRUD operations. Additionally, it includes functionality for managing 
    /// voucher-room associations and user-specific voucher assignments.</remarks>
    public interface IVoucherService
    {
        /// <summary>
        /// Asynchronously retrieves the total count of vouchers based on the specified filtering and pagination
        /// parameters.
        /// </summary>
        /// <param name="pageParameters">The parameters used to filter and paginate the voucher data. Must not be <see langword="null"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// with the total count of vouchers matching the specified criteria.</returns>
        Task<IBaseResult<int>> VoucherCountAsync(VoucherPageParameters pageParameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a paginated list of vouchers based on the specified parameters.
        /// </summary>
        /// <remarks>Use this method to retrieve vouchers in a paginated format, which is useful for
        /// scenarios where the dataset is large and needs to be fetched in smaller chunks.</remarks>
        /// <param name="pageParameters">The parameters that define the pagination and filtering criteria for the vouchers. This includes page size,
        /// page number, and any additional filters.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A <see cref="PaginatedResult{VoucherDto}"/> containing the vouchers that match the specified criteria. The
        /// result includes pagination metadata such as total count and current page.</returns>
        Task<PaginatedResult<VoucherDto>> PagedVouchersAsync(VoucherPageParameters pageParameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a list of all available vouchers asynchronously.
        /// </summary>
        /// <remarks>This method returns a collection of vouchers, represented as <see cref="VoucherDto"/>
        /// objects, encapsulated in an <see cref="IBaseResult{T}"/>. The operation is performed asynchronously and
        /// supports cancellation via the provided <see cref="CancellationToken"/>.</remarks>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. If the operation is canceled, the task will complete with a
        /// <see cref="TaskCanceledException"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// wrapping a list of <see cref="VoucherDto"/> objects.</returns>
		Task<IBaseResult<List<VoucherDto>>> AllVouchersAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the details of a voucher based on its unique identifier.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch voucher details. Ensure that
        /// the provided <paramref name="voucherId"/> is valid and corresponds to an existing voucher. Use the <paramref
        /// name="cancellationToken"/> to cancel the operation if needed.</remarks>
        /// <param name="voucherId">The unique identifier of the voucher to retrieve. Must be a positive integer.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional; defaults to <see langword="default"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="IBaseResult{VoucherDto}"/> object, which includes the voucher details if the operation is successful.</returns>
		Task<IBaseResult<VoucherDto>> VoucherAsync(int voucherId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new voucher asynchronously based on the provided model.
        /// </summary>
        /// <remarks>This method performs the creation of a voucher and returns the result encapsulated in
        /// an <see cref="IBaseResult{VoucherDto}"/>. Ensure that the <paramref name="model"/> contains  valid data
        /// before calling this method.</remarks>
        /// <param name="model">The data transfer object containing the details of the voucher to be created. Must not be <see
        /// langword="null"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see
        /// cref="IBaseResult{VoucherDto}"/> object, which includes the created voucher details  and the operation's
        /// success status.</returns>
		Task<IBaseResult<VoucherDto>> CreateVoucherAsync(VoucherDto model, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing voucher with the specified details.
        /// </summary>
        /// <remarks>The operation will fail if the provided <paramref name="model"/> is invalid or if the
        /// voucher does not exist. Ensure that the <paramref name="cancellationToken"/> is used to handle cancellation
        /// scenarios gracefully.</remarks>
        /// <param name="model">The <see cref="VoucherDto"/> containing the updated voucher details. Cannot be <see langword="null"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the update operation.</returns>
		Task<IBaseResult> UpdateVoucherAsync(VoucherDto model, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a voucher with the specified ID asynchronously.
        /// </summary>
        /// <remarks>Use this method to remove a voucher from the system. Ensure that the voucher ID is
        /// valid and exists before calling this method. If the operation fails, the returned <see cref="IBaseResult"/>
        /// will contain details about the failure.</remarks>
        /// <param name="voucherId">The unique identifier of the voucher to delete. Must be a positive integer.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional; defaults to <see langword="default"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
		Task<IBaseResult> DeleteVoucherAsync(int voucherId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a collection of rooms associated with the specified voucher.
        /// </summary>
        /// <param name="voucherId">The unique identifier of the voucher for which the rooms are to be retrieved. Must be a positive integer.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of  <see
        /// cref="RoomDto"/> objects representing the rooms associated with the voucher. If no rooms are associated with
        /// the voucher, the collection will be empty.</returns>
		Task<IBaseResult<ICollection<RoomDto>>> VoucherRoomsAsync(int voucherId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a room associated with a specific voucher.
        /// </summary>
        /// <remarks>This method is used to retrieve information about a room that is linked to a specific
        /// voucher. Ensure that both <paramref name="voucherId"/> and <paramref name="roomId"/> are valid
        /// identifiers.</remarks>
        /// <param name="voucherId">The unique identifier of the voucher. Must be a positive integer.</param>
        /// <param name="roomId">The unique identifier of the room. Must be a positive integer.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="IBaseResult{RoomDto}"/> object, which includes the details of the room associated with the voucher.</returns>
		Task<IBaseResult<RoomDto>> VoucherRoomAsync(int voucherId, int roomId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new voucher room based on the provided data transfer object.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to create a voucher room. Ensure that
        /// the  <paramref name="dto"/> parameter contains valid data before calling this method.</remarks>
        /// <param name="dto">The data transfer object containing the details of the voucher room to be created. This parameter must not
        /// be null and should include all required fields for the voucher room.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see langword="default"/>,  which
        /// indicates no cancellation token is provided.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the success or failure of the operation.</returns>
        Task<IBaseResult> CreateVoucherRoomAsync(VoucherRoomDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Maps a voucher to a room based on the provided mapping details.
        /// </summary>
        /// <remarks>This method performs the mapping asynchronously and may involve external service
        /// calls. Ensure that the <paramref name="cancellationToken"/> is properly managed to avoid unnecessary
        /// resource usage.</remarks>
        /// <param name="dto">The data transfer object containing the voucher and room mapping information. This parameter must not be
        /// <see langword="null"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the success or failure of the mapping operation.</returns>
        Task<IBaseResult> MapVoucherRoomAsync(VoucherMappingDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a room from the specified voucher.
        /// </summary>
        /// <remarks>Use this method to disassociate a room from a voucher. Ensure that both the voucher
        /// and room identifiers are valid before calling this method.</remarks>
        /// <param name="voucherId">The unique identifier of the voucher from which the room will be removed.</param>
        /// <param name="roomId">The unique identifier of the room to be removed.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
		Task<IBaseResult> RemoveVoucherRoomsAsync(int voucherId, int roomId, CancellationToken cancellationToken = default);

        #region User Vouchers

        /// <summary>
        /// Retrieves a paginated list of user vouchers for the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose vouchers are being retrieved. Must not be <see langword="null"/> or
        /// empty.</param>
        /// <param name="pageParameters">The pagination parameters, including page size and page number. Must not be <see langword="null"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a collection of <see cref="UserVoucherDto"/> objects
        /// representing the user's vouchers, along with pagination metadata.</returns>
        Task<PaginatedResult<UserVoucherDto>> PagedUserVouchersAsync(string userId, VoucherPageParameters pageParameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the details of a user voucher based on the specified voucher ID.
        /// </summary>
        /// <remarks>Use this method to retrieve information about a specific user voucher, such as its
        /// status or associated user details. Ensure that the provided <paramref name="userVoucherId"/> is valid and
        /// corresponds to an existing voucher.</remarks>
        /// <param name="userVoucherId">The unique identifier of the user voucher to retrieve. This parameter cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the details of the user voucher as a <see cref="UserVoucherDto"/>.</returns>
        Task<IBaseResult<UserVoucherDto>> UserVoucherAsync(string userVoucherId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a voucher for a user based on the specified request.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to create a voucher for a user. Ensure
        /// that the <paramref name="request"/> contains valid data required for voucher creation. The operation can be
        /// canceled using the provided <paramref name="cancellationToken"/>.</remarks>
        /// <param name="request">The request containing the details required to create the user voucher. This cannot be null.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. If not provided, the default token is used.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the voucher creation process.</returns>
        Task<IBaseResult> CreateUserVoucherAsync(CreateUserVoucherRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a user voucher identified by the specified ID.
        /// </summary>
        /// <remarks>Use this method to remove a user voucher from the system. Ensure that the <paramref
        /// name="userVoucherId"/> corresponds to a valid voucher. If the operation is canceled via the <paramref
        /// name="cancellationToken"/>, the task will be marked as canceled.</remarks>
        /// <param name="userVoucherId">The unique identifier of the user voucher to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional; defaults to <see langword="default"/>.</param>
        /// <returns>An <see cref="IBaseResult"/> representing the outcome of the operation. The result indicates whether the
        /// removal was successful.</returns>
        Task<IBaseResult> RemoveUserVoucherAsync(string userVoucherId, CancellationToken cancellationToken = default);

        #endregion
    }
}
