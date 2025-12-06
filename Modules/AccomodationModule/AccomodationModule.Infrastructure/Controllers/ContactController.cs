using AccomodationModule.Domain.Arguments;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using FilingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Mvc;

namespace AccomodationModule.Infrastructure.Controllers;

/// <summary>
/// API controller for managing guide entities.
/// </summary>
[Route("api/contacts"), ApiController]
public class ContactController(IContactService service) : Controller
{
    #region Get Operations

    /// <summary>
    /// Retrieves all contacts.
    /// </summary>
    /// <returns>An IActionResult containing all contacts.</returns>
    [HttpGet("all")] public async Task<IActionResult> GetAllAsync()
    {
        var result = await service.GetAllAsync(HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a list of featured items asynchronously.
    /// </summary>
    /// <remarks>This method handles HTTP GET requests to the "featured" endpoint and returns a collection of
    /// featured items. The operation is cancellable through the HTTP request's cancellation token.</remarks>
    /// <returns>An <see cref="IActionResult"/> containing the list of featured items. The result is returned with an HTTP 200 OK
    /// status.</returns>
    [HttpGet("featured")] public async Task<IActionResult> GetFeaturedAsync()
    {
        var result = await service.GetFeaturedAsync(HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a paged list of contacts based on the provided request parameters.
    /// </summary>
    /// <param name="requestParameters">The parameters for pagination, sorting, and filtering.</param>
    /// <returns>An IActionResult containing the paged list of contacts.</returns>
    [HttpGet("paged")] public async Task<IActionResult> GetPagedAsync([FromQuery] ContactsPageParams requestParameters)
    {
        var result = await service.PagedAsync(requestParameters, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a contact by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the contact to retrieve.</param>
    /// <returns>An IActionResult containing the contact with the specified ID.</returns>
    [HttpGet("{id}")] public async Task<IActionResult> GetByIdAsync(string id)
    {
        var result = await service.GetByIdAsync(id, HttpContext.RequestAborted);
        return Ok(result);
    }

    #endregion

    #region Crud Operations

    /// <summary>
    /// Creates a new contact.
    /// </summary>
    /// <param name="contact">The DTO containing the contact data.</param>
    /// <returns>An IActionResult indicating the result of the create operation.</returns>
    [HttpPut] public async Task<IActionResult> CreateAsync(ContactDto contact)
    {
        var result = await service.AddAsync(contact, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Updates an existing contact.
    /// </summary>
    /// <param name="contact">The DTO containing the updated contact data.</param>
    /// <returns>An IActionResult indicating the result of the update operation.</returns>
    [HttpPost] public async Task<IActionResult> UpdateAsync(ContactDto contact)
    {
        var result = await service.EditAsync(contact, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Deletes a contact by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the contact to delete.</param>
    /// <returns>An IActionResult indicating the result of the delete operation.</returns>
    [HttpDelete("{id}")] public async Task<IActionResult> RemoveAsync(string id)
    {
        var result = await service.DeleteAsync(id, HttpContext.RequestAborted);
        return Ok(result);
    }

    #endregion

    #region Images

    /// <summary>
    /// Adds an image to a vacation entity.
    /// </summary>
    /// <remarks>This method asynchronously processes the image addition request and returns an HTTP
    /// 200 OK response with the result.</remarks>
    /// <param name="dto">The data transfer object containing the image and associated entity information.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
    [HttpPost("addImage")]
    public async Task<IActionResult> AddImage([FromBody] AddEntityImageRequest dto)
    {
        var result = await service.AddImage(dto, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Removes a vacation image identified by the specified image ID.
    /// </summary>
    /// <remarks>This method sends a request to delete the image associated with the given image ID.
    /// The operation is asynchronous and respects the cancellation token from the HTTP context.</remarks>
    /// <param name="imageId">The unique identifier of the image to be removed.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
    [HttpDelete("deleteImage/{imageId}/")]
    public async Task<IActionResult> RemoveImage(string imageId)
    {
        var result = await service.RemoveImage(imageId, HttpContext.RequestAborted);
        return Ok(result);
    }

    #endregion
}
