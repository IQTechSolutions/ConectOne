using BusinessModule.Domain.DataTransferObjects;
using BusinessModule.Domain.RequestFeatures;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Mvc;

namespace BusinessModule.Domain.Interfaces;

/// <summary>
/// Defines operations for managing business directory listings, including creation, updates, removal, and approval.
/// </summary>
/// <remarks>This service provides methods to perform CRUD operations on business listings and manage their
/// approval status. Each method returns a result indicating the success or failure of the operation, along with any
/// relevant data or error information.</remarks>
public interface IBusinessDirectoryCommandService
{
    /// <summary>
    /// Creates a new business listing asynchronously.
    /// </summary>
    /// <remarks>The method performs validation on the provided <paramref name="dto"/> and may return  an
    /// error result if the input is invalid or if the creation process fails.</remarks>
    /// <param name="dto">The data transfer object containing the details of the business listing to create.  This parameter cannot be
    /// <see langword="null"/>.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult"/>
    /// object wrapping the created <see cref="BusinessListingDto"/>  if the operation is successful.</returns>
    Task<IBaseResult<BusinessListingDto>> CreateAsync(BusinessListingDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends an enquiry to the owner of a business listing.
    /// </summary>
    /// <param name="request">The request payload containing the enquiry details.</param>
    /// <param name="cancellationToken">An optional cancellation token for the operation.</param>
    /// <returns>A result indicating whether the enquiry was queued successfully.</returns>
    Task<IBaseResult> ContactListingOwnerAsync(ListingContactRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the business listing with the provided data.
    /// </summary>
    /// <remarks>Use this method to update an existing business listing with new information. Ensure that the 
    /// <paramref name="dto"/> parameter contains valid and complete data before calling this method.  The operation can
    /// be canceled by passing a cancellation token.</remarks>
    /// <param name="dto">The data transfer object containing the updated information for the business listing.  This parameter cannot be
    /// null.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the outcome of the update operation.</returns>
    Task<IBaseResult> UpdateAsync(BusinessListingDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Renews the listing identified by <paramref name="listingId"/> for an additional active period.
    /// </summary>
    /// <param name="listingId">The unique identifier of the listing to renew.</param>
    /// <param name="cancellationToken">A token used to observe cancellation requests.</param>
    /// <returns>The refreshed listing details when the operation succeeds.</returns>
    Task<IBaseResult<BusinessListingDto>> RenewAsync(string listingId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a listing identified by the specified <paramref name="listingId"/>.
    /// </summary>
    /// <remarks>Use this method to asynchronously remove a listing from the system. Ensure that the <paramref
    /// name="listingId"/>  is valid and corresponds to an existing listing. The operation can be canceled by passing a 
    /// <see cref="CancellationToken"/>.</remarks>
    /// <param name="listingId">The unique identifier of the listing to be removed. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
    /// indicating the outcome of the removal operation.</returns>
    Task<IBaseResult> RemoveAsync(string listingId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Approves a listing with the specified identifier.
    /// </summary>
    /// <remarks>Use this method to approve a listing asynchronously. Ensure that the <paramref
    /// name="listingId"/> is valid and not null or empty before calling this method. The operation may be canceled by
    /// providing a <paramref name="cancellationToken"/>.</remarks>
    /// <param name="listingId">The unique identifier of the listing to approve. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation will be canceled if the token is triggered.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the outcome of the approval process.</returns>
    Task<IBaseResult> ApproveAsync(string listingId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Rejects the specified listing asynchronously.
    /// </summary>
    /// <remarks>Use this method to reject a listing identified by its unique ID. The operation is performed
    /// asynchronously  and can be canceled by providing a cancellation token.</remarks>
    /// <param name="listingId">The unique identifier of the listing to reject. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
    /// indicating the outcome of the rejection operation.</returns>
    Task<IBaseResult> RejectAsync(string listingId, CancellationToken cancellationToken = default);

    #region Images

    /// <summary>
    /// Adds an image to the specified entity.
    /// </summary>
    /// <remarks>Use this method to associate an image with an existing entity. Ensure that the entity
    /// specified in the request exists before calling this method.</remarks>
    /// <param name="request">The request containing the details of the image to be added, including the entity identifier and image data.
    /// Cannot be null.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the operation.</returns>
    Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes an image with the specified identifier from the system.
    /// </summary>
    /// <remarks>Use this method to remove an image from the system by providing its unique identifier. 
    /// Ensure that the <paramref name="imageId"/> corresponds to an existing image.</remarks>
    /// <param name="imageId">The unique identifier of the image to be removed. This value cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation will be canceled if the token is triggered.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
    /// indicating the success or failure of the operation.</returns>
    Task<IBaseResult> RemoveImage(string imageId, CancellationToken cancellationToken = default);

    #endregion

    #region Videos

    /// <summary>
    /// Adds an image to the specified entity.
    /// </summary>
    /// <remarks>This method creates a new image entity and attempts to add it to the repository. It
    /// then saves the changes to the repository. If the operation fails at any step, the method returns a failure
    /// result with the associated error messages.</remarks>
    /// <param name="request">The request containing the image details and the entity to which the image will be added.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the operation.</returns>
    Task<IBaseResult> AddVideo(AddEntityVideoRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a video identified by the specified video ID.
    /// </summary>
    /// <remarks>The operation may fail if the video does not exist or if the user does not have sufficient
    /// permissions to remove it.  Check the returned <see cref="IBaseResult"/> for details about the operation's
    /// success or failure.</remarks>
    /// <param name="videoId">The unique identifier of the video to be removed. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the outcome of the operation.</returns>
    Task<IBaseResult> RemoveVideo(string videoId, CancellationToken cancellationToken = default);

    #endregion

    #region Listing Service

    /// <summary>
    /// Adds a new listing service to the repository.
    /// </summary>
    /// <remarks>This method creates a new listing service using the provided <paramref name="dto"/> and saves
    /// it to the repository. If the operation fails at any stage, the returned result will contain the failure
    /// messages.</remarks>
    /// <param name="dto">The data transfer object containing the details of the listing service to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the operation.</returns>
    Task<IBaseResult> AddListingService(ListingServiceDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing listing service with the provided details.
    /// </summary>
    /// <remarks>The method attempts to find an existing listing service by the ID provided in the <paramref
    /// name="dto"/>. If the service is not found, the operation fails with an appropriate error message. If the service
    /// is found, its details are updated with the values from the <paramref name="dto"/>. The operation succeeds only
    /// if the update is successfully persisted.</remarks>
    /// <param name="dto">The data transfer object containing the updated details of the listing service.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the update operation.</returns>
    Task<IBaseResult> UpdateListingService(ListingServiceDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a listing service identified by the specified ID.
    /// </summary>
    /// <remarks>This method attempts to delete the listing service and save the changes to the repository. If
    /// the operation fails at any step,  the returned result will contain the failure messages.</remarks>
    /// <param name="listingServiceId">The unique identifier of the listing service to be removed. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the operation.</returns>
    Task<IBaseResult> RemoveListingService(string listingServiceId, CancellationToken cancellationToken = default);

    #endregion

    #region Listing Service Images

    /// <summary>
    /// Adds an image to the specified entity.
    /// </summary>
    /// <remarks>Use this method to associate an image with an existing entity. Ensure that the entity
    /// specified in the request exists before calling this method.</remarks>
    /// <param name="request">The request containing the details of the image to be added, including the entity identifier and image data.
    /// Cannot be null.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the operation.</returns>
    Task<IBaseResult> AddListingServiceImage(AddEntityImageRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes an image with the specified identifier from the system.
    /// </summary>
    /// <remarks>Use this method to remove an image from the system by providing its unique identifier. 
    /// Ensure that the <paramref name="imageId"/> corresponds to an existing image.</remarks>
    /// <param name="imageId">The unique identifier of the image to be removed. This value cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation will be canceled if the token is triggered.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
    /// indicating the success or failure of the operation.</returns>
    Task<IBaseResult> RemoveListingServiceImage(string imageId, CancellationToken cancellationToken = default);

    #endregion

    #region Listing Products

    /// <summary>
    /// Adds a new listing product to the repository.
    /// </summary>
    /// <remarks>This method creates a new listing product using the provided <paramref name="dto"/> and saves
    /// it to the repository. If the operation fails, the returned result will contain error messages describing the
    /// failure.</remarks>
    /// <param name="dto">The data transfer object containing the details of the listing product to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the operation.</returns>
    Task<IBaseResult> AddListingProduct([FromBody] ListingProductDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing listing product with the provided details.
    /// </summary>
    /// <remarks>The method updates the name, description, and price of the listing product identified by the
    /// <c>Id</c> property in the <paramref name="dto"/>. If the specified product is not found, the operation fails
    /// with an appropriate error message.</remarks>
    /// <param name="dto">The data transfer object containing the updated details of the listing product.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the update operation.</returns>
    Task<IBaseResult> UpdateListingProduct([FromBody] ListingProductDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a listing product identified by the specified ID.
    /// </summary>
    /// <remarks>This method attempts to delete the specified listing product and persist the changes to the
    /// repository. If the deletion or save operation fails, the returned result will indicate failure with the
    /// corresponding error messages.</remarks>
    /// <param name="listingProductId">The unique identifier of the listing product to be removed. This value cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the operation. If the operation fails, the result includes error messages.</returns>
    Task<IBaseResult> RemoveListingProduct(string listingProductId, CancellationToken cancellationToken = default);

    #endregion

    #region Listing Product Images

    /// <summary>
    /// Adds an image to the specified entity.
    /// </summary>
    /// <remarks>Use this method to associate an image with an existing entity. Ensure that the entity
    /// specified in the request exists before calling this method.</remarks>
    /// <param name="request">The request containing the details of the image to be added, including the entity identifier and image data.
    /// Cannot be null.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the operation.</returns>
    Task<IBaseResult> AddListingProductImage(AddEntityImageRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes an image with the specified identifier from the system.
    /// </summary>
    /// <remarks>Use this method to remove an image from the system by providing its unique identifier. 
    /// Ensure that the <paramref name="imageId"/> corresponds to an existing image.</remarks>
    /// <param name="imageId">The unique identifier of the image to be removed. This value cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation will be canceled if the token is triggered.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
    /// indicating the success or failure of the operation.</returns>
    Task<IBaseResult> RemoveListingProductImage(string imageId, CancellationToken cancellationToken = default);

    #endregion
}
