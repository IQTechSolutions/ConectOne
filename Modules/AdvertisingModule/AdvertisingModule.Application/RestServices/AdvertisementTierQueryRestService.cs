using AdvertisingModule.Domain.DataTransferObjects;
using AdvertisingModule.Domain.Enums;
using AdvertisingModule.Domain.Interfaces;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;

namespace AdvertisingModule.Application.RestServices
{
    /// <summary>
    /// Provides methods for querying advertisement tiers via a RESTful service.
    /// </summary>
    /// <remarks>This service allows retrieval of advertisement tiers, either as a collection or individually
    /// by ID,  based on the specified advertisement type. It communicates with a REST API using the provided HTTP
    /// provider.</remarks>
    /// <param name="provider"></param>
    public class AdvertisementTierQueryRestService(IBaseHttpProvider provider) : IAdvertisementTierQueryService
    {
        /// <summary>
        /// Retrieves all advertisement tiers for the specified advertisement type.
        /// </summary>
        /// <remarks>This method communicates with an external provider to fetch the advertisement tiers.
        /// Ensure that the <paramref name="type"/> parameter corresponds to a valid advertisement type supported by the
        /// provider.</remarks>
        /// <param name="type">The type of advertisement for which to retrieve the tiers.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// containing an enumerable collection of <see cref="AdvertisementTierDto"/> objects representing the
        /// advertisement tiers.</returns>
        public async Task<IBaseResult<IEnumerable<AdvertisementTierDto>>> AllAdvertisementTiersAsync(AdvertisementType type, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<AdvertisementTierDto>>($"advertisement_tiers/all/{type}");
            return result;
        }

        /// <summary>
        /// Retrieves the advertisement tier details for the specified identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the advertisement tier to retrieve.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the advertisement tier details as an <see cref="AdvertisementTierDto"/>.</returns>
        public async Task<IBaseResult<AdvertisementTierDto>> AdvertisementTierAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<AdvertisementTierDto>($"advertisement_tiers/{id}");
            return result;
        }
    }
}
