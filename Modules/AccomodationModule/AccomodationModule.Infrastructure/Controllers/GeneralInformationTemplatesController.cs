using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccomodationModule.Infrastructure.Controllers;

/// <summary>
/// API controller for managing general information templates.
/// </summary>
[Route("api/general-information-templates"), ApiController]
public class GeneralInformationTemplatesController(IGeneralInformationTemplateService service) : Controller
{
    #region Get Operations

    /// <summary>
    /// Retrieves all templates.
    /// </summary>
    [HttpGet("all")]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await service.GetAllAsync(HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a template by id.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        var result = await service.GetByIdAsync(id, HttpContext.RequestAborted);
        return Ok(result);
    }

    #endregion

    #region Crud Operations

    /// <summary>
    /// Creates a new template.
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> CreateAsync(GeneralInformationTemplateDto dto)
    {
        var result = await service.AddAsync(dto, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Updates an existing template.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> UpdateAsync(GeneralInformationTemplateDto dto)
    {
        var result = await service.EditAsync(dto, HttpContext.RequestAborted);
        return Ok(result);

        GeneralInformationTemplate UpdateAction(GeneralInformationTemplate entity, GeneralInformationTemplateDto data)
        {
            entity.Name = data.Name;
            entity.Information = data.Information;
            return entity;
        }
    }

    /// <summary>
    /// Removes a template.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveAsync(string id)
    {
        var result = await service.DeleteAsync(id, HttpContext.RequestAborted);
        return Ok(result);
    }

    #endregion
}