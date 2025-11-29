using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;
using IdentityModule.Domain.Entities;

namespace SchoolsModule.Domain.Entities
{
    /// <summary>
    /// Represents the association between a school event and a user, including metadata about the relationship.
    /// </summary>
    /// <remarks>This class is used to track which users are associated with specific school events.  It
    /// includes references to both the school event and the user, enabling the retrieval of detailed information  about
    /// the event and the user involved.</remarks>
    public class SchoolEventViews : EntityBase<string>
    {
        /// <summary>
        /// Gets or sets the unique identifier of the associated school event.
        /// </summary>
        [ForeignKey(nameof(SchoolEvent))] public string SchoolEventId { get; set; }

        /// <summary>
        /// Gets or sets the school event associated with a specific activity group.
        /// </summary>
        public SchoolEvent<ActivityGroup> SchoolEvent { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the user associated with this entity.
        /// </summary>
        [ForeignKey(nameof(UserInfo))] public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the user information associated with the current context.
        /// </summary>
        public UserInfo UserInfo { get; set; }
    }
}
