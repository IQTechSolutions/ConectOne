using ConectOne.Domain.ResultWrappers;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Domain.Interfaces.ActivityGroups
{
    public interface IActivityGroupQueryService
    {
        /// <summary>
        /// Retrieves all activity groups matching the specified filtering and paging parameters.
        /// </summary>
        /// <param name="pageParameters">
        /// An <see cref="ActivityGroupPageParameters"/> controlling which activity groups
        /// to filter and how to page/sort the results.
        /// </param>
        /// <returns>
        /// A result object containing a collection of <see cref="ActivityGroupDto"/>.
        /// </returns>
        Task<IBaseResult<IEnumerable<ActivityGroupDto>>> AllActivityGroupsAsync(ActivityGroupPageParameters pageParameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns a paginated list of activity groups, respecting 
        /// the given <see cref="ActivityGroupPageParameters"/> for pagination, search, etc.
        /// </summary>
        Task<PaginatedResult<ActivityGroupDto>> PagedActivityGroupsAsync(ActivityGroupPageParameters pageParameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns a paginated list of activity groups specifically associated 
        /// with a given event, respecting the provided page parameters.
        /// </summary>
        /// <param name="pageParameters">
        /// Filters and paging info that might reference an event ID 
        /// or other relevant fields.
        /// </param>
        Task<PaginatedResult<ActivityGroupDto>> PagedActivityGroupsForEventAsync(ActivityGroupPageParameters pageParameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns a paginated list of activity groups that belong to 
        /// participating event categories, often used to find which groups 
        /// are linked to certain event-based categories.
        /// </summary>
        /// <param name="pageParameters">
        /// Filters, page size, and other parameters controlling the results.
        /// </param>
        Task<PaginatedResult<ActivityGroupDto>> PagedActivityGroupsForParticipatingEventCategories(ActivityGroupPageParameters pageParameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a single activity group by its ID. 
        /// Can optionally track changes if needed for updates.
        /// </summary>
        /// <param name="activityGroupId">The unique identifier of the activity group.</param>
        /// <param name="trackChanges">If true, EF tracking is enabled for potential updates.</param>
        /// <returns>
        /// A success/failure result containing the <see cref="ActivityGroupDto"/>.
        /// </returns>
        Task<IBaseResult<ActivityGroupDto>> ActivityGroupAsync(string activityGroupId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a paginated list of learners associated with a specific activity group.
        /// </summary>
        /// <param name="pageParameters">Parameters that include the activity group ID and paging info.</param>
        /// <param name="trackChanges">Indicates whether change tracking should be enabled.</param>
        /// <param name="cancellationToken">Cancellation token for async operations.</param>
        /// <returns>
        /// A <see cref="PaginatedResult{LearnerDto}"/> containing the list of learners,
        /// or a failure result if the operation could not be completed.
        /// </returns>
        Task<PaginatedResult<LearnerDto>> PagedActivityGroupTeamMembersAsync(LearnerPageParameters pageParameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a paginated list of learners participating in a specific activity group
        /// within a given school event.
        /// </summary>
        /// <param name="pageParameters">
        /// Contains the event ID, activity group ID, and paging parameters such as page number and size.
        /// </param>
        /// <param name="trackChanges">
        /// Indicates whether Entity Framework should track changes to the retrieved entities.
        /// </param>
        /// <returns>
        /// A <see cref="PaginatedResult{LearnerDto}"/> containing the list of learners,
        /// or a failure result if the event is not found or data retrieval fails.
        /// </returns>
        Task<PaginatedResult<LearnerDto>> PagedEventActivityGroupTeamMembersAsync(LearnerPageParameters pageParameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the learners (or user info objects) that form the team members of a given group.
        /// </summary>
        /// <param name="activityGroupId">The unique ID of the activity group.</param>
        /// <returns>A list of <see cref="LearnerDto"/> objects representing the team members.</returns>
        Task<IBaseResult<IEnumerable<LearnerDto>>> ActivityGroupTeamMembersAsync(LearnerPageParameters args, CancellationToken cancellationToken = default);
    }
}
