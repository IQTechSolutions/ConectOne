using AdvertisingModule.Domain.DataTransferObjects;
using AdvertisingModule.Domain.Interfaces;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.RequestFeatures;

namespace AdvertisingModule.Application.RestServices
{
    /// <summary>
    /// Provides methods for managing affiliate entities via RESTful HTTP operations.
    /// </summary>
    /// <remarks>This service allows creating, updating, and removing affiliates, as well as managing
    /// affiliate images. It communicates with a REST API using the provided <see cref="IBaseHttpProvider"/> to perform
    /// HTTP operations.</remarks>
    /// <param name="provider"></param>
    public class AffiliateCommandRestService(IBaseHttpProvider provider) : IAffiliateCommandService
    {
        /// <summary>
        /// Creates a new affiliate asynchronously.
        /// </summary>
        /// <param name="affiliate">The affiliate data to be created.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// object that includes the created <see cref="AffiliateDto"/> or details about the operation's outcome.</returns>
        public async Task<IBaseResult<AffiliateDto>> CreateAsync(AffiliateDto affiliate, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<AffiliateDto, AffiliateDto>("affiliates", affiliate);
            return result;
        }

        /// <summary>
        /// Updates the affiliate information asynchronously.
        /// </summary>
        /// <remarks>This method sends the updated affiliate data to the underlying provider. Ensure that
        /// the <paramref name="affiliate"/> parameter contains valid data before calling this method.</remarks>
        /// <param name="affiliate">The affiliate data to be updated.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateAsync(AffiliateDto affiliate, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<AffiliateDto, AffiliateDto>("affiliates", affiliate);
            return result;
        }

        /// <summary>
        /// Updates the display order of affiliates based on the provided request data.
        /// </summary>
        /// <remarks>This method sends the update request to the underlying provider to modify the display
        /// order of affiliates. Ensure that the <paramref name="dto"/> contains valid and complete data to avoid errors
        /// during the operation.</remarks>
        /// <param name="dto">The request object containing the affiliate display order update details. This must include the necessary
        /// information to identify the affiliates and their new display order.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation will terminate early if the token is canceled.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateAffiliateDisplayOrderAsync(AffiliateOrderUpdateRequest dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync("affiliates/updateDisplayOrder", dto);
            return result;
        }

        /// <summary>
        /// Removes an affiliate by its identifier.
        /// </summary>
        /// <remarks>This method sends a delete request to the underlying provider to remove the specified
        /// affiliate. Ensure that the <paramref name="affiliateId"/> corresponds to a valid affiliate.</remarks>
        /// <param name="affiliateId">The unique identifier of the affiliate to remove. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the removal operation.</returns>
        public async Task<IBaseResult> RemoveAsync(string affiliateId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync("affiliates", affiliateId);
            return result;
        }

        /// <summary>
        /// Adds an image to an entity based on the provided request.
        /// </summary>
        /// <remarks>This method sends the image data to the "affiliates/addImage" endpoint. Ensure that
        /// the request object contains valid data before calling this method.</remarks>
        /// <param name="request">The request containing the details of the image to be added, including the entity identifier and image data.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync("affiliates/addImage", request);
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
            var result = await provider.DeleteAsync("affiliates/deleteImage", imageId);
            return result;
        }
    }
}
