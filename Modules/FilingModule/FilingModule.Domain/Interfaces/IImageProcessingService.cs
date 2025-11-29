using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.DataTransferObjects;
using FilingModule.Domain.RequestFeatures;

namespace FilingModule.Domain.Interfaces
{
    /// <summary>
    /// Defines methods for processing images and creating image-related entities.
    /// </summary>
    /// <typeparam name="T">The type of entity associated with the image processing operations. Must implement  <see
    /// cref="IAuditableEntity"/>.</typeparam>
    public interface IImageProcessingService
    {
        /// <summary>
        /// Retrieves all images asynchronously.
        /// </summary>
        /// <remarks>The returned collection may be empty if no images are available. Ensure to check the
        /// result's status to determine if the operation was successful.</remarks>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// containing an enumerable collection of <see cref="ImageDto"/> objects.</returns>
        Task<IBaseResult<IEnumerable<ImageDto>>> AllImagesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a cover image file from a base64-encoded image string and saves it to the specified path.
        /// </summary>
        /// <remarks>Ensure that the provided base64 string represents a valid image format. The method
        /// will overwrite the file at the specified path if it already exists.</remarks>
        /// <param name="base64Image">The base64-encoded string representing the image to be converted into a file. This parameter cannot be null
        /// or empty.</param>
        /// <param name="path">The file system path where the cover image will be saved. This parameter must be a valid path and writable.</param>
        /// <returns>A <see cref="FileBaseDto"/> object representing the created cover file, or <see langword="null"/> if the
        /// operation fails.</returns>
        Task<IBaseResult<ImageDto>> UploadImage(Base64ImageUploadRequest model, CancellationToken cancellationToken = default);

        /// <summary>
        /// Uploads an image to the server and returns the result of the operation.
        /// </summary>
        /// <remarks>The method performs an asynchronous upload of the provided image data. Ensure that
        /// the <paramref name="model"/> contains valid image content and metadata before calling this method. The
        /// operation may be canceled by passing a cancellation token.</remarks>
        /// <param name="model">The image data to be uploaded, including metadata and content.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation will terminate early if the token is canceled.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the uploaded image details and the status of the operation.</returns>
        Task<IBaseResult<ImageDto>> UploadImage(ImageDto model, CancellationToken cancellationToken = default);

        /// <summary>
        /// Uploads an image based on the provided request model.
        /// </summary>
        /// <remarks>The method performs an asynchronous operation to upload an image. Ensure that the
        /// provided <paramref name="model"/> contains valid image data and metadata. The operation can be canceled by
        /// passing a cancellation token.</remarks>
        /// <param name="model">The request model containing the image data and associated metadata to be uploaded.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the uploaded image details as an <see cref="ImageDto"/>.</returns>
        Task<IBaseResult<ImageDto>> UploadImage(ImageUploadRequest model, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves information about an image based on the specified file name.
        /// </summary>
        /// <remarks>The method performs an asynchronous operation to retrieve metadata or other details
        /// about the image associated with the specified file name. Ensure that the file name corresponds to a valid
        /// image file accessible to the application.</remarks>
        /// <param name="fileName">The name of the file for which to retrieve image information. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that wraps an <see cref="ImageDto"/> containing the image information.</returns>
        Task<IBaseResult<FileInfoResponse>> GetInfoAsync(string fileName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an image with the specified identifier.
        /// </summary>
        /// <param name="imageId">The unique identifier of the image to delete. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the delete operation.</returns>
        Task<IBaseResult> DeleteImageAsync(string imageId, CancellationToken cancellationToken = default);
    }
}
