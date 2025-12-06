using ConectOne.Domain.ResultWrappers;
using FeedbackModule.Domain.DataTransferObjects;
using FilingModule.Domain.RequestFeatures;

namespace AccomodationModule.Domain.Interfaces;

/// <summary>
/// Provides methods to manage vacation reviews.
/// </summary>
public interface IVacationReviewService
{
    /// <summary>
    /// Retrieves all reviews linked to a specific vacation.
    /// </summary>
    /// <param name="vacationId">The identity of the vacation.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of reviews for the vacation.</returns>
    Task<IBaseResult<IEnumerable<ReviewDto>>> VacationReviewListAsync(string vacationId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a single vacation review by its review identifier.
    /// </summary>
    /// <param name="reviewId">The identity of the review.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The review.</returns>
    Task<IBaseResult<ReviewDto>> VacationReviewAsync(string reviewId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new vacation review.
    /// </summary>
    /// <param name="dto">The data transfer object containing review data.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<IBaseResult> CreateVacationReviewAsync(ReviewDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing vacation review.
    /// </summary>
    /// <param name="dto">The data transfer object containing review data.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<IBaseResult> UpdateVacationReviewAsync(ReviewDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a vacation review.
    /// </summary>
    /// <param name="reviewId">The identity of the review to remove.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<IBaseResult> RemoveVacationReviewAsync(string reviewId, CancellationToken cancellationToken = default);

    #region Images

    /// <summary>
    /// Adds an image to a vacation entity.
    /// </summary>
    /// <remarks>This method allows adding an image to a vacation entity, which can be used to enhance
    /// the entity's details with visual content.</remarks>
    /// <param name="request">The request containing the image and associated metadata to be added to the vacation entity. Cannot be null.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the operation.</returns>
    Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a vacation image identified by the specified image ID.
    /// </summary>
    /// <param name="imageId">The unique identifier of the image to be removed. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the operation.</returns>
    Task<IBaseResult> RemoveImage(string imageId, CancellationToken cancellationToken = default);

    #endregion
}
