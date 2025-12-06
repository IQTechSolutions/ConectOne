using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Application.RestServices
{
    /// <summary>
    /// Provides methods for managing custom variable tags through RESTful API calls.   
    /// </summary>
    /// <remarks>This service allows clients to perform CRUD operations on custom variable tags, including
    /// retrieving all tags, fetching a specific tag by its identifier, adding new tags, editing existing tags, and
    /// deleting tags.</remarks>
    /// <param name="provider"></param>
    public class CustomVariableRestService(IBaseHttpProvider provider) : ICustomVariableTagService
    {
        /// <summary>
        /// Retrieves all custom variable tags asynchronously.
        /// </summary>
        /// <remarks>This method fetches all custom variable tags from the underlying data source. The
        /// operation can be canceled by passing a cancellation token.</remarks>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// containing an enumerable collection of <see cref="CustomVariableTagDto"/> objects.</returns>
        public async Task<IBaseResult<IEnumerable<CustomVariableTagDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<CustomVariableTagDto>>("custom-variable-tags/all");
            return result;
        }

        /// <summary>
        /// Retrieves a custom variable tag by its unique identifier.
        /// </summary>
        /// <remarks>This method sends a request to retrieve the custom variable tag associated with the
        /// specified <paramref name="id"/>. Ensure that the provided <paramref name="id"/> corresponds to an existing
        /// tag in the system.</remarks>
        /// <param name="id">The unique identifier of the custom variable tag to retrieve. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// containing the <see cref="CustomVariableTagDto"/> if the tag is found.</returns>
        public async Task<IBaseResult<CustomVariableTagDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<CustomVariableTagDto>($"custom-variable-tags/{id}");
            return result;
        }

        /// <summary>
        /// Adds a new custom variable tag asynchronously.
        /// </summary>
        /// <remarks>This method sends the provided <see cref="CustomVariableTagDto"/> to the underlying
        /// provider for creation. Ensure that the <paramref name="dto"/> contains valid data before calling this
        /// method.</remarks>
        /// <param name="dto">The data transfer object representing the custom variable tag to be added.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that includes the added <see cref="CustomVariableTagDto"/> if the operation is successful.</returns>
        public async Task<IBaseResult<CustomVariableTagDto>> AddAsync(CustomVariableTagDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<CustomVariableTagDto, CustomVariableTagDto>($"custom-variable-tags", dto);
            return result;
        }

        /// <summary>
        /// Updates an existing custom variable tag asynchronously.
        /// </summary>
        /// <remarks>This method sends the updated custom variable tag details to the server and returns
        /// the server's response. Ensure that the <paramref name="dto"/> contains valid data before calling this
        /// method.</remarks>
        /// <param name="dto">The data transfer object containing the updated details of the custom variable tag.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that wraps the updated <see cref="CustomVariableTagDto"/>.</returns>
        public async Task<IBaseResult<CustomVariableTagDto>> EditAsync(CustomVariableTagDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<CustomVariableTagDto, CustomVariableTagDto>($"custom-variable-tags", dto);
            return result;
        }

        /// <summary>
        /// Deletes a custom variable tag with the specified identifier.
        /// </summary>
        /// <remarks>This method sends a delete request to the underlying provider to remove the specified
        /// custom variable tag. Ensure the <paramref name="id"/> corresponds to an existing tag; otherwise, the
        /// operation may fail.</remarks>
        /// <param name="id">The unique identifier of the custom variable tag to delete. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous delete operation. The task result contains an <see
        /// cref="IBaseResult"/> indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"custom-variable-tags", id);
            return result;
        }
    }
}
