using AdvertisingModule.Domain.DataTransferObjects;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.RequestFeatures;

namespace AdvertisingModule.Domain.Interfaces;

/// <summary>
/// Defines a contract for managing affiliate-related operations, including creating, updating,  and removing
/// affiliates, as well as managing associated images.
/// </summary>
/// <remarks>This service provides methods for performing command operations on affiliates, such as creating  new
/// advertisements, updating affiliate details, and managing images associated with affiliates.  Each operation returns
/// a result indicating the success or failure of the operation.</remarks>
public interface IAffiliateCommandService
{
    /// <summary>
    /// Creates a new advertisement asynchronously for the specified affiliate.
    /// </summary>
    /// <remarks>This method performs the creation operation asynchronously. Ensure that the provided
    /// <paramref name="affiliate"/> contains valid data required for creating an advertisement.</remarks>
    /// <param name="affiliate">The affiliate for whom the advertisement will be created. Cannot be <see langword="null"/>.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// object that includes the created advertisement as an <see cref="AdvertisementDto"/> or details about the
    /// operation's outcome.</returns>
    Task<IBaseResult<AffiliateDto>> CreateAsync(AffiliateDto affiliate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the specified affiliate's information asynchronously.
    /// </summary>
    /// <remarks>The operation will fail if the provided affiliate data is invalid or if the update cannot be
    /// completed due to external constraints. Ensure that the affiliate data is valid before calling this
    /// method.</remarks>
    /// <param name="affiliate">The affiliate data to update. Must not be <see langword="null"/>.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the outcome of the update operation.</returns>
    Task<IBaseResult> UpdateAsync(AffiliateDto affiliate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the display order of affiliates based on the provided request.
    /// </summary>
    /// <remarks>The display order update is applied to the affiliates specified in the request. Ensure that
    /// the request object contains valid affiliate identifiers and order values.</remarks>
    /// <param name="dto">The request object containing the affiliate identifiers and their new display order.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the update operation.</returns>
    Task<IBaseResult> UpdateAffiliateDisplayOrderAsync(AffiliateOrderUpdateRequest dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes the affiliate with the specified identifier asynchronously.
    /// </summary>
    /// <param name="affiliateId">The unique identifier of the affiliate to remove. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
    /// indicating the outcome of the removal operation.</returns>
    Task<IBaseResult> RemoveAsync(string affiliateId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds an image to the specified entity.
    /// </summary>
    /// <remarks>The operation may fail if the request is invalid or if the entity does not exist. Ensure that
    /// the  <paramref name="request"/> parameter is properly populated before calling this method.</remarks>
    /// <param name="request">The request containing the details of the entity and the image to be added.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
    /// indicating the outcome of the operation.</returns>
    Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes an image identified by the specified image ID.
    /// </summary>
    /// <param name="imageId">The unique identifier of the image to be removed. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the outcome of the operation.</returns>
    Task<IBaseResult> RemoveImage(string imageId, CancellationToken cancellationToken = default);
}
