using ConectOne.Domain.ResultWrappers;
using SchoolsModule.Domain.DataTransferObjects;

namespace SchoolsModule.Domain.Interfaces.Discipline;

/// <summary>
/// Service for logging and retrieving disciplinary incidents.
/// </summary>
public interface IDisciplinaryIncidentService
{
    /// <summary>
    /// Creates a new disciplinary incident and returns the result of the operation.
    /// </summary>
    /// <remarks>The operation may fail due to validation errors or other business rules. Check the result for
    /// success or failure details.</remarks>
    /// <param name="incident">The disciplinary incident data to be created. Cannot be <see langword="null"/>.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// object with the created <see cref="DisciplinaryIncidentDto"/> if the operation is successful.</returns>
    Task<IBaseResult<DisciplinaryIncidentDto>> CreateAsync(DisciplinaryIncidentDto incident, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing disciplinary incident asynchronously.
    /// </summary>
    /// <remarks>The method updates the disciplinary incident based on the provided data. Ensure that the
    /// <paramref name="incident"/> contains valid and complete information before calling this method. The operation
    /// may be canceled by passing a cancellation token.</remarks>
    /// <param name="incident">The disciplinary incident data to update. Must not be <see langword="null"/>.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the outcome of the update operation.</returns>
    Task<IBaseResult> UpdateAsync(DisciplinaryIncidentDto incident, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the incident with the specified identifier asynchronously.
    /// </summary>
    /// <param name="incidentId">The unique identifier of the incident to delete. Cannot be <see langword="null"/> or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the outcome of the delete operation.</returns>
    Task<IBaseResult> DeleteAsync(string incidentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a collection of disciplinary incidents associated with a specific learner.
    /// </summary>
    /// <remarks>The method performs an asynchronous operation to fetch disciplinary incidents for the
    /// specified learner. Ensure that the <paramref name="learnerId"/> is valid and corresponds to an existing learner
    /// in the system.</remarks>
    /// <param name="learnerId">The unique identifier of the learner whose disciplinary incidents are to be retrieved. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// containing an enumerable collection of <see cref="DisciplinaryIncidentDto"/> objects representing the learner's
    /// disciplinary incidents.</returns>
    Task<IBaseResult<IEnumerable<DisciplinaryIncidentDto>>> IncidentsByLearnerAsync(string learnerId, CancellationToken cancellationToken = default);
}
