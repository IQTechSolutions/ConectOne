using System.ComponentModel.DataAnnotations;

namespace ConectOne.Domain.Mailing.Entities
{
    /// <summary>
    /// Represents the details of an email, including sender, recipient, subject, and content.
    /// </summary>
    public class EmailDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailDetails"/> class.
        /// </summary>
        /// <param name="toName">The name of the recipient.</param>
        /// <param name="toEmail">The email address of the recipient.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="fromName">The name of the sender.</param>
        /// <param name="fromEmail">The email address of the sender.</param>
        /// <param name="content">The content of the email.</param>
        /// <param name="isHtml">Indicates whether the email content is HTML.</param>
        public EmailDetails(string toName, string toEmail, string subject, string fromName, string fromEmail, string? content = null, bool isHtml = true)
        {
            FromName = fromName;
            FromEmail = fromEmail;
            ToName = toName;
            ToEmail = toEmail;
            Subject = subject;
            Content = content;
            IsHtml = isHtml;
        }

        /// <summary>
        /// Gets or sets the name of the sender.
        /// </summary>
        [Display(Name = "Sender")]
        public string FromName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the sender.
        /// </summary>
        [Display(Name = "Sender Email")]
        public string FromEmail { get; set; }

        /// <summary>
        /// Gets or sets the name of the recipient.
        /// </summary>
        [Display(Name = "Recipient")]
        public string ToName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the recipient.
        /// </summary>
        [Display(Name = "Recipient Email")]
        public string ToEmail { get; set; }

        /// <summary>
        /// Gets or sets the subject of the email.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the content of the email.
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the email content is HTML.
        /// </summary>
        [Display(Name = "Is Html")]
        public bool IsHtml { get; set; }
    }
}
