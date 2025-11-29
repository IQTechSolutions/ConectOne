using ConectOne.Domain.ResultWrappers;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Domain.Interfaces.ActivityGroups
{
    /// <summary>
    /// Defines command operations for managing activity groups, including creation, updating, and deletion.
    /// </summary>
    public interface IActivityGroupCommandService
    {
        /// <summary>
        /// Creates a new activity group using the provided data.
        /// </summary>
        /// <param name="activitiyGroup">The data transfer object representing the activity group to be created.</param>
        /// <returns>
        /// A result containing the created <see cref="ActivityGroupDto"/> if successful, or error messages if the operation fails.
        /// </returns>
        Task<IBaseResult<ActivityGroupDto>> CreateAsync(ActivityGroupDto activitiyGroup, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing activity group based on the supplied information.
        /// </summary>
        /// <param name="activitiyGroup">The data transfer object with updated activity group data.</param>
        /// <returns>
        /// A result indicating whether the update was successful, including any error messages.
        /// </returns>
        Task<IBaseResult> UpdateAsync(ActivityGroupDto activitiyGroup, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an activity group by its unique identifier.
        /// </summary>
        /// <param name="activitiyGroupId">The unique ID of the activity group to be deleted.</param>
        /// <returns>
        /// A result indicating success or failure of the deletion process.
        /// </returns>
        Task<IBaseResult> DeleteAsync(string activitiyGroupId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes consent (e.g., attendance/transport) for a user in the context of this activity group.
        /// For instance, a parent might retract previously granted permission.
        /// </summary>
        /// <param name="args">Holds user and permission details.</param>
        Task<IBaseResult> RemoveConsent(RemoveConsentArgs args, CancellationToken cancellationToken = default);

        /// <summary>
        /// Associates an activity group with a particular category, enabling 
        /// further filtering or grouping in the system.
        /// </summary>
        /// <param name="categoryId">ID of the category being linked.</param>
        /// <param name="activityGroupId">ID of the activity group to link.</param>
        /// <returns>A success/failure result.</returns>
        Task<IBaseResult> CreateActivityGroupCategoryAsync(string categoryId, string activityGroupId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a learner to an activity group as a team member.
        /// </summary>
        /// <param name="activityGroupId">ID of the activity group.</param>
        /// <param name="learnerId">ID of the learner to add.</param>
        /// <returns>A result indicating success or failure.</returns>
        Task<IBaseResult> CreateActivityGroupTeamMemberAsync(string activityGroupId, string learnerId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a learner from an activity group team.
        /// </summary>
        /// <param name="activityGroupId">ID of the activity group.</param>
        /// <param name="learnerId">ID of the learner to remove.</param>
        /// <returns>A result indicating success or failure.</returns>
        Task<IBaseResult> RemoveActivityGroupTeamMemberAsync(string activityGroupId, string learnerId, CancellationToken cancellationToken = default);
    }

}
