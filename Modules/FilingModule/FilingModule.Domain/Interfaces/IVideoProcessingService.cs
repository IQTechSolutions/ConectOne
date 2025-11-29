using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.DataTransferObjects;
using FilingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Http;

namespace FilingModule.Domain.Interfaces
{
    /// <summary>
    /// Defines a contract for video processing services, including video uploads, image uploads, and file information
    /// retrieval.
    /// </summary>
    /// <remarks>This interface provides methods for handling video and image uploads, as well as retrieving
    /// metadata about files. Implementations of this interface should ensure proper validation of input parameters and
    /// handle cancellation tokens appropriately for asynchronous operations.</remarks>
    public interface IVideoProcessingService
    {
        /// <summary>
        /// Retrieves all available videos asynchronously.
        /// </summary>
        /// <remarks>The method returns an empty collection if no videos are available. The operation
        /// supports cancellation through the provided <paramref name="cancellationToken"/>.</remarks>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// with an enumerable collection of <see cref="VideoDto"/> objects representing the videos.</returns>
        Task<IBaseResult<IEnumerable<VideoDto>>> AllVideosAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Uploads a video based on the provided request model and returns the result.
        /// </summary>
        /// <remarks>The method performs an asynchronous upload operation. Ensure that the provided
        /// <paramref name="model"/> contains valid video data and metadata. The operation may be canceled using the
        /// <paramref name="cancellationToken"/>.</remarks>
        /// <param name="model">The request model containing the video data and associated metadata to be uploaded.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation can be canceled by passing a cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the uploaded video's details encapsulated in an <see cref="ImageDto"/> instance.</returns>
        Task<IBaseResult<VideoUploadResponse>> UploadVideoAsync(VideoUploadRequest model, CancellationToken cancellationToken = default);

        /// <summary>
        /// Uploads a video file asynchronously and returns the result of the upload operation.
        /// </summary>
        /// <remarks>The method uploads the provided video file and returns the result, which includes
        /// information about the upload status and any additional response data. Ensure the video file meets the
        /// required format and size constraints before calling this method.</remarks>
        /// <param name="video">The video file to be uploaded. Must not be <see langword="null"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the details of the upload operation, including the response data.</returns>
        Task<IBaseResult<VideoUploadResponse>> UploadVideoAsync(HttpContent video, CancellationToken cancellationToken = default);

        /// <summary>
        /// Uploads a video file asynchronously and returns the result of the upload operation.
        /// </summary>
        /// <remarks>The method uploads the provided video file and returns the result, which includes
        /// information about the upload status and any additional response data. Ensure the video file meets the
        /// required format and size constraints before calling this method.</remarks>
        /// <param name="video">The video file to be uploaded. Must not be <see langword="null"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the details of the upload operation, including the response data.</returns>
        Task<IBaseResult<VideoUploadResponse>> UploadVideoAsync(IFormFile video, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves information about a file based on its name.
        /// </summary>
        /// <remarks>The method performs an asynchronous operation to fetch file details. Ensure the
        /// provided file name is valid and accessible within the context of the operation. The returned <see
        /// cref="IBaseResult{T}"/> may indicate success or failure, depending on the outcome of the
        /// operation.</remarks>
        /// <param name="fileName">The name of the file to retrieve information for. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping a <see cref="FileInfoResponse"/> with the file's details.</returns>
        Task<IBaseResult<FileInfoResponse>> GetInfoAsync(string fileName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the video with the specified identifier.
        /// </summary>
        /// <remarks>The operation will fail if the specified video does not exist or if the caller lacks
        /// the necessary permissions.</remarks>
        /// <param name="videoId">The unique identifier of the video to delete. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the delete operation.</returns>
        Task<IBaseResult> DeleteVideoAsync(string videoId, CancellationToken cancellationToken = default);
    }
}
