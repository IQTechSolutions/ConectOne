using BusinessModule.Domain.DataTransferObjects;
using BusinessModule.Domain.Entities;
using BusinessModule.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusinessModule.Infrastructure.Implementation;

/// <summary>
/// Provides query operations for retrieving listing tier data.
/// </summary>
/// <remarks>This service is responsible for querying listing tiers and their associated data, such as images. It
/// uses a repository pattern to interact with the data source and returns results in a standardized format.</remarks>
/// <param name="repository"></param>
public class ListingTierQueryService(IRepository<ListingTier, string> repository) : IListingTierQueryService
{
    /// <summary>
    /// Retrieves all listing tiers, including their associated images, as an asynchronous operation.
    /// </summary>
    /// <remarks>This method queries the repository for all listing tiers and maps the results to DTOs.  The
    /// associated images for each listing tier are included in the result.</remarks>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an  IBaseResult{T} of IEnumerable{T}
    /// containing ListingTierDto objects. If the operation fails, the result will include an error message.</returns>
    public async Task<IBaseResult<IEnumerable<ListingTierDto>>> AllListingTiersAsync(CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<ListingTier>(a => true);
        spec.AddInclude(i => i.Include(c => c.Images).ThenInclude(c => c.Image));

        var result = await repository.ListAsync(spec, false, cancellationToken);
        if (!result.Succeeded)
            return await Result<IEnumerable<ListingTierDto>>.FailAsync(result.Messages.FirstOrDefault() ?? "Failed to query Listing Tier.");
        return await Result<IEnumerable<ListingTierDto>>.SuccessAsync(result.Data.Select(a => new ListingTierDto(a)));
    }

    /// <summary>
    /// Retrieves a listing tier by its identifier.
    /// </summary>
    /// <remarks>This method queries the repository for a listing tier that matches the specified identifier. 
    /// If the listing tier is found, it is returned as a ListingTierDto. If the listing  tier is not found or the query
    /// fails, the result will indicate failure with an appropriate message.</remarks>
    /// <param name="id">The unique identifier of the listing tier to retrieve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an  IBaseResult{T} object that
    /// includes the requested ListingTierDto  if the operation succeeds, or an error message if it fails.</returns>
    public async Task<IBaseResult<ListingTierDto>> ListingTierAsync(string id, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<ListingTier>(a => a.Id == id);
        spec.AddInclude(i => i.Include(c => c.Images).ThenInclude(c => c.Image));

        var result = await repository.FirstOrDefaultAsync(spec, false, cancellationToken);
        if (!result.Succeeded)
            return await Result<ListingTierDto>.FailAsync(result.Messages.FirstOrDefault() ?? "Failed to query Listing Tier.");

        return await Result<ListingTierDto>.SuccessAsync(new ListingTierDto(result.Data!));
    }
}