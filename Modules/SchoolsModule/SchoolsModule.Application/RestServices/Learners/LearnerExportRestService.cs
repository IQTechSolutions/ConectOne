using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using SchoolsModule.Domain.Interfaces.Learners;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Application.RestServices.Learners
{
    /// <summary>
    /// Provides REST-based services for importing learner and parent data, as well as learner grades, into the system.
    /// </summary>
    /// <remarks>This service interacts with a REST API to perform operations related to importing learners
    /// and their associated data. It implements the <see cref="ILearnerExportService"/> interface and relies on an <see
    /// cref="IBaseHttpProvider"/> for HTTP communication.</remarks>
    /// <param name="provider"></param>
    public class LearnerExportRestService(IBaseHttpProvider provider) : ILearnerExportService
    {
        /// <summary>
        /// Imports new learners and their associated parents based on the provided request data.
        /// </summary>
        /// <remarks>This method sends the import data to the "learners/import/new" endpoint. Ensure that
        /// the <paramref name="request"/> contains valid data to avoid errors during the import process.</remarks>
        /// <param name="request">The request containing the data for the new learners and parents to be imported.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the import operation.</returns>
        public async Task<IBaseResult> ImportNewLearnersAndParents(SaveImportedLearnersRequest request, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync("learners/import/new", request);
            return result;
        }

        /// <summary>
        /// Imports new learner grades based on the specified request data.
        /// </summary>
        /// <remarks>This method sends the provided learner grade data to the
        /// "learners/import/grades/byId" endpoint for processing.  Ensure that the <paramref name="request"/> parameter
        /// contains valid data to avoid errors during the import.</remarks>
        /// <param name="request">The request containing the learner grade data to be imported. This must include all necessary details  for
        /// the import operation.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the import operation.</returns>
        public async Task<IBaseResult> ImportNewLearnersGradesById(SaveImportedLearnerGradesRequest request, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync("learners/import/grades/byId", request);
            return result;
        }
    }
}
