using ConectOne.Domain.ResultWrappers;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Domain.Interfaces.Learners
{
    /// <summary>
    /// Defines learner query operations for retrieving, searching, and managing learner data.
    /// </summary>
    public interface ILearnerQueryService
    {
        /// <summary>
        /// Retrieves a paginated list of learners based on the specified parameters.
        /// </summary>
        /// <param name="pageParameters">Paging and filtering options for the learners.</param>
        /// <param name="trackChanges">Whether to track entity changes in the EF context.</param>
        /// <param name="cancellationToken">Token to cancel the async operation.</param>
        Task<PaginatedResult<LearnerDto>> PagedLearnersAsync(LearnerPageParameters pageParameters, bool trackChanges = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all learners that match the optional filters for learner ID, gender, and age range.
        /// </summary>
        /// <param name="learnerId">Specific learner ID to filter by (optional).</param>
        /// <param name="gender">Gender to filter by (default is all).</param>
        /// <param name="minAge">Minimum age (inclusive).</param>
        /// <param name="maxAge">Maximum age (inclusive).</param>
        /// <param name="cancellationToken">Token to cancel the async operation.</param>
        Task<IBaseResult<IEnumerable<LearnerDto>>> AllLearnersAsync(LearnerPageParameters pageParameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the total number of learners in the system.
        /// </summary>
        /// <param name="cancellationToken">Token to cancel the async operation.</param>
        Task<IBaseResult<int>> LearnerCount(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a specific learner by their ID, with optional tracking.
        /// </summary>
        /// <param name="learnerId">The unique identifier of the learner.</param>
        /// <param name="trackChanges">Whether to track entity changes in the EF context.</param>
        /// <param name="cancellationToken">Token to cancel the async operation.</param>
        Task<IBaseResult<LearnerDto>> LearnerAsync(string learnerId, bool trackChanges = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds a learner by their email address.
        /// </summary>
        /// <param name="emailAddress">The email address of the learner.</param>
        /// <param name="cancellationToken">Token to cancel the async operation.</param>
        Task<IBaseResult<LearnerDto>> LearnerByEmailAsync(string emailAddress, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a learner exists based on their email address and returns their ID.
        /// </summary>
        /// <param name="emailAddress">The email address of the learner.</param>
        /// <param name="cancellationToken">Token to cancel the async operation.</param>
        Task<IBaseResult<string>> LearnerExist(string emailAddress, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all parents associated with the specified learner.
        /// </summary>
        /// <param name="learnerId">The unique identifier of the learner.</param>
        /// <param name="cancellationToken">Token to cancel the async operation.</param>
        Task<IBaseResult<List<ParentDto>>> LearnerParentsAsync(string learnerId, CancellationToken cancellationToken = default);
    }
}
