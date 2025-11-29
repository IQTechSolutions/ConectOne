using AdvertisingModule.Domain.DataTransferObjects;
using AdvertisingModule.Domain.Interfaces;
using AdvertisingModule.Domain.RequestFeatures;
using FilingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Mvc;

namespace AdvertisingModule.Infrastructure.Controllers;

/// <summary>
/// Provides API endpoints for managing advertisements, including retrieving, creating, updating, deleting, and
/// approving advertisements.
/// </summary>
/// <remarks>This controller handles operations related to advertisements, such as retrieving active or all
/// advertisements, creating new advertisements, updating existing ones, removing advertisements, and approving
/// advertisements. Each action corresponds to a specific HTTP endpoint and supports standard HTTP methods (GET, POST,
/// PUT, DELETE).</remarks>
/// <param name="command"></param>
/// <param name="query"></param>
[Route("api/advertisements"), ApiController]
public class AdvertisementsController(IAdvertisementCommandService command, IAdvertisementQueryService query) : ControllerBase
{
    /// <summary>
    /// Retrieves a list of active advertisements.
    /// </summary>
    /// <remarks>This method returns all advertisements that are currently marked as active.  The result is
    /// returned as an HTTP 200 OK response containing the list of active advertisements.</remarks>
    /// <returns>An <see cref="IActionResult"/> containing an HTTP 200 OK response with the list of active advertisements.</returns>
    [HttpGet("active")] public async Task<IActionResult> ActiveAdvertisements()
    {
        var result = await query.ActiveAdvertisementsAsync();
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a paginated list of advertisements based on the specified page parameters.
    /// </summary>
    /// <remarks>This method supports HTTP GET requests and returns the paginated results in the response
    /// body. The pagination settings are determined by the <paramref name="pageParameters"/> argument.</remarks>
    /// <param name="pageParameters">The parameters that define the pagination settings, such as page number and page size.</param>
    /// <returns>An <see cref="IActionResult"/> containing the paginated list of advertisements.</returns>
    [HttpGet("paged")] public async Task<IActionResult> PagedAdvertisements([FromQuery] AdvertisementListingPageParameters pageParameters)
    {
        var result = await query.PagedListingsAsync(pageParameters, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves all active advertisements.
    /// </summary>
    /// <remarks>This method performs an asynchronous operation to fetch all active advertisements and returns
    /// the result in the HTTP response body.</remarks>
    /// <returns>An <see cref="IActionResult"/> containing a collection of active advertisements. The response is returned with
    /// an HTTP 200 status code if the operation is successful.</returns>
    [HttpGet] public async Task<IActionResult> AllAdvertisements()
    {
        var result = await query.AllAdvertisementsAsync();
        return Ok(result);
    }

    /// <summary>
    /// Retrieves advertisement details asynchronously based on the specified identifier.
    /// </summary>
    /// <remarks>This method performs an asynchronous operation to fetch advertisement data. Ensure the
    /// <paramref name="id"/> is valid and corresponds to an existing advertisement.</remarks>
    /// <param name="id">The unique identifier of the advertisement to retrieve. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> containing the advertisement details if found; otherwise, a not found result.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> AdvertisementAsync(string id)
    {
        var result = await query.AdvertisementAsync(id);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new advertisement based on the provided data.
    /// </summary>
    /// <param name="dto">The data transfer object containing the details of the advertisement to create. Cannot be null.</param>
    /// <returns>An <see cref="IActionResult"/> containing the result of the creation operation. Returns an HTTP 200 response
    /// with the created advertisement details if successful.</returns>
    [HttpPut] public async Task<IActionResult> CreateAdvertisement([FromBody] AdvertisementDto dto)
    {
        var result = await command.CreateAsync(dto);
        return Ok(result);
    }

    /// <summary>
    /// Updates an advertisement with the provided data.
    /// </summary>
    /// <remarks>The <paramref name="dto"/> parameter must contain valid advertisement data.  Ensure all
    /// required fields are populated before calling this method.</remarks>
    /// <param name="dto">The advertisement data to update, provided as an <see cref="AdvertisementDto"/> object.</param>
    /// <returns>An <see cref="IActionResult"/> containing the result of the update operation.  Typically, this will be an HTTP
    /// 200 OK response with the update result.</returns>
    [HttpPost] public async Task<IActionResult> UpdateAdvertisement([FromBody] AdvertisementDto dto)
    {
        var result = await command.UpdateAsync(dto);
        return Ok(result);
    }

    /// <summary>
    /// Deletes an advertisement with the specified identifier.
    /// </summary>
    /// <remarks>This action is invoked using an HTTP DELETE request. Ensure the <paramref name="id"/>
    /// corresponds  to a valid advertisement in the system.</remarks>
    /// <param name="id">The unique identifier of the advertisement to be removed. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.  Typically returns an HTTP 200 OK
    /// response with the result of the deletion.</returns>
    [HttpDelete("{id}")] public async Task<IActionResult> RemoveAdvertisement(string id)
    {
        var result = await command.RemoveAsync(id);
        return Ok(result);
    }

    /// <summary>
    /// Approves an advertisement with the specified identifier.
    /// </summary>
    /// <remarks>This method processes an HTTP POST request to approve an advertisement.  Ensure the provided
    /// <paramref name="id"/> corresponds to a valid advertisement.</remarks>
    /// <param name="id">The unique identifier of the advertisement to approve. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> containing the result of the approval operation. Typically, this will be an HTTP
    /// 200 response with the operation result.</returns>
    [HttpPost("approve/{id}")] public async Task<IActionResult> ApproveAdvertisement(string id)
    {
        var result = await command.ApproveAsync(id);
        return Ok(result);
    }

    /// <summary>
    /// Rejects an advertisement with the specified identifier.
    /// </summary>
    /// <remarks>This method invokes the rejection process for the advertisement identified by <paramref
    /// name="id"/>.  Ensure the identifier corresponds to a valid advertisement before calling this method.</remarks>
    /// <param name="id">The unique identifier of the advertisement to reject. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Typically returns an HTTP 200 OK response
    /// with the result of the rejection.</returns>
    [HttpPost("reject/{id}")] public async Task<IActionResult> RejectAdvertisement(string id)
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
    [HttpPost("addImage")]
    public async Task<IActionResult> AddVacationImage([FromBody] AddEntityImageRequest dto)
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
    [HttpDelete("deleteImage/{imageId}/")]
    public async Task<IActionResult> RemoveVacationImage(string imageId)
    {
        var result = await command.RemoveImage(imageId, HttpContext.RequestAborted);
        return Ok(result);
    }

    #endregion
}