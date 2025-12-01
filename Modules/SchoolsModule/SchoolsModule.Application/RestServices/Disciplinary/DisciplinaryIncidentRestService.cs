using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.Discipline;

namespace SchoolsModule.Application.RestServices.Disciplinary
{
    /// <summary>
    /// Provides REST-based operations for managing disciplinary incidents.
    /// </summary>
    /// <remarks>This service allows clients to create, update, delete, and retrieve disciplinary incidents.
    /// It interacts with a REST API to perform these operations, leveraging the provided HTTP provider.</remarks>
    /// <param name="provider"></param>
    public class DisciplinaryIncidentRestService(IBaseHttpProvider provider) : IDisciplinaryIncidentService
    {
        /// <summary>
        /// Creates a new disciplinary incident asynchronously.
        /// </summary>
        /// <remarks>This method sends the provided disciplinary incident data to the underlying provider
        /// for creation. Ensure that the <paramref name="incident"/> parameter contains valid data before calling this
        /// method.</remarks>
        /// <param name="incident">The <see cref="DisciplinaryIncidentDto"/> representing the disciplinary incident to be created.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// object that includes the created <see cref="DisciplinaryIncidentDto"/> if the operation is successful.</returns>
        public async Task<IBaseResult<DisciplinaryIncidentDto>> CreateAsync(DisciplinaryIncidentDto incident, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<DisciplinaryIncidentDto, DisciplinaryIncidentDto>("discipline/incidents", incident);
            return result;
        }

        /// <summary>
        /// Updates a disciplinary incident asynchronously.
        /// </summary>
        /// <remarks>This method sends the provided disciplinary incident data to the underlying provider
        /// for processing. Ensure that the <paramref name="incident"/> parameter contains valid data before calling
        /// this method.</remarks>
        /// <param name="incident">The disciplinary incident data to be updated.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateAsync(DisciplinaryIncidentDto incident, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync("discipline/incidents", incident);
            return result;
        }

        /// <summary>
        /// Deletes an incident with the specified identifier asynchronously.
        /// </summary>
        /// <param name="incidentId">The unique identifier of the incident to delete. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the delete operation.</returns>
        public async Task<IBaseResult> DeleteAsync(string incidentId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync("discipline/incidents", incidentId);
            return result;
        }

        /// <summary>
        /// Retrieves a collection of disciplinary incidents associated with a specific learner.
        /// </summary>
        /// <remarks>This method communicates with an external provider to fetch the disciplinary
        /// incidents for the specified learner. Ensure that the <paramref name="learnerId"/> is valid and corresponds
        /// to an existing learner in the system.</remarks>
        /// <param name="learnerId">The unique identifier of the learner whose disciplinary incidents are to be retrieved.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// containing an enumerable collection of <see cref="DisciplinaryIncidentDto"/> objects representing the
        /// learner's disciplinary incidents.</returns>
        public async Task<IBaseResult<IEnumerable<DisciplinaryIncidentDto>>> IncidentsByLearnerAsync(string learnerId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<DisciplinaryIncidentDto>>($"discipline/incidents/learner/{learnerId}");
            return result;
        }
    }
}
