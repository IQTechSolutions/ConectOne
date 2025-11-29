using AdvertisingModule.Domain.DataTransferObjects;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.RequestFeatures;

namespace AdvertisingModule.Domain.Interfaces;

/// <summary>
/// Defines a service for managing advertisement-related commands, such as creating, updating,  removing, and approving
/// advertisements.
/// </summary>
/// <remarks>This interface provides asynchronous methods for performing operations on advertisements.  Each
/// method returns a result indicating the success or failure of the operation, and some  methods may include additional
/// data in the result. Cancellation tokens can be used to  cancel ongoing operations.</remarks>
public interface IAdvertisementCommandService
{
    /// <summary>
    /// Creates a new advertisement asynchronously.
    /// </summary>
    /// <remarks>The method performs the creation operation asynchronously and returns the result encapsulated
    /// in an <see cref="IBaseResult"/>. Ensure that the provided <paramref name="advertisement"/> contains valid
    /// data before calling this method.</remarks>
    /// <param name="advertisement">The advertisement data to be created. Must not be <see langword="null"/>.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> with
    /// the created <see cref="AdvertisementDto"/> if the operation is successful.</returns>
    Task<IBaseResult<AdvertisementDto>> CreateAsync(AdvertisementDto advertisement, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing advertisement with the provided details asynchronously.
    /// </summary>
    /// <remarks>Ensure that the provided <paramref name="advertisement"/> contains valid and complete data 
    /// before calling this method. The operation may fail if the data is invalid or if the advertisement  does not
    /// exist.</remarks>
    /// <param name="advertisement">The advertisement data to update. Must not be <see langword="null"/>.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
    /// indicating the outcome of the update operation.</returns>
    Task<IBaseResult> UpdateAsync(AdvertisementDto advertisement, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes an advertisement with the specified identifier asynchronously.
    /// </summary>
    /// <remarks>Use this method to remove an advertisement by its unique identifier. Ensure that the
    /// identifier is valid  and corresponds to an existing advertisement. The operation can be canceled by passing a
    /// cancellation token.</remarks>
    /// <param name="advertisementId">The unique identifier of the advertisement to be removed. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
    /// indicating the outcome of the removal operation.</returns>
    Task<IBaseResult> RemoveAsync(string advertisementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Approves the advertisement with the specified identifier.
    /// </summary>
    /// <remarks>Use this method to programmatically approve advertisements. Ensure that the advertisement ID
    /// is valid  and that the advertisement is in a state that allows approval.</remarks>
    /// <param name="advertisementId">The unique identifier of the advertisement to approve. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
    /// indicating the outcome of the approval process.</returns>
    Task<IBaseResult> ApproveAsync(string advertisementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Rejects the advertisement with the specified identifier.
    /// </summary>
    /// <remarks>Use this method to asynchronously reject an advertisement by its identifier. The operation
    /// may be canceled  by providing a cancellation token.</remarks>
    /// <param name="advertisementId">The unique identifier of the advertisement to reject. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
    /// indicating the outcome of the rejection operation.</returns>
    Task<IBaseResult> RejectAsync(string advertisementId, CancellationToken cancellationToken = default);

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
}
