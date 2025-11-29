using ConectOne.Domain.Entities;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using FilingModule.Domain.Entities;
using FilingModule.Domain.Enums;
using MessagingModule.Domain.Entities;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.Interfaces.Learners;
using SchoolsModule.Domain.Specifications;

namespace SchoolsModule.Infrastructure.Implementation.Learners
{
    /// <summary>
    /// Provides commands for creating, updating, and deleting Learners along with handling related data such as parents, notifications, and messages.
    /// </summary>
    public class LearnerCommandService(ISchoolsModuleRepoManager schoolsModuleRepoManager, IRepository<Notification, string> notificationRepository, IRepository<Message, string> messageRepository,
        IRepository<EntityImage<Learner, string>, string> learnerImageRepo) : ILearnerCommandService
    {
        /// <summary>
        /// Creates a new learner along with initial contact, email, and parent associations.
        /// </summary>
        /// <param name="learnerDto">The DTO containing learner data to be persisted.</param>
        /// <param name="cancellationToken">Optional cancellation token for the async operation.</param>
        /// <returns>The created learner as a <see cref="LearnerDto"/>, wrapped in a result object.</returns>
        public async Task<IBaseResult<LearnerDto>> CreateAsync(LearnerDto learnerDto, CancellationToken cancellationToken = default)
        {
            var learnerId = string.IsNullOrEmpty(learnerDto.LearnerId) ? Guid.NewGuid().ToString() : learnerDto.LearnerId;

            var learner = new Learner
            {
                Id = learnerId,
                SchoolUid = Guid.NewGuid().ToString(),
                ChildGuid = learnerId,
                FirstName = learnerDto.FirstName!,
                MiddleName = learnerDto.MiddleName,
                LastName = learnerDto.LastName!,
                Description = learnerDto.Description,
                MedicalNotes = learnerDto.MedicalNotes,
                MedicalAidParentId = learnerDto.MedicalAidParent?.ParentId,
                IdNumber = learnerDto.IdNumber!,
                Gender = learnerDto.Gender,
                SchoolGradeId = learnerDto.Grade?.SchoolGradeId,
                SchoolClassId = learnerDto.SchoolClass?.SchoolClassId,
                ReceiveNotifications = learnerDto.ReceiveNotifications,
                ReceiveMessages = learnerDto.ReceiveMessages,
                RecieveEmails = learnerDto.RecieveEmails,
                ContactNumbers = new List<ContactNumber<Learner>> {
                new() {
                    Number = learnerDto.PhoneNr ?? string.Empty,
                    InternationalCode = "27",
                    AreaCode = "",
                    Default = true
                }
            },
                EmailAddresses = new List<EmailAddress<Learner>> {
                new() {
                    Email = learnerDto.EmailAddress ?? string.Empty,
                    Default = true
                }
            },
                Parents = learnerDto.SelectedParents
                    .Select(p => new LearnerParent(learnerId, p.ParentId))
                    .ToList()
            };

            var folderPath = Path.Combine("StaticFiles", "activitygroup", learner.FirstName+" "+learner.LastName);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            //var imageFile = ImageFileFactoryExtensions.ResizeImage(folderPath, learnerDto.CoverImageUrl);
            //learner.Images.Add(imageFile.ToImageFile<Learner, string>(learner.Id, UploadType.Cover));

            var createResult = await schoolsModuleRepoManager.Learners.CreateAsync(learner, cancellationToken);
            if (!createResult.Succeeded)
                return await Result<LearnerDto>.FailAsync(createResult.Messages);

            var parentCreateResult = await schoolsModuleRepoManager.LearnerParents.CreateRangeAsync(learner.Parents);
            if (!parentCreateResult.Succeeded)
                return await Result<LearnerDto>.FailAsync(parentCreateResult.Messages);

            var parentsSaveResult = await schoolsModuleRepoManager.LearnerParents.SaveAsync(cancellationToken);
            if (!parentsSaveResult.Succeeded)
                return await Result<LearnerDto>.FailAsync(parentsSaveResult.Messages);

            var saveResult = await schoolsModuleRepoManager.Learners.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded)
                return await Result<LearnerDto>.FailAsync(saveResult.Messages);

            return await Result<LearnerDto>.SuccessAsync(new LearnerDto(learner, true));
        }

        /// <summary>
        /// Updates learner information and synchronizes parent relationships.
        /// </summary>
        /// <param name="learnerDto">The DTO containing updated learner information.</param>
        /// <param name="cancellationToken">Optional cancellation token for the async operation.</param>
        /// <returns>A result indicating success or failure of the update operation.</returns>
        public async Task<IBaseResult> UpdateAsync(LearnerDto learnerDto, CancellationToken cancellationToken = default)
        {
            var spec = new SingleLearnerWithParentDetailsSpecification(learnerDto.LearnerId!);
            var learnerResult = await schoolsModuleRepoManager.Learners.FirstOrDefaultAsync(spec, true, cancellationToken);
            if (!learnerResult.Succeeded)
                return await Result<LearnerDto>.FailAsync(learnerResult.Messages);

            var learner = learnerResult.Data;
            if (learner == null)
                return await Result<LearnerDto>.FailAsync($"No learner matching id '{learnerDto.LearnerId}' found in the datastore");

            learner.ChildGuid = learnerDto.LearnerId!;
            learner.FirstName = learnerDto.FirstName!;
            learner.MiddleName = learnerDto.MiddleName;
            learner.LastName = learnerDto.LastName!;
            learner.Description = learnerDto.Description;
            learner.MedicalNotes = learnerDto.MedicalNotes;
            learner.MedicalAidParentId = learnerDto.MedicalAidParent.ParentId;
            learner.IdNumber = learnerDto.IdNumber ?? "";
            learner.Gender = learnerDto.Gender;
            learner.SchoolGradeId = learnerDto.Grade?.SchoolGradeId;
            learner.SchoolClassId = learnerDto.SchoolClass?.SchoolClassId;
            learner.ReceiveNotifications = learnerDto.ReceiveNotifications;
            learner.ReceiveMessages = learnerDto.ReceiveMessages;
            learner.RecieveEmails = learnerDto.RecieveEmails;

            var updateResult = schoolsModuleRepoManager.Learners.Update(learner);
            if (!updateResult.Succeeded)
                return await Result<LearnerDto>.FailAsync(updateResult.Messages);

            if (learner.Images.Any(c => c.Image.ImageType == UploadType.Cover))
            {
                await learnerImageRepo.DeleteAsync(learner.Images.First(c => c.Image.ImageType == UploadType.Cover).Id, cancellationToken);
            }

            var folderPath = Path.Combine("StaticFiles", "activitygroup", learner.FirstName+" "+learner.LastName);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // Sync parent relationships
            var existingParentIds = learner.Parents.Select(p => p.ParentId).ToHashSet();
            var newParentIds = learnerDto.SelectedParents.Select(p => p.ParentId).ToHashSet();

            var toAdd = newParentIds.Except(existingParentIds);
            var toRemove = existingParentIds.Except(newParentIds);

            foreach (var parentId in toAdd)
            {
                var parentResult = schoolsModuleRepoManager.Parents.FindByCondition(p => p.Id == parentId, false);
                if (parentResult.Succeeded && parentResult.Data.FirstOrDefault() is { } parent)
                {
                    var createResult = await schoolsModuleRepoManager.LearnerParents.CreateAsync(new LearnerParent(learner.Id, parentId)
                    {
                        ParentConsentRequired = parent.RequireConsent
                    }, cancellationToken);

                    if (!createResult.Succeeded)
                        return await Result<LearnerDto>.FailAsync(createResult.Messages);
                }
            }

            foreach (var parentId in toRemove)
            {
                var toRemoveEntity = learner.Parents.FirstOrDefault(p => p.ParentId == parentId);
                if (toRemoveEntity != null)
                    schoolsModuleRepoManager.LearnerParents.Delete(toRemoveEntity);
            }

            var parentsSaveResult = await schoolsModuleRepoManager.LearnerParents.SaveAsync(cancellationToken);
            if (!parentsSaveResult.Succeeded)
                return await Result<LearnerDto>.FailAsync(parentsSaveResult.Messages);

            var saveResult = await schoolsModuleRepoManager.Learners.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded)
                return await Result<LearnerDto>.FailAsync(saveResult.Messages);

            return await Result<LearnerDto>.SuccessAsync("Learner updated successfully");
        }

        /// <summary>
        /// Removes a learner and cleans up associated notifications and messages.
        /// </summary>
        /// <param name="learnerId">The ID of the learner to be deleted.</param>
        /// <param name="cancellationToken">Optional cancellation token for the async operation.</param>
        /// <returns>A result indicating success or failure of the removal operation.</returns>
        public async Task<IBaseResult> RemoveAsync(string learnerId, CancellationToken cancellationToken = default)
        {
            var learnerDeleteResult = await schoolsModuleRepoManager.Learners.DeleteAsync(learnerId, cancellationToken);
            if (!learnerDeleteResult.Succeeded)
                return await Result<LearnerDto>.FailAsync(learnerDeleteResult.Messages);

            // Cleanup notifications
            var notificationsResult = notificationRepository.FindByCondition(c => c.EntityId == learnerId, true);
            if (notificationsResult.Succeeded)
            {
                foreach (var notification in notificationsResult.Data)
                {
                    var deleteResult = await notificationRepository.DeleteAsync(notification.Id, cancellationToken);
                    if (!deleteResult.Succeeded)
                        return await Result<LearnerDto>.FailAsync(deleteResult.Messages);
                }
            }

            // Cleanup messages
            var messagesResult = messageRepository.FindByCondition(c => c.EntityId == learnerId, true);
            if (messagesResult.Succeeded)
            {
                foreach (var message in messagesResult.Data)
                {
                    var deleteResult = await messageRepository.DeleteAsync(message.Id, cancellationToken);
                    if (!deleteResult.Succeeded)
                        return await Result<LearnerDto>.FailAsync(deleteResult.Messages);
                }
            }

            var learnerSave = await schoolsModuleRepoManager.Learners.SaveAsync(cancellationToken);
            if (!learnerSave.Succeeded)
                return await Result<LearnerDto>.FailAsync(learnerSave.Messages);

            return await Result<LearnerDto>.SuccessAsync("Learner removed successfully");
        }

        /// <summary>
        /// Updates the list of parents associated with a learner. Adds new associations and removes outdated ones.
        /// </summary>
        public async Task<IBaseResult> UpdateLearnerParentsAsync(string learnerId, List<ParentDto> learnerParents, CancellationToken cancellationToken = default)
        {
            var existingResult = await schoolsModuleRepoManager.LearnerParents.ListAsync(new LambdaSpec<LearnerParent>(lp => lp.LearnerId == learnerId), true, cancellationToken);
            if (!existingResult.Succeeded)
                return await Result.FailAsync(existingResult.Messages);

            var existing = existingResult.Data;
            var existingParentIds = existing.Select(lp => lp.ParentId).ToHashSet();
            var incomingParentIds = learnerParents.Select(p => p.ParentId).ToHashSet();

            foreach (var parentId in incomingParentIds.Except(existingParentIds))
            {
                var createResult = await schoolsModuleRepoManager.LearnerParents.CreateAsync(new LearnerParent(learnerId, parentId), cancellationToken);
                if (!createResult.Succeeded)
                    return await Result.FailAsync(createResult.Messages);
            }

            foreach (var lp in existing.Where(lp => !incomingParentIds.Contains(lp.ParentId)))
            {
                schoolsModuleRepoManager.LearnerParents.Delete(lp);
            }

            var saveResult = await schoolsModuleRepoManager.LearnerParents.SaveAsync(cancellationToken);
            return saveResult.Succeeded
                ? await Result.SuccessAsync("Learner parents updated successfully")
                : await Result.FailAsync(saveResult.Messages);
        }
    }

}
