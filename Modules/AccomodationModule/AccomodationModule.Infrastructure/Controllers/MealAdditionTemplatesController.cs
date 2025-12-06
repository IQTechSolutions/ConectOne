using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Enums;
using AccomodationModule.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccomodationModule.Infrastructure.Controllers;

/// <summary>
/// API controller for managing meal addition templates.
/// </summary>
[Route("api/meal-addition-templates"), ApiController]
public class MealAdditionTemplatesController(IMealAdditionTemplateService service) : Controller
{
    #region Get Operations
    /// <summary>
    /// Retrieves all templates.
    /// </summary>
    [HttpGet("all")] public async Task<IActionResult> GetAllAsync()
    {
        var result = await service.GetAllAsync(HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a template by id.
    /// </summary>
    [HttpGet("{id}")] public async Task<IActionResult> GetByIdAsync(string id)
    {
        var result = await service.GetByIdAsync(id, HttpContext.RequestAborted);
        return Ok(result);
    }
    #endregion

    #region Crud Operations
    /// <summary>
    /// Creates a new template.
    /// </summary>
    [HttpPut] public async Task<IActionResult> CreateAsync(MealAdditionTemplateDto dto)
    {
        var result = await service.AddAsync(dto, HttpContext.RequestAborted);
        return Ok(result);

        static MealAdditionTemplate MapToEntity(MealAdditionTemplateDto d)
        {
            return new MealAdditionTemplate
            {
                GuestType = (GuestType)d.GuestType,
                MealType = (MealType)d.MealType,
                Notes = d.Notes,
                RestaurantId = d.Restaurant.Id
            };
        }
    }

    /// <summary>
    /// Updates an existing template.
    /// </summary>
    [HttpPost] public async Task<IActionResult> UpdateAsync(MealAdditionTemplateDto dto)
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
