using ConectOne.Domain.ResultWrappers;
using IdentityModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Domain.Interfaces.Learners
{
    /// <summary>
    /// Service interface for generating notification recipient lists related to learners.
    /// </summary>
    public interface ILearnerNotificationService
    {
        /// <summary>
        /// Retrieves a filtered list of learners and their associated notification details
        /// (such as email addresses and preferences) based on the provided parameters.
        /// </summary>
        /// <param name="learnerPageParameters">Filter and paging parameters used to refine the learner query.</param>
        /// <param name="cancellationToken">Optional cancellation token for the async operation.</param>
        /// <returns>A result containing a list of <see cref="RecipientDto"/> representing learners to notify.</returns>
        Task<IBaseResult<IEnumerable<RecipientDto>>> LearnersNotificationList(LearnerPageParameters learnerPageParameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a global list of all potential email notification recipients,
        /// including learners, parents, and teachers, with their preferences and contact info.
        /// </summary>
        /// <param name="cancellationToken">Optional cancellation token for the async operation.</param>
        /// <returns>A result containing a full list of <see cref="RecipientDto"/> entries.</returns>
        Task<IBaseResult<IEnumerable<RecipientDto>>> GlobalMailRecipientList(CancellationToken cancellationToken = default);
    }
}
