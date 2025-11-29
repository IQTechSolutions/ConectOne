using ConectOne.Domain.Mailing;
using ConectOne.Domain.Mailing.Entities;
using ConectOne.Domain.Mailing.Interfaces;
using ConectOne.Domain.Mailing.TemplateSender;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.DataTransferObjects;
using Microsoft.Extensions.Configuration;
using NeuralTech.Base.Enums;

namespace SchoolsEnterprise.Base.Mails
{
    /// <summary>
    /// Extends the <see cref="DefaultEmailSender"/> to provide specialized email-sending 
    /// methods for the Schools Enterprise application, such as sending notifications, 
    /// registration emails, and forgot-password emails. Methods rely on template-driven 
    /// emails via an <see cref="IEmailTemplateSender"/> and configurable data from <see cref="IConfiguration"/>.
    /// </summary>
    public class SchoolsEnterpriseEmailSender(IEmailTemplateSender emailTemplateSender, IConfiguration configuration) : DefaultEmailSender(emailTemplateSender, configuration)
    {
        /// <summary>
        /// Sends a general notification email (e.g., a custom message) using a 
        /// template-based system. The subject defaults to:
        /// "Notification from {ApplicationName} Primary School"
        /// </summary>
        /// <param name="toName">Name of the recipient (e.g., "John Doe").</param>
        /// <param name="toEmail">Recipient's email address.</param>
        /// <param name="title">The headline or title displayed in the email content.</param>
        /// <param name="message">The main body or content of the email.</param>
        /// <param name="messageType">Type of the message (e.g., <see cref="MessageType.Global"/>) for usage in the template.</param>
        /// <param name="buttonUrl">An optional URL for a button or link in the email template.</param>
        /// <param name="logoUrl">A URL or path to the logo image displayed in the email.</param>
        /// <param name="caption">An additional piece of text or tagline in the email (like "Eversdal Primary School").</param>
        /// <param name="logoLink">The URL to open when clicking the logo.</param>
        /// <returns>An <see cref="IBaseResult"/> indicating success or failure.</returns>
        public async Task<IBaseResult> SendGeneralNotificationEmailAsync(string toName, string toEmail, string title, string message, MessageType messageType, string buttonUrl, 
            string logoUrl, string caption, string logoLink, List<string> documentLinks = null, List<DocumentDto> documents = null)
        {
            // Construct the email details, including subject, recipient name/email, etc.
            var emailDetails = new EmailDetails(toName, toEmail, $"Notification from {configuration["ApplicationConfiguration:AppliactionName"]} Primary School",
                $"{configuration["ApplicationConfiguration:AppliactionName"]} Primary School Administration",
                $"{configuration["ApplicationConfiguration:DoNotReplyEmailAddress"]}"
            );

            var documentLinkString = string.Empty;


            if (documents != null && documents.Any() || documentLinks != null && documentLinks.Any())
            {

                documentLinkString = "<tr>" +
                                     "<td height=\"20\" class=\"hide\"></td>" +
                                     "</tr>" +
                                     "<tr>" +
                                     "<td align=\"left\" class=\"MsoNormal resp-content\" style=\"color:#333333;font-family:'Segoe UI',sans-serif,Arial,Helvetica,Lato;font-size:13px;font-weight:600;letter-spacing:1px;line-height:26px;text-transform:capitalize;\">" +
                                     "Documents and Information Links:" +
                                     "</td>" +
                                     "</tr>" +
                                     "<tr>" +
                                     "<td height=\"20\"></td>" +
                                     "</tr>" +
                                     "<tr>" +
                                     "<td align=\"left\">" +
                                     "<table align=\"left\" style=\"width:100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:auto; border-collapse:collapse; mso-table-lspace:0pt; mso-table-rspace:0pt;\">";

                if (documentLinks != null && documentLinks.Any())
                {
                    foreach (var linkItem in documentLinks)
                    {
                        documentLinkString = documentLinkString + "<tr>" +
                                             "<td align=\"center\">" +
                                             "<table align=\"left\" class=\"display-width-list\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse:collapse; mso-table-lspace:0pt; mso-table-rspace:0pt;\">" +
                                             "<tr>" +
                                             "<td align=\"left\">" +
                                             "<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"70%\" style=\"width:auto !important;\">" +
                                             "<tr>" +
                                             "<td align=\"left\" width=\"25\">" +
                                             "<img src=\"https://totius.connect-one.co.za/paperclip.png\" alt=\"12x12\" width=\"15\" />" +
                                             "</td>" +
                                             "<td align=\"left\" class=\"MsoNormal\" style=\"color:#666666;font-family:'Segoe UI',sans-serif,Arial,Helvetica,Lato;font-size:12px;font-weight:600;letter-spacing:1px;line-height:20px;\">" +
                                             $"<a href=\"{linkItem}\">{linkItem}</a>" +
                                             "</td>" +
                                             "</tr>" +
                                             "</table>" +
                                             "</td>" +
                                             "</tr>" +
                                             "<tr>" +
                                             "<td height=\"10\"></td>" +
                                             "</tr>";


                    }
                }

                if (documents != null && documents.Any())
                {
                    foreach (var linkItem in documents)
                    {
                        documentLinkString = documentLinkString + "<tr>" +
                                             "<td align=\"center\">" +
                                             "<table align=\"left\" class=\"display-width-list\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse:collapse; mso-table-lspace:0pt; mso-table-rspace:0pt;\">" +
                                             "<tr>" +
                                             "<td align=\"left\">" +
                                             "<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"70%\" style=\"width:auto !important;\">" +
                                             "<tr>" +
                                             "<td align=\"left\" width=\"25\">" +
                                             "<img src=\"https://totius.connect-one.co.za/paperclip.png\" alt=\"12x12\" width=\"15\" />" +
                                             "</td>" +
                                             "<td align=\"left\" class=\"MsoNormal\" style=\"color:#666666;font-family:'Segoe UI',sans-serif,Arial,Helvetica,Lato;font-size:12px;font-weight:600;letter-spacing:1px;line-height:20px;\">" +
                                             $"<a href=\"{configuration["ApiConfiguration:BaseApiAddress"]}/StaticFiles/messages/images/{linkItem.FileName}\">{linkItem.FileName}</a>" +
                                             "</td>" +
                                             "</tr>" +
                                             "</table>" +
                                             "</td>" +
                                             "</tr>" +
                                             "<tr>" +
                                             "<td height=\"10\"></td>" +
                                             "</tr>";


                    }
                }

                documentLinkString = documentLinkString + "</table>" +
                                     "</td>" +
                                     "</tr>" +
                                     "</table>" +
                                     "</td>" +
                                     "</tr>";

            }

            // Leverage the template-sending method. Additional parameters 
            // (logoUrl, caption, messageType, etc.) are passed to customize the template.
            var emailResult = await emailTemplateSender.SendGeneralNotificationEmailAsync(
                emailDetails,
                configuration["ApplicationConfiguration:AppliactionName"],
                logoUrl,
                caption,
                logoLink,
                toName,
                title,
                message,
                messageType,
                documentLinkString,
                "https://totius.connect-one.co.za", // placeholder or relevant domain
                new TemplateInfo(
                    AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assem => assem.GetName().Name == "SchoolsEnterprise.Base"),
                    "SchoolsEnterprise.Base.Mails.EmailTemplates.notificationEmail.htm"
                )
            );

            // If sending the email fails, return a fail result with the messages
            if (!emailResult.Succeeded)
                return await Result.FailAsync(emailResult.Messages);

            // Otherwise, return success
            return await Result.SuccessAsync("Notification email successfully sent");
        }

        /// <summary>
        /// Sends a registration-related email, typically sent to a user upon new registration or 
        /// account activation. Utilizes a dedicated "registrationEmail.htm" template.
        /// </summary>
        /// <param name="toName">The recipient's name.</param>
        /// <param name="toEmail">The recipient's email.</param>
        /// <param name="title">The title displayed in the email content (not necessarily the subject).</param>
        /// <param name="message">Main body content of the email.</param>
        /// <param name="buttonUrl">An optional URL for a button/link. Often used for "Confirm Registration."</param>
        /// <param name="logoUrl">URL or path for the school or app logo image.</param>
        /// <param name="caption">A tagline or short text displayed under the logo.</param>
        /// <param name="logoLink">A link to open when the logo is clicked.</param>
        /// <returns>An <see cref="IBaseResult"/> with success or failure status.</returns>
        public async Task<IBaseResult> SendRegistrationEmailAsync(string toName, string toEmail, string title, string message, string buttonUrl, string logoUrl, string caption, string logoLink)
        {
            // Construct the email details using the from address, subject, etc.
            var emailDetails = new EmailDetails(
                toName,
                toEmail,
                $"Notification from {configuration["ApplicationConfiguration:AppliactionName"]} Primary School",
                $"{configuration["ApplicationConfiguration:AppliactionName"]} Primary School Administration",
                $"{configuration["ApplicationConfiguration:DoNotReplyEmailAddress"]}"
            );

            // Use SendRegistrationEmailAsync to handle the specialized template.
            var emailResult = await emailTemplateSender.SendRegistrationEmailAsync(
                emailDetails,
                configuration["ApplicationConfiguration:AppliactionName"],
                logoUrl,
                caption,
                logoLink,
                toName,
                title,
                message,
                buttonUrl,
                new TemplateInfo(AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assem => assem.GetName().Name == "SchoolsEnterprise.Base"),
                    "SchoolsEnterprise.Base.Mails.EmailTemplates.registrationEmail.htm"
                )
            );

            if (!emailResult.Succeeded)
                return await Result.FailAsync(emailResult.Messages);

            return await Result.SuccessAsync("Registration email successfully sent");
        }

        /// <summary>
        /// Sends a "forgot password" email, which usually includes a link for the user 
        /// to reset their password. Uses a dedicated "forgotPassword.htm" template.
        /// </summary>
        /// <param name="toEmail">The user’s email address to send the reset link.</param>
        /// <param name="buttonUrl">The link for resetting the password (usually includes a token).</param>
        /// <param name="logoUrl">URL for the school/app logo displayed in the email.</param>
        /// <param name="caption">A tagline or short descriptive text included in the email’s header.</param>
        /// <param name="logoLink">A link to open when the logo is clicked.</param>
        /// <returns>An <see cref="IBaseResult"/> indicating if the email was sent successfully.</returns>
        public async Task<IBaseResult> SendForgotPasswordEmailAsync(string toEmail, string buttonUrl, string logoUrl, string caption, string logoLink)
        {
            // Prepare the email details (recipient name is set same as email if unknown).
            var emailDetails = new EmailDetails(
                toEmail,
                toEmail,
                $"Forgot Notification from ",
                $"{configuration["ApplicationConfiguration:AppliactionName"]} {configuration["ApplicationConfiguration:SchoolTypeNameLeft"]} Administration",
                $"{configuration["ApplicationConfiguration:DoNotReplyEmailAddress"]}"
            );

            // Use the specialized forgotPassword.htm template for building the email.
            var emailResult = await emailTemplateSender.SendForgotPasswordEmailAsync(
                emailDetails,
                logoUrl,
                caption,
                logoLink,
                buttonUrl,
                $" {configuration["ApplicationConfiguration:AppliactionName"]} {configuration["ApplicationConfiguration:SchoolTypeNameLeft"]}",
                new TemplateInfo(
                    AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assem => assem.GetName().Name == "SchoolsEnterprise.Base"),
                    "SchoolsEnterprise.Base.Mails.EmailTemplates.forgotPassword.htm"
                )
            );

            if (!emailResult.Succeeded)
                return await Result.FailAsync(emailResult.Messages);

            return await Result.SuccessAsync("Forgot Password Email successfully sent");
        }
    }
}
