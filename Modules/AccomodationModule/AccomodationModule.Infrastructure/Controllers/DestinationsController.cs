using AccomodationModule.Domain.Arguments;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using FilingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Mvc;

namespace AccomodationModule.Infrastructure.Controllers
{
    /// <summary>
    /// API controller for managing destinations.
    /// Provides endpoints for CRUD operations on destinations.
    /// </summary>
    [Route("api/destinations"), ApiController]
    public class DestinationsController(IDestinationService destinationService) : ControllerBase
    {
        #region Get Methods

        /// <summary>
        /// Retrieves a paginated list of destinations based on the provided parameters.
        /// </summary>
        /// <param name="pageParameters">The parameters for pagination and filtering.</param>
        /// <returns>A paginated list of destinations.</returns>
        [HttpGet] public async Task<IActionResult> PagedDestinationsAsync([FromQuery] DestinationPageParameters pageParameters)
        {
            var newPackage = await destinationService.PagedDestinationsAsync(pageParameters, HttpContext.RequestAborted);
            return Ok(newPackage);
        }

        /// <summary>
        /// Retrieves all destinations.
        /// </summary>
        [HttpGet("all")] public async Task<IActionResult> AllDestinationsAsync()
        {
            var newPackage = await destinationService.AllDestinationsAsync(HttpContext.RequestAborted);
            return Ok(newPackage);
        }

        /// <summary>
        /// Retrieves the details of a specific destination by its ID.
        /// </summary>
        /// <param name="destinationId">The ID of the destination to retrieve.</param>
        /// <returns>The details of the specified destination.</returns>
        [HttpGet("{destinationId}")]
        public async Task<IActionResult> VacationAsync(string destinationId)
        {
            var result = await destinationService.DestinationAsync(destinationId, HttpContext.RequestAborted);
            return Ok(result);
        }

        #endregion

        #region Create Method

        /// <summary>
        /// Creates a new destination.
        /// </summary>
        /// <param name="dto">The data transfer object containing the details of the destination to create.</param>
        /// <returns>The result of the creation operation.</returns>
        [HttpPut] public async Task<IActionResult> CreateAsync([FromBody] DestinationDto dto)
        {
            var newPackage = await destinationService.CreateAsync(dto, HttpContext.RequestAborted);
            return Ok(newPackage);
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Updates an existing destination.
        /// </summary>
        /// <param name="dto">The data transfer object containing the updated details of the destination.</param>
        /// <returns>The result of the update operation.</returns>
        [HttpPost] public async Task<IActionResult> UpdateAsync([FromBody] DestinationDto dto)
        {
            var result = await destinationService.UpdateAsync(dto);
            return Ok(result);
        }

        #endregion

        #region Delete Method

        /// <summary>
        /// Deletes a destination by its ID.
        /// </summary>
        /// <param name="destinationId">The ID of the destination to delete.</param>
        /// <returns>The result of the deletion operation.</returns>
        [HttpDelete("{destinationId}")] public async Task<IActionResult> RemoveAsync(string destinationId)
        {
            var result = await destinationService.RemoveAsync(destinationId, HttpContext.RequestAborted);
            return Ok(result);
        }

        #endregion

        #region Images

        /// <summary>
        /// Adds an image to a vacation destination.
        /// </summary>
        /// <remarks>This method processes an HTTP POST request to add an image to a specified vacation
        /// destination. The operation is asynchronous and respects the cancellation token provided by the HTTP
        /// context.</remarks>
        /// <param name="dto">The data transfer object containing the image details to be added.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPost("addImage")]
        public async Task<IActionResult> AddImage([FromBody] AddEntityImageRequest dto)
        {
            var result = await destinationService.AddImage(dto, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Removes a vacation image identified by the specified image ID.
        /// </summary>
        /// <remarks>This method sends a request to delete the image associated with the given image ID.
        /// The operation respects the cancellation token provided by the HTTP context.</remarks>
        /// <param name="imageId">The unique identifier of the image to be removed.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpDelete("deleteImage/{imageId}/")]
        public async Task<IActionResult> RemoveImage(string imageId)
        {
            var result = await destinationService.RemoveImage(imageId, HttpContext.RequestAborted);
            return Ok(result);
        }

        #endregion

        #region Videos

        /// <summary>
        /// Adds a new vacation video to the specified destination.
        /// </summary>
        /// <remarks>This method processes an HTTP POST request to add a video associated with a
        /// destination. The operation is asynchronous and respects the cancellation token provided by the HTTP
        /// context.</remarks>
        /// <param name="dto">The data transfer object containing the video details to be added.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPost("addVideo")]
        public async Task<IActionResult> AddVacationVideo([FromBody] AddEntityVideoRequest dto)
        {
            var result = await destinationService.AddVideo(dto, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Removes a vacation video identified by the specified video ID.
        /// </summary>
        /// <remarks>This method sends a request to delete a video from the vacation video collection. The
        /// operation is asynchronous and respects the cancellation token provided by the HTTP context.</remarks>
        /// <param name="videoId">The unique identifier of the video to be removed.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpDelete("deleteVideo/{videoId}/")]
        public async Task<IActionResult> RemoveVacationVideo(string videoId)
        {
            var result = await destinationService.RemoveVideo(videoId, HttpContext.RequestAborted);
            return Ok(result);
        }

        #endregion
    }
}
