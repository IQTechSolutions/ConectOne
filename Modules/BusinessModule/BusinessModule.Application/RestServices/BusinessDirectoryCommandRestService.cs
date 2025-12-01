using BusinessModule.Domain.DataTransferObjects;
using BusinessModule.Domain.Interfaces;
using BusinessModule.Domain.RequestFeatures;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.RequestFeatures;

namespace BusinessModule.Application.RestServices
{
    /// <summary>
    /// Provides a REST-based implementation of the <see cref="IBusinessDirectoryCommandService"/> interface,  enabling
    /// the management of business directory listings, services, products, and associated images.
    /// </summary>
    /// <remarks>This service allows clients to perform various operations on business directory entities,
    /// such as creating,  updating, approving, rejecting, and deleting listings, as well as managing associated
    /// services, products,  and images. Each operation communicates with a REST API via the provided <see
    /// cref="IBaseHttpProvider"/>.</remarks>
    /// <param name="provider"></param>
    public class BusinessDirectoryCommandRestService(IBaseHttpProvider provider) : IBusinessDirectoryCommandService 
    {
        /// <summary>
        /// Creates a new business listing asynchronously.
        /// </summary>
        /// <remarks>This method sends the provided business listing data to the underlying provider for
        /// creation. Ensure that the <paramref name="dto"/> contains valid data before calling this method.</remarks>
        /// <param name="dto">The data transfer object containing the details of the business listing to create.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// object that includes the created <see cref="BusinessListingDto"/> if the operation is successful.</returns>
        public async Task<IBaseResult<BusinessListingDto>> CreateAsync(BusinessListingDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<BusinessListingDto, BusinessListingDto>("businessdirectory", dto);
            return result;
        }

        /// <summary>
        /// Sends an enquiry message to the owner of a business listing.
        /// </summary>
        /// <param name="request">The enquiry payload that should be forwarded to the owner.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the request.</param>
        /// <returns>A result describing whether the enquiry request was accepted.</returns>
        public async Task<IBaseResult> ContactListingOwnerAsync(ListingContactRequest request, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync("businessdirectory/contact", request);
            return result;
        }

        /// <summary>
        /// Updates an existing business listing asynchronously.
        /// </summary>
        /// <remarks>This method sends the updated business listing details to the server. Ensure that the
        /// <see cref="BusinessListingDto.Id"/> property is correctly set to the identifier of the listing to be
        /// updated.</remarks>
        /// <param name="dto">The data transfer object containing the updated details of the business listing. The <see
        /// cref="BusinessListingDto.Id"/> property must be set to identify the listing to update.</param>
        /// <param name="cancellationToken">An optional token to cancel the operation. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateAsync(BusinessListingDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"businessdirectory/{dto.Id}", dto);
            return result;
        }

        /// <summary>
        /// Renews the specified business listing for another active period.
        /// </summary>
        /// <param name="listingId">The unique identifier of the listing to renew.</param>
        /// <param name="cancellationToken">A token used to cancel the operation.</param>
        /// <returns>The updated listing details.</returns>
        public async Task<IBaseResult<BusinessListingDto>> RenewAsync(string listingId, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<BusinessListingDto>($"businessdirectory/{listingId}/renew");
            return result;
        }

        /// <summary>
        /// Removes a listing with the specified identifier asynchronously.
        /// </summary>
        /// <remarks>This method sends a delete request to the "businessdirectory" endpoint to remove the
        /// specified listing. Ensure that the <paramref name="listingId"/> corresponds to an existing
        /// listing.</remarks>
        /// <param name="listingId">The unique identifier of the listing to be removed. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the removal operation.</returns>
        public async Task<IBaseResult> RemoveAsync(string listingId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync("businessdirectory", listingId);
            return result;
        }

        /// <summary>
        /// Approves a business directory listing with the specified identifier.
        /// </summary>
        /// <remarks>This method sends an approval request for the specified listing to the underlying
        /// provider.  Ensure the <paramref name="listingId"/> is valid and corresponds to an existing
        /// listing.</remarks>
        /// <param name="listingId">The unique identifier of the listing to approve. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the approval operation.</returns>
        public async Task<IBaseResult> ApproveAsync(string listingId, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"businessdirectory/approve/{listingId}");
            return result;
        }

        /// <summary>
        /// Rejects a business directory listing with the specified identifier.
        /// </summary>
        /// <remarks>This method sends a request to reject the specified listing. Ensure that the
        /// <paramref name="listingId"/> is valid and corresponds to an existing listing. The operation may fail if the
        /// listing does not exist or if the user lacks sufficient permissions.</remarks>
        /// <param name="listingId">The unique identifier of the listing to reject.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the rejection operation.</returns>
        public async Task<IBaseResult> RejectAsync(string listingId, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"businessdirectory/reject/{listingId}");
            return result;
        }

        /// <summary>
        /// Adds an image to an entity in the business directory.
        /// </summary>
        /// <remarks>The <paramref name="request"/> parameter must include valid image data and entity
        /// information.  Ensure that the entity exists in the business directory before calling this method.</remarks>
        /// <param name="request">The request containing the image data and associated entity details.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"businessdirectory/addImage", request);
            return result;
        }

        /// <summary>
        /// Removes an image with the specified identifier.
        /// </summary>
        /// <param name="imageId">The unique identifier of the image to be removed. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveImage(string imageId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"businessdirectory/deleteImage", imageId);
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
            var result = await provider.PostAsync($"businessdirectory/addVideo", request);
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
            var result = await provider.DeleteAsync($"businessdirectory/deleteVideo", videoId);
            return result;
        }

        /// <summary>
        /// Adds a new listing service using the provided data transfer object (DTO).
        /// </summary>
        /// <remarks>This method sends a POST request to add a new listing service to the business directory.
        /// Ensure that the <paramref name="dto"/> contains valid data before calling this method.</remarks>
        /// <param name="dto">The <see cref="ListingServiceDto"/> containing the details of the listing service to be added.</param>
        /// <param name="cancellationToken">An optional <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        /// <returns>An <see cref="IBaseResult"/> representing the result of the operation. The result indicates whether the
        /// addition was successful.</returns>
        public async Task<IBaseResult> AddListingService(ListingServiceDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"businessdirectory/listingServices", dto);
            return result;
        }

        /// <summary>
        /// Updates a listing service with the specified details.
        /// </summary>
        /// <remarks>The <paramref name="dto"/> parameter must contain valid data for the listing service
        /// update to succeed. If the operation is canceled via the <paramref name="cancellationToken"/>, the task will
        /// be marked as canceled.</remarks>
        /// <param name="dto">The data transfer object containing the details of the listing service to update.</param>
        /// <param name="cancellationToken">An optional token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateListingService(ListingServiceDto dto, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(dto.Id))
            {
                return await Result.FailAsync("A listing service identifier is required to update the record.");
            }

            var result = await provider.PutAsync($"businessdirectory/listingServices/{dto.Id}", dto);
            return result;
        }

        /// <summary>
        /// Removes a listing service identified by the specified ID.
        /// </summary>
        /// <remarks>This method sends a request to delete the specified listing service. Ensure that the
        /// provided <paramref name="listingServiceId"/> corresponds to an existing listing service.</remarks>
        /// <param name="listingServiceId">The unique identifier of the listing service to be removed. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the removal operation.</returns>
        public async Task<IBaseResult> RemoveListingService(string listingServiceId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"businessdirectory/listingServices", listingServiceId);
            return result;
        }

        /// <summary>
        /// Adds an image to a listing service.
        /// </summary>
        /// <remarks>This method sends the image data to the listing service for addition. Ensure that the
        /// <paramref name="request"/> contains valid data before calling this method.</remarks>
        /// <param name="request">The request containing the image data and associated listing service details.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> AddListingServiceImage(AddEntityImageRequest request, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"businessdirectory/listingServices/addImage", request);
            return result;
        }

        /// <summary>
        /// Removes an image associated with a listing service.
        /// </summary>
        /// <remarks>This method sends a request to delete the specified image from the listing service.
        /// Ensure that the  <paramref name="imageId"/> corresponds to a valid image.</remarks>
        /// <param name="imageId">The unique identifier of the image to be removed. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveListingServiceImage(string imageId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"businessdirectory/listingServices/deleteImage", imageId);
            return result;
        }

        /// <summary>
        /// Adds a new listing product to the business directory.
        /// </summary>
        /// <remarks>This method sends a POST request to the "businessdirectory/listingProducts"
        /// endpoint to add the specified listing product. Ensure that the <paramref name="dto"/> contains valid data
        /// before calling this method.</remarks>
        /// <param name="dto">The data transfer object containing the details of the listing product to be added.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> AddListingProduct(ListingProductDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"businessdirectory/listingProducts", dto);
            return result;
        }

        /// <summary>
        /// Updates the details of a listing product.
        /// </summary>
        /// <remarks>This method sends the updated listing product details to the server for processing.
        /// Ensure that the <paramref name="dto"/> parameter contains valid data before calling this method.</remarks>
        /// <param name="dto">The data transfer object containing the updated details of the listing product.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateListingProduct(ListingProductDto dto, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(dto.Id))
            {
                return await Result.FailAsync("A listing product identifier is required to update the record.");
            }

            var result = await provider.PutAsync($"businessdirectory/listingProducts/{dto.Id}", dto);
            return result;
        }

        /// <summary>
        /// Removes a listing product identified by the specified ID.
        /// </summary>
        /// <remarks>This method sends a request to delete the specified listing product. Ensure the
        /// provided <paramref name="listingProductId"/> is valid and corresponds to an existing listing
        /// product.</remarks>
        /// <param name="listingProductId">The unique identifier of the listing product to be removed. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveListingProduct(string listingProductId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"businessdirectory/listingProducts", listingProductId);
            return result;
        }

        /// <summary>
        /// Adds an image to a listing product in the business directory.
        /// </summary>
        /// <remarks>This method sends a request to the business directory service to associate an image
        /// with a specific listing product. Ensure that the <paramref name="request"/> contains valid data, including
        /// the required listing product identifier.</remarks>
        /// <param name="request">The request containing the details of the image to be added, including the listing product identifier and
        /// the image data.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> AddListingProductImage(AddEntityImageRequest request, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"businessdirectory/listingProducts/addImage", request);
            return result;
        }

        /// <summary>
        /// Removes an image associated with a listing product.
        /// </summary>
        /// <remarks>This method sends a request to delete the specified image from the listing product.
        /// Ensure that the  <paramref name="imageId"/> corresponds to a valid image.</remarks>
        /// <param name="imageId">The unique identifier of the image to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveListingProductImage(string imageId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"businessdirectory/listingProducts/deleteImage", imageId);
            return result;
        }
    }
}
