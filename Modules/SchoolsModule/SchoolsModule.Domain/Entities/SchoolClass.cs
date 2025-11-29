using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;
using MessagingModule.Domain.Entities;

namespace SchoolsModule.Domain.Entities
{
    /// <summary>
    /// Represents a school class entity with links to grade, learners, personnel, and notifications.
    /// </summary>
    public class SchoolClass : EntityBase<string>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the school class (e.g., "Grade 10 A").
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether a chat group should be automatically created for this class.
        /// </summary>
        public bool AutoCreateChatGroup { get; set; }

        /// <summary>
        /// Gets or sets the foreign key identifier for the associated grade.
        /// </summary>
        [ForeignKey(nameof(Grade))] public string? GradeId { get; set; }

        /// <summary>
        /// Gets or sets the grade entity to which this class belongs.
        /// </summary>
        public SchoolGrade? Grade { get; set; }

        /// <summary>
        /// Gets or sets the learners assigned to this class.
        /// </summary>
        public virtual ICollection<Learner> Learners { get; set; } = [];

        /// <summary>
        /// Gets or sets the teachers assigned to this class.
        /// </summary>
        public virtual ICollection<Teacher> PersonnelCollection { get; set; } = [];

        /// <summary>
        /// Gets or sets the notifications associated with this class.
        /// </summary>
        public virtual ICollection<Notification> Notifications { get; set; } = [];

        #endregion
    }
}
