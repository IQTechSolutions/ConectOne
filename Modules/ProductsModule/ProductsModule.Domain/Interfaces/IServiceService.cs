using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.RequestFeatures;
using ProductsModule.Domain.DataTransferObjects;
using ProductsModule.Domain.RequestFeatures;

namespace ProductsModule.Domain.Interfaces
{
    /// <summary>
    /// Defines a contract for managing services, including operations for retrieving, creating, updating, and deleting
    /// services, as well as managing associated images.
    /// </summary>
    /// <remarks>This interface provides methods for working with services in a paginated and filtered manner,
    /// retrieving details of specific services, and performing CRUD operations. It also includes functionality for
    /// managing images associated with entities. Many methods support asynchronous execution and cancellation through
    /// <see cref="CancellationToken"/>.</remarks>
    public interface IServiceService
    {
        /// <summary>
        /// Retrieves a paginated list of services based on the specified parameters.
        /// </summary>
        /// <remarks>The method supports pagination and filtering based on the provided <paramref
        /// name="serviceParameters"/>. If <paramref name="trackChanges"/> is set to <see langword="true"/>, the
        /// retrieved entities will be tracked  by the underlying data context, which may impact performance in
        /// high-load scenarios.</remarks>
        /// <param name="serviceParameters">The parameters used to filter and sort the services.</param>
        /// <param name="trackChanges">A value indicating whether to track changes to the retrieved entities.  Set to <see langword="true"/> to
        /// enable change tracking; otherwise, <see langword="false"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a collection of <see cref="ServiceDto"/> objects  that match
        /// the specified parameters, along with pagination metadata.</returns>
        Task<PaginatedResult<ServiceDto>> PagedServicesAsync(ServiceParameters serviceParameters, bool trackChanges = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a paginated list of all services based on the specified parameters.
        /// </summary>
        /// <remarks>The method supports pagination and filtering through the <paramref
        /// name="pageParameters"/> parameter.  If <paramref name="trackChanges"/> is set to <see langword="true"/>, the
        /// retrieved entities will be tracked  for changes, which may have performance implications in certain
        /// scenarios.</remarks>
        /// <param name="pageParameters">The parameters used to define pagination and filtering options for the services.</param>
        /// <param name="trackChanges">A boolean value indicating whether to track changes to the retrieved entities.  Set to <see
        /// langword="true"/> to enable change tracking; otherwise, <see langword="false"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// containing an enumerable collection of <see cref="ServiceDto"/> objects.</returns>
        Task<IBaseResult<IEnumerable<ServiceDto>>> AllServicesAsync(ServiceParameters pageParameters, bool trackChanges = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a collection of services associated with the specified category.
        /// </summary>
        /// <remarks>The method performs an asynchronous operation to fetch services for the given
        /// category. Ensure that the <paramref name="categoryId"/> corresponds to a valid category in the
        /// system.</remarks>
        /// <param name="categoryId">The unique identifier of the category whose services are to be retrieved. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with an enumerable collection of <see cref="ServiceDto"/> objects representing the services in the specified
        /// category.</returns>
        Task<IBaseResult<IEnumerable<ServiceDto>>> CategoryServicesAsync(string categoryId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the details of a service based on the specified service ID.
        /// </summary>
        /// <remarks>Use this method to retrieve service details asynchronously. The <paramref
        /// name="trackChanges"/> parameter  determines whether the retrieved entity is tracked for changes, which may
        /// impact performance in certain scenarios.</remarks>
        /// <param name="serviceId">The unique identifier of the service to retrieve. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="trackChanges">A value indicating whether to track changes to the retrieved service entity.  If <see langword="true"/>,
        /// changes to the entity will be tracked; otherwise, they will not.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// object wrapping the <see cref="ServiceDto"/> for the requested service.</returns>
        Task<IBaseResult<ServiceDto>> ServiceAsync(string serviceId, bool trackChanges = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing service with the provided data.
        /// </summary>
        /// <remarks>This method performs an update operation on an existing service. Ensure that the
        /// provided <paramref name="service"/> contains valid data and corresponds to an existing service record. The
        /// operation may be canceled by passing a cancellation token.</remarks>
        /// <param name="service">The data transfer object containing the updated service information. Cannot be <see langword="null"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that includes the updated <see cref="ServiceDto"/> and the operation's status.</returns>
        Task<IBaseResult<ServiceDto>> UpdateAsync(ServiceDto service, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new service based on the provided data.
        /// </summary>
        /// <remarks>The operation may fail if the provided service data is invalid or if a service with
        /// the same identifier already exists. Ensure that the <paramref name="service"/> object contains all required
        /// fields before calling this method.</remarks>
        /// <param name="service">The data transfer object containing the details of the service to create. Cannot be <see langword="null"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that includes the created <see cref="ServiceDto"/> and the operation's outcome.</returns>
        Task<IBaseResult<ServiceDto>> CreateAsync(ServiceDto service, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the product with the specified identifier asynchronously.
        /// </summary>
        /// <param name="productId">The unique identifier of the product to delete. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the delete operation.</returns>
        Task<IBaseResult> DeleteAsync(string productId, CancellationToken cancellationToken = default);

        #region Images

        /// <summary>
        /// Adds an image to the specified entity.
        /// </summary>
        /// <param name="request">The request containing the details of the image to add, including the entity identifier and image data.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes an image with the specified identifier.
        /// </summary>
        /// <param name="imageId">The unique identifier of the image to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        Task<IBaseResult> RemoveImage(string imageId, CancellationToken cancellationToken = default);

        #endregion

        #region Videos

        /// <summary>
        /// Adds an image to the specified entity.
        /// </summary>
        /// <remarks>This method creates a new image entity and attempts to add it to the repository. It
        /// then saves the changes to the repository. If the operation fails at any step, the method returns a failure
        /// result with the associated error messages.</remarks>
        /// <param name="request">The request containing the image details and the entity to which the image will be added.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
        Task<IBaseResult> AddVideo(AddEntityVideoRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a video identified by the specified video ID.
        /// </summary>
        /// <remarks>The operation may fail if the video does not exist or if the user does not have sufficient
        /// permissions to remove it.  Check the returned <see cref="IBaseResult"/> for details about the operation's
        /// success or failure.</remarks>
        /// <param name="videoId">The unique identifier of the video to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        Task<IBaseResult> RemoveVideo(string videoId, CancellationToken cancellationToken = default);

        #endregion
    }
}
