using AdvertisingModule.Domain.DataTransferObjects;
using AdvertisingModule.Domain.Entities;
using AdvertisingModule.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using FilingModule.Domain.Entities;
using FilingModule.Domain.RequestFeatures;

namespace AdvertisingModule.Infrastructure.Implementation;

/// <summary>
/// Command service for managing advertisement tiers and their images.
/// </summary>
/// <remarks>
/// Provides create, update, delete, and image association operations using repository abstractions.
/// </remarks>
public class AdvertisementTierCommandService(IRepository<AdvertisementTier, string> repository, IRepository<EntityImage<AdvertisementTier, string>, string> imageRepository) : IAdvertisementTierCommandService
{
    /// <summary>
    /// Creates a new advertisement tier.
    /// </summary>
    /// <param name="advertisementTier">The DTO representing the advertisement tier to create. If null, a default entity is created.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>
    /// A result containing the created <see cref="AdvertisementTierDto"/> when successful; otherwise, a failure result with messages.
    /// </returns>
    public async Task<IBaseResult<AdvertisementTierDto>> CreateAsync(AdvertisementTierDto advertisementTier, CancellationToken cancellationToken = default)
    {
        var entity = advertisementTier.ToEntity();
        var result = await repository.CreateAsync(entity, cancellationToken);
        if (!result.Succeeded)
            return await Result<AdvertisementTierDto>.FailAsync(result.Messages);

        var save = await repository.SaveAsync(cancellationToken);
        if (!save.Succeeded)
            return await Result<AdvertisementTierDto>.FailAsync(save.Messages);

        return await Result<AdvertisementTierDto>.SuccessAsync(new AdvertisementTierDto(result.Data));
    }

    /// <summary>
    /// Updates an existing advertisement tier.
    /// </summary>
    /// <param name="advertisementTier">The DTO containing updated values for the advertisement tier.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>
    /// A result indicating success or failure of the update operation.
    /// </returns>
    public async Task<IBaseResult> UpdateAsync(AdvertisementTierDto advertisementTier, CancellationToken cancellationToken = default)
    {
        var queryResult = await repository.FindByConditionAsync(a => a.Id == advertisementTier.Id, true, cancellationToken);
        var entity = queryResult.Succeeded ? queryResult.Data.FirstOrDefault() : null;
        if (entity == null)
            return await Result.FailAsync("Affiliate not found.");

        entity.Name = advertisementTier.Name;
        entity.ShortDescription = advertisementTier.ShortDescription;
        entity.Description = advertisementTier.Description;
        entity.AvailabilityCount = advertisementTier.AvailabilityCount;
        entity.Days = advertisementTier.Days;
        entity.Order = advertisementTier.Order;
        entity.Price = advertisementTier.Price;
        entity.AdvertisementType = advertisementTier.AdvertisementType; 

        var update = repository.Update(entity);
        if (!update.Succeeded)
            return await Result.FailAsync(update.Messages);

        return await repository.SaveAsync(cancellationToken);
    }

    /// <summary>
    /// Removes an advertisement tier by its identifier.
    /// </summary>
    /// <param name="advertisementTierId">The unique identifier of the advertisement tier to remove.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>
    /// A result indicating success or failure of the delete operation.
    /// </returns>
    public async Task<IBaseResult> RemoveAsync(string advertisementTierId, CancellationToken cancellationToken = default)
    {
        var result = await repository.DeleteAsync(advertisementTierId, cancellationToken);
        if (!result.Succeeded)
            return await Result.FailAsync(result.Messages);
        return await repository.SaveAsync(cancellationToken);
    }

    #region Images

    /// <summary>
    /// Adds an image to the specified entity with the provided details.
    /// </summary>
    /// <remarks>This method creates a new image entity and associates it with the specified entity. The
    /// operation involves creating the image in the repository and saving the changes. If the operation fails at any
    /// step, the result will contain the corresponding error messages.</remarks>
    /// <param name="request">The request containing the image details, including the image ID, entity ID, selector, and order.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the operation.</returns>
    public async Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default)
    {
        var image = new EntityImage<AdvertisementTier, string>(request.ImageId, request.EntityId) { Selector = request.Selector, Order = request.Order };

        var addResult = await imageRepository.CreateAsync(image, cancellationToken);
        if (!addResult.Succeeded) return await Result.FailAsync(addResult.Messages);

        var saveResult = await imageRepository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

        return await Result.SuccessAsync();
    }

    /// <summary>
    /// Removes an image with the specified identifier from the repository.
    /// </summary>
    /// <remarks>This method first attempts to delete the image from the repository. If the deletion succeeds,
    /// it then saves the changes to the repository. If either operation fails, the method returns a failure result
    /// containing the associated error messages.</remarks>
    /// <param name="imageId">The unique identifier of the image to be removed. Cannot be <see langword="null"/> or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the outcome of the operation. If the operation succeeds, the result will indicate success. Otherwise,
    /// it will contain error messages describing the failure.</returns>
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
