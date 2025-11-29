using BusinessModule.Domain.DataTransferObjects;
using ConectOne.Domain.ResultWrappers;

namespace BusinessModule.Domain.Interfaces;

/// <summary>
/// Provides methods for querying listing tier information.
/// </summary>
/// <remarks>This service is used to retrieve details about listing tiers, including fetching all available tiers
/// or retrieving a specific tier by its identifier. The methods return results wrapped in a standardized result type to
/// indicate success or failure.</remarks>
public interface IListingTierQueryService
{
    /// <summary>
    /// Retrieves all available listing tiers.
    /// </summary>
    /// <remarks>The returned collection may be empty if no listing tiers are available. Ensure to check the
    /// result's status and data for proper handling.</remarks>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation will be canceled if the token is triggered.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> with
    /// an enumerable collection of <see cref="ListingTierDto"/> objects representing the listing tiers.</returns>
    Task<IBaseResult<IEnumerable<ListingTierDto>>> AllListingTiersAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the listing tier details associated with the specified identifier.
    /// </summary>
    /// <remarks>Use this method to retrieve detailed information about a specific listing tier. Ensure that
    /// the provided identifier is valid and corresponds to an existing listing tier in the system.</remarks>
    /// <param name="id">The unique identifier of the listing tier to retrieve. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// object that includes the details of the listing tier as a <see cref="ListingTierDto"/>. If the identifier is
    /// invalid or the listing tier is not found, the result may indicate an error.</returns>
    Task<IBaseResult<ListingTierDto>> ListingTierAsync(string id, CancellationToken cancellationToken = default);
}
