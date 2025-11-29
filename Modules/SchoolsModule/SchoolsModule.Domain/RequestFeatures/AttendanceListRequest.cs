using SchoolsModule.Domain.Enums;

namespace SchoolsModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents a request to retrieve an attendance list for a specific group and attendance type.
    /// </summary>
    /// <param name="GroupId">The unique identifier of the group for which the attendance list is requested. Cannot be null or empty.</param>
    /// <param name="AttendanceType">The type of attendance to retrieve, such as present, absent, or excused.</param>
    public record AttendanceListRequest(string GroupId, AttendanceType AttendanceType);
}
