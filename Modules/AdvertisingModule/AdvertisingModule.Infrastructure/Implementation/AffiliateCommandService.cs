using AdvertisingModule.Domain.DataTransferObjects;
using AdvertisingModule.Domain.Entities;
using AdvertisingModule.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using FilingModule.Domain.Entities;
using FilingModule.Domain.RequestFeatures;

namespace AdvertisingModule.Infrastructure.Implementation;

/// <summary>
/// Provides methods for managing affiliates, including creating, updating, and removing affiliates, as well as managing
/// associated images. This service acts as a command layer for affiliate-related operations.
/// </summary>
/// <remarks>This service is designed to handle operations related to affiliates and their associated images. It
/// ensures that changes are persisted to the underlying data store and provides appropriate feedback on the success or
/// failure of operations. Use this service to perform create, update, and delete operations on affiliates, as well as
/// to manage images linked to affiliates.</remarks>
/// <param name="repository"></param>
/// <param name="imageRepository"></param>
public class AffiliateCommandService(IRepository<Affiliate, string> repository, IRepository<EntityImage<Affiliate, string>, string> imageRepository) : IAffiliateCommandService
{
    /// <summary>
    /// Creates a new affiliate advertisement asynchronously.
    /// </summary>
    /// <remarks>This method validates the provided affiliate data, creates a new advertisement entity, and
    /// saves it to the repository. If the operation fails at any stage, the result will include the corresponding error
    /// messages.</remarks>
    /// <param name="affiliate">The affiliate data to create, including the title, description, and URL.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> with
    /// the created <see cref="AffiliateDto"/> if the operation succeeds, or error messages if it fails.</returns>
    public async Task<IBaseResult<AffiliateDto>> CreateAsync(AffiliateDto affiliate, CancellationToken cancellationToken = default)
    {
        var affiliateList = await repository.ListAsync(false, cancellationToken);
        var count = affiliateList.Data.Count;

        var entity = new Affiliate()
        {
            Title = affiliate.Title,
            Description = affiliate.Description,
            Url = affiliate.Url,
            Featured = affiliate.Featured,
            DisplayOrder = count + 1
        };
        var result = await repository.CreateAsync(entity, cancellationToken);
        if (!result.Succeeded)
            return await Result<AffiliateDto>.FailAsync(result.Messages);

        var save = await repository.SaveAsync(cancellationToken);
        if (!save.Succeeded)
            return await Result<AffiliateDto>.FailAsync(save.Messages);

        return await Result<AffiliateDto>.SuccessAsync(new AffiliateDto(result.Data));
    }

    /// <summary>
    /// Updates an existing affiliate entity with the provided data.
    /// </summary>
    /// <remarks>This method attempts to update an existing affiliate entity in the repository. The operation
    /// will fail if the specified affiliate does not exist. Ensure that the <see cref="AffiliateDto.Id"/> property
    /// corresponds to a valid entity in the repository.</remarks>
    /// <param name="affiliate">The data transfer object containing the updated affiliate information. The <see cref="AffiliateDto.Id"/>
    /// property must match an existing entity.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the outcome of the update operation. If the update is successful, the result will indicate success.
    /// If the affiliate is not found or the update fails, the result will indicate failure with appropriate messages.</returns>
    public async Task<IBaseResult> UpdateAsync(AffiliateDto affiliate, CancellationToken cancellationToken = default)
    {
        var queryResult = await repository.FindByConditionAsync(a => a.Id == affiliate.Id!, true, cancellationToken);
        var entity = queryResult.Succeeded ? queryResult.Data.FirstOrDefault() : null;
        if (entity == null)
            return await Result.FailAsync("Affiliate not found.");

        entity.Title = affiliate.Title;
        entity.Description = affiliate.Description;
        entity.Url = affiliate.Url;
        entity.Featured = affiliate.Featured;

        var update = repository.Update(entity);
        if (!update.Succeeded)
            return await Result.FailAsync(update.Messages);

        return await repository.SaveAsync(cancellationToken);
    }

    /// <summary>
    /// Updates the display order and selector values for vacation pricing information based on the provided affiliate
    /// order update request.
    /// </summary>
    /// <remarks>This method retrieves the vacation pricing items associated with the specified affiliate,
    /// updates their display order and selector values based on the provided request, and saves the changes to the
    /// repository. If the save operation fails, the result will contain the failure messages.</remarks>
    /// <param name="dto">The request object containing the affiliate ID and the updated display order and selector values for each
    /// vacation pricing item.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the operation.</returns>
    public async Task<IBaseResult> UpdateAffiliateDisplayOrderAsync(AffiliateOrderUpdateRequest dto, CancellationToken cancellationToken = default)
    {
        var collection = await repository.ListAsync(false, cancellationToken);

        foreach (var item in collection.Data)
        {
            var itemToUpdate = dto.Items.FirstOrDefault(c => c.Id == item.Id);
            if (itemToUpdate is not null)
            {
                item.DisplayOrder = itemToUpdate.DisplayOrder;
            }

            repository.Update(item);
        }

        var saveResult = await repository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

        return await Result.SuccessAsync($"Affiliates display order was updated successfully");
    }

    /// <summary>
    /// Removes an affiliate by its identifier asynchronously.
    /// </summary>
    /// <remarks>If the operation fails, the returned result will contain error messages describing the
    /// failure.</remarks>
    /// <param name="affiliateId">The unique identifier of the affiliate to remove. Cannot be <see langword="null"/> or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the outcome of the operation.</returns>
    public async Task<IBaseResult> RemoveAsync(string affiliateId, CancellationToken cancellationToken = default)
    {
        var result = await repository.DeleteAsync(affiliateId, cancellationToken);
        if (!result.Succeeded) return await Result.FailAsync(result.Messages);

        var saveResult = await repository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

        return result;
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
        var image = new EntityImage<Affiliate, string>(request.ImageId, request.EntityId) { Selector = request.Selector, Order = request.Order };

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
