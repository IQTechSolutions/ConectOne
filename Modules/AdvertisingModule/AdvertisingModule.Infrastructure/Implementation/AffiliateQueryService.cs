using AdvertisingModule.Domain.DataTransferObjects;
using AdvertisingModule.Domain.Entities;
using AdvertisingModule.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AdvertisingModule.Infrastructure.Implementation;

/// <summary>
/// Provides query operations for retrieving affiliate data.
/// </summary>
/// <remarks>This service is responsible for querying affiliate information, including retrieving all affiliates
/// or a specific affiliate by its identifier. The service uses a repository pattern to interact with the underlying
/// data store and supports cancellation tokens for asynchronous operations.</remarks>
/// <param name="repository"></param>
public class AffiliateQueryService(IRepository<Affiliate, string> repository) : IAffiliateQueryService
{
    /// <summary>
    /// Retrieves a collection of all affiliates, including their associated images, asynchronously.
    /// </summary>
    /// <remarks>This method queries all affiliates from the data source and maps them to <see
    /// cref="AffiliateDto"/>  objects. The result includes any associated images for each affiliate. If the query
    /// fails, the  returned result will indicate failure along with an appropriate error message.</remarks>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult"/>
    /// where <c>T</c> is an <see cref="IEnumerable{T}"/> of  <see cref="AffiliateDto"/> objects. If the operation
    /// fails, the result contains an error message.</returns>
    public async Task<IBaseResult<IEnumerable<AffiliateDto>>> AllAffiliatesAsync(CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<Affiliate>(a => true);
        spec.AddInclude(c => c.Include(g => g.Images).ThenInclude(g => g.Image));

        var result = await repository.ListAsync(spec, false, cancellationToken);
        if (!result.Succeeded)
            return await Result<IEnumerable<AffiliateDto>>.FailAsync(result.Messages.FirstOrDefault() ?? "Failed to query affiliates.");
        return await Result<IEnumerable<AffiliateDto>>.SuccessAsync(result.Data.OrderBy(c => c.DisplayOrder).Select(a => new AffiliateDto(a)));
    }

    /// <summary>
    /// Retrieves an affiliate by its unique identifier.
    /// </summary>
    /// <remarks>This method queries the repository for an affiliate matching the specified identifier.  If
    /// the affiliate is found, it is returned as an <see cref="AffiliateDto"/>. If no affiliate  is found or the query
    /// fails, the result will indicate failure with an appropriate message.</remarks>
    /// <param name="id">The unique identifier of the affiliate to retrieve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult{T}"/> of
    /// type <see cref="AffiliateDto"/>. If the operation succeeds,  the result contains the affiliate data; otherwise,
    /// it contains an error message.</returns>
    public async Task<IBaseResult<AffiliateDto>> AffiliateAsync(string id, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<Affiliate>(a => a.Id == id);
        spec.AddInclude(c => c.Include(g => g.Images).ThenInclude(g => g.Image));

        var result = await repository.FirstOrDefaultAsync(spec, false, cancellationToken);
        if (!result.Succeeded)
            return await Result<AffiliateDto>.FailAsync(result.Messages.FirstOrDefault() ?? "Failed to query affiliates.");

        return await Result<AffiliateDto>.SuccessAsync(new AffiliateDto(result.Data));
    }
}
