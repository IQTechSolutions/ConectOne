using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Application.RestServices
{
    /// <summary>
    /// Provides methods for managing gifts, including retrieving, creating, updating, and deleting gift data.
    /// </summary>
    /// <remarks>This service acts as a REST client for interacting with the gift-related endpoints of the
    /// underlying provider. It supports asynchronous operations for retrieving gift lists, fetching gift details,
    /// creating new gifts, updating existing gifts, and removing gifts.</remarks>
    /// <param name="provider"></param>
    public class GiftRestService(IBaseHttpProvider provider) : IGiftService
    {
        /// <summary>
        /// Retrieves a list of available gifts asynchronously.
        /// </summary>
        /// <remarks>This method fetches the list of gifts from the underlying provider. The caller can
        /// use the cancellation token to cancel the operation if needed.</remarks>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// of <see cref="IEnumerable{T}"/> containing <see cref="GiftDto"/> objects representing the available gifts.</returns>
        public Task<IBaseResult<IEnumerable<GiftDto>>> GiftListAsync(CancellationToken cancellationToken = default)
        {
            var result = provider.GetAsync<IEnumerable<GiftDto>>("gifts");
            return result;
        }

        /// <summary>
        /// Retrieves the details of a gift asynchronously based on the specified gift ID.
        /// </summary>
        /// <remarks>This method sends a request to retrieve the gift details from the underlying
        /// provider. Ensure that the <paramref name="giftId"/> corresponds to a valid gift in the system.</remarks>
        /// <param name="giftId">The unique identifier of the gift to retrieve. This value cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping the <see cref="GiftDto"/> for the specified gift.</returns>
        public Task<IBaseResult<GiftDto>> GiftAsync(string giftId, CancellationToken cancellationToken = default)
        {
            var result = provider.GetAsync<GiftDto>($"gifts/{giftId}");
            return result;
        }

        /// <summary>
        /// Creates a new gift asynchronously using the provided gift data.
        /// </summary>
        /// <remarks>This method sends the gift data to the underlying provider for creation. Ensure that
        /// the <paramref name="dto"/> contains valid data before calling this method.</remarks>
        /// <param name="dto">The data transfer object containing the details of the gift to be created.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public Task<IBaseResult> CreateGiftAsync(GiftDto dto, CancellationToken cancellationToken = default)
        {
            var result = provider.PutAsync($"gifts", dto);
            return result;
        }

        /// <summary>
        /// Updates the details of a gift asynchronously.
        /// </summary>
        /// <remarks>This method sends the updated gift details to the server. Ensure that the <paramref
        /// name="dto"/> parameter contains valid data before calling this method.</remarks>
        /// <param name="dto">The data transfer object containing the updated gift details.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        public Task<IBaseResult> UpdateGiftAsync(GiftDto dto, CancellationToken cancellationToken = default)
        {
            var result = provider.PostAsync($"gifts", dto);
            return result;
        }

        /// <summary>
        /// Removes a gift with the specified identifier asynchronously.
        /// </summary>
        /// <remarks>This method sends a request to delete the specified gift. Ensure the <paramref
        /// name="giftId"/> corresponds to an existing gift. The operation may fail if the gift does not exist or if
        /// there are network issues.</remarks>
        /// <param name="giftId">The unique identifier of the gift to be removed. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public Task<IBaseResult> RemoveGiftAsync(string giftId, CancellationToken cancellationToken = default)
        {
            var result = provider.DeleteAsync($"gifts", giftId);
            return result;
        }
    }
}
