using MessagingModule.Domain.Entities;

namespace MessagingModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Data Transfer Object (DTO) representing a chat message exchanged between users in a chat group.
    /// </summary>
    public record ChatMessageDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChatMessageDto"/> class.
        /// </summary>
        public ChatMessageDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatMessageDto"/> class 
        /// from the domain <see cref="ChatMessage"/> entity.
        /// </summary>
        /// <param name="chatMessage">The domain model to map from.</param>
        public ChatMessageDto(ChatMessage chatMessage)
        {
            Id = chatMessage.Id;
            ChatGroupId = chatMessage.ChatGroupId;
            SenderId = chatMessage.SenderId;
            SenderUserName = chatMessage.SenderUserName;
            SenderDisplayName = chatMessage.SenderDisplayName;
            Content = chatMessage.Content;
            IsRead = chatMessage.IsRead;
            ReadTime = chatMessage.ReadTime;
            Timestamp = chatMessage.Timestamp;
            Images = chatMessage.Images?.Select(i => i.Image.RelativePath)?.ToList() ?? new List<string>();
            Documents = chatMessage.Documents?.Select(f => f.Document.RelativePath)?.ToList() ?? new List<string>();
            Videos = chatMessage.Videos?.Select(f => f.Video.RelativePath)?.ToList() ?? new List<string>();
        }

        /// <summary>
        /// Gets the unique identifier of the chat message.
        /// </summary>
        public string Id { get; init; }

        /// <summary>
        /// Gets the ID of the chat group this message belongs to.
        /// </summary>
        public string ChatGroupId { get; init; }

        /// <summary>
        /// Gets the ID of the user who sent the message.
        /// </summary>
        public string SenderId { get; init; }

        /// <summary>
        /// Gets the sender's username.
        /// </summary>
        public string SenderUserName { get; init; }

        /// <summary>
        /// Gets the sender's display name for UI rendering.
        /// </summary>
        public string SenderDisplayName { get; init; }

        /// <summary>
        /// Gets the sender's profile image URL (defaults to placeholder image).
        /// </summary>
        public string SenderCoverImageUrl { get; init; } = "_content/FilingModule.Blazor/images/profileImage128x128.png";

        /// <summary>
        /// Gets the main content/body of the message.
        /// </summary>
        public string Content { get; init; }

        /// <summary>
        /// Indicates whether the message has been marked as read.
        /// </summary>
        public bool IsRead { get; init; }

        /// <summary>
        /// Gets the timestamp when the message was read, if available.
        /// </summary>
        public DateTime? ReadTime { get; init; }

        /// <summary>
        /// Gets or sets the timestamp when the message was sent.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the collection of image file paths.
        /// </summary>
        public List<string> Images { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the collection of file paths.
        /// </summary>
        public List<string> Documents { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the collection of videos associated with the current context.
        /// </summary>
        public List<string> Videos { get; set; } = new List<string>();
    }
}
