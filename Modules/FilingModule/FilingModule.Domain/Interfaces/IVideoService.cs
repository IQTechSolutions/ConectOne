using ConectOne.Domain.Interfaces;
using ConectOne.Domain.RequestFeatures;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.DataTransferObjects;

namespace FilingModule.Domain.Interfaces
{
    /// <summary>
    /// Defines a contract for managing and retrieving video-related data.
    /// </summary>
    /// <remarks>This interface provides methods for retrieving video data, including paginated results, all
    /// videos matching specific criteria, and individual video details. Implementations of this interface are expected
    /// to handle data access and business logic related to video entities.</remarks>
    /// <typeparam name="TEntity">The type of the entity that the service operates on. Must implement <see cref="IAuditableEntity"/>.</typeparam>
    public interface IVideoService<TEntity> where TEntity : IAuditableEntity
    {
        /// <summary>
        /// Retrieves a paginated list of videos based on the specified request parameters.
        /// </summary>
        /// <remarks>The <paramref name="parameters"/> object must specify valid pagination settings, such
        /// as page number and page size. If no videos match the specified criteria, the result will contain an empty
        /// collection.</remarks>
        /// <param name="parameters">The parameters that define the pagination and filtering options for the request.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a collection of <see cref="VideoDto"/> objects and pagination
        /// metadata.</returns>
        Task<PaginatedResult<VideoDto>> PagedVideosAsync(RequestParameters parameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a collection of all videos based on the specified request parameters.
        /// </summary>
        /// <remarks>The <paramref name="parameters"/> argument allows customization of the query, such as
        /// specifying filters, sorting options, and pagination details. If no parameters are provided, the method
        /// returns all available videos.</remarks>
        /// <param name="parameters">The parameters used to filter, sort, and paginate the video collection.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// containing an enumerable collection of <see cref="VideoDto"/> objects.</returns>
        Task<IBaseResult<IEnumerable<VideoDto>>> AllVideosAsync(RequestParameters parameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves video details associated with the specified document ID.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch video details. Ensure that
        /// the provided <paramref name="documentId"/> is valid and corresponds to an existing document. The operation
        /// can be canceled by passing a cancellation token.</remarks>
        /// <param name="documentId">The unique identifier of the document for which video details are requested. Cannot be <see
        /// langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// of type <see cref="VideoDto"/> representing the video details. The result may indicate success or failure
        /// depending on the operation outcome.</returns>
        Task<IBaseResult<VideoDto>> VideoAsync(string documentId, CancellationToken cancellationToken = default);
    }
}
