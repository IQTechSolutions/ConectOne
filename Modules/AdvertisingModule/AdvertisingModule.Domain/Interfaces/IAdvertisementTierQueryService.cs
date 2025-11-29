using AdvertisingModule.Domain.DataTransferObjects;
using AdvertisingModule.Domain.Enums;
using ConectOne.Domain.ResultWrappers;

namespace AdvertisingModule.Domain.Interfaces;

/// <summary>
/// Defines a contract for querying advertisement tier information.
/// </summary>
/// <remarks>This service provides methods to retrieve advertisement tier data, including all available tiers and
/// details for a specific tier. The results are wrapped in an <see cref="IBaseResult"/> to include both the data and
/// the operation's status. Implementations of this interface should handle cancellation requests via the provided <see
/// cref="CancellationToken"/>.</remarks>
public interface IAdvertisementTierQueryService
{
    /// <summary>
    /// Retrieves all advertisement tiers available in the system.
    /// </summary>
    /// <remarks>The returned collection may be empty if no advertisement tiers are available. Ensure to check
    /// the result's status and data for proper handling.</remarks>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// wrapping an enumerable collection of <see cref="AdvertisementTierDto"/> objects representing the advertisement
    /// tiers.</returns>
    Task<IBaseResult<IEnumerable<AdvertisementTierDto>>> AllAdvertisementTiersAsync(AdvertisementType type, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the advertisement tier details for the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the advertisement tier to retrieve. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// object wrapping the <see cref="AdvertisementTierDto"/> for the specified identifier.</returns>
    Task<IBaseResult<AdvertisementTierDto>> AdvertisementTierAsync(string id, CancellationToken cancellationToken = default);
}
