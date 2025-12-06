using AccomodationModule.Domain.DataTransferObjects;
using ConectOne.Domain.RequestFeatures;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.RequestFeatures;

namespace AccomodationModule.Domain.Interfaces
{
    /// <summary>
    /// Service contract for managing golf courses.
    /// Provides methods for retrieving, creating, updating, and deleting golf course data.
    /// </summary>
    public interface IGolfCoursesService
    {
        #region Methods

        /// <summary>
        /// Retrieves a paginated list of golf courses based on the specified request parameters.
        /// </summary>
        /// <remarks>Use this method to retrieve golf courses in a paginated format, which is useful for
        /// scenarios where the dataset is large and needs to be divided into manageable chunks.</remarks>
        /// <param name="pageParameters">The parameters that define the pagination and filtering criteria, such as page number and page size.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing the paginated list of golf courses. The result
        /// includes metadata such as total count and current page information.</returns>
        Task<PaginatedResult<GolfCourseDto>> PagedGolfCoursesAsync(RequestParameters pageParameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a specific golf course by its unique identifier.
        /// </summary>
        /// <param name="golfCourseId">The unique identifier of the golf course.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>An <see cref="IBaseResult{GolfCourseDto}"/> containing the golf course data.</returns>
        Task<IBaseResult<GolfCourseDto>> GolfCourseAsync(string golfCourseId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new golf course using the provided data transfer object.
        /// </summary>
        /// <param name="dto">The <see cref="GolfCourseDto"/> containing the data for the new golf course.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>An <see cref="IBaseResult"/> indicating the success or failure of the operation.</returns>
        Task<IBaseResult> CreateAsync(GolfCourseDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing golf course using the provided data transfer object.
        /// </summary>
        /// <param name="dto">The <see cref="GolfCourseDto"/> containing the updated data for the golf course.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>An <see cref="IBaseResult"/> indicating the success or failure of the operation.</returns>
        Task<IBaseResult> UpdateAsync(GolfCourseDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a specific golf course by its unique identifier.
        /// </summary>
        /// <param name="golfCourseId">The unique identifier of the golf course to remove.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>An <see cref="IBaseResult"/> indicating the success or failure of the operation.</returns>
        Task<IBaseResult> RemoveAsync(string golfCourseId, CancellationToken cancellationToken = default);

        #endregion

        #region Images

        /// <summary>
        /// Adds an image to the specified entity.
        /// </summary>
        /// <remarks>This method is asynchronous and can be awaited. Ensure that the <paramref
        /// name="request"/> parameter is properly populated with valid data before calling this method.</remarks>
        /// <param name="request">The request containing the details of the image to be added, including the entity identifier and image data.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
        Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes an image from the system based on the specified image identifier.
        /// </summary>
        /// <param name="imageId">The unique identifier of the image to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
        Task<IBaseResult> RemoveImage(string imageId, CancellationToken cancellationToken = default);

        #endregion

        #region Videos

        /// <summary>
        /// Adds a new video entity to the system.
        /// </summary>
        /// <remarks>This method initiates an asynchronous operation to add a video entity. Ensure that
        /// the <paramref name="request"/> parameter is properly populated with all required video details before
        /// calling this method.</remarks>
        /// <param name="request">The request object containing details of the video to be added. This includes metadata such as title,
        /// description, and video file information.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
        Task<IBaseResult> AddVideo(AddEntityVideoRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a video from the system using the specified video identifier.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to remove a video. Ensure that the
        /// video identifier is valid and that the operation is not cancelled prematurely.</remarks>
        /// <param name="videoId">The unique identifier of the video to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
        Task<IBaseResult> RemoveVideo(string videoId, CancellationToken cancellationToken = default);

        #endregion
    }
}
