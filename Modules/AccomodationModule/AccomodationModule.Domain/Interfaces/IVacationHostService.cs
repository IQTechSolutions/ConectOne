using AccomodationModule.Domain.DataTransferObjects;
using ConectOne.Domain.RequestFeatures;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.RequestFeatures;

namespace AccomodationModule.Domain.Interfaces
{
    /// <summary>
    /// Defines a service for managing vacation hosts, including operations for retrieving, creating, updating, and
    /// deleting vacation host records.
    /// </summary>
    /// <remarks>This interface provides methods to interact with vacation host data, including paginated
    /// retrieval, lookup by ID or name, and CRUD operations. Implementations of this service are expected to handle
    /// data persistence and validation.</remarks>
    public interface IVacationHostService
    {
        /// <summary>
        /// Retrieves a paginated list of vacation hosts based on the specified request parameters.
        /// </summary>
        /// <remarks>Use this method to retrieve vacation host data in a paginated format, which is useful
        /// for scenarios where the dataset is large and needs to be processed in smaller chunks. Ensure that the
        /// <paramref name="pageParameters"/> are correctly configured to avoid unexpected results.</remarks>
        /// <param name="pageParameters">The parameters that define the pagination and filtering criteria for the request.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing the paginated list of vacation hosts.</returns>
        Task<PaginatedResult<VacationHostDto>> PagedVacationHostsAsync(RequestParameters pageParameters, CancellationToken cancellationToken = default);

        Task<IBaseResult<IEnumerable<VacationHostDto>>> AllVacationHostsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves vacation host details asynchronously based on the specified host ID.
        /// </summary>
        /// <param name="vacationHostId">The unique identifier of the vacation host to retrieve. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional; defaults to <see langword="default"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="IBaseResult{VacationHostDto}"/> object with the details of the vacation host.</returns>
        Task<IBaseResult<VacationHostDto>> VacationHostAsync(string vacationHostId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves information about a vacation host based on the provided name.
        /// </summary>
        /// <remarks>Use this method to retrieve vacation host details asynchronously. Ensure the provided
        /// name is valid and non-empty. The result will include relevant information about the host, or indicate that
        /// no match was found.</remarks>
        /// <param name="vacationHostName">The name of the vacation host to search for. This parameter cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="IBaseResult{VacationHostDto}"/> object with details about the vacation host, or an empty result if no
        /// matching host is found.</returns>
        Task<IBaseResult<VacationHostDto>> VacationHostFromNameAsync(string vacationHostName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new vacation host asynchronously based on the provided data transfer object (DTO).
        /// </summary>
        /// <remarks>Use this method to create a vacation host entity in the system. Ensure that the
        /// <paramref name="dto"/>  contains valid and complete data before calling this method. The operation may be
        /// canceled using the  <paramref name="cancellationToken"/>.</remarks>
        /// <param name="dto">The <see cref="VacationHostDto"/> containing the details of the vacation host to be created. This parameter
        /// cannot be null.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation. Defaults to <see
        /// langword="default"/> if not provided.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the creation process.</returns>
        Task<IBaseResult> CreateAsync(VacationHostDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the vacation host information asynchronously.
        /// </summary>
        /// <remarks>Use this method to update vacation host details in the system. Ensure that the
        /// <paramref name="dto"/>  contains valid data before calling this method. If the operation is canceled via the
        /// <paramref name="cancellationToken"/>, the task will complete with a canceled state.</remarks>
        /// <param name="dto">The data transfer object containing the updated vacation host details. This parameter cannot be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional; defaults to <see langword="default"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the update operation.</returns>
        Task<IBaseResult> UpdateAsync(VacationHostDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes an entity with the specified identifier asynchronously.
        /// </summary>
        /// <remarks>Use this method to remove an entity by its identifier. Ensure that the <paramref
        /// name="id"/> corresponds to a valid entity. The operation may fail if the entity does not exist or if there
        /// are constraints preventing its removal.</remarks>
        /// <param name="id">The unique identifier of the entity to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. Defaults to <see cref="CancellationToken.None"/> if not
        /// provided.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the removal operation.</returns>
        Task<IBaseResult> RemoveAsync(string id, CancellationToken cancellationToken = default);

        #region Images

        /// <summary>
        /// Adds a new image to the entity as specified in the request.
        /// </summary>
        /// <param name="request">An object containing the details of the image to add, including the target entity and image data. Cannot be
        /// null.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the add operation.</returns>
        Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes the image with the specified identifier from the system asynchronously.
        /// </summary>
        /// <param name="imageId">The unique identifier of the image to remove. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous remove operation. The task result contains an <see
        /// cref="IBaseResult"/> indicating the outcome of the operation.</returns>
        Task<IBaseResult> RemoveImage(string imageId, CancellationToken cancellationToken = default);

        #endregion
    }
}
