using AccomodationModule.Domain.DataTransferObjects;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Domain.Interfaces
{
    /// <summary>
    /// Defines a contract for managing area-related operations, including retrieval, creation, updating, and deletion
    /// of areas.
    /// </summary>
    /// <remarks>This service provides asynchronous methods for interacting with area data. Each method
    /// returns a result wrapped in  <see cref="IBaseResult"/> to encapsulate the operation's outcome, including
    /// success or failure details.</remarks>
    public interface IAreaService
    {
        /// <summary>
        /// Retrieves all available areas asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. The default value is <see langword="default"/>,  which
        /// indicates that no cancellation is requested.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// with a collection of <see cref="AreaDto"/> objects representing the available areas.</returns>
        Task<IBaseResult<IEnumerable<AreaDto>>> AllAreasAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves detailed information about a specific area asynchronously.
        /// </summary>
        /// <remarks>Use this method to fetch detailed information about an area, such as its name,
        /// description, and other metadata. Ensure that the <paramref name="areaId"/> is valid and corresponds to an
        /// existing area.</remarks>
        /// <param name="areaId">The unique identifier of the area to retrieve. This parameter cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping the <see cref="AreaDto"/> for the specified area.</returns>
        Task<IBaseResult<AreaDto>> AreaAsync(string areaId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new area asynchronously based on the provided model.
        /// </summary>
        /// <remarks>This method performs validation on the provided model and creates a new area in the
        /// system. Ensure that the <paramref name="model"/> contains valid data before calling this method.</remarks>
        /// <param name="model">The data transfer object containing the details of the area to be created. This parameter cannot be null.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// object with the created <see cref="AreaDto"/> if the operation is successful.</returns>
        Task<IBaseResult<AreaDto>> CreateAreaAsync(AreaDto model, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the specified area with new information.
        /// </summary>
        /// <remarks>Use this method to update an existing area with new details. Ensure that the
        /// <paramref name="areaDto"/> contains valid data before calling this method.</remarks>
        /// <param name="areaDto">The data transfer object containing the updated information for the area. Cannot be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the updated <see cref="AreaDto"/> if the operation succeeds.</returns>
        Task<IBaseResult<AreaDto>> UpdateAreaAsync(AreaDto areaDto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes an area identified by the specified ID asynchronously.
        /// </summary>
        /// <remarks>Use this method to remove an area from the system. Ensure that the <paramref
        /// name="areaId"/> corresponds to a valid area. The operation may fail if the area does not exist or if there
        /// are constraints preventing its removal.</remarks>
        /// <param name="areaId">The unique identifier of the area to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
        Task<IBaseResult> RemoveAreaAsync(string areaId, CancellationToken cancellationToken = default);
    }
}
