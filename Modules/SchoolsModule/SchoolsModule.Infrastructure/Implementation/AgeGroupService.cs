using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using MessagingModule.Domain.Entities;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Infrastructure.Implementation;

/// <summary>
/// Service responsible for handling CRUD operations and queries for <see cref="AgeGroup"/> entities.
/// </summary>
public class AgeGroupService(ISchoolsModuleRepoManager schoolsModuleRepoManager, IRepository<Notification, string> notificationRepository, IRepository<Message, string> messageRepository) : IAgeGroupService
{
    /// <summary>
    /// Retrieves all age groups from the data store.
    /// </summary>
    public async Task<IBaseResult<IEnumerable<AgeGroupDto>>> AllAgeGroupsAsync(CancellationToken cancellationToken = default)
    {
        var ageGroupResult = await schoolsModuleRepoManager.AgeGroups.ListAsync(false, cancellationToken);
        if (!ageGroupResult.Succeeded)
            return await Result<IEnumerable<AgeGroupDto>>.FailAsync(ageGroupResult.Messages);

        return await Result<IEnumerable<AgeGroupDto>>.SuccessAsync(ageGroupResult.Data.Select(c => new AgeGroupDto(c)));
    }

    /// <summary>
    /// Retrieves a paginated list of age groups.
    /// </summary>
    public async Task<PaginatedResult<AgeGroupDto>> PagedAgeGroupsAsync(AgeGroupPageParameters pageParameters, CancellationToken cancellationToken = default)
    {
        var ageGroupResult = await schoolsModuleRepoManager.AgeGroups.ListAsync(false, cancellationToken);
        if (!ageGroupResult.Succeeded)
            return PaginatedResult<AgeGroupDto>.Failure(ageGroupResult.Messages);

        var response = ageGroupResult.Data.Select(c => new AgeGroupDto(c)).ToList();
        return PaginatedResult<AgeGroupDto>.Success(response, response.Count, pageParameters.PageNr, pageParameters.PageSize);
    }

    /// <summary>
    /// Retrieves a single age group by ID.
    /// </summary>
    public async Task<IBaseResult<AgeGroupDto>> AgeGroupAsync(string ageGroupId, bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        var ageGroupResult = await schoolsModuleRepoManager.AgeGroups.FirstOrDefaultAsync(
            new LambdaSpec<AgeGroup>(c => c.Id == ageGroupId), trackChanges, cancellationToken);

        if (!ageGroupResult.Succeeded)
            return await Result<AgeGroupDto>.FailAsync(ageGroupResult.Messages);

        var response = new AgeGroupDto(ageGroupResult.Data!);
        return await Result<AgeGroupDto>.SuccessAsync(response);
    }

    /// <summary>
    /// Creates a new age group.
    /// </summary>
    public async Task<IBaseResult> CreateAsync(AgeGroupDto ageGroup, CancellationToken cancellationToken = default)
    {
        var createResult = await schoolsModuleRepoManager.AgeGroups.CreateAsync(ageGroup.CreateAgeGroup(), cancellationToken);
        if (!createResult.Succeeded)
            return await Result.FailAsync(createResult.Messages);

        var saveResult = await schoolsModuleRepoManager.AgeGroups.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded)
            return await Result.FailAsync(saveResult.Messages);

        return await Result.SuccessAsync("Age group successfully created");
    }

    /// <summary>
    /// Updates an existing age group.
    /// </summary>
    public async Task<IBaseResult> UpdateAsync(AgeGroupDto ageGroup, CancellationToken cancellationToken = default)
    {
        var ageGroupResult = await schoolsModuleRepoManager.AgeGroups.FirstOrDefaultAsync(
            new LambdaSpec<AgeGroup>(c => c.Id == ageGroup.AgeGroupId), true, cancellationToken);

        if (!ageGroupResult.Succeeded)
            return await Result.FailAsync(ageGroupResult.Messages);

        var result = ageGroupResult.Data!;
        result.Name = ageGroup.Name;
        result.MinAge = ageGroup.MinAge;
        result.MaxAge = ageGroup.MaxAge;

        var updateResult = schoolsModuleRepoManager.AgeGroups.Update(result);
        if (!updateResult.Succeeded)
            return await Result.FailAsync(updateResult.Messages);

        var saveResult = await schoolsModuleRepoManager.AgeGroups.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded)
            return await Result.FailAsync(saveResult.Messages);

        return await Result.SuccessAsync("Age group successfully updated");
    }

    /// <summary>
    /// Deletes an age group and associated notifications and messages.
    /// </summary>
    public async Task<IBaseResult> DeleteAsync(string ageGroupId, CancellationToken cancellationToken = default)
    {
        var deleteResult = await schoolsModuleRepoManager.AgeGroups.DeleteAsync(ageGroupId, cancellationToken);
        if (!deleteResult.Succeeded)
            return await Result.FailAsync(deleteResult.Messages);

        // Delete related notifications
        var notificationsResult = await notificationRepository.ListAsync(
            new LambdaSpec<Notification>(c => c.EntityId == ageGroupId), true, cancellationToken);

        if (notificationsResult.Succeeded)
        {
            foreach (var notification in notificationsResult.Data)
            {
                var notificationDeletionResult = await notificationRepository.DeleteAsync(notification.Id, cancellationToken);
                if (!notificationDeletionResult.Succeeded)
                    return await Result.FailAsync(notificationDeletionResult.Messages);
            }
        }

        // Delete related messages
        var messageResult = await messageRepository.ListAsync(
            new LambdaSpec<Message>(c => c.EntityId == ageGroupId), true, cancellationToken);

        if (messageResult.Succeeded)
        {
            foreach (var message in messageResult.Data)
            {
                var messageDeletionResult = await messageRepository.DeleteAsync(message.Id, cancellationToken);
                if (!messageDeletionResult.Succeeded)
                    return await Result.FailAsync(messageDeletionResult.Messages);
            }
        }

        var saveResult = await schoolsModuleRepoManager.AgeGroups.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded)
            return await Result.FailAsync(saveResult.Messages);

        return await Result.SuccessAsync("Age group successfully deleted");
    }
}