using NeuralTech.Base.Enums;

namespace IdentityModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Data Transfer Object (DTO) for representing a recipient.
    /// This DTO is used to transfer recipient data between different layers of the application.
    /// </summary>
    public record RecipientDto
    {
        /// <summary>
        /// Initializes a new instance of the RecipientDto class with the specified recipient details.
        /// </summary>
        /// <param name="id">The unique identifier for the recipient. Cannot be null.</param>
        /// <param name="name">The first name of the recipient. Cannot be null.</param>
        /// <param name="lastName">The last name of the recipient. Cannot be null.</param>
        /// <param name="emailAddresses">A list of email addresses associated with the recipient. Cannot be null or contain null elements.</param>
        /// <param name="receiveNotifications">A value indicating whether the recipient should receive notifications. Set to <see langword="true"/> to
        /// enable notifications; otherwise, <see langword="false"/>.</param>
        /// <param name="receiveEmails">A value indicating whether the recipient should receive emails. Set to <see langword="true"/> to enable
        /// emails; otherwise, <see langword="false"/>.</param>
        /// <param name="coverImageUrl">The URL of the recipient's cover image, or <see langword="null"/> if no image is specified.</param>
        /// <param name="messageType">The type of message associated with the recipient, or <see langword="null"/> if not specified.</param>
        public RecipientDto(string id, string name, string lastName, List<string> emailAddresses, bool receiveNotifications, bool receiveEmails, string? coverImageUrl = null, MessageType? messageType = null)
        {
            Id = id;
            Name = name;
            LastName = lastName;
            EmailAddresses = emailAddresses;
            CoverImageUrl = coverImageUrl;
            ReceiveNotifications = receiveNotifications;
            ReceiveEmails = receiveEmails;
            MessageType = messageType;
        }

        /// <summary>
        /// Gets the unique identifier of the recipient.
        /// </summary>
        public string Id { get; init; }

        /// <summary>
        /// Gets the first name of the recipient.
        /// </summary>
        public string? Name { get; init; }

        /// <summary>
        /// Gets the last name of the recipient.
        /// </summary>
        public string LastName { get; init; }

        /// <summary>
        /// Gets the URL of the cover image for the recipient.
        /// </summary>
        public string? CoverImageUrl { get; init; }

        /// <summary>
        /// Gets the list of email addresses associated with the recipient.
        /// </summary>
        public List<string> EmailAddresses { get; init; } = new();

        /// <summary>
        /// Gets a value indicating whether the user has opted to receive notifications.
        /// </summary>
        public bool ReceiveNotifications { get; init; }

        /// <summary>
        /// Gets a value indicating whether the user has opted to receive emails.
        /// </summary>
        public bool ReceiveEmails { get; init; }

        /// <summary>
        /// Gets or sets the type of the message.
        /// </summary>
        public MessageType? MessageType { get; set; } = null;
    }
}
