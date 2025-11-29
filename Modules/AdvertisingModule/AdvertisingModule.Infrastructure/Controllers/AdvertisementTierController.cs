using AdvertisingModule.Domain.DataTransferObjects;
using AdvertisingModule.Domain.Enums;
using AdvertisingModule.Domain.Interfaces;
using FilingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Mvc;

namespace AdvertisingModule.Infrastructure.Controllers;

/// <summary>
/// Provides endpoints for managing advertisement tiers, including retrieving, creating, updating, and deleting
/// advertisement tiers, as well as managing associated images.
/// </summary>
/// <remarks>This controller handles HTTP requests related to advertisement tiers and their associated images. It
/// supports operations such as: - Retrieving all advertisement tiers or a specific advertisement tier by its
/// identifier. - Creating, updating, and deleting advertisement tiers. - Adding and removing images associated with
/// advertisement tiers.  The controller uses dependency-injected services to perform the underlying operations: - <see
/// cref="IAdvertisementTierCommandService"/> for command operations (create, update, delete). - <see
/// cref="IAdvertisementTierQueryService"/> for query operations (retrieve data).  All operations are asynchronous and
/// return appropriate HTTP responses.</remarks>
/// <param name="command"></param>
/// <param name="query"></param>
[Route("api/advertisement_tiers"), ApiController]
public class AdvertisementTierController(IAdvertisementTierCommandService command, IAdvertisementTierQueryService query) : ControllerBase
{
    /// <summary>
    /// Retrieves all affiliate advertisement tiers.
    /// </summary>
    /// <remarks>This method returns a collection of advertisement tiers associated with affiliates.  The data
    /// is retrieved asynchronously and returned as an HTTP 200 OK response.</remarks>
    /// <returns>An <see cref="IActionResult"/> containing the collection of advertisement tiers. The response is serialized as
    /// JSON.</returns>
    [HttpGet("all/{type}")] public async Task<IActionResult> AllAdvertisementTiers(AdvertisementType type)
    {
        var result = await query.AllAdvertisementTiersAsync(type);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves the advertisement tier information for the specified affiliate ID.
    /// </summary>
    /// <remarks>This method is an HTTP GET endpoint that retrieves data asynchronously. Ensure the provided
    /// <paramref name="id"/> corresponds to a valid affiliate.</remarks>
    /// <param name="id">The unique identifier of the affiliate whose advertisement tier information is to be retrieved. Cannot be null
    /// or empty.</param>
    /// <returns>An <see cref="IActionResult"/> containing the advertisement tier information if the affiliate ID is valid.
    /// Returns a 200 OK response with the result on success.</returns>
    [HttpGet("{id}")] public async Task<IActionResult> AdvertisementTierAsync(string id)
    {
        var result = await query.AdvertisementTierAsync(id);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new affiliate based on the provided advertisement tier data.
    /// </summary>
    /// <param name="dto">The advertisement tier data used to create the affiliate. This parameter must not be null.</param>
    /// <returns>An <see cref="IActionResult"/> containing the result of the operation. Typically, this will be an HTTP 200 OK
    /// response with the created affiliate data.</returns>
    [HttpPut] public async Task<IActionResult> CreateAdvertisementTier([FromBody] AdvertisementTierDto dto)
    {
        var result = await command.CreateAsync(dto);
        return Ok(result);
    }

    /// <summary>
    /// Updates the affiliate information based on the provided advertisement tier data.
    /// </summary>
    /// <remarks>This method is intended to be used as an HTTP POST endpoint. Ensure that the provided
    /// <paramref name="dto"/> contains valid data required for the update operation.</remarks>
    /// <param name="dto">The advertisement tier data to update, provided as a <see cref="AdvertisementTierDto"/> object. This parameter
    /// must not be <c>null</c>.</param>
    /// <returns>An <see cref="IActionResult"/> containing the result of the update operation. Typically, this will be an HTTP
    /// 200 OK response with the result of the update.</returns>
    [HttpPost] public async Task<IActionResult> UpdateAdvertisementTier([FromBody] AdvertisementTierDto dto)
    {
        var result = await command.UpdateAsync(dto);
        return Ok(result);
    }

    /// <summary>
    /// Removes an affiliate with the specified identifier.
    /// </summary>
    /// <remarks>This method is an HTTP DELETE endpoint and is intended to be used for removing affiliates 
    /// from the system. Ensure the <paramref name="id"/> corresponds to a valid affiliate.</remarks>
    /// <param name="id">The unique identifier of the affiliate to remove. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.  Typically returns an HTTP 200 OK
    /// response with the result of the removal operation.</returns>
    [HttpDelete("{id}")] public async Task<IActionResult> RemoveAdvertisementTier(string id)
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
    [HttpPost("addImage")] public async Task<IActionResult> AddImage([FromBody] AddEntityImageRequest dto)
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
    [HttpDelete("deleteImage/{imageId}/")] public async Task<IActionResult> RemoveImage(string imageId)
    {
        var result = await command.RemoveImage(imageId, HttpContext.RequestAborted);
        return Ok(result);
    }

    #endregion
}
