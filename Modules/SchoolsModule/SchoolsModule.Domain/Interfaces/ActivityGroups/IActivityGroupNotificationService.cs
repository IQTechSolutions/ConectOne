using ConectOne.Domain.ResultWrappers;
using IdentityModule.Domain.DataTransferObjects;

namespace SchoolsModule.Domain.Interfaces.ActivityGroups
{
    /// <summary>
    /// Defines operations for retrieving notification recipients related to activity groups and categories.
    /// </summary>
    public interface IActivityGroupNotificationService
    {
        /// <summary>
        /// Gets a list of recipients (learners, their parents, and the teacher) associated with a specific activity group.
        /// </summary>
        /// <param name="activityGroupId">The unique identifier of the activity group.</param>
        /// <param name="cancellationToken">Optional token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of recipients.</returns>
        Task<IBaseResult<IEnumerable<RecipientDto>>> ActivityGroupNotificationList(string activityGroupId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a list of recipients associated with a specific activity group category,
        /// including entities under its subcategories.
        /// </summary>
        /// <param name="activityGroupCategoryId">The unique identifier of the activity group category.</param>
        /// <param name="cancellationToken">Optional token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of recipients.</returns>
        Task<IBaseResult<IEnumerable<RecipientDto>>> ActivityGroupCategoryNotificationList(string activityGroupCategoryId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a list of recipients involved in a participating activity group for a school event.
        /// </summary>
        /// <param name="activityGroupId">The unique identifier of the participating activity group.</param>
        /// <param name="cancellationToken">Optional token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of recipients.</returns>
        Task<IBaseResult<IEnumerable<RecipientDto>>> ParticipatingActivityGroupNotificationList(string activityGroupId, CancellationToken cancellationToken = default);
    }
}
