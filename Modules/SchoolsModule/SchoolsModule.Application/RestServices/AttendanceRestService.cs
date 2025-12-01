using ConectOne.Domain.Extensions;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Application.RestServices
{
    /// <summary>
    /// Provides methods for managing and retrieving attendance data through RESTful API calls.
    /// </summary>
    /// <remarks>This service acts as a client for interacting with attendance-related endpoints, enabling
    /// operations such as retrieving attendance lists, creating attendance groups, and exporting attendance data. It
    /// relies on an <see cref="IBaseHttpProvider"/> to perform HTTP requests.</remarks>
    /// <param name="provider"></param>
    public class AttendanceRestService(IBaseHttpProvider provider) : IAttendanceService
    {
        /// <summary>
        /// Retrieves a list of learner attendance records that need to be completed.
        /// </summary>
        /// <remarks>The returned list is filtered based on the criteria specified in the args parameter.
        /// If no attendance records match the criteria, the result will contain an empty list.</remarks>
        /// <param name="args">The request parameters used to filter and customize the attendance list.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  IBaseResult{T} object with a
        /// list of LearnerAttendanceDto  representing the attendance records to be completed.</returns>
        public async Task<IBaseResult<List<LearnerAttendanceDto>>> GetAttendanceListToCompleteAsync(AttendanceListRequest args, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<List<LearnerAttendanceDto>>($"attendance/required?{args.GetQueryString()}");
            return result;
        }

        /// <summary>
        /// Creates an attendance group asynchronously.
        /// </summary>
        /// <remarks>This method sends a request to create an attendance group using the provided details.
        /// Ensure that the <paramref name="args"/> parameter contains valid data before calling this method.</remarks>
        /// <param name="args">The request object containing the details of the attendance group to be created.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> CreateAttendanceGroupAsync(AttendanceResultListRequest args, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync($"attendance", args);
            return result;
        }

        /// <summary>
        /// Exports attendance data for a specified group based on the provided request.
        /// </summary>
        /// <remarks>This method sends the export request to the underlying provider and returns the
        /// result. Ensure that the <paramref name="request"/> object is properly populated before calling this
        /// method.</remarks>
        /// <param name="request">The request object containing the parameters for the attendance export operation. This includes details such
        /// as the group identifier and any additional filters.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation will terminate early if the token is canceled.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a string representing the
        /// exported attendance data, typically in a serialized format.</returns>
        public async Task<IBaseResult<string>> ExportAttendanceGroup(ExportAttendanceRequest request, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<string, ExportAttendanceRequest>($"attendance/exportAttendance", request);
            return result;
        }
    }
}
