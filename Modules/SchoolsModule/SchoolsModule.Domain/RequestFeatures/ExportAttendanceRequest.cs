using SchoolsModule.Domain.Enums;

namespace SchoolsModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents a request to export attendance data for a specific group.
    /// </summary>
    /// <remarks>This request allows specifying the group for which attendance data should be exported, as
    /// well as an optional filter for the type of attendance to include in the export.</remarks>
    public class ExportAttendanceRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier for the group.
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// Gets or sets the type of attendance associated with the entity.
        /// </summary>
        public AttendanceType? AttendanceType { get; set; }
    }
}
