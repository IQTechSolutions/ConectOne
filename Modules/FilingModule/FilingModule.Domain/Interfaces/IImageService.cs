using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.DataTransferObjects;
using FilingModule.Domain.RequestFeatures;

namespace FilingModule.Domain.Interfaces
{
    /// <summary>
    /// Defines a contract for managing and retrieving image data, including paginated results, all images, and specific
    /// image details.
    /// </summary>
    /// <remarks>This interface provides methods for retrieving image data in various formats, such as
    /// paginated results, all available images, or a specific image by its identifier. It is designed to work with
    /// entities that implement the <see cref="IAuditableEntity"/> interface.</remarks>
    /// <typeparam name="TEntity">The type of the entity associated with the image service. Must implement <see cref="IAuditableEntity"/>.</typeparam>
    public interface IImageService<TEntity> where TEntity : IAuditableEntity
    {
        /// <summary>
        /// Retrieves a paginated list of images based on the specified parameters.
        /// </summary>
        /// <remarks>The method supports pagination and optional filtering based on the provided <paramref
        /// name="parameters"/>. If no images match the criteria, the result will contain an empty collection.</remarks>
        /// <param name="parameters">The pagination and filtering parameters used to retrieve the images.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a collection of <see cref="ImageDto"/> objects and pagination
        /// metadata.</returns>
        Task<PaginatedResult<ImageDto>> PagedImagesAsync(ImagePageParameters parameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a paginated list of images based on the specified parameters.
        /// </summary>
        /// <remarks>The method supports pagination and filtering through the <paramref
        /// name="parameters"/> argument. If no images match the specified criteria, the result will contain an empty
        /// collection.</remarks>
        /// <param name="parameters">The pagination and filtering parameters to apply when retrieving the images.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with an enumerable collection of <see cref="ImageDto"/> objects representing the images.</returns>
        Task<IBaseResult<IEnumerable<ImageDto>>> AllImagesAsync(ImagePageParameters parameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves image data associated with the specified document.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch image data. Ensure that the
        /// provided <paramref name="documentId"/> corresponds to a valid document in the system. The operation may be
        /// canceled by passing a cancellation token.</remarks>
        /// <param name="documentId">The unique identifier of the document for which the image data is requested. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that wraps the image data as an <see cref="ImageDto"/>. If the document is not found, the result may
        /// indicate an error.</returns>
        Task<IBaseResult<ImageDto>> ImageAsync(string documentId, CancellationToken cancellationToken = default);
    }
}
