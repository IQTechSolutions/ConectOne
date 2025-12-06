using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccomodationModule.Infrastructure.Controllers;

/// <summary>
/// API controller for managing cancellation terms templates.
/// </summary>
[Route("api/cancellation-terms-templates"), ApiController]
public class CancellationTermsTemplateController(ICancellationTermsTemplateService service) : Controller
{
    #region Get Operations
    
    /// <summary>
    /// Retrieves all available items asynchronously.
    /// </summary>
    /// <remarks>This method sends an HTTP GET request to fetch all items. The operation respects the
    /// cancellation token  provided by the current HTTP request context.</remarks>
    /// <returns>An <see cref="IActionResult"/> containing the collection of items in the response body.</returns>
    [HttpGet("all")] public async Task<IActionResult> GetAllAsync()
    {
        var result = await service.AllAsync(HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves an entity by its unique identifier.
    /// </summary>
    /// <remarks>This method performs an asynchronous operation to fetch the entity. If the entity is not
    /// found,  the response will indicate the appropriate HTTP status code.</remarks>
    /// <param name="id">The unique identifier of the entity to retrieve.</param>
    /// <returns>An <see cref="IActionResult"/> containing the entity if found, or an appropriate HTTP response if not.</returns>
    [HttpGet("{id}")] public async Task<IActionResult> GetByIdAsync(string id)
    {
        var result = await service.ByIdAsync(id, HttpContext.RequestAborted);
        return Ok(result);
    }
    #endregion

    #region Crud Operations
    
    /// <summary>
    /// Creates a new cancellation terms template asynchronously.
    /// </summary>
    /// <remarks>This method uses the provided <paramref name="dto"/> to create a new cancellation terms
    /// template  and returns the result of the operation. The operation respects the cancellation token  provided by
    /// the HTTP request.</remarks>
    /// <param name="dto">The data transfer object containing the details of the cancellation terms template to create.</param>
    /// <returns>An <see cref="IActionResult"/> containing the result of the operation.  Typically, this is an HTTP 200 OK
    /// response with the created template.</returns>
    [HttpPut] public async Task<IActionResult> CreateAsync(CancellationTermsTemplateDto dto)
    {
        var result = await service.AddAsync(dto, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Updates the cancellation terms template with the provided data.
    /// </summary>
    /// <param name="dto">The data transfer object containing the updated cancellation terms template details.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the update operation.  Typically, this will be an HTTP
    /// 200 OK response containing the result of the update.</returns>
    [HttpPost] public async Task<IActionResult> UpdateAsync(CancellationTermsTemplateDto dto)
    {
        var result = await service.EditAsync(dto, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Deletes the resource with the specified identifier.
    /// </summary>
    /// <remarks>This method performs an asynchronous operation to delete the resource.  If the resource does
    /// not exist, the behavior depends on the implementation of the underlying service.</remarks>
    /// <param name="id">The unique identifier of the resource to delete. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.  Typically returns an HTTP 200 OK
    /// response with the result of the deletion.</returns>
    [HttpDelete("{id}")] public async Task<IActionResult> RemoveAsync(string id)
    {
        var result = await service.DeleteAsync(id, HttpContext.RequestAborted);
        return Ok(result);
    }
    #endregion
}
