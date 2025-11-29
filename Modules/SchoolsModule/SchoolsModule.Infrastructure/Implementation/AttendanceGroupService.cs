using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.RequestFeatures;
using SchoolsModule.Domain.Specifications;

namespace SchoolsModule.Infrastructure.Implementation;

/// <summary>
/// Service providing CRUD operations for <see cref="AttendanceGroup"/> entities.
/// </summary>
public class AttendanceGroupService(ISchoolsModuleRepoManager repoManager) : IAttendanceGroupService
{
    /// <summary>
    /// Asynchronously retrieves all attendance groups that belong to the specified parent group.
    /// </summary>
    /// <param name="parentGroupId">The identifier of the parent group whose attendance groups are to be retrieved.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> of
    /// <see cref="IEnumerable{T}"/> of <see cref="AttendanceGroupDto"/> representing the attendance groups.</returns>
    public async Task<IBaseResult<IEnumerable<AttendanceGroupDto>>> AllAttendanceGroupsAsync(string parentGroupId,  CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<AttendanceGroup>(g => g.ParentGroupId == parentGroupId);

        var result = await repoManager.AttendanceGroups.ListAsync(false, cancellationToken);
        if (!result.Succeeded)
            return await Result<IEnumerable<AttendanceGroupDto>>.FailAsync(result.Messages);

        return await Result<IEnumerable<AttendanceGroupDto>>.SuccessAsync(result.Data.Select(g => new AttendanceGroupDto(g)));
    }

    /// <summary>
    /// Retrieves a paginated list of attendance groups based on the specified paging parameters.
    /// </summary>
    /// <remarks>This method queries the repository for attendance groups and returns them in a paginated
    /// format. The result includes the total count of items and the current page information.</remarks>
    /// <param name="pageParameters">The parameters that define the pagination settings, including page number and page size.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="PaginatedResult{AttendanceGroupDto}"/> containing the list of attendance groups and pagination
    /// details.</returns>
    public async Task<PaginatedResult<AttendanceGroupDto>> PagedAttendanceGroupsAsync(AttendanceGroupPageParameters pageParameters, CancellationToken cancellationToken = default)
    {
        var spec = new PagedAttendanceGroupSpecification(pageParameters);
        var result = await repoManager.AttendanceGroups.ListAsync(spec, false, cancellationToken);
        if (!result.Succeeded)
            return PaginatedResult<AttendanceGroupDto>.Failure(result.Messages);

        var response = result.Data.Select(g => new AttendanceGroupDto(g)).ToList();
        return PaginatedResult<AttendanceGroupDto>.Success(response, response.Count, pageParameters.PageNr, pageParameters.PageSize);
    }

    /// <summary>
    /// Asynchronously retrieves an attendance group by its identifier.
    /// </summary>
    /// <param name="attendanceGroupId">The unique identifier of the attendance group to retrieve.</param>
    /// <param name="trackChanges">A boolean value indicating whether to track changes to the retrieved entity.  true to track changes; otherwise,
    /// false.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is CancellationToken.None.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an  IBaseResult{AttendanceGroupDto}
    /// which includes the attendance group data if found,  or an error message if the group is not found.</returns>
    public async Task<IBaseResult<AttendanceGroupDto>> AttendanceGroupAsync(string attendanceGroupId, bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<AttendanceGroup>(g => g.Id == attendanceGroupId);
        var result = await repoManager.AttendanceGroups.FirstOrDefaultAsync(spec, trackChanges, cancellationToken);
        if (!result.Succeeded || result.Data == null)
            return await Result<AttendanceGroupDto>.FailAsync(result.Messages ?? ["Attendance Group not found"]);

        return await Result<AttendanceGroupDto>.SuccessAsync(new AttendanceGroupDto(result.Data));
    }

    /// <summary>
    /// Asynchronously creates a new attendance group.
    /// </summary>
    /// <param name="attendanceGroup">The data transfer object containing the details of the attendance group to create.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the operation, along with any relevant messages.</returns>
    public async Task<IBaseResult> CreateAsync(AttendanceGroupDto attendanceGroup, CancellationToken cancellationToken = default)
    {
        var createResult = await repoManager.AttendanceGroups.CreateAsync(attendanceGroup.CreateAttendanceGroup(), cancellationToken);
        if (!createResult.Succeeded)
            return await Result.FailAsync(createResult.Messages);

        var saveResult = await repoManager.AttendanceGroups.SaveAsync(cancellationToken);
        return saveResult.Succeeded
            ? await Result.SuccessAsync("Attendance group successfully created")
            : await Result.FailAsync(saveResult.Messages);
    }

    /// <summary>
    /// Asynchronously updates an existing attendance group with the specified details.
    /// </summary>
    /// <remarks>This method updates the attendance group identified by the <c>AttendanceGroupId</c> property
    /// of the <paramref name="attendanceGroup"/> parameter. If the specified attendance group is not found, the
    /// operation fails with an appropriate message.</remarks>
    /// <param name="attendanceGroup">The data transfer object containing the updated details of the attendance group.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the update operation.</returns>
    public async Task<IBaseResult> UpdateAsync(AttendanceGroupDto attendanceGroup, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<AttendanceGroup>(g => g.Id == attendanceGroup.AttendanceGroupId);
        var result = await repoManager.AttendanceGroups.FirstOrDefaultAsync(spec, true, cancellationToken);
        if (!result.Succeeded || result.Data == null)
            return await Result.FailAsync(result.Messages ?? ["Attendance Group not found"]);

        var entity = result.Data;
        entity.Date = attendanceGroup.Date;
        entity.Name = attendanceGroup.Name;
        entity.Type = attendanceGroup.Type;
        entity.ParentGroupId = attendanceGroup.ParentGroupId;

        repoManager.AttendanceGroups.Update(entity);
        var saveResult = await repoManager.AttendanceGroups.SaveAsync(cancellationToken);
        return saveResult.Succeeded
            ? await Result.SuccessAsync("Attendance group successfully updated")
            : await Result.FailAsync(saveResult.Messages);
    }

    /// <summary>
    /// Asynchronously deletes an attendance group identified by the specified ID.
    /// </summary>
    /// <param name="attendanceGroupId">The unique identifier of the attendance group to delete. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the operation.</returns>
    public async Task<IBaseResult> DeleteAsync(string attendanceGroupId, CancellationToken cancellationToken = default)
    {
        var delete = await repoManager.AttendanceGroups.DeleteAsync(attendanceGroupId, cancellationToken);
        if (!delete.Succeeded)
            return await Result.FailAsync(delete.Messages);

        var save = await repoManager.AttendanceGroups.SaveAsync(cancellationToken);
        return save.Succeeded
            ? await Result.SuccessAsync("Attendance group successfully deleted")
            : await Result.FailAsync(save.Messages);
    }
}
