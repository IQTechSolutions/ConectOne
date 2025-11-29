namespace ConectOne.Domain.Mailing
{
    /// <summary>
    /// Represents the configuration settings required for sending emails.
    /// </summary>
    public class EmailConfiguration
    {
        /// <summary>
        /// Gets or sets the email address from which the emails will be sent.
        /// </summary>
        public string From { get; set; } = null!;

        /// <summary>
        /// Gets or sets the SMTP server address.
        /// </summary>
        public string SmtpServer { get; set; } = null!;

        /// <summary>
        /// Gets or sets the port number for the SMTP server.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the username for the SMTP server authentication.
        /// </summary>
        public string UserName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the password for the SMTP server authentication.
        /// </summary>
        public string Password { get; set; } = null!;

        /// <summary>
        /// Gets or sets the API key for SendGrid.
        /// </summary>
        public string SendGridApiKey { get; set; } = null!;

        /// <summary>
        /// Gets or sets the reply-to email address.
        /// </summary>
        public string ReplyEmail { get; set; } = null!;

        public string LogoUrl { get; set; } = null!;

        public string Caption { get; set; } = null!;

        public string LogoLink { get; set; } = null!;
    }
}