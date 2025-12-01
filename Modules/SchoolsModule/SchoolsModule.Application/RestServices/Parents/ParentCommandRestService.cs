using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.Parents;

namespace SchoolsModule.Application.RestServices.Parents
{
    /// <summary>
    /// Provides methods for managing parent-related operations, including creating, updating, and removing parent
    /// records, as well as managing parent-learner relationships and exporting parent data.
    /// </summary>
    /// <remarks>This service acts as a REST client for interacting with parent-related endpoints. It provides
    /// functionality to create and update parent records, manage parent profiles, handle parent-learner associations,
    /// and export parent data. Each method communicates with the underlying HTTP provider to perform the corresponding
    /// operation.</remarks>
    /// <param name="provider"></param>
    public class ParentCommandRestService(IBaseHttpProvider provider) : IParentCommandService
    {
        /// <summary>
        /// Creates or updates a parent entity asynchronously.
        /// </summary>
        /// <param name="parent">The parent entity to create or update.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  IBaseResult{T} object with
        /// the created or updated ParentDto.</returns>
        public async Task<IBaseResult<ParentDto>> CreateAsync(ParentDto parent, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<ParentDto, ParentDto>("parents", parent);
            return result;
        }

        /// <summary>
        /// Updates the parent entity asynchronously.
        /// </summary>
        /// <remarks>This method sends the provided <paramref name="parent"/> data to the underlying
        /// provider for updating. Ensure that the <paramref name="parent"/> object is properly populated before calling
        /// this method.</remarks>
        /// <param name="parent">The parent entity to be updated, represented as a <see cref="ParentDto"/> object.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateAsync(ParentDto parent, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync("parents", parent);
            return result;
        }

        /// <summary>
        /// Updates the profile information for a parent asynchronously.
        /// </summary>
        /// <param name="parent">The parent profile data to be updated. Cannot be <see langword="null"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateProfileAsync(ParentDto parent, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync("parents/updateprofile", parent);
            return result;
        }

        /// <summary>
        /// Removes a parent entity asynchronously based on the specified identifier.
        /// </summary>
        /// <param name="parentId">The unique identifier of the parent entity to be removed. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the removal operation.</returns>
        public async Task<IBaseResult> RemoveAsync(string parentId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync("parents", parentId);
            return result;
        }

        /// <summary>
        /// Creates a parent chat group and associates it with the specified parent ID.
        /// </summary>
        /// <remarks>This method sends a request to create a new chat group for the specified parent and
        /// adds the specified group member to it. Ensure that the provided identifiers are valid and that the operation
        /// is not canceled via the <paramref name="cancellationToken"/>.</remarks>
        /// <param name="parentId">The unique identifier of the parent for whom the chat group is being created. Cannot be null or empty.</param>
        /// <param name="groupMemberId">The identifier of the group member to be added to the chat group. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the unique identifier of the
        /// created chat group.</returns>
        public async Task<IBaseResult<string>> CreateParentChatGroupAsync(string parentId, string groupMemberId, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<string, string>($"parent/chats/{parentId}", groupMemberId);
            return result;
        }

        /// <summary>
        /// Creates a parent-learner association asynchronously.
        /// </summary>
        /// <remarks>This method sends a request to associate a learner with a parent. Ensure that both
        /// the parent and learner  identifiers are valid and exist in the system before calling this method.</remarks>
        /// <param name="parentId">The unique identifier of the parent.</param>
        /// <param name="learnerId">The unique identifier of the learner to associate with the parent.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> CreateParentLearnerAsync(string parentId, string learnerId, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<string>($"parent/learners/{parentId}/create", learnerId);
            return result;
        }

        /// <summary>
        /// Removes the association between a parent and a learner asynchronously.
        /// </summary>
        /// <remarks>This method removes the relationship between a parent and a learner in the system.
        /// Ensure that both the parent and learner identifiers are valid and exist in the system before calling this
        /// method.</remarks>
        /// <param name="parentId">The unique identifier of the parent whose association is to be removed. Cannot be <see langword="null"/> or
        /// empty.</param>
        /// <param name="learnerId">The unique identifier of the learner whose association with the parent is to be removed. Cannot be <see
        /// langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveParentLearnerAsync(string parentId, string learnerId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync("parents", parentId);
            return result;
        }

        /// <summary>
        /// Exports parent data and retrieves the result as a string.
        /// </summary>
        /// <remarks>This method sends a request to export parent data and returns the result. The
        /// operation is asynchronous and can be canceled using the provided <paramref
        /// name="cancellationToken"/>.</remarks>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the exported parent data as a string.</returns>
        public async Task<IBaseResult<string>> ExportParents(CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<string>("parents/export");
            return result;
        }
    }
}
