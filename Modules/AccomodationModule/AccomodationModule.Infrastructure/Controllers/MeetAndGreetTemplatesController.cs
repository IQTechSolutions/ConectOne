using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccomodationModule.Infrastructure.Controllers;

/// <summary>
/// API controller for managing meet and greet templates.
/// </summary>
[Route("api/meet-and-greet-templates"), ApiController]
public class MeetAndGreetTemplatesController(IMeetAndGreetTemplateService service) : Controller
{
    #region Get Operations

    /// <summary>
    /// Retrieves all templates.
    /// </summary>
    [HttpGet("all")] public async Task<IActionResult> GetAllAsync()
    {
        var result = await service.AllAsync(HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a template by id.
    /// </summary>
    [HttpGet("{id}")] public async Task<IActionResult> GetByIdAsync(string id)
    {
        var result = await service.ByIdAsync(id, HttpContext.RequestAborted);
        return Ok(result);
    }

    #endregion

    #region Crud Operations

    /// <summary>
    /// Creates a new template.
    /// </summary>
    [HttpPut] public async Task<IActionResult> CreateAsync(MeetAndGreetTemplateDto dto)
    {
        var result = await service.AddAsync(dto, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Updates an existing template.
    /// </summary>
    [HttpPost] public async Task<IActionResult> UpdateAsync(MeetAndGreetTemplateDto dto)
    {
        var result = await service.EditAsync(dto, HttpContext.RequestAborted);
        return Ok(result);

    }

    /// <summary>
    /// Removes a template.
    /// </summary>
    [HttpDelete("{id}")] public async Task<IActionResult> RemoveAsync(string id)
    {
        var result = await service.DeleteAsync(id, HttpContext.RequestAborted);
        return Ok(result);
    }

    #endregion
}
