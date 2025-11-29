using BusinessModule.Domain.DataTransferObjects;
using BusinessModule.Domain.RequestFeatures;
using ConectOne.Domain.ResultWrappers;

namespace BusinessModule.Domain.Interfaces;

/// <summary>
/// Provides methods for querying business directory data.
/// </summary>
public interface IBusinessDirectoryQueryService
{
    /// <summary>
    /// Retrieves a paginated list of business listings based on the specified request parameters.
    /// </summary>
    /// <remarks>Use this method to retrieve business listings in a paginated format. Ensure that the
    /// <paramref name="pageParameters"/>  specify valid pagination values, such as page number and page size, to avoid
    /// unexpected results.</remarks>
    /// <param name="pageParameters">The parameters that define the pagination and filtering options for the request.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A <see cref="PaginatedResult{T}"/> containing a collection of <see cref="BusinessListingDto"/> objects  and
    /// pagination metadata, such as the total number of items and pages.</returns>
    Task<PaginatedResult<BusinessListingDto>> PagedListingsAsync(BusinessListingPageParameters pageParameters, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a collection of active business listings.
    /// </summary>
    /// <remarks>The returned collection will only include listings that are currently active. If no active
    /// listings are found, the collection will be empty.</remarks>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation will be canceled if the token is triggered.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> with
    /// an enumerable collection of <see cref="BusinessListingDto"/> objects representing the active listings.</returns>
    Task<IBaseResult<IEnumerable<BusinessListingDto>>> ActiveListingsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the business listing details for the specified identifier.
    /// </summary>
    /// <remarks>The returned <see cref="IBaseResult{T}"/> may include additional metadata about the
    /// operation's success or failure. Ensure that the <paramref name="cancellationToken"/> is used to cancel the
    /// operation if needed to avoid unnecessary processing.</remarks>
    /// <param name="id">The unique identifier of the business listing to retrieve. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// object wrapping the <see cref="BusinessListingDto"/> for the specified identifier.</returns>
    Task<IBaseResult<BusinessListingDto>> ListingAsync(string id, CancellationToken cancellationToken = default);
}
