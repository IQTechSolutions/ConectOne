using AdvertisingModule.Domain.DataTransferObjects;
using AdvertisingModule.Domain.Interfaces;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;

namespace AdvertisingModule.Application.RestServices
{
    /// <summary>
    /// Provides methods for querying affiliate data from a RESTful API.
    /// </summary>
    /// <remarks>This service is designed to interact with a REST API to retrieve affiliate information. It
    /// supports retrieving all affiliates or a specific affiliate by their unique identifier.</remarks>
    /// <param name="provider"></param>
    public class AffiliateQueryRestService(IBaseHttpProvider provider) : IAffiliateQueryService
    {
        /// <summary>
        /// Retrieves a collection of all affiliates.
        /// </summary>
        /// <remarks>This method fetches all affiliates from the underlying data provider. The returned
        /// result includes the affiliates as a collection of <see cref="AffiliateDto"/> objects. If no affiliates are
        /// available, the collection will be empty.</remarks>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// containing an enumerable collection of <see cref="AffiliateDto"/> objects.</returns>
        public async Task<IBaseResult<IEnumerable<AffiliateDto>>> AllAffiliatesAsync(CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<AffiliateDto>>("affiliates");
            return result;
        }

        /// <summary>
        /// Retrieves affiliate information based on the specified identifier.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to retrieve affiliate details from the
        /// underlying provider. Ensure that the <paramref name="id"/> corresponds to a valid affiliate.</remarks>
        /// <param name="id">The unique identifier of the affiliate to retrieve. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that wraps the <see cref="AffiliateDto"/> for the specified affiliate.</returns>
        public async Task<IBaseResult<AffiliateDto>> AffiliateAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<AffiliateDto>($"affiliates/{id}");
            return result;
        }
    }
}
