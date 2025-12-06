using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccomodationModule.Infrastructure.Controllers
{
    /// <summary>
    /// Provides endpoints for managing areas in the system.
    /// </summary>
    /// <remarks>This controller exposes CRUD operations for areas, including retrieving all areas, 
    /// retrieving a specific area by its identifier, creating a new area, editing an existing area,  and deleting an
    /// area. All endpoints are accessible via HTTP and follow RESTful conventions.</remarks>
    /// <param name="areaService"></param>
    [Route("api/areas"),ApiController]
    public class AreasController(IAreaService areaService) : ControllerBase
    {
        /// <summary>
        /// Retrieves a list of all areas.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch all areas from the underlying
        /// service. The result is returned as an HTTP 200 OK response containing the list of areas.</remarks>
        /// <returns>An <see cref="IActionResult"/> containing the list of areas. The response is an HTTP 200 OK with the data if
        /// successful.</returns>
        [HttpGet] public async Task<IActionResult> AllAreas()
        {
            var result = await areaService.AllAreasAsync();
            return Ok(result);
        }

        /// <summary>
        /// Retrieves information about a specific area based on the provided area identifier.
        /// </summary>
        /// <remarks>This method uses the <c>areaService</c> to asynchronously fetch area details. The
        /// response is returned as an HTTP 200 OK status with the area data if successful, or an error status if the
        /// operation fails.</remarks>
        /// <param name="areaId">The unique identifier of the area to retrieve. Cannot be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> containing the area information if found, or an appropriate HTTP response
        /// indicating the result of the operation.</returns>
        [HttpGet("{areaId}")] public async Task<IActionResult> Area(string areaId)
        {
            var result = await areaService.AreaAsync(areaId);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new area using the provided data.
        /// </summary>
        /// <remarks>This method processes the creation of an area by delegating the operation to the
        /// underlying service.  Ensure that the <paramref name="area"/> parameter contains valid data before calling
        /// this method.</remarks>
        /// <param name="area">The data transfer object containing the details of the area to be created. Cannot be null.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation. Typically, this will be an HTTP 200
        /// response with the created area data.</returns>
        [HttpPut] public async Task<IActionResult> CreateArea([FromBody] AreaDto area)
        {
            var result = await areaService.CreateAreaAsync(area);
            return Ok(result);
        }

        /// <summary>
        /// Updates the details of an existing area.
        /// </summary>
        /// <remarks>This method processes an HTTP POST request to update the area information.  The
        /// <paramref name="package"/> parameter must include valid area data for the update to succeed.</remarks>
        /// <param name="package">The data transfer object containing the updated area details. Cannot be null.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the update operation.</returns>
        [HttpPost] public async Task<IActionResult> EditArea([FromBody] AreaDto package)
        {
            var result = await areaService.UpdateAreaAsync(package);
            return Ok(result);
        }

        /// <summary>
        /// Deletes an area identified by the specified area ID.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to remove the specified area.  Ensure
        /// the <paramref name="areaId"/> corresponds to a valid area before calling this method.</remarks>
        /// <param name="areaId">The unique identifier of the area to delete. Cannot be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.  Typically returns an HTTP 200
        /// response with the result of the deletion.</returns>
        [HttpDelete("{areaId}")] public async Task<IActionResult> DeleteArea(string areaId)
        {
            var result = await areaService.RemoveAreaAsync(areaId);
            return Ok(result);
        }
    }
}
