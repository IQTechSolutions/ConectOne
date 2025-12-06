using AccomodationModule.Domain.Arguments;
using AccomodationModule.Domain.DataTransferObjects;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.RequestFeatures;

namespace AccomodationModule.Domain.Interfaces;

/// <summary>
/// Defines the contract for a service that manages <see cref="Contact"/> entities.
/// This interface extends the base service for CRUD operations and adds specific methods
/// for retrieving paginated lists of guides and updating guide images.
/// </summary>
public interface IContactService 
{
    /// <summary>
    /// Retrieves a paginated list of contacts based on the specified page parameters.
    /// </summary>
    /// <remarks>The method supports pagination by taking page parameters that specify the page number and
    /// size. Use the <see cref="ContactsPageParams"/> object to configure these settings. The result includes metadata
    /// about the total number of items and pages.</remarks>
    /// <param name="pageParameters">The parameters that define the pagination settings, such as page number and page size.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="PaginatedResult{T}"/>
    /// of <see cref="ContactDto"/> objects representing the paginated contacts.</returns>
    Task<PaginatedResult<ContactDto>> PagedAsync(ContactsPageParams pageParameters, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves all contacts.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an IBaseResult with a list of
    /// ContactDto objects representing all contacts.</returns>
    Task<IBaseResult<List<ContactDto>>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves a list of featured contacts.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> with
    /// a list of <see cref="ContactDto"/> objects representing the featured contacts. If no contacts are featured, the
    /// list will be empty.</returns>
    Task<IBaseResult<List<ContactDto>>> GetFeaturedAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves a contact by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the contact to retrieve. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see
    /// cref="IBaseResult{ContactDto}"/> with the contact data if found; otherwise, the result indicates failure.</returns>
    Task<IBaseResult<ContactDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Creates a new contact and persists its profile image.
    /// </summary>
    Task<IBaseResult<ContactDto>> AddAsync(ContactDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Updates scalar properties and profile image of the specified contact.
    /// </summary>
    Task<IBaseResult<ContactDto>> EditAsync(ContactDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Deletes a contact (and relies on cascade‑delete or relational hooks to remove dependent images).
    /// </summary>
    Task<IBaseResult> DeleteAsync(string id, CancellationToken cancellationToken = default);

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
}
