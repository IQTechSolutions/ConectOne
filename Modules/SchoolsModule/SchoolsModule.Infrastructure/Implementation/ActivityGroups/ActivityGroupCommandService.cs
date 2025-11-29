using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ConectOne.Domain.Entities;
using ConectOne.Domain.Extensions;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using FilingModule.Domain.Entities;
using FilingModule.Domain.Enums;
using FilingModule.Domain.Interfaces;
using GroupingModule.Domain.Entities;
using IdentityModule.Domain.Entities;
using IdentityModule.Domain.Interfaces;
using IdentityModule.Domain.RequestFeatures;
using MessagingModule.Domain.Entities;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.Interfaces.ActivityGroups;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Infrastructure.Implementation.ActivityGroups
{
    /// <summary>
    /// Service for handling commands related to Activity Groups including creation, update,
    /// deletion, consent revocation, and category assignments.
    /// </summary>
    /// <remarks>
    /// - Uses ISchoolsModuleRepoManager for primary data operations.
    /// - Uses generic repositories for Notification and Message entities.
    /// - Uses IGroupingRepositoryManager to handle category associations.
    /// </remarks>
    public class ActivityGroupCommandService(ISchoolsModuleRepoManager schoolsModuleRepoManager, IRepository<Notification, string> notificationRepository, IUserService userService,
        IRepository<Message, string> messageRepository, IRepository<ChatGroup, string> chatGroupRepo, IRepository<EntityCategory<ActivityGroup>, string> activityGroupRepo,
        IRepository<ChatGroupMember, string> chatGroupMemberRepo, IRepository<EntityImage<ActivityGroup, string>, string> imageRepo, IImageProcessingService imageProcessingService) 
        : IActivityGroupCommandService
    {
        /// <summary>
        /// Creates a new activity group and persists it to the database.
        /// </summary>
        /// <param name="activityGroup">DTO containing details for the new activity group.</param>
        /// <returns>Result containing the created ActivityGroupDto or error messages.</returns>
        public async Task<IBaseResult<ActivityGroupDto>> CreateAsync(ActivityGroupDto activityGroup, CancellationToken cancellationToken = default)
        {
            if (activityGroup == null)
                return await Result<ActivityGroupDto>.FailAsync("Activity group data must be provided.");

            var newGroup = activityGroup.CreateActivityGroup();
            newGroup.TeamMembers = activityGroup.TeamMembers.Select(tm => new ActivityGroupTeamMember { ActivityGroupId = newGroup.Id, LearnerId = tm.LearnerId }).ToList();

            var folderPath = Path.Combine("StaticFiles", "activitygroup", newGroup.Name);

            //var imageFile = imageProcessingService.CreateImage(folderPath, activityGroup.CoverImageUrl);
            //newGroup.Images.Add(imageFile.ToImageFile<ActivityGroup, string>(newGroup.Id, UploadType.Cover));

            var createResult = await schoolsModuleRepoManager.ActivityGroups.CreateAsync(newGroup, cancellationToken);
            if (!createResult.Succeeded)
                return await Result<ActivityGroupDto>.FailAsync(createResult.Messages);

            if (activityGroup.AutoCreateChatGroup)
            {
                var newChatGroup = new ChatGroup
                {
                    Id = activityGroup.ActivityGroupId,
                    Name = activityGroup.Name
                };

                await chatGroupRepo.CreateAsync(newChatGroup, cancellationToken);
            }

            var saveResult = await schoolsModuleRepoManager.ActivityGroups.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded)
                return await Result<ActivityGroupDto>.FailAsync(saveResult.Messages);

            return await Result<ActivityGroupDto>.SuccessAsync(new ActivityGroupDto(createResult.Data));
        }

        /// <summary>
        /// Updates an existing activity group and synchronizes its team members.
        /// </summary>
        /// <param name="activityGroup">DTO containing updated activity group data.</param>
        /// <returns>Result indicating success or failure.</returns>
        public async Task<IBaseResult> UpdateAsync(ActivityGroupDto activityGroup, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<ActivityGroup>(x => x.Id == activityGroup.ActivityGroupId);
            spec.AddInclude(c => c.Include(i => i.Images));
            spec.AddInclude(g => g.Include(g => g.Teacher).ThenInclude(c => c.EmailAddresses));
            spec.AddInclude(g => g.Include(g => g.TeamMembers).ThenInclude(c => c.Learner).ThenInclude(c => c.Parents).ThenInclude(c => c.Parent).ThenInclude(c => c.EmailAddresses));

            var query = await schoolsModuleRepoManager.ActivityGroups.ListAsync(spec, true, cancellationToken);
            var entity = query.Data.FirstOrDefault();
            if (entity == null) return await Result.FailAsync("Activity group not found.");

            entity.Name = activityGroup.Name;
            entity.Gender = activityGroup.Gender;
            entity.AgeGroupId = activityGroup.AgeGroup?.AgeGroupId;
            entity.TeacherId = activityGroup.Teacher?.TeacherId;
            entity.AutoCreateChatGroup = activityGroup.AutoCreateChatGroup;

            var updateResult = schoolsModuleRepoManager.ActivityGroups.Update(entity);
            if (!updateResult.Succeeded) return await Result.FailAsync(updateResult.Messages);

            if (activityGroup.AutoCreateChatGroup)
                await EnsureChatGroupAsync(entity, cancellationToken);

            if (activityGroup.CoverImageUrl.IsBase64String() && entity.Images.Any(c => c.Image.ImageType == UploadType.Cover))
            {
                await imageRepo.DeleteAsync(entity.Images.First(c => c.Image.ImageType == UploadType.Cover).Id, cancellationToken);
            }

            var folderPath = Path.Combine("StaticFiles", "activitygroup", entity.Name);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            //var file = imageProcessingService.CreateImage(folderPath, activityGroup.CoverImageUrl);
            //if (file != null)
            //    await imageRepo.CreateAsync(file.ToImageFile<ActivityGroup, string>(entity.Id, UploadType.Cover), cancellationToken);

            if (activityGroup.AutoCreateChatGroup)
                await EnsureChatGroupAsync(entity, cancellationToken);

            var syncResult = await SyncTeamMembersAsync(entity.Id, activityGroup.TeamMembers.ToList(), entity.TeamMembers.ToList(), cancellationToken);

            var saveResult = await schoolsModuleRepoManager.ActivityGroups.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded)
                return await Result<ActivityGroupDto>.FailAsync(saveResult.Messages);

            return syncResult;
        }

        /// <summary>
        /// Deletes an activity group and all related messages and notifications.
        /// </summary>
        /// <param name="activityGroupId">The ID of the activity group to delete.</param>
        /// <returns>Result indicating the operation's outcome.</returns>
        public async Task<IBaseResult> DeleteAsync(string activityGroupId, CancellationToken cancellationToken = default)
        {
            var deleteResult = await schoolsModuleRepoManager.ActivityGroups.DeleteAsync(activityGroupId, cancellationToken);
            if (!deleteResult.Succeeded)
                return await Result.FailAsync(deleteResult.Messages);

            var deleteNotifs = await DeleteEntitiesAsync(notificationRepository, x => x.EntityId == activityGroupId, cancellationToken);
            if (!deleteNotifs.Succeeded)
                return deleteNotifs;

            var deleteMessages = await DeleteEntitiesAsync(messageRepository, x => x.EntityId == activityGroupId, cancellationToken);
            if (!deleteMessages.Succeeded)
                return deleteMessages;


            var saveResult = await schoolsModuleRepoManager.ActivityGroups.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded)
                return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync("Activity Group deleted successfully.");
        }

        /// <summary>
        /// Removes a parent's consent for a specific learner and event.
        /// </summary>
        /// <param name="args">Consent removal arguments.</param>
        /// <returns>Result indicating success or failure of the removal.</returns>
        public async Task<IBaseResult> RemoveConsent(RemoveConsentArgs args, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(args.LearnerId) || string.IsNullOrWhiteSpace(args.EventId) || args.ConsentType == null)
                return await Result.FailAsync("LearnerId, EventId, and ConsentType are required.");

            var spec = new LambdaSpec<ParentPermission>(pp =>
                pp.LearnerId == args.LearnerId &&
                pp.EventId == args.EventId &&
                pp.ConsentType == args.ConsentType);

            var result = await schoolsModuleRepoManager.ParentPermissions.ListAsync(spec, true, cancellationToken);
            if (!result.Succeeded) return await Result.FailAsync(result.Messages);

            var filtered = !string.IsNullOrWhiteSpace(args.ParentId)
                ? result.Data.Where(p => p.ParentId == args.ParentId).ToList()
                : result.Data;

            if (!filtered.Any())
                return await Result.FailAsync("No matching consents found to remove.");

            foreach (var permission in filtered)
            {
                schoolsModuleRepoManager.ParentPermissions.Delete(permission);
            }

            var save = await schoolsModuleRepoManager.ParentPermissions.SaveAsync(cancellationToken);
            return save.Succeeded
                ? await Result.SuccessAsync("Consent was successfully revoked.")
                : await Result.FailAsync(save.Messages);
        }

        /// <summary>
        /// Links an activity group to a category.
        /// </summary>
        /// <param name="categoryId">ID of the category.</param>
        /// <param name="activityGroupId">ID of the activity group.</param>
        /// <returns>Result indicating whether the operation was successful.</returns>
        public async Task<IBaseResult> CreateActivityGroupCategoryAsync(string categoryId, string activityGroupId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(categoryId) || string.IsNullOrWhiteSpace(activityGroupId))
                return await Result.FailAsync("Both CategoryId and ActivityGroupId must be provided.");

            var entityCategory = new EntityCategory<ActivityGroup>(activityGroupId, categoryId);

            var createResult = await activityGroupRepo.CreateAsync(entityCategory, cancellationToken);
            if (!createResult.Succeeded)
                return await Result.FailAsync(createResult.Messages);

            var saveResult = await activityGroupRepo.SaveAsync(cancellationToken);
            return saveResult.Succeeded
                ? await Result.SuccessAsync("Activity group category successfully created.")
                : await Result.FailAsync(saveResult.Messages);
        }

        /// <summary>
        /// Creates a new ActivityGroupTeamMember linking a learner to an activity group.
        /// </summary>
        public async Task<IBaseResult> CreateActivityGroupTeamMemberAsync(string activityGroupId, string learnerId, CancellationToken cancellationToken = default)
        {
            var activityGroupTeamMember = new ActivityGroupTeamMember(activityGroupId, learnerId);
            var creationResult = await schoolsModuleRepoManager.ActivityGroupTeamMembers.CreateAsync(activityGroupTeamMember, cancellationToken);
            if (!creationResult.Succeeded) return await Result.FailAsync(creationResult.Messages);

            var saveResult = await schoolsModuleRepoManager.ActivityGroupTeamMembers.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);
            return await Result.SuccessAsync("Team Member successfully added");

        }

        /// <summary>
        /// Removes a learner from an activity group team.
        /// </summary>
        public async Task<IBaseResult> RemoveActivityGroupTeamMemberAsync(string activityGroupId, string learnerId, CancellationToken cancellationToken = default)
        {
            var activityGroupTeamMemberResult = await schoolsModuleRepoManager.ActivityGroupTeamMembers.FirstOrDefaultAsync(new LambdaSpec<ActivityGroupTeamMember>(c => c.ActivityGroupId == activityGroupId && c.LearnerId == learnerId), false, cancellationToken);
            if (!activityGroupTeamMemberResult.Succeeded) return await Result.FailAsync(activityGroupTeamMemberResult.Messages);

            if (activityGroupTeamMemberResult.Data is null) return await Result.FailAsync($"Team member with learner id '{learnerId}' does not exist in the database");

            var removalResult = await schoolsModuleRepoManager.ActivityGroupTeamMembers.DeleteAsync(activityGroupTeamMemberResult.Data?.Id, cancellationToken);
            if (!removalResult.Succeeded) return await Result.FailAsync(removalResult.Messages);

            var saveResult = await schoolsModuleRepoManager.ActivityGroupTeamMembers.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync("Team Member removed successfully");
        }

        #region Helpers

        /// <summary>
        /// Synchronizes the team members of an activity group by adding and removing members as needed.
        /// </summary>
        private async Task<IBaseResult> SyncTeamMembersAsync(string activityGroupId, List<LearnerDto> newMembers, List<ActivityGroupTeamMember> currentMembers, CancellationToken cancellationToken = default)
        {
            var newIds = new HashSet<string>(newMembers.Select(m => m.LearnerId));
            var currentIds = new HashSet<string>(currentMembers.Select(m => m.LearnerId));

            var toAdd = newIds.Except(currentIds);
            var toRemove = currentIds.Except(newIds);

            foreach (var learnerId in toAdd)
            {
                var teamMember = new ActivityGroupTeamMember(activityGroupId, learnerId);
                var result = await schoolsModuleRepoManager.ActivityGroupTeamMembers.CreateAsync(teamMember, cancellationToken);
                if (!result.Succeeded) return await Result.FailAsync(result.Messages);
            }

            foreach (var learnerId in toRemove)
            {
                var memberToRemove = currentMembers.FirstOrDefault(m => m.LearnerId == learnerId);
                if (memberToRemove != null)
                {
                    var deletionResult = await schoolsModuleRepoManager.ActivityGroupTeamMembers.DeleteAsync(memberToRemove.Id, cancellationToken);
                    if (!deletionResult.Succeeded) return await Result.FailAsync(deletionResult.Messages);
                }
            }

            var saveResult = await schoolsModuleRepoManager.ActivityGroupTeamMembers.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync("Team members synced.");
        }

        /// <summary>
        /// Deletes all entities from a repository matching a specific condition.
        /// </summary>
        private async Task<IBaseResult> DeleteEntitiesAsync<T>(IRepository<T, string> repository, Expression<Func<T, bool>> condition, CancellationToken cancellationToken = default) where T : EntityBase<string>
        {
            var spec = new LambdaSpec<T>(condition);
            var result = await repository.ListAsync(spec, true, cancellationToken);

            if (!result.Succeeded)
                return await Result.FailAsync(result.Messages);

            foreach (var entity in result.Data)
            {
                var delete = await repository.DeleteAsync(entity.Id, cancellationToken);
                if (!delete.Succeeded)
                    return await Result.FailAsync(delete.Messages);
            }

            var save = await repository.SaveAsync(cancellationToken);
            return save.Succeeded
                ? await Result.SuccessAsync()
                : await Result.FailAsync(save.Messages);
        }

        /// <summary>
        /// Ensures a chat group exists and is populated for the given school class.
        /// </summary>
        private async Task EnsureChatGroupAsync(ActivityGroup activityGroup, CancellationToken cancellationToken)
        {
            var spec = new LambdaSpec<ChatGroup>(g => g.Id == activityGroup.Id);
            spec.AddInclude(g => g.Include(g => g.Members));

            var chatGroupResult = await chatGroupRepo.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!chatGroupResult.Succeeded) return;

            var chatGroup = chatGroupResult.Data;
            if (chatGroup == null)
            {
                var result = await chatGroupRepo.CreateAsync(new ChatGroup
                {
                    Id = activityGroup.Id,
                    Name = activityGroup.Name
                }, cancellationToken);

                chatGroup = result.Data;
            }

            var users = await userService.AllUsers(new UserPageParameters());
            //await AddMembersToChatGroupAsync(users, chatGroup, activityGroup.Teacher.EmailAddresses, cancellationToken);
            //await AddMembersToChatGroupAsync(users, chatGroup, activityGroup.TeamMembers.SelectMany(g => g.Learner.Parents.SelectMany(p => p.Parent.EmailAddresses)), cancellationToken);
        }

        /// <summary>
        /// Adds users to chat group if not already members.
        /// </summary>
        private async Task AddMembersToChatGroupAsync(IEnumerable<ApplicationUser> users, ChatGroup chatGroup, IEnumerable<EmailAddress> emails, CancellationToken cancellationToken)
        {
            foreach (var email in emails.Select(e => e.Email).Distinct())
            {
                var user = users.FirstOrDefault(u => u.Email == email);
                if (user != null && chatGroup.Members.All(m => m.UserId != user.Id))
                {
                    await chatGroupMemberRepo.CreateAsync(new ChatGroupMember
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = user.Id,
                        GroupId = chatGroup.Id
                    }, cancellationToken);
                }
            }
        }

        #endregion
    }
}
