using ConectOne.Domain.Extensions;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.RequestFeatures;
using ProductsModule.Domain.DataTransferObjects;
using ProductsModule.Domain.Interfaces;
using ProductsModule.Domain.RequestFeatures;

namespace ProductsModule.Application.RestServices
{
    /// <summary>
    /// Provides methods for managing services, including retrieving, creating, updating, and deleting services,  as
    /// well as managing associated images.
    /// </summary>
    /// <remarks>This class acts as a REST-based service layer for interacting with service-related data. It
    /// supports  operations such as retrieving paginated or filtered lists of services, managing service details, and 
    /// handling associated images. All methods are asynchronous and rely on an <see cref="IBaseHttpProvider"/>  for
    /// HTTP communication.</remarks>
    /// <param name="provider"></param>
    public class ServiceRestService(IBaseHttpProvider provider) : IServiceService
    {
        /// <summary>
        /// Retrieves a paginated list of services based on the specified parameters.
        /// </summary>
        /// <remarks>The method fetches data from the "services" endpoint and applies the specified
        /// pagination and filtering criteria.</remarks>
        /// <param name="serviceParameters">The parameters used to filter and paginate the services.</param>
        /// <param name="trackChanges">A value indicating whether to track changes to the retrieved entities.  Set to <see langword="true"/> to
        /// enable change tracking; otherwise, <see langword="false"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing the paginated list of services.</returns>
        public async Task<PaginatedResult<ServiceDto>> PagedServicesAsync(ServiceParameters serviceParameters, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetPagedAsync<ServiceDto, ServiceParameters>("services", serviceParameters);
            return result;
        }

        /// <summary>
        /// Retrieves a collection of all services based on the specified parameters.
        /// </summary>
        /// <remarks>The method sends a request to retrieve all services, applying the specified
        /// pagination and filtering parameters. The <paramref name="trackChanges"/> parameter does not affect the data
        /// retrieval process but may influence how the data is tracked in the underlying data context.</remarks>
        /// <param name="pageParameters">The parameters used to control pagination and filtering of the service collection.</param>
        /// <param name="trackChanges">A value indicating whether to track changes to the retrieved entities.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// containing an enumerable collection of <see cref="ServiceDto"/> objects.</returns>
        public async Task<IBaseResult<IEnumerable<ServiceDto>>> AllServicesAsync(ServiceParameters pageParameters, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<ServiceDto>>($"services/all?{pageParameters.GetQueryString()}");
            return result;
        }

        /// <summary>
        /// Retrieves a collection of services associated with the specified category.
        /// </summary>
        /// <remarks>This method sends a request to retrieve services for the specified category. The
        /// caller can use the <paramref name="cancellationToken"/> to cancel the operation if needed.</remarks>
        /// <param name="categoryId">The unique identifier of the category for which to retrieve services. Cannot be <see langword="null"/> or
        /// empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// containing an enumerable collection of <see cref="ServiceDto"/> objects associated with the specified
        /// category.</returns>
        public async Task<IBaseResult<IEnumerable<ServiceDto>>> CategoryServicesAsync(string categoryId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<ServiceDto>>($"services/category/{categoryId}");
            return result;
        }

        /// <summary>
        /// Retrieves the details of a service by its identifier.
        /// </summary>
        /// <remarks>This method retrieves the service details from the underlying provider using the
        /// specified service identifier. The caller can optionally track changes to the retrieved service by setting
        /// <paramref name="trackChanges"/> to <see langword="true"/>.</remarks>
        /// <param name="serviceId">The unique identifier of the service to retrieve. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="trackChanges">A value indicating whether to track changes to the retrieved service.  If <see langword="true"/>, changes to
        /// the service may be tracked; otherwise, they are not.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// object that wraps the <see cref="ServiceDto"/> representing the service details.</returns>
        public async Task<IBaseResult<ServiceDto>> ServiceAsync(string serviceId, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<ServiceDto>($"services/{serviceId}");
            return result;
        }

        /// <summary>
        /// Updates an existing service asynchronously.
        /// </summary>
        /// <param name="service">The <see cref="ServiceDto"/> object containing the updated service details.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation. The default value is <see
        /// cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult{T}"/>
        /// of type <see cref="ServiceDto"/> representing the updated service.</returns>
        public async Task<IBaseResult<ServiceDto>> UpdateAsync(ServiceDto service, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<ServiceDto, ServiceDto>($"services", service);
            return result;
        }

        /// <summary>
        /// Creates a new service asynchronously.
        /// </summary>
        /// <param name="service">The <see cref="ServiceDto"/> object representing the service to be created.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult{T}"/>
        /// of type <see cref="ServiceDto"/> representing the created service.</returns>
        public async Task<IBaseResult<ServiceDto>> CreateAsync(ServiceDto service, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<ServiceDto, ServiceDto>($"services", service);
            return result;
        }

        /// <summary>
        /// Deletes a product with the specified identifier asynchronously.
        /// </summary>
        /// <param name="productId">The unique identifier of the product to delete. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous delete operation. The task result contains an <see
        /// cref="IBaseResult"/>  indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> DeleteAsync(string productId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"services", productId);
            return result;
        }

        /// <summary>
        /// Adds an image to the specified entity using the provided request data.
        /// </summary>
        /// <remarks>This method sends the image data and associated entity details to the service
        /// endpoint for processing. Ensure that the request object contains valid data as required by the
        /// service.</remarks>
        /// <param name="request">The request containing the image data and associated entity details. Cannot be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the outcome of the add image
        /// operation.</returns>
        public async Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"services/addImage", request);
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
            var result = await provider.DeleteAsync($"services/addImage", imageId);
            return result;
        }

        /// <summary>
        /// Adds a video to the business directory using the specified request data.
        /// </summary>
        /// <remarks>This method sends a POST request to the "businessdirectory/addVideo" endpoint with
        /// the provided video data. Ensure that the <paramref name="request"/> object contains all required fields
        /// before calling this method.</remarks>
        /// <param name="request">The request object containing the video details to be added.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> AddVideo(AddEntityVideoRequest request, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"services/addVideo", request);
            return result;
        }

        /// <summary>
        /// Removes a video with the specified identifier.
        /// </summary>
        /// <remarks>This method sends a request to delete the video identified by <paramref
        /// name="videoId"/>. Ensure the identifier is valid and corresponds to an existing video.</remarks>
        /// <param name="videoId">The unique identifier of the video to be removed. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveVideo(string videoId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"services/deleteVideo", videoId);
            return result;
        }
    }
}
