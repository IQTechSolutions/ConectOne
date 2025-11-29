using ConectOne.Domain.ResultWrappers;
using SchoolsModule.Domain.DataTransferObjects;

namespace SchoolsModule.Domain.Interfaces.Discipline;

/// <summary>
/// Defines a contract for managing disciplinary actions and severity scales within the system.
/// </summary>
/// <remarks>This service provides methods for retrieving, creating, updating, and deleting both severity scales
/// and disciplinary actions. It supports asynchronous operations and cancellation tokens to ensure responsiveness and
/// cancellation support.</remarks>
public interface IDisciplinaryActionService
{
    /// <summary>
    /// Retrieves all severity scales available in the system.
    /// </summary>
    /// <remarks>This method is used to fetch a complete list of severity scales, which may be used for
    /// categorization or other domain-specific purposes. The operation supports cancellation through the provided
    /// <paramref name="cancellationToken"/>.</remarks>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// containing an enumerable collection of <see cref="SeverityScaleDto"/> objects.</returns>
    Task<IBaseResult<IEnumerable<SeverityScaleDto>>> AllSeverityScalesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the severity scale details for the specified scale identifier.
    /// </summary>
    /// <param name="scaleId">The unique identifier of the severity scale to retrieve. Cannot be <see langword="null"/> or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// object wrapping the <see cref="SeverityScaleDto"/> for the specified scale identifier.</returns>
    Task<IBaseResult<SeverityScaleDto>> SeverityScaleAsync(string scaleId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new severity scale asynchronously.
    /// </summary>
    /// <remarks>The severity scale is used to define the levels of severity for a specific context. Ensure
    /// that the  <paramref name="scale"/> parameter contains valid data before calling this method.</remarks>
    /// <param name="scale">The severity scale data to be created. Must not be <see langword="null"/>.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
    /// indicating the outcome of the operation.</returns>
    Task<IBaseResult> CreateSeverityScaleAsync(SeverityScaleDto scale, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the severity scale with the specified details.
    /// </summary>
    /// <remarks>This method performs an asynchronous update of the severity scale. Ensure that the provided
    /// <paramref name="scale"/> contains valid data.</remarks>
    /// <param name="scale">The severity scale data to be updated. Must not be <see langword="null"/>.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the outcome of the update operation.</returns>
    Task<IBaseResult> UpdateSeverityScaleAsync(SeverityScaleDto scale, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the severity scale with the specified identifier.
    /// </summary>
    /// <remarks>This method performs an asynchronous operation to delete a severity scale. Ensure that the
    /// specified <paramref name="scaleId"/> exists  and is not in use before calling this method. The operation may
    /// fail if the scale is referenced elsewhere.</remarks>
    /// <param name="scaleId">The unique identifier of the severity scale to delete. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the outcome of the operation.</returns>
    Task<IBaseResult> DeleteSeverityScaleAsync(string scaleId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all disciplinary actions asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// object that wraps an enumerable collection of <see cref="DisciplinaryActionDto"/> instances.</returns>
    Task<IBaseResult<IEnumerable<DisciplinaryActionDto>>> AllActionsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a disciplinary action based on the specified action identifier.
    /// </summary>
    /// <remarks>Use this method to perform a disciplinary action identified by the given <paramref
    /// name="actionId"/>. Ensure that the action identifier corresponds to a valid and executable action.</remarks>
    /// <param name="actionId">The unique identifier of the disciplinary action to execute. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// object wrapping a <see cref="DisciplinaryActionDto"/> that provides details about the executed action.</returns>
    Task<IBaseResult<DisciplinaryActionDto>> ActionAsync(string actionId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously creates a new disciplinary action.
    /// </summary>
    /// <remarks>This method validates the provided <paramref name="action"/> and performs the necessary
    /// operations to persist the disciplinary action. Ensure that the <paramref name="action"/> object contains all
    /// required fields before calling this method.</remarks>
    /// <param name="action">The data transfer object containing the details of the disciplinary action to be created. Cannot be <see
    /// langword="null"/>.</param>
    /// <param name="cancellationToken">An optional token to cancel the operation. Defaults to <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the outcome of the operation.</returns>
    Task<IBaseResult> CreateActionAsync(DisciplinaryActionDto action, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing disciplinary action with the provided details.
    /// </summary>
    /// <param name="action">The disciplinary action details to update. Must not be <see langword="null"/>.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
    /// indicating the outcome of the update operation.</returns>
    Task<IBaseResult> UpdateActionAsync(DisciplinaryActionDto action, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the specified action asynchronously.
    /// </summary>
    /// <param name="actionId">The unique identifier of the action to delete. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the outcome of the delete operation.</returns>
    Task<IBaseResult> DeleteActionAsync(string actionId, CancellationToken cancellationToken = default);
}
