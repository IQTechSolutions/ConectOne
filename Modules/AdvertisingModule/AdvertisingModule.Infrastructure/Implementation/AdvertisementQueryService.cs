using AdvertisingModule.Domain.DataTransferObjects;
using AdvertisingModule.Domain.Entities;
using AdvertisingModule.Domain.Interfaces;
using AdvertisingModule.Domain.RequestFeatures;
using AdvertisingModule.Infrastructure.Specifications;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AdvertisingModule.Infrastructure.Implementation;

/// <summary>
/// Provides query operations for retrieving advertisements from the repository.
/// </summary>
/// <remarks>This service is responsible for querying advertisements based on specific conditions, such as
/// retrieving active and approved advertisements.</remarks>
/// <param name="repository"></param>
public class AdvertisementQueryService(IRepository<Advertisement, string> repository) : IAdvertisementQueryService
{
    /// <summary>
    /// Retrieves a collection of active and approved advertisements.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an  <IBaseResult{T}/> with a
    /// collection of <AdvertisementDto/> objects  representing the active and approved advertisements, ordered by their
    /// tier. If the operation  fails, the result contains an error message.</returns>
    public async Task<IBaseResult<IEnumerable<AdvertisementDto>>> ActiveAdvertisementsAsync(CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<Advertisement>(a => true);
        spec.AddInclude(c => c.Include(g => g.AdvertisementTier));
        spec.AddInclude(c => c.Include(g => g.Images).ThenInclude(g => g.Image));

        var result = await repository.ListAsync(spec, false, cancellationToken);
        if (!result.Succeeded)
            return await Result<IEnumerable<AdvertisementDto>>.FailAsync(result.Messages.FirstOrDefault() ?? "Failed to query ads.");

        var ads = result.Data.Where(c => c.Active).OrderBy(a => a.AdvertisementTier).ToList();
        return await Result<IEnumerable<AdvertisementDto>>.SuccessAsync(ads.Select(a => new AdvertisementDto(a)));
    }

    /// <summary>
    /// Retrieves all advertisements, ordered by their tier.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an  <IBaseResult{T}/> where <c>T</c>
    /// is an <IEnumerable{T}/> of  <AdvertisementDto/> objects. If the operation succeeds, the result contains  the
    /// advertisements ordered by tier; otherwise, it contains an error message.</returns>
    public async Task<IBaseResult<IEnumerable<AdvertisementDto>>> AllAdvertisementsAsync(CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<Advertisement>(a => true);
        spec.AddInclude(c => c.Include(g => g.AdvertisementTier));
        spec.AddInclude(c => c.Include(g => g.Images).ThenInclude(g => g.Image));

        var result = await repository.ListAsync(spec, false, cancellationToken);
        if (!result.Succeeded)
            return await Result<IEnumerable<AdvertisementDto>>.FailAsync(result.Messages.FirstOrDefault() ?? "Failed to query ads.");

        var ads = result.Data.OrderBy(a => a.AdvertisementTier).ToList();
        return await Result<IEnumerable<AdvertisementDto>>.SuccessAsync(ads.Select(a => new AdvertisementDto(a)));
    }

    /// <summary>
    /// Retrieves a paginated list of advertisements based on the specified page parameters.
    /// </summary>
    /// <remarks>This method retrieves advertisements along with their associated images. The results are
    /// paginated based on the provided <paramref name="pageParameters"/>. If the operation fails, the returned <see
    /// cref="PaginatedResult{T}"/> will contain error messages.</remarks>
    /// <param name="pageParameters">The parameters that define the pagination settings, including the page number and page size.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A <see cref="PaginatedResult{T}"/> containing a collection of <see cref="AdvertisementDto"/> objects
    /// representing the advertisements for the specified page, along with pagination metadata.</returns>
    public async Task<PaginatedResult<AdvertisementDto>> PagedListingsAsync(AdvertisementListingPageParameters pageParameters, CancellationToken cancellationToken = default)
    {
        var result = await repository.ListAsync(new AdvertisementListingSpecification(pageParameters), trackChanges: false, cancellationToken);
        if (!result.Succeeded)
            return PaginatedResult<AdvertisementDto>.Failure(result.Messages);

        var resultItems = result.Data;

        var page = resultItems.Select(v => new AdvertisementDto(v)).ToList();
        return PaginatedResult<AdvertisementDto>.Success(page, result.Data.Count, pageParameters.PageNr, pageParameters.PageSize);
    }

    /// <summary>
    /// Asynchronously retrieves an advertisement by its identifier.
    /// </summary>
    /// <remarks>This method queries the repository for an advertisement with the specified identifier.  If
    /// the advertisement is found, it returns a successful result containing the advertisement data. If the
    /// advertisement is not found or the query fails, it returns a failure result with an appropriate
    /// message.</remarks>
    /// <param name="id">The unique identifier of the advertisement to retrieve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an  IBaseResult{AdvertisementDto}
    /// which includes the advertisement data if successful.</returns>
    public async Task<IBaseResult<AdvertisementDto>> AdvertisementAsync(string id, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<Advertisement>(a => a.Id == id);
        spec.AddInclude(c => c.Include(g => g.AdvertisementTier));
        spec.AddInclude(c => c.Include(g => g.Images).ThenInclude(g => g.Image));

        var result = await repository.FirstOrDefaultAsync(spec, false, cancellationToken);
        if (!result.Succeeded)
            return await Result<AdvertisementDto>.FailAsync(result.Messages.FirstOrDefault() ?? "Failed to query ads.");

        return await Result<AdvertisementDto>.SuccessAsync(new AdvertisementDto(result.Data));
    }
}
