using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using BeneficiariesModule.Domain.DataTransferObjects;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.Mailing;
using ConectOne.Domain.Mailing.Entities;
using ConectOne.Domain.Mailing.TemplateSender;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.Enums;
using Microsoft.Extensions.Configuration;

namespace KiburiOnline.Blazor.Shared.Mails
{
    /// <summary>
    /// Handles the sending of various Kwagga-related emails by leveraging the KwaggaEmailTemplateSender and various providers.
    /// </summary>
    public class ProGolfEmailSender : DefaultEmailSender
    {
        private readonly ProGolfEmailTemplateSender _emailTemplateSender;
        private readonly IConfiguration _configuration;
        private readonly ILodgingService _provider;
        private readonly IRoomDataService _roomDataService;
        private readonly IVoucherService _voucherService;

        /// <summary>
        /// Initializes a new instance of the <see cref="KwaggaEmailSender"/> class.
        /// </summary>
        /// <param name="emailTemplateSender">The template sender for formatting emails.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="provider">Provides information from the API.</param>
        public ProGolfEmailSender(ProGolfEmailTemplateSender emailTemplateSender, IConfiguration configuration, ILodgingService provider, IRoomDataService roomDataService, IVoucherService voucherService) : base(emailTemplateSender, configuration)
        {
            _emailTemplateSender = emailTemplateSender;
            _configuration = configuration;
            _provider = provider;
            _roomDataService = roomDataService;
            _voucherService = voucherService;
        }

        /// <summary>
        /// Sends a support ticket email (Contact Us) to the given recipient.
        /// </summary>
        /// <param name="toName">Name of the recipient.</param>
        /// <param name="toEmail">Email address of the recipient.</param>
        /// <param name="fromName">Name of the sender (company or support team name).</param>
        /// <param name="fromEmail">Email address of the sender (company or support team email).</param>
        /// <param name="title">The subject/title of the support ticket.</param>
        /// <param name="message">The message body content of the ticket.</param>
        /// <param name="displayName">Name of the person submitting the support request.</param>
        /// <param name="displayEmail">Email of the person submitting the support request.</param>
        /// <returns>A task that represents the asynchronous send operation result.</returns>
        public async Task<IBaseResult> SendSupportTicketEmailAsync(string toName, string toEmail, string fromName, string fromEmail, string cell, string title, string message, string? emailSubjectLine = null)
        {
            return await _emailTemplateSender.SendContactUsEmailAsync(
                new EmailDetails(toName, toEmail, string.IsNullOrEmpty(emailSubjectLine) ?$"Thank You for submitting a support request to {fromName}" : emailSubjectLine, $"{fromName} Administration", fromEmail),
                toName, cell, toEmail, title, message);
        }

        /// <summary>
        /// Sends a booking confirmation email to the given recipient for all bookings contained in an order.
        /// </summary>
        /// <param name="orderDto">Order data transfer object containing booking information.</param>
        /// <param name="toName">Name of the recipient.</param>
        /// <param name="toEmail">Recipient's email.</param>
        /// <param name="fromName">Name used in the email "From" display.</param>
        /// <param name="fromEmail">Actual from email address.</param>
        /// <param name="callbackUrl">A callback URL (not currently used, can be passed as "#").</param>
        /// <returns>A task that represents the asynchronous send operation result.</returns>
        public async Task<IBaseResult> SendBookingConformationEmailAsync(OrderDto orderDto, string? toName, string toEmail, string fromName, string fromEmail, string callbackUrl = "#")
        {
            var errorMessages = new List<string>();

            foreach (var booking in orderDto.Bookings)
            {
                var lodgingResult = await _provider.LodgingAsync(booking.LodgingId);
                var roomResult = await _roomDataService.RoomAsync(booking.RoomId.Value);

                var content = GenerateBookingContent(lodgingResult.Data, roomResult.Data, booking);
                var templateInfo = GetTemplateInfo("ProGolf.Base.Mails.EmailTemplates.booking-confirmation.htm");

                var emailResult = await _emailTemplateSender.SendBookingConformationEmailAsync(new EmailDetails(toName ?? "stranger", toEmail, "Booking Confirmation",
                        $"{fromName} Administration", fromEmail), booking, lodgingResult.Data, roomResult.Data, new BeneficiaryDto(), content, templateInfo);

                errorMessages.AddRange(emailResult.Messages);
            }

            if (errorMessages.Any())
                return await Result.FailAsync(errorMessages);

            return await Result.SuccessAsync("Confirmation email successfully sent");
        }

        /// <summary>
        /// Sends a booking cancellation email to the given recipient for all bookings contained in an order.
        /// </summary>
        /// <param name="orderDto">Order data transfer object containing booking information.</param>
        /// <param name="toName">Name of the recipient.</param>
        /// <param name="toEmail">Recipient's email.</param>
        /// <param name="fromName">Name used in the email "From" display.</param>
        /// <param name="fromEmail">Actual from email address.</param>
        /// <param name="callbackUrl">A callback URL (not currently used, can be passed as "#").</param>
        /// <returns>A task that represents the asynchronous send operation result.</returns>
        public async Task<IBaseResult> SendBookingCancellationEmailAsync(OrderDto orderDto, string? toName, string toEmail, string fromName, string fromEmail, string callbackUrl = "#")
        {
            var errorMessages = new List<string>();

            foreach (var booking in orderDto.Bookings)
            {
                var lodgingResult = await _provider.LodgingAsync(booking.LodgingId);
                var roomResult = await _roomDataService.RoomAsync(booking.RoomId.Value);

                // Generate the booking content HTML snippet
                var content = GenerateBookingContent(lodgingResult.Data, roomResult.Data, booking);

                // Retrieve the template info for booking confirmation
                var templateInfo = GetTemplateInfo("Kwagga.Base.Mails.EmailTemplates.booking-confirmation.htm");

                var emailResult = await _emailTemplateSender.SendBookingCancellationEmailAsync(
                    new EmailDetails(toName ?? "stranger", toEmail, "Booking Confirmation",
                        $"{fromName} Administration", fromEmail), booking, lodgingResult.Data, new BeneficiaryDto());

                errorMessages.AddRange(emailResult.Messages);
            }

            if (errorMessages.Any())
                return await Result.FailAsync(errorMessages);

            return await Result.SuccessAsync("Confirmation email successfully sent");
        }

        /// <summary>
        /// Sends a manual booking confirmation email, similar to SendBookingConformationEmailAsync but triggered manually (e.g., by an admin).
        /// </summary>
        public async Task<IBaseResult> SendManualBookingConformationEmailAsync(OrderDto orderDto, string? toName, string toEmail, string fromName, string fromEmail, string callbackUrl = "#")
        {
            var errorMessages = new List<string>();

            foreach (var booking in orderDto.Bookings)
            {
                var lodgingResult = await _provider.LodgingAsync(booking.LodgingId);
                var roomResult = await _roomDataService.RoomAsync(booking.RoomId.Value);

                // Generate the booking content HTML snippet for manual confirmation
                var content = GenerateBookingContent(lodgingResult.Data, roomResult.Data, booking);

                // Retrieve template info
                var templateInfo = GetTemplateInfo("Kwagga.Base.Mails.EmailTemplates.booking-confirmation.htm");

                var emailResult = await _emailTemplateSender.SendManualBookingConformationEmailAsync(
                    new EmailDetails(toName ?? "stranger", toEmail, "Booking Confirmation", $"{fromName} Administration", fromEmail), booking, lodgingResult.Data, roomResult.Data, new BeneficiaryDto(), content, templateInfo);

                if(!emailResult.Succeeded)
                    errorMessages.AddRange(emailResult.Messages);
            }

            if (errorMessages.Any())
                return await Result.FailAsync(errorMessages);

            return await Result.SuccessAsync("Confirmation email successfully sent");
        }

        /// <summary>
        /// Sends a manual booking confirmation email to administrators, providing booking and lodging details.
        /// </summary>
        public async Task<IBaseResult> SendManualBookingConformationAdminEmailAsync(OrderDto orderDto, string? toName, string toEmail, string fromName, string fromEmail, string callbackUrl = "#")
        {
            var errorMessages = new List<string>();

            foreach (var booking in orderDto.Bookings)
            {
                var lodgingResult = await _provider.LodgingAsync(booking.LodgingId);
                var roomResult = await _roomDataService.RoomAsync(booking.RoomId.Value);

                // Voucher confirmation template is used for manual admin emails (as per original code)
                var templateInfo = GetTemplateInfo("Kwagga.Base.Mails.EmailTemplates.booking-confirmation.htm");

                var emailResult = await _emailTemplateSender.SendManualBookingConformationAdminEmailAsync(
                    new EmailDetails(toName ?? "stranger", toEmail, "Booking Confirmation",
                        $"{fromName} Administration", fromEmail),
                    booking, lodgingResult.Data, roomResult.Data, templateInfo);

                errorMessages.AddRange(emailResult.Messages);
            }

            if (errorMessages.Any())
                return await Result.FailAsync(errorMessages);

            return await Result.SuccessAsync("Confirmation email successfully sent");
        }

        /// <summary>
        /// Sends a voucher confirmation email. This email details the voucher and related lodging/room information.
        /// </summary>
        public async Task<IBaseResult> SendVoucherConformationEmailAsync(OrderDto orderDto, string? toName, string toEmail, string toPhoneNr, string fromName, string fromEmail, string callbackUrl = "#")
        {
            var errorMessages = new List<string>();

            foreach (var voucher in orderDto.Vouchers)
            {
                var voucherResult = await _voucherService.UserVoucherAsync(voucher.UserVoucherId);
                var roomResult = await _voucherService.VoucherRoomAsync(voucher.Voucher.VoucherId.Value, voucher.Room.RoomTypeId.Value);

                var lodgingResult = await _provider.LodgingAsync(roomResult.Data.LodgingId);

                var templateInfo = GetTemplateInfo("Kwagga.Base.Mails.EmailTemplates.voucher-confirmation.htm");

                var emailResult = await _emailTemplateSender.SendVoucherConformationEmailAsync(new EmailDetails(toName ?? "stranger", toEmail, "Booking Confirmation", $"{fromName} Administration", fromEmail), toPhoneNr, voucher, lodgingResult.Data, roomResult.Data, templateInfo);

                errorMessages.AddRange(emailResult.Messages);
            }

            if (errorMessages.Any())
                return await Result.FailAsync(errorMessages);

            return await Result.SuccessAsync("Confirmation email successfully sent");
        }

        #region Private Helper Methods

        /// <summary>
        /// Retrieves the assembly containing this client's embedded email templates.
        /// </summary>
        /// <returns>The assembly containing the email templates.</returns>
        private System.Reflection.Assembly GetEmailTemplatesAssembly()
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .SingleOrDefault(assem => assem.GetName().Name == "Kwagga.Base");

            if (assembly == null)
                throw new Exception("Unable to find the 'Kwagga.Blazor.SiteClient' assembly with embedded templates.");

            return assembly;
        }

        /// <summary>
        /// Creates a <see cref="TemplateInfo"/> object for a given template file.
        /// </summary>
        /// <param name="templateFileName">The name of the template file (an embedded resource).</param>
        /// <returns>A TemplateInfo object with the specified template loaded from the assembly.</returns>
        private TemplateInfo GetTemplateInfo(string templateFileName)
        {
            return new TemplateInfo(GetEmailTemplatesAssembly(), templateFileName);
        }

        /// <summary>
        /// Generates the HTML snippet used in booking confirmation emails, displaying lodging and room details.
        /// </summary>
        /// <param name="lodging">Lodging details.</param>
        /// <param name="room">Room details.</param>
        /// <param name="booking">Booking details.</param>
        /// <returns>An HTML string representing the booking content block.</returns>
        private string GenerateBookingContent(LodgingDto lodging, RoomDto room, BookingDto booking)
        {
            // The following is the HTML snippet used to detail a single booking line item.
            // It includes lodging images, room details, stay duration, and pricing.
            // This HTML is structured in tables, similar to a styled email layout.
            var content = "<tr><td height=\"20\"></td></tr><tr>" +
                          "<td width=\"20\"></td>" +
                          "<td align=\"left\" width=\"250\" class=\"title-width\">" +
                          "<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"display-width\" width=\"100%\">" +
                          "<tr><td align=\"left\" valign=\"middle\" width=\"70\">" +
                          "<img src=\"https://kwaggatravel.co.za/" + lodging.Images.FirstOrDefault(c => c.ImageType == UploadType.Image).RelativePath + "\" alt=\"" +
                          lodging.Name + "\" width=\"60\" />" +
                          "</td>" +
                          "<td class=\"hide\">" +
                          "<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"display-width\" width=\"100%\">" +
                          "<tr>" +
                          "<td align=\"left\" class=\"MsoNormal\" style=\"color:#333333;font-family:'Segoe UI',sans-serif,Arial,Helvetica,Lato;font-size:15px;font-weight:600;letter-spacing:1px;line-height:18px;text-transform:capitalize;\">" +
                          lodging.Name +
                          "</tr>" +
                          "<tr><td height=\"5\"></td></tr>" +
                          "<tr><td>" +
                          "<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"display-width\" width=\"100%\" style=\"width:auto !important;\">" +
                          "<tr>" +
                          "<td align=\"left\" valign=\"middle\" width=\"20\">" +
                          "<img src=\"http://pennyblacktemplates.com/demo/notifications/order-confirmation/images/15x15.png\" alt=\"15x15\" width=\"15\" />" +
                          "</td>" +
                          "<td align=\"left\" class=\"MsoNormal\" style=\"color:#666666;font-family:'Segoe UI',sans-serif,Arial,Helvetica,Lato;font-size:11px;letter-spacing:1px;line-height:12px;text-transform:capitalize;\">" +
                          "Room :" +
                          "</td>" +
                          "<td width=\"5\"></td>" +
                          "<td align=\"left\" class=\"MsoNormal\" style=\"color:#ff4202;font-family:'Segoe UI',sans-serif,Arial,Helvetica,Lato;font-size:11px;letter-spacing:1px;line-height:12px;text-transform:capitalize; font-weight:600;\">" +
                          room.Name +
                          "</td>" +
                          "</tr>" +
                          "<tr>" +
                          "<td align=\"left\" valign=\"middle\" width=\"20\"></td>" +
                          "<td align=\"left\" class=\"MsoNormal\" style=\"color:#666666;font-family:'Segoe UI',sans-serif,Arial,Helvetica,Lato;font-size:11px;letter-spacing:1px;line-height:12px;text-transform:capitalize;\">" +
                          "From :" +
                          "</td>" +
                          "<td width=\"5\"></td>" +
                          "<td align=\"left\" class=\"MsoNormal\" style=\"color:#ff4202;font-family:'Segoe UI',sans-serif,Arial,Helvetica,Lato;font-size:11px;letter-spacing:1px;line-height:12px;text-transform:capitalize; font-weight:600;\">" +
                          booking.StartDate.ToShortDateString() +
                          "</td>" +
                          "</tr>" +
                          "<tr>" +
                          "<td align=\"left\" valign=\"middle\" width=\"20\"></td>" +
                          "<td align=\"left\" class=\"MsoNormal\" style=\"color:#666666;font-family:'Segoe UI',sans-serif,Arial,Helvetica,Lato;font-size:11px;letter-spacing:1px;line-height:12px;text-transform:capitalize;\">" +
                          "To :" +
                          "</td>" +
                          "<td width=\"5\"></td>" +
                          "<td align=\"left\" class=\"MsoNormal\" style=\"color:#ff4202;font-family:'Segoe UI',sans-serif,Arial,Helvetica,Lato;font-size:11px;letter-spacing:1px;line-height:12px;text-transform:capitalize; font-weight:600;\">" +
                          booking.EndDate.ToShortDateString() +
                          "</td>" +
                          "</tr>" +
                          "</table>" +
                          "</td>" +
                          "</tr>" +
                          "</table>" +
                          "</td>" +
                          "</tr>" +
                          "</table>" +
                          "</td>" +
                          "<td width=\"20\"></td>" +
                          "<td align=\"left\" width=\"90\" class=\"MsoNormal\" style=\"color:#333333;font-family:'Segoe UI',sans-serif,Arial,Helvetica,Lato;font-size:14px;font-weight:600;letter-spacing:1px;line-height:18px;text-transform:capitalize;\">" +
                          "<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"display-width\" width=\"100%\" style=\"width:auto !important;\">" +
                          "<tr>" +
                          "<td align=\"left\" valign=\"middle\" width=\"20\"></td>" +
                          "<td align=\"left\" class=\"MsoNormal\" style=\"color:#666666;font-family:'Segoe UI',sans-serif,Arial,Helvetica,Lato;font-size:11px;letter-spacing:1px;line-height:12px;text-transform:capitalize;\">" +
                          "Adults :" +
                          "</td>" +
                          "<td width=\"5\"></td>" +
                          "<td align=\"left\" class=\"MsoNormal\" style=\"color:#ff4202;font-family:'Segoe UI',sans-serif,Arial,Helvetica,Lato;font-size:11px;letter-spacing:1px;line-height:12px;text-transform:capitalize; font-weight:600;\">" +
                          booking.Adults +
                          "</td>" +
                          "</tr>" +
                          "<tr>" +
                          "<td align=\"left\" valign=\"middle\" width=\"20\"></td>" +
                          "<td align=\"left\" class=\"MsoNormal\" style=\"color:#666666;font-family:'Segoe UI',sans-serif,Arial,Helvetica,Lato;font-size:11px;letter-spacing:1px;line-height:12px;text-transform:capitalize;\">" +
                          "Kids :" +
                          "</td>" +
                          "<td width=\"5\"></td>" +
                          "<td align=\"left\" class=\"MsoNormal\" style=\"color:#ff4202;font-family:'Segoe UI',sans-serif,Arial,Helvetica,Lato;font-size:11px;letter-spacing:1px;line-height:12px;text-transform:capitalize; font-weight:600;\">" +
                          booking.Children +
                          "</td>" +
                          "</tr>" +
                          "</table>" +
                          "</td>" +
                          "<td width=\"20\"></td>" +
                          "<td align=\"left\" width=\"60\" class=\"MsoNormal\" style=\"color:#333333;font-family:'Segoe UI',sans-serif,Arial,Helvetica,Lato;font-size:14px;font-weight:600;letter-spacing:1px;line-height:18px;text-transform:capitalize;\">" +
                          booking.Nights + " Nights" +
                          "</td>" +
                          "<td width=\"20\"></td>" +
                          "<td align=\"left\" width=\"60\" class=\"MsoNormal\" style=\"color:#333333;font-family:'Segoe UI',sans-serif,Arial,Helvetica,Lato;font-size:14px;font-weight:600;letter-spacing:1px;line-height:18px;text-transform:capitalize;\">" +
                          booking.AmountDueExcl.ToString("C") +
                          "</td>" +
                          "<td width=\"20\"></td>" +
                          "</tr>" +
                          "<tr><td height=\"20\"></td></tr>" +
                          "<tr><td colspan=\"10\" style=\"border-bottom:1px solid #dddddd;\"></td></tr>" +
                          "<tr><td height=\"20\"></td></tr><tr><td height=\"20\"></td></tr>";

            return content;
        }

        #endregion
    }
}
