using System.Net;
using System.Net.Mail;
using ConectOne.Domain.Mailing.Entities;
using ConectOne.Domain.Mailing.Interfaces;
using ConectOne.Domain.ResultWrappers;

namespace ConectOne.Domain.Mailing.Services
{
    /// <summary>
    /// The <see cref="EmailSender"/> class provides functionality for sending emails 
    /// (both synchronously and asynchronously) via SMTP. 
    /// It implements the <see cref="IEmailSender"/> interface, 
    /// ensuring a consistent contract for email operations across the application.
    /// 
    /// <para>
    /// By default, <c>EnableSsl</c> is set to <c>false</c>, but you may need to set it to <c>true</c> 
    /// depending on your SMTP server’s security requirements.
    /// </para>
    /// <para>
    /// Exceptions encountered while sending are caught and returned via 
    /// </para>
    /// </summary>
    /// <remarks>
    /// Requires a valid <see cref="EmailConfiguration"/> that specifies 
    /// SMTP server details (host, port, username, password).
    /// </remarks>
    public class EmailSender(EmailConfiguration emailConfig) : IEmailSender
    {
        /// <summary>
        /// Sends an email asynchronously using the <see cref="EmailConfiguration"/> for SMTP details.
        /// </summary>
        /// <param name="details">
        /// Contains key email parameters such as sender, recipient, subject, and body content.
        /// </param>
        /// <returns>
        /// An <see cref="IBaseResult"/> indicating whether the email was sent successfully or failed.
        /// </returns>
        public async Task<IBaseResult> SendEmailAsync(EmailDetails details)
        {
            // Construct a MailMessage with HTML formatting.
            var msg = new MailMessage(details.FromEmail, details.ToEmail, details.Subject, details.Content)
            {
                IsBodyHtml = true
            };

            try
            {
                // Set up the SMTP client using credentials from emailConfig.
                var smtpClient = new SmtpClient(emailConfig.SmtpServer, emailConfig.Port)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(emailConfig.UserName, emailConfig.Password),
                    EnableSsl = false, // Adjust to 'true' if your SMTP server requires SSL.
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };

                // Send the email asynchronously.
                await smtpClient.SendMailAsync(msg);

                // Return success if sending did not throw an exception.
                return await Result.SuccessAsync(
                    $"Email was successfully sent to {details.ToName} at {details.ToEmail}"
                );
            }
            catch (Exception ex)
            {
                // Log or handle the exception as necessary, returning a failed result.
                return await Result.FailAsync($"{nameof(EmailSender)} => {ex.Message}");
            }
        }

        /// <summary>
        /// Sends multiple emails asynchronously, each email’s parameters 
        /// are provided in the <paramref name="details"/> list.
        /// </summary>
        /// <param name="details">A list of <see cref="EmailDetails"/> representing emails to send.</param>
        /// <returns>An <see cref="IBaseResult"/> indicating success or failure.</returns>
        public async Task<IBaseResult> SendEmailsAsync(List<EmailDetails> details)
        {
            try
            {
                // Configure the SMTP client once outside the loop.
                var smtpClient = new SmtpClient(emailConfig.SmtpServer, emailConfig.Port)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(emailConfig.UserName, emailConfig.Password),
                    EnableSsl = false, // Adjust to 'true' if your SMTP server requires SSL.
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };

                // Send each email in the list asynchronously.
                foreach (var detail in details)
                {
                    var msg = new MailMessage(detail.FromEmail, detail.ToEmail, detail.Subject, detail.Content)
                    {
                        IsBodyHtml = true
                    };

                    await smtpClient.SendMailAsync(msg);
                }

                return await Result.SuccessAsync("Emails were successfully sent");
            }
            catch (Exception ex)
            {
                return await Result.FailAsync($"{nameof(EmailSender)} => {ex.Message}");
            }
        }

        /// <summary>
        /// Sends an email synchronously using SMTP details from <see cref="EmailDetails"/>.
        /// </summary>
        /// <param name="details">Contains parameters like sender, recipient, subject, and content.</param>
        /// <returns>
        /// An <see cref="IBaseResult"/> that indicates success or failure in sending the email.
        /// </returns>
        /// <remarks>
        /// This call blocks until the email is fully sent or fails. 
        /// For high-traffic scenarios, consider using <see cref="SendEmailAsync"/>.
        /// </remarks>
        public IBaseResult SendEmail(EmailDetails details)
        {
            // Construct a MailMessage with HTML formatting.
            var msg = new MailMessage(details.FromEmail, details.ToEmail, details.Subject, details.Content)
            {
                IsBodyHtml = true
            };

            try
            {
                // Set up the SMTP client with credentials from emailConfig.
                var smtpClient = new SmtpClient(emailConfig.SmtpServer, emailConfig.Port)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(emailConfig.UserName, emailConfig.Password),
                    EnableSsl = false, // Adjust to 'true' if your SMTP server requires SSL.
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };

                // Synchronous call to send the email.
                smtpClient.Send(msg);

                return Result.Success(
                    $"Email was successfully sent to {details.ToName} at {details.ToEmail}"
                );
            }
            catch (Exception ex)
            {
                return Result.Fail($"{nameof(EmailSender)} => {ex.Message}");
            }
        }
    }
}
