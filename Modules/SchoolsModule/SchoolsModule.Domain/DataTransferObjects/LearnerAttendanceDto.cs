using SchoolsModule.Domain.Enums;

namespace SchoolsModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents the attendance information for a learner, including their identification, name, attendance status,
    /// and optional notes.
    /// </summary>
    /// <remarks>This data transfer object is typically used to encapsulate attendance-related details for a
    /// learner in scenarios such as reporting or updating attendance records.</remarks>
    public class LearnerAttendanceDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the group.
        /// </summary>
        public string GroupId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for a learner.
        /// </summary>
        public string LearnerId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the full name of the individual.
        /// </summary>
        public string FullName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the date associated with the current operation or entity.
        /// </summary>
        public DateTime? Date { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the currently selected attendance status.
        /// </summary>
        public AttendanceStatus SelectedStatus { get; set; }

        /// <summary>
        /// Gets or sets optional notes or comments associated with the object.
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether detailed information should be displayed.
        /// </summary>
        private bool ShowDetails { get; set; }
    }
}
