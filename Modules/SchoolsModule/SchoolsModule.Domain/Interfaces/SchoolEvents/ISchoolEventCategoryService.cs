using ConectOne.Domain.ResultWrappers;
using GroupingModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Domain.Interfaces.SchoolEvents
{
    /// <summary>
    /// Defines a contract for retrieving school event categories, participating activity groups, and their team members.
    /// </summary>
    public interface ISchoolEventCategoryService
    {
        #region Categories

        /// <summary>
        /// Retrieves a paginated list of school event activity group categories relevant to the logged-in user (parent or teacher).
        /// </summary>
        /// <param name="pageParameters">Parameters for pagination and filtering.</param>
        /// <param name="trackChanges">Specifies whether entity tracking should be enabled.</param>
        /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
        Task<PaginatedResult<CategoryDto>> PagedSchoolEventActivityGroupCategoriesForAppAsync(SchoolEventPageParameters pageParameters, bool trackChanges = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all activity group categories participating in a given school event.
        /// </summary>
        /// <param name="eventId">The ID of the school event.</param>
        /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
        Task<IBaseResult<IEnumerable<CategoryDto>>> ParticipatingActivityGroupCategories(string eventId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a specific participating activity group category for a school event.
        /// </summary>
        /// <param name="eventId">The ID of the school event.</param>
        /// <param name="participatingActivityCategoryId">The ID of the category to retrieve.</param>
        /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
        Task<IBaseResult<CategoryDto>> ParticipatingActivityGroupCategory(string eventId, string participatingActivityCategoryId, CancellationToken cancellationToken = default);

        #endregion

        #region Activity Groups

        /// <summary>
        /// Retrieves all activity groups participating in a given school event.
        /// </summary>
        /// <param name="eventId">The ID of the school event.</param>
        /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
        Task<IBaseResult<IEnumerable<ActivityGroupDto>>> ParticipatingActivityGroups(string eventId, CancellationToken cancellationToken = default);

        #endregion

        #region TeamMembers

        /// <summary>
        /// Retrieves all team members (learners) associated with a specific activity group in a school event.
        /// </summary>
        /// <param name="eventId">The ID of the school event.</param>
        /// <param name="activityGroupId">The ID of the activity group.</param>
        /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
        Task<IBaseResult<List<LearnerDto>>> ParticipatingActivityGroupTeamMembers(string eventId, string activityGroupId, CancellationToken cancellationToken = default);

        #endregion
    }

}
