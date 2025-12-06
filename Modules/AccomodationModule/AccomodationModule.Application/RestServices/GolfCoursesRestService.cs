using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.RequestFeatures;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.RequestFeatures;

namespace AccomodationModule.Application.RestServices
{
    /// <summary>
    /// Provides a REST-based implementation of the <see cref="IGolfCoursesService"/> interface for managing golf
    /// courses and their associated media.
    /// </summary>
    /// <remarks>This service interacts with a RESTful API to perform operations such as retrieving, creating,
    /// updating, and deleting golf courses, as well as managing associated images and videos. It relies on an <see
    /// cref="IBaseHttpProvider"/> to handle HTTP requests and responses.</remarks>
    /// <param name="provider"></param>
    public class GolfCoursesRestService(IBaseHttpProvider provider) : IGolfCoursesService
    {
        /// <summary>
        /// Retrieves a paginated list of golf courses based on the specified request parameters.
        /// </summary>
        /// <remarks>This method fetches data from the "golfcourses" endpoint and supports pagination
        /// through the <paramref name="pageParameters"/> parameter. The caller can use the returned pagination metadata
        /// to navigate through the available pages.</remarks>
        /// <param name="pageParameters">The pagination and filtering parameters used to define the subset of golf courses to retrieve.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a collection of <see cref="GolfCourseDto"/> objects and
        /// pagination metadata.</returns>
        public async Task<PaginatedResult<GolfCourseDto>> PagedGolfCoursesAsync(RequestParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetPagedAsync<GolfCourseDto, RequestParameters>("golfcourses", pageParameters);
            return result;
        }

        /// <summary>
        /// Retrieves information about a specific golf course.
        /// </summary>
        /// <remarks>This method sends a request to retrieve the details of the specified golf course.
        /// Ensure that the  <paramref name="golfCourseId"/> is valid and corresponds to an existing golf course in the
        /// system.</remarks>
        /// <param name="golfCourseId">The unique identifier of the golf course to retrieve.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult{T}"/>
        /// object with the details of the golf course as a <see cref="GolfCourseDto"/>.</returns>
        public async Task<IBaseResult<GolfCourseDto>> GolfCourseAsync(string golfCourseId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<GolfCourseDto>($"golfcourses/{golfCourseId}");
            return result;
        }

        /// <summary>
        /// Creates or updates a golf course asynchronously.
        /// </summary>
        /// <remarks>This method sends the provided golf course data to the underlying provider for
        /// creation or update.  Ensure that the <paramref name="dto"/> parameter contains valid data before calling
        /// this method.</remarks>
        /// <param name="dto">The data transfer object containing the details of the golf course to create or update.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> CreateAsync(GolfCourseDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync($"golfcourses", dto);
            return result;
        }

        /// <summary>
        /// Updates the golf course information asynchronously.
        /// </summary>
        /// <param name="dto">The data transfer object containing the updated golf course information.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateAsync(GolfCourseDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"golfcourses", dto);
            return result;
        }

        /// <summary>
        /// Removes a golf course with the specified identifier asynchronously.
        /// </summary>
        /// <param name="golfCourseId">The unique identifier of the golf course to remove. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the removal operation.</returns>
        public async Task<IBaseResult> RemoveAsync(string golfCourseId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"golfcourses", golfCourseId);
            return result;
        }

        /// <summary>
        /// Adds an image to an entity based on the provided request.
        /// </summary>
        /// <remarks>This method sends a request to add an image to an entity. Ensure that the <paramref
        /// name="request"/> contains valid data, including the entity identifier and image details, before calling this
        /// method.</remarks>
        /// <param name="request">The request containing the details of the image to be added, including the entity identifier and image data.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"golfcourses/addImage", request);
            return result;
        }

        /// <summary>
        /// Removes an image with the specified identifier.
        /// </summary>
        /// <remarks>This method sends a request to delete the image identified by <paramref
        /// name="imageId"/>.  Ensure that the provided identifier corresponds to an existing image.</remarks>
        /// <param name="imageId">The unique identifier of the image to be removed. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveImage(string imageId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"golfcourses/deleteImage", imageId);
            return result;
        }

        /// <summary>
        /// Adds a video to the specified entity.
        /// </summary>
        /// <remarks>This method sends a request to add a video to an entity, such as a golf course, using
        /// the provided details. Ensure that the <paramref name="request"/> object contains all required fields before
        /// calling this method.</remarks>
        /// <param name="request">The request object containing the details of the video to be added.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> AddVideo(AddEntityVideoRequest request, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"golfcourses/addVideo", request);
            return result;
        }

        /// <summary>
        /// Removes a video with the specified identifier.
        /// </summary>
        /// <remarks>This method sends a request to delete the video identified by <paramref
        /// name="videoId"/>.  Ensure that the provided identifier corresponds to an existing video.</remarks>
        /// <param name="videoId">The unique identifier of the video to be removed. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveVideo(string videoId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"golfcourses/deleteVideo", videoId);
            return result;
        }
    }
}
