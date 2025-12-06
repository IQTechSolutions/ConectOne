using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccomodationModule.Infrastructure.Controllers;

/// <summary>
/// API controller for managing payment exclusion templates.
/// </summary>
[Route("api/payment-exclusion-templates"), ApiController]
public class PaymentExclusionTemplateController(IPaymentExclusionTemplateService service) : Controller
{
    #region Get Operations
    
    /// <summary>
    /// Retrieves all available items asynchronously.
    /// </summary>
    /// <remarks>This method sends an HTTP GET request to fetch all items. The operation respects the 
    /// cancellation token provided by the current HTTP context.</remarks>
    /// <returns>An <see cref="IActionResult"/> containing the collection of items if the operation is successful.</returns>
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
    /// Creates a new payment exclusion template.
    /// </summary>
    /// <param name="dto">The data transfer object containing the details of the payment exclusion template to create.</param>
    /// <returns>An <see cref="IActionResult"/> containing the result of the operation.  Returns an HTTP 200 OK response with the
    /// created template if the operation is successful.</returns>
    [HttpPut] public async Task<IActionResult> CreateAsync(PaymentExclusionTemplateDto dto)
    {
        var result = await service.AddAsync(dto, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Updates the payment exclusion template with the provided data.
    /// </summary>
    /// <remarks>This method processes the update asynchronously and respects the cancellation token provided
    /// by the HTTP context.</remarks>
    /// <param name="dto">The data transfer object containing the updated payment exclusion template details.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the update operation.  Typically returns an HTTP 200 OK
    /// response with the result of the operation.</returns>
    [HttpPost] public async Task<IActionResult> UpdateAsync(PaymentExclusionTemplateDto dto)
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
