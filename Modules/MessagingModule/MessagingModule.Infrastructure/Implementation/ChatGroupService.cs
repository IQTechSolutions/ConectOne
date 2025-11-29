using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using IdentityModule.Domain.DataTransferObjects;
using MessagingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.Entities;
using MessagingModule.Domain.Interfaces;
using MessagingModule.Domain.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace MessagingModule.Infrastructure.Implementation
{
    /// <summary>
    /// Service for managing chat groups and their members.
    /// Handles creation, updating, deletion, and retrieval of group data.
    /// </summary>
    public class ChatGroupService(IRepository<ChatGroup, string> chatGroupRepo, IRepository<ChatGroupMember, string> chatGroupMemberRepo) : IChatGroupService
    {
        /// <summary>
        /// Retrieves all chat groups in the system.
        /// </summary>
        public async Task<IBaseResult<IEnumerable<ChatGroupDto>>> ChatGroups(CancellationToken cancellationToken = default)
        {
            var result = await chatGroupRepo.ListAsync(false, cancellationToken);
            if (!result.Succeeded) return await Result<IEnumerable<ChatGroupDto>>.FailAsync(result.Messages);

            return await Result<IEnumerable<ChatGroupDto>>.SuccessAsync(result.Data.Select(c => new ChatGroupDto(c)));
        }

        /// <summary>
        /// Retrieves all chat groups that the specified user is a member of.
        /// </summary>
        public async Task<IBaseResult<IEnumerable<ChatGroupDto>>> ChatGroups(string userId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<ChatGroupMember>(c => c.UserId == userId);
            spec.AddInclude(c => c.Include(g => g.Group));

            var result = await chatGroupMemberRepo.ListAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<IEnumerable<ChatGroupDto>>.FailAsync(result.Messages);

            return await Result<IEnumerable<ChatGroupDto>>.SuccessAsync(result.Data.Select(c => new ChatGroupDto(c.Group)));
        }

        /// <summary>
        /// Creates a new chat group and assigns members to it.
        /// </summary>
        public async Task<IBaseResult<ChatGroupDto>> CreateGroupAsync(AddUpdateChatGroupRequest request, CancellationToken cancellationToken = default)
        {
            var group = new ChatGroup { Id = request.Id, Name = request.Name };
            await chatGroupRepo.CreateAsync(group, cancellationToken);

            foreach (var userId in request.UserIds)
            {
                await chatGroupMemberRepo.CreateAsync(new ChatGroupMember { GroupId = group.Id, UserId = userId }, cancellationToken);
            }

            var saveResult = await chatGroupRepo.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result<ChatGroupDto>.FailAsync(saveResult.Messages);

            return await Result<ChatGroupDto>.SuccessAsync(new ChatGroupDto(group));
        }

        /// <summary>
        /// Updates the metadata (e.g., name) of an existing chat group.
        /// </summary>
        public async Task<IBaseResult> UpdateGroupAsync(AddUpdateChatGroupRequest dto, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<ChatGroup>(g => g.Id == dto.Id);
            var groupResult = await chatGroupRepo.FirstOrDefaultAsync(spec, false, cancellationToken);

            if (!groupResult.Succeeded) return await Result<bool>.FailAsync(groupResult.Messages);
            if (groupResult.Data == null) return await Result<bool>.FailAsync("Group not found");

            groupResult.Data.Name = dto.Name;

            var saveResult = await chatGroupRepo.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result<bool>.FailAsync(saveResult.Messages);

            return await Result<bool>.SuccessAsync(true);
        }

        /// <summary>
        /// Deletes a chat group and all its associated members.
        /// </summary>
        public async Task<IBaseResult> DeleteGroupAsync(string groupId, CancellationToken cancellationToken = default)
        {
            var membersResult = await chatGroupMemberRepo.ListAsync(
                new LambdaSpec<ChatGroupMember>(m => m.GroupId == groupId), false, cancellationToken);

            if (!membersResult.Succeeded) return await Result<bool>.FailAsync(membersResult.Messages);

            chatGroupMemberRepo.RemoveRange(membersResult.Data);
            await chatGroupRepo.DeleteAsync(groupId, cancellationToken);

            var saveResult = await chatGroupRepo.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result<bool>.FailAsync(saveResult.Messages);

            return await Result<bool>.SuccessAsync(true);
        }

        /// <summary>
        /// Retrieves the members of a specified chat group.
        /// </summary>
        public async Task<IBaseResult<List<UserInfoDto>>> GetGroupMembersAsync(string groupId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<ChatGroupMember>(m => m.GroupId == groupId);
            spec.AddInclude(m => m.Include(g => g.User).ThenInclude(c => c.UserInfo).ThenInclude(c => c.Images));

            var membersResult = await chatGroupMemberRepo.ListAsync(spec, false, cancellationToken);

            if (!membersResult.Succeeded) return await Result<List<UserInfoDto>>.FailAsync(membersResult.Messages);
            if (membersResult.Data == null || !membersResult.Data.Any()) return await Result<List<UserInfoDto>>.SuccessAsync(new List<UserInfoDto>());

            var users = membersResult.Data.Select(u => new UserInfoDto(u.User)).ToList();
            return await Result<List<UserInfoDto>>.SuccessAsync(users);
        }

        /// <summary>
        /// Replaces the current members of a chat group with a new list of users.
        /// </summary>
        public async Task<IBaseResult> UpdateGroupMembersAsync(AddUpdateChatGroupRequest request, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<ChatGroupMember>(g => g.GroupId == request.Id);
            var result = await chatGroupMemberRepo.ListAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result.FailAsync(result.Messages);

            chatGroupMemberRepo.RemoveRange(result.Data);

            foreach (var userId in request.UserIds)
            {
                await chatGroupMemberRepo.CreateAsync(new ChatGroupMember
                {
                    GroupId = request.Id,
                    UserId = userId
                }, cancellationToken);
            }

            var saveResult = await chatGroupMemberRepo.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync("Group members updated successfully.");
        }
    }
}
