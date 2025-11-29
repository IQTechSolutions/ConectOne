using AdvertisingModule.Domain.DataTransferObjects;
using AdvertisingModule.Domain.Entities;
using AdvertisingModule.Domain.Interfaces;
using ConectOne.Domain.Enums;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using FilingModule.Domain.Entities;
using FilingModule.Domain.RequestFeatures;

namespace AdvertisingModule.Infrastructure.Implementation;

/// <summary>
/// Provides methods for managing advertisements, including creating, updating, removing, and approving advertisements.
/// </summary>
/// <remarks>This service interacts with a repository to perform CRUD operations on advertisements. It ensures
/// that changes are persisted and provides feedback on the success or failure of operations. Use this service to manage
/// advertisement data in a consistent and reliable manner.</remarks>
/// <param name="repository"></param>
public class AdvertisementCommandService(IRepository<Advertisement,string> repository, IRepository<EntityImage<Advertisement, string>, string> imageRepository) : IAdvertisementCommandService
{
    /// <summary>
    /// Asynchronously creates a new advertisement and persists it to the repository.
    /// </summary>
    /// <remarks>This method validates and saves the provided advertisement data to the repository. If the
    /// operation fails at any stage, the returned result will include the failure messages. The repository's save
    /// operation is also invoked to ensure the changes are persisted.</remarks>
    /// <param name="advertisement">The advertisement data to create, including title, description, URL, and other properties.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> with
    /// the created <see cref="AdvertisementDto"/> if the operation succeeds, or error messages if it fails.</returns>
    public async Task<IBaseResult<AdvertisementDto>> CreateAsync(AdvertisementDto advertisement, CancellationToken cancellationToken = default)
    {
        var entity = new Advertisement
        {
            Id = string.IsNullOrWhiteSpace(advertisement.Id) ? null : advertisement.Id,
            Title = advertisement.Title,
            Description = advertisement.Description,
            Url = advertisement.Url,
            StartDate = advertisement.StartDate,
            EndDate = advertisement.EndDate,
            Status = advertisement.Status,
            SetupCompleted = advertisement.SetupCompleted,
            AdvertisementType = advertisement.AdvertisementType,
            AdvertisementTierId = advertisement.AdvertisementTier?.Id,
            UserId = advertisement.UserId
        };
        var result = await repository.CreateAsync(entity, cancellationToken);
        if (!result.Succeeded)
            return await Result<AdvertisementDto>.FailAsync(result.Messages);

        var save = await repository.SaveAsync(cancellationToken);
        if (!save.Succeeded)
            return await Result<AdvertisementDto>.FailAsync(save.Messages);

        return await Result<AdvertisementDto>.SuccessAsync(new AdvertisementDto(result.Data));
    }

    /// <summary>
    /// Updates an existing advertisement with the provided data.
    /// </summary>
    /// <remarks>The method attempts to locate the advertisement by its identifier. If the advertisement is
    /// found, its properties are updated with the provided values. The changes are then saved to the repository. If the
    /// advertisement is not found or the update/save operation fails, the method returns a failure result.</remarks>
    /// <param name="advertisement">The advertisement data to update, including the identifier and updated values.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the update operation. If successful, the result reflects the state after
    /// the update. If the advertisement is not found or the update fails, the result contains error messages.</returns>
    public async Task<IBaseResult> UpdateAsync(AdvertisementDto advertisement, CancellationToken cancellationToken = default)
    {
        var queryResult = await repository.FindByConditionAsync(a => a.Id == advertisement.Id!, true, cancellationToken);
        var entity = queryResult.Succeeded ? queryResult.Data.FirstOrDefault() : null;
        if (entity == null)
            return await Result.FailAsync("Advertisement not found.");

        entity.Title = advertisement.Title;
        entity.Description = advertisement.Description;
        entity.Url = advertisement.Url;
        entity.StartDate = advertisement.StartDate;
        entity.EndDate = advertisement.EndDate;
        entity.Status = advertisement.Status;
        entity.SetupCompleted = advertisement.SetupCompleted;
        entity.AdvertisementType = advertisement.AdvertisementType;
        entity.AdvertisementTierId = advertisement.AdvertisementTier?.Id ?? entity.AdvertisementTierId;
        if (!string.IsNullOrWhiteSpace(advertisement.UserId))
        {
            entity.UserId = advertisement.UserId;
        }

        var update = repository.Update(entity);
        if (!update.Succeeded)
            return await Result.FailAsync(update.Messages);

        return await repository.SaveAsync(cancellationToken);
    }

    /// <summary>
    /// Removes an advertisement by its unique identifier asynchronously.
    /// </summary>
    /// <remarks>If the removal operation fails, the returned result will contain failure messages. Ensure to
    /// check the <see cref="IBaseResult.Succeeded"/> property to determine the outcome.</remarks>
    /// <param name="advertisementId">The unique identifier of the advertisement to be removed. Cannot be <see langword="null"/> or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the operation, along with any associated messages.</returns>
    public async Task<IBaseResult> RemoveAsync(string advertisementId, CancellationToken cancellationToken = default)
    {
        var result = await repository.DeleteAsync(advertisementId, cancellationToken);
        if (!result.Succeeded)
            return await Result.FailAsync(result.Messages);
        return await repository.SaveAsync(cancellationToken);
    }

    /// <summary>
    /// Approves the advertisement with the specified identifier.
    /// </summary>
    /// <remarks>This method attempts to find the advertisement by its identifier. If the advertisement is
    /// found, it is marked  as approved and the changes are saved to the repository. If the advertisement is not found
    /// or the update fails,  the result will indicate failure with appropriate error messages.</remarks>
    /// <param name="advertisementId">The unique identifier of the advertisement to approve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
    /// indicating the success or failure of the operation. If successful, the advertisement is marked as approved.</returns>
    public async Task<IBaseResult> ApproveAsync(string advertisementId, CancellationToken cancellationToken = default)
    {
        var spec = await repository.FindByConditionAsync(a => a.Id == advertisementId, true, cancellationToken);
        var entity = spec.Succeeded ? spec.Data.FirstOrDefault() : null;
        if (entity == null)
            return await Result.FailAsync("Advertisement not found.");
        entity.Status = ReviewStatus.Approved;
        var update = repository.Update(entity);
        if (!update.Succeeded)
            return await Result.FailAsync(update.Messages);
        return await repository.SaveAsync(cancellationToken);
    }

    /// <summary>
    /// Rejects the advertisement with the specified identifier by updating its status to "Rejected".
    /// </summary>
    /// <remarks>This method attempts to find the advertisement by its identifier. If the advertisement is
    /// found, its status is updated to "Rejected" and the changes are saved to the repository. If the advertisement is
    /// not found or the update operation fails, the result will indicate failure with appropriate error
    /// messages.</remarks>
    /// <param name="advertisementId">The unique identifier of the advertisement to reject.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the outcome of the operation. If the advertisement is not found, the result will indicate failure.</returns>
    public async Task<IBaseResult> RejectAsync(string advertisementId, CancellationToken cancellationToken = default)
    {
        var spec = await repository.FindByConditionAsync(a => a.Id == advertisementId, true, cancellationToken);
        var entity = spec.Succeeded ? spec.Data.FirstOrDefault() : null;
        if (entity == null)
            return await Result.FailAsync("Advertisement not found.");
        entity.Status = ReviewStatus.Rejected;
        var update = repository.Update(entity);
        if (!update.Succeeded)
            return await Result.FailAsync(update.Messages);
        return await repository.SaveAsync(cancellationToken);
    }

    #region Images

    /// <summary>
    /// Adds an image to the specified entity with the provided details.
    /// </summary>
    /// <remarks>This method creates an association between an image and an entity, saving the image details
    /// to the repository. The operation will fail if the repository operations (create or save) are
    /// unsuccessful.</remarks>
    /// <param name="request">The request containing the image details, including the image ID, entity ID, selector, and order.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the operation. If the operation fails, the result includes error messages.</returns>
    public async Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default)
    {
        var image = new EntityImage<Advertisement, string>(request.ImageId, request.EntityId) { Selector = request.Selector, Order = request.Order };

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
    /// it then saves the changes to the repository. If either operation fails, the method returns a failure result with
    /// the associated error messages.</remarks>
    /// <param name="imageId">The unique identifier of the image to be removed. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the operation. If the operation fails, the result includes error messages.</returns>
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
