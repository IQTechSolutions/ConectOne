using System.Text;
using ConectOne.Domain.Mailing.Entities;
using ConectOne.Domain.Mailing.Extensions;
using ConectOne.Domain.Mailing.Interfaces;
using ConectOne.Domain.Mailing.Services;
using ConectOne.Domain.ResultWrappers;
using Microsoft.Extensions.Configuration;
using NeuralTech.Base.Enums;

namespace ConectOne.Domain.Mailing.TemplateSender;

/// <summary>
/// Provides a template-driven approach for sending emails. It retrieves HTML templates 
/// (from embedded resources), replaces placeholders with dynamic values (e.g., user name, logos, links),
/// and then uses an <see cref="IEmailSender"/> to dispatch the final email. This promotes 
/// consistency in email styling across the application.
/// </summary>
public class EmailTemplateSender : IEmailTemplateSender
{
    private readonly IEmailSender _sender;
    private readonly EmailQueue _emailQueue;
    private readonly IConfiguration? _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailTemplateSender"/> class, 
    /// binding an email sender, a configuration, and an <see cref="EmailQueue"/>.
    /// </summary>
    /// <param name="sender">An <see cref="IEmailSender"/> for sending the final messages.</param>
    /// <param name="configuration">Used to fetch the <c>ApplicationConfiguration</c> section or other settings.</param>
    /// <param name="emailQueue">A queue for deferring the email-sending process if desired.</param>
    public EmailTemplateSender(IEmailSender sender, IConfiguration configuration, EmailQueue emailQueue)
    {
        _sender = sender;
        _emailQueue = emailQueue;
        _configuration = configuration;
    }

    /// <summary>
    /// Retrieves an email template from embedded resources. 
    /// If a <paramref name="templateInfo"/> is provided, it uses the specified assembly and template file.
    /// Otherwise, it defaults to the "NeuralTech.Base" assembly and "GeneralTemplate.htm" file.
    /// </summary>
    /// <param name="templateInfo">Optional parameter to specify a different assembly or template file.</param>
    /// <returns>A result containing the template text if successful, or an error message if the template is not found.</returns>
    public async Task<IBaseResult<string>> GetTemplate(TemplateInfo? templateInfo = null)
    {
        string templateText;

        var assembly = templateInfo is not null ? templateInfo.Assembly : AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assem => assem.GetName().Name == "NeuralTech.Base");
        var templateFile = templateInfo is not null ? templateInfo.TemplateFile : "NeuralTech.Base.Mailing.TemplateSender.Templates.GeneralTemplate.htm";

        var stream = assembly!.GetManifestResourceStream(templateFile);
        if (stream is null) return await Result<string>.FailAsync($"No Template file found");

        using (var reader = new StreamReader(stream, Encoding.UTF8))
        {
            templateText = await reader.ReadToEndAsync();
        }

        return await Result<string>.SuccessAsync(data: templateText);
    }

    /// <summary>
    /// Sends a 'Contact Us' email using the specified details and user-provided information.
    /// </summary>
    /// <remarks>The email content is generated from an embedded HTML template, with user-provided information
    /// inserted into predefined placeholders. If the template cannot be loaded or the email fails to send, the result
    /// will indicate failure with an appropriate error message.</remarks>
    /// <param name="details">The email details, including recipient, subject, and other metadata required to send the message. The content
    /// property will be populated with the formatted message body.</param>
    /// <param name="name">The full name of the user submitting the contact request. Used to personalize the email content.</param>
    /// <param name="cell">The user's phone number to include in the email message. Can be null or empty if not provided.</param>
    /// <param name="email">The email address of the user submitting the contact request. Used for reply-to information and included in the
    /// message body.</param>
    /// <param name="title">The subject line for the contact email. Describes the topic or reason for the inquiry.</param>
    /// <param name="content">The main message or inquiry submitted by the user. This text will be included in the body of the email.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an object indicating the success or
    /// failure of the email sending operation.</returns>
    public async Task<IBaseResult> SendContactUsEmailAsync(EmailDetails details, string name, string cell, string email, string title, string content)
    {
        try
        {
            // The assembly where the template is embedded.
            var assembly = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assem => assem.GetName().Name == "ProGolf.Base");

            if (assembly is null)
                throw new Exception("Unable to find the ProGolf.Blazor.SiteClient assembly.");

            // The resource file name of the embedded template.
            var templateFile = "ProGolf.Base.Mails.EmailTemplates.contactus.htm";

            // Load the template as a string.
            var templateText = await MailingExtensions.LoadTemplateAsync(assembly, templateFile);

            // Define tokens and their replacements in the template.
            var replacements = new Dictionary<string, string>
                {
                    { "--CustomerNameAndSurname--", name },
                    { "--CustomerEmail--", email },
                    { "--Subject--", title },
                    { "--Message--", content },
                    { "--PhoneNr--", cell}
                };

            // Replace tokens in the template with actual values.
            details.Content = MailingExtensions.ReplaceTokens(templateText, replacements);

            // Send the email.
            return await _sender.SendEmailAsync(details);
        }
        catch (Exception ex)
        {
            return await Result.FailAsync($"{nameof(EmailTemplateSender)} => {ex.Message}");
        }
    }

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
    public async Task<IBaseResult> SendContactUsAdminEmailAsync(EmailDetails details, string name, string surname, string cell, string email, string title, string content)
    {
        try
        {
            // The assembly where the template is embedded.
            var assembly = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assem => assem.GetName().Name == "ProGolf.Base");

            if (assembly is null)
                throw new Exception("Unable to find the ProGolf.Blazor.SiteClient assembly.");

            // The resource file name of the embedded template.
            var templateFile = "ProGolf.Base.Mails.EmailTemplates.contactusadmin.htm";

            // Load the template as a string.
            var templateText = await MailingExtensions.LoadTemplateAsync(assembly, templateFile);

            // Define tokens and their replacements in the template.
            var replacements = new Dictionary<string, string>
                {
                    { "--CustomerName--", name },
                    { "--CustomerSurname--", surname },
                    { "--CustomerEmail--", email },
                    { "--Subject--", title },
                    { "--Message--", content },
                    { "--PhoneNr--", cell}
                };

            // Replace tokens in the template with actual values.
            details.Content = MailingExtensions.ReplaceTokens(templateText, replacements);

            // Send the email.
            return await _sender.SendEmailAsync(details);
        }
        catch (Exception ex)
        {
            return await Result.FailAsync($"{nameof(EmailTemplateSender)} => {ex.Message}");
        }
    }

    /// <summary>
    /// Sends a generic HTML email by substituting placeholders 
    /// (e.g., <c>--Title--</c>, <c>--Content1--</c>) in a default or custom template file.
    /// </summary>
    /// <param name="details">Holds basic email info (From, To, Subject, etc.).</param>
    /// <param name="companyName">Used in the email body to brand the message.</param>
    /// <param name="title">The primary heading displayed inside the template.</param>
    /// <param name="content1">A first block of text shown in the template.</param>
    /// <param name="content2">A second block of text shown in the template.</param>
    /// <param name="buttonText">Optional text for a call-to-action button.</param>
    /// <param name="buttonUrl">Where the button should link. Defaults to "#".</param>
    /// <param name="templateInfo">Allows specifying a different assembly or resource name for the HTML template.</param>
    /// <returns>A result indicating success/failure and any relevant messages.</returns>
    public async Task<IBaseResult> SendGeneralEmailAsync(EmailDetails details, string companyName, string title, string content1, string content2,
        string buttonText = "", string buttonUrl = "#", TemplateInfo? templateInfo = null)
    {
        try
        {
            var templateResult = await GetTemplate(templateInfo);
            if (!templateResult.Succeeded) return await Result.FailAsync(templateResult.Messages);

            var templateText = templateResult.Data;

            templateText = templateText
                .Replace("--WebAddress--", "kwaggatravel.co.za")
                .Replace("--CompanyName--", _configuration["ApplicationConfiguration:AppliactionName"] ?? "YourCompany")
                .Replace("--Title--", title)
                .Replace("--Content1--", content1)
                .Replace("--Content2--", content2)
                .Replace("--ButtonText--", buttonText)
                .Replace("--ButtonUrl--", buttonUrl);

            details.Content = templateText;

            return await _sender.SendEmailAsync(details);
        }
        catch (Exception ex)
        {
            return await Result.FailAsync($"{nameof(EmailTemplateSender)} => {ex.Message}");
        }
    }

    /// <summary>
    /// Sends a “general notification” email with branding elements like logos, 
    /// and placeholders for the recipient’s name, a custom message, and other bits.
    /// </summary>
    /// <param name="details">Basic sender/recipient info for the email.</param>
    /// <param name="companyName">Shown in the email for branding.</param>
    /// <param name="logoUrl">The URL or path for the top image/branding logo.</param>
    /// <param name="caption">A short tagline or descriptive text displayed in the header.</param>
    /// <param name="logoLink">If the logo is clickable, this link is the target.</param>
    /// <param name="name">The recipient’s display name for greeting (e.g., “John Doe”).</param>
    /// <param name="title">A heading or subject line for the email body.</param>
    /// <param name="message">The main textual content, typically displayed under the title.</param>
    /// <param name="messageType">Represents the type of notification (e.g., Global, Learner, etc.).</param>
    /// <param name="documentLinks"></param>
    /// <param name="buttonUrl">An optional call-to-action link, defaults to "#".</param>
    /// <param name="templateInfo">Optionally defines a different embedded resource or assembly for the template.</param>
    /// <returns>A result indicating success or failure, with any error messages if it fails.</returns>
    public async Task<IBaseResult> SendGeneralNotificationEmailAsync(EmailDetails details, string companyName, string logoUrl, string caption, string logoLink, string name,
        string title, string message, MessageType messageType, string documentLinks, string buttonUrl = "#", TemplateInfo? templateInfo = null)
    {
        try
        {
            var templateResult = await GetTemplate(templateInfo);
            if (!templateResult.Succeeded) return await Result.FailAsync(templateResult.Messages);

            var templateText = templateResult.Data;

            templateText = templateText
                .Replace("--Logo--", logoUrl)
                .Replace("--Caption--", caption)
                .Replace("--LogoLink--", logoLink)
                .Replace("--CompanyName--", companyName)
                .Replace("--CustomerNameAndSurname--", name)
                .Replace("--MessageTitle--", title)
                .Replace("--Message--", message)
                .Replace("--MessageType--", messageType.ToString())
                .Replace("--DocumentLinks--", documentLinks)
                .Replace("--ButtonUrl--", buttonUrl);

            // Queue the email for sending
            details.Content = templateText;
            _emailQueue.Enqueue(details);

            return await Result.SuccessAsync("Email Queued successfully");
        }
        catch (Exception ex)
        {
            return await Result.FailAsync($"{nameof(EmailTemplateSender)} => {ex.Message}");
        }
    }

    public async Task<IBaseResult> SendBlogPostNotificationEmailAsync(EmailDetails details, string companyName, string logoUrl, string caption, string logoLink, string name,
        string title, string message, MessageType messageType, string category, string documentLinks, string buttonUrl = "#", TemplateInfo? templateInfo = null)
    {
        try
        {
            var templateResult = await GetTemplate(templateInfo);
            if (!templateResult.Succeeded) return await Result.FailAsync(templateResult.Messages);

            var templateText = templateResult.Data;

            templateText = templateText
                .Replace("--Logo--", logoUrl)
                .Replace("--Caption--", caption)
                .Replace("--LogoLink--", logoLink)
                .Replace("--CompanyName--", companyName)
                .Replace("--CustomerNameAndSurname--", name)
                .Replace("--MessageTitle--", title)
                .Replace("--Message--", message)
                .Replace("--MessageType--", messageType.ToString())
                .Replace("--Category--", category)
                .Replace("--DocumentLinks--", documentLinks)
                .Replace("--ButtonUrl--", buttonUrl);

            // Queue the email for sending
            details.Content = templateText;
            _emailQueue.Enqueue(details);

            return await Result.SuccessAsync("Email Queued successfully");
        }
        catch (Exception ex)
        {
            return await Result.FailAsync($"{nameof(EmailTemplateSender)} => {ex.Message}");
        }
    }

    /// <summary>
    /// Sends a registration email, typically used post-signup or to finalize an account creation step,
    /// by filling placeholders in a dedicated HTML template with branding, welcome text, and a CTA link.
    /// </summary>
    public async Task<IBaseResult> SendRegistrationEmailAsync(EmailDetails details, string companyName, string logoUrl, string caption, string logoLink, string name,
        string title, string message, string buttonUrl = "#", TemplateInfo? templateInfo = null)
    {
        try
        {
            var templateResult = await GetTemplate(templateInfo);
            if (!templateResult.Succeeded) return await Result.FailAsync(templateResult.Messages);

            var templateText = templateResult.Data;

            templateText = templateText
                .Replace("--Logo--", logoUrl)
                .Replace("--Caption--", caption)
                .Replace("--LogoLink--", logoLink)
                .Replace("--CompanyName--", companyName)
                .Replace("--CustomerNameAndSurname--", name)
                .Replace("--MessageTitle--", title)
                .Replace("--Message--", message)
                .Replace("--ButtonUrl--", buttonUrl);

            details.Content = templateText;
            _emailQueue.Enqueue(details);

            return await Result.SuccessAsync("Email Queued successfully");
        }
        catch (Exception ex)
        {
            return await Result.FailAsync($"{nameof(EmailTemplateSender)} => {ex.Message}");
        }
    }

    /// <summary>
    /// Dispatches a “forgot password” email, typically including a link leading to 
    /// a reset form. Uses placeholders (logo, caption, link) in the HTML template to 
    /// produce a consistent, branded password reset message.
    /// </summary>
    public async Task<IBaseResult> SendForgotPasswordEmailAsync(EmailDetails details, string logoUrl, string caption, string logoLink, string buttonUrl = "#", string conpanyName = "", TemplateInfo? templateInfo = null)
    {
        try
        {
            var templateResult = await GetTemplate(templateInfo);
            if (!templateResult.Succeeded) return await Result.FailAsync(templateResult.Messages);

            var templateText = templateResult.Data;
            templateText = templateText
                .Replace("--CompanyName--", conpanyName)
                .Replace("--Logo--", logoUrl)
                .Replace("--Caption--", caption)
                .Replace("--LogoLink--", logoLink)
                .Replace("--ButtonUrl--", buttonUrl);

            details.Content = templateText;
            _emailQueue.Enqueue(details);

            return await Result.SuccessAsync("Email Queued successfully");
        }
        catch (Exception ex)
        {
            return await Result.FailAsync($"{nameof(EmailTemplateSender)} => {ex.Message}");
        }
    }

    /// <summary>
    /// Sends a booking confirmation email by loading a specialized template 
    /// and performing (partial) placeholder replacements (e.g., booking number, user ID).
    /// In a real-world scenario, you’d fill in all relevant placeholders for a complete 
    /// confirmation email. The template path is provided via <paramref name="templateInfo"/>.
    /// </summary>
    public async Task<IBaseResult> SendBookingConfirmationEmailAsync(EmailDetails details, string userId, string customerName, string bookingNr, TemplateInfo templateInfo)
    {
        try
        {
            var templateResult = await GetTemplate(templateInfo);
            if (!templateResult.Succeeded) return await Result.FailAsync(templateResult.Messages);

            var templateText = templateResult.Data;

            // templateText = templateText
            //     .Replace("--UserId--", userId)
            //     .Replace("--CustomerNameAndSurname--", customerName)
            //     .Replace("--BookingNr--", bookingNr);

            details.Content = templateText;
            return await _sender.SendEmailAsync(details);
        }
        catch (Exception ex)
        {
            return await Result.FailAsync($"{nameof(EmailTemplateSender)} => {ex.Message}");
        }
    }
}
