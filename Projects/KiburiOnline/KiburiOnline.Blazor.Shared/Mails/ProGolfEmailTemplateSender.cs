using AccomodationModule.Domain.DataTransferObjects;
using BeneficiariesModule.Domain.DataTransferObjects;
using ConectOne.Domain.Entities;
using ConectOne.Domain.Mailing.Entities;
using ConectOne.Domain.Mailing.Extensions;
using ConectOne.Domain.Mailing.Interfaces;
using ConectOne.Domain.Mailing.TemplateSender;
using ConectOne.Domain.ResultWrappers;
using Microsoft.Extensions.Configuration;
using NeuralTech.Base.Enums;

namespace KiburiOnline.Blazor.Shared.Mails
{
    /// <summary>
    /// Provides functionality to send various email templates using the IEmailSender.
    /// This class handles loading HTML templates, injecting data into them, and sending the resulting emails.
    /// </summary>
    public class ProGolfEmailTemplateSender : IEmailTemplateSender
    {
        private readonly IEmailSender _sender;
        private readonly ApplicationConfiguration? _aConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProGolfEmailTemplateSender"/> class.
        /// </summary>
        /// <param name="sender">The email sender used to send emails.</param>
        /// <param name="configuration">The application configuration, used to retrieve environment-specific settings.</param>
        public ProGolfEmailTemplateSender(IEmailSender sender, IConfiguration configuration)
        {
            _sender = sender;
            _aConfig = configuration.GetSection(nameof(ApplicationConfiguration)).Get<ApplicationConfiguration>();
        }

        /// <summary>
        /// Sends a "Contact Us" email using a predefined template.
        /// </summary>
        /// <param name="details">The email details such as recipients, subject line, etc.</param>
        /// <param name="name">The customer's name who is contacting.</param>
        /// <param name="email">The customer's email address.</param>
        /// <param name="title">The subject/title of the inquiry.</param>
        /// <param name="content">The inquiry message/content.</param>
        /// <returns>An IBaseResult indicating the success or failure of the sending operation.</returns>
        public async Task<IBaseResult> SendContactUsEmailAsync(EmailDetails details, string name, string cell,  string email, string title, string content)
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
        /// Sends a booking confirmation email with details about the lodging, room, and beneficiary.
        /// </summary>
        /// <param name="details">Email details including recipient, subject, etc.</param>
        /// <param name="booking">Booking data transfer object containing booking details.</param>
        /// <param name="lodging">Lodging data transfer object containing lodging details.</param>
        /// <param name="room">Room data transfer object containing room details.</param>
        /// <param name="beneficiary">Beneficiary data transfer object containing beneficiary details.</param>
        /// <param name="content">Additional content or booking details to insert into the template.</param>
        /// <param name="templateInfo">Contains assembly and template file info for loading the email template.</param>
        /// <returns>An IBaseResult indicating the success or failure of the operation.</returns>
        public async Task<IBaseResult> SendBookingConformationEmailAsync(
            EmailDetails details,
            BookingDto booking,
            LodgingDto lodging,
            RoomDto room,
            BeneficiaryDto beneficiary,
            string content,
            TemplateInfo templateInfo)
        {
            try
            {
                // Load the template text from the provided template info.
                var templateText = await MailingExtensions.LoadTemplateAsync(templateInfo.Assembly, templateInfo.TemplateFile);

                // Map template tokens to their replacements.
                var replacements = new Dictionary<string, string>
                {
                    { "--UserId--", booking.UserId },
                    { "--CustomerNameAndSurname--", booking.Name },
                    { "--BookingNr--", booking.OrderNr },
                    { "--LodgingName--", lodging.Name },
                    { "--CheckInTimeAndDate--", booking.StartDate.ToShortDateString() },
                    { "--CheckOutTimeAndDate--", booking.EndDate.ToShortDateString() },
                    { "--DurationOfStay--", booking.Nights.ToString() },
                    { "--GeustTotals--", $"Adults:{booking.Adults} Kids: {booking.Children}" },
                    { "--LodgingPhoneNr--", lodging.PhoneNr },
                    { "--LodgingEmail--", lodging.Email },
                    { "--LodgingAddress--", lodging.Address },
                    { "--ConformationCode--", booking.BookingReferenceNr },
                    { "--BookingDetails--", content },
                    { "--SubTotal--", booking.AmountDueExcl.ToString("C") },
                    { "--Vat--", "R0.00" },
                    { "--TotalDue--", booking.AmountDueExcl.ToString("C") },
                    { "--BeneficiaryInfo--", beneficiary.Name },
                    { "--ChildPolicy--", room.ChildPolicyDescription },
                    { "--CancellationPolicy--", lodging.CancellationPolicyDescription },
                    { "--TermsConditions--", lodging.TermsAndConditions }
                };

                // Replace template tokens.
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
        /// Sends a manual booking confirmation email with similar details as the standard booking confirmation,
        /// but possibly triggered manually by an admin or system user rather than an automated process.
        /// </summary>
        public async Task<IBaseResult> SendManualBookingConformationEmailAsync(
            EmailDetails details,
            BookingDto booking,
            LodgingDto lodging,
            RoomDto room,
            BeneficiaryDto beneficiary,
            string content,
            TemplateInfo templateInfo)
        {
            try
            {
                var templateText = await MailingExtensions.LoadTemplateAsync(templateInfo.Assembly!, templateInfo.TemplateFile);

                var replacements = new Dictionary<string, string>
                {
                    { "--UserId--", booking.UserId },
                    { "--CustomerNameAndSurname--", booking.Name },
                    { "--BookingNr--", booking.OrderNr },
                    { "--LodgingName--", lodging.Name },
                    { "--CheckInTimeAndDate--", booking.StartDate.ToShortDateString() },
                    { "--CheckOutTimeAndDate--", booking.EndDate.ToShortDateString() },
                    { "--DurationOfStay--", booking.Nights.ToString() },
                    { "--GeustTotals--", $"Adults:{booking.Adults} Kids: {booking.Children}" },
                    { "--LodgingPhoneNr--", lodging.PhoneNr },
                    { "--LodgingEmail--", lodging.Email },
                    { "--LodgingAddress--", lodging.Address },
                    { "--ConformationCode--", booking.BookingReferenceNr },
                    { "--BookingDetails--", content },
                    { "--SubTotal--", booking.AmountDueExcl.ToString("C") },
                    { "--Vat--", "R0.00" },
                    { "--TotalDue--", booking.AmountDueExcl.ToString("C") },
                    { "--BeneficiaryInfo--", beneficiary.Name },
                    { "--ChildPolicy--", room.ChildPolicyDescription },
                    { "--CancellationPolicy--", lodging.CancellationPolicyDescription },
                    { "--TermsConditions--", lodging.TermsAndConditions }
                };

                details.Content = MailingExtensions.ReplaceTokens(templateText, replacements);
                return await _sender.SendEmailAsync(details);
            }
            catch (Exception ex)
            {
                return await Result.FailAsync($"{nameof(EmailTemplateSender)} => {ex.Message}");
            }
        }

        /// <summary>
        /// Sends a manual booking confirmation email specifically intended for admin recipients,
        /// providing booking and lodging details without the full customer information.
        /// </summary>
        public async Task<IBaseResult> SendManualBookingConformationAdminEmailAsync(
            EmailDetails details,
            BookingDto booking,
            LodgingDto lodging,
            RoomDto room,
            TemplateInfo templateInfo)
        {
            try
            {
                var templateText = await MailingExtensions.LoadTemplateAsync(templateInfo.Assembly!, templateInfo.TemplateFile);

                var replacements = new Dictionary<string, string>
                {
                    { "--CustomerNameAndSurname--", booking.Name },
                    { "--EmailAddress--", booking.Email },
                    { "--TelephoneNr--", lodging.PhoneNr },
                    { "--LodgingName--", lodging.Name },
                    { "--RoomName--", room.Name },
                    { "--CheckInTimeAndDate--", booking.StartDate.ToShortDateString() },
                    { "--CheckOutTimeAndDate--", booking.EndDate.ToShortDateString() },
                    { "--GeustTotals--", $"Adults:{booking.Adults} Kids: {booking.Children}" },
                    { "--BedTypes--", string.Join(", ", room.BedTypes.Select(c => c.Description)) },
                    { "--ManualBookingNotes--", lodging.Email }
                };

                details.Content = MailingExtensions.ReplaceTokens(templateText, replacements);
                return await _sender.SendEmailAsync(details);
            }
            catch (Exception ex)
            {
                return await Result.FailAsync($"{nameof(EmailTemplateSender)} => {ex.Message}");
            }
        }

        /// <summary>
        /// Sends a booking cancellation email with details about the booking and lodging.
        /// </summary>
        /// <param name="details">Email details including recipient, subject, etc.</param>
        /// <param name="booking">Booking data transfer object containing booking details.</param>
        /// <param name="lodging">Lodging data transfer object containing lodging details.</param>
        /// <param name="beneficiary">Beneficiary data transfer object containing beneficiary details.</param>
        /// <returns>An IBaseResult indicating the success or failure of the operation.</returns>
        public async Task<IBaseResult> SendBookingCancellationEmailAsync(
            EmailDetails details,
            BookingDto booking,
            LodgingDto lodging,
            BeneficiaryDto beneficiary)
        {
            try
            {
                // The assembly where the template is embedded.
                var assembly = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assem => assem.GetName().Name == "ProGolf.Base");

                if (assembly is null)
                    throw new Exception("Unable to find the ProGolf.Base assembly.");

                // The resource file name of the embedded template.
                var templateFile = "ProGolf.Base.Mails.EmailTemplates.contactus.htm";
                // Load the template text from the provided template info.
                var templateText = await MailingExtensions.LoadTemplateAsync(assembly!, templateFile);

                // Map template tokens to their replacements.
                var replacements = new Dictionary<string, string>
                {
                    { "--UserId--", booking.UserId },
                    { "--CustomerNameAndSurname--", booking.Name },
                    { "--BookingNr--", booking.OrderNr },
                    { "--LodgingName--", lodging.Name },
                    { "--CancellationPolicy--", lodging.CancellationPolicyDescription }
                };

                // Replace template tokens.
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
        /// Sends a voucher confirmation email that provides details about the purchased voucher,
        /// related lodging, and room information.
        /// </summary>
        public async Task<IBaseResult> SendVoucherConformationEmailAsync(
            EmailDetails details,
            string userPhoneNr,
            UserVoucherDto voucher,
            LodgingDto lodging,
            RoomDto room,
            TemplateInfo templateInfo)
        {
            try
            {
                var templateText = await MailingExtensions.LoadTemplateAsync(templateInfo.Assembly!, templateInfo.TemplateFile);

                var replacements = new Dictionary<string, string>
                {
                    { "--CustomerNameAndSurname--", details.ToName },
                    { "--VoucherNr--", voucher.Voucher.VoucherId.ToString() },
                    { "--EmailAddress--", details.ToEmail },
                    { "--TelephoneNr--", userPhoneNr },
                    { "--LodgingName--", lodging.Name },
                    { "--RoomName--", room.Name },
                    { "--Meals--", string.Empty },
                    { "--VoucherName--", voucher.Voucher.Title },
                    { "--VoucherDescription--", voucher.Voucher.LongDescription },
                    { "--BeneficiaryInfo--", string.Empty },
                    { "--VoucherTerms--", voucher.Voucher.Terms },
                    { "--CancellationPolicy--", lodging.CancellationPolicyDescription },
                    { "--TermsConditions--", lodging.TermsAndConditions }
                };

                details.Content = MailingExtensions.ReplaceTokens(templateText, replacements);
                return await _sender.SendEmailAsync(details);
            }
            catch (Exception ex)
            {
                return await Result.FailAsync($"{nameof(EmailTemplateSender)} => {ex.Message}");
            }
        }

        /// <summary>
        /// Sends a general notification email. (Not implemented)
        /// </summary>
        public Task<IBaseResult> SendGeneralNotificationEmailAsync(EmailDetails details, string name, string title, string message, string buttonUrl = "#", TemplateInfo? templateInfo = null)
        {
            // Future implementation
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sends a general email with arbitrary content blocks. (Not implemented)
        /// </summary>
        public Task<IBaseResult> SendGeneralEmailAsync(EmailDetails details, string title, string content1, string content2, string buttonText = "", string buttonUrl = "#", TemplateInfo? templateInfo = null)
        {
            // Future implementation
            throw new NotImplementedException();
        }

        public async Task<IBaseResult> SendGeneralEmailAsync(EmailDetails details, string companyName, string title, string content1, string content2,
            string buttonText = "", string buttonUrl = "#", TemplateInfo? templateInfo = null)
        {
            try
            {
                // The assembly where the template is embedded.
                var assembly = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assem => assem.GetName().Name == "ProGolf.Base");

                if (assembly is null)
                    throw new Exception("Unable to find the ProGolf.Base assembly.");

                // The resource file name of the embedded template.
                var templateFile = "ProGolf.Base.Mails.EmailTemplates.contactus.htm";
                // Load the template text from the provided template info.
                var templateText = await MailingExtensions.LoadTemplateAsync(assembly!, templateFile);

                templateText = templateText
                    .Replace("--WebAddress--", "kwaggatravel.co.za")
                    .Replace("--CompanyName--", _aConfig?.AppliactionName ?? "YourCompany")
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

        public Task<IBaseResult> SendGeneralNotificationEmailAsync(EmailDetails details, string companyName, string logoUrl, string caption,
            string logoLink, string name, string title, string message, MessageType messageType, string documentLinks,
            string buttonUrl = "#", TemplateInfo? templateInfo = null)
        {
            throw new NotImplementedException();
        }

        public Task<IBaseResult> SendRegistrationEmailAsync(EmailDetails details, string companyName, string logoUrl, string caption,
            string logoLink, string name, string title, string message, string buttonUrl = "#",
            TemplateInfo? templateInfo = null)
        {
            throw new NotImplementedException();
        }

        public Task<IBaseResult> SendForgotPasswordEmailAsync(EmailDetails details, string logoUrl, string caption, string logoLink,
            string buttonUrl = "#", string companyName = "", TemplateInfo? templateInfo = null)
        {
            throw new NotImplementedException();
        }

        public Task<IBaseResult> SendBlogPostNotificationEmailAsync(EmailDetails details, string companyName, string logoUrl, string caption, string logoLink, string name, string title, string message, MessageType messageType, string category, string documentLinks, string buttonUrl = "#", TemplateInfo? templateInfo = null)
        {
            throw new NotImplementedException();
        }
    }
}
