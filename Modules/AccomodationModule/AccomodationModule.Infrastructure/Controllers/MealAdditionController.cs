using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccomodationModule.Infrastructure.Controllers
{
    /// <summary>
    /// API controller for managing vacation entities.
    /// </summary>
    [Route("api/vacations/mealAdditions"), ApiController]
    public class MealAdditionController(IMealAdditionService mealAdditionService) : ControllerBase
    {
        /// <summary>
        /// Retrieves all meal addition for a specific vacation.
        /// </summary>
        /// <param name="vacationId">The ID of the vacation to retrieve meal additions for.</param>
        /// <returns>An IActionResult containing the meal additions.</returns>
        [HttpGet("{vacationId}")] public async Task<IActionResult> MealAdditionsAsync(string vacationId)
        {
            var result = await mealAdditionService.MealAdditionsAsync(vacationId, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Get a meal addition
        /// </summary>
        /// <param name="mealAdditionId">The identity of the meal addition to retrieve</param>
        /// <returns>A result indicating the success of the operation.</returns>
        [HttpGet("{mealAdditionId}")] public async Task<IActionResult> MealAdditionAsync(string mealAdditionId)
        {
            var result = await mealAdditionService.MealAdditionAsync(mealAdditionId, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new meal addition.
        /// </summary>
        /// <param name="dto">The DTO containing the meal addition data.</param>
        /// <returns>An IActionResult indicating the result of the create operation.</returns>
        [HttpPut] public async Task<IActionResult> CreateMealAdditionAsync([FromBody] MealAdditionDto dto)
        {
            var newPackage = await mealAdditionService.CreateMealAdditionAsync(dto, HttpContext.RequestAborted);
            return Ok(newPackage);
        }

        /// <summary>
        /// Update a new meal addition.
        /// </summary>
        /// <param name="dto">The DTO containing the meal addition data.</param>
        /// <returns>An IActionResult indicating the result of the update operation.</returns>
        [HttpPost] public async Task<IActionResult> UpdateMealAdditionAsync([FromBody] MealAdditionDto dto)
        {
            var newPackage = await mealAdditionService.UpdateMealAdditionAsync(dto, HttpContext.RequestAborted);
            return Ok(newPackage);
        }

        /// <summary>
        /// Deletes a vacation price by its unique identifier.
        /// </summary>
        /// <param name="mealAdditionId">The ID of the vacation price to delete.</param>
        /// <returns>An IActionResult indicating the result of the delete operation.</returns>
        [HttpDelete("{mealAdditionId}")] public async Task<IActionResult> RemoveMealAdditionAsync(string mealAdditionId)
        {
            var result = await mealAdditionService.RemoveMealAdditionAsync(mealAdditionId, HttpContext.RequestAborted);
            return Ok(result);
        }
    }
}
