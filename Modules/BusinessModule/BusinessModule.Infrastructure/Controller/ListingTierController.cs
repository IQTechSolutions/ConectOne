using BusinessModule.Domain.DataTransferObjects;
using BusinessModule.Domain.Interfaces;
using FilingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Mvc;

namespace BusinessModule.Infrastructure.Controller;

/// <summary>
/// Provides endpoints for managing listing tiers, including retrieving, creating, updating, and deleting listing tiers,
/// as well as managing associated images.
/// </summary>
/// <remarks>This controller handles operations related to listing tiers, such as retrieving all listing tiers,
/// fetching a specific  listing tier by ID, creating new listing tiers, updating existing ones, and removing them.
/// Additionally, it provides  functionality for adding and removing images associated with listing tiers.</remarks>
/// <param name="command"></param>
/// <param name="query"></param>
[Route("api/listing_tiers"), ApiController]
public class ListingTierController(IListingTierCommandService command, IListingTierQueryService query) : ControllerBase
{
    /// <summary>
    /// Retrieves all listing tiers of the specified type.
    /// </summary>
    /// <remarks>This method returns a collection of listing tiers based on the provided type.  The result is
    /// returned as an HTTP 200 OK response containing the data.</remarks>
    /// <returns>An <see cref="IActionResult"/> containing the listing tiers as the response payload.</returns>
    [HttpGet("all")] public async Task<IActionResult> AllListingTiers()
    {
        var result = await query.AllListingTiersAsync();
        return Ok(result);
    }

    /// <summary>
    /// Retrieves the tier information for a specific listing based on the provided identifier.
    /// </summary>
    /// <remarks>This method is an HTTP GET endpoint that retrieves tier information for a listing. Ensure
    /// that the provided <paramref name="id"/> corresponds to a valid listing.</remarks>
    /// <param name="id">The unique identifier of the listing whose tier information is to be retrieved. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> containing the tier information for the specified listing. Returns a 200 OK
    /// response with the tier information if the operation is successful.</returns>
    [HttpGet("{id}")] public async Task<IActionResult> ListingTierAsync(string id)
    {
        var result = await query.ListingTierAsync(id);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new listing tier based on the provided data transfer object (DTO).
    /// </summary>
    /// <param name="dto">The data transfer object containing the details of the listing tier to create. Cannot be null.</param>
    /// <returns>An <see cref="IActionResult"/> containing the result of the operation. Typically returns an HTTP 200 OK response
    /// with the created listing tier.</returns>
    [HttpPut] public async Task<IActionResult> CreateListingTier([FromBody] ListingTierDto dto)
    {
        var result = await command.CreateAsync(dto);
        return Ok(result);
    }

    /// <summary>
    /// Updates the listing tier with the specified details.
    /// </summary>
    /// <remarks>The <paramref name="dto"/> parameter must contain valid data for the update operation to
    /// succeed. Ensure that all required fields in the DTO are populated before calling this method.</remarks>
    /// <param name="dto">The data transfer object containing the updated listing tier details.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.  Typically returns an HTTP 200 OK
    /// response with the result of the update.</returns>
    [HttpPost] public async Task<IActionResult> UpdateListingTier([FromBody] ListingTierDto dto)
    {
        var result = await command.UpdateAsync(dto);
        return Ok(result);
    }

    /// <summary>
    /// Removes a listing tier with the specified identifier.
    /// </summary>
    /// <remarks>This method is an HTTP DELETE endpoint. Ensure the specified <paramref name="id"/>
    /// corresponds to an existing listing tier.</remarks>
    /// <param name="id">The unique identifier of the listing tier to remove. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Typically returns an HTTP 200 OK response
    /// with the result of the removal.</returns>
    [HttpDelete("{id}")] public async Task<IActionResult> RemoveListingTier(string id)
    {
        var result = await command.RemoveAsync(id);
        return Ok(result);
    }

    #region Images
    
    /// <summary>
    /// Adds an image to the specified entity.
    /// </summary>
    /// <remarks>This method processes the provided image data and associates it with the specified entity.
    /// Ensure that the request object contains valid data.</remarks>
    /// <param name="dto">The request object containing the details of the image to add, including the entity identifier and image data.</param>
    /// <returns>An <see cref="IActionResult"/> containing the result of the operation. Typically, this is an HTTP 200 response
    /// with the operation result.</returns>
    [HttpPost("addImage")] public async Task<IActionResult> AddImage([FromBody] AddEntityImageRequest dto)
    {
        var result = await command.AddImage(dto, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Deletes an image with the specified identifier.
    /// </summary>
    /// <remarks>This method performs an asynchronous operation to delete the image. If the operation is
    /// canceled via the HTTP request's cancellation token, the deletion will be aborted.</remarks>
    /// <param name="imageId">The unique identifier of the image to delete. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Typically returns an HTTP 200 OK response
    /// with the result of the deletion.</returns>
    [HttpDelete("deleteImage/{imageId}/")] public async Task<IActionResult> RemoveImage(string imageId)
    {
        var result = await command.RemoveImage(imageId, HttpContext.RequestAborted);
        return Ok(result);
    }

    #endregion
}
