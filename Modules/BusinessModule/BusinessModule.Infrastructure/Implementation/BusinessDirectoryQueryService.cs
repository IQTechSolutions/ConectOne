using BusinessModule.Domain.DataTransferObjects;
using BusinessModule.Domain.Entities;
using BusinessModule.Domain.Interfaces;
using BusinessModule.Domain.RequestFeatures;
using BusinessModule.Infrastructure.Specifications;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusinessModule.Infrastructure.Implementation;

/// <summary>
/// Provides query operations for retrieving active business listings.
/// </summary>
public class BusinessDirectoryQueryService(IRepository<BusinessListing, string> repository) : IBusinessDirectoryQueryService
{
    /// <summary>
    /// Retrieves a paginated list of business listings, including associated images and categories.
    /// </summary>
    /// <remarks>This method fetches business listings from the repository and includes related images and
    /// categories  in the result. The returned data is mapped to <see cref="BusinessListingDto"/> objects.</remarks>
    /// <param name="pageParameters">The pagination parameters, including the page number and page size.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="PaginatedResult{T}"/> containing a collection of <see cref="BusinessListingDto"/> objects 
    /// representing the business listings for the specified page, along with pagination metadata.</returns>
    public async Task<PaginatedResult<BusinessListingDto>> PagedListingsAsync(BusinessListingPageParameters pageParameters, CancellationToken cancellationToken = default)
    {
        var result = await repository.ListAsync(new BusinessListingSpecification(pageParameters), trackChanges: false, cancellationToken);
        if (!result.Succeeded)
            return PaginatedResult<BusinessListingDto>.Failure(result.Messages);

        var resultItems = result.Data;

        if (!string.IsNullOrEmpty(pageParameters.CategoryId))
            resultItems = resultItems.Where(c => c.Categories.Any(c => c.CategoryId == pageParameters.CategoryId)).ToList();

        if (!string.IsNullOrEmpty(pageParameters.UserId))
        {
            resultItems = resultItems.Where(c => c.UserId == pageParameters.UserId).ToList();
        }

        var page = resultItems.Select(v => new BusinessListingDto(v)).ToList();
        return PaginatedResult<BusinessListingDto>.Success(page, result.Data.Count, pageParameters.PageNr, pageParameters.PageSize);
    }

    /// <summary>
    /// Retrieves all active business listings that have been approved.
    /// </summary>
    /// <remarks>The listings are ordered by their tier before being returned. If the query fails,  the result
    /// will indicate failure and include an appropriate error message.</remarks>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an  <IBaseResult{T}/> of
    /// <IEnumerable{T}/> containing  <BusinessListingDto/> objects representing the active business listings.</returns>
    public async Task<IBaseResult<IEnumerable<BusinessListingDto>>> ActiveListingsAsync(CancellationToken cancellationToken = default)
    {
        var result = await repository.ListAsync(new DetailedBusinessListingSpecification(), false, cancellationToken);
        if (!result.Succeeded)
            return await Result<IEnumerable<BusinessListingDto>>.FailAsync(result.Messages.FirstOrDefault() ?? "Failed to query listings.");
        
        var listings = result.Data.OrderBy(l => l.ListingTier?.Order).ToList();
        return await Result<IEnumerable<BusinessListingDto>>.SuccessAsync(listings.Select(l => new BusinessListingDto(l)));
    }
    
    /// <summary>
    /// Retrieves a business listing by its identifier, including related categories and images.
    /// </summary>
    /// <remarks>This method queries the repository for a business listing and includes its associated
    /// categories and images. If the listing is not found or the query fails, the result will indicate failure with an
    /// appropriate error message.</remarks>
    /// <param name="id">The unique identifier of the business listing to retrieve.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> of
    /// type <see cref="BusinessListingDto"/>. If the operation succeeds, the result contains the business listing data;
    /// otherwise, it contains an error message.</returns>
    public async Task<IBaseResult<BusinessListingDto>> ListingAsync(string id, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<BusinessListing>(c => c.Id == id);
        spec.AddInclude(l => l.Include(c => c.ListingTier));
        spec.AddInclude(l => l.Include(c => c.Categories).ThenInclude(c => c.Category));
        spec.AddInclude(l => l.Include(c => c.Images).ThenInclude(c => c.Image));
        spec.AddInclude(l => l.Include(c => c.Products).ThenInclude(c => c.Images).ThenInclude(c => c.Image));
        spec.AddInclude(l => l.Include(c => c.Services).ThenInclude(c => c.Images).ThenInclude(c => c.Image));

        var result = await repository.FirstOrDefaultAsync(spec, false, cancellationToken);
        if (!result.Succeeded)
            return await Result<BusinessListingDto>.FailAsync(result.Messages.FirstOrDefault() ?? "Failed to query listing.");
        return await Result<BusinessListingDto>.SuccessAsync(new BusinessListingDto(result.Data));
    }
}