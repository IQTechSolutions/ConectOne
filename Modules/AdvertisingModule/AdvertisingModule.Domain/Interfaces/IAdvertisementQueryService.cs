using AdvertisingModule.Domain.DataTransferObjects;
using AdvertisingModule.Domain.RequestFeatures;
using ConectOne.Domain.ResultWrappers;

namespace AdvertisingModule.Domain.Interfaces;

/// <summary>
/// Provides methods for querying advertisements.
/// </summary>
/// <remarks>This service is used to retrieve information about advertisements, such as those that are currently
/// active.</remarks>
public interface IAdvertisementQueryService
{
    /// <summary>
    /// Retrieves a collection of active advertisements.
    /// </summary>
    /// <remarks>The returned collection will be empty if no active advertisements are found. Ensure proper
    /// handling of the <paramref name="cancellationToken"/> to avoid unnecessary resource usage.</remarks>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation will be canceled if the token is triggered.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> with
    /// a collection of <see cref="AdvertisementDto"/> objects representing the active advertisements.</returns>
    Task<IBaseResult<IEnumerable<AdvertisementDto>>> ActiveAdvertisementsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all advertisements asynchronously.
    /// </summary>
    /// <remarks>The returned collection may be empty if no advertisements are available. Ensure to handle the
    /// result appropriately.</remarks>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> with
    /// an enumerable collection of <see cref="AdvertisementDto"/> objects representing the advertisements.</returns>
    Task<IBaseResult<IEnumerable<AdvertisementDto>>> AllAdvertisementsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a paginated list of advertisements based on the specified page parameters.
    /// </summary>
    /// <remarks>Use this method to retrieve advertisements in a paginated format, which is useful for
    /// scenarios  where large datasets need to be displayed incrementally. Ensure that the <paramref
    /// name="pageParameters"/>  are valid to avoid unexpected results.</remarks>
    /// <param name="pageParameters">The parameters that define the pagination and filtering options for the advertisement listings.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A <see cref="PaginatedResult{T}"/> containing a collection of <see cref="AdvertisementDto"/> objects  and
    /// pagination metadata. The result will be empty if no advertisements match the specified criteria.</returns>
    Task<PaginatedResult<AdvertisementDto>> PagedListingsAsync(AdvertisementListingPageParameters pageParameters, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves an advertisement asynchronously based on the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the advertisement to retrieve. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see
    /// cref="IBaseResult{AdvertisementDto}"/> with the advertisement details if found; otherwise, an appropriate error
    /// result.</returns>
    Task<IBaseResult<AdvertisementDto>> AdvertisementAsync(string id, CancellationToken cancellationToken = default);
}
