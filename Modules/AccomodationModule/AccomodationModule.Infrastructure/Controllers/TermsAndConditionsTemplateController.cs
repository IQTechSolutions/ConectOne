using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccomodationModule.Infrastructure.Controllers;

/// <summary>
/// API controller for managing terms and conditions templates.
/// </summary>
[Route("api/terms-and-conditions-templates"), ApiController]
public class TermsAndConditionsTemplateController(ITermsAndConditionsTemplateService service) : Controller
{
    #region Get Operations
    
    /// <summary>
    /// Retrieves all available items asynchronously.
    /// </summary>
    /// <remarks>This method sends an HTTP GET request to fetch all items from the underlying service.  The
    /// operation respects the cancellation token provided by the current HTTP context.</remarks>
    /// <returns>An <see cref="IActionResult"/> containing the result of the operation.  Typically, this will be an HTTP 200 OK
    /// response with the retrieved items in the response body.</returns>
    [HttpGet("all")] public async Task<IActionResult> GetAllAsync()
    {
        var result = await service.GetAllAsync(HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves an entity by its unique identifier.
    /// </summary>
    /// <remarks>This method performs an asynchronous operation to fetch the entity. The result is returned as
    /// an HTTP 200 OK response  with the entity in the body if the operation is successful. If the entity is not found,
    /// an appropriate HTTP status code  is returned.</remarks>
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
    /// Creates a new Terms and Conditions template asynchronously.
    /// </summary>
    /// <remarks>This method uses the provided <paramref name="dto"/> to create a new template and returns the
    /// result. The operation respects the cancellation token provided by the current HTTP request.</remarks>
    /// <param name="dto">The data transfer object containing the details of the Terms and Conditions template to create.</param>
    /// <returns>An <see cref="IActionResult"/> containing the result of the operation.  Typically, this is an HTTP 200 OK
    /// response with the created template.</returns>
    [HttpPut] public async Task<IActionResult> CreateAsync(TermsAndConditionsTemplateDto dto)
    {
        var result = await service.AddAsync(dto, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Updates the terms and conditions template with the provided data.
    /// </summary>
    /// <remarks>This method processes the update asynchronously and returns an HTTP 200 OK response with the
    /// result of the operation.</remarks>
    /// <param name="dto">The data transfer object containing the updated terms and conditions template details.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IActionResult"/>
    /// indicating the outcome of the update operation.</returns>
    [HttpPost] public async Task<IActionResult> UpdateAsync(TermsAndConditionsTemplateDto dto)
    {
        var result = await service.EditAsync(dto, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Deletes the resource with the specified identifier.
    /// </summary>
    /// <remarks>This method performs an asynchronous operation to delete the resource. If the resource does
    /// not exist, the behavior depends on the implementation of the underlying service.</remarks>
    /// <param name="id">The unique identifier of the resource to delete. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Typically, an HTTP 200 OK response is
    /// returned with the result of the deletion.</returns>
    [HttpDelete("{id}")] public async Task<IActionResult> RemoveAsync(string id)
    {
        var result = await service.DeleteAsync(id, HttpContext.RequestAborted);
        return Ok(result);
    }
    #endregion
}
