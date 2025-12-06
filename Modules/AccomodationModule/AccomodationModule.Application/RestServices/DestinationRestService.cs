using AccomodationModule.Domain.Arguments;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.RequestFeatures;

namespace AccomodationModule.Application.RestServices
{
    /// <summary>
    /// Provides methods for managing destinations, including retrieving, creating, updating, and deleting destinations,
    /// as well as managing associated media such as images and videos.
    /// </summary>
    /// <remarks>This service interacts with a REST API to perform operations on destination data. It supports
    /// paginated retrieval  of destinations, fetching all destinations, and retrieving a specific destination by its
    /// identifier. Additionally,  it provides functionality to add or remove images and videos associated with
    /// destinations.</remarks>
    /// <param name="provider"></param>
    public class DestinationRestService(IBaseHttpProvider provider) : IDestinationService
    {
        /// <summary>
        /// Retrieves a paginated list of destinations based on the specified page parameters.
        /// </summary>
        /// <remarks>This method queries the "destinations" endpoint to retrieve the paginated data.
        /// Ensure that the <paramref name="pageParameters"/> are valid to avoid unexpected results.</remarks>
        /// <param name="pageParameters">The parameters that define the pagination settings, such as page number and page size.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a collection of <see cref="DestinationDto"/> objects and
        /// pagination metadata.</returns>
        public async Task<PaginatedResult<DestinationDto>> PagedDestinationsAsync(DestinationPageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetPagedAsync<DestinationDto, DestinationPageParameters>("destinations", pageParameters);
            return result;
        }

        /// <summary>
        /// Retrieves a collection of all available destinations.
        /// </summary>
        /// <remarks>This method fetches the data from the underlying provider and returns the result. If
        /// no destinations are available, the enumerable collection in the result may be empty.</remarks>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// containing an enumerable collection of <see cref="DestinationDto"/> objects representing the destinations.</returns>
        public async Task<IBaseResult<IEnumerable<DestinationDto>>> AllDestinationsAsync(CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<DestinationDto>>("destinations/all");
            return result;
        }

        /// <summary>
        /// Retrieves the destination details for the specified destination ID.
        /// </summary>
        /// <remarks>This method sends a request to retrieve the destination details from the provider. 
        /// Ensure that the destinationId is valid and not empty.</remarks>
        /// <param name="destinationId">The unique identifier of the destination to retrieve.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  IBaseResult{T} of type
        /// DestinationDto with the destination details.</returns>
        public async Task<IBaseResult<DestinationDto>> DestinationAsync(string destinationId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<DestinationDto>($"destinations/{destinationId}");
            return result;
        }

        /// <summary>
        /// Creates a new destination asynchronously.
        /// </summary>
        /// <remarks>This method sends the provided destination details to the underlying provider for
        /// creation. Ensure that the <paramref name="dto"/> contains valid data before calling this method.</remarks>
        /// <param name="dto">The data transfer object containing the details of the destination to create.</param>
        /// <param name="cancellationToken">An optional token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// representing the outcome of the creation operation.</returns>
        public async Task<IBaseResult> CreateAsync(DestinationDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync($"destinations", dto);
            return result;
        }

        /// <summary>
        /// Updates the destination information asynchronously.
        /// </summary>
        /// <remarks>This method sends the provided destination details to the underlying provider for
        /// updating. Ensure that the <paramref name="dto"/> contains valid data before calling this method.</remarks>
        /// <param name="dto">The data transfer object containing the destination details to be updated.</param>
        /// <param name="cancellationToken">An optional token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateAsync(DestinationDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"destinations", dto);
            return result;
        }

        /// <summary>
        /// Removes the destination with the specified identifier asynchronously.
        /// </summary>
        /// <remarks>This method sends a delete request to the underlying provider to remove the specified
        /// destination. Ensure that the <paramref name="destinationId"/> corresponds to an existing
        /// destination.</remarks>
        /// <param name="destinationId">The unique identifier of the destination to remove.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the removal operation.</returns>
        public async Task<IBaseResult> RemoveAsync(string destinationId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"destinations", destinationId);
            return result;
        }

        /// <summary>
        /// Adds an image to the specified entity based on the provided request.
        /// </summary>
        /// <remarks>This method sends a request to add an image to an entity. Ensure that the <paramref
        /// name="request"/> contains valid data, including the entity identifier and image details, before calling this
        /// method.</remarks>
        /// <param name="request">The request containing the details of the image to be added, including the entity identifier and image data.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"destinations/addImage", request);
            return result;
        }

        /// <summary>
        /// Removes an image with the specified identifier.
        /// </summary>
        /// <param name="imageId">The unique identifier of the image to be removed. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the removal operation.</returns>
        public async Task<IBaseResult> RemoveImage(string imageId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"destinations/deleteImage", imageId);
            return result;
        }

        /// <summary>
        /// Adds a video to the specified destination.
        /// </summary>
        /// <remarks>The <paramref name="request"/> parameter must contain all required video details.
        /// Ensure that the destination specified in the request is valid and accessible.</remarks>
        /// <param name="request">The request containing the video details to be added.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> AddVideo(AddEntityVideoRequest request, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"destinations/addVideo", request);
            return result;
        }

        /// <summary>
        /// Removes a video with the specified identifier.
        /// </summary>
        /// <remarks>This method sends a request to remove the video identified by <paramref
        /// name="videoId"/>.  Ensure that the provided identifier corresponds to an existing video. The operation may
        /// fail if the video does not exist or if there are network issues.</remarks>
        /// <param name="videoId">The unique identifier of the video to be removed. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveVideo(string videoId, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"destinations/deleteVideo", videoId);
            return result;
        }
    }
}
