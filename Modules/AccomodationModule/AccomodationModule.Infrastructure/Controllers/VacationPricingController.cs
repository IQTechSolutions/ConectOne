using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccomodationModule.Infrastructure.Controllers
{
    /// <summary>
    /// API controller for managing vacation entities.
    /// </summary>
    [Route("api/vacations/vacationPrices"), ApiController]
    public class VacationPricingController(IVacationPricingService vacationService) : ControllerBase
    {
        /// <summary>
        /// Retrieves all vacation prices for a specific vacation.
        /// </summary>
        /// <param name="vacationId">The ID of the vacation to retrieve prices for.</param>
        /// <returns>An IActionResult containing the vacation prices.</returns>
        [HttpGet("{vacationId}")] public async Task<IActionResult> VacationPricesAsync(string vacationId)
        {
            var result = await vacationService.VacationPricesAsync(vacationId, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Get a vacation price
        /// </summary>
        /// <param name="vacationPriceId">The identity of the vacation price to retrieve</param>
        /// <returns>A result indicating the success of the operation.</returns>
        [HttpGet("price/{vacationPriceId}")] public async Task<IActionResult> VacationPriceAsync(string vacationPriceId)
        {
            var result = await vacationService.VacationPriceAsync(vacationPriceId, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new vacation price.
        /// </summary>
        /// <param name="dto">The DTO containing the vacation price data.</param>
        /// <returns>An IActionResult indicating the result of the create operation.</returns>
        [HttpPut] public async Task<IActionResult> CreateVacationPriceAsync([FromBody] VacationPricingItemDto dto)
        {
            var newPackage = await vacationService.CreateVacationPriceAsync(dto, HttpContext.RequestAborted);
            return Ok(newPackage);
        }

        /// <summary>
        /// Update a new vacation price.
        /// </summary>
        /// <param name="dto">The DTO containing the vacation price data.</param>
        /// <returns>An IActionResult indicating the result of the update operation.</returns>
        [HttpPost] public async Task<IActionResult> UpdateVacationPriceAsync([FromBody] VacationPricingItemDto dto)
        {
            var newPackage = await vacationService.UpdateVacationPriceAsync(dto, HttpContext.RequestAborted);
            return Ok(newPackage);
        }

        /// <summary>
        /// Updates a Vacation Inclusion Display Info Section.
        /// </summary>
        /// <param name="dto">The DTO containing the Vacation Inclusion Display Info Section data.</param>
        /// <returns>An IActionResult indicating the result of the create operation.</returns>
        [HttpPost("updateDisplayOrder")]
        public async Task<IActionResult> UpdateVacationPriceDisplayOrderAsync([FromBody] VacationPricingItemGroupUpdateRequest dto)
        {
            var newPackage = await vacationService.UpdateVacationPriceDisplayOrderAsync(dto, HttpContext.RequestAborted);
            return Ok(newPackage);
        }
        
        /// <summary>
        /// Deletes a vacation price by its unique identifier.
        /// </summary>
        /// <param name="vacationPricingItemId">The ID of the vacation price to delete.</param>
        /// <returns>An IActionResult indicating the result of the delete operation.</returns>
        [HttpDelete("{vacationPricingItemId}")] public async Task<IActionResult> RemoveVacationPriceAsync(string vacationPricingItemId)
        {
            var result = await vacationService.RemoveVacationPriceAsync(vacationPricingItemId, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves all vacation prices for a specific vacation.
        /// </summary>
        /// <param name="vacationId">The ID of the vacation to retrieve prices for.</param>
        /// <returns>An IActionResult containing the vacation prices.</returns>
        [HttpGet("paymentRules/{vacationId}")]
        public async Task<IActionResult> PaymentRuleAsync(string vacationId)
        {
            var result = await vacationService.VacationPricesAsync(vacationId, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new Payment Schedule Entry.
        /// </summary>
        /// <param name="dto">The DTO containing the Payment Schedule Entry data.</param>
        /// <returns>An IActionResult indicating the result of the create operation.</returns>
        [HttpPut("paymentRule")]
        public async Task<IActionResult> CreatePaymentScheduleEntryAsync([FromBody] PaymentRuleDto dto)
        {
            var newPackage = await vacationService.CreatePaymentScheduleEntryAsync(dto, HttpContext.RequestAborted);
            return Ok(newPackage);
        }

        /// <summary>
        /// Update a new Payment Schedule Entry.
        /// </summary>
        /// <param name="dto">The DTO containing the Payment Schedule Entry data.</param>
        /// <returns>An IActionResult indicating the result of the update operation.</returns>
        [HttpPost("paymentRule")]
        public async Task<IActionResult> UpdatePaymentScheduleEntryAsync([FromBody] PaymentRuleDto dto)
        {
            var newPackage = await vacationService.UpdatePaymentScheduleEntryAsync(dto, HttpContext.RequestAborted);
            return Ok(newPackage);
        }

        /// <summary>
        /// Deletes a Payment Schedule Entry by its unique identifier.
        /// </summary>
        /// <param name="paymentRuleId">The ID of the Payment Schedule Entry to delete.</param>
        /// <returns>An IActionResult indicating the result of the delete operation.</returns>
        [HttpDelete("paymentRule/{paymentRuleId}")]
        public async Task<IActionResult> RemovePaymentScheduleEntryAsync(string paymentRuleId)
        {
            var result = await vacationService.RemovePaymentScheduleEntryAsync(paymentRuleId, HttpContext.RequestAborted);
            return Ok(result);
        }

        #region Pricing Groups

        /// <summary>
        /// Retrieves a list of vacation pricing groups.
        /// </summary>
        /// <remarks>This method fetches vacation pricing groups from the underlying service and returns
        /// them as a response. The result is typically used to categorize or group vacation pricing
        /// information.</remarks>
        /// <returns>An <see cref="IActionResult"/> containing the list of vacation pricing groups. The response is serialized as
        /// JSON.</returns>
        [HttpGet("groups/{vacationId}")] public async Task<IActionResult> VacationPricingGroupsAsync(string vacationId)
        {
            var result = await vacationService.VacationPricingGroupsAsync(vacationId, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new vacation pricing group based on the provided data.
        /// </summary>
        /// <remarks>This method processes the creation of a vacation pricing group and returns the result
        /// in an HTTP response. The operation respects the cancellation token provided by the HTTP context.</remarks>
        /// <param name="dto">The data transfer object containing the details of the vacation pricing group to create.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation, including the created pricing group
        /// details.</returns>
        [HttpPut("groups")] public async Task<IActionResult> CreateVacationPricingGroupAsync(VacationPriceGroupDto dto)
        {
            var result = await vacationService.CreateVacationPricingGroupAsync(dto, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Updates the vacation pricing group with the specified details.
        /// </summary>
        /// <remarks>This method processes the update request for a vacation pricing group and returns the
        /// result of the operation. The operation respects the cancellation token provided by the HTTP
        /// context.</remarks>
        /// <param name="dto">The data transfer object containing the details of the vacation pricing group to update.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPost("groups")] public async Task<IActionResult> UpdateVacationPricingGroupAsync(VacationPriceGroupDto dto)
        {
            var result = await vacationService.UpdateVacationPricingGroupAsync(dto, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Removes a vacation pricing group identified by the specified group ID.
        /// </summary>
        /// <remarks>This operation is performed asynchronously. If the group ID does not exist, the
        /// behavior depends on the implementation of the underlying service.</remarks>
        /// <param name="groupId">The unique identifier of the vacation pricing group to remove. Cannot be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Typically returns an HTTP 200 OK
        /// response with the result of the removal operation.</returns>
        [HttpDelete("groups/{groupId}")] public async Task<IActionResult> RemoveVacationPricingGroupAsync(string groupId)
        {
            var result = await vacationService.RemoveVacationPricingGroupAsync(groupId, HttpContext.RequestAborted);
            return Ok(result);
        }

        #endregion
    }
}
