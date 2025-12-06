using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccomodationModule.Infrastructure.Controllers;

/// <summary>
/// API controller for managing gifts associated with vacations.
/// </summary>
[Route("api/gifts"), ApiController]
public class GiftsController(IGiftService giftService) : ControllerBase
{
    /// <summary>
    /// Retrieves all gifts for a given vacation.
    /// </summary>
    /// <param name="vacationId">The id of the vacation.</param>
    /// <returns>A list of gifts linked to the vacation.</returns>
    [HttpGet] public async Task<IActionResult> GiftsAsync()
    {
        var result = await giftService.GiftListAsync(HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a specific gift by its id.
    /// </summary>
    /// <param name="giftId">The id of the gift.</param>
    /// <returns>The requested gift information.</returns>
    [HttpGet("{giftId}")] public async Task<IActionResult> GiftAsync(string giftId)
    {
        var result = await giftService.GiftAsync(giftId, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new gift.
    /// </summary>
    /// <param name="dto">Gift data transfer object.</param>
    /// <returns>The result of the create operation.</returns>
    [HttpPut] public async Task<IActionResult> CreateGiftAsync([FromBody] GiftDto dto)
    {
        var newGift = await giftService.CreateGiftAsync(dto, HttpContext.RequestAborted);
        return Ok(newGift);
    }

    /// <summary>
    /// Updates an existing gift.
    /// </summary>
    /// <param name="dto">Gift data transfer object.</param>
    /// <returns>The result of the update operation.</returns>
    [HttpPost] public async Task<IActionResult> UpdateGiftAsync([FromBody] GiftDto dto)
    {
        var result = await giftService.UpdateGiftAsync(dto, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Removes a gift by its id.
    /// </summary>
    /// <param name="giftId">The id of the gift to remove.</param>
    /// <returns>The result of the delete operation.</returns>
    [HttpDelete("{giftId}")] public async Task<IActionResult> RemoveGiftAsync(string giftId)
    {
        var result = await giftService.RemoveGiftAsync(giftId, HttpContext.RequestAborted);
        return Ok(result);
    }
}
