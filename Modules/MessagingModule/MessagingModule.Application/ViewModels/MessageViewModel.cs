using ConectOne.Domain.Enums;
using FilingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.DataTransferObjects;
using NeuralTech.Base.Enums;

namespace MessagingModule.Application.ViewModels
{
    /// <summary>
    /// The <see cref="MessageViewModel"/> serves as a view model for displaying message-related data in the UI layer. 
    /// It includes sender and receiver information, content details (subject, message body, etc.), 
    /// timestamps (delivered/read), and attached documents or links.
    /// </summary>
    public class MessageViewModel
    {
        #region Constructors

        /// <summary>
        /// Parameterless constructor to allow manual instantiation and property assignment.
        /// </summary>
        public MessageViewModel()
        {
        }

        /// <summary>
        /// Initializes the <see cref="MessageViewModel"/> from a <see cref="MessageDto"/>,
        /// copying relevant fields (timestamps, subject, content, and so forth).
        /// </summary>
        /// <param name="message">A <see cref="MessageDto"/> containing source data.</param>
        public MessageViewModel(MessageDto message)
        {
            MessageId = message.MessageId;
            DeliveredTime = message.DeliveredTime;
            ReadTime = message.ReadTime;
            Subject = message.Subject;
            Gender = message.Gender;
            ShortDescription = message.ShortDescription;
            Message = message.Message;
            MessageType = message.MessageType;
            SenderId = message.SenderId;
            SenderName = message.SenderName;
            SenderImageUrl = message.SenderImageUrl;
            ReceiverId = message.ReceiverId;
            ReceiverName = message.ReceiverName;
            ReceiverImageUrl = message.ReceiverImageUrl;
            MessageReceivedTimeString = message.MessageReceivedTimeString;
            Public = message.Public;
            EntityId = message.EntityId;

            Documents = message.Documents;
            DocumentLinks = message.DocumentLinks;
        }

        #endregion

        /// <summary>
        /// Unique identifier for the message, defaults to a new GUID if not set.
        /// </summary>
        public string? MessageId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Optional entity ID the message may relate to (e.g., event, product, or user).
        /// </summary>
        public string? EntityId { get; set; }

        /// <summary>
        /// When the message was delivered to the recipient, if known.
        /// </summary>
        public DateTime? DeliveredTime { get; set; }

        /// <summary>
        /// When the recipient read the message, if known.
        /// </summary>
        public DateTime? ReadTime { get; set; }

        /// <summary>
        /// Subject or title of the message.
        /// </summary>
        public string Subject { get; set; } = null!;

        /// <summary>
        /// A short preview or snippet describing the message.
        /// </summary>
        public string? ShortDescription { get; set; }

        /// <summary>
        /// The main body content of the message.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Indicates the gender or audience targeted by the message, if applicable.
        /// Defaults to <see cref="ConectOne.Domain.Enums.Gender.All"/>.
        /// </summary>
        public Gender Gender { get; set; } = Gender.All;

        /// <summary>
        /// Gets or sets a value indicating whether notifications should be sent.
        /// </summary>
        public bool SendNotification { get; set; } = true;

        public bool Public { get; set; } = false;

        /// <summary>
        /// The ID of the user who sent this message.
        /// </summary>
        public string? SenderId { get; set; }

        /// <summary>
        /// The display name of the sender, if available.
        /// </summary>
        public string? SenderName { get; set; }

        /// <summary>
        /// Optional URL or path to an image representing the sender (e.g., an avatar).
        /// </summary>
        public string? SenderImageUrl { get; set; }

        /// <summary>
        /// Indicates the message category or origin (e.g., Global, Parent, etc.).
        /// </summary>
        public MessageType MessageType { get; set; } = MessageType.Global;

        /// <summary>
        /// The ID of the user to whom this message is addressed.
        /// </summary>
        public string? ReceiverId { get; set; }

        /// <summary>
        /// The display name of the receiver, if available.
        /// </summary>
        public string? ReceiverName { get; set; }

        /// <summary>
        /// Optional URL or path to an image representing the receiver (e.g., an avatar).
        /// </summary>
        public string? ReceiverImageUrl { get; set; }

        /// <summary>
        /// A string representation of when the message was received (optional for UI display).
        /// </summary>
        public string? MessageReceivedTimeString { get; set; }

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
        /// Collection of attached documents (e.g., images or files) represented by <see cref="DocumentFileDto"/> objects.
        /// </summary>
        public ICollection<DocumentDto> Documents { get; set; } = new List<DocumentDto>();

        /// <summary>
        /// A list of external links or URLs associated with this message.
        /// </summary>
        public ICollection<string> DocumentLinks { get; set; } = new List<string>();

        #region Methods

        /// <summary>
        /// Converts the current message instance to a <see cref="MessageDto"/> object.
        /// </summary>
        /// <remarks>This method creates a new <see cref="MessageDto"/> instance and populates its
        /// properties based on the current object's state. If the <see cref="MessageId"/> is null or empty, a new GUID
        /// is generated and used as the message identifier.</remarks>
        /// <returns>A <see cref="MessageDto"/> object containing the data from the current message instance.</returns>
        public MessageDto ToDto()
        {
            return new MessageDto
            {
                MessageId = string.IsNullOrEmpty(this.MessageId)
                    ? Guid.NewGuid().ToString()
                    : this.MessageId,

                DeliveredTime = this.DeliveredTime,
                ReadTime = this.ReadTime,
                Subject = this.Subject,
                ShortDescription = this.ShortDescription,
                Message = this.Message,
                Gender = this.Gender,

                SenderId = this.SenderId,
                SenderName = this.SenderName,
                SenderImageUrl = this.SenderImageUrl,

                MessageType = this.MessageType,
                Public = this.Public,

                //ReceiverId = message.ReceiverId;
                //ReceiverName = message.ReceiverName;
                //ReceiverImageUrl = message.ReceiverImageUrl;

                EntityId = this.EntityId,

                DocumentLinks = this.DocumentLinks,
                Documents = this.Documents.Any() && this.Documents is not null ? this.Documents.ToList() : [],
            };
        }
        public NotificationDto ToNotificationDto(string notificationUrl = "")
        {
            return new NotificationDto
            {
                MessageId = string.IsNullOrEmpty(this.MessageId) ? Guid.NewGuid().ToString() : this.MessageId,
                ReadTime = this.ReadTime,
                Title = this.Subject,
                ShortDescription = this.ShortDescription,
                Message = this.Message,
                Gender = this.Gender,

                MessageType = this.MessageType,
                EntityId = this.EntityId,
                NotificationUrl = notificationUrl,
                DocumentLinks = this.DocumentLinks,
                Documents = this.Documents.Any() && this.Documents is not null ? this.Documents.ToList() : [],
            };
        }

        #endregion
    }
}
