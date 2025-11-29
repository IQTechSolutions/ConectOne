using ConectOne.Domain.ResultWrappers;
using SchoolsModule.Domain.DataTransferObjects;

namespace SchoolsModule.Domain.Interfaces.Learners
{
    /// <summary>
    /// Interface for managing create, update, and delete operations for learners.
    /// </summary>
    public interface ILearnerCommandService
    {
        /// <summary>
        /// Creates a new learner entity in the data store.
        /// </summary>
        /// <param name="learnerDto">The learner data transfer object containing details of the learner to create.</param>
        /// <param name="cancellationToken">Optional token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created <see cref="LearnerDto"/> wrapped in a result object.</returns>
        Task<IBaseResult<LearnerDto>> CreateAsync(LearnerDto learnerDto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing learner entity in the data store.
        /// </summary>
        /// <param name="learner">The updated learner data transfer object.</param>
        /// <param name="cancellationToken">Optional token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous update operation.</returns>
        Task<IBaseResult> UpdateAsync(LearnerDto learner, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a learner and any associated data (e.g., notifications, messages) from the system.
        /// </summary>
        /// <param name="parentId">The ID of the learner to be deleted.</param>
        /// <param name="cancellationToken">Optional token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous delete operation.</returns>
        Task<IBaseResult> RemoveAsync(string parentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the parent associations for a learner, syncing additions and removals.
        /// </summary>
        /// <param name="learnerId">The ID of the learner.</param>
        /// <param name="learnerParents">The updated list of parents.</param>
        /// <param name="cancellationToken">Token to cancel the async operation.</param>
        Task<IBaseResult> UpdateLearnerParentsAsync(string learnerId, List<ParentDto> learnerParents, CancellationToken cancellationToken = default);
    }

}
