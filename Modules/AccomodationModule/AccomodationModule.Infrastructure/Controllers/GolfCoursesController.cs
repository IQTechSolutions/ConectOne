using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.RequestFeatures;
using FilingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Mvc;

namespace AccomodationModule.Infrastructure.Controllers
{
    /// <summary>
    /// API controller for managing golf courses.
    /// Provides endpoints for retrieving, creating, updating, and deleting golf course data.
    /// </summary>
    [Route("api/golfcourses"), ApiController]
    public class GolfCoursesController(IGolfCoursesService golfCourseService) : ControllerBase
    {
        #region HTTP Endpoints

        /// <summary>
        /// Retrieves a paginated list of golf courses.
        /// </summary>
        /// <param name="pageParameters">The parameters for pagination, such as page number and page size.</param>
        /// <returns>A paginated list of golf courses.</returns>
        [HttpGet]
        public async Task<IActionResult> PagedGolfCoursesAsync([FromQuery] RequestParameters pageParameters)
        {
            var newPackage = await golfCourseService.PagedGolfCoursesAsync(pageParameters, HttpContext.RequestAborted);
            return Ok(newPackage);
        }

        /// <summary>
        /// Retrieves a specific golf course by its unique identifier.
        /// </summary>
        /// <param name="golfCourseId">The unique identifier of the golf course.</param>
        /// <returns>The golf course data.</returns>
        [HttpGet("{golfCourseId}")]
        public async Task<IActionResult> GolfCourseAsync(string golfCourseId)
        {
            var newPackage = await golfCourseService.GolfCourseAsync(golfCourseId, HttpContext.RequestAborted);
            return Ok(newPackage);
        }

        /// <summary>
        /// Creates a new golf course.
        /// </summary>
        /// <param name="dto">The data transfer object containing the golf course details.</param>
        /// <returns>The result of the creation operation.</returns>
        [HttpPut]
        public async Task<IActionResult> CreateAsync([FromBody] GolfCourseDto dto)
        {
            var newPackage = await golfCourseService.CreateAsync(dto, HttpContext.RequestAborted);
            return Ok(newPackage);
        }

        /// <summary>
        /// Updates an existing golf course.
        /// </summary>
        /// <param name="dto">The data transfer object containing the updated golf course details.</param>
        /// <returns>The result of the update operation.</returns>
        [HttpPost]
        public async Task<IActionResult> UpdateAsync([FromBody] GolfCourseDto dto)
        {
            var result = await golfCourseService.UpdateAsync(dto, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a specific golf course by its unique identifier.
        /// </summary>
        /// <param name="golfCourseId">The unique identifier of the golf course to delete.</param>
        /// <returns>The result of the deletion operation.</returns>
        [HttpDelete("{golfCourseId}")]
        public async Task<IActionResult> RemoveAsync(string golfCourseId)
        {
            var result = await golfCourseService.RemoveAsync(golfCourseId, HttpContext.RequestAborted);
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
        public async Task<IActionResult> AddVacationImage([FromBody] AddEntityImageRequest dto)
        {
            var result = await golfCourseService.AddImage(dto, HttpContext.RequestAborted);
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
        public async Task<IActionResult> RemoveVacationImage(string imageId)
        {
            var result = await golfCourseService.RemoveImage(imageId, HttpContext.RequestAborted);
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
            var result = await golfCourseService.AddVideo(dto, HttpContext.RequestAborted);
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
            var result = await golfCourseService.RemoveVideo(videoId, HttpContext.RequestAborted);
            return Ok(result);
        }

        #endregion
    }
}
