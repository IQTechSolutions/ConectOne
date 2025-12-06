using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccomodationModule.Infrastructure.Controllers;

/// <summary>
/// API controller for managing booking terms.
/// Exposes endpoints for performing CRUD operations on booking terms.
/// </summary>
[Route("api/booking-terms-description-templates"), ApiController]
public class BookingTermsDescriptionTemplateController(IBookingTermsDescriptionTemplateService service) : Controller
{
    #region Get Operations

    /// <summary>
    /// Retrieves all booking terms.
    /// </summary>
    /// <returns>An IActionResult containing all booking terms.</returns>
    [HttpGet("all")]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await service.GetAllAsync(HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a booking term by its ID.
    /// </summary>
    /// <param name="id">The ID of the booking term to retrieve.</param>
    /// <returns>An IActionResult containing the requested booking term.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        var result = await service.GetByIdAsync(id, HttpContext.RequestAborted);
        return Ok(result);
    }

    #endregion

    #region Crud Operations

    /// <summary>
    /// Creates a new booking term.
    /// </summary>
    /// <param name="guide">The DTO containing the data for the new booking term.</param>
    /// <returns>An IActionResult indicating the result of the create operation.</returns>
    [HttpPut]
    public async Task<IActionResult> CreateAsync(BookingTermsDescriptionTemplateDto guide)
    {
        var result = await service.AddAsync(guide, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Updates an existing booking term.
    /// </summary>
    /// <param name="supportTicket">The DTO containing the updated data for the booking term.</param>
    /// <returns>An IActionResult indicating the result of the update operation.</returns>
    [HttpPost]
    public async Task<IActionResult> UpdateAsync(BookingTermsDescriptionTemplateDto supportTicket)
    {
        var result = await service.EditAsync(supportTicket, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Deletes a booking term by its ID.
    /// </summary>
    /// <param name="id">The ID of the booking term to delete.</param>
    /// <returns>An IActionResult indicating the result of the delete operation.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveAsync(string id)
    {
        var result = await service.DeleteAsync(id, HttpContext.RequestAborted);
        return Ok(result);
    }

    #endregion
}