using AdvertisingModule.Domain.DataTransferObjects;
using AdvertisingModule.Domain.Interfaces;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.RequestFeatures;

namespace AdvertisingModule.Application.RestServices
{
    /// <summary>
    /// Provides methods for managing advertisement tiers, including creating, updating, and removing tiers,  as well as
    /// adding and removing associated images.
    /// </summary>
    /// <remarks>This service interacts with a REST API to perform operations on advertisement tiers. It uses
    /// an  <see cref="IBaseHttpProvider"/> to send HTTP requests and handle responses. The methods in this  service
    /// return results wrapped in <see cref="IBaseResult"/> to indicate the success or failure of  the
    /// operations.</remarks>
    /// <param name="provider"></param>
    public class AdvertisementTierCommandRestService(IBaseHttpProvider provider) : IAdvertisementTierCommandService
    {
        /// <summary>
        /// Creates a new advertisement tier asynchronously.
        /// </summary>
        /// <remarks>This method sends the provided advertisement tier data to the underlying provider for
        /// creation. Ensure that the <paramref name="advertisementTier"/> contains valid data before calling this
        /// method.</remarks>
        /// <param name="advertisementTier">The advertisement tier data to be created.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// of type <see cref="AdvertisementTierDto"/> representing the created advertisement tier.</returns>
        public async Task<IBaseResult<AdvertisementTierDto>> CreateAsync(AdvertisementTierDto advertisementTier, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<AdvertisementTierDto, AdvertisementTierDto>("advertisement_tiers", advertisementTier);
            return result;
        }

        /// <summary>
        /// Updates an advertisement tier asynchronously.
        /// </summary>
        /// <param name="advertisementTier">The advertisement tier data to be updated. This parameter cannot be <see langword="null"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// representing the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateAsync(AdvertisementTierDto advertisementTier, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<AdvertisementTierDto, AdvertisementTierDto>("advertisement_tiers", advertisementTier);
            return result;
        }

        /// <summary>
        /// Removes an advertisement tier asynchronously.
        /// </summary>
        /// <remarks>This method sends a delete request to the underlying provider to remove the specified
        /// advertisement tier. Ensure that the <paramref name="advertisementTierId"/> corresponds to a valid
        /// advertisement tier.</remarks>
        /// <param name="advertisementTierId">The unique identifier of the advertisement tier to remove. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the removal operation.</returns>
        public async Task<IBaseResult> RemoveAsync(string advertisementTierId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync("advertisement_tiers", advertisementTierId);
            return result;
        }

        /// <summary>
        /// Adds an image to an advertisement tier.
        /// </summary>
        /// <remarks>This method sends the image data to the server for association with the specified
        /// advertisement tier. Ensure that the <paramref name="request"/> contains valid data before calling this
        /// method.</remarks>
        /// <param name="request">The request containing the image data and associated advertisement tier details.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync("advertisement_tiers/addImage", request);
            return result;
        }

        /// <summary>
        /// Removes an image associated with the specified image identifier.
        /// </summary>
        /// <remarks>This method sends a request to delete the image identified by <paramref
        /// name="imageId"/>.  Ensure that the provided identifier corresponds to an existing image.</remarks>
        /// <param name="imageId">The unique identifier of the image to be removed. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveImage(string imageId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync("advertisement_tiers/deleteImage", imageId);
            return result; ;
        }
    }
}
