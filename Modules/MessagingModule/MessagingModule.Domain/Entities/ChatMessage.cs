using System.ComponentModel.DataAnnotations;
using FilingModule.Domain.Entities;
using MessagingModule.Domain.DataTransferObjects;

namespace MessagingModule.Domain.Entities
{ 

    /// <summary>
    /// Represents a message sent within a chat group, including sender, receiver, content, and metadata.
    /// </summary>
    public class ChatMessage : FileCollection<ChatMessage, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChatMessage"/> class.
        /// </summary>
        public ChatMessage() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatMessage"/> class from a <see cref="ChatMessageDto"/>.
        /// </summary>
        /// <param name="chatMessageDto">The DTO to map from.</param>
        public ChatMessage(ChatMessageDto chatMessageDto)
        {
            Id = chatMessageDto.Id;
            ChatGroupId = chatMessageDto.ChatGroupId;
            SenderId = chatMessageDto.SenderId;
            SenderUserName = chatMessageDto.SenderUserName;
            SenderDisplayName = chatMessageDto.SenderDisplayName;
            Content = chatMessageDto.Content;
            Timestamp = chatMessageDto.Timestamp;
            IsRead = chatMessageDto.IsRead;
            ReadTime = chatMessageDto.ReadTime;
        }

        /// <summary>
        /// Gets or sets the ID of the chat group this message belongs to.
        /// </summary>
        [Required] public string ChatGroupId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who sent the message.
        /// </summary>
        [Required] public string SenderId { get; set; }

        /// <summary>
        /// Gets or sets the sender's username.
        /// </summary>
        [Required] public string SenderUserName { get; set; }

        /// <summary>
        /// Gets or sets the sender's display name.
        /// </summary>
        [Required] public string SenderDisplayName { get; set; }
        
        /// <summary>
        /// Gets or sets the content of the message.
        /// </summary>
        [Required] public string Content { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the message was sent.
        /// Defaults to <see cref="DateTime.UtcNow"/>.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets a value indicating whether the message has been read.
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when this message was read.
        /// </summary>
        public DateTime? ReadTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the message has been archived.
        /// Useful for filtering in UI or for retention policies.
        /// </summary>
        public bool IsArchived { get; set; }
    }

}
