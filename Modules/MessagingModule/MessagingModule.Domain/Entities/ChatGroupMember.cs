using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;
using IdentityModule.Domain.Entities;

namespace MessagingModule.Domain.Entities
{
    /// <summary>
    /// Represents the relationship between a user and a chat group, 
    /// including the time the user joined the group.
    /// </summary>
    public class ChatGroupMember : EntityBase<string>
    {
        /// <summary>
        /// Gets or sets the ID of the associated chat group.
        /// This is a foreign key to the <see cref="Group"/> navigation property.
        /// </summary>
        [ForeignKey(nameof(Group))] public string GroupId { get; set; }

        /// <summary>
        /// Gets or sets the navigation reference to the related <see cref="ChatGroup"/>.
        /// </summary>
        public ChatGroup Group { get; set; }

        /// <summary>
        /// Gets or sets the ID of the associated user.
        /// This is a foreign key to the <see cref="User"/> navigation property.
        /// </summary>
        [ForeignKey(nameof(User))] public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the navigation reference to the user who is a member of the chat group.
        /// </summary>
        public ApplicationUser User { get; set; }

        /// <summary>
        /// Gets or sets the UTC timestamp of when the user joined the group.
        /// </summary>
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}
