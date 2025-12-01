using BusinessModule.Domain.DataTransferObjects;
using BusinessModule.Domain.Interfaces;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;

namespace BusinessModule.Application.RestServices
{
    /// <summary>
    /// Provides methods for querying listing tier data from a RESTful service.
    /// </summary>
    /// <remarks>This service is responsible for retrieving listing tier information, including all available
    /// tiers and details for specific tiers, from the underlying data provider. It implements the <see
    /// cref="IListingTierQueryService"/> interface and uses an <see cref="IBaseHttpProvider"/> to perform HTTP
    /// operations.</remarks>
    /// <param name="provider"></param>
    public class ListingTierQueryRestService(IBaseHttpProvider provider) : IListingTierQueryService
    {
        /// <summary>
        /// Retrieves all available listing tiers.
        /// </summary>
        /// <remarks>This method fetches all listing tiers from the underlying data provider. The result
        /// includes details about each listing tier, such as its properties and attributes. If no listing tiers are
        /// available, the result may contain an empty collection.</remarks>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// containing an enumerable collection of <see cref="ListingTierDto"/> objects representing the listing tiers.</returns>
        public async Task<IBaseResult<IEnumerable<ListingTierDto>>> AllListingTiersAsync(CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<ListingTierDto>>("listing_tiers/all");
            return result;
        }

        /// <summary>
        /// Retrieves the listing tier details for the specified identifier.
        /// </summary>
        /// <remarks>This method fetches the listing tier details from the provider using the specified
        /// identifier. Ensure that the <paramref name="id"/> corresponds to a valid listing tier.</remarks>
        /// <param name="id">The unique identifier of the listing tier to retrieve.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the listing tier details as a <see cref="ListingTierDto"/>.</returns>
        public async Task<IBaseResult<ListingTierDto>> ListingTierAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<ListingTierDto>($"listing_tiers/{id}");
            return result;
        }
    }
}
