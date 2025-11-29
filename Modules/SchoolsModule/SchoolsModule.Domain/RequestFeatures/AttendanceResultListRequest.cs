using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Enums;

namespace SchoolsModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents a request to retrieve a list of attendance results for a specific group and date.
    /// </summary>
    /// <param name="ParentGroupId">The unique identifier of the parent group for which attendance results are requested. Cannot be null or empty.</param>
    /// <param name="GroupName">The name of the group for which attendance results are requested. Cannot be null or empty.</param>
    /// <param name="AttendanceType">The type of attendance being recorded or requested.</param>
    /// <param name="Date">The date for which attendance results are requested.</param>
    /// <param name="AttendanceResults">A list of attendance result data transfer objects representing individual learner attendance records. Cannot be
    /// null.</param>
    public record AttendanceResultListRequest(string ParentGroupId, string GroupName, AttendanceType AttendanceType, DateTime Date, List<LearnerAttendanceDto> AttendanceResults);
}
