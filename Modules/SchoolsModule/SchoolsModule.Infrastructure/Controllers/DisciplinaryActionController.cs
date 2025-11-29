using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.Discipline;

namespace SchoolsModule.Infrastructure.Controllers;

/// <summary>
/// Endpoints for managing disciplinary actions and severity scales.
/// </summary>
[Route("api/discipline/actions"), ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class DisciplinaryActionController(IDisciplinaryActionService service) : ControllerBase
{
    /// <summary>
    /// Retrieves all severity scales.
    /// </summary>
    /// <remarks>This method invokes the service to fetch all available severity scales
    /// asynchronously.</remarks>
    /// <returns>An <see cref="IActionResult"/> containing a collection of severity scales. The response is returned with an HTTP
    /// 200 status code if successful.</returns>
    [HttpGet("scales")] public async Task<IActionResult> Scales()
    {
        var result = await service.AllSeverityScalesAsync();
        return Ok(result);
    }

    /// <summary>
    /// Retrieves the severity scale associated with the specified scale identifier.
    /// </summary>
    /// <remarks>This method calls the underlying service to fetch the severity scale data and returns it as
    /// part of the HTTP response. Ensure that the <paramref name="scaleId"/> corresponds to a valid scale in the
    /// system.</remarks>
    /// <param name="scaleId">The unique identifier of the severity scale to retrieve. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> containing the severity scale data if found, or an appropriate HTTP response if
    /// not.</returns>
    [HttpGet("scales/{scaleId}")] public async Task<IActionResult> Scale(string scaleId)
    {
        var result = await service.SeverityScaleAsync(scaleId);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new severity scale based on the provided data transfer object (DTO).
    /// </summary>
    /// <remarks>The <paramref name="dto"/> parameter must contain valid data for the severity scale.  Ensure
    /// that all required fields in the DTO are populated before calling this method.</remarks>
    /// <param name="dto">The severity scale data transfer object containing the details of the scale to be created.</param>
    /// <returns>An <see cref="IActionResult"/> containing the result of the operation.  Returns an HTTP 200 OK response with the
    /// created severity scale if the operation is successful.</returns>
    [HttpPut("scales")] public async Task<IActionResult> CreateScale([FromBody] SeverityScaleDto dto)
    {
        var result = await service.CreateSeverityScaleAsync(dto);
        return Ok(result);
    }

    /// <summary>
    /// Updates the severity scale based on the provided data transfer object (DTO).
    /// </summary>
    /// <remarks>The <paramref name="dto"/> parameter must contain valid severity scale data.  Ensure that all
    /// required fields in the DTO are populated before calling this method.</remarks>
    /// <param name="dto">The severity scale data transfer object containing the updated scale information.</param>
    /// <returns>An <see cref="IActionResult"/> representing the result of the operation.  Typically, this will be an HTTP 200 OK
    /// response containing the updated severity scale.</returns>
    [HttpPost("scales")] public async Task<IActionResult> UpdateScale([FromBody] SeverityScaleDto dto)
    {
        var result = await service.UpdateSeverityScaleAsync(dto);
        return Ok(result);
    }

    /// <summary>
    /// Deletes the severity scale with the specified identifier.
    /// </summary>
    /// <remarks>This operation removes the severity scale identified by <paramref name="scaleId"/>.  Ensure
    /// that the specified scale ID exists before calling this method.</remarks>
    /// <param name="scaleId">The unique identifier of the severity scale to delete. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.  Typically returns an HTTP 200 OK
    /// response with the result of the deletion.</returns>
    [HttpDelete("scales/{scaleId}")] public async Task<IActionResult> DeleteScale(string scaleId)
    {
        var result = await service.DeleteSeverityScaleAsync(scaleId);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a list of all available actions.
    /// </summary>
    /// <remarks>This method performs an asynchronous operation to fetch the actions  and returns them in the
    /// HTTP response. The result is serialized to JSON.</remarks>
    /// <returns>An <see cref="IActionResult"/> containing the list of actions.  The response is an HTTP 200 OK with the actions
    /// in the response body.</returns>
    [HttpGet] public async Task<IActionResult> Actions()
    {
        var result = await service.AllActionsAsync();
        return Ok(result);
    }

    /// <summary>
    /// Retrieves the result of an action based on the specified action identifier.
    /// </summary>
    /// <remarks>This method calls the underlying service to perform the action asynchronously. Ensure that
    /// <paramref name="actionId"/> corresponds to a valid action in the system.</remarks>
    /// <param name="actionId">The unique identifier of the action to retrieve.</param>
    /// <returns>An <see cref="IActionResult"/> containing the result of the action.  Returns an HTTP 200 OK response with the
    /// result if successful.</returns>
    [HttpGet("{actionId}")] public async Task<IActionResult> Action(string actionId)
    {
        var result = await service.ActionAsync(actionId);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new disciplinary action based on the provided data.
    /// </summary>
    /// <remarks>This method processes the provided disciplinary action data and delegates the creation logic
    /// to the underlying service.</remarks>
    /// <param name="dto">The data transfer object containing the details of the disciplinary action to create.</param>
    /// <returns>An <see cref="IActionResult"/> containing the result of the operation. Returns an HTTP 200 OK response with the
    /// created disciplinary action if successful.</returns>
    [HttpPut] public async Task<IActionResult> Create([FromBody] DisciplinaryActionDto dto)
    {
        var result = await service.CreateActionAsync(dto);
        return Ok(result);
    }

    /// <summary>
    /// Updates an existing disciplinary action with the provided data.
    /// </summary>
    /// <remarks>The provided <paramref name="dto"/> must include all necessary fields for the update
    /// operation.  Ensure that the data complies with the expected format and validation rules.</remarks>
    /// <param name="dto">The data transfer object containing the updated details of the disciplinary action.</param>
    /// <returns>An <see cref="IActionResult"/> containing the result of the update operation.  Typically returns an HTTP 200 OK
    /// response with the updated disciplinary action details.</returns>
    [HttpPost] public async Task<IActionResult> Update([FromBody] DisciplinaryActionDto dto)
    {
        var result = await service.UpdateActionAsync(dto);
        return Ok(result);
    }

    /// <summary>
    /// Deletes the action with the specified identifier.
    /// </summary>
    /// <remarks>This method invokes the underlying service to delete the specified action asynchronously.
    /// Ensure the <paramref name="actionId"/> corresponds to a valid action.</remarks>
    /// <param name="actionId">The unique identifier of the action to delete. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Typically returns an HTTP 200 OK response
    /// with the result of the deletion.</returns>
    [HttpDelete("{actionId}")] public async Task<IActionResult> Delete(string actionId)
    {
        var result = await service.DeleteActionAsync(actionId);
        return Ok(result);
    }
}
