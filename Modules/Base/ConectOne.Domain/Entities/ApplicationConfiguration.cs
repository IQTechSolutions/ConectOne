namespace ConectOne.Domain.Entities
{
    /// <summary>
    /// Represents configuration settings for an application, including application name, contact email addresses, and
    /// web address.
    /// </summary>
    public class ApplicationConfiguration
    {
        /// <summary>
        /// Gets or sets the name of the application.
        /// </summary>
        public string AppliactionName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address of the administrator responsible for managing the application.
        /// </summary>
        public string AdminEmailAddress { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address to be used for customer support inquiries.
        /// </summary>
        public string SupportEmailAddress { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address to which abuse reports should be sent.
        /// </summary>
        public string AbuseEmailAddress { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address used for general information or contact purposes.
        /// </summary>
        public string InfoEmailAddress { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address used as the sender for automated or no-reply messages.
        /// </summary>
        public string DoNotReplyEmailAddress { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the web address associated with this instance.
        /// </summary>
        public string WebAddress { get; set; } = string.Empty;
    }
}
