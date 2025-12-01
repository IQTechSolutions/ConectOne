using AdvertisingModule.Domain.DataTransferObjects;
using AdvertisingModule.Domain.Interfaces;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.RequestFeatures;

namespace AdvertisingModule.Application.RestServices
{
    /// <summary>
    /// Provides methods for managing advertisements through RESTful API calls.
    /// </summary>
    /// <remarks>This service allows clients to create, update, remove, approve, and reject advertisements, as
    /// well as manage advertisement images. All operations are performed asynchronously and rely on the provided <see
    /// cref="IBaseHttpProvider"/> for HTTP communication.</remarks>
    /// <param name="provider"></param>
    public class AdvertisementCommandRestService(IBaseHttpProvider provider) : IAdvertisementCommandService
    {
        /// <summary>
        /// Creates a new advertisement asynchronously.
        /// </summary>
        /// <param name="advertisement">The advertisement data to be created.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult"/>
        /// of type <see cref="AdvertisementDto"/> representing the created advertisement.</returns>
        public async Task<IBaseResult<AdvertisementDto>> CreateAsync(AdvertisementDto advertisement, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<AdvertisementDto, AdvertisementDto>("advertisements", advertisement);
            return result;
        }

        /// <summary>
        /// Updates an advertisement asynchronously.
        /// </summary>
        /// <remarks>This method sends the provided advertisement data to the underlying service for
        /// updating.  Ensure that the <paramref name="advertisement"/> parameter contains valid data before calling
        /// this method.</remarks>
        /// <param name="advertisement">The advertisement data to be updated.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the outcome of the update
        /// operation.</returns>
        public async Task<IBaseResult> UpdateAsync(AdvertisementDto advertisement, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<AdvertisementDto, AdvertisementDto>("advertisements", advertisement);
            return result;
        }

        /// <summary>
        /// Removes an advertisement with the specified identifier asynchronously.
        /// </summary>
        /// <remarks>This method performs the removal operation asynchronously. Ensure that the provided 
        /// <paramref name="advertisementId"/> corresponds to an existing advertisement.</remarks>
        /// <param name="advertisementId">The unique identifier of the advertisement to remove. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the removal operation.</returns>
        public async Task<IBaseResult> RemoveAsync(string advertisementId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync("advertisements", advertisementId);
            return result;
        }

        /// <summary>
        /// Approves the advertisement with the specified identifier.
        /// </summary>
        /// <remarks>This method sends a request to approve the specified advertisement. Ensure the
        /// provided  <paramref name="advertisementId"/> is valid and corresponds to an existing
        /// advertisement.</remarks>
        /// <param name="advertisementId">The unique identifier of the advertisement to approve. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the approval operation.</returns>
        public async Task<IBaseResult> ApproveAsync(string advertisementId, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"advertisements/approve/{advertisementId}");
            return result;
        }

        /// <summary>
        /// Rejects the advertisement with the specified identifier.
        /// </summary>
        /// <remarks>This method sends a request to reject the specified advertisement. Ensure the
        /// provided  <paramref name="advertisementId"/> is valid and corresponds to an existing
        /// advertisement.</remarks>
        /// <param name="advertisementId">The unique identifier of the advertisement to reject. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the rejection operation.</returns>
        public async Task<IBaseResult> RejectAsync(string advertisementId, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"advertisements/reject/{advertisementId}");
            return result;
        }

        /// <summary>
        /// Adds an image to an existing advertisement entity.
        /// </summary>
        /// <remarks>This method sends a request to add an image to an advertisement entity. Ensure that
        /// the <paramref name="request"/> contains valid data, including the advertisement identifier and the image
        /// details.</remarks>
        /// <param name="request">The request object containing the advertisement entity details and the image to be added.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<AdvertisementDto, AddEntityImageRequest>("advertisements/addImage", request);
            return result;
        }

        /// <summary>
        /// Removes an image with the specified identifier.
        /// </summary>
        /// <remarks>This method sends a request to delete the image associated with the specified
        /// <paramref name="imageId"/>.  Ensure that the identifier corresponds to an existing image before calling this
        /// method.</remarks>
        /// <param name="imageId">The unique identifier of the image to be removed. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveImage(string imageId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync("advertisements/deleteImage", imageId);
            return result;
        }
    }
}
