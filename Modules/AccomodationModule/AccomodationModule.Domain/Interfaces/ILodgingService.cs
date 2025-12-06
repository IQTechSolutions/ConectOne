using AccomodationModule.Domain.Arguments.Response;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.RequestFeatures;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.RequestFeatures;

namespace AccomodationModule.Domain.Interfaces
{
    /// <summary>
    /// Interface for managing lodging-related operations.
    /// </summary>
    public interface ILodgingService
    {
        #region Lodging List Requests

        /// <summary>
        /// Creates a lodging list request based on the provided model.
        /// </summary>
        /// <remarks>The method processes the provided <paramref name="model"/> to create a lodging list
        /// request and returns the result. Ensure that the <paramref name="model"/> contains valid data before calling
        /// this method.</remarks>
        /// <param name="model">The data transfer object containing the details of the lodging list request to be created.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>An <see cref="IBaseResult"/> containing the result of the lodging list request creation operation.</returns>
        Task<IBaseResult<LodgingListingRequestDto>> CreateLodgingListReqeust(LodgingListingRequestDto model, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a paginated list of lodging listing requests based on the provided page parameters.
        /// </summary>
        /// <param name="pageParameters">The parameters for pagination and filtering.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A paginated result containing lodging listing request DTOs.</returns>
        Task<PaginatedResult<LodgingListingRequestDto>> PagedLodgingListReqeusts(LodgingListingRequestParameters pageParameters, CancellationToken cancellationToken);

        #endregion

        /// <summary>
        /// Retrieves the total count of lodgings.
        /// </summary>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result containing the count of lodgings.</returns>
        Task<IBaseResult<LodgingCountResponse>> LodgingCountAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a paginated list of lodgings based on the provided page parameters.
        /// </summary>
        /// <param name="pageParameters">The parameters for pagination and filtering.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A paginated result containing lodging DTOs.</returns>
        Task<PaginatedResult<LodgingDto>> PagedLodgingsAsync(LodgingParameters pageParameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all lodgings, optionally filtered by category ID.
        /// </summary>
        /// <param name="categoryId">The category ID to filter by (optional).</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result containing a list of lodging DTOs.</returns>
        Task<IBaseResult<List<LodgingDto>>> AllLodgings(string? categoryId = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a specific lodging by product ID.
        /// </summary>
        /// <param name="productId">The product ID of the lodging to retrieve.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result containing the lodging DTO.</returns>
        Task<IBaseResult<LodgingDto>> LodgingAsync(string productId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a specific lodging by unique service ID.
        /// </summary>
        /// <param name="bbid">The unique service ID of the lodging to retrieve.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result containing the lodging DTO.</returns>
        Task<IBaseResult<LodgingDto>> ProductByUniqueServiceIdAsync(string bbid, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new lodging.
        /// </summary>
        /// <param name="nbProduct">The lodging DTO containing the details of the new lodging.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result containing the created lodging DTO.</returns>
        Task<IBaseResult<LodgingDto>> CreateLodgingAsync(LodgingDto nbProduct, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing lodging.
        /// </summary>
        /// <param name="product">The lodging DTO containing the updated details.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        Task<IBaseResult> UpdateLodgingAsync(LodgingDto product, CancellationToken cancellationToken = default);


        /// <summary>
        /// Removes a specific lodging by ID.
        /// </summary>
        /// <param name="id">The ID of the lodging to remove.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        Task<IBaseResult> RemoveLodgingAsync(string id, CancellationToken cancellationToken = default);

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
        Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default);

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
        Task<IBaseResult> RemoveImage(string imageId, CancellationToken cancellationToken = default);

        #endregion

        #region Videos

        /// <summary>
        /// Adds a video to the lodging entity.
        /// </summary>
        /// <remarks>This method attempts to add a video to the specified lodging entity. It first creates
        /// the video entity and then saves it to the repository. If either the creation or saving operation fails, the
        /// method returns a failure result with the associated error messages.</remarks>
        /// <param name="request">The request containing the video details to be added, including the video ID and entity ID.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
        Task<IBaseResult> AddVideo(AddEntityVideoRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a video identified by the specified video ID.
        /// </summary>
        /// <remarks>This method attempts to delete the video from the repository and save the changes. If
        /// either operation fails, the method returns a failure result with the associated error messages.</remarks>
        /// <param name="videoId">The unique identifier of the video to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
        Task<IBaseResult> RemoveVideo(string videoId, CancellationToken cancellationToken = default);

        #endregion

        #region Cancellation Rules

        /// <summary>
        /// Creates a new cancellation rule.
        /// </summary>
        /// <param name="cancellation">The cancellation rule DTO containing the details of the new rule.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        Task<IBaseResult> CreateCancellationRule(CancellationRuleDto cancellation, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a specific cancellation rule by ID.
        /// </summary>
        /// <param name="id">The ID of the cancellation rule to remove.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        Task<IBaseResult> RemoveCancellationRule(int id, CancellationToken cancellationToken = default);

        #endregion

        /// <summary>
        /// Retrieves a collection of featured images for a specific lodging.
        /// </summary>
        /// <param name="lodgingId">The ID of the lodging to retrieve featured images for.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result containing a collection of featured image DTOs.</returns>
        Task<IBaseResult<ICollection<FeaturedImageDto>>> FeaturedImages(string lodgingId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sets a specific image as the featured cover image for a lodging.
        /// </summary>
        /// <param name="lodgingId">The ID of the lodging to set the cover image for.</param>
        /// <param name="featuredImageId">The ID of the image to set as the cover image.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        Task<IBaseResult> SetFeaturedCoverImage(string lodgingId, string featuredImageId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes the featured cover image for a lodging.
        /// </summary>
        /// <param name="featuredImageId">The ID of the featured image to remove.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        Task<IBaseResult> RemoveFeaturedCoverImage(string featuredImageId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a new featured image for a lodging.
        /// </summary>
        /// <param name="dto">The featured image DTO containing the details of the new image.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result containing the created featured image DTO.</returns>
        Task<IBaseResult<FeaturedImageDto>> AddFeaturedImage(FeaturedImageDto dto, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Removes a specific featured lodging image by ID.
        /// </summary>
        /// <param name="featuredImageId">The ID of the featured image to remove.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        Task<IBaseResult> RemoveFeaturedLodgingImageAsync(string featuredImageId, CancellationToken cancellationToken = default);
    }
}
