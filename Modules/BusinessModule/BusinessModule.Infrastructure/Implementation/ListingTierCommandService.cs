using BusinessModule.Domain.DataTransferObjects;
using BusinessModule.Domain.Entities;
using BusinessModule.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using FilingModule.Domain.Entities;
using FilingModule.Domain.RequestFeatures;

namespace BusinessModule.Infrastructure.Implementation;

/// <summary>
/// Provides methods for managing listing tiers, including creating, updating, and removing listing tiers, as well as
/// adding and removing associated images.
/// </summary>
/// <remarks>This service is designed to handle operations related to listing tiers, such as creating new tiers,
/// updating existing ones, and managing their associated images. It interacts with repositories to persist changes and
/// ensures that operations are transactional where applicable.</remarks>
/// <param name="repository"></param>
/// <param name="imageRepository"></param>
public class ListingTierCommandService(IRepository<ListingTier, string> repository, IRepository<EntityImage<ListingTier, string>, string> imageRepository) : IListingTierCommandService
{
    /// <summary>
    /// Creates a new listing tier asynchronously.
    /// </summary>
    /// <remarks>This method attempts to create a new listing tier and save the changes to the repository. If
    /// the operation fails at any stage, the result will include the corresponding error messages.</remarks>
    /// <param name="listingTier">The data transfer object representing the listing tier to be created.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// object that indicates the success or failure of the operation. If successful, the result contains the created
    /// <see cref="ListingTierDto"/> object.</returns>
    public async Task<IBaseResult<ListingTierDto>> CreateAsync(ListingTierDto listingTier, CancellationToken cancellationToken = default)
    {
        var entity = listingTier.ToEntity();

        var countResult = await repository.ListAsync(false, cancellationToken);
        entity.Order = countResult.Succeeded ? countResult.Data.Count + 1 : 1;

        var result = await repository.CreateAsync(entity, cancellationToken);
        if (!result.Succeeded)
            return await Result<ListingTierDto>.FailAsync(result.Messages);

        var save = await repository.SaveAsync(cancellationToken);
        if (!save.Succeeded)
            return await Result<ListingTierDto>.FailAsync(save.Messages);

        return await Result<ListingTierDto>.SuccessAsync(new ListingTierDto(result.Data));
    }

    /// <summary>
    /// Updates an existing listing tier with the specified details.
    /// </summary>
    /// <remarks>The method attempts to find an existing listing tier by its identifier. If the listing tier
    /// is not found, the operation fails with an appropriate error message. If the listing tier is found, its details
    /// are updated and the changes are saved to the repository.</remarks>
    /// <param name="listingTier">The updated details of the listing tier, including its identifier.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the outcome of the update operation. If the update is successful, the result will indicate success;
    /// otherwise, it will contain error messages.</returns>
    public async Task<IBaseResult> UpdateAsync(ListingTierDto listingTier, CancellationToken cancellationToken = default)
    {
        var queryResult = await repository.FindByConditionAsync(a => a.Id == listingTier.Id, true, cancellationToken);
        var entity = queryResult.Succeeded ? queryResult.Data.FirstOrDefault() : null;
        if (entity == null)
            return await Result.FailAsync("Listing Tier not found.");

        entity.Name = listingTier.Name;
        entity.ShortDescription = listingTier.ShortDescription;
        entity.Description = listingTier.Description;
        entity.Order = listingTier.Order;
        entity.Price = listingTier.Price;
        entity.AllowServiceAndProductListing = listingTier.AllowServiceAndProductListing;

        var update = repository.Update(entity);
        if (!update.Succeeded)
            return await Result.FailAsync(update.Messages);

        return await repository.SaveAsync(cancellationToken);
    }

    /// <summary>
    /// Removes a listing tier asynchronously based on the specified identifier.
    /// </summary>
    /// <param name="listingTierId">The unique identifier of the listing tier to be removed. Cannot be <see langword="null"/> or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the outcome of the operation. If the removal fails, the result will include failure messages.</returns>
    public async Task<IBaseResult> RemoveAsync(string listingTierId, CancellationToken cancellationToken = default)
    {
        var result = await repository.DeleteAsync(listingTierId, cancellationToken);
        if (!result.Succeeded)
            return await Result.FailAsync(result.Messages);
        return await repository.SaveAsync(cancellationToken);
    }

    #region Images

    /// <summary>
    /// Adds an image to the specified entity with the provided details.
    /// </summary>
    /// <remarks>This method creates an image entity using the provided details and saves it to the
    /// repository. If the operation fails at any step, the result will include the failure messages.</remarks>
    /// <param name="request">The request containing the image details, including the image ID, entity ID, selector, and order.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the operation.</returns>
    public async Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default)
    {
        var image = new EntityImage<ListingTier, string>(request.ImageId, request.EntityId) { Selector = request.Selector, Order = request.Order };

        var addResult = await imageRepository.CreateAsync(image, cancellationToken);
        if (!addResult.Succeeded) return await Result.FailAsync(addResult.Messages);

        var saveResult = await imageRepository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

        return await Result.SuccessAsync();
    }

    /// <summary>
    /// Removes an image with the specified identifier from the repository.
    /// </summary>
    /// <param name="imageId">The unique identifier of the image to be removed. Cannot be <see langword="null"/> or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the outcome of the operation. Returns a successful result if the image is removed and changes are
    /// saved successfully; otherwise, a failure result with error messages.</returns>
    public async Task<IBaseResult> RemoveImage(string imageId, CancellationToken cancellationToken = default)
    {
        var addResult = await imageRepository.DeleteAsync(imageId, cancellationToken);
        if (!addResult.Succeeded) return await Result.FailAsync(addResult.Messages);

        var saveResult = await imageRepository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

        return await Result.SuccessAsync();
    }

    #endregion
}
