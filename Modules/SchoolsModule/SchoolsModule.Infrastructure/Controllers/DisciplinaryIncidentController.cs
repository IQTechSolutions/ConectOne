using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.Discipline;

namespace SchoolsModule.Infrastructure.Controllers;

/// <summary>
/// Endpoints for logging disciplinary incidents.
/// </summary>
[Route("api/discipline/incidents"), ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class DisciplinaryIncidentController(IDisciplinaryIncidentService service) : ControllerBase
{
    /// <summary>
    /// Retrieves a list of incidents associated with the specified learner.
    /// </summary>
    /// <remarks>This method calls the underlying service to fetch incidents related to the specified learner.
    /// Ensure that the <paramref name="learnerId"/> corresponds to a valid learner in the system.</remarks>
    /// <param name="learnerId">The unique identifier of the learner whose incidents are to be retrieved. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> containing a collection of incidents for the specified learner. The response is
    /// returned with an HTTP 200 status code if successful.</returns>
    [HttpGet("learner/{learnerId}")] public async Task<IActionResult> LearnerIncidents(string learnerId)
    {
        var result = await service.IncidentsByLearnerAsync(learnerId);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new disciplinary incident based on the provided data.
    /// </summary>
    /// <remarks>This method processes the provided disciplinary incident data and delegates the creation
    /// logic to the underlying service.</remarks>
    /// <param name="dto">The data transfer object containing the details of the disciplinary incident to create.</param>
    /// <returns>An <see cref="IActionResult"/> containing the result of the operation. Typically returns an HTTP 200 OK response
    /// with the created incident details.</returns>
    [HttpPut] public async Task<IActionResult> Create([FromBody] DisciplinaryIncidentDto dto)
    {
        var result = await service.CreateAsync(dto);
        return Ok(result);
    }

    /// <summary>
    /// Updates an existing disciplinary incident with the provided data.
    /// </summary>
    /// <remarks>The method expects a valid <paramref name="dto"/> object to be provided in the request body.
    /// The update operation is performed asynchronously, and the result is returned in the HTTP response.</remarks>
    /// <param name="dto">The data transfer object containing the updated details of the disciplinary incident.</param>
    /// <returns>An <see cref="IActionResult"/> containing the result of the update operation.</returns>
    [HttpPost] public async Task<IActionResult> Update([FromBody] DisciplinaryIncidentDto dto)
    {
        var result = await service.UpdateAsync(dto);
        return Ok(result);
    }

    /// <summary>
    /// Deletes the incident with the specified identifier.
    /// </summary>
    /// <remarks>This method invokes the underlying service to delete the incident. Ensure the <paramref
    /// name="incidentId"/> corresponds to a valid incident.</remarks>
    /// <param name="incidentId">The unique identifier of the incident to delete. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Typically returns an HTTP 200 OK response
    /// with the result of the deletion.</returns>
    [HttpDelete("{incidentId}")] public async Task<IActionResult> Delete(string incidentId)
    {
        var result = await service.DeleteAsync(incidentId);
        return Ok(result);
    }
}
