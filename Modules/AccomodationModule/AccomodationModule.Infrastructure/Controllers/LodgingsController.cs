using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using AccomodationModule.Domain.RequestFeatures;
using FilingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Mvc;

namespace AccomodationModule.Infrastructure.Controllers
{
    /// <summary>
    /// API controller for managing lodging-related operations.
    /// </summary>
    [Route("api/lodgings"), ApiController]
    public class LodgingsController(ILodgingService lodgingService, IRoomDataService roomDataService) : ControllerBase
    {
        #region Lodging Listing Requests

        /// <summary>
        /// Retrieves the count of lodgings.
        /// </summary>
        /// <returns>An IActionResult containing the count of lodgings.</returns>
        [HttpGet("count")] public async Task<IActionResult> LodgingsCountAsync()
        {
            var result = await lodgingService.LodgingCountAsync(HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a paged list of lodging listing requests based on the provided parameters.
        /// </summary>
        /// <param name="parameters">The parameters for filtering, sorting, and pagination.</param>
        /// <returns>An IActionResult containing the paged list of lodging listing requests.</returns>
        [HttpGet("lodginglistingreqeust")] public async Task<IActionResult> PagedLodgingListingRequests([FromQuery] LodgingListingRequestParameters parameters)
        {
            var result = await lodgingService.PagedLodgingListReqeusts(parameters, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new lodging listing request.
        /// </summary>
        /// <param name="model">The DTO containing the data for the new lodging listing request.</param>
        /// <returns>An IActionResult indicating the result of the create operation.</returns>
        [HttpPut("lodginglistingreqeust")] public async Task<IActionResult> CreateLodgingListingRequest([FromBody] LodgingListingRequestDto model)
        {
            var lodgingListingReqeust = await lodgingService.CreateLodgingListReqeust(model, HttpContext.RequestAborted);
            return Ok(lodgingListingReqeust);
        }

        #endregion

        /// <summary>
        /// Retrieves a paged list of lodgings based on the provided parameters.
        /// </summary>
        /// <param name="parameters">The parameters for filtering, sorting, and pagination.</param>
        /// <returns>An IActionResult containing the paged list of lodgings.</returns>
        [HttpGet] public async Task<IActionResult> PagedLogingsAsync([FromQuery] LodgingParameters parameters)
        {
            var result = await lodgingService.PagedLodgingsAsync(parameters, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves all lodgings.
        /// </summary>
        /// <returns>An IActionResult containing all lodgings.</returns>
        [HttpGet("all")] public async Task<IActionResult> AllLodgings()
        {
            var lodging = await lodgingService.AllLodgings(null, HttpContext.RequestAborted);
            return Ok(lodging);
        }

        /// <summary>
        /// Retrieves a specific lodging by its ID.
        /// </summary>
        /// <param name="lodgingid">The ID of the lodging to retrieve.</param>
        /// <returns>An IActionResult containing the lodging with the specified ID.</returns>
        [HttpGet("details/{lodgingid}")] public async Task<IActionResult> Lodging(string lodgingid)
        {
            var lodging = await lodgingService.LodgingAsync(lodgingid, HttpContext.RequestAborted);
            return Ok(lodging);
        }

        /// <summary>
        /// Retrieves a specific lodging by its ID.
        /// </summary>
        /// <param name="bbid">The ID of the lodging to retrieve.</param>
        /// <returns>An IActionResult containing the lodging with the specified ID.</returns>
        [HttpGet("details/byUniqueId/{bbid}")]
        public async Task<IActionResult> ProductByUniqueServiceIdAsync(string bbid)
        {
            var lodging = await lodgingService.ProductByUniqueServiceIdAsync(bbid, HttpContext.RequestAborted);
            return Ok(lodging);
        }

        /// <summary>
        /// Creates a new lodging.
        /// </summary>
        /// <param name="lodging">The DTO containing the data for the new lodging.</param>
        /// <returns>An IActionResult indicating the result of the create operation.</returns>
        [HttpPut("createLodging")] public async Task<IActionResult> Create([FromBody] LodgingDto lodging)
        {
            var newLodging = await lodgingService.CreateLodgingAsync(lodging);
            return Ok(newLodging);
        }

        /// <summary>
        /// Updates an existing lodging.
        /// </summary>
        /// <param name="model">The DTO containing the updated data for the lodging.</param>
        /// <returns>An IActionResult indicating the result of the update operation.</returns>
        [HttpPost] public async Task<IActionResult> Edit([FromBody] LodgingDto model)
        {
            var result = await lodgingService.UpdateLodgingAsync(model);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a lodging by its ID.
        /// </summary>
        /// <param name="lodgingId">The ID of the lodging to delete.</param>
        /// <returns>An IActionResult indicating the result of the delete operation.</returns>
        [HttpDelete("{lodgingId}")] public async Task<IActionResult> Delete(string lodgingId)
        {
            var result = await lodgingService.RemoveLodgingAsync(lodgingId);
            return Ok(result);
        }

        #region CancellationRules

        /// <summary>
        /// Creates a new cancellation rule.
        /// </summary>
        /// <param name="modal">The DTO containing the data for the new cancellation rule.</param>
        /// <returns>An IActionResult indicating the result of the create operation.</returns>
        [HttpPut("createCancellationRule")] public async Task<IActionResult> CreateCancellationRule([FromBody] CancellationRuleDto modal)
        {
            var result = await lodgingService.CreateCancellationRule(modal);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a cancellation rule by its ID.
        /// </summary>
        /// <param name="cancellationRuleId">The ID of the cancellation rule to delete.</param>
        /// <returns>An IActionResult indicating the result of the delete operation.</returns>
        [HttpDelete("deleteCancellationRule/{cancellationRuleId}")] public async Task<IActionResult> DeleteCancellationRule(int cancellationRuleId)
        {
            var result = await lodgingService.RemoveCancellationRule(cancellationRuleId);
            return Ok(result);
        }

        #endregion

        #region Images

        /// <summary>
        /// Adds an image to a vacation entity.
        /// </summary>
        /// <remarks>This method asynchronously processes the image addition request and returns an HTTP
        /// 200 OK response with the result.</remarks>
        /// <param name="dto">The data transfer object containing the image and associated entity information.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPost("addImage")] public async Task<IActionResult> AddVacationImage([FromBody] AddEntityImageRequest dto)
        {
            var result = await lodgingService.AddImage(dto, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Removes a vacation image identified by the specified image ID.
        /// </summary>
        /// <remarks>This method sends a request to delete the image associated with the given image ID.
        /// The operation is asynchronous and respects the cancellation token from the HTTP context.</remarks>
        /// <param name="imageId">The unique identifier of the image to be removed.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpDelete("deleteImage/{imageId}/")] public async Task<IActionResult> RemoveVacationImage(string imageId)
        {
            var result = await lodgingService.RemoveImage(imageId, HttpContext.RequestAborted);
            return Ok(result);
        }

        #endregion

        #region Videos

        /// <summary>
        /// Adds a new vacation video to the lodging service.
        /// </summary>
        /// <remarks>This method processes an HTTP POST request to add a video associated with a vacation
        /// entity. The operation is asynchronous and respects the cancellation token provided by the HTTP
        /// context.</remarks>
        /// <param name="dto">The data transfer object containing the video details to be added.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPost("addVideo")] public async Task<IActionResult> AddVacationVideo([FromBody] AddEntityVideoRequest dto)
        {
            var result = await lodgingService.AddVideo(dto, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Removes a vacation video identified by the specified video ID.
        /// </summary>
        /// <remarks>This method sends a request to delete the specified video and returns the result of
        /// the operation.</remarks>
        /// <param name="videoId">The unique identifier of the video to be removed. Cannot be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpDelete("deleteVideo/{videoId}/")] public async Task<IActionResult> RemoveVacationVideo(string videoId)
        {
            var result = await lodgingService.RemoveVideo(videoId, HttpContext.RequestAborted);
            return Ok(result);
        }

        #endregion

        #region Featured Images

        [HttpPost("setFeaturedCoverImage/{lodgingId}/{featuredImageId}")] public async Task<IActionResult> SetFeaturedCoverImage(string lodgingId, string featuredImageId)
        {
            var result = await lodgingService.SetFeaturedCoverImage(lodgingId, featuredImageId);
            return Ok(result);
        }

        [HttpPost("removeFeaturedCoverImage/{featuredImageId}")] public async Task<IActionResult> RemoveFeaturedCoverImage(string featuredImageId)
        {
            var result = await lodgingService.RemoveFeaturedCoverImage(featuredImageId);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves featured images for a specific lodging.
        /// </summary>
        /// <param name="lodgingId">The ID of the lodging to retrieve featured images for.</param>
        /// <returns>An IActionResult containing the featured images.</returns>
        [HttpGet("featuredImages/{lodgingId}")] public async Task<IActionResult> FeaturedImages(string lodgingId)
        {
            var lodging = await lodgingService.FeaturedImages(lodgingId);
            return Ok(lodging);
        }
        
        /// <summary>
        /// Adds a featured image to a lodging.
        /// </summary>
        /// <param name="dto">The DTO containing the data for the new featured image.</param>
        /// <returns>An IActionResult indicating the result of the add operation.</returns>
        [HttpPut("addFeaturedImage")] public async Task<IActionResult> AddFeaturedImage([FromBody] FeaturedImageDto dto)
        {
            var result = await lodgingService.AddFeaturedImage(dto);
            return Ok(result);
        }

        /// <summary>
        /// Removes a featured image from a lodging.
        /// </summary>
        /// <param name="featuredImageId">The ID of the featured image to remove.</param>
        /// <returns>An IActionResult indicating the result of the delete operation.</returns>
        [HttpDelete("featuredImages/{featuredImageId}")] public async Task<IActionResult> RemoveFeaturedImageAsync(string featuredImageId)
        {
            var result = await lodgingService.RemoveFeaturedLodgingImageAsync(featuredImageId);
            return Ok(result);
        }

        #endregion

	}
}
