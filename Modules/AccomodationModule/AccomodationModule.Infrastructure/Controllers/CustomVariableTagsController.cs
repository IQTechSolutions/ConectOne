using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccomodationModule.Infrastructure.Controllers;

/// <summary>
/// API controller for managing custom variable tags.
/// </summary>
[Route("api/custom-variable-tags"), ApiController]
public class CustomVariableTagsController(ICustomVariableTagService service) : ControllerBase
{
    #region Get Operations
    
    /// <summary>
    /// Retrieves all available items asynchronously.
    /// </summary>
    /// <remarks>This method sends an HTTP GET request to fetch all items from the underlying service. The
    /// operation respects the cancellation token provided by the current HTTP context.</remarks>
    /// <returns>An <see cref="IActionResult"/> containing the collection of items if the operation is successful.</returns>
    [HttpGet("all")] public async Task<IActionResult> GetAllAsync()
    {
        var result = await service.GetAllAsync(HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves an entity by its unique identifier.
    /// </summary>
    /// <remarks>This method performs an asynchronous operation to fetch the entity. If the entity is not
    /// found,  the response will indicate the appropriate HTTP status code.</remarks>
    /// <param name="id">The unique identifier of the entity to retrieve. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> containing the entity if found, or an appropriate HTTP response if not.</returns>
    [HttpGet("{id}")] public async Task<IActionResult> GetByIdAsync(string id)
    {
        var result = await service.GetByIdAsync(id, HttpContext.RequestAborted);
        return Ok(result);
    }
    #endregion

    #region Crud Operations
    
    /// <summary>
    /// Creates a new custom variable tag asynchronously.
    /// </summary>
    /// <remarks>The operation is cancellable via the <see cref="HttpContext.RequestAborted"/>
    /// token.</remarks>
    /// <param name="dto">The data transfer object containing the details of the custom variable tag to create.</param>
    /// <returns>An <see cref="IActionResult"/> containing the result of the operation.  Typically, this will be an HTTP 200
    /// response with the created custom variable tag.</returns>
    [HttpPut] public async Task<IActionResult> CreateAsync(CustomVariableTagDto dto)
    {
        var result = await service.AddAsync(dto, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Updates an existing custom variable tag asynchronously.
    /// </summary>
    /// <remarks>This method processes the update request by delegating the operation to the service layer. 
    /// The operation respects the cancellation token provided by the HTTP context.</remarks>
    /// <param name="dto">The data transfer object containing the updated details of the custom variable tag.</param>
    /// <returns>An <see cref="IActionResult"/> representing the result of the update operation.  Typically, this will be an HTTP
    /// 200 OK response containing the updated entity.</returns>
    [HttpPost] public async Task<IActionResult> UpdateAsync(CustomVariableTagDto dto)
    {
        var result = await service.EditAsync(dto, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Deletes the resource identified by the specified ID.
    /// </summary>
    /// <remarks>This method performs an asynchronous delete operation and respects the cancellation token 
    /// provided by the HTTP request. Ensure the <paramref name="id"/> corresponds to an existing resource.</remarks>
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
