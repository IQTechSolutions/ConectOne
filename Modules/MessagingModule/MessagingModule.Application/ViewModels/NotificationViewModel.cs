using ConectOne.Domain.Entities;
using FilingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.DataTransferObjects;
using NeuralTech.Base.Enums;

namespace MessagingModule.Application.ViewModels
{
    /// <summary>
    /// Represents a view model for displaying notification data in the UI. 
    /// It provides properties for showing who receives the notification, 
    /// message-related details (title, content, URLs), and timestamps 
    /// (when created, read, or sent). Optionally contains document attachments.
    /// </summary>
    public class NotificationViewModel : ModalModel
    {
        #region Constructors

        /// <summary>
        /// Parameterless constructor to allow manual assignment of fields/properties.
        /// </summary>
        public NotificationViewModel() { }

        /// <summary>
        /// Constructs a NotificationViewModel from a <see cref="NotificationDto"/>, 
        /// copying over relevant fields (receiver info, message details, etc.) 
        /// and providing default images or fallback data if needed.
        /// </summary>
        /// <param name="notification">The DTO containing notification data.</param>
        public NotificationViewModel(NotificationDto notification)
        {
            NotificationId = notification.NotificationId!;
            ReceiverId = notification.ReceiverId!;
            ReceiverName = string.IsNullOrEmpty(notification.ReceiverName)
                ? ""
                : notification.ReceiverName;
            ReceiverImageUrl = string.IsNullOrEmpty(notification.ReceiverImageUrl)
                ? "_content/FilingModule.Blazor/images/profileImage128x128.png"
                : notification.ReceiverImageUrl;

            // If notification.MessageType is null, default to Global.
            MessageType = notification.MessageType;

            Title = notification.Title!;
            ShortDescription = notification.ShortDescription ?? string.Empty;
            Message = notification.Message ?? string.Empty;
            NotificationUrl = notification.NotificationUrl ?? string.Empty;

            // Timestamps
            Created = notification.Created!.Value;
            ReadTime = notification.ReadTime;
            SendTime = notification.SendTime;
        }

        #endregion

        /// <summary>
        /// A unique identifier for this notification (often a GUID).
        /// </summary>
        public string NotificationId { get; set; } = null!;

        /// <summary>
        /// The user ID of the notification’s receiver.
        /// </summary>
        public string ReceiverId { get; set; } = null!;

        /// <summary>
        /// Display-friendly name for the receiver (e.g., "John Doe").
        /// </summary>
        public string ReceiverName { get; set; } = null!;

        /// <summary>
        /// A URL to the receiver’s profile image (or a fallback image).
        /// </summary>
        public string? ReceiverImageUrl { get; set; }

        /// <summary>
        /// Indicates the type/category of the notification (e.g., Global, Learner, etc.).
        /// </summary>
        public MessageType MessageType { get; set; }

        /// <summary>
        /// A brief title or subject line for the notification.
        /// </summary>
        public string Title { get; set; } = null!;

        /// <summary>
        /// A short synopsis or teaser text for the notification.
        /// </summary>
        public string ShortDescription { get; set; } = null!;

        /// <summary>
        /// The main message body. 
        /// </summary>
        public string Message { get; set; } = null!;

        /// <summary>
        /// A link or path providing more details about the notification.
        /// </summary>
        public string NotificationUrl { get; set; } = null!;

        /// <summary>
        /// Timestamp indicating when this notification was first created.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Timestamp indicating when the receiver read/opened this notification, if applicable.
        /// </summary>
        public DateTime? ReadTime { get; set; }

        /// <summary>
        /// Timestamp indicating when the notification was sent, if known.
        /// </summary>
        public DateTime? SendTime { get; set; }

        /// <summary>
        /// Indicates whether the notification has been read (true if <see cref="ReadTime"/> is not null).
        /// </summary>
        public bool Read => ReadTime != null;

        /// <summary>
        /// Indicates whether the notification has been sent (true if <see cref="SendTime"/> is not null).
        /// </summary>
        public bool Sent => SendTime != null;

        /// <summary>
        /// Any associated documents or attachments, represented as <see cref="DocumentDto"/> objects.
        /// </summary>
        public ICollection<DocumentDto> Documents { get; set; } = [];

        #region Methods

        /// <summary>
        /// Converts the current notification entity to a <see cref="NotificationDto"/> object.
        /// </summary>
        /// <remarks>This method maps the properties of the current notification entity to a new instance
        /// of  <see cref="NotificationDto"/>. The resulting DTO can be used for data transfer purposes,  such as
        /// sending notification details to a client application.</remarks>
        /// <returns>A <see cref="NotificationDto"/> object containing the mapped properties of the current notification entity.</returns>
        public NotificationDto ToDto()
        {
            return new NotificationDto
            {
                NotificationId = this.NotificationId,
                ReceiverId = this.ReceiverId,
                ReceiverName = this.ReceiverName,
                ReceiverImageUrl = this.ReceiverImageUrl,
                MessageType = this.MessageType,
                Title = this.Title,
                ShortDescription = this.ShortDescription,
                Message = this.Message,
                NotificationUrl = this.NotificationUrl,
                Created = this.Created,
                ReadTime = this.ReadTime,
                SendTime = this.SendTime
            };
        }
    
        #endregion
    }
}
