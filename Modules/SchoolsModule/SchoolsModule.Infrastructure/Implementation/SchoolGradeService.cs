using ConectOne.Domain.Extensions;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using IdentityModule.Domain.DataTransferObjects;
using MessagingModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.RequestFeatures;
using SchoolsModule.Domain.Specifications;

namespace SchoolsModule.Infrastructure.Implementation;

/// <summary>
/// Service for managing CRUD operations and notifications related to <see cref="SchoolGrade"/> entities.
/// </summary>
public class SchoolGradeService(ISchoolsModuleRepoManager schoolsModuleRepoManager, IRepository<Notification, string> notificationRepository, IRepository<Message, string> messageRepository) : ISchoolGradeService
{
    /// <summary>
    /// Retrieves all school grades from the data store.
    /// </summary>
    public async Task<IBaseResult<IEnumerable<SchoolGradeDto>>> AllSchoolGradesAsync(CancellationToken cancellationToken = default)
    {
        var schoolGradeResult = await schoolsModuleRepoManager.SchoolGrades.ListAsync(false, cancellationToken);
        return schoolGradeResult.Succeeded
            ? await Result<IEnumerable<SchoolGradeDto>>.SuccessAsync(schoolGradeResult.Data.Select(c => new SchoolGradeDto(c)))
            : await Result<IEnumerable<SchoolGradeDto>>.FailAsync(schoolGradeResult.Messages);
    }

    /// <summary>
    /// Retrieves a paginated list of school grades.
    /// </summary>
    public async Task<PaginatedResult<SchoolGradeDto>> PagedSchoolGradesAsync(SchoolGradePageParameters pageParameters, CancellationToken cancellationToken = default)
    {
        var result = await schoolsModuleRepoManager.SchoolGrades.ListAsync(new PagedSchoolGradesSpecification(pageParameters), false, cancellationToken);
        if (!result.Succeeded)
            return PaginatedResult<SchoolGradeDto>.Failure(result.Messages);

        var response = result.Data.Select(c => new SchoolGradeDto(c)).ToList();
        return PaginatedResult<SchoolGradeDto>.Success(response, response.Count, pageParameters.PageNr, pageParameters.PageSize);
    }

    /// <summary>
    /// Retrieves a single school grade by ID.
    /// </summary>
    public async Task<IBaseResult<SchoolGradeDto>> SchoolGradeAsync(string schoolGradeId, bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        var result = await schoolsModuleRepoManager.SchoolGrades.FirstOrDefaultAsync(
            new SingleSchoolGradeSpecification(schoolGradeId), trackChanges, cancellationToken);

        return !result.Succeeded || result.Data == null
            ? await Result<SchoolGradeDto>.FailAsync(result.Messages ?? ["School Grade not found"])
            : await Result<SchoolGradeDto>.SuccessAsync(new SchoolGradeDto(result.Data));
    }

    /// <summary>
    /// Creates a new school grade.
    /// </summary>
    public async Task<IBaseResult> CreateAsync(SchoolGradeDto dto, CancellationToken cancellationToken = default)
    {
        var result = await schoolsModuleRepoManager.SchoolGrades.CreateAsync(dto.CreateSchoolGrade(), cancellationToken);
        if (!result.Succeeded) return await Result.FailAsync(result.Messages);

        var save = await schoolsModuleRepoManager.SchoolGrades.SaveAsync(cancellationToken);
        return save.Succeeded
            ? await Result.SuccessAsync("School Grade successfully created")
            : await Result.FailAsync(save.Messages);
    }

    /// <summary>
    /// Updates an existing school grade.
    /// </summary>
    public async Task<IBaseResult> UpdateAsync(SchoolGradeDto dto, CancellationToken cancellationToken = default)
    {
        var result = await schoolsModuleRepoManager.SchoolGrades.FirstOrDefaultAsync(
            new SingleSchoolGradeSpecification(dto.SchoolGradeId), true, cancellationToken);
        if (!result.Succeeded || result.Data == null)
            return await Result.FailAsync(result.Messages ?? ["School Grade not found"]);

        result.Data.Name = dto.SchoolGrade;

        var update = schoolsModuleRepoManager.SchoolGrades.Update(result.Data);
        if (!update.Succeeded) return await Result.FailAsync(update.Messages);

        var save = await schoolsModuleRepoManager.SchoolGrades.SaveAsync(cancellationToken);
        return save.Succeeded
            ? await Result.SuccessAsync("School Grade successfully updated")
            : await Result.FailAsync(save.Messages);
    }

    /// <summary>
    /// Deletes a school grade and any associated notifications or messages.
    /// </summary>
    public async Task<IBaseResult> DeleteAsync(string schoolGradeId, CancellationToken cancellationToken = default)
    {
        var delete = await schoolsModuleRepoManager.SchoolGrades.DeleteAsync(schoolGradeId);
        if (!delete.Succeeded) return await Result.FailAsync(delete.Messages);

        // Cascade delete: Notifications
        var notifications = await notificationRepository.ListAsync(new LambdaSpec<Notification>(c => c.EntityId == schoolGradeId), true, cancellationToken);
        if (notifications.Succeeded)
        {
            foreach (var note in notifications.Data)
            {
                var result = await notificationRepository.DeleteAsync(note.Id, cancellationToken);
                if (!result.Succeeded) return await Result.FailAsync(result.Messages);
            }
        }

        // Cascade delete: Messages
        var messages = await messageRepository.ListAsync(new LambdaSpec<Message>(c => c.EntityId == schoolGradeId), true, cancellationToken);
        if (messages.Succeeded)
        {
            foreach (var message in messages.Data)
            {
                var result = await messageRepository.DeleteAsync(message.Id, cancellationToken);
                if (!result.Succeeded) return await Result.FailAsync(result.Messages);
            }
        }

        var save = await schoolsModuleRepoManager.SchoolGrades.SaveAsync(cancellationToken);
        return save.Succeeded
            ? await Result.SuccessAsync("School Grade successfully deleted")
            : await Result.FailAsync(save.Messages);
    }

    #region Notifications

    /// <summary>
    /// Builds a list of recipients for a school grade-related notification, including learners, their parents, and teachers.
    /// </summary>
    public async Task<IBaseResult<IEnumerable<RecipientDto>>> SchoolGradeNotificationList(LearnerPageParameters parameters, CancellationToken cancellationToken = default)
    {
        var recipients = new List<RecipientDto>();

        // Learners in the grade
        var learners = await schoolsModuleRepoManager.Learners.ListAsync(new SchoolGradeNotificationListSpecification(parameters), false, cancellationToken);
        if (!learners.Succeeded) return await Result<IEnumerable<RecipientDto>>.FailAsync(learners.Messages);

        foreach (var learner in learners.Data)
        {
            var age = learner.IdNumber.GetAge();
            if (age < parameters.MinAge || age > parameters.MaxAge) continue;

            if (recipients.All(r => r.Id != learner.Id))
                recipients.Add(new RecipientDto(learner.Id, learner.FirstName, learner.LastName, learner.EmailAddresses.Select(e => e.Email).ToList(), true, true));

            foreach (var parent in learner.Parents)
            {
                if (recipients.All(r => r.Id != parent.Parent!.Id))
                    recipients.Add(new RecipientDto(parent.Parent.Id, parent.Parent.FirstName, parent.Parent.LastName, parent.Parent.EmailAddresses.Select(e => e.Email).ToList(), parent.Parent.ReceiveNotifications, parent.Parent.RecieveEmails));
            }
        }

        // Teachers of the grade
        var teacherSpec = new LambdaSpec<Teacher>(t => t.GradeId == parameters.GradeId);
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
}