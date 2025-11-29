using ConectOne.Domain.Enums;
using FilingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.Entities;
using NeuralTech.Base.Enums;

namespace MessagingModule.Domain.DataTransferObjects
{
    /// <summary>
    /// The MessageDto record encapsulates message-related data transferred between the client 
    /// and server layers. It can be constructed from various sources (e.g., a <see cref="Message"/> 
    /// entity or a <see cref="MessageViewModel"/>), and it provides a method to convert 
    /// back into a <see cref="Message"/> entity for persistence or additional processing.
    /// </summary>
    public record MessageDto
    {
        #region Constructors

        /// <summary>
        /// Parameterless constructor to allow manual property assignment.
        /// </summary>
        public MessageDto() { }

        /// <summary>
        /// Constructs a <see cref="MessageDto"/> by projecting a <see cref="Message"/> entity 
        /// from the database, including attachments, document links, and sender/receiver data.
        /// </summary>
        /// <param name="message">The domain entity representing a message.</param>
        public MessageDto(Message message)
        {
            MessageId = message.Id;
            DeliveredTime = message.DeliveredTime;
            ReadTime = message.ReadTime;
            CreatedTime = message.CreatedOn;
            Subject = message.Subject;
            ShortDescription = message.ShortDescription;
            Message = message.Description;
            Public = message.Public;


            MessageType = message.MessageType;

            //// Receiver info
            //ReceiverId = message.ReceiverId;
            //ReceiverName = message.Receiver?.FirstName + " " + message.Receiver?.LastName;
            //ReceiverImageUrl = message.Receiver?.Images
            //    .FirstOrDefault(c => c.ImageType == Filing.Base.DataTypes.UploadType.Cover)
            //    ?.RelativePath;

            EntityId = message.EntityId;

            // Split any stored links into a list
            DocumentLinks = !string.IsNullOrEmpty(message.DocumentLinks)
                ? message.DocumentLinks.Split(";")
                : new List<string>();

            // Convert attached documents into DocumentDto objects
            Documents = message.Documents
                .Select(c => new DocumentDto() { FileName = c.Document.FileName, Size = c.Document.Size, Url = c.Document.RelativePath})
                .ToList();

            // Optionally set the created time again (redundant if already set above)
            CreatedTime = message.CreatedOn;
        }

        #endregion

        /// <summary>
        /// Unique ID for this message, typically a GUID.
        /// </summary>
        public string? MessageId { get; init; }

        /// <summary>
        /// An optional ID referencing a corresponding notification record, if any.
        /// </summary>
        public string? NotificationId { get; init; }

        /// <summary>
        /// Timestamp indicating when this message was created.
        /// </summary>
        public DateTime? CreatedTime { get; init; }

        /// <summary>
        /// Timestamp indicating when this message was delivered to the recipient, if known.
        /// </summary>
        public DateTime? DeliveredTime { get; init; }

        /// <summary>
        /// Timestamp indicating when the recipient read the message, if it has been read.
        /// </summary>
        public DateTime? ReadTime { get; init; }

        /// <summary>
        /// The subject or title of the message.
        /// </summary>
        public string Subject { get; init; } = null!;

        /// <summary>
        /// A short preview or snippet of the message’s content.
        /// </summary>
        public string? ShortDescription { get; init; }

        /// <summary>
        /// The main body or content of the message.
        /// </summary>
        public string? Message { get; init; }

        /// <summary>
        /// Indicates the gender or audience this message is targeted at, if applicable.
        /// Defaults to <see cref="ConectOne.Domain.Enums.Gender.All"/>.
        /// </summary>
        public Gender Gender { get; init; }

        /// <summary>
        /// ID of the user who sent the message.
        /// </summary>
        public string? SenderId { get; init; }

        /// <summary>
        /// The display name of the sender, if available.
        /// </summary>
        public string? SenderName { get; init; }

        /// <summary>
        /// An optional URL or path to the sender’s image/avatar.
        /// </summary>
        public string? SenderImageUrl { get; init; }

        /// <summary>
        /// A <see cref="MessageType"/> indicating how or why this message was generated (e.g., Global, Parent).
        /// </summary>
        public MessageType MessageType { get; init; }

        /// <summary>
        /// ID of the user to whom this message was sent.
        /// </summary>
        public string? ReceiverId { get; init; }

        /// <summary>
        /// The display name of the receiver, if available.
        /// </summary>
        public string? ReceiverName { get; init; }

        /// <summary>
        /// An optional URL or path to the receiver’s image/avatar.
        /// </summary>
        public string? ReceiverImageUrl { get; init; }

        /// <summary>
        /// An optional entity ID that the message might relate to (e.g., a product, event, or user).
        /// </summary>
        public string? EntityId { get; set; }

        /// <summary>
        /// Potentially used for displaying a timestamp of message receipt as a string in the UI.
        /// </summary>
        public string? MessageReceivedTimeString { get; init; }

        /// <summary>
        /// Potentially used for displaying a timestamp of message read time as a string in the UI.
        /// </summary>
        public string? MessageReadTimeString { get; init; }

        /// <summary>
        /// Potentially used for displaying a timestamp of when the message was last modified as a string.
        /// </summary>
        public string? MessageModifiedTimeString { get; init; }

        public bool Public { get; set; } = false;

        /// <summary>
        /// A list of file/document records (mapped as <see cref="DocumentDto"/>) attached to this message.
        /// </summary>
        public List<DocumentDto> Documents { get; init; } = [];

        /// <summary>
        /// A collection of external links or URLs associated with this message (e.g. user-generated links).
        /// </summary>
        public ICollection<string> DocumentLinks { get; set; } = [];

        #region ReadOnly

        /// <summary>
        /// True if the message has been read (i.e., <see cref="ReadTime"/> is set).
        /// </summary>
        public bool Read => ReadTime != null;

        /// <summary>
        /// True if the message has been delivered (i.e., <see cref="DeliveredTime"/> is set).
        /// </summary>
        public bool Delivered => DeliveredTime != null;

        #endregion

        /// <summary>
        /// Converts this DTO back into a <see cref="Message"/> entity,
        /// including any attached documents or links.
        /// </summary>
        /// <returns>A new <see cref="Message"/> entity populated with this DTO’s data.</returns>
        public Message CreateMessage()
        {
            var message = new Message
            {
                Id = string.IsNullOrEmpty(MessageId) ? Guid.NewGuid().ToString() : MessageId,
                ReadTime = ReadTime,
                DeliveredTime = DeliveredTime,
                Subject = Subject,
                ShortDescription = ShortDescription,
                Description = string.IsNullOrEmpty(Message) ? "" : Message,
                EntityId = EntityId,
                MessageType = MessageType,
                SenderId = SenderId,
                Public = Public,
          //      ReceiverId = ReceiverId,
                DocumentLinks = DocumentLinks.Any() && DocumentLinks is not null ?  string.Join(";", DocumentLinks) : "",
                CreatedOn = DateTime.Now
            };

            return message;
        }
    }
}
