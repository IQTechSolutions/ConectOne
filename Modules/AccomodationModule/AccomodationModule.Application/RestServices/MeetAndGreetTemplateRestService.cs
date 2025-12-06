using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Application.RestServices
{
    /// <summary>
    /// Provides REST-based operations for managing Meet and Greet templates.
    /// </summary>
    /// <remarks>This service allows clients to perform CRUD operations on Meet and Greet templates, 
    /// including retrieving all templates, fetching a specific template by ID, adding new templates,  editing existing
    /// templates, and deleting templates. The service communicates with a REST API  using the provided <see
    /// cref="IBaseHttpProvider"/>.</remarks>
    /// <param name="provider"></param>
    public class MeetAndGreetTemplateRestService(IBaseHttpProvider provider) : IMeetAndGreetTemplateService
    {
        /// <summary>
        /// Retrieves all meet-and-greet templates asynchronously.
        /// </summary>
        /// <remarks>This method fetches all meet-and-greet templates from the underlying data provider.
        /// The operation is asynchronous and can be canceled by passing a cancellation token.</remarks>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// containing an enumerable collection of <see cref="MeetAndGreetTemplateDto"/> objects.</returns>
        public async Task<IBaseResult<IEnumerable<MeetAndGreetTemplateDto>>> AllAsync(CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<MeetAndGreetTemplateDto>>("meet-and-greet-templates/all");
            return result;
        }

        /// <summary>
        /// Retrieves a meet-and-greet template by its unique identifier.
        /// </summary>
        /// <remarks>This method sends a request to the underlying provider to retrieve the meet-and-greet
        /// template. Ensure the <paramref name="id"/> corresponds to a valid template; otherwise, the result may
        /// indicate a failure.</remarks>
        /// <param name="id">The unique identifier of the meet-and-greet template to retrieve. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping the <see cref="MeetAndGreetTemplateDto"/> corresponding to the specified identifier.</returns>
        public async Task<IBaseResult<MeetAndGreetTemplateDto>> ByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<MeetAndGreetTemplateDto>($"meet-and-greet-templates/{id}");
            return result;
        }

        /// <summary>
        /// Adds a new Meet and Greet template asynchronously.
        /// </summary>
        /// <remarks>This method sends the provided template to the underlying data provider for storage.
        /// Ensure that the <paramref name="dto"/> contains all required fields before calling this method.</remarks>
        /// <param name="dto">The data transfer object representing the Meet and Greet template to be added.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that includes the added <see cref="MeetAndGreetTemplateDto"/> if the operation is successful.</returns>
        public async Task<IBaseResult<MeetAndGreetTemplateDto>> AddAsync(MeetAndGreetTemplateDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<MeetAndGreetTemplateDto, MeetAndGreetTemplateDto>("meet-and-greet-templates", dto);
            return result;
        }

        /// <summary>
        /// Updates an existing Meet and Greet template with the specified data.
        /// </summary>
        /// <param name="id">The unique identifier of the Meet and Greet template to update.</param>
        /// <param name="dto">The data transfer object containing the updated template details.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  IBaseResult{T} object with
        /// the updated MeetAndGreetTemplateDto  if the operation is successful.</returns>
        public async Task<IBaseResult<MeetAndGreetTemplateDto>> EditAsync(MeetAndGreetTemplateDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<MeetAndGreetTemplateDto, MeetAndGreetTemplateDto>("meet-and-greet-templates", dto);
            return result;
        }

        /// <summary>
        /// Deletes a meet-and-greet template with the specified identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the meet-and-greet template to delete.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the delete operation.</returns>
        public async Task<IBaseResult> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync("meet-and-greet-templates", id);
            return result;
        }
    }
}
