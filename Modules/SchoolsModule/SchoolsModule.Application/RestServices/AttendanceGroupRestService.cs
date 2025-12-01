using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Application.RestServices
{
    /// <summary>
    /// Provides REST-based operations for managing attendance groups.
    /// </summary>
    /// <remarks>This service allows clients to perform CRUD operations on attendance groups, including
    /// retrieving all groups, fetching paginated results, and managing individual attendance group entities. It is
    /// designed to work with an HTTP provider for communication and implements the <see
    /// cref="IAttendanceGroupService"/> interface.</remarks>
    /// <param name="provider"></param>
    public class AttendanceGroupRestService(IBaseHttpProvider provider) : IAttendanceGroupService
    {
        /// <summary>
        /// Retrieves all attendance groups associated with the specified parent group.
        /// </summary>
        /// <param name="parentGroupId">The unique identifier of the parent group whose attendance groups are to be retrieved. Cannot be null or
        /// empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// containing an enumerable collection of <see cref="AttendanceGroupDto"/> objects representing the attendance
        /// groups.</returns>
        public async Task<IBaseResult<IEnumerable<AttendanceGroupDto>>> AllAttendanceGroupsAsync(string parentGroupId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<AttendanceGroupDto>>($"attendancegroups/all/{parentGroupId}");
            return result;
        }

        /// <summary>
        /// Retrieves a paginated list of attendance groups based on the specified page parameters.
        /// </summary>
        /// <remarks>This method queries the attendance groups using the provided pagination parameters
        /// and returns the results in a paginated format. The caller can use the pagination metadata in the result to
        /// navigate through the available pages.</remarks>
        /// <param name="pageParameters">The parameters that define the pagination and filtering options for the attendance groups.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{AttendanceGroupDto}"/> containing the paginated list of attendance groups.</returns>
        public async Task<PaginatedResult<AttendanceGroupDto>> PagedAttendanceGroupsAsync(AttendanceGroupPageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetPagedAsync<AttendanceGroupDto, AttendanceGroupPageParameters>("attendancegroups/paged", pageParameters);
            return result;
        }

        /// <summary>
        /// Retrieves an attendance group by its unique identifier.
        /// </summary>
        /// <param name="attendanceGroupId">The unique identifier of the attendance group to retrieve. Cannot be null or empty.</param>
        /// <param name="trackChanges">A value indicating whether to track changes to the retrieved entity. Defaults to <see langword="false"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping the <see cref="AttendanceGroupDto"/> for the specified attendance group.</returns>
        public async Task<IBaseResult<AttendanceGroupDto>> AttendanceGroupAsync(string attendanceGroupId, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<AttendanceGroupDto>($"attendancegroups/{attendanceGroupId}");
            return result;
        }

        /// <summary>
        /// Creates or updates an attendance group asynchronously.
        /// </summary>
        /// <remarks>This method sends the provided attendance group data to the underlying provider for
        /// creation or update.  Ensure that the <paramref name="attendanceGroup"/> parameter contains valid data before
        /// calling this method.</remarks>
        /// <param name="attendanceGroup">The data transfer object representing the attendance group to be created or updated.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> CreateAsync(AttendanceGroupDto attendanceGroup, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync("attendancegroups", attendanceGroup);
            return result;
        }

        /// <summary>
        /// Updates an attendance group asynchronously.
        /// </summary>
        /// <param name="attendanceGroup">The attendance group data to be updated.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateAsync(AttendanceGroupDto attendanceGroup, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync("attendancegroups", attendanceGroup);
            return result;
        }

        /// <summary>
        /// Deletes an attendance group asynchronously.
        /// </summary>
        /// <remarks>This method sends a delete request to the underlying provider to remove the specified
        /// attendance group.  Ensure that the <paramref name="attendanceGroupId"/> corresponds to an existing group
        /// before calling this method.</remarks>
        /// <param name="attendanceGroupId">The unique identifier of the attendance group to delete.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the delete operation.</returns>
        public async Task<IBaseResult> DeleteAsync(string attendanceGroupId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync("attendancegroups", attendanceGroupId);
            return result;
        }
    }
}
