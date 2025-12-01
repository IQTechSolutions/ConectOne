using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.Discipline;

namespace SchoolsModule.Application.RestServices.Disciplinary
{
    /// <summary>
    /// Provides methods for managing disciplinary actions and severity scales through a RESTful service.
    /// </summary>
    /// <remarks>This service acts as an abstraction over the underlying HTTP provider, enabling operations
    /// such as retrieving, creating, updating, and deleting disciplinary actions and severity scales. All methods are
    /// asynchronous and return results wrapped in <see cref="IBaseResult"/> or <see cref="IBaseResult"/> to indicate
    /// the outcome of the operation. Cancellation tokens are supported for all operations to allow graceful termination
    /// of requests.</remarks>
    /// <param name="provider"></param>
    public class DisciplinaryActionRestService(IBaseHttpProvider provider) : IDisciplinaryActionService
    {
        /// <summary>
        /// Retrieves all severity scales asynchronously.
        /// </summary>
        /// <remarks>This method fetches severity scales from the underlying data provider. The result
        /// includes all available severity scales, or an empty collection if none are found.</remarks>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// containing an enumerable collection of <see cref="SeverityScaleDto"/> objects.</returns>
        public async Task<IBaseResult<IEnumerable<SeverityScaleDto>>> AllSeverityScalesAsync(CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<SeverityScaleDto>>("discipline/actions/scales");
            return result;
        }

        /// <summary>
        /// Retrieves the severity scale details for the specified scale identifier.
        /// </summary>
        /// <remarks>This method sends a request to retrieve the severity scale details from the provider.
        /// Ensure that the <paramref name="scaleId"/> corresponds to a valid severity scale.</remarks>
        /// <param name="scaleId">The unique identifier of the severity scale to retrieve. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the severity scale details as a <see cref="SeverityScaleDto"/>.</returns>
        public async Task<IBaseResult<SeverityScaleDto>> SeverityScaleAsync(string scaleId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<SeverityScaleDto>($"discipline/actions/scales/{scaleId}");
            return result;
        }

        /// <summary>
        /// Creates or updates a severity scale asynchronously.
        /// </summary>
        /// <remarks>This method sends the provided severity scale data to the underlying provider for
        /// creation or update. Ensure that the <paramref name="scale"/> parameter contains valid data before calling
        /// this method.</remarks>
        /// <param name="scale">The severity scale data to be created or updated.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> CreateSeverityScaleAsync(SeverityScaleDto scale, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync($"discipline/actions/scales", scale);
            return result;
        }

        /// <summary>
        /// Updates the severity scale by sending the provided data to the server.
        /// </summary>
        /// <remarks>This method sends the severity scale data to the server using a POST request. Ensure
        /// that the provided <paramref name="scale"/> contains valid data before calling this method.</remarks>
        /// <param name="scale">The severity scale data to be updated. This must not be <see langword="null"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateSeverityScaleAsync(SeverityScaleDto scale, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"discipline/actions/scales", scale);
            return result;
        }

        /// <summary>
        /// Deletes a severity scale with the specified identifier.
        /// </summary>
        /// <remarks>This method sends a delete request to the underlying provider to remove the severity
        /// scale identified by <paramref name="scaleId"/>. Ensure the identifier is valid and corresponds to an
        /// existing scale.</remarks>
        /// <param name="scaleId">The unique identifier of the severity scale to delete. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the delete operation.</returns>
        public async Task<IBaseResult> DeleteSeverityScaleAsync(string scaleId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"discipline/actions/scales", scaleId);
            return result;
        }

        /// <summary>
        /// Retrieves all disciplinary actions asynchronously.
        /// </summary>
        /// <remarks>This method sends a request to retrieve all disciplinary actions and returns the
        /// result as a collection of <see cref="DisciplinaryActionDto"/> objects. If no actions are available, the
        /// result may contain an empty collection.</remarks>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// of <see cref="IEnumerable{T}"/> containing <see cref="DisciplinaryActionDto"/> objects representing the
        /// disciplinary actions.</returns>
        public async Task<IBaseResult<IEnumerable<DisciplinaryActionDto>>> AllActionsAsync(CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<DisciplinaryActionDto>>("discipline/actions");
            return result;
        }

        /// <summary>
        /// Retrieves a disciplinary action by its unique identifier.
        /// </summary>
        /// <remarks>This method sends a request to retrieve the disciplinary action associated with the
        /// specified <paramref name="actionId"/>. Ensure that the <paramref name="actionId"/> corresponds to a valid
        /// action in the system.</remarks>
        /// <param name="actionId">The unique identifier of the disciplinary action to retrieve. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping the <see cref="DisciplinaryActionDto"/> for the specified action.</returns>
        public async Task<IBaseResult<DisciplinaryActionDto>> ActionAsync(string actionId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<DisciplinaryActionDto>($"discipline/actions/{actionId}");
            return result;
        }

        /// <summary>
        /// Creates a disciplinary action asynchronously.
        /// </summary>
        /// <remarks>This method sends the disciplinary action data to the underlying provider for
        /// creation. Ensure that the <paramref name="action"/> parameter contains valid data before calling this
        /// method.</remarks>
        /// <param name="action">The disciplinary action data to be created.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> CreateActionAsync(DisciplinaryActionDto action, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync($"discipline/actions", action);
            return result;
        }

        /// <summary>
        /// Updates a disciplinary action asynchronously.
        /// </summary>
        /// <remarks>This method sends the provided disciplinary action data to the server for updating.
        /// Ensure that the <paramref name="action"/> parameter contains valid data before calling this
        /// method.</remarks>
        /// <param name="action">The disciplinary action data to be updated.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateActionAsync(DisciplinaryActionDto action, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"discipline/actions", action);
            return result;
        }

        /// <summary>
        /// Deletes a discipline action with the specified identifier.
        /// </summary>
        /// <remarks>This method sends a delete request to the underlying provider to remove the specified
        /// discipline action. Ensure that the <paramref name="actionId"/> corresponds to a valid action before calling
        /// this method.</remarks>
        /// <param name="actionId">The unique identifier of the discipline action to delete. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>An <see cref="IBaseResult"/> representing the result of the delete operation.</returns>
        public async Task<IBaseResult> DeleteActionAsync(string actionId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"discipline/actions", actionId);
            return result;
        }
    }
}
