using ConectOne.Domain.Entities;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using ConectOne.Infrastructure.Implementation;
using IdentityModule.Domain.DataTransferObjects;
using MessagingModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.RequestFeatures;
using SchoolsModule.Domain.Specifications;

namespace SchoolsModule.Infrastructure.Implementation
{
    /// <summary>
    /// Service class for managing Teacher-related operations.
    /// </summary>
    public class TeacherService(ISchoolsModuleRepoManager schoolsModuleRepoManager, IRepository<Notification, string> notificationRepository, 
        IRepository<Message, string> messageRepository, IRepository<ContactNumber<Teacher>, string> contactNumberRepo,
        IRepository<EmailAddress<Teacher>, string> emailAddressRepo) : ContactIntoService<Teacher>(contactNumberRepo, emailAddressRepo), ITeacherService
    {
        /// <summary>
        /// Retrieves all teachers asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of Teacher entities.</returns>
        public async Task<IBaseResult<IEnumerable<TeacherDto>>> AllTeachersAsync(CancellationToken cancellationToken = default)
        {
            var teacherResult = await schoolsModuleRepoManager.Teachers.ListAsync(false, cancellationToken);
            if (!teacherResult.Succeeded) return await Result<IEnumerable<TeacherDto>>.FailAsync(teacherResult.Messages);
            return await Result<IEnumerable<TeacherDto>>.SuccessAsync(teacherResult.Data.Select(c => new TeacherDto(c)));
        }

        /// <summary>
        /// Retrieves a paginated list of teachers asynchronously.
        /// </summary>
        /// <param name="pageParameters">The pagination parameters.</param>
        /// <param name="trackChanges">Indicates whether to track changes.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a paginated result of TeacherDto entities.</returns>
        public async Task<PaginatedResult<TeacherDto>> PagedTeachersAsync(TeacherPageParameters pageParameters, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var pagedSpec = new PagedTeachersSpecification(pageParameters);

            var result = await schoolsModuleRepoManager.Teachers.ListAsync(pagedSpec, trackChanges, cancellationToken);
            if (!result.Succeeded)
                return PaginatedResult<TeacherDto>.Failure(result.Messages);

            var countResult = await schoolsModuleRepoManager.Teachers.CountAsync(null, cancellationToken);
            if (!countResult.Succeeded)
                return PaginatedResult<TeacherDto>.Failure(countResult.Messages);

            var dtoList = result.Data.Select(t => new TeacherDto(t)).ToList();
            return PaginatedResult<TeacherDto>.Success(dtoList, countResult.Data, pageParameters.PageNr, pageParameters.PageSize);
        }

        /// <summary>
        /// Retrieves a teacher by ID asynchronously.
        /// </summary>
        /// <param name="teacherId">The ID of the teacher.</param>
        /// <param name="trackChanges">Indicates whether to track changes.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a TeacherDto entity.</returns>
        public async Task<IBaseResult<TeacherDto>> TeacherAsync(string teacherId, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Teacher>(c => c.Id == teacherId);
            spec.AddInclude(g => g.Include(c => c.Images));
            spec.AddInclude(g => g.Include(c => c.EmailAddresses));
            spec.AddInclude(g => g.Include(c => c.ContactNumbers));
            spec.AddInclude(g => g.Include(c => c.SchoolClass));
            spec.AddInclude(g => g.Include(c => c.Grade));
            spec.AddInclude(g => g.Include(c => c.Address));

            var parentResult = await schoolsModuleRepoManager.Teachers.FirstOrDefaultAsync(spec, trackChanges, cancellationToken);
            if (!parentResult.Succeeded) return await Result<TeacherDto>.FailAsync(parentResult.Messages);
            return await Result<TeacherDto>.SuccessAsync(new TeacherDto(parentResult.Data));
        }

        /// <summary>
        /// Retrieves a teacher by email address asynchronously.
        /// </summary>
        /// <param name="emailAddress">The email address of the teacher.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a TeacherDto entity.</returns>
        public async Task<IBaseResult<TeacherDto>> TeacherByEmailAsync(string emailAddress, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Teacher>(c => c.EmailAddresses.Any(c => c.Email == emailAddress));
            spec.AddInclude(g => g.Include(c => c.Images));
            spec.AddInclude(g => g.Include(c => c.EmailAddresses));
            spec.AddInclude(g => g.Include(c => c.ContactNumbers));
            spec.AddInclude(g => g.Include(c => c.SchoolClass));
            spec.AddInclude(g => g.Include(c => c.Grade));
            spec.AddInclude(g => g.Include(c => c.Address));

            var teacherResult = await schoolsModuleRepoManager.Teachers.FirstOrDefaultAsync(spec,false, cancellationToken);
            if (!teacherResult.Succeeded) return await Result<TeacherDto>.FailAsync(teacherResult.Messages);

            if (!teacherResult.Succeeded)
                return await Result<TeacherDto>.FailAsync(teacherResult.Messages);

            if (teacherResult.Data == null)
                return await Result<TeacherDto>.FailAsync("No teacher found");

            return await Result<TeacherDto>.SuccessAsync(new TeacherDto(teacherResult.Data));
        }

        /// <summary>
        /// Checks if a teacher exists by email address asynchronously.
        /// </summary>
        /// <param name="emailAddress">The email address of the teacher.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the teacher's ID if found.</returns>
        public async Task<IBaseResult<string>> TeacherExist(string emailAddress, CancellationToken cancellationToken = default)
        {
            var teacherResult = await schoolsModuleRepoManager.Teachers.FirstOrDefaultAsync(new SingleTeacherSpecification(emailAddress: emailAddress), false, cancellationToken);
            if (!teacherResult.Succeeded) return await Result<string>.FailAsync(teacherResult.Messages);
            return await Result<string>.SuccessAsync(data: teacherResult.Data?.Id);
        }

        /// <summary>
        /// Creates a new teacher asynchronously.
        /// </summary>
        /// <param name="teacher">The teacher data transfer object.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a TeacherDto entity.</returns>
        public async Task<IBaseResult<TeacherDto>> CreateAsync(TeacherDto teacher, CancellationToken cancellationToken = default)
        {
            var parentResult = await schoolsModuleRepoManager.Teachers.CreateAsync(teacher.ToTeacher(),cancellationToken);
            if (!parentResult.Succeeded) return await Result<TeacherDto>.FailAsync(parentResult.Messages);

            var saveResult = await schoolsModuleRepoManager.Teachers.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result<TeacherDto>.FailAsync(saveResult.Messages);

            return await Result<TeacherDto>.SuccessAsync(new TeacherDto(parentResult.Data));
        }

        /// <summary>
        /// Updates an existing teacher asynchronously.
        /// </summary>
        /// <param name="teacherDto">The teacher data transfer object.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task<IBaseResult> UpdateAsync(TeacherDto teacherDto, CancellationToken cancellationToken = default)
        {
            var teacherResult = await schoolsModuleRepoManager.Teachers.FirstOrDefaultAsync(new LambdaSpec<Teacher>(c => c.Id == teacherDto.TeacherId), true, cancellationToken);
            
            if (!teacherResult.Succeeded) return await Result.FailAsync(teacherResult.Messages);

            if (teacherResult.Data == null) return await Result.FailAsync($"No teacher with id matching '{teacherDto.TeacherId}' found in the database");

            var teacher = teacherResult.Data;
            teacher.Title = teacherDto.Title;
            teacher.Name = teacherDto.FirstName;
            teacher.Surname = teacherDto.LastName;
            teacher.GradeId = teacherDto.Grade?.SchoolGradeId;
            teacher.SchoolClassId = teacherDto.SchoolClass?.SchoolClassId;

            teacher.ReceiveNotifications = teacherDto.ReceiveNotifications;
            teacher.ReceiveMessages = teacherDto.ReceiveMessages;
            teacher.RecieveEmails = teacherDto.RecieveEmails;

            var teacherUpdateResult = schoolsModuleRepoManager.Teachers.Update(teacher);
            if (!teacherUpdateResult.Succeeded) return await Result.FailAsync(teacherUpdateResult.Messages);

            var saveResult = await schoolsModuleRepoManager.Teachers.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);
            return await Result.SuccessAsync("Teacher was successfully updated");
        }

        /// <summary>
        /// Removes a teacher by ID asynchronously.
        /// </summary>
        /// <param name="teacherId">The ID of the teacher.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task<IBaseResult> RemoveAsync(string teacherId, CancellationToken cancellationToken = default)
        {
            var deleteResult = await schoolsModuleRepoManager.Teachers.DeleteAsync(teacherId, cancellationToken);
            if (!deleteResult.Succeeded) return await Result<TeacherDto>.FailAsync(deleteResult.Messages);

            var notificationsResult = await notificationRepository.ListAsync(new LambdaSpec<Notification>(c => c.EntityId == teacherId), true, cancellationToken);
            if (notificationsResult.Succeeded)
            {
                foreach (var notification in notificationsResult.Data)
                {
                    var notificationDeletionResult = await notificationRepository.DeleteAsync(notification.Id, cancellationToken);
                    if (!notificationDeletionResult.Succeeded) return await Result.FailAsync(notificationDeletionResult.Messages);
                }
            }

            var messageResult = await messageRepository.ListAsync(new LambdaSpec<Message>(c => c.EntityId == teacherId), true, cancellationToken);
            if (notificationsResult.Succeeded)
            {
                foreach (var message in messageResult.Data)
                {
                    var messageDeletionResult = await messageRepository.DeleteAsync(message.Id, cancellationToken);
                    if (!messageDeletionResult.Succeeded) return await Result.FailAsync(messageDeletionResult.Messages);
                }
            }

            var saveResult = await schoolsModuleRepoManager.Teachers.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);
            return await Result<TeacherDto>.SuccessAsync("Teacher was removed successfully");
        }

        /// <summary>
        /// Retrieves a list of teachers for notifications asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of UserInfoDto entities.</returns>
        public async Task<IBaseResult<IEnumerable<RecipientDto>>> TeachersNotificationList(TeacherPageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var spec = pageParameters.TeacherId != null ? new LambdaSpec<Teacher>(p => p.Id == pageParameters.TeacherId) : new LambdaSpec<Teacher>(p => true);
            spec.AddInclude(q => q.Include(p => p.EmailAddresses));

            var result = await schoolsModuleRepoManager.Teachers.ListAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<IEnumerable<RecipientDto>>.FailAsync(result.Messages);

            var recipients = new List<RecipientDto>();

            foreach (var teacher in result.Data)
            {
                if (recipients.All(c => c.Id != teacher.Id))
                    recipients.Add(new RecipientDto(teacher.Id, teacher.Name, teacher.Surname, teacher.EmailAddresses.Select(c => c.Email).ToList(), true, false));
            }
            return await Result<IEnumerable<RecipientDto>>.SuccessAsync(recipients);
        }
    }
}
