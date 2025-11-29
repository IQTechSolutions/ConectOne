using ConectOne.Domain.Entities;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using FilingModule.Domain.Entities;
using FilingModule.Domain.Enums;
using FilingModule.Domain.Interfaces;
using IdentityModule.Domain.Interfaces;
using IdentityModule.Domain.RequestFeatures;
using MessagingModule.Domain.Interfaces;
using MessagingModule.Domain.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.Interfaces.Parents;

namespace SchoolsModule.Infrastructure.Implementation.Parents
{
    /// <summary>
    /// Service responsible for executing command operations (create, update, delete) on Parent entities.
    /// </summary>
    public class ParentCommandService(ISchoolsModuleRepoManager schoolsModuleRepoManager, IExcelService excelService, IChatGroupService chatGroupService,
        IRepository<EntityImage<Parent, string>, string> parentImageRepo, IUserService userService) : IParentCommandService
    {
        /// <summary>
        /// Creates a new Parent entity from a DTO and saves it to the database.
        /// </summary>
        /// <param name="parent">DTO containing parent data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Success result with the updated ParentDto or failure result with error messages.</returns>
        public async Task<IBaseResult<ParentDto>> CreateAsync(ParentDto parent, CancellationToken cancellationToken)
        {
            if (parent is null)
                throw new ArgumentNullException(nameof(parent));

            var entity = parent.CreateParent();
            MapContactInfo(parent, entity);
            MapLearners(parent, entity);

            var parentResult = await schoolsModuleRepoManager.Parents.CreateAsync(entity, cancellationToken);
            if (!parentResult.Succeeded) return await Result<ParentDto>.FailAsync(parentResult.Messages);

            var saveResult = await schoolsModuleRepoManager.Parents.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result<ParentDto>.FailAsync(saveResult.Messages);

            parent.ParentId = parentResult.Data.Id;
            return await Result<ParentDto>.SuccessAsync(parent);
        }

        /// <summary>
        /// Updates the specified parent entity and its associated learners, addresses, and images.
        /// </summary>
        /// <remarks>This method performs the following operations: <list type="bullet"> <item>Fetches the
        /// parent entity from the database, including its related addresses and learners.</item> <item>Adds new
        /// learners that are not already associated with the parent.</item> <item>Removes learners that are no longer
        /// associated with the parent.</item> <item>Updates the parent entity's details, including its images and
        /// consent settings.</item> <item>Saves all changes to the database.</item> </list> If the parent entity does
        /// not exist, or if any operation fails, the method returns a failure result.</remarks>
        /// <param name="parent">The <see cref="ParentDto"/> containing the updated details of the parent entity.</param>
        /// <param name="cancellation">A <see cref="CancellationToken"/> that can be used to cancel the operation. Defaults to <see
        /// langword="default"/> if not provided.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating  the success or failure of the update operation.</returns>
        public async Task<IBaseResult> UpdateAsync(ParentDto parent, CancellationToken cancellation = default)
        {
            // Step 1: Fetch parent entity using a LambdaSpec with eager loading
            var spec = new LambdaSpec<Parent>(c => c.Id == parent.ParentId);
            spec.AddInclude(q => q.Include(p => p.Addresses));
            spec.AddInclude(q => q.Include(p => p.Learners).ThenInclude(lp => lp.Learner));

            var parentResult = await schoolsModuleRepoManager.Parents.FirstOrDefaultAsync(spec, true, cancellation);
            if (!parentResult.Succeeded)
                return await Result.FailAsync(parentResult.Messages);

            if (parentResult.Data == null)
                return await Result.FailAsync($"No parent with id '{parent.ParentId}' was found in the database");

            var parentEntity = parentResult.Data;

            // Step 2: Add new learners not already associated
            foreach (var learner in parent.Learners.Where(dto => parentEntity.Learners.All(lp => lp.LearnerId != dto.LearnerId)))
            {
                var creationResult = await schoolsModuleRepoManager.LearnerParents.CreateAsync(new LearnerParent
                {
                    ParentId = parentEntity.Id,
                    LearnerId = learner.LearnerId
                }, cancellation);

                if (!creationResult.Succeeded)
                    return await Result.FailAsync(creationResult.Messages);
            }

            // Step 3: Remove learners that are no longer associated
            foreach (var learner in parentEntity.Learners.Where(lp => parent.Learners.All(dto => dto.LearnerId != lp.LearnerId)))
            {
                var removalResult = await RemoveParentLearnerAsync(learner.ParentId!, learner.LearnerId!, cancellation);
                if (!removalResult.Succeeded)
                    return removalResult;
            }

            var learnerParentSaveResult = await schoolsModuleRepoManager.LearnerParents.SaveAsync(cancellation);
            if (!learnerParentSaveResult.Succeeded)
                return await Result.FailAsync(learnerParentSaveResult.Messages);

            // Step 4: Update parent entity
            var updateResult = schoolsModuleRepoManager.Parents.Update(parent.UpdateParent(parentEntity));
            if (!updateResult.Succeeded)
                return await Result.FailAsync(updateResult.Messages);

            if (parentEntity.Images.Any(c => c.Image.ImageType == UploadType.Cover))
            {
                await parentImageRepo.DeleteAsync(parentEntity.Images.First(c => c.Image.ImageType == UploadType.Cover).Id, cancellation);
            }

            var folderPath = Path.Combine("StaticFiles", "activitygroup", parentEntity.FirstName+" "+parentEntity.LastName);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            //var file = imageProcessingService.CreateImage(folderPath, parent.CoverImageUrl);
            //if (file != null)
            //    await parentImageRepo.CreateAsync(file.ToImageFile<Parent, string>(parentEntity.Id, UploadType.Cover), cancellation);

            // Step 5: Update learner consent
            var consentSpec = new LambdaSpec<LearnerParent>(lp => lp.ParentId == parentEntity.Id);
            var learnerParentsResult = await schoolsModuleRepoManager.LearnerParents.ListAsync(consentSpec);
            if (learnerParentsResult.Succeeded)
            {
                foreach (var lp in learnerParentsResult.Data)
                {
                    lp.ParentConsentRequired = parent.RequireConsent;
                    schoolsModuleRepoManager.LearnerParents.Update(lp);
                }
            }

            // Step 6: Final save
            var saveResult = await schoolsModuleRepoManager.Parents.SaveAsync();
            if (!saveResult.Succeeded)
                return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync("Parent updated successfully.");
        }

        /// <summary>
        /// Updates only profile details of an existing Parent entity.
        /// </summary>
        /// <param name="parent">DTO containing updated profile data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Success or failure result indicating the outcome.</returns>
        public async Task<IBaseResult> UpdateProfileAsync(ParentDto parent, CancellationToken cancellationToken = default)
        {
            var parentEntity = await LoadParentAsync(parent.ParentId, cancellationToken);
            if (!parentEntity.Succeeded) return parentEntity;

            var updateResponse = schoolsModuleRepoManager.Parents.Update(parent.UpdateParent(parentEntity.Data));
            if (!updateResponse.Succeeded) return await Result.FailAsync(updateResponse.Messages);

            UpdateLearnerConsent(parent.RequireConsent, parentEntity.Data.Id);

            var saveResult = await schoolsModuleRepoManager.Parents.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync("Parent Updated successfully");
        }

        /// <summary>
        /// Removes a parent and associated learner relationships and consents.
        /// </summary>
        /// <param name="parentId">The ID of the parent to remove.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Success or failure result based on the outcome.</returns>
        public async Task<IBaseResult> RemoveAsync(string parentId, CancellationToken cancellationToken = default)
        {
            var learnerParentsResult = await schoolsModuleRepoManager.LearnerParents.ListAsync(new LambdaSpec<LearnerParent>(c => c.ParentId == parentId), false, cancellationToken);
            if (learnerParentsResult.Succeeded)
            {
                foreach (var learner in learnerParentsResult.Data)
                {
                    schoolsModuleRepoManager.LearnerParents.Delete(learner);
                }
            }

            var eventConsents = await schoolsModuleRepoManager.ParentPermissions.ListAsync(new LambdaSpec<ParentPermission>(c => c.ParentId == parentId), false, cancellationToken);
            if (eventConsents.Succeeded)
            {
                foreach (var eventConsent in eventConsents.Data)
                {
                    schoolsModuleRepoManager.ParentPermissions.Delete(eventConsent);
                }
            }

            await schoolsModuleRepoManager.Parents.DeleteAsync(parentId, cancellationToken);

            var saveResult = await schoolsModuleRepoManager.Parents.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync("Parent Removed successfully");
        }

        /// <summary>
        /// Creates a learner-parent relationship.
        /// </summary>
        /// <param name="parentId">ID of the parent.</param>
        /// <param name="learnerId">ID of the learner.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Result indicating the creation success or failure.</returns>
        public async Task<IBaseResult> CreateParentLearnerAsync(string parentId, string learnerId, CancellationToken cancellationToken = default)
        {
            var creation = await schoolsModuleRepoManager.LearnerParents.CreateAsync(new LearnerParent(learnerId, parentId));
            if (!creation.Succeeded) return await Result<List<LearnerDto>>.FailAsync(creation.Messages);

            var save = await schoolsModuleRepoManager.LearnerParents.SaveAsync();
            if (!save.Succeeded) return await Result<List<LearnerDto>>.FailAsync(save.Messages);

            return await Result<List<LearnerDto>>.SuccessAsync("Learner created successfully");
        }

        /// <summary>
        /// Removes the relationship between a parent and learner.
        /// </summary>
        /// <param name="parentId">ID of the parent.</param>
        /// <param name="learnerId">ID of the learner.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Result indicating the removal success or failure.</returns>
        public async Task<IBaseResult> RemoveParentLearnerAsync(string parentId, string learnerId, CancellationToken cancellationToken = default)
        {
            var lookup = await schoolsModuleRepoManager.LearnerParents.FirstOrDefaultAsync(new LambdaSpec<LearnerParent>(c => c.ParentId == parentId && c.LearnerId == learnerId), false, cancellationToken);
            if (!lookup.Succeeded || lookup.Data == null) return await Result.FailAsync(lookup.Messages);

            var delete = await schoolsModuleRepoManager.LearnerParents.DeleteAsync(lookup.Data.Id);
            if (!delete.Succeeded) return await Result.FailAsync(delete.Messages);

            var save = await schoolsModuleRepoManager.LearnerParents.SaveAsync();
            if (!save.Succeeded) return await Result.FailAsync(save.Messages);

            return await Result.SuccessAsync();
        }

        /// <summary>
        /// Exports a list of parents to an Excel file, including their contact and learner information.
        /// </summary>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>
        /// A result containing the path or content reference to the exported Excel file,
        /// or an error message if the operation fails.
        /// </returns>
        public async Task<IBaseResult<string>> ExportParents(CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Parent>(p => true);
            spec.AddInclude(q => q.Include(p => p.Learners));
            spec.AddInclude(q => q.Include(p => p.ContactNumbers));
            spec.AddInclude(q => q.Include(p => p.EmailAddresses));

            var result = await schoolsModuleRepoManager.Parents.ListAsync(spec, false, cancellationToken);
            if (!result.Succeeded)
                return await Result<string>.FailAsync(result.Messages);

            var export = await excelService.ExportAsync(result.Data, new Dictionary<string, Func<Parent, object>>
            {
                { "Id", p => p.Id },
                { "FirstName", p => p.FirstName },
                { "LastName", p => p.LastName },
                { "Email", p =>
                    p.EmailAddresses.FirstOrDefault(e => e.Default)?.Email ??
                    p.EmailAddresses.FirstOrDefault()?.Email ?? string.Empty
                },
                { "PhoneNr", p =>
                    p.ContactNumbers.FirstOrDefault(c => c.Default)?.Number ??
                    p.ContactNumbers.FirstOrDefault()?.Number ?? string.Empty
                },
                { "Children", p => p.Learners.Count }
            }, sheetName: "Parents");

            return await Result<string>.SuccessAsync(data:export.Data);
        }

        #region Chats

        /// <summary>
        /// Creates a new chat group for a parent and adds specified members to the group.
        /// </summary>
        /// <remarks>This method performs several operations to create a parent-specific chat group: <list
        /// type="bullet"> <item>Validates the parent by checking their existence and registration status.</item>
        /// <item>Creates a chat group with the parent and the specified member.</item> <item>Saves the changes to the
        /// repository.</item> </list> If any of these steps fail, the method returns a failure result with appropriate
        /// error messages.</remarks>
        /// <param name="parentId">The unique identifier of the parent for whom the chat group is being created.</param>
        /// <param name="groupMemberId">The unique identifier of the member to be added to the chat group along with the parent.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>An <see cref="IBaseResult{T}"/> containing the unique identifier of the created chat group if the operation
        /// succeeds; otherwise, an error result with failure messages.</returns>
        public async Task<IBaseResult<string>> CreateParentChatGroupAsync(string parentId, string groupMemberId, CancellationToken cancellationToken = default)
        {
            var parentSpec = new LambdaSpec<Parent>(c => c.Id == parentId);
            parentSpec.AddInclude(q => q.Include(p => p.EmailAddresses));

            var parentResult = await schoolsModuleRepoManager.Parents.FirstOrDefaultAsync(parentSpec, true, cancellationToken);
            if (!parentResult.Succeeded) return await Result<string>.FailAsync(parentResult.Messages);

            var userList = await userService.AllUsers(new UserPageParameters());
            var user = userList.Data.FirstOrDefault(u => parentResult.Data.EmailAddresses.Any(e => e.Email == u?.EmailAddress));
       
            if (user == null) return await Result<string>.FailAsync("Parent is not registered");

            var groupResult = await chatGroupService.CreateGroupAsync(new AddUpdateChatGroupRequest() { Id = Guid.NewGuid().ToString(), Name = $"{parentResult.Data.FirstName} {parentResult.Data.LastName}", UserIds = new List<string>() { groupMemberId, user.UserId } }, cancellationToken);
            if (!groupResult.Succeeded) return await Result<string>.FailAsync(groupResult.Messages);

            var saveResult = await schoolsModuleRepoManager.Parents.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result<string>.FailAsync(saveResult.Messages);

            return await Result<string>.SuccessAsync(data: groupResult.Data.Id);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Maps contact info from DTO to entity.
        /// </summary>
        private static void MapContactInfo(ParentDto dto, Parent entity)
        {
            entity.ContactNumbers = dto.ContactNumbers.Select(c => new ContactNumber<Parent>(c.Number, c.Default)).ToList();
            entity.EmailAddresses = dto.EmailAddresses.Select(c => new EmailAddress<Parent>(c.EmailAddress, c.Default)).ToList();
            entity.EmergencyContacts = dto.EmergencyContacts.Select(contactDto =>
            {
                var contact = contactDto.ToEntity();
                if (string.IsNullOrEmpty(contact.ParentId) && !string.IsNullOrEmpty(entity.Id))
                {
                    contact.ParentId = entity.Id;
                }

                contact.Parent = entity;
                return contact;
            }).ToList();
        }

        /// <summary>
        /// Maps learner info from DTO to entity.
        /// </summary>
        private static void MapLearners(ParentDto dto, Parent entity)
        {
            entity.Learners = dto.Learners.Select(c => new LearnerParent { ParentId = entity.Id, LearnerId = c.LearnerId }).ToList();
        }

        /// <summary>
        /// Loads a parent by ID using the repository.
        /// </summary>
        private async Task<IBaseResult<Parent>> LoadParentAsync(string parentId, CancellationToken cancellationToken)
        {
            var spec = new LambdaSpec<Parent>(c => c.Id == parentId);
            spec.AddInclude(q => q.Include(p => p.Learners).ThenInclude(c => c.Learner));

            var result = await schoolsModuleRepoManager.Parents.FirstOrDefaultAsync(spec, true, cancellationToken);
            return result.Data is null ? await Result<Parent>.FailAsync($"No parent with id '{parentId}' was found") : await Result<Parent>.SuccessAsync(result.Data);
        }

        /// <summary>
        /// Updates learner parent consent based on current requirement.
        /// </summary>
        private void UpdateLearnerConsent(bool requireConsent, string parentId)
        {
            var learnerParents = schoolsModuleRepoManager.LearnerParents.ListAsync(new LambdaSpec<LearnerParent>(c => c.ParentId == parentId)).Result;
            if (!learnerParents.Succeeded) return;

            foreach (var lp in learnerParents.Data)
            {
                lp.ParentConsentRequired = requireConsent;
                schoolsModuleRepoManager.LearnerParents.Update(lp);
            }
        }

        #endregion
    }
}
