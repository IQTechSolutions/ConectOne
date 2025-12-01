using AdvertisingModule.Domain.Interfaces;
using AdvertisingModule.Domain.DataTransferObjects;
using AdvertisingModule.Domain.RequestFeatures;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;

namespace AdvertisingModule.Application.RestServices
{
    /// <summary>
    /// Provides methods for querying advertisements through a RESTful API.
    /// </summary>
    /// <remarks>This service allows clients to retrieve advertisement data, including active advertisements, 
    /// all advertisements, paginated listings, and individual advertisement details. The methods  return results
    /// wrapped in appropriate result types to indicate success or failure, and support  cancellation through <see
    /// cref="CancellationToken"/>.</remarks>
    /// <param name="provider"></param>
    public class AdvertisementQueryRestService(IBaseHttpProvider provider) : IAdvertisementQueryService
    {
        /// <summary>
        /// Retrieves a collection of active advertisements.
        /// </summary>
        /// <remarks>The method fetches active advertisements from the underlying data provider. If no
        /// advertisements are active, the result may contain an empty collection.</remarks>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// with a collection of <see cref="AdvertisementDto"/> representing the active advertisements.</returns>
        public async Task<IBaseResult<IEnumerable<AdvertisementDto>>> ActiveAdvertisementsAsync(CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<AdvertisementDto>>("advertisements/active");
            return result;
        }

        /// <summary>
        /// Retrieves all advertisements asynchronously.
        /// </summary>
        /// <remarks>This method fetches all advertisements from the underlying data provider. The
        /// operation may be canceled by passing a cancellation token.</remarks>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// containing an enumerable collection of <see cref="AdvertisementDto"/> objects.</returns>
        public async Task<IBaseResult<IEnumerable<AdvertisementDto>>> AllAdvertisementsAsync(CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<AdvertisementDto>>("advertisements");
            return result;
        }

        /// <summary>
        /// Retrieves a paginated list of advertisements based on the specified page parameters.
        /// </summary>
        /// <remarks>The method fetches advertisements from the data source using the provided pagination
        /// and filtering criteria. Ensure that <paramref name="pageParameters"/> contains valid values to avoid
        /// unexpected results.</remarks>
        /// <param name="pageParameters">The parameters that define the pagination and filtering options for the advertisement listings.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing the paginated list of advertisements.</returns>
        public async Task<PaginatedResult<AdvertisementDto>> PagedListingsAsync(AdvertisementListingPageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetPagedAsync<AdvertisementDto, AdvertisementListingPageParameters>("advertisements/paged", pageParameters);
            return result;
        }

        /// <summary>
        /// Retrieves an advertisement by its unique identifier.
        /// </summary>
        /// <remarks>This method fetches the advertisement details from the underlying provider using the
        /// specified identifier. Ensure that the <paramref name="id"/> corresponds to a valid advertisement in the
        /// system.</remarks>
        /// <param name="id">The unique identifier of the advertisement to retrieve. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping the <see cref="AdvertisementDto"/> for the specified advertisement.</returns>
        public async Task<IBaseResult<AdvertisementDto>> AdvertisementAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<AdvertisementDto>($"advertisements/{id}");
            return result;
        }
    }
}
