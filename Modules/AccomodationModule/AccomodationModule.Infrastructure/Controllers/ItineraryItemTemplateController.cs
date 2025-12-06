using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccomodationModule.Infrastructure.Controllers;

/// <summary>
/// API controller for managing terms and conditions templates.
/// </summary>
[Route("api/itinerary-item-templates"), ApiController]
public class ItineraryItemTemplateController(IItineraryItemTemplateService service) : Controller
{
    #region Get Operations
    
    /// <summary>
    /// Retrieves all available items asynchronously.
    /// </summary>
    /// <remarks>This method sends an HTTP GET request to the "all" endpoint and returns the result as an HTTP
    /// 200 OK response. The operation respects the cancellation token provided by the current HTTP context.</remarks>
    /// <returns>An <see cref="IActionResult"/> containing the collection of items retrieved. The response is serialized as JSON.</returns>
    [HttpGet("all")] public async Task<IActionResult> GetAllAsync()
    {
        var result = await service.GetAllAsync(HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves an entity by its unique identifier.
    /// </summary>
    /// <remarks>This method performs an asynchronous operation to fetch the entity associated with the
    /// specified identifier. If the entity is not found, the response will indicate the appropriate HTTP status
    /// code.</remarks>
    /// <param name="id">The unique identifier of the entity to retrieve. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> containing the entity if found, or a suitable HTTP response if not.</returns>
    [HttpGet("{id}")] public async Task<IActionResult> GetByIdAsync(string id)
    {
        var result = await service.GetByIdAsync(id, HttpContext.RequestAborted);
        return Ok(result);
    }

    #endregion

    #region Crud Operations

    /// <summary>
    /// Creates a new itinerary entry item template.
    /// </summary>
    /// <param name="dto">The data transfer object containing the details of the itinerary entry item template to create.</param>
    /// <returns>An <see cref="IActionResult"/> containing the result of the operation.  Returns an HTTP 200 OK response with the
    /// created item if the operation is successful.</returns>
    [HttpPut] public async Task<IActionResult> CreateAsync(ItineraryEntryItemTemplateDto dto)
    {
        var result = await service.AddAsync(dto, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Updates an itinerary entry item template with the provided data.
    /// </summary>
    /// <remarks>This method processes the update asynchronously and respects the cancellation token provided
    /// by the HTTP context.</remarks>
    /// <param name="dto">The data transfer object containing the updated details of the itinerary entry item template.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the update operation.  Typically, this is an HTTP 200 OK
    /// response containing the result of the update.</returns>
    [HttpPost] public async Task<IActionResult> UpdateAsync(ItineraryEntryItemTemplateDto dto)
    {
        var result = await service.EditAsync(dto, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Deletes the resource with the specified identifier.
    /// </summary>
    /// <remarks>This method sends an HTTP DELETE request to remove the resource identified by <paramref
    /// name="id"/>. If the resource does not exist, the behavior depends on the implementation of the underlying
    /// service.</remarks>
    /// <param name="id">The unique identifier of the resource to delete.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.  Typically, this is an HTTP 200 OK
    /// response containing the result of the deletion.</returns>
    [HttpDelete("{id}")] public async Task<IActionResult> RemoveAsync(string id)
    {
        var result = await service.DeleteAsync(id, HttpContext.RequestAborted);
        return Ok(result);
    }
    #endregion
}
