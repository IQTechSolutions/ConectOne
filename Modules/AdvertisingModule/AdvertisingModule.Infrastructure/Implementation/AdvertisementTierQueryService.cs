using AdvertisingModule.Domain.DataTransferObjects;
using AdvertisingModule.Domain.Entities;
using AdvertisingModule.Domain.Enums;
using AdvertisingModule.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AdvertisingModule.Infrastructure.Implementation;

/// <summary>
/// Provides query operations for retrieving advertisement tier data.
/// </summary>
/// <remarks>This service is responsible for querying advertisement tier information from the underlying data
/// store. It supports retrieving all advertisement tiers or a specific advertisement tier by its identifier.</remarks>
/// <param name="repository"></param>
public class AdvertisementTierQueryService(IRepository<AdvertisementTier, string> repository) : IAdvertisementTierQueryService
{
    /// <summary>
    /// Retrieves all advertisement tiers as a collection of data transfer objects (DTOs).
    /// </summary>
    /// <remarks>This method queries all advertisement tiers from the repository and maps them to DTOs. If the
    /// query fails, the result will indicate failure with an appropriate error message.</remarks>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// where <c>T</c> is an <see cref="IEnumerable{T}"/> of <see cref="AdvertisementTierDto"/>. If the operation
    /// succeeds, the result contains the collection of advertisement tiers; otherwise, it contains an error message.</returns>
    public async Task<IBaseResult<IEnumerable<AdvertisementTierDto>>> AllAdvertisementTiersAsync(AdvertisementType type, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<AdvertisementTier>(a => a.AdvertisementType == type);
        spec.AddInclude(i => i.Include(c => c.Images).ThenInclude(c => c.Image));

        var result = await repository.ListAsync(spec, false, cancellationToken);
        if (!result.Succeeded)
            return await Result<IEnumerable<AdvertisementTierDto>>.FailAsync(result.Messages.FirstOrDefault() ?? "Failed to query Advertisement Tier.");
        return await Result<IEnumerable<AdvertisementTierDto>>.SuccessAsync(result.Data.Select(a => new AdvertisementTierDto(a)));
    }

    /// <summary>
    /// Retrieves the advertisement tier associated with the specified identifier.
    /// </summary>
    /// <remarks>This method queries the repository for an advertisement tier matching the specified
    /// identifier. If no matching record is found, the result will indicate failure with an appropriate error
    /// message.</remarks>
    /// <param name="id">The unique identifier of the advertisement tier to retrieve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// object that includes the advertisement tier data as an <see cref="AdvertisementTierDto"/> if the operation
    /// succeeds, or an error message if it fails.</returns>
    public async Task<IBaseResult<AdvertisementTierDto>> AdvertisementTierAsync(string id, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<AdvertisementTier>(a => a.Id == id);
        spec.AddInclude(i => i.Include(c => c.Images).ThenInclude(c => c.Image));

        var result = await repository.FirstOrDefaultAsync(spec, false, cancellationToken);
        if (!result.Succeeded)
            return await Result<AdvertisementTierDto>.FailAsync(result.Messages.FirstOrDefault() ?? "Failed to query Advertisement Tier.");

        return await Result<AdvertisementTierDto>.SuccessAsync(new AdvertisementTierDto(result.Data));
    }
}
