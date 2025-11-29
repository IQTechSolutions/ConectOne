using ConectOne.Domain.ResultWrappers;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Domain.Interfaces.Learners
{
    /// <summary>
    /// Interface defining methods for importing learner and parent data into the system.
    /// </summary>
    public interface ILearnerExportService
    {
        /// <summary>
        /// Imports a batch of new learners along with their associated parent details into the system.
        /// </summary>
        /// <param name="request">A request object containing the list of learners and parents to import.</param>
        /// <param name="cancellationToken">Optional token to cancel the operation.</param>
        /// <returns>A result indicating success or failure of the import operation.</returns>
        Task<IBaseResult> ImportNewLearnersAndParents(SaveImportedLearnersRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Imports and updates grade assignments for existing learners using their IDs.
        /// </summary>
        /// <param name="request">A request object containing learner IDs and the corresponding grade data.</param>
        /// <param name="cancellationToken">Optional token to cancel the operation.</param>
        /// <returns>A result indicating the outcome of the grade update operation.</returns>
        Task<IBaseResult> ImportNewLearnersGradesById(SaveImportedLearnerGradesRequest request, CancellationToken cancellationToken = default);
    }
}
