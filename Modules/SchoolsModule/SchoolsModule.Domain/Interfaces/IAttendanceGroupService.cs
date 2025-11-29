using ConectOne.Domain.ResultWrappers;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Domain.Interfaces;

/// <summary>
/// Service contract for managing <see cref="AttendanceGroup"/> entities.
/// </summary>
public interface IAttendanceGroupService
{
    /// <summary>
    /// Retrieves all attendance groups associated with the specified parent group.
    /// </summary>
    /// <param name="parentGroupId">The identifier of the parent group for which to retrieve attendance groups. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> of
    /// <see cref="IEnumerable{AttendanceGroupDto}"/> representing the attendance groups.</returns>
    Task<IBaseResult<IEnumerable<AttendanceGroupDto>>> AllAttendanceGroupsAsync(string parentGroupId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a paginated list of attendance groups based on the specified paging parameters.
    /// </summary>
    /// <param name="pageParameters">The parameters that define the pagination and filtering criteria for the attendance groups.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests, allowing the operation to be cancelled.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see
    /// cref="PaginatedResult{AttendanceGroupDto}"/> with the paginated attendance groups.</returns>
    Task<PaginatedResult<AttendanceGroupDto>> PagedAttendanceGroupsAsync(AttendanceGroupPageParameters pageParameters, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the attendance group details for the specified group ID.
    /// </summary>
    /// <param name="attendanceGroupId">The unique identifier of the attendance group to retrieve.</param>
    /// <param name="trackChanges">Indicates whether to track changes to the retrieved entity. Defaults to <see langword="false"/>.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see
    /// cref="IBaseResult{AttendanceGroupDto}"/> with the attendance group details.</returns>
    Task<IBaseResult<AttendanceGroupDto>> AttendanceGroupAsync(string attendanceGroupId, bool trackChanges = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously creates a new attendance group based on the provided data.
    /// </summary>
    /// <param name="attendanceGroup">The data transfer object containing the details of the attendance group to be created. Cannot be null.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the outcome of the creation process.</returns>
    Task<IBaseResult> CreateAsync(AttendanceGroupDto attendanceGroup, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously updates the specified attendance group.
    /// </summary>
    /// <remarks>This method updates the attendance group with the provided details. Ensure that the <paramref
    /// name="attendanceGroup"/> contains valid data before calling this method.</remarks>
    /// <param name="attendanceGroup">The attendance group data transfer object containing the updated information.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous update operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the outcome of the update.</returns>
    Task<IBaseResult> UpdateAsync(AttendanceGroupDto attendanceGroup, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously deletes the attendance group with the specified identifier.
    /// </summary>
    /// <param name="attendanceGroupId">The unique identifier of the attendance group to delete. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the delete operation.</returns>
    Task<IBaseResult> DeleteAsync(string attendanceGroupId, CancellationToken cancellationToken = default);
}

