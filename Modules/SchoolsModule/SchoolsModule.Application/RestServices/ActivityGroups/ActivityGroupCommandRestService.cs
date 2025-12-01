using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.ActivityGroups;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Application.RestServices.ActivityGroups
{
    /// <summary>
    /// Provides methods for managing activity groups, including creating, updating, deleting,  and managing associated
    /// categories and team members.
    /// </summary>
    /// <remarks>This service acts as a REST client for interacting with activity group-related endpoints.  It
    /// supports operations such as creating and updating activity groups, removing consents,  and managing
    /// relationships between activity groups, categories, and team members.</remarks>
    /// <param name="provider"></param>
    public class ActivityGroupCommandRestService(IBaseHttpProvider provider) : IActivityGroupCommandService
    {
        /// <summary>
        /// Creates a new activity group asynchronously.
        /// </summary>
        /// <param name="activitiyGroup">The activity group data to be created.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult"/>
        /// containing the created <see cref="ActivityGroupDto"/>.</returns>
        public async Task<IBaseResult<ActivityGroupDto>> CreateAsync(ActivityGroupDto activitiyGroup, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<ActivityGroupDto, ActivityGroupDto>("activitygroups", activitiyGroup);
            return result;
        }

        /// <summary>
        /// Updates an activity group asynchronously.
        /// </summary>
        /// <remarks>This method sends the provided activity group data to the underlying provider for
        /// updating. Ensure that  the <paramref name="activitiyGroup"/> parameter contains valid data before calling
        /// this method.</remarks>
        /// <param name="activitiyGroup">The activity group data to be updated.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateAsync(ActivityGroupDto activitiyGroup, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync("activitygroups", activitiyGroup);
            return result;
        }

        /// <summary>
        /// Deletes an activity group with the specified identifier.
        /// </summary>
        /// <remarks>This method sends a delete request to the underlying provider to remove the specified
        /// activity group. Ensure that the <paramref name="activitiyGroupId"/> corresponds to a valid activity
        /// group.</remarks>
        /// <param name="activitiyGroupId">The unique identifier of the activity group to delete. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the delete operation.</returns>
        public async Task<IBaseResult> DeleteAsync(string activitiyGroupId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync("activitygroups", activitiyGroupId);
            return result;
        }

        /// <summary>
        /// Removes a user's consent for a specific activity group.
        /// </summary>
        /// <remarks>This method sends a request to remove the specified consent and returns the result of
        /// the operation.  Ensure that the <paramref name="args"/> parameter contains valid data to avoid
        /// errors.</remarks>
        /// <param name="args">The arguments specifying the consent to be removed, including user and activity group details.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the consent removal operation.</returns>
        public async Task<IBaseResult> RemoveConsent(RemoveConsentArgs args, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync("activitygroups/removeConsent", args);
            return result;
        }

        /// <summary>
        /// Associates an activity group with a category asynchronously.
        /// </summary>
        /// <remarks>This method sends a request to associate the specified activity group with the
        /// specified category. Ensure that both identifiers are valid and exist in the system before calling this
        /// method.</remarks>
        /// <param name="categoryId">The unique identifier of the category to which the activity group will be added. Cannot be null or empty.</param>
        /// <param name="activityGroupId">The unique identifier of the activity group to associate with the category. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> CreateActivityGroupCategoryAsync(string categoryId, string activityGroupId, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync($"activitygroups/categories/add/{categoryId}/{activityGroupId}", "");
            return result;
        }

        /// <summary>
        /// Adds a learner to the specified activity group as a team member.
        /// </summary>
        /// <remarks>This method sends a request to add the specified learner to the activity group.
        /// Ensure that the  <paramref name="activityGroupId"/> and <paramref name="learnerId"/> are valid and that the
        /// user has  the necessary permissions to perform this operation.</remarks>
        /// <param name="activityGroupId">The unique identifier of the activity group to which the learner will be added.</param>
        /// <param name="learnerId">The unique identifier of the learner to be added as a team member.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> CreateActivityGroupTeamMemberAsync(string activityGroupId, string learnerId, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync($"activitygroups/teamMembers/add/{activityGroupId}/{learnerId}", "");
            return result;
        }

        /// <summary>
        /// Removes a team member from the specified activity group.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to remove a team member from an
        /// activity group.  Ensure that the provided identifiers are valid and that the caller has the necessary
        /// permissions  to perform this operation.</remarks>
        /// <param name="activityGroupId">The unique identifier of the activity group from which the team member will be removed.</param>
        /// <param name="learnerId">The unique identifier of the team member to be removed.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveActivityGroupTeamMemberAsync(string activityGroupId, string learnerId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"activitygroups/teamMembers/remove/{activityGroupId}/{learnerId}", "");
            return result;
        }
    }
}
