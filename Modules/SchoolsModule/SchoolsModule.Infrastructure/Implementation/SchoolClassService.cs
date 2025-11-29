using ConectOne.Domain.Entities;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using FilingModule.Domain.Interfaces;
using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Entities;
using IdentityModule.Domain.Interfaces;
using IdentityModule.Domain.RequestFeatures;
using MessagingModule.Domain.Entities;
using MessagingModule.Domain.Interfaces;
using MessagingModule.Domain.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.RequestFeatures;
using SchoolsModule.Domain.Specifications;

namespace SchoolsModule.Infrastructure.Implementation
{
    /// <summary>
    /// Service for managing <see cref="SchoolClass"/> entities, including CRUD operations and notification-related queries.
    /// </summary>
    public class SchoolClassService(ISchoolsModuleRepoManager schoolsModuleRepoManager, IRepository<Notification, string> notificationRepository, IRepository<Message, string> messageRepository,
        IRepository<ChatGroup, string> chatGroupRepo, IRepository<ChatGroupMember, string> chatGroupMemberRepo, IChatGroupService chatGroupService, IExcelService excelService, IUserService userService) 
        : ISchoolClassService
    {
        /// <summary>
        /// Retrieves all school classes.
        /// </summary>
        public async Task<IBaseResult<IEnumerable<SchoolClassDto>>> AllSchoolClassesAsync(CancellationToken cancellationToken = default)
        {
            var result = await schoolsModuleRepoManager.SchoolClasses.ListAsync(false, cancellationToken);
            return result.Succeeded
                ? await Result<IEnumerable<SchoolClassDto>>.SuccessAsync(result.Data.Select(c => new SchoolClassDto(c)))
                : await Result<IEnumerable<SchoolClassDto>>.FailAsync(result.Messages);
        }

        /// <summary>
        /// Retrieves a paginated list of school classes using specified filtering parameters.
        /// </summary>
        public async Task<PaginatedResult<SchoolClassDto>> PagedSchoolClassesAsync(SchoolClassPageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var result = await schoolsModuleRepoManager.SchoolClasses.ListAsync(new PagedSchoolClassSpecification(pageParameters), false, cancellationToken);
            if (!result.Succeeded)
                return PaginatedResult<SchoolClassDto>.Failure(result.Messages);

            var response = result.Data.Select(c => new SchoolClassDto(c)).ToList();
            return PaginatedResult<SchoolClassDto>.Success(response, response.Count, pageParameters.PageNr, pageParameters.PageSize);
        }

        /// <summary>
        /// Retrieves a school class by its unique identifier, including its grade information.
        /// </summary>
        public async Task<IBaseResult<SchoolClassDto>> SchoolClassAsync(string schoolClassId, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<SchoolClass>(c => c.Id == schoolClassId);
            spec.AddInclude(c => c.Include(c => c.Grade));

            var result = await schoolsModuleRepoManager.SchoolClasses.FirstOrDefaultAsync(spec, trackChanges, cancellationToken);
            if (!result.Succeeded || result.Data == null)
                return await Result<SchoolClassDto>.FailAsync(result.Messages ?? ["School Class not found"]);

            return await Result<SchoolClassDto>.SuccessAsync(new SchoolClassDto(result.Data));
        }

        /// <summary>
        /// Creates a new school class.
        /// </summary>
        public async Task<IBaseResult> CreateAsync(SchoolClassDto schoolClass, CancellationToken cancellationToken = default)
        {
            var result = await schoolsModuleRepoManager.SchoolClasses.CreateAsync(schoolClass.CreateSchoolClass(), cancellationToken);
            if (!result.Succeeded) return await Result.FailAsync(result.Messages);

            if (schoolClass.AutoCreateChatGroup)
            {
                var newChatGroup = new ChatGroup
                {
                    Id = schoolClass.SchoolClassId,
                    Name = schoolClass.SchoolClass
                };

                await chatGroupRepo.CreateAsync(newChatGroup, cancellationToken);
            }
            
            var save = await schoolsModuleRepoManager.SchoolClasses.SaveAsync(cancellationToken);
            return save.Succeeded
                ? await Result.SuccessAsync("School Class successfully created")
                : await Result.FailAsync(save.Messages);
        }

        /// <summary>
        /// Updates an existing school class.
        /// </summary>
        public async Task<IBaseResult> UpdateAsync(SchoolClassDto schoolClass, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<SchoolClass>(c => c.Id == schoolClass.SchoolClassId);
            spec.AddInclude(c => c.Include(g => g.PersonnelCollection));
            spec.AddInclude(c => c.Include(g => g.Learners).ThenInclude(l => l.Parents)
                .ThenInclude(p => p.Parent).ThenInclude(e => e.EmailAddresses));

            var classResult = await schoolsModuleRepoManager.SchoolClasses.FirstOrDefaultAsync(spec, true, cancellationToken);
            if (!classResult.Succeeded || classResult.Data == null)
                return await Result.FailAsync(classResult.Messages ?? ["School Class not found"]);

            var schoolClassEntity = classResult.Data;
            schoolClassEntity.Name = schoolClass.SchoolClass!;
            schoolClassEntity.GradeId = schoolClass.GradeId;
            schoolClassEntity.AutoCreateChatGroup = schoolClass.AutoCreateChatGroup;

            var updateResult = schoolsModuleRepoManager.SchoolClasses.Update(schoolClassEntity);
            if (!updateResult.Succeeded)
                return await Result.FailAsync(updateResult.Messages);

            if (schoolClass.AutoCreateChatGroup)
                await EnsureChatGroupAsync(schoolClassEntity, cancellationToken);

            var saveResult = await schoolsModuleRepoManager.SchoolClasses.SaveAsync(cancellationToken);
            return saveResult.Succeeded
                ? await Result.SuccessAsync("School Class successfully updated")
                : await Result.FailAsync(saveResult.Messages);
        }

        /// <summary>
        /// Deletes a school class and related notifications and messages.
        /// </summary>
        public async Task<IBaseResult> DeleteAsync(string schoolClassId, CancellationToken cancellationToken = default)
        {
            var delete = await schoolsModuleRepoManager.SchoolClasses.DeleteAsync(schoolClassId);
            if (!delete.Succeeded) return await Result.FailAsync(delete.Messages);

            var notifications = await notificationRepository.ListAsync(
                new LambdaSpec<Notification>(c => c.EntityId == schoolClassId), true, cancellationToken);

            if (notifications.Succeeded)
            {
                foreach (var note in notifications.Data)
                {
                    var deleteNote = await notificationRepository.DeleteAsync(note.Id, cancellationToken);
                    if (!deleteNote.Succeeded) return await Result.FailAsync(deleteNote.Messages);
                }
            }

            var messages = messageRepository.FindByCondition(c => c.EntityId == schoolClassId, true);
            if (messages.Succeeded)
            {
                foreach (var msg in messages.Data)
                {
                    var deleteMsg = await messageRepository.DeleteAsync(msg.Id, cancellationToken);
                    if (!deleteMsg.Succeeded) return await Result.FailAsync(deleteMsg.Messages);
                }
            }

            var save = await schoolsModuleRepoManager.SchoolClasses.SaveAsync(cancellationToken);
            return save.Succeeded
                ? await Result.SuccessAsync("School Class successfully deleted")
                : await Result.FailAsync(save.Messages);
        }

        #region Notifications

        /// <summary>
        /// Retrieves a list of learners, their parents, and teachers associated with a given school class
        /// for notification purposes.
        /// </summary>
        public async Task<IBaseResult<IEnumerable<RecipientDto>>> SchoolClassNotificationList(LearnerPageParameters parameters, CancellationToken cancellationToken = default)
        {
            var recipients = new List<RecipientDto>();

            // Get learners and parents
            var learners = await schoolsModuleRepoManager.Learners.ListAsync(new SchoolClassNotificationListSpecification(parameters), false, cancellationToken);
            if (!learners.Succeeded) return await Result<IEnumerable<RecipientDto>>.FailAsync(learners.Messages);

            foreach (var learner in learners.Data)
            {
                if (recipients.All(r => r.Id != learner.Id))
                    recipients.Add(new RecipientDto(learner.Id, learner.FirstName, learner.LastName, learner.EmailAddresses.Select(e => e.Email).ToList(), true, true));

                foreach (var parent in learner.Parents)
                {
                    if (recipients.All(r => r.Id != parent.Parent.Id))
                        recipients.Add(new RecipientDto(parent.Parent.Id, parent.Parent.FirstName, parent.Parent.LastName, parent.Parent.EmailAddresses.Select(e => e.Email).ToList(), parent.Parent.ReceiveNotifications, parent.Parent.RecieveEmails));
                }
            }

            // Get teachers
            var teacherSpec = new LambdaSpec<Teacher>(t => t.SchoolClassId == parameters.SchoolClassId);
            teacherSpec.AddInclude(t => t.Include(t => t.EmailAddresses));
            var teachers = await schoolsModuleRepoManager.Teachers.ListAsync(teacherSpec, false, cancellationToken);

            foreach (var teacher in teachers.Data)
            {
                if (recipients.All(r => r.Id != teacher.Id))
                    recipients.Add(new RecipientDto(teacher.Id, teacher.Name, teacher.Surname, teacher.EmailAddresses.Select(e => e.Email).ToList(), true, false));
            }

            return await Result<IEnumerable<RecipientDto>>.SuccessAsync(recipients);
        }

        #endregion

        #region Chats

        /// <summary>
        /// Creates a chat group for a specified school class, including its learners, their parents, and associated
        /// personnel.
        /// </summary>
        /// <remarks>This method retrieves the specified school class, gathers the email addresses of its
        /// learners, their parents, and associated personnel,  and creates a chat group including all relevant users.
        /// The method ensures that duplicate user entries are avoided.</remarks>
        /// <param name="schoolClassId">The unique identifier of the school class for which the chat group is being created.</param>
        /// <param name="groupMemberId">The unique identifier of the initial group member to include in the chat group.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A result containing the unique identifier of the created chat group if the operation succeeds; otherwise, an
        /// error message.</returns>
        public async Task<IBaseResult<string>> CreateSchoolClassChatGroupAsync(string schoolClassId, string groupMemberId, CancellationToken cancellationToken = default)
        {
            var classSpec = new LambdaSpec<SchoolClass>(c => c.Id == schoolClassId);
            classSpec.AddInclude(q => q.Include(p => p.Learners).ThenInclude(c => c.Parents).ThenInclude(c => c.Parent).ThenInclude(c => c.EmailAddresses));
            classSpec.AddInclude(q => q.Include(p => p.PersonnelCollection).ThenInclude(c => c.EmailAddresses));

            var classResult = await schoolsModuleRepoManager.SchoolClasses.FirstOrDefaultAsync(classSpec, true, cancellationToken);
            if (!classResult.Succeeded) return await Result<string>.FailAsync(classResult.Messages);

            var userList = await userService.AllUsers(new UserPageParameters());
            var usersIds = new List<string>() { groupMemberId };
            foreach (var learner in classResult.Data.Learners)
            {
                foreach (var learnerParent in learner.Parents)
                {
                    var user = userList.Data.FirstOrDefault(u => learnerParent.Parent.EmailAddresses.Any(e => e.Email == u?.EmailAddress));
                    if (user != null && !usersIds.Contains(user.UserId))
                    {
                        usersIds.Add(user.UserId);
                    }
                }
            }

            foreach (var teacher in classResult.Data.PersonnelCollection)
            {
                var user = userList.Data.FirstOrDefault(u => teacher.EmailAddresses.Any(e => e.Email == u?.EmailAddress));
                if (user != null && !usersIds.Contains(user.UserId))
                {
                    usersIds.Add(user.UserId);
                }
            }

            var groupResult = await chatGroupService.CreateGroupAsync(new AddUpdateChatGroupRequest() { Id = Guid.NewGuid().ToString(), Name = classResult.Data.Name, UserIds = usersIds }, cancellationToken);
            if (!groupResult.Succeeded) return await Result<string>.FailAsync(groupResult.Messages);

            var saveResult = await schoolsModuleRepoManager.Parents.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result<string>.FailAsync(saveResult.Messages);

            return await Result<string>.SuccessAsync(data: groupResult.Data.Id);
        }

        #endregion

        #region Import/Export
        
        /// <summary>
        /// Exports attendance records for a specific group that are yet to be completed into an Excel file.
        /// </summary>
        /// <remarks>The exported Excel file includes columns for the learner's first name, last name,
        /// status, and notes. The sheet name is dynamically generated based on the group's name and date.</remarks>
        /// <param name="request">The request containing the criteria for the attendance group to export.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with the file path of the exported Excel file if the operation succeeds, or error messages if it fails.</returns>
        public async Task<IBaseResult<string>> ExportAttendanceGroupToBeCompleted(ExportAttendanceRequest request, CancellationToken cancellationToken = default)
        {
            var spec = new ExportAttendanceGroupSpec(request);
            var result = await schoolsModuleRepoManager.AttendanceGroups.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!result.Succeeded)
                return await Result<string>.FailAsync(result.Messages);

            var export = await excelService.ExportAsync(result.Data.AttendanceRecords, new Dictionary<string, Func<AttendanceRecord, object>>
                {
                    { "FirstName", p => p.Learner.FirstName },
                    { "LastName", p => p.Learner.LastName },
                    { "Status", p => "" },
                    { "Notes", p => "" }
                }, sheetName: $"{result.Data.Name} -- {result.Data.Date}");

            return await Result<string>.SuccessAsync(data: export.Data);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Ensures a chat group exists and is populated for the given school class.
        /// </summary>
        private async Task EnsureChatGroupAsync(SchoolClass schoolClassEntity, CancellationToken cancellationToken)
        {
            var spec = new LambdaSpec<ChatGroup>(g => g.Id == schoolClassEntity.Id);
            spec.AddInclude(g => g.Include(g => g.Members));

            var chatGroupResult = await chatGroupRepo.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!chatGroupResult.Succeeded) return;

            var chatGroup = chatGroupResult.Data;
            if (chatGroup == null)
            {
                var result = await chatGroupRepo.CreateAsync(new ChatGroup
                {
                    Id = schoolClassEntity.Id,
                    Name = schoolClassEntity.Name
                }, cancellationToken);

                chatGroup = result.Data;
            }

            var users = await userService.AllUsers(new UserPageParameters());
            //await AddMembersToChatGroupAsync(users, chatGroup, schoolClassEntity.PersonnelCollection.SelectMany(p => p.EmailAddresses), cancellationToken);
            //await AddMembersToChatGroupAsync(users, chatGroup, schoolClassEntity.Learners.SelectMany(l => l.Parents.SelectMany(p => p.Parent.EmailAddresses)), cancellationToken);
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
