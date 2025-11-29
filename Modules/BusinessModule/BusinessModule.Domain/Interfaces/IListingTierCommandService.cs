using BusinessModule.Domain.DataTransferObjects;
using BusinessModule.Domain.Entities;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.RequestFeatures;

namespace BusinessModule.Domain.Interfaces;

/// <summary>
/// Defines a contract for managing listing tiers, including creation, updates, deletion, and image management.
/// </summary>
/// <remarks>This service provides methods to perform operations on listing tiers, such as creating new tiers,
/// updating existing ones,  removing tiers, and managing associated images. Each operation returns a result indicating
/// the success or failure of the operation.</remarks>
public interface IListingTierCommandService
{
    /// <summary>
    /// Creates a new listing tier asynchronously.
    /// </summary>
    /// <param name="listingTier">The data transfer object containing the details of the listing tier to create. Cannot be <see langword="null"/>.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// object  with the created <see cref="ListingTier"/> if the operation is successful.</returns>
    Task<IBaseResult<ListingTierDto>> CreateAsync(ListingTierDto listingTier, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the specified listing tier asynchronously.
    /// </summary>
    /// <remarks>This method performs an update operation on the specified listing tier. Ensure that the
    /// provided <paramref name="listingTier"/> contains valid data before calling this method. The operation may be
    /// canceled by passing a cancellation token.</remarks>
    /// <param name="listingTier">The data transfer object representing the listing tier to be updated.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the outcome of the update operation.</returns>
    Task<IBaseResult> UpdateAsync(ListingTierDto listingTier, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a listing tier asynchronously.
    /// </summary>
    /// <param name="listingTierId">The unique identifier of the listing tier to remove. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
    /// indicating the outcome of the removal operation.</returns>
    Task<IBaseResult> RemoveAsync(string listingTierId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds an image to the specified entity.
    /// </summary>
    /// <remarks>The <paramref name="request"/> parameter must not be <see langword="null"/>. Ensure that the
    /// entity specified  in the request exists and that the image meets any required format or size
    /// constraints.</remarks>
    /// <param name="request">The request containing the details of the entity and the image to be added.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
    /// indicating the outcome of the operation.</returns>
    Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes an image with the specified identifier.
    /// </summary>
    /// <remarks>Use this method to remove an image from the system. Ensure that the <paramref
    /// name="imageId"/> corresponds to an existing image. The operation is asynchronous and can be canceled by passing
    /// a cancellation token.</remarks>
    /// <param name="imageId">The unique identifier of the image to be removed. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the outcome of the operation.</returns>
    Task<IBaseResult> RemoveImage(string imageId, CancellationToken cancellationToken = default);
}
