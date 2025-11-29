using System.ComponentModel.DataAnnotations;
using ConectOne.Domain.Entities;

namespace MessagingModule.Domain.Entities
{
    /// <summary>
    /// Represents a chat group within the system. 
    /// A chat group can contain multiple members and messages.
    /// </summary>
    public class ChatGroup : EntityBase<string>
    {
        /// <summary>
        /// Gets or sets the name of the chat group.
        /// This field is required.
        /// </summary>
        [Required] public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the chat group.
        /// </summary>
        public virtual ICollection<ChatGroupMember> Members { get; set; } = [];
    }

}
