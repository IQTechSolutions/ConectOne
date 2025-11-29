using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;
using SchoolsModule.Domain.Enums;

namespace SchoolsModule.Domain.Entities
{
    /// <summary>
    /// Represents a record of a learner's attendance for a specific schedule.
    /// </summary>
    /// <remarks>This class is used to track attendance details, including the learner's ID, the associated
    /// schedule, the attendance status, the date of the attendance, and any additional notes.</remarks>
    public class AttendanceRecord : EntityBase<string>
    {
        /// <summary>
        /// Gets or sets the date the attendance was recorded.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the attendance status of an individual.
        /// </summary>
        public AttendanceStatus Status { get; set; }

        /// <summary>
        /// Gets or sets additional notes or comments.
        /// </summary>
        public string? Notes { get; set; }

        #region OneToManyRelationships

        /// <summary>
        /// Gets or sets the unique identifier for the learner associated with this entity.
        /// </summary>
        [ForeignKey(nameof(Learner))] public string LearnerId { get; set; }

        /// <summary>
        /// Gets or sets the learner associated with the current context.
        /// </summary>
        public Learner Learner { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for the schedule.
        /// </summary>
        [ForeignKey(nameof(Group))] public string GroupId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the attendance group associated with the current entity.
        /// </summary>
        public AttendanceGroup Group { get; set; }

        #endregion
    }
}
