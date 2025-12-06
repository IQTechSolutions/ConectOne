using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Application.RestServices
{
    /// <summary>
    /// Provides methods for managing terms and conditions templates through RESTful API calls.
    /// </summary>
    /// <remarks>This service allows clients to perform CRUD operations on terms and conditions templates,
    /// including retrieving all templates,  retrieving a specific template by ID, adding new templates, editing
    /// existing templates, and deleting templates.  The service communicates with the underlying HTTP provider to
    /// execute these operations.</remarks>
    /// <param name="provider"></param>
    public class TermsAndConditionsTemplateRestService(IBaseHttpProvider provider) : ITermsAndConditionsTemplateService
    {
        /// <summary>
        /// Retrieves all terms and conditions templates asynchronously.
        /// </summary>
        /// <remarks>This method fetches all available terms and conditions templates from the underlying
        /// data source. If no templates are available, the result will contain an empty list.</remarks>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// containing a list of <see cref="TermsAndConditionsTemplateDto"/> objects.</returns>
        public async Task<IBaseResult<List<TermsAndConditionsTemplateDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<List<TermsAndConditionsTemplateDto>>("terms-and-conditions-templates");
            return result;
        }

        /// <summary>
        /// Retrieves a terms and conditions template by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the terms and conditions template to retrieve. Cannot be <see langword="null"/> or
        /// empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// containing the <see cref="TermsAndConditionsTemplateDto"/> if found, or an appropriate result indicating
        /// failure.</returns>
        public async Task<IBaseResult<TermsAndConditionsTemplateDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<TermsAndConditionsTemplateDto>($"terms-and-conditions-templates/{id}");
            return result;
        }

        /// <summary>
        /// Adds a new terms and conditions template asynchronously.
        /// </summary>
        /// <remarks>This method sends the provided terms and conditions template to the underlying data
        /// provider  for storage. Ensure that the dto contains all required fields before calling  this
        /// method.</remarks>
        /// <param name="dto">The data transfer object representing the terms and conditions template to add.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  IBaseResult{T} object that
        /// includes the added terms and conditions template.</returns>
        public async Task<IBaseResult<TermsAndConditionsTemplateDto>> AddAsync(TermsAndConditionsTemplateDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<TermsAndConditionsTemplateDto, TermsAndConditionsTemplateDto>("terms-and-conditions-templates", dto);
            return result;
        }

        /// <summary>
        /// Updates an existing Terms and Conditions template with the provided data.
        /// </summary>
        /// <remarks>This method sends the updated template data to the underlying provider for
        /// processing.  Ensure that the dto contains valid data before calling this method.</remarks>
        /// <param name="dto">The data transfer object containing the updated details of the Terms and Conditions template.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. This allows the operation to be canceled if needed.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  IBaseResult{T} object with
        /// the updated Terms and Conditions template data.</returns>
        public async Task<IBaseResult<TermsAndConditionsTemplateDto>> EditAsync(TermsAndConditionsTemplateDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<TermsAndConditionsTemplateDto, TermsAndConditionsTemplateDto>("terms-and-conditions-templates", dto);
            return result;
        }

        /// <summary>
        /// Deletes a resource with the specified identifier asynchronously.
        /// </summary>
        /// <remarks>This method performs an asynchronous delete operation on the resource identified by
        /// <paramref name="id"/>. Ensure that the provided identifier corresponds to an existing resource. The
        /// operation may fail if the resource does not exist.</remarks>
        /// <param name="id">The unique identifier of the resource to delete. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the delete operation.</returns>
        public async Task<IBaseResult> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync("terms-and-conditions-templates", id);
            return result;
        }
    }
}
