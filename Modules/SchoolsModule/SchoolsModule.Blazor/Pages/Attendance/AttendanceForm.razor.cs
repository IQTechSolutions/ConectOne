using Microsoft.AspNetCore.Components;

namespace SchoolsModule.Blazor.Pages.Attendance
{
    /// <summary>
    /// Represents a form for managing attendance, including group identification and attendance type.
    /// </summary>
    /// <remarks>This class is typically used in scenarios where attendance data needs to be captured or
    /// processed for a specific group. The <see cref="GroupId"/> property identifies the group, and the  <see
    /// cref="AttendanceType"/> property specifies the type of attendance being recorded.</remarks>
    public partial class AttendanceForm
    {
        /// <summary>
        /// Gets or sets the unique identifier for the group.
        /// </summary>
        [Parameter] public string GroupId { get; set; }

        /// <summary>
        /// Gets or sets the type of attendance represented as an integer value.
        /// </summary>
        [Parameter] public int AttendanceType { get; set; }
    }
}
