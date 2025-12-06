
using AccomodationModule.Domain.Arguments.Requests;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using AccomodationModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Mvc;

namespace AccomodationModule.Infrastructure.Controllers
{
    /// <summary>
    /// Provides endpoints for managing vouchers and voucher-related operations.
    /// </summary>
    /// <remarks>This controller includes methods for retrieving, creating, updating, and deleting vouchers,
    /// as well as managing voucher rooms and user-specific vouchers. The endpoints are designed to handle paginated
    /// data, individual voucher details, and mappings between vouchers and rooms.</remarks>
    /// <param name="voucherService"></param>
    [Route("api/vouchers"), ApiController]
    public class VouchersController(IVoucherService voucherService) : ControllerBase
    {
        /// <summary>
        /// Retrieves the total count of vouchers based on the specified pagination and filtering parameters.
        /// </summary>
        /// <remarks>This method is an HTTP GET endpoint accessible at "count". It delegates the counting
        /// operation to the underlying service layer, which processes the provided <paramref
        /// name="pageParameters"/>.</remarks>
        /// <param name="pageParameters">The parameters used to define pagination and filtering criteria for the voucher count. This includes
        /// properties such as page size, page number, and any applicable filters.</param>
        /// <returns>An <see cref="IActionResult"/> containing the total count of vouchers that match the specified criteria. The
        /// result is returned as an HTTP 200 response with the count as the payload.</returns>
        [HttpGet("count")] public async Task<IActionResult> VouchersCountAsync([FromQuery] VoucherPageParameters pageParameters)
        {
            var result = await voucherService.VoucherCountAsync(pageParameters, HttpContext.RequestAborted);
            return Ok(result);
        }
        
        /// <summary>
        /// Retrieves a paginated list of vouchers based on the specified parameters.
        /// </summary>
        /// <remarks>This method is an HTTP GET endpoint and expects the pagination parameters to be
        /// provided as query string values. Ensure that the <paramref name="pageParameters"/> object contains valid
        /// values for page size and page number.</remarks>
        /// <param name="pageParameters">The parameters used to define the pagination and filtering of the vouchers.  This includes page size, page
        /// number, and any additional filtering criteria.</param>
        /// <returns>An <see cref="IActionResult"/> containing the paginated list of vouchers.  The result is returned as an HTTP
        /// 200 OK response with the data in the response body.</returns>
        [HttpGet] public async Task<IActionResult> PagedVouchersAsync([FromQuery]VoucherPageParameters pageParameters)
        {
            var result = await voucherService.PagedVouchersAsync(pageParameters, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves all available vouchers.
        /// </summary>
        /// <remarks>This method returns a collection of vouchers by invoking the corresponding service
        /// method. The result is returned as an HTTP 200 OK response containing the collection in the response
        /// body.</remarks>
        /// <returns>An <see cref="IActionResult"/> containing an HTTP 200 OK response with the collection of vouchers.</returns>
        [HttpGet("all")] public async Task<IActionResult> AllVouchersAsync()
        {
            var result = await voucherService.AllVouchersAsync();
            return Ok(result);
        }

        /// <summary>
        /// Retrieves the details of a voucher based on the specified voucher ID.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch voucher details from the
        /// underlying service. If the voucher ID does not exist, the response will indicate the failure (e.g., a 404
        /// Not Found status).</remarks>
        /// <param name="voucherId">The unique identifier of the voucher to retrieve. Must be a positive integer.</param>
        /// <returns>An <see cref="IActionResult"/> containing the voucher details if found, or an appropriate HTTP response
        /// indicating the result of the operation.</returns>
        [HttpGet("{voucherId}")] public async Task<IActionResult> VoucherAsync(int voucherId)
        {
            var model = await voucherService.VoucherAsync(voucherId, HttpContext.RequestAborted);
            return Ok(model);
        }

        /// <summary>
        /// Creates a new voucher based on the provided data.
        /// </summary>
        /// <remarks>This method uses the <see cref="VoucherDto"/> object to create a new voucher
        /// asynchronously. Ensure that the provided data in <paramref name="model"/> meets all required validation
        /// criteria.</remarks>
        /// <param name="model">The data for the voucher to be created. Must not be null.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.  Returns an HTTP 200 response with
        /// the created voucher if successful.</returns>
        [HttpPut] public async Task<IActionResult> Create([FromBody] VoucherDto model)
        {
            var lodging = await voucherService.CreateVoucherAsync(model, HttpContext.RequestAborted);
            return Ok(lodging);
        }

        /// <summary>
        /// Updates an existing voucher with the provided details.
        /// </summary>
        /// <remarks>This method processes an HTTP POST request to update a voucher. Ensure that the
        /// <paramref name="model"/> contains valid data before calling this method.</remarks>
        /// <param name="model">The data transfer object containing the updated voucher details. Cannot be null.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the update operation. Typically, this will be an
        /// HTTP 200 response with the updated voucher data.</returns>
        [HttpPost] public async Task<IActionResult> Edit([FromBody] VoucherDto model)
        {
            var result = await voucherService.UpdateVoucherAsync(model, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a voucher with the specified ID.
        /// </summary>
        /// <remarks>This method performs an HTTP DELETE operation to remove a voucher. Ensure that the
        /// provided  <paramref name="voucherId"/> corresponds to an existing voucher in the system.</remarks>
        /// <param name="voucherId">The unique identifier of the voucher to be deleted. Must be a valid, existing voucher ID.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns a success response if the
        /// voucher was deleted successfully.</returns>
        [HttpDelete("{voucherId}")] public async Task<IActionResult> DeleteConformation(int voucherId)
        {
			var result = await voucherService.DeleteVoucherAsync(voucherId, HttpContext.RequestAborted);
			return Ok(result);
		}

        /// <summary>
        /// Retrieves the rooms associated with a specific voucher.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch the rooms linked to the given
        /// voucher. The result is returned as an HTTP 200 OK response containing the room data.</remarks>
        /// <param name="voucherId">The unique identifier of the voucher for which the associated rooms are to be retrieved. Must be a positive
        /// integer.</param>
        /// <returns>An <see cref="IActionResult"/> containing the list of rooms associated with the specified voucher. The
        /// response is serialized as JSON.</returns>
		[HttpGet("{voucherId}/voucherRooms")] public async Task<IActionResult> VoucherRoomsAsync(int voucherId)
		{
			var model = await voucherService.VoucherRoomsAsync(voucherId, HttpContext.RequestAborted);
			return Ok(model);
		}

        /// <summary>
        /// Retrieves information about a specific room associated with a voucher.
        /// </summary>
        /// <remarks>This method calls the underlying service to fetch the room details for the given
        /// voucher. Ensure that both <paramref name="voucherId"/> and <paramref name="roomId"/> are valid
        /// identifiers.</remarks>
        /// <param name="voucherId">The unique identifier of the voucher.</param>
        /// <param name="roomId">The unique identifier of the room, provided as a string.</param>
        /// <returns>An <see cref="IActionResult"/> containing the room details associated with the specified voucher. The
        /// response is typically serialized as JSON.</returns>
        [HttpGet("{voucherId}/voucherRooms/{roomId}")] public async Task<IActionResult> VoucherRoomAsync(int voucherId, string roomId)
        {
            var model = await voucherService.VoucherRoomAsync(voucherId, Convert.ToInt32(roomId), HttpContext.RequestAborted);
            return Ok(model);
        }

        /// <summary>
        /// Creates a new voucher room based on the provided data transfer object.
        /// </summary>
        /// <remarks>This method processes the provided <paramref name="dto"/> to create a voucher room
        /// and returns the result in an HTTP response. Ensure that the <paramref name="dto"/>  contains valid data as
        /// required by the underlying service.</remarks>
        /// <param name="dto">The data transfer object containing the details of the voucher room to be created. This parameter must not
        /// be <see langword="null"/>.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation. If successful, the response includes
        /// the created voucher room model.</returns>
        [HttpPut("createvoucherRoom")] public async Task<IActionResult> CreateVoucherRoomAsync([FromBody] VoucherRoomDto dto)
        {
            var model = await voucherService.CreateVoucherRoomAsync(dto, HttpContext.RequestAborted);
            return Ok(model);
        }

        /// <summary>
        /// Maps a voucher to a room based on the provided mapping details.
        /// </summary>
        /// <remarks>This endpoint processes the mapping of a voucher to a room using the provided
        /// details. Ensure that the <paramref name="dto"/> contains valid and complete information for the
        /// mapping.</remarks>
        /// <param name="dto">The data transfer object containing the mapping details, including voucher and room information. Must not be
        /// null.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the mapping operation. Typically returns an HTTP 200
        /// status with the mapping result if successful.</returns>
        [HttpPost("maproom")] public async Task<IActionResult> MapVoucherRoomAsync([FromBody] VoucherMappingDto dto)
		{
			var model = await voucherService.MapVoucherRoomAsync(dto, HttpContext.RequestAborted);
			return Ok(model);
		}

        /// <summary>
        /// Removes the association between a voucher and a room.
        /// </summary>
        /// <remarks>This method is an HTTP GET endpoint that removes the specified room from the voucher.
        /// Ensure that both <paramref name="voucherId"/> and <paramref name="roomId"/> are valid identifiers.</remarks>
        /// <param name="voucherId">The unique identifier of the voucher to be updated.</param>
        /// <param name="roomId">The unique identifier of the room to be removed from the voucher.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation. Typically, this includes a success
        /// status and any relevant data.</returns>
		[HttpGet("{voucherId}/voucherRooms/{roomId}/delete")] public async Task<IActionResult> RemoveVoucherRoomAsync(int voucherId, int roomId)
		{
			var result = await voucherService.RemoveVoucherRoomsAsync(voucherId, roomId, HttpContext.RequestAborted);
			return Ok(result);
		}
        
        #region User Vouchers

        /// <summary>
        /// Retrieves a paginated list of vouchers associated with a specific user.
        /// </summary>
        /// <remarks>This method uses the provided pagination parameters to limit the number of vouchers
        /// returned in a single response. Ensure that <paramref name="pageParameters"/> contains valid values for page
        /// number and page size.</remarks>
        /// <param name="userId">The unique identifier of the user whose vouchers are being retrieved.</param>
        /// <param name="pageParameters">The pagination parameters, including page number and page size, used to control the paginated result.</param>
        /// <returns>An <see cref="IActionResult"/> containing the paginated list of vouchers for the specified user. The result
        /// is returned in the HTTP response body.</returns>
        [HttpGet("user/{userId}")] public async Task<IActionResult> PagedUserVouchersAsync(string userId, [FromQuery] VoucherPageParameters pageParameters)
        {
            var result = await voucherService.PagedUserVouchersAsync(userId, pageParameters, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves the details of a user voucher based on the specified voucher ID.
        /// </summary>
        /// <remarks>This endpoint is typically used to fetch information about a specific user voucher.
        /// Ensure that the <paramref name="userVoucherId"/> is valid and corresponds to an existing voucher.</remarks>
        /// <param name="userVoucherId">The unique identifier of the user voucher to retrieve. This parameter cannot be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> containing the details of the user voucher if found. Returns a 404 status
        /// code if the voucher does not exist.</returns>
        [HttpGet("user/voucher/{userVoucherId}")] public async Task<IActionResult> UserVoucherAsync(string userVoucherId)
        {
            var result = await voucherService.UserVoucherAsync(userVoucherId, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Creates a user voucher based on the provided request model.
        /// </summary>
        /// <remarks>This method processes the request to create a user voucher and returns the result.
        /// The request model should include all necessary information for voucher creation.</remarks>
        /// <param name="model">The request model containing the details required to create the user voucher. This parameter must not be
        /// null.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. If successful, the response contains
        /// the created voucher details.</returns>
        [HttpPut("user/voucher")] public async Task<IActionResult> CreateUserVoucherAsync([FromBody] CreateUserVoucherRequest model)
        {
            var lodging = await voucherService.CreateUserVoucherAsync(model, HttpContext.RequestAborted);
            return Ok(lodging);
        }

        /// <summary>
        /// Removes a user voucher identified by the specified ID.
        /// </summary>
        /// <remarks>This method deletes a user voucher from the system based on the provided ID. Ensure
        /// that the ID corresponds to an existing voucher before calling this method.</remarks>
        /// <param name="userVoucherId">The unique identifier of the user voucher to be removed. This value cannot be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Typically, this will be an HTTP 200
        /// response with the result of the removal operation.</returns>
        [HttpDelete("user/voucher/{userVoucherId}")] public async Task<IActionResult> RemoveUserVoucherAsync(string userVoucherId)
        {
            var result = await voucherService.RemoveUserVoucherAsync(userVoucherId, HttpContext.RequestAborted);
            return Ok(result);
        }

        #endregion
    }
}
