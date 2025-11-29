using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;
using GroupingModule.Domain.Entities;

namespace SchoolsModule.Domain.Entities
{
    /// <summary>
    /// Represents a category associated with a participating activity group in the context of an event.
    /// </summary>
    /// <remarks>This class links an activity group category to a specific event, allowing for hierarchical
    /// categorization and organization of activities. It also supports optional parent-child relationships between
    /// categories.</remarks>
    public class ParticipatingActivityGroupCategory : EntityBase<string>
    {
        /// <summary>
        /// Gets or sets the identifier for the activity group category.
        /// </summary>
        [ForeignKey(nameof(ActivityGroup))] public string? ActivityGroupCategoryId { get; set; } 

        /// <summary>
        /// Gets or sets the category associated with an activity group.
        /// </summary>
        public Category<ActivityGroup>? ActivityGroupCategory { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the parent activity category.
        /// </summary>
        public string? ActivityCategoryParentId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated event.
        /// </summary>
        [ForeignKey(nameof(Event))] public string? EventId { get; set; }

        /// <summary>
        /// Gets or sets the school event associated with a specific category of activity groups.
        /// </summary>
        public SchoolEvent<Category<ActivityGroup>>? Event { get; set; }
    }
}
