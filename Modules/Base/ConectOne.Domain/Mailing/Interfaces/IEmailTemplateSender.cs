using ConectOne.Domain.Mailing.Entities;
using ConectOne.Domain.Mailing.TemplateSender;
using ConectOne.Domain.ResultWrappers;
using NeuralTech.Base.Enums;

namespace ConectOne.Domain.Mailing.Interfaces
{
    /// <summary>
    /// The <see cref="EmailTemplateSender"/> class provides functionality 
    /// to send various styled or templated emails by leveraging an 
    /// underlying <see cref="IEmailSender"/>. It loads email templates 
    /// (in HTML form) from embedded resources, inserts dynamic values 
    /// into placeholders, and dispatches the final rendered content.
    /// 
    /// Example use cases:
    /// <list type="bullet">
    ///     <item><description>Sending a general, two-section text email with a single button link.</description></item>
    ///     <item><description>Sending a more specialized "notification" email with branding, logo, and user greeting.</description></item>
    ///     <item><description>Sending a booking confirmation email (although not fully implemented in detail here).</description></item>
    /// </list>
    /// This separation of concerns (template loading, placeholder substitution, and final email sending) 
    /// helps maintain consistency across different email styles in the application.
    /// </summary>
    /// <remarks>
    /// Most methods accept a <see cref="TemplateInfo"/> object optionally, which can override 
    /// default embedded resource locations or assemblies for reading the template. 
    /// </remarks>
    public interface IEmailTemplateSender
    {
        /// <summary>
        /// Sends a general-purpose email using the specified details and content parameters asynchronously.
        /// </summary>
        /// <param name="details">The email recipient and sender details, including addresses and any additional metadata required to
        /// construct the email.</param>
        /// <param name="companyName">The name of the company to display in the email content. This is typically used for branding or
        /// personalization.</param>
        /// <param name="title">The subject or main heading of the email message.</param>
        /// <param name="content1">The primary body content of the email. This text appears as the main message to the recipient.</param>
        /// <param name="content2">Additional body content to include in the email, such as a secondary message or footer.</param>
        /// <param name="buttonText">The text to display on the call-to-action button in the email. If not specified, the button may be omitted.</param>
        /// <param name="buttonUrl">The URL to which the call-to-action button should link. Defaults to "#" if not specified.</param>
        /// <param name="templateInfo">Optional template information to customize the appearance or layout of the email. If null, a default
        /// template is used.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an object indicating the outcome
        /// of the email send operation.</returns>
        Task<IBaseResult> SendGeneralEmailAsync(EmailDetails details, string companyName, string title, string content1, string content2, string buttonText = "", string buttonUrl = "#", TemplateInfo? templateInfo = null);

        /// <summary>
        /// Sends a contact us email asynchronously using the specified sender and message details.
        /// </summary>
        /// <param name="details">An object containing the email configuration and delivery details. Cannot be null.</param>
        /// <param name="name">The name of the person submitting the contact request. Cannot be null or empty.</param>
        /// <param name="cell">The cell phone number of the person submitting the contact request. May be null or empty if not provided.</param>
        /// <param name="email">The email address of the person submitting the contact request. Cannot be null or empty.</param>
        /// <param name="title">The subject or title of the contact request. Cannot be null or empty.</param>
        /// <param name="content">The body content of the contact request message. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an object indicating the outcome
        /// of the email send operation.</returns>
        Task<IBaseResult> SendContactUsEmailAsync(EmailDetails details, string name, string cell, string email, string title, string content);

        /// <summary>
        /// Sends a "Contact Us" email to the designated recipient using the provided details and content.
        /// </summary>
        /// <remarks>This method uses an embedded email template to format the content of the email. Tokens in the
        /// template  are replaced with the provided customer details and message content before sending. If the template 
        /// cannot be loaded or an error occurs during email sending, the method returns a failure result.</remarks>
        /// <param name="details">The email details, including the recipient, subject, and other metadata.</param>
        /// <param name="name">The first name of the customer submitting the inquiry.</param>
        /// <param name="surname">The last name of the customer submitting the inquiry.</param>
        /// <param name="cell">The customer's phone number.</param>
        /// <param name="email">The customer's email address.</param>
        /// <param name="title">The subject or title of the inquiry.</param>
        /// <param name="content">The message content of the inquiry.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the success or failure of the email sending operation.</returns>
        Task<IBaseResult> SendContactUsAdminEmailAsync(EmailDetails details, string name, string surname, string cell, string email, string title, string content);

        /// <summary>
        /// Sends a general notification email to the specified recipient using the provided details and formatting
        /// options.
        /// </summary>
        /// <param name="details">The email details, including recipient information and any additional metadata required to send the email.
        /// Cannot be null.</param>
        /// <param name="companyName">The name of the company to display in the email content and header.</param>
        /// <param name="logoUrl">The URL of the company logo to include in the email. Should be a valid image URL or left empty if no logo is
        /// to be displayed.</param>
        /// <param name="caption">The caption text to display prominently in the email, typically as a heading or subheading.</param>
        /// <param name="logoLink">The URL to which the logo image should link when clicked. Should be a valid URL or left empty if no link is
        /// desired.</param>
        /// <param name="name">The name of the recipient or the person to address in the email greeting.</param>
        /// <param name="title">The title or subject line to display within the email body.</param>
        /// <param name="message">The main message content of the email. Supports plain text or HTML formatting as required by the template.</param>
        /// <param name="messageType">The type of message being sent, which determines the visual style or icon used in the email. Must be a valid
        /// value of the MessageType enumeration.</param>
        /// <param name="documentLinks">A string containing one or more document links to include in the email. May be empty if no documents are to
        /// be referenced.</param>
        /// <param name="buttonUrl">The URL to use for the primary action button in the email. Defaults to "#" if not specified.</param>
        /// <param name="templateInfo">Optional template information to override default email formatting or layout. If null, the default template
        /// is used.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an IBaseResult indicating the
        /// outcome of the email send operation.</returns>
        Task<IBaseResult> SendGeneralNotificationEmailAsync(EmailDetails details, string companyName, string logoUrl, string caption, string logoLink, string name,
            string title, string message, MessageType messageType, string documentLinks, string buttonUrl = "#", TemplateInfo? templateInfo = null);

        /// <summary>
        /// Sends an email notification about a new blog post to the specified recipients using the provided email
        /// details and template information.
        /// </summary>
        /// <remarks>This method is asynchronous and does not block the calling thread. Ensure that all
        /// required parameters are provided and valid to avoid failures in email delivery.</remarks>
        /// <param name="details">The email details, including recipient information and any additional email metadata. Cannot be null.</param>
        /// <param name="companyName">The name of the company to display in the email content.</param>
        /// <param name="logoUrl">The URL of the company logo to include in the email. Should be a valid image URL.</param>
        /// <param name="caption">The caption text to display in the email, typically shown near the logo or header.</param>
        /// <param name="logoLink">The URL to which the logo should link when clicked in the email.</param>
        /// <param name="name">The name of the person or entity associated with the blog post, such as the author.</param>
        /// <param name="title">The title of the blog post to include in the email notification.</param>
        /// <param name="message">The main message or summary content of the blog post to display in the email body.</param>
        /// <param name="messageType">The type of message being sent, which determines the email's visual style or emphasis.</param>
        /// <param name="category">The category of the blog post, used for organizing or filtering notifications.</param>
        /// <param name="documentLinks">A string containing one or more document links related to the blog post, formatted as required by the email
        /// template.</param>
        /// <param name="buttonUrl">The URL to use for the primary call-to-action button in the email. Defaults to "#" if not specified.</param>
        /// <param name="templateInfo">Optional template information to customize the appearance or layout of the email. If null, a default
        /// template is used.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an object indicating the outcome
        /// of the email send operation.</returns>
        Task<IBaseResult> SendBlogPostNotificationEmailAsync(EmailDetails details, string companyName, string logoUrl, string caption, string logoLink, string name,
            string title, string message, MessageType messageType, string category, string documentLinks, string buttonUrl = "#", TemplateInfo? templateInfo = null);

        /// <summary>
        /// Sends a registration email, typically used post-signup or to finalize an account creation step,
        /// by filling placeholders in a dedicated HTML template with branding, welcome text, and a CTA link.
        /// </summary>
        Task<IBaseResult> SendRegistrationEmailAsync(EmailDetails details, string companyName, string logoUrl, string caption, string logoLink, string name, 
            string title, string message, string buttonUrl = "#", TemplateInfo? templateInfo = null);

        /// <summary>
        /// Dispatches a “forgot password” email, typically including a link leading to 
        /// a reset form. Uses placeholders (logo, caption, link) in the HTML template to 
        /// produce a consistent, branded password reset message.
        /// </summary>
        Task<IBaseResult> SendForgotPasswordEmailAsync(EmailDetails details, string logoUrl, string caption, string logoLink, string buttonUrl = "#", string companyName = "", TemplateInfo? templateInfo = null);

    }
}
