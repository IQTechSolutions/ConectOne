using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;
using GroupingModule.Domain.Entities;

namespace SchoolsModule.Domain.Entities
{
    /// <summary>
    /// Represents a group participating in an event, associated with an activity group and its members.
    /// </summary>
    /// <remarks>This class links an activity group to a specific event and tracks the members participating
    /// as part of the group.</remarks>
    public class ParticipatingActivityGroup : EntityBase<string>
    {
        /// <summary>
        /// Gets or sets the identifier of the associated activity group.
        /// </summary>
        [ForeignKey(nameof(ActivityGroup))] public string? ActivityGroupId { get; set; } 

        /// <summary>
        /// Gets or sets the activity group associated with the current context.
        /// </summary>
        public ActivityGroup? ActivityGroup { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated event.
        /// </summary>
        [ForeignKey(nameof(Event))] public string? EventId { get; set; }

        /// <summary>
        /// Gets or sets the school event associated with a specific category of activity groups.
        /// </summary>
        public SchoolEvent<Category<ActivityGroup>>? Event { get; set; }

        /// <summary>
        /// Gets or sets the collection of team members participating in the activity group.
        /// </summary>
        public ICollection<ParticipatingActitivityGroupTeamMember> ParticipatingTeamMembers { get; set; } = [];
    }
}
