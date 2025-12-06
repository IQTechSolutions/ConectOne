using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.RequestFeatures;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccomodationModule.Infrastructure.Controllers
{
    /// <summary>
    /// Provides endpoints for managing vacation hosts, including retrieving, creating, updating, and deleting vacation
    /// host data.
    /// </summary>
    /// <remarks>This controller handles HTTP requests related to vacation hosts, such as paginated retrieval,
    /// fetching by ID or name, creating new vacation hosts, updating existing ones, and removing them. It interacts
    /// with the <see cref="IVacationHostService"/> to perform the necessary operations.</remarks>
    /// <param name="vacationHostService"></param>
    [Route("api/vacationHosts"), ApiController]
    public class VacationHostController(IVacationHostService vacationHostService) : ControllerBase
    {
        /// <summary>
        /// Retrieves a paginated list of vacation hosts based on the specified request parameters.
        /// </summary>
        /// <remarks>This method uses the provided pagination parameters to query and return a subset of
        /// vacation hosts. The result is returned as an HTTP response with a status code of 200 (OK) if
        /// successful.</remarks>
        /// <param name="pageParameters">The parameters used to define pagination settings, such as page number and page size.</param>
        /// <returns>An <see cref="IActionResult"/> containing the paginated list of vacation hosts.</returns>
        [HttpGet] public async Task<IActionResult> PagedVacationHostsAsync([FromQuery] RequestParameters pageParameters)
        {
            var result = await vacationHostService.PagedVacationHostsAsync(pageParameters, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a list of all vacation hosts.
        /// </summary>
        /// <remarks>This method returns an HTTP 200 OK response containing the list of vacation hosts. If
        /// the request is canceled, the operation is aborted.</remarks>
        /// <returns>An <see cref="IActionResult"/> containing the list of vacation hosts.</returns>
        [HttpGet("all")] public async Task<IActionResult> AllVacationHostsAsync()
        {
            var result = await vacationHostService.AllVacationHostsAsync(HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves vacation host details based on the specified identifier.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch vacation host details. The
        /// request can be canceled using the <see cref="HttpContext.RequestAborted"/> token.</remarks>
        /// <param name="vacationHostId">The unique identifier of the vacation host to retrieve. Cannot be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> containing the vacation host details if found, or an appropriate HTTP
        /// response if not.</returns>
        [HttpGet("{vacationHostId}")] public async Task<IActionResult> VacationHostAsync(string vacationHostId)
        {
            var result = await vacationHostService.VacationHostAsync(vacationHostId, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves vacation host details based on the specified vacation host name.
        /// </summary>
        /// <remarks>This method uses the <see cref="HttpContext.RequestAborted"/> token to handle request
        /// cancellation.</remarks>
        /// <param name="vacationHostName">The name of the vacation host to retrieve details for. Cannot be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> containing the vacation host details if found. Returns a 200 OK response with
        /// the vacation host data, or an appropriate error response if the host is not found.</returns>
        [HttpGet("fromName/{vacationName}")] public async Task<IActionResult> VacationHostFromNameAsync(string vacationHostName)
        {
            var newPackage = await vacationHostService.VacationHostFromNameAsync(vacationHostName, HttpContext.RequestAborted);
            return Ok(newPackage);
        }

        /// <summary>
        /// Creates a new vacation host package asynchronously.
        /// </summary>
        /// <remarks>If the <paramref name="dto"/> includes a non-empty image path, the image will be
        /// resized and stored in the application's static files directory. The operation respects the cancellation
        /// token provided by the HTTP request.</remarks>
        /// <param name="dto">The data transfer object containing the details of the vacation host package to create. Must include valid
        /// data for the package.</param>
        /// <returns>An <see cref="IActionResult"/> containing the newly created vacation host package.</returns>
        [HttpPut] public async Task<IActionResult> CreateAsync([FromBody] VacationHostDto dto)
        {
	        var newPackage = await vacationHostService.CreateAsync(dto, HttpContext.RequestAborted);
            return Ok(newPackage);
        }

        /// <summary>
        /// Updates the details of a vacation host asynchronously.
        /// </summary>
        /// <remarks>This method processes the provided vacation host data, including handling image
        /// updates if applicable. If the image path specified in <paramref name="dto"/> differs from the existing image
        /// path, the image is resized. The operation respects the cancellation token provided by the HTTP
        /// context.</remarks>
        /// <param name="dto">The data transfer object containing the updated vacation host information. The <see
        /// cref="VacationHostDto.VacationHostId"/> property must specify the ID of the vacation host to update.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the update operation. Returns <see
        /// cref="OkObjectResult"/> containing the updated vacation host details if the operation succeeds.</returns>
        [HttpPost] public async Task<IActionResult> UpdateAsync([FromBody] VacationHostDto dto)
        {
	        var result = await vacationHostService.UpdateAsync(dto, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Deletes the specified vacation host asynchronously.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to delete a vacation host identified
        /// by <paramref name="vacationHostId"/>. The operation respects the cancellation token provided by the HTTP
        /// context.</remarks>
        /// <param name="vacationHostId">The unique identifier of the vacation host to be removed. Cannot be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Typically returns an HTTP 200 status
        /// code if the removal is successful.</returns>
        [HttpDelete("{vacationHostId}")] public async Task<IActionResult> RemoveAsync(string vacationHostId)
        {
            var result = await vacationHostService.RemoveAsync(vacationHostId, HttpContext.RequestAborted);
            return Ok(result);
        }

        #region Images

        /// <summary>
        /// Adds an image to the specified entity with the provided details.
        /// </summary>
        /// <remarks>This method creates an association between an image and an entity, saving the image details
        /// to the repository. The operation will fail if the repository operations (create or save) are
        /// unsuccessful.</remarks>
        /// <param name="request">The request containing the image details, including the image ID, entity ID, selector, and order.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation. If the operation fails, the result includes error messages.</returns>
        [HttpPost("addImage")]
        public async Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default)
        {
            var addResult = await vacationHostService.AddImage(request, cancellationToken);
            return addResult;
        }

        /// <summary>
        /// Removes an image with the specified identifier from the repository.
        /// </summary>
        /// <remarks>This method first attempts to delete the image from the repository. If the deletion succeeds,
        /// it then saves the changes to the repository. If either operation fails, the method returns a failure result with
        /// the associated error messages.</remarks>
        /// <param name="imageId">The unique identifier of the image to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation. If the operation fails, the result includes error messages.</returns>
        [HttpDelete("deleteImage/{imageId}/")]
        public async Task<IBaseResult> RemoveImage(string imageId, CancellationToken cancellationToken = default)
        {
            var addResult = await vacationHostService.RemoveImage(imageId, cancellationToken);
            return addResult;
        }

        #endregion
    }
}
