using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using FeedbackModule.Domain.DataTransferObjects;
using FeedbackModule.Domain.RequestFeatures;
using FilingModule.Domain.RequestFeatures;

namespace AccomodationModule.Application.RestServices
{
    /// <summary>
    /// Provides methods for managing vacation reviews and associated images through RESTful API calls.
    /// </summary>
    /// <remarks>This service allows clients to perform operations such as retrieving vacation reviews,
    /// creating or updating reviews,  and managing images associated with vacation entities. All methods are
    /// asynchronous and rely on an HTTP provider  to communicate with the underlying API endpoints.</remarks>
    /// <param name="provider"></param>
    public class VacationReviewRestService(IBaseHttpProvider provider) : IVacationReviewService
    {
        /// <summary>
        /// Retrieves a list of reviews associated with the specified vacation.
        /// </summary>
        /// <remarks>This method fetches reviews in a paged format from the underlying data provider.
        /// Ensure that the <paramref name="vacationId"/>  corresponds to a valid vacation entity in the
        /// system.</remarks>
        /// <param name="vacationId">The unique identifier of the vacation for which reviews are to be retrieved. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// containing an enumerable collection of <see cref="ReviewDto"/> objects representing the reviews for the
        /// specified vacation.</returns>
        public async Task<IBaseResult<IEnumerable<ReviewDto>>> VacationReviewListAsync(string vacationId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<ReviewDto>>($"reviews/paged?{(new ReviewPageParameters() { EntityId = vacationId })}");
            return result;
        }

        /// <summary>
        /// Retrieves the details of a vacation review based on the specified review ID.
        /// </summary>
        /// <remarks>Use this method to retrieve detailed information about a specific vacation review.
        /// Ensure that the <paramref name="reviewId"/> is valid and corresponds to an existing review.</remarks>
        /// <param name="reviewId">The unique identifier of the review to retrieve. This value cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that wraps the <see cref="ReviewDto"/> containing the review details.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IBaseResult<ReviewDto>> VacationReviewAsync(string reviewId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new vacation review based on the provided data.
        /// </summary>
        /// <remarks>Use this method to asynchronously create a vacation review. Ensure that the <paramref
        /// name="dto"/> parameter  contains valid data before calling this method. The operation can be canceled by
        /// passing a cancellation token.</remarks>
        /// <param name="dto">The data transfer object containing the details of the vacation review to create.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IBaseResult> CreateVacationReviewAsync(ReviewDto dto, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates the details of a vacation review.
        /// </summary>
        /// <remarks>Use this method to update the details of an existing vacation review. Ensure that the
        /// <paramref name="dto"/>  contains valid data before calling this method. The operation may be canceled by
        /// passing a cancellation token.</remarks>
        /// <param name="dto">The data transfer object containing the updated review details.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the update operation.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IBaseResult> UpdateVacationReviewAsync(ReviewDto dto, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes a vacation review identified by the specified review ID.
        /// </summary>
        /// <param name="reviewId">The unique identifier of the review to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IBaseResult> RemoveVacationReviewAsync(string reviewId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds an image to an entity based on the specified request.
        /// </summary>
        /// <remarks>This method sends the image data to the "lodgings/addImage" endpoint. Ensure that the
        /// request object is properly populated before calling this method.</remarks>
        /// <param name="request">The request containing the details of the image to be added, including the entity identifier and image data.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync("lodgings/addImage", request);
            return result;
        }

        /// <summary>
        /// Removes an image with the specified identifier.
        /// </summary>
        /// <remarks>This method sends a request to delete the image identified by <paramref
        /// name="imageId"/>.  Ensure that the provided identifier corresponds to an existing image.</remarks>
        /// <param name="imageId">The unique identifier of the image to be removed. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveImage(string imageId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync("lodgings/deleteImage", imageId);
            return result;
        }
    }
}
