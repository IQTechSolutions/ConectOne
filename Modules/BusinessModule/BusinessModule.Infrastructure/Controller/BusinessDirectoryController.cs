using BusinessModule.Domain.DataTransferObjects;
using BusinessModule.Domain.Interfaces;
using BusinessModule.Domain.RequestFeatures;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Mvc;

namespace BusinessModule.Infrastructure.Controller;

/// <summary>
/// Provides API endpoints for managing business directory listings, including creating, updating, retrieving,
/// approving, and removing listings.
/// </summary>
/// <remarks>This controller handles operations related to business directory listings. It supports retrieving
/// active listings, creating new listings, updating existing ones, approving listings, and removing listings. Each
/// endpoint is designed to interact with the underlying command and query services to perform the necessary
/// operations.</remarks>
/// <param name="command">The injected <see cref="IBusinessDirectoryCommandService"/></param>
/// <param name="query">The injected <see cref="IBusinessDirectoryQueryService"/></param>
[Route("api/businessdirectory"), ApiController]
public class BusinessDirectoryController(IBusinessDirectoryCommandService command, IBusinessDirectoryQueryService query) : ControllerBase
{
    /// <summary>
    /// Retrieves a paginated list of items based on the specified request parameters.
    /// </summary>
    /// <remarks>This method processes the pagination parameters provided in the query string  and returns the
    /// corresponding subset of items. The operation respects the  cancellation token provided by the HTTP
    /// context.</remarks>
    /// <param name="pageParameters">The pagination and filtering parameters for the request.</param>
    /// <returns>An <see cref="IActionResult"/> containing the paginated list of items.  The response is returned with an HTTP
    /// 200 status code if successful.</returns>
    [HttpGet("paged")] public async Task<IActionResult> PagedListingsAsync([FromQuery] BusinessListingPageParameters pageParameters)
    {
        var newPackage = await query.PagedListingsAsync(pageParameters, HttpContext.RequestAborted);
        return Ok(newPackage);
    }

    /// <summary>
    /// Retrieves a list of active listings.
    /// </summary>
    /// <remarks>This method returns all currently active listings by invoking the underlying query service.
    /// The result is returned as an HTTP 200 OK response containing the list of active listings.</remarks>
    /// <returns>An <see cref="IActionResult"/> containing an HTTP 200 OK response with the list of active listings.</returns>
    [HttpGet("active")] public async Task<IActionResult> ActiveListings()
    {
        var result = await query.ActiveListingsAsync();
        return Ok(result);
    }

    /// <summary>
    /// Retrieves the listing details for the specified identifier.
    /// </summary>
    /// <remarks>This method performs an asynchronous operation to fetch the listing details. The operation
    /// respects the cancellation token provided by the HTTP request.</remarks>
    /// <param name="id">The unique identifier of the listing to retrieve. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> containing the listing details if found, or an appropriate HTTP status code if
    /// not.</returns>
    [HttpGet("{id}")] public async Task<IActionResult> Listing(string id)
    {
        var result = await query.ListingAsync(id, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Sends an enquiry to the owner of the specified business listing.
    /// </summary>
    /// <param name="id">The identifier of the listing that should receive the enquiry.</param>
    /// <param name="request">The enquiry payload provided by the visitor.</param>
    /// <returns>A result indicating whether the enquiry could be delivered.</returns>
    [HttpPost("contact")] public async Task<IActionResult> ContactListingOwner([FromBody] ListingContactRequest request)
    {
        if (request is null)
            return Ok(await Result.FailAsync("Request payload cannot be null."));

        var result = await command.ContactListingOwnerAsync(request, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new business listing based on the provided data.
    /// </summary>
    /// <param name="dto">The data transfer object containing the details of the business listing to create.</param>
    /// <returns>An <see cref="IActionResult"/> containing the result of the operation.  Returns an HTTP 200 OK response with the
    /// created listing if successful.</returns>
    [HttpPut] public async Task<IActionResult> CreateListing([FromBody] BusinessListingDto dto)
    {
        var result = await command.CreateAsync(dto);
        return Ok(result);
    }

    /// <summary>
    /// Updates an existing business listing with the specified data.
    /// </summary>
    /// <remarks>The <paramref name="id"/> parameter is used to identify the listing to update, and the
    /// <paramref name="dto"/> parameter provides the new data for the listing. The <c>Id</c> property of the <paramref
    /// name="dto"/> is automatically set to match the provided <paramref name="id"/>.</remarks>
    /// <param name="id">The unique identifier of the business listing to update.</param>
    /// <param name="dto">The data transfer object containing the updated details of the business listing.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the update operation. Returns an HTTP 200 response with
    /// the updated listing if successful.</returns>
    [HttpPost("{id}")] public async Task<IActionResult> UpdateListing(string id, [FromBody] BusinessListingDto dto)
    {
        dto.Id = id;
        var result = await command.UpdateAsync(dto);
        return Ok(result);
    }

    /// <summary>
    /// Renews the active period for the specified listing.
    /// </summary>
    /// <param name="id">The identifier of the listing to renew.</param>
    /// <returns>An <see cref="IActionResult"/> describing the outcome of the renewal request.</returns>
    [HttpPost("{id}/renew")] public async Task<IActionResult> RenewListing(string id)
    {
        var result = await command.RenewAsync(id, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Deletes a listing with the specified identifier.
    /// </summary>
    /// <remarks>This method performs an asynchronous operation to remove the listing.  Ensure the provided
    /// <paramref name="id"/> corresponds to an existing listing.</remarks>
    /// <param name="id">The unique identifier of the listing to be removed. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.  Typically returns an HTTP 200 OK
    /// response with the result of the deletion.</returns>
    [HttpDelete("{id}")] public async Task<IActionResult> RemoveListing(string id)
    {
        var result = await command.RemoveAsync(id);
        return Ok(result);
    }

    /// <summary>
    /// Approves a listing with the specified identifier.
    /// </summary>
    /// <remarks>This method processes an HTTP POST request to approve a listing. The approval logic is
    /// handled asynchronously, and the result of the operation is returned in the response.</remarks>
    /// <param name="id">The unique identifier of the listing to approve. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> containing the result of the approval operation. Typically, this will be an HTTP
    /// 200 response with the operation result.</returns>
    [HttpPost("approve/{id}")] public async Task<IActionResult> ApproveListing(string id)
    {
        var result = await command.ApproveAsync(id);
        return Ok(result);
    }

    /// <summary>
    /// Rejects a listing with the specified identifier.
    /// </summary>
    /// <remarks>This method is invoked via an HTTP POST request to the endpoint "reject/{id}". Ensure the
    /// <paramref name="id"/> corresponds to a valid listing identifier.</remarks>
    /// <param name="id">The unique identifier of the listing to be rejected. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Typically returns an HTTP 200 OK response
    /// with the operation result.</returns>
    [HttpPost("reject/{id}")] public async Task<IActionResult> RejectListing(string id)
    {
        var result = await command.RejectAsync(id);
        return Ok(result);
    }

    #region Images

    /// <summary>
    /// Adds an image to a vacation entity.
    /// </summary>
    /// <param name="dto">The request object containing the image data and associated entity information.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Typically returns an HTTP 200 response
    /// with the result of the image addition.</returns>
    [HttpPost("addImage")] public async Task<IActionResult> AddListingImage([FromBody] AddEntityImageRequest dto)
    {
        var result = await command.AddImage(dto, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Deletes a vacation image with the specified identifier.
    /// </summary>
    /// <remarks>This method performs an asynchronous operation to delete the specified image.  If the
    /// operation is canceled via the HTTP request, the task will be aborted.</remarks>
    /// <param name="imageId">The unique identifier of the image to be deleted. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Typically returns an HTTP 200 response
    /// with the result of the deletion.</returns>
    [HttpDelete("deleteImage/{imageId}/")] public async Task<IActionResult> RemoveListingImage(string imageId)
    {
        var result = await command.RemoveImage(imageId, HttpContext.RequestAborted);
        return Ok(result);
    }

    #endregion

    #region Videos

    /// <summary>
    /// Adds a video entity based on the provided request.
    /// </summary>
    /// <remarks>This method delegates the operation to the underlying service layer. Ensure that the
    /// <paramref name="request"/> contains valid data before invoking this method.</remarks>
    /// <param name="request">The request containing the details of the video to be added.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the outcome of the operation.</returns>
    [HttpPost("addVideo")]
    public async Task<IBaseResult> AddVideo(AddEntityVideoRequest request, CancellationToken cancellationToken = default)
    {
        var addResult = await command.AddVideo(request, cancellationToken);
        return addResult;
    }

    /// <summary>
    /// Deletes a video with the specified identifier.
    /// </summary>
    /// <remarks>This method sends an HTTP DELETE request to remove the video identified by <paramref
    /// name="videoId"/>. Ensure the video ID is valid and exists in the system before calling this method.</remarks>
    /// <param name="videoId">The unique identifier of the video to delete. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the outcome of the delete operation.</returns>
    [HttpDelete("deleteVideo/{videoId}/")]
    public async Task<IBaseResult> RemoveVideo(string videoId, CancellationToken cancellationToken = default)
    {
        var addResult = await command.RemoveVideo(videoId, cancellationToken);
        return addResult;
    }

    #endregion

    #region Listing Services

    /// <summary>
    /// Adds a new listing service based on the provided data.
    /// </summary>
    /// <remarks>The operation is performed asynchronously. The request can be canceled using the HTTP
    /// context's cancellation token.</remarks>
    /// <param name="dto">The data transfer object containing the details of the listing service to add.</param>
    /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
    [HttpPost("listingServices")] public async Task<IActionResult> AddListingService([FromBody] ListingServiceDto dto)
    {
        var result = await command.AddListingService(dto, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Updates an existing listing service with the provided data.
    /// </summary>
    /// <remarks>This method processes an HTTP POST request to update a listing service. The update operation
    /// is performed asynchronously, and the result is returned in the HTTP response.</remarks>
    /// <param name="dto">The data transfer object containing the updated details of the listing service.</param>
    /// <returns>An <see cref="IActionResult"/> containing the result of the update operation.</returns>
    [HttpPut("listingServices/{listingServiceId}")] public async Task<IActionResult> UpdateListingService(string listingServiceId, [FromBody] ListingServiceDto dto)
    {
        var updateDto = string.IsNullOrWhiteSpace(dto.Id) ? dto with { Id = listingServiceId } : dto;
        var result = await command.UpdateListingService(updateDto, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Removes a listing service with the specified identifier.
    /// </summary>
    /// <remarks>This method performs an HTTP DELETE operation to remove the specified listing service.  The
    /// operation respects the cancellation token provided by the current HTTP request.</remarks>
    /// <param name="listingServiceId">The unique identifier of the listing service to be removed. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Typically returns an HTTP 200 response
    /// with the operation result.</returns>
    [HttpDelete("listingServices/{listingServiceId}")] public async Task<IActionResult> RemoveListingService(string listingServiceId)
    {
        var result = await command.RemoveListingService(listingServiceId, HttpContext.RequestAborted);
        return Ok(result);
    }

    #endregion

    #region Listing Service Images

    /// <summary>
    /// Adds an image to a vacation entity.
    /// </summary>
    /// <param name="dto">The request object containing the image data and associated entity information.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Typically returns an HTTP 200 response
    /// with the result of the image addition.</returns>
    [HttpPost("listingServices/addImage")] public async Task<IActionResult> AddListingServiceImage([FromBody] AddEntityImageRequest dto)
    {
        var result = await command.AddListingServiceImage(dto, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Deletes a vacation image with the specified identifier.
    /// </summary>
    /// <remarks>This method performs an asynchronous operation to delete the specified image.  If the
    /// operation is canceled via the HTTP request, the task will be aborted.</remarks>
    /// <param name="imageId">The unique identifier of the image to be deleted. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Typically returns an HTTP 200 response
    /// with the result of the deletion.</returns>
    [HttpDelete("listingServices/deleteImage/{imageId}/")] public async Task<IActionResult> RemoveListingServiceImage(string imageId)
    {
        var result = await command.RemoveListingServiceImage(imageId, HttpContext.RequestAborted);
        return Ok(result);
    }

    #endregion

    #region Listing Products

    /// <summary>
    /// Adds a new listing product to the system.
    /// </summary>
    /// <remarks>This method processes the provided <paramref name="dto"/> to add a new listing product.  The
    /// operation is asynchronous and respects the cancellation token provided by the HTTP context.</remarks>
    /// <param name="dto">The data transfer object containing the details of the listing product to add.</param>
    /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
    [HttpPost("listingProducts")] public async Task<IActionResult> AddListingProduct([FromBody] ListingProductDto dto)
    {
        var result = await command.AddListingProduct(dto, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Updates the details of a listing product based on the provided data transfer object (DTO).
    /// </summary>
    /// <remarks>This method processes the update request for a listing product and returns the result of the
    /// operation. The update operation is performed asynchronously.</remarks>
    /// <param name="dto">The data transfer object containing the updated details of the listing product.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the update operation.</returns>
    [HttpPut("listingProducts/{listingProductId}")] public async Task<IActionResult> UpdateListingProduct(string listingProductId, [FromBody] ListingProductDto dto)
    {
        var updateDto = string.IsNullOrWhiteSpace(dto.Id) ? dto with { Id = listingProductId } : dto;
        var result = await command.UpdateListingProduct(updateDto, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Removes a listing product identified by the specified ID.
    /// </summary>
    /// <remarks>This method performs an asynchronous operation to remove the specified listing product.  The
    /// operation respects the cancellation token provided by the HTTP context.</remarks>
    /// <param name="listingProductId">The unique identifier of the listing product to be removed. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Typically returns an HTTP 200 response
    /// with the operation result.</returns>
    [HttpDelete("listingProducts/{listingProductId}")] public async Task<IActionResult> RemoveListingProduct(string listingProductId)
    {
        var result = await command.RemoveListingProduct(listingProductId, HttpContext.RequestAborted);
        return Ok(result);
    }

    #endregion

    #region Listing Product Images

    /// <summary>
    /// Adds an image to a vacation entity.
    /// </summary>
    /// <param name="dto">The request object containing the image data and associated entity information.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Typically returns an HTTP 200 response
    /// with the result of the image addition.</returns>
    [HttpPost("listingProducts/addImage")] public async Task<IActionResult> AddListingProductImage([FromBody] AddEntityImageRequest dto)
    {
        var result = await command.AddListingProductImage(dto, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Deletes a vacation image with the specified identifier.
    /// </summary>
    /// <remarks>This method performs an asynchronous operation to delete the specified image.  If the
    /// operation is canceled via the HTTP request, the task will be aborted.</remarks>
    /// <param name="imageId">The unique identifier of the image to be deleted. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Typically returns an HTTP 200 response
    /// with the result of the deletion.</returns>
    [HttpDelete("listingProducts/deleteImage/{imageId}/")] public async Task<IActionResult> RemoveListingProductImage(string imageId)
    {
        var result = await command.RemoveListingProductImage(imageId, HttpContext.RequestAborted);
        return Ok(result);
    }

    #endregion
}