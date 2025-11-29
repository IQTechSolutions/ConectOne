using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Enums;
using FilingModule.Domain.Entities;
using GroupingModule.Domain.Entities;
using MessagingModule.Domain.Entities;

namespace SchoolsModule.Domain.Entities
{
    /// <summary>
    /// Represents an activity group within the school system.
    /// This class includes properties for the group's name, gender, associated age group, teacher, and related collections.
    /// </summary>
    public class ActivityGroup : FileCollection<ActivityGroup, string>
    {
        /// <summary>
        /// Gets or sets the name of the activity group.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether a chat group should be automatically created for this class.
        /// </summary>
        public bool AutoCreateChatGroup { get; set; }

        /// <summary>
        /// Gets or sets the gender associated with the activity group.
        /// </summary>
        public Gender Gender { get; set; }

        #region One-To-Many Relationships

        /// <summary>
        /// Gets or sets the ID of the associated age group.
        /// </summary>
        [ForeignKey(nameof(AgeGroup))] public string? AgeGroupId { get; set; }

        /// <summary>
        /// Gets or sets the associated age group.
        /// </summary>
        public AgeGroup? AgeGroup { get; set; }

        /// <summary>
        /// Gets or sets the ID of the associated teacher.
        /// </summary>
        [ForeignKey(nameof(Teacher))] public string? TeacherId { get; set; }

        /// <summary>
        /// Gets or sets the associated teacher.
        /// </summary>
        public Teacher? Teacher { get; set; }

        #endregion

        #region Collections

        /// <summary>
        /// Gets or sets the collection of participating activity groups.
        /// </summary>
        public virtual ICollection<ParticipatingActivityGroup> ParticipatingActivityGroups { get; set; } = new List<ParticipatingActivityGroup>();

        /// <summary>
        /// Gets or sets the collection of team members in the activity group.
        /// </summary>
        public virtual ICollection<ActivityGroupTeamMember> TeamMembers { get; set; } = new List<ActivityGroupTeamMember>();

        /// <summary>
        /// Gets or sets the collection of categories associated with the activity group.
        /// </summary>
        public virtual ICollection<EntityCategory<ActivityGroup>> Categories { get; set; } = new List<EntityCategory<ActivityGroup>>();

        /// <summary>
        /// Gets or sets the collection of messages related to the activity group.
        /// </summary>
        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

        #endregion
    }
}
