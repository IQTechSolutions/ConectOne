using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using FeedbackModule.Domain.DataTransferObjects;
using FeedbackModule.Domain.Entities;
using FilingModule.Domain.Entities;
using FilingModule.Domain.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace AccomodationModule.Infrastructure.Implimentation;

/// <summary>
/// Provides operations for linking <see cref="Review{TEntity}"/> entities to <see cref="Vacation"/> instances.
/// </summary>
public class VacationReviewService(IAccomodationRepositoryManager accomodationRepositoryManager, IRepository<EntityImage<Lodging, string>, string> imageRepository) : IVacationReviewService
{
    /// <summary>
    /// Retrieves a list of reviews associated with a specific vacation.
    /// </summary>
    /// <remarks>The method retrieves reviews for the specified vacation, including associated images. If the
    /// operation is unsuccessful, the result will include the failure messages.</remarks>
    /// <param name="vacationId">The unique identifier of the vacation for which reviews are to be retrieved.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// where <c>T</c> is an <see cref="IEnumerable{T}"/> of <see cref="ReviewDto"/> objects representing the reviews.
    /// If the operation fails, the result contains error messages.</returns>
    public async Task<IBaseResult<IEnumerable<ReviewDto>>> VacationReviewListAsync(string vacationId, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<VacationReview>(r => r.VacationId == vacationId);
        spec.AddInclude(i => i.Include(r => r.Review).ThenInclude(ii => ii.Images).ThenInclude(ii => ii.Image));

        var result = await accomodationRepositoryManager.VacationReviews.ListAsync(spec, false, cancellationToken);
        if (!result.Succeeded)
            return await Result<IEnumerable<ReviewDto>>.FailAsync(result.Messages);

        var dtos = result.Data.Select(vr => ReviewDto.ToReviewDto(vr.Review!));
        return await Result<IEnumerable<ReviewDto>>.SuccessAsync(dtos);
    }

    /// <inheritdoc />
    public async Task<IBaseResult<ReviewDto>> VacationReviewAsync(string reviewId, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<VacationReview>(r => r.ReviewId == reviewId);
        spec.AddInclude(i => i.Include(r => r.Review).ThenInclude(ii => ii.Images).ThenInclude(ii => ii.Image));

        var result = await accomodationRepositoryManager.VacationReviews.FirstOrDefaultAsync(spec, false, cancellationToken);
        if (!result.Succeeded || result.Data == null)
            return await Result<ReviewDto>.FailAsync(result.Messages);

        return await Result<ReviewDto>.SuccessAsync(ReviewDto.ToReviewDto(result.Data.Review!));
    }

    /// <inheritdoc />
    public async Task<IBaseResult> CreateVacationReviewAsync(ReviewDto dto, CancellationToken cancellationToken = default)
    {
        var review = dto.ToReview<Vacation>();
        var createResult = await accomodationRepositoryManager.Reviews.CreateAsync(review, cancellationToken);
        if (!createResult.Succeeded)
            return await Result.FailAsync(createResult.Messages);

        var saveReview = await accomodationRepositoryManager.Reviews.SaveAsync(cancellationToken);
        if (!saveReview.Succeeded)
            return await Result.FailAsync(saveReview.Messages);

        var link = new VacationReview { VacationId = dto.EntityId, ReviewId = review.Id };
        await accomodationRepositoryManager.VacationReviews.CreateAsync(link, cancellationToken);
        var saveLink = await accomodationRepositoryManager.VacationReviews.SaveAsync(cancellationToken);
        if (!saveLink.Succeeded)
            return await Result.FailAsync(saveLink.Messages);

        return await Result.SuccessAsync("Vacation review was created successfully");
    }

    /// <inheritdoc />
    public async Task<IBaseResult> UpdateVacationReviewAsync(ReviewDto dto, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<VacationReview>(r => r.ReviewId == dto.Id);
        spec.AddInclude(i => i.Include(r => r.Review));

        var result = await accomodationRepositoryManager.VacationReviews.FirstOrDefaultAsync(spec, true, cancellationToken);
        if (!result.Succeeded || result.Data == null)
            return await Result.FailAsync(result.Messages);

        var review = result.Data.Review!;
        review.Name = dto.Name;
        review.JobTitle = dto.JobTitle;
        review.CompanyName = dto.CompanyName;
        review.Location = dto.Location;
        review.ReviewText = dto.ReviewText;
        review.EntityId = dto.EntityId;

        var updateResult = accomodationRepositoryManager.Reviews.Update(review);
        if (!updateResult.Succeeded)
            return await Result.FailAsync(updateResult.Messages);

        result.Data.VacationId = dto.EntityId;
        accomodationRepositoryManager.VacationReviews.Update(result.Data);

        var saveReview = await accomodationRepositoryManager.Reviews.SaveAsync(cancellationToken);
        if (!saveReview.Succeeded)
            return await Result.FailAsync(saveReview.Messages);

        var saveLink = await accomodationRepositoryManager.VacationReviews.SaveAsync(cancellationToken);
        if (!saveLink.Succeeded)
            return await Result.FailAsync(saveLink.Messages);

        return await Result.SuccessAsync("Vacation review updated successfully");
    }

    /// <inheritdoc />
    public async Task<IBaseResult> RemoveVacationReviewAsync(string reviewId, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<VacationReview>(r => r.ReviewId == reviewId);
        var linkResult = await accomodationRepositoryManager.VacationReviews.FirstOrDefaultAsync(spec, true, cancellationToken);
        if (linkResult.Succeeded && linkResult.Data != null)
        {
            accomodationRepositoryManager.VacationReviews.Delete(linkResult.Data);
            var saveLink = await accomodationRepositoryManager.VacationReviews.SaveAsync(cancellationToken);
            if (!saveLink.Succeeded)
                return await Result.FailAsync(saveLink.Messages);
        }

        var deleteResult = await accomodationRepositoryManager.Reviews.DeleteAsync(reviewId, cancellationToken);
        if (!deleteResult.Succeeded)
            return await Result.FailAsync(deleteResult.Messages);

        var saveReview = await accomodationRepositoryManager.Reviews.SaveAsync(cancellationToken);
        if (!saveReview.Succeeded)
            return await Result.FailAsync(saveReview.Messages);

        return await Result.SuccessAsync($"Vacation review with id '{reviewId}' was successfully removed");
    }

    #region Images

    /// <summary>
    /// Adds an image to the specified entity.
    /// </summary>
    /// <remarks>This method creates an image entity and attempts to save it to the repository. If the
    /// operation fails at any step, it returns a failure result with the associated error messages.</remarks>
    /// <param name="request">The request containing the image and entity details. Must not be null.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the operation.</returns>
    public async Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default)
    {
        var image = new EntityImage<Lodging, string>(request.ImageId, request.EntityId) { Selector = request.Selector, Order = request.Order };

        var addResult = await imageRepository.CreateAsync(image, cancellationToken);
        if (!addResult.Succeeded) return await Result.FailAsync(addResult.Messages);

        var saveResult = await imageRepository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

        return await Result.SuccessAsync();
    }

    /// <summary>
    /// Removes an image identified by the specified image ID from the repository.
    /// </summary>
    /// <remarks>This method attempts to delete the image from the repository and then save the
    /// changes. If either operation fails, the method returns a failure result with the associated error
    /// messages.</remarks>
    /// <param name="imageId">The unique identifier of the image to be removed. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the operation.</returns>
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
