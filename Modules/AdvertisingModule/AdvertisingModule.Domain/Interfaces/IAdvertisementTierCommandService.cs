using AdvertisingModule.Domain.DataTransferObjects;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.RequestFeatures;

namespace AdvertisingModule.Domain.Interfaces;

/// <summary>
/// Defines a contract for managing advertisement tiers, including creation, updates, deletion, and image management.
/// </summary>
/// <remarks>This service provides methods to perform operations on advertisement tiers, such as creating new
/// tiers, updating existing ones,  removing tiers, and managing associated images. Each operation returns a result
/// indicating the success or failure of the operation.</remarks>
public interface IAdvertisementTierCommandService
{
    /// <summary>
    /// Creates a new advertisement tier.
    /// </summary>
    /// <param name="advertisementTier">The advertisement tier to create.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A result containing the created <see cref="AdvertisementTierDto"/> when successful.</returns>
    Task<IBaseResult<AdvertisementTierDto>> CreateAsync(AdvertisementTierDto advertisementTier, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing advertisement tier.
    /// </summary>
    /// <param name="advertisementTier">The advertisement tier with updated values.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<IBaseResult> UpdateAsync(AdvertisementTierDto advertisementTier, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes an advertisement tier by its identifier.
    /// </summary>
    /// <param name="advertisementTierId">The unique identifier of the advertisement tier to remove.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<IBaseResult> RemoveAsync(string advertisementTierId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Associates an existing image with an advertisement tier.
    /// </summary>
    /// <param name="request">The request describing the image-to-entity association.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes an image association from an advertisement tier.
    /// </summary>
    /// <param name="imageId">The unique identifier of the image to remove.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<IBaseResult> RemoveImage(string imageId, CancellationToken cancellationToken = default);
}