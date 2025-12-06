using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Application.RestServices
{
    /// <summary>
    /// Provides methods for managing booking terms description templates through RESTful API calls.
    /// </summary>
    /// <remarks>This service allows clients to perform CRUD operations on booking terms description
    /// templates,  including retrieving all templates, retrieving a specific template by ID, adding new templates, 
    /// editing existing templates, and deleting templates. The service communicates with a REST API  using the provided
    /// <see cref="IBaseHttpProvider"/>.</remarks>
    /// <param name="provider"></param>
    public class BookingTermsDescriptionTemplateRestService(IBaseHttpProvider provider) : IBookingTermsDescriptionTemplateService
    {
        /// <summary>
        /// Retrieves all booking terms description templates asynchronously.
        /// </summary>
        /// <remarks>This method sends a request to retrieve all booking terms description templates from
        /// the underlying data provider. The operation is performed asynchronously and can be canceled using the
        /// provided <paramref name="cancellationToken"/>.</remarks>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// containing a list of <see cref="BookingTermsDescriptionTemplateDto"/> objects representing the booking terms
        /// description templates.</returns>
        public async Task<IBaseResult<List<BookingTermsDescriptionTemplateDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<List<BookingTermsDescriptionTemplateDto>>("booking-terms-description-templates/all");
            return result;
        }

        /// <summary>
        /// Retrieves a booking terms description template by its unique identifier.
        /// </summary>
        /// <remarks>This method sends a request to retrieve the booking terms description template
        /// associated with the specified identifier. Ensure the <paramref name="id"/> corresponds to a valid
        /// template.</remarks>
        /// <param name="id">The unique identifier of the booking terms description template to retrieve. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping the <see cref="BookingTermsDescriptionTemplateDto"/> if the template is found.</returns>
        public async Task<IBaseResult<BookingTermsDescriptionTemplateDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<BookingTermsDescriptionTemplateDto>($"booking-terms-description-templates/{id}");
            return result;
        }

        /// <summary>
        /// Adds a new booking terms description template asynchronously.
        /// </summary>
        /// <remarks>This method sends the provided booking terms description template to the underlying
        /// data provider for creation. Ensure that the <paramref name="dto"/> contains valid data before calling this
        /// method.</remarks>
        /// <param name="dto">The data transfer object representing the booking terms description template to be added.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that includes the added <see cref="BookingTermsDescriptionTemplateDto"/> if the operation is
        /// successful.</returns>
        public async Task<IBaseResult<BookingTermsDescriptionTemplateDto>> AddAsync(BookingTermsDescriptionTemplateDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<BookingTermsDescriptionTemplateDto, BookingTermsDescriptionTemplateDto>($"booking-terms-description-templates", dto);
            return result;
        }

        /// <summary>
        /// Updates an existing booking terms description template.
        /// </summary>
        /// <remarks>This method sends the updated template details to the underlying provider for
        /// processing. Ensure that the <paramref name="dto"/> contains valid data before calling this method.</remarks>
        /// <param name="dto">The data transfer object containing the updated details of the booking terms description template.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with the updated <see cref="BookingTermsDescriptionTemplateDto"/>.</returns>
        public async Task<IBaseResult<BookingTermsDescriptionTemplateDto>> EditAsync(BookingTermsDescriptionTemplateDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<BookingTermsDescriptionTemplateDto, BookingTermsDescriptionTemplateDto>($"booking-terms-description-templates", dto);
            return result;
        }

        /// <summary>
        /// Deletes a resource identified by the specified ID asynchronously.
        /// </summary>
        /// <remarks>This method sends a delete request to the underlying provider to remove the resource.
        /// Ensure that the specified <paramref name="id"/> corresponds to an existing resource. The operation is
        /// asynchronous and can be canceled using the provided <paramref name="cancellationToken"/>.</remarks>
        /// <param name="id">The unique identifier of the resource to delete. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous delete operation. The task result contains an <see
        /// cref="IBaseResult"/> indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"booking-terms-description-templates", id);
            return result;
        }
    }
}
