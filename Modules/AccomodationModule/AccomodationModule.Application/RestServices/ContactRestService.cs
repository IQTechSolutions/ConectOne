using AccomodationModule.Domain.Arguments;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.RequestFeatures;

namespace AccomodationModule.Application.RestServices
{
    /// <summary>
    /// Provides RESTful operations for managing contacts, including retrieving, adding, editing, and deleting contacts,
    /// as well as managing associated images.
    /// </summary>
    /// <remarks>This service acts as an abstraction over HTTP-based operations for contact management. It
    /// supports paginated  retrieval of contacts, fetching all or featured contacts, and CRUD operations for individual
    /// contacts.  Additionally, it provides functionality to add or remove images associated with a contact.</remarks>
    public class ContactRestService(IBaseHttpProvider provider) : IContactService
    {
        /// <summary>
        /// Retrieves a paginated list of contacts based on the specified page parameters.
        /// </summary>
        /// <remarks>This method sends a request to the "contacts/paged" endpoint to retrieve the
        /// paginated data.</remarks>
        /// <param name="pageParameters">The parameters that define the pagination settings, such as page number and page size.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a collection of <see cref="ContactDto"/> objects and
        /// pagination metadata.</returns>
        public async Task<PaginatedResult<ContactDto>> PagedAsync(ContactsPageParams pageParameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetPagedAsync<ContactDto, ContactsPageParams>("contacts/paged", pageParameters);
            return result;
        }

        /// <summary>
        /// Retrieves all contacts asynchronously.
        /// </summary>
        /// <remarks>This method sends a request to fetch all contacts and returns the result as a
        /// strongly-typed list. If no contacts are available, the result may contain an empty list.</remarks>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that holds a list of <see cref="ContactDto"/> instances representing the retrieved contacts.</returns>
        public async Task<IBaseResult<List<ContactDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<List<ContactDto>>("contacts/all");
            return result;
        }

        /// <summary>
        /// Retrieves a list of featured contacts asynchronously.
        /// </summary>
        /// <remarks>The method fetches the featured contacts from the underlying data provider. If no
        /// featured contacts are available, the result may contain an empty list.</remarks>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with a list of <see cref="ContactDto"/> representing the featured contacts.</returns>
        public async Task<IBaseResult<List<ContactDto>>> GetFeaturedAsync(CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<List<ContactDto>>("contacts/featured");
            return result;
        }

        /// <summary>
        /// Retrieves a contact by its unique identifier.
        /// </summary>
        /// <remarks>This method sends a request to retrieve the contact data associated with the
        /// specified identifier. Ensure that the identifier is valid and corresponds to an existing contact.</remarks>
        /// <param name="id">The unique identifier of the contact to retrieve. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that wraps the <see cref="ContactDto"/> for the specified identifier, or an error result if the
        /// contact could not be retrieved.</returns>
        public async Task<IBaseResult<ContactDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<ContactDto>($"contacts/{id}");
            return result;
        }

        /// <summary>
        /// Adds a new contact asynchronously.
        /// </summary>
        /// <remarks>This method sends the contact data to the underlying provider for storage. Ensure
        /// that the <paramref name="dto"/> contains valid data before calling this method.</remarks>
        /// <param name="dto">The data transfer object representing the contact to be added.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that includes the added contact data.</returns>
        public async Task<IBaseResult<ContactDto>> AddAsync(ContactDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<ContactDto, ContactDto>($"contacts", dto);
            return result;
        }

        /// <summary>
        /// Updates an existing contact with the specified details.
        /// </summary>
        /// <remarks>The method sends the updated contact information to the underlying provider. Ensure
        /// that the <paramref name="dto"/> parameter contains valid data before calling this method.</remarks>
        /// <param name="dto">The data transfer object containing the updated contact details.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the updated contact details.</returns>
        public async Task<IBaseResult<ContactDto>> EditAsync(ContactDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<ContactDto, ContactDto>($"contacts", dto);
            return result;
        }

        /// <summary>
        /// Deletes a contact with the specified identifier asynchronously.
        /// </summary>
        /// <remarks>This method sends a delete request to the underlying provider to remove the contact
        /// with the specified identifier. Ensure the identifier corresponds to an existing contact.</remarks>
        /// <param name="id">The unique identifier of the contact to delete. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the delete operation.</returns>
        public async Task<IBaseResult> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"contacts", id);
            return result;
        }

        /// <summary>
        /// Adds an image to an entity based on the provided request.
        /// </summary>
        /// <remarks>This method sends the image addition request to the underlying provider. Ensure that
        /// the <paramref name="request"/> contains valid data before calling this method.</remarks>
        /// <param name="request">The request containing the entity details and the image to be added.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"contacts/addImage", request);
            return result;
        }

        /// <summary>
        /// Removes an image associated with the specified image identifier.
        /// </summary>
        /// <remarks>This method sends a request to delete the image identified by <paramref
        /// name="imageId"/>. Ensure that the provided identifier corresponds to an existing image.</remarks>
        /// <param name="imageId">The unique identifier of the image to be removed. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveImage(string imageId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"contacts/deleteImage", imageId);
            return result;
        }
    }
}
