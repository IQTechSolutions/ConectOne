using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccomodationModule.Infrastructure.Controllers
{
    /// <summary>
    /// Provides API endpoints for managing lodging types.
    /// </summary>
    /// <remarks>This controller allows clients to perform CRUD operations on lodging types, including
    /// retrieving all lodging types,  retrieving a specific lodging type by ID, creating a new lodging type, updating
    /// an existing lodging type, and deleting a lodging type.</remarks>
    /// <param name="lodgingTypeService"></param>
    [Route("api/lodgingTypes"),ApiController]
    public class LodgingTypeController(ILodgingTypeService lodgingTypeService) : ControllerBase
    {
        /// <summary>
        /// Retrieves a list of all lodging types.
        /// </summary>
        /// <remarks>This method returns a collection of lodging types available in the system.  The
        /// result is returned as an HTTP 200 OK response containing the data.</remarks>
        /// <returns>An <see cref="IActionResult"/> containing an HTTP 200 OK response with the list of lodging types.</returns>
        [HttpGet] public async Task<IActionResult> LodgingTypes()
        {
            var lodgingTypes = await lodgingTypeService.AllLodgingTypesAsync();
            return Ok(lodgingTypes);
        }

        /// <summary>
        /// Retrieves a lodging package based on the specified lodging type identifier.
        /// </summary>
        /// <remarks>This method calls the lodging type service to retrieve the package details for the
        /// given  lodging type identifier. The result is returned as an HTTP 200 OK response if successful.</remarks>
        /// <param name="lodgingTypeId">The unique identifier of the lodging type for which the package is to be retrieved.  This value cannot be
        /// null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> containing the lodging package associated with the specified  lodging type
        /// identifier, or an appropriate HTTP response if the operation fails.</returns>
        [HttpGet("{lodgingTypeId}")] public async Task<IActionResult> Package(string lodgingTypeId)
        {
            var newPackage = await lodgingTypeService.LodgingTypeAsync(lodgingTypeId);
            return Ok(newPackage);
        }

        /// <summary>
        /// Creates a new lodging type based on the provided data transfer object (DTO).
        /// </summary>
        /// <param name="dto">The data transfer object containing the details of the lodging type to create. Cannot be null.</param>
        /// <returns>An <see cref="IActionResult"/> containing the newly created lodging type if the operation is successful.
        /// Returns an HTTP 200 OK response with the created lodging type in the response body.</returns>
        [HttpPut] public async Task<IActionResult> Create([FromBody] LodgingTypeDto dto)
        {
            var newPackage = await lodgingTypeService.CreateLodgingTypeAsync(dto);
            return Ok(newPackage);
        }

        /// <summary>
        /// Updates an existing lodging type with the provided data.
        /// </summary>
        /// <remarks>This method processes an HTTP POST request to update an existing lodging type.  The
        /// <paramref name="dto"/> parameter is expected to include all necessary fields  for the update operation. The
        /// result of the update is returned in the response body.</remarks>
        /// <param name="dto">The data transfer object containing the updated details of the lodging type.  This parameter must not be
        /// <see langword="null"/>.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the update operation.  Typically, this will be an
        /// HTTP 200 response with the updated lodging type data.</returns>
        [HttpPost] public async Task<IActionResult> Edit([FromBody] LodgingTypeDto dto)
        {
            var result = await lodgingTypeService.UpdateLodgingTypeAsync(dto);
            return Ok(result);
        }

        /// <summary>
        /// Deletes the lodging type with the specified identifier.
        /// </summary>
        /// <remarks>This method removes the lodging type associated with the provided identifier. Ensure
        /// that the identifier corresponds to an existing lodging type.</remarks>
        /// <param name="lodgingTypeId">The unique identifier of the lodging type to delete. Cannot be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Typically returns an HTTP 200
        /// response with the result of the deletion.</returns>
        [HttpDelete("{lodgingTypeId}")] public async Task<IActionResult> Delete(string lodgingTypeId)
        {
            var result = await lodgingTypeService.RemoveLodgingTypeAsync(lodgingTypeId);
            return Ok(result);
        }
    }
}
