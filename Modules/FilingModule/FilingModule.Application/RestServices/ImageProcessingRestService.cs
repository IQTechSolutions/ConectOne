using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.DataTransferObjects;
using FilingModule.Domain.Interfaces;
using FilingModule.Domain.RequestFeatures;

namespace FilingModule.Application.RestServices
{
    /// <summary>
    /// Provides REST-based image processing operations, including uploading, retrieving, and deleting images.
    /// </summary>
    /// <remarks>This service acts as a client for interacting with an image processing REST API. It supports
    /// multiple methods for uploading images, retrieving image metadata, and deleting images. The service relies on an
    /// HTTP provider to perform the underlying HTTP requests.</remarks>
    /// <param name="provider"></param>
    public class ImageProcessingRestService(IBaseHttpProvider provider) : IImageProcessingService
    {
        /// <summary>
        /// Retrieves all images asynchronously.
        /// </summary>
        /// <remarks>This method fetches all available images from the underlying data provider. The
        /// operation is performed asynchronously and supports cancellation through the provided <paramref
        /// name="cancellationToken"/>.</remarks>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// containing an enumerable collection of <see cref="ImageDto"/> objects representing the images.</returns>
        public async Task<IBaseResult<IEnumerable<ImageDto>>> AllImagesAsync(CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<ImageDto>>("images/all");
            return result;
        }

        /// <summary>
        /// Uploads an image encoded in Base64 format to the server.
        /// </summary>
        /// <remarks>This method sends the Base64-encoded image data to the server using a POST request.
        /// Ensure that the <paramref name="model"/> parameter contains valid Base64 data and any required metadata
        /// before calling this method.</remarks>
        /// <param name="model">The request model containing the Base64-encoded image data and associated metadata.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the uploaded image details as an <see cref="ImageDto"/> instance.</returns>
        public async Task<IBaseResult<ImageDto>> UploadImage(Base64ImageUploadRequest model, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<ImageDto, Base64ImageUploadRequest>("images/uploadbase64", model);
            return result;
        }

        /// <summary>
        /// Uploads an image to the server and returns the result.
        /// </summary>
        /// <remarks>This method sends the provided image data to the server using a POST request. The
        /// server's response is returned as an <see cref="IBaseResult{T}"/> containing the uploaded image
        /// details.</remarks>
        /// <param name="model">The image data to be uploaded, represented as an <see cref="ImageDto"/> object.</param>
        /// <param name="cancellationToken">An optional token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the uploaded image data.</returns>
        public async Task<IBaseResult<ImageDto>> UploadImage(ImageDto model, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<ImageDto, ImageDto>("images/uploadUrl", model);
            return result;
        }

        /// <summary>
        /// Uploads an image to the server and returns the result.
        /// </summary>
        /// <remarks>This method sends the image data to the server using a POST request. Ensure that the
        /// <paramref name="model"/> contains valid image data and metadata before calling this method. The operation
        /// can be canceled by passing a cancellation token.</remarks>
        /// <param name="model">The request model containing the image data and associated metadata to be uploaded.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the uploaded image details as an <see cref="ImageDto"/>.</returns>
        public async Task<IBaseResult<ImageDto>> UploadImage(ImageUploadRequest model, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<ImageDto, ImageUploadRequest>("images/upload", model);
            return result;
        }

        /// <summary>
        /// Retrieves information about a file with the specified name.
        /// </summary>
        /// <remarks>This method sends a request to retrieve file information from the provider. Ensure
        /// that the file name is valid and that the provider is properly configured before calling this
        /// method.</remarks>
        /// <param name="fileName">The name of the file to retrieve information for. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the file information as a <see cref="FileInfoResponse"/>. If the file does not exist, the result
        /// may indicate an error.</returns>
        public async Task<IBaseResult<FileInfoResponse>> GetInfoAsync(string fileName, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<FileInfoResponse>($"images/info/{fileName}");
            return result;
        }

        /// <summary>
        /// Deletes an image with the specified identifier.
        /// </summary>
        /// <remarks>This method sends a delete request to the underlying provider to remove the specified
        /// image.  Ensure that the <paramref name="imageId"/> corresponds to an existing image.</remarks>
        /// <param name="imageId">The unique identifier of the image to delete. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the delete operation.</returns>
        public async Task<IBaseResult> DeleteImageAsync(string imageId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"images", imageId);
            return result;
        }
    }
}
