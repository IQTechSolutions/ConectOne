using BusinessModule.Domain.DataTransferObjects;
using BusinessModule.Domain.Interfaces;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.RequestFeatures;

namespace BusinessModule.Application.RestServices
{
    /// <summary>
    /// Provides a REST-based implementation of the <see cref="IListingTierCommandService"/> interface for managing
    /// listing tiers.
    /// </summary>
    /// <remarks>This service allows for creating, updating, and removing listing tiers, as well as managing
    /// associated images. It communicates with an underlying HTTP provider to perform these operations
    /// asynchronously.</remarks>
    /// <param name="provider"></param>
    public class ListingTierCommandRestService(IBaseHttpProvider provider) : IListingTierCommandService
    {
        /// <summary>
        /// Creates a new listing tier asynchronously.
        /// </summary>
        /// <remarks>This method sends the provided <see cref="ListingTierDto"/> to the underlying
        /// provider for creation. Ensure that the <paramref name="listingTier"/> object contains all required fields
        /// before calling this method.</remarks>
        /// <param name="listingTier">The <see cref="ListingTierDto"/> object representing the listing tier to be created.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation. The default value is <see
        /// cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult"/>
        /// of type <see cref="ListingTierDto"/> representing the created listing tier.</returns>
        public Task<IBaseResult<ListingTierDto>> CreateAsync(ListingTierDto listingTier, CancellationToken cancellationToken = default)
        {
            var result = provider.PutAsync<ListingTierDto, ListingTierDto>("listing_tiers", listingTier);
            return result;
        }

        /// <summary>
        /// Updates the listing tier information asynchronously.
        /// </summary>
        /// <param name="listingTier">The data transfer object containing the updated listing tier information.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the update operation.</returns>
        public Task<IBaseResult> UpdateAsync(ListingTierDto listingTier, CancellationToken cancellationToken = default)
        {
            var result = provider.PostAsync("listing_tiers", listingTier);
            return result;
        }

        /// <summary>
        /// Removes a listing tier asynchronously.
        /// </summary>
        /// <remarks>This method sends a delete request to the underlying provider to remove the specified
        /// listing tier. Ensure that the <paramref name="listingTierId"/> corresponds to an existing listing
        /// tier.</remarks>
        /// <param name="listingTierId">The unique identifier of the listing tier to be removed. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the removal operation.</returns>
        public Task<IBaseResult> RemoveAsync(string listingTierId, CancellationToken cancellationToken = default)
        {
            var result = provider.DeleteAsync("listing_tiers", listingTierId);
            return result;
        }

        /// <summary>
        /// Adds an image to an entity based on the specified request.
        /// </summary>
        /// <remarks>This method sends a request to add an image to the specified entity. Ensure that the
        /// <paramref name="request"/> contains valid data.</remarks>
        /// <param name="request">The request containing the details of the image to be added, including the entity identifier and image data.
        /// Cannot be <see langword="null"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default)
        {
            var result = provider.PostAsync("listing_tiers/addImage", request);
            return result;
        }

        /// <summary>
        /// Removes an image associated with the specified image identifier.
        /// </summary>
        /// <remarks>This method sends a request to delete the image identified by <paramref
        /// name="imageId"/>. Ensure that the provided identifier corresponds to an existing image.</remarks>
        /// <param name="imageId">The unique identifier of the image to be removed. This value cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public Task<IBaseResult> RemoveImage(string imageId, CancellationToken cancellationToken = default)
        {
            var result = provider.DeleteAsync("listing_tiers/deleteImage", imageId);
            return result;
        }
    }
}
