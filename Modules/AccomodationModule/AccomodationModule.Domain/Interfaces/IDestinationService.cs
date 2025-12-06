using AccomodationModule.Domain.Arguments;
using AccomodationModule.Domain.DataTransferObjects;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.RequestFeatures;

namespace AccomodationModule.Domain.Interfaces
{
    /// <summary>
    /// Interface for managing destination-related operations.
    /// </summary>
    public interface IDestinationService
    {
        /// <summary>
        /// Retrieves a paginated list of destinations based on the specified page parameters.
        /// </summary>
        /// <remarks>Use this method to fetch a subset of destinations in a paginated format. Ensure that
        /// the <paramref name="pageParameters"/> specify valid values for pagination, such as a positive page number
        /// and page size.</remarks>
        /// <param name="pageParameters">The parameters that define the pagination settings, such as page number and page size.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a collection of <see cref="DestinationDto"/> objects for the
        /// requested page.</returns>
        Task<PaginatedResult<DestinationDto>> PagedDestinationsAsync(DestinationPageParameters pageParameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all destinations.
        /// </summary>
        Task<IBaseResult<IEnumerable<DestinationDto>>> AllDestinationsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the details of a destination based on the specified destination ID.
        /// </summary>
        /// <remarks>Use this method to asynchronously fetch information about a specific destination.
        /// Ensure that the provided <paramref name="destinationId"/> is valid and corresponds to an existing
        /// destination.</remarks>
        /// <param name="destinationId">The unique identifier of the destination to retrieve. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping a <see cref="DestinationDto"/> with the destination details, or an error result if the
        /// operation fails.</returns>
        Task<IBaseResult<DestinationDto>> DestinationAsync(string destinationId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new destination based on the provided data transfer object (DTO).
        /// </summary>
        /// <remarks>The operation may involve validation of the provided DTO and other business logic.
        /// Ensure that the DTO contains valid data before calling this method.</remarks>
        /// <param name="dto">The data transfer object containing the details of the destination to create. Cannot be <see
        /// langword="null"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the creation operation.</returns>
        Task<IBaseResult> CreateAsync(DestinationDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the destination with the specified data asynchronously.
        /// </summary>
        /// <remarks>This method performs an asynchronous update operation. Ensure that the provided
        /// <paramref name="dto"/> contains valid data before calling this method. The operation may be canceled by
        /// passing a <see cref="CancellationToken"/> that is signaled.</remarks>
        /// <param name="dto">The data transfer object containing the updated destination information.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        Task<IBaseResult> UpdateAsync(DestinationDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes the specified destination asynchronously.
        /// </summary>
        /// <param name="destinationId">The unique identifier of the destination to remove. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the removal operation.</returns>
        Task<IBaseResult> RemoveAsync(string destinationId, CancellationToken cancellationToken = default);

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
