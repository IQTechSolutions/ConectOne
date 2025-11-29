using AdvertisingModule.Domain.DataTransferObjects;
using ConectOne.Domain.ResultWrappers;

namespace AdvertisingModule.Domain.Interfaces;

/// <summary>
/// Defines methods for querying affiliate advertisements.
/// </summary>
/// <remarks>This service provides functionality to retrieve information about affiliate advertisements, 
/// including fetching all affiliates or retrieving a specific affiliate by its identifier.</remarks>
public interface IAffiliateQueryService
{
    /// <summary>
    /// Retrieves all affiliate advertisements asynchronously.
    /// </summary>
    /// <remarks>This method retrieves all affiliate advertisements available in the system. The operation
    /// supports cancellation through the provided <paramref name="cancellationToken"/>.</remarks>
    /// <param name="cancellationToken">A token that can be used to cancel the operation. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> with
    /// an enumerable collection of <see cref="AdvertisementDto"/> representing the affiliate advertisements.</returns>
    Task<IBaseResult<IEnumerable<AffiliateDto>>> AllAffiliatesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves affiliate information for the specified advertisement.
    /// </summary>
    /// <param name="id">The unique identifier of the advertisement to retrieve affiliate information for. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// object wrapping the <see cref="AdvertisementDto"/> with the affiliate information.</returns>
    Task<IBaseResult<AffiliateDto>> AffiliateAsync(string id, CancellationToken cancellationToken = default);
}
