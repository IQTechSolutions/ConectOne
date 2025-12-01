using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using IdentityModule.Domain.DataTransferObjects;
using MessagingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.Interfaces;
using MessagingModule.Domain.RequestFeatures;

namespace MessagingModule.Application.RestServices
{
    /// <summary>
    /// Provides REST-based operations for managing chat groups, including creating, updating, deleting groups, and
    /// retrieving group information and members.
    /// </summary>
    /// <remarks>This service implements group management functionality by communicating with a remote REST
    /// API. All methods are asynchronous and support cancellation via a CancellationToken. Instances of this class are
    /// typically used in applications that require integration with a chat group's backend service.</remarks>
    /// <param name="provider">The HTTP provider used to perform REST API requests for chat group operations.</param>
    public class ChatGroupRestService(IBaseHttpProvider provider) : IChatGroupService
    {
        /// <summary>
        /// Retrieves a collection of chat groups available to the current user.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// with an enumerable collection of <see cref="ChatGroupDto"/> objects representing the chat groups.</returns>
        public async Task<IBaseResult<IEnumerable<ChatGroupDto>>> ChatGroups(CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<ChatGroupDto>>("chats/groups");
            return result;
        }

        /// <summary>
        /// Retrieves the list of chat groups associated with the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose chat groups are to be retrieved. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with a collection of <see cref="ChatGroupDto"/> objects representing the user's chat groups.</returns>
        public async Task<IBaseResult<IEnumerable<ChatGroupDto>>> ChatGroups(string userId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<ChatGroupDto>>($"chats/groups/{userId}");
            return result;
        }

        /// <summary>
        /// Creates a new chat group asynchronously with the specified group details.
        /// </summary>
        /// <param name="request">An object containing the information required to create the chat group. Cannot be null.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="IBaseResult{ChatGroupDto}"/> with the details of the created chat group.</returns>
        public async Task<IBaseResult<ChatGroupDto>> CreateGroupAsync(AddUpdateChatGroupRequest request, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<ChatGroupDto, AddUpdateChatGroupRequest>("chats/groups", request);
            return result;
        }

        /// <summary>
        /// Updates an existing chat group with the specified details asynchronously.
        /// </summary>
        /// <param name="dto">An object containing the updated information for the chat group. Cannot be null.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateGroupAsync(AddUpdateChatGroupRequest dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<AddUpdateChatGroupRequest>("chats/groups", dto);
            return result;
        }

        /// <summary>
        /// Deletes the specified group asynchronously.
        /// </summary>
        /// <param name="groupId">The unique identifier of the group to delete. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the delete operation.</param>
        /// <returns>A task that represents the asynchronous delete operation. The task result contains an <see
        /// cref="IBaseResult"/> indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> DeleteGroupAsync(string groupId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"chats/groups", groupId);
            return result;
        }

        /// <summary>
        /// Asynchronously retrieves the list of members belonging to the specified group.
        /// </summary>
        /// <param name="groupId">The unique identifier of the group whose members are to be retrieved. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with a list of <see cref="UserInfoDto"/> objects representing the group's members.</returns>
        public async Task<IBaseResult<List<UserInfoDto>>> GetGroupMembersAsync(string groupId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<List<UserInfoDto>>($"chats/groups/members/{groupId}");
            return result;
        }

        /// <summary>
        /// Updates the members of a chat group asynchronously based on the specified request.
        /// </summary>
        /// <param name="request">An object containing the details of the group and the members to add or update. Cannot be null.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an object indicating the outcome
        /// of the update operation.</returns>
        public async Task<IBaseResult> UpdateGroupMembersAsync(AddUpdateChatGroupRequest request, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync("chats/groups/members", request);
            return result;
        }
    }
}
