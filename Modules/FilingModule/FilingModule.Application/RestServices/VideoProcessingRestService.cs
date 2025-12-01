using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.DataTransferObjects;
using FilingModule.Domain.Interfaces;
using FilingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Http;

namespace FilingModule.Application.RestServices
{
    /// <summary>
    /// Provides REST-based video processing operations, including video uploads and metadata retrieval.
    /// </summary>
    /// <remarks>This service allows clients to interact with a video processing backend via HTTP. It supports
    /// uploading video files and retrieving metadata about existing videos. The service relies on an <see
    /// cref="IBaseHttpProvider"/> for making HTTP requests and implements the <see cref="IVideoProcessingService"/>
    /// interface.</remarks>
    /// <param name="provider"></param>
    public class VideoProcessingRestService(IBaseHttpProvider provider) : IVideoProcessingService
    {
        /// <summary>
        /// Retrieves a collection of all available videos.
        /// </summary>
        /// <remarks>This method fetches all videos from the underlying data provider. The operation may
        /// be canceled by passing a cancellation token.</remarks>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// containing an enumerable collection of <see cref="VideoDto"/> objects representing the videos.</returns>
        public async Task<IBaseResult<IEnumerable<VideoDto>>> AllVideosAsync(CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<VideoDto>>("videos/all");
            return result;
        }

        /// <summary>
        /// Uploads a video to the server using the specified upload request model.
        /// </summary>
        /// <remarks>This method sends the video upload request to the server and returns the server's
        /// response. Ensure that the <paramref name="model"/> parameter contains all required fields for the upload to
        /// succeed.</remarks>
        /// <param name="model">The request model containing the video data and metadata required for the upload.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the server's response to the video upload request.</returns>
        public async Task<IBaseResult<VideoUploadResponse>> UploadVideoAsync(VideoUploadRequest model, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<VideoUploadResponse, VideoUploadRequest>("videos/upload", model);
            return result;
        }

        /// <summary>
        /// Uploads a video file asynchronously to the server.
        /// </summary>
        /// <remarks>This method sends the video file to the server using an HTTP POST request. Ensure the
        /// <paramref name="file"/> contains valid video content and meets the server's requirements for
        /// upload.</remarks>
        /// <param name="file">The video file content to be uploaded, represented as an <see cref="HttpContent"/> object. Cannot be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the server's response, including details about the uploaded video.</returns>
        public async Task<IBaseResult<VideoUploadResponse>> UploadVideoAsync(HttpContent file, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostingAsync<VideoUploadResponse>("videos/uploadMemoryStream", file);
            return result;
        }

        /// <summary>
        /// Uploads a video file asynchronously to the server.
        /// </summary>
        /// <remarks>This method sends the video file to the server using an HTTP POST request. Ensure the
        /// <paramref name="file"/> contains valid video content and meets the server's requirements for
        /// upload.</remarks>
        /// <param name="file">The video file content to be uploaded, represented as an <see cref="HttpContent"/> object. Cannot be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the server's response, including details about the uploaded video.</returns>
        public async Task<IBaseResult<VideoUploadResponse>> UploadVideoAsync(IFormFile file, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException("Call is reserved for server side only");
        }

        /// <summary>
        /// Retrieves information about a video file based on the specified file name.
        /// </summary>
        /// <remarks>This method sends a request to retrieve metadata about a video file. Ensure the
        /// <paramref name="fileName"/> corresponds to a valid file in the system. The operation may fail if the file
        /// does not exist or if there are network issues.</remarks>
        /// <param name="fileName">The name of the video file to retrieve information for. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the file information as a <see cref="FileInfoResponse"/>. If the file is not found, the result
        /// may indicate an error.</returns>
        public async Task<IBaseResult<FileInfoResponse>> GetInfoAsync(string fileName, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<FileInfoResponse>($"videos/info/{fileName}");
            return result;
        }

        /// <summary>
        /// Deletes a video with the specified identifier.
        /// </summary>
        /// <remarks>This method sends a delete request to the underlying provider to remove the video
        /// resource.  Ensure that the <paramref name="videoId"/> corresponds to an existing video.</remarks>
        /// <param name="videoId">The unique identifier of the video to delete. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>An <see cref="IBaseResult"/> representing the result of the delete operation. The result indicates whether
        /// the operation was successful.</returns>
        public async Task<IBaseResult> DeleteVideoAsync(string videoId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"videos", videoId);
            return result;
        }
    }
}
