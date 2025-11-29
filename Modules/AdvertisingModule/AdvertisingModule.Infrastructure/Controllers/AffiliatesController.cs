using AdvertisingModule.Domain.DataTransferObjects;
using AdvertisingModule.Domain.Interfaces;
using FilingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Mvc;

namespace AdvertisingModule.Infrastructure.Controllers;

/// <summary>
/// Provides endpoints for managing affiliates, including retrieving, creating, updating, and deleting affiliates,  as
/// well as managing affiliate-related images.
/// </summary>
/// <remarks>This controller exposes a RESTful API for affiliate management. It includes operations to retrieve
/// all affiliates  or a specific affiliate by ID, create or update affiliate records, and remove affiliates.
/// Additionally, it provides  endpoints for adding and removing images associated with affiliates.</remarks>
/// <param name="command"></param>
/// <param name="query"></param>
[Route("api/affiliates"), ApiController]
public class AffiliatesController(IAffiliateCommandService command, IAffiliateQueryService query) : ControllerBase
{
    /// <summary>
    /// Retrieves a list of all affiliates.
    /// </summary>
    /// <remarks>This method performs an asynchronous operation to fetch the affiliates and returns the result
    /// in the HTTP response. Ensure that the underlying data source is properly configured and accessible.</remarks>
    /// <returns>An <see cref="IActionResult"/> containing the list of affiliates. The result is returned      as an HTTP 200 OK
    /// response with the data, or an appropriate error response if the operation fails.</returns>
    [HttpGet] public async Task<IActionResult> AllAffiliates()
    {
        var result = await query.AllAffiliatesAsync();
        return Ok(result);
    }

    /// <summary>
    /// Retrieves affiliate information based on the specified identifier.
    /// </summary>
    /// <remarks>This method is an HTTP GET endpoint that fetches affiliate data using the provided
    /// identifier. The result is returned as an HTTP 200 OK response with the affiliate data, or another HTTP status
    /// code if the operation fails.</remarks>
    /// <param name="id">The unique identifier of the affiliate to retrieve. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> containing the affiliate information if found, or an appropriate HTTP response if
    /// not.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> AffiliateAsync(string id)
    {
        var result = await query.AffiliateAsync(id);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new affiliate using the provided data.
    /// </summary>
    /// <remarks>This method processes the provided affiliate data and creates a new affiliate record.  The
    /// operation is asynchronous and returns the result of the creation process.</remarks>
    /// <param name="dto">The data transfer object containing the details of the affiliate to create.</param>
    /// <returns>An <see cref="IActionResult"/> containing the result of the operation. Typically, this is an HTTP 200 response
    /// with the created affiliate's details.</returns>
    [HttpPut] public async Task<IActionResult> CreateAffiliate([FromBody] AffiliateDto dto)
    {
        var result = await command.CreateAsync(dto);
        return Ok(result);
    }

    /// <summary>
    /// Updates the details of an affiliate using the provided data transfer object (DTO).
    /// </summary>
    /// <param name="dto">The data transfer object containing the updated affiliate details. Cannot be null.</param>
    /// <returns>An <see cref="IActionResult"/> containing the result of the update operation. Typically returns an HTTP 200 OK
    /// response with the update result.</returns>
    [HttpPost] public async Task<IActionResult> UpdateAffiliate([FromBody] AffiliateDto dto)
    {
        var result = await command.UpdateAsync(dto);
        return Ok(result);
    }

    /// <summary>
    /// Updates the display order of affiliates based on the provided request data.
    /// </summary>
    /// <remarks>This method processes the display order update asynchronously. The request data must be
    /// provided in the body of the HTTP POST request. The operation respects the cancellation token provided by the
    /// HTTP context.</remarks>
    /// <param name="dto">The request object containing the updated display order information for affiliates.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns an HTTP 200 OK response with the
    /// updated package if the operation is successful.</returns>
    [HttpPost("updateDisplayOrder")]
    public async Task<IActionResult> UpdateAffiliateDisplayOrderAsync([FromBody] AffiliateOrderUpdateRequest dto)
    {
        var newPackage = await command.UpdateAffiliateDisplayOrderAsync(dto, HttpContext.RequestAborted);
        return Ok(newPackage);
    }

    /// <summary>
    /// Removes an affiliate with the specified identifier.
    /// </summary>
    /// <remarks>This operation is performed asynchronously. Ensure the provided <paramref name="id"/>
    /// corresponds to a valid affiliate.</remarks>
    /// <param name="id">The unique identifier of the affiliate to remove. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Typically returns an HTTP 200 OK response
    /// with the result of the removal.</returns>
    [HttpDelete("{id}")] public async Task<IActionResult> RemoveAffiliate(string id)
    {
        var result = await command.RemoveAsync(id);
        return Ok(result);
    }

    #region Images
    
    /// <summary>
    /// Adds an image to the specified entity based on the provided request data.
    /// </summary>
    /// <remarks>This method processes the image addition asynchronously. The operation respects the
    /// cancellation token provided by the HTTP context.</remarks>
    /// <param name="dto">The request data containing the details of the image to be added, including the entity it is associated with.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Typically returns an HTTP 200 OK response
    /// with the result of the image addition.</returns>
    [HttpPost("addImage")]
    public async Task<IActionResult> AddImage([FromBody] AddEntityImageRequest dto)
    {
        var result = await command.AddImage(dto, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Deletes an image with the specified identifier.
    /// </summary>
    /// <remarks>This method performs an asynchronous operation to delete the image.  The operation respects
    /// the cancellation token provided by the HTTP request.</remarks>
    /// <param name="imageId">The unique identifier of the image to delete. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.  Typically returns an HTTP 200 OK
    /// response with the result of the deletion.</returns>
    [HttpDelete("deleteImage/{imageId}/")]
    public async Task<IActionResult> RemoveImage(string imageId)
    {
        var result = await command.RemoveImage(imageId, HttpContext.RequestAborted);
        return Ok(result);
    }

    #endregion
}

