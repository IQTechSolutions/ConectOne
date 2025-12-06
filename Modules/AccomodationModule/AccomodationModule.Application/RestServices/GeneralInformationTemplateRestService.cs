using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Application.RestServices
{
    /// <summary>
    /// Provides RESTful operations for managing meet-and-greet templates.
    /// </summary>
    /// <remarks>This service offers methods to retrieve, add, update, and delete meet-and-greet templates.
    /// All operations are asynchronous and rely on an underlying HTTP provider for communication.</remarks>
    /// <param name="provider"></param>
    public class GeneralInformationTemplateRestService(IBaseHttpProvider provider) : IGeneralInformationTemplateService
    {
        /// <summary>
        /// Retrieves all general information templates asynchronously.
        /// </summary>
        /// <remarks>This method fetches all available general information templates from the underlying
        /// data source. The operation can be canceled by passing a cancellation token.</remarks>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// containing an enumerable collection of <see cref="GeneralInformationTemplateDto"/> objects.</returns>
        public async Task<IBaseResult<IEnumerable<GeneralInformationTemplateDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<GeneralInformationTemplateDto>>("general-information-templates/all");
            return result;
        }

        /// <summary>
        /// Retrieves a general information template by its unique identifier.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch the general information
        /// template from the provider. Ensure that the <paramref name="id"/> corresponds to a valid template;
        /// otherwise, the result may indicate a failure.</remarks>
        /// <param name="id">The unique identifier of the general information template to retrieve. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping the <see cref="GeneralInformationTemplateDto"/> corresponding to the specified identifier.</returns>
        public async Task<IBaseResult<GeneralInformationTemplateDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<GeneralInformationTemplateDto>($"general-information-templates/{id}");
            return result;
        }

        /// <summary>
        /// Adds a new general information template asynchronously.
        /// </summary>
        /// <remarks>This method sends the provided general information template to the underlying
        /// provider  for storage. Ensure that the dto contains valid data before calling  this method.</remarks>
        /// <param name="dto">The data transfer object representing the general information template to be added.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  IBaseResult{T} of type
        /// GeneralInformationTemplateDto  representing the result of the operation.</returns>
        public async Task<IBaseResult<GeneralInformationTemplateDto>> AddAsync(GeneralInformationTemplateDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<GeneralInformationTemplateDto, GeneralInformationTemplateDto>($"general-information-templates", dto);
            return result;
        }

        /// <summary>
        /// Updates an existing general information template with the specified data.
        /// </summary>
        /// <remarks>This method sends the updated template data to the underlying provider for
        /// processing. Ensure that the <paramref name="dto"/> contains valid data before calling this method.</remarks>
        /// <param name="dto">The data transfer object containing the updated information for the general information template.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. This allows the operation to be canceled if needed.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping the updated <see cref="GeneralInformationTemplateDto"/> if the operation is successful.</returns>
        public async Task<IBaseResult<GeneralInformationTemplateDto>> EditAsync(GeneralInformationTemplateDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<GeneralInformationTemplateDto, GeneralInformationTemplateDto>($"general-information-templates", dto);
            return result;
        }

        /// <summary>
        /// Deletes the specified resource asynchronously.
        /// </summary>
        /// <remarks>This method sends a delete request to the underlying provider to remove the resource
        /// identified by the  specified <paramref name="id"/>. Ensure the <paramref name="id"/> corresponds to an
        /// existing resource.</remarks>
        /// <param name="id">The unique identifier of the resource to delete. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the delete operation.</returns>
        public async Task<IBaseResult> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"general-information-templates", id);
            return result;
        }
    }
}
