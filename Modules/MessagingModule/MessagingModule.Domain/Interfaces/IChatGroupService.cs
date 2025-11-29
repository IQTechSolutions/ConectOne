using ConectOne.Domain.ResultWrappers;
using IdentityModule.Domain.DataTransferObjects;
using MessagingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.RequestFeatures;

namespace MessagingModule.Domain.Interfaces
{
    /// <summary>
    /// Defines operations for managing chat groups and their members,
    /// including creating, updating, retrieving, and deleting groups.
    /// </summary>
    public interface IChatGroupService
    {
        /// <summary>
        /// Retrieves all chat groups in the system.
        /// </summary>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>A result containing a collection of chat groups.</returns>
        Task<IBaseResult<IEnumerable<ChatGroupDto>>> ChatGroups(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all chat groups that a specific user is a member of.
        /// </summary>
        /// <param name="userId">The user ID whose group memberships are being retrieved.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>A result containing a collection of chat groups.</returns>
        Task<IBaseResult<IEnumerable<ChatGroupDto>>> ChatGroups(string userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new chat group with the specified members.
        /// </summary>
        /// <param name="request">The request containing group name and user IDs.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>A result containing the newly created chat group.</returns>
        Task<IBaseResult<ChatGroupDto>> CreateGroupAsync(AddUpdateChatGroupRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the metadata (e.g., name) of an existing chat group.
        /// </summary>
        /// <param name="dto">The DTO containing updated group information.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>A result indicating success or failure.</returns>
        Task<IBaseResult> UpdateGroupAsync(AddUpdateChatGroupRequest dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an existing chat group and its related member entries.
        /// </summary>
        /// <param name="groupId">The ID of the chat group to delete.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>A result indicating success or failure.</returns>
        Task<IBaseResult> DeleteGroupAsync(string groupId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the members of a specific chat group.
        /// </summary>
        /// <param name="groupId">The ID of the chat group.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>A result containing a list of user details.</returns>
        Task<IBaseResult<List<UserInfoDto>>> GetGroupMembersAsync(string groupId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the user membership of a chat group by replacing the entire user list.
        /// </summary>
        /// <param name="request">The request containing group ID and updated list of user IDs.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>A result indicating success or failure.</returns>
        Task<IBaseResult> UpdateGroupMembersAsync(AddUpdateChatGroupRequest request, CancellationToken cancellationToken = default);
    }

}
