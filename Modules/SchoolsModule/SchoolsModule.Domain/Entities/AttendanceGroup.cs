using ConectOne.Domain.Entities;
using SchoolsModule.Domain.Enums;

namespace SchoolsModule.Domain.Entities
{
    /// <summary>
    /// Represents a group used to categorize attendance records.
    /// </summary>
    /// <remarks>An <see cref="AttendanceGroup"/> is typically used to group attendance data by a specific
    /// name and type. This class provides properties to define the group's name and the type of attendance it
    /// represents.</remarks>
    public class AttendanceGroup : EntityBase<string>
    {
        /// <summary>
        /// Gets or sets the date associated with this instance.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the type of attendance associated with the current instance.
        /// </summary>
        public AttendanceType Type { get; set; } 

        /// <summary>
        /// Gets or sets the identifier of the parent entity.
        /// </summary>
        public string ParentGroupId { get; set; }

        /// <summary>
        /// Represents a collection of attendance records.
        /// </summary>
        /// <remarks>This collection is used to store and manage attendance records for individuals. It
        /// can be used to add, remove, or query attendance data.</remarks>
        public ICollection<AttendanceRecord> AttendanceRecords { get; set; } = [];
    }
}
