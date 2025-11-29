using ConectOne.Domain.ResultWrappers;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Domain.Interfaces
{
    /// <summary>
    /// Defines methods for managing and retrieving attendance-related data.
    /// </summary>
    /// <remarks>This interface provides functionality for retrieving attendance lists and creating attendance
    /// groups. Implementations of this interface should handle the underlying logic for these operations, including any
    /// necessary validation and data persistence.</remarks>
    public interface IAttendanceService
    {
        /// <summary>
        /// Retrieves a list of learner attendance records that need to be completed.
        /// </summary>
        /// <remarks>This method is designed to retrieve attendance records based on the criteria
        /// specified in the <paramref name="args"/> parameter. The caller can use the <paramref
        /// name="cancellationToken"/> to cancel the operation if needed.</remarks>
        /// <param name="args">The request parameters specifying the criteria for retrieving the attendance list.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// with a list of <see cref="LearnerAttendanceDto"/> objects representing the attendance records to be
        /// completed.</returns>
        Task<IBaseResult<List<LearnerAttendanceDto>>> GetAttendanceListToCompleteAsync(AttendanceListRequest args, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new attendance group asynchronously based on the specified request parameters.
        /// </summary>
        /// <remarks>This method performs the operation asynchronously. Ensure that the provided <paramref
        /// name="args"/> contains all required fields and valid data to avoid errors.</remarks>
        /// <param name="args">The request parameters containing the details required to create the attendance group. This must include
        /// valid data for the group configuration.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. If the operation is canceled, the task will end prematurely.</param>
        /// <returns>An <see cref="IBaseResult"/> representing the outcome of the operation. The result contains information
        /// about the success or failure of the attendance group creation.</returns>
        Task<IBaseResult> CreateAttendanceGroupAsync(AttendanceResultListRequest args, CancellationToken cancellationToken = default);

        /// <summary>
        /// Exports attendance group data based on the specified request parameters.
        /// </summary>
        /// <remarks>The exported data is returned as a string, which may represent a file path, a
        /// serialized data format, or other export output, depending on the implementation. Ensure that the <paramref
        /// name="request"/> contains valid parameters to avoid errors during the export process.</remarks>
        /// <param name="request">The request containing the parameters for the attendance group export. Must not be <see langword="null"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a string representing the
        /// exported attendance group data.</returns>
        Task<IBaseResult<string>> ExportAttendanceGroup(ExportAttendanceRequest request, CancellationToken cancellationToken = default);
    }
}
