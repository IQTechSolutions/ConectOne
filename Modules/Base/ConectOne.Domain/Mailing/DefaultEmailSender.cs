using ConectOne.Domain.Mailing.Entities;
using ConectOne.Domain.Mailing.Interfaces;
using ConectOne.Domain.Mailing.TemplateSender;
using ConectOne.Domain.ResultWrappers;
using Microsoft.Extensions.Configuration;

namespace ConectOne.Domain.Mailing
{
    /// <summary>
    /// Provides default email sending functionality using an email template sender.
    /// </summary>
    public class DefaultEmailSender
    {
        private readonly IEmailTemplateSender _emailTemplateSender;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultEmailSender"/> class.
        /// </summary>
        /// <param name="emailTemplateSender">The email template sender to use for sending emails.</param>
        public DefaultEmailSender(IEmailTemplateSender emailTemplateSender, IConfiguration configuration)
        {
            _emailTemplateSender = emailTemplateSender;
            _configuration = configuration;
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
                new EmailDetails(toName, toEmail, string.IsNullOrEmpty(emailSubjectLine) ? $"Thank You for submitting a support request to {fromName}" : emailSubjectLine, $"{fromName} Administration", fromEmail),
                toName, cell, toEmail, title, message);
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
        public async Task<IBaseResult> SendAdminsSupportTicketEmailAsync(string toName, string toEmail, string fromName, string fromSurname, string fromEmail, string cell, string title, string message, string? emailSubjectLine = null)
        {
            return await _emailTemplateSender.SendContactUsAdminEmailAsync(
                new EmailDetails(toName, toEmail, string.IsNullOrEmpty(emailSubjectLine) ? $"Thank You for submitting a support request to {fromName}" : emailSubjectLine, $"{fromName} Administration", fromEmail),
                fromName, fromSurname, cell, fromEmail, title, message);
        }
        
        /// <summary>
        /// Sends a user support ticket email.
        /// </summary>
        /// <param name="toName">The name of the recipient.</param>
        /// <param name="toEmail">The email address of the recipient.</param>
        /// <param name="fromName">The name of the sender.</param>
        /// <param name="fromEmail">The email address of the sender.</param>
        /// <param name="callbackUrl">The callback URL for the email.</param>
        /// <returns>A task that represents the asynchronous send operation result.</returns>
        public async Task<IBaseResult> SendUserSupportTicketEmailAsync(string? toName, string toEmail, string fromName, string fromEmail, string callbackUrl = "#")
        {
            return await _emailTemplateSender.SendGeneralEmailAsync(
                new EmailDetails(toName ?? "stranger", toEmail, $"Thank You for submitting a support request to {fromName}", $"{fromName} Administration", fromEmail),
                "Thank You",
                $"Hi {toName ?? "stranger"},",
                $"Thank you for submitting a support request with us. A {fromName} service representative will get back to you shortly.",
                "View your Profile",
                callbackUrl
            );
        }

        /// <summary>
        /// Sends an admin support ticket email.
        /// </summary>
        /// <param name="toName">The name of the recipient.</param>
        /// <param name="toEmail">The email address of the recipient.</param>
        /// <param name="displayName">The display name of the sender.</param>
        /// <param name="subject">The subject of the support ticket.</param>
        /// <param name="cell">The cell phone number of the sender.</param>
        /// <param name="details">The details of the support ticket.</param>
        /// <param name="email">The email address of the sender.</param>
        /// <param name="callbackUrl">The callback URL for the email.</param>
        /// <returns>A task that represents the asynchronous send operation result.</returns>
        public async Task<IBaseResult> SendAdminSupportTicketEmailAsync(string toName, string toEmail, string displayName, string subject, string cell, string details, string email, string callbackUrl = "#")
        {
            return await _emailTemplateSender.SendGeneralEmailAsync(
                new EmailDetails(toName, toEmail, $"Support Ticket from {displayName}", $"{toName} Website Administration", toEmail),
                $"Support Ticket Submitted by {displayName}",
                "Hi Administrator,",
                $"{displayName} submitted a support request with the following details.<br/>" +
                $"<strong>Subject : </strong> {subject}.<br/> " +
                $"<strong>Email : </strong> {email}.<br/> " +
                $"<strong>Telephone : </strong> {cell}.<br/> " +
                $"<strong>Details : </strong> {details}.",
                "Go to Support Ticket",
                callbackUrl
            );
        }

        /// <summary>
        /// Sends a user verification email.
        /// </summary>
        /// <param name="toName">The name of the recipient.</param>
        /// <param name="toEmail">The email address of the recipient.</param>
        /// <param name="fromName">The name of the sender.</param>
        /// <param name="fromEmail">The email address of the sender.</param>
        /// <param name="verificationUrl">The verification URL for the email.</param>
        /// <returns>A task that represents the asynchronous send operation result.</returns>
        public async Task<IBaseResult> SendUserVerificationEmailAsync(string toName, string toEmail, string fromName, string fromEmail, string verificationUrl)
        {
            return await _emailTemplateSender.SendGeneralEmailAsync(
                new EmailDetails(toName, toEmail, "Please Verify Your Email Address", $"{fromName} Website Administration", fromEmail),
                "Verify Email",
                $"Hi {toName ?? "stranger"},",
                "Thanks for creating an account with us.<br/>To continue please verify your email with us.",
                "Verify Email",
                verificationUrl
            );
        }

        /// <summary>
        /// Sends a welcome email to a new user.
        /// </summary>
        /// <param name="toName">The name of the recipient.</param>
        /// <param name="toEmail">The email address of the recipient.</param>
        /// <param name="fromName">The name of the sender.</param>
        /// <param name="fromEmail">The email address of the sender.</param>
        /// <param name="callbackUrl">The callback URL for the email.</param>
        /// <returns>A task that represents the asynchronous send operation result.</returns>
        public async Task<IBaseResult> SendWelcomeUserEmailAsync(string toName, string toEmail, string fromName, string fromEmail, string callbackUrl)
        {
            return await _emailTemplateSender.SendGeneralEmailAsync(
                new EmailDetails(toName, toEmail, $"You have been registered as a valued Customer at {fromName}", $"{fromName} Website Admin", fromEmail),
                "Welcome Email",
                $"Hi {fromName ?? "stranger"},",
                "You have been registered as a valued customer at OliRoss. Thank you for doing business with us.<br/> " +
                $"You can use the link below to go to the login screen of your account.",
                "Welcome to the OliRoss Family",
                callbackUrl
            );
        }

        /// <summary>
        /// Sends a forgot password email.
        /// </summary>
        /// <param name="email">The email address of the recipient.</param>
        /// <param name="fromName">The name of the sender.</param>
        /// <param name="fromEmail">The email address of the sender.</param>
        /// <param name="verificationUrl">The verification URL for the email.</param>
        /// <returns>A task that represents the asynchronous send operation result.</returns>
        public async Task<IBaseResult> SendForgotPasswordEmailAsync(string email, string fromName, string fromEmail, string verificationUrl)
        {
            return await _emailTemplateSender.SendGeneralEmailAsync(
                new EmailDetails("User", email, "Please Reset your Password", $"{fromName} Website Administration", fromEmail),
                "Reset Password",
                $"Hi User,",
                "You have requested to reset your password. If you did not request this reset, please contact our support team.",
                "Please reset your password by clicking on the button below",
                "Reset Password",
                verificationUrl
            );
        }

        /// <summary>
        /// Sends a message email.
        /// </summary>
        /// <param name="receiver">The name of the recipient.</param>
        /// <param name="receiverEmailAddress">The email address of the recipient.</param>
        /// <param name="sender">The name of the sender.</param>
        /// <param name="fromName">The name of the sender's organization.</param>
        /// <param name="fromEmail">The email address of the sender.</param>
        /// <param name="message">The message content.</param>
        /// <param name="callbackUrl">The callback URL for the email.</param>
        /// <returns>A task that represents the asynchronous send operation result.</returns>
        public async Task<IBaseResult> SendMessageEmailAsync(string receiver, string receiverEmailAddress, string sender, string fromName, string fromEmail, string message, string callbackUrl)
        {
            return await _emailTemplateSender.SendGeneralEmailAsync(
                new EmailDetails(receiver, receiverEmailAddress, $"Hi {receiver}", $"{fromName} Message", fromEmail),
                $"You received the following message from {sender}",
                $"Hi {receiver}",
                $"{message}",
                "View",
                callbackUrl
            );
        }

        /// <summary>
        /// Sends a notification email.
        /// </summary>
        /// <param name="receiver">The name of the recipient.</param>
        /// <param name="receiverEmailAddress">The email address of the recipient.</param>
        /// <param name="fromName">The name of the sender's organization.</param>
        /// <param name="fromEmail">The email address of the sender.</param>
        /// <param name="notification">The notification content.</param>
        /// <param name="callbackUrl">The callback URL for the email.</param>
        /// <returns>A task that represents the asynchronous send operation result.</returns>
        public async Task<IBaseResult> SendNotificationEmailAsync(string receiver, string receiverEmailAddress, string fromName, string fromEmail, string notification, string callbackUrl)
        {
            return await _emailTemplateSender.SendGeneralEmailAsync(
                new EmailDetails(receiver, receiverEmailAddress, $"Hi {receiver}", $"{fromName} Message", fromEmail),
                $"You received the following notification from {fromName}",
                $"Hi {receiver}",
                $"{notification}",
                "View",
                callbackUrl
            );
        }

        /// <summary>
        /// Sends a team member invitation email.
        /// </summary>
        /// <param name="toName">The name of the recipient.</param>
        /// <param name="toEmail">The email address of the recipient.</param>
        /// <param name="fromName">The name of the sender's organization.</param>
        /// <param name="fromEmail">The email address of the sender.</param>
        /// <param name="callbackUrl">The callback URL for the email.</param>
        /// <returns>A task that represents the asynchronous send operation result.</returns>
        public async Task<IBaseResult> SendTeamMemberInvitationEmailsAsync(string toName, string toEmail, string fromName, string fromEmail, string callbackUrl)
        {
            return await _emailTemplateSender.SendGeneralEmailAsync(
                new EmailDetails(toName, toEmail, $"Hi {toName}, you are invited to {fromName}", $"{fromName} Website Administration", fromEmail),
                $"{fromName} Team Invitation",
                $"Hi {toName}",
                $"You were invited to join {fromName}'s team. Please click on the link to accept this invitation.",
                "Accept this Invitation",
                callbackUrl
            );
        }

        /// <summary>
        /// Sends a welcome email to a new team member.
        /// </summary>
        /// <param name="toName">The name of the recipient.</param>
        /// <param name="toEmail">The email address of the recipient.</param>
        /// <param name="fromName">The name of the sender's organization.</param>
        /// <param name="fromEmail">The email address of the sender.</param>
        /// <param name="callbackUrl">The callback URL for the email.</param>
        /// <returns>A task that represents the asynchronous send operation result.</returns>
        public async Task<IBaseResult> SendWelcomeTeamMemberEmailAsync(string toName, string toEmail, string fromName, string fromEmail, string callbackUrl)
        {
            return await _emailTemplateSender.SendGeneralEmailAsync(
                new EmailDetails(toName, toEmail, $"Hi {toEmail}, you have been accepted to {fromName}", $"Welcome to {fromName}", fromEmail),
                $"Welcome to the {fromName} Team",
                $"Hi {toName}",
                $"You accepted the invitation from {fromName}'s team. Please click on the link to view your profile.",
                "View my Profile",
                callbackUrl
            );
        }

        /// <summary>
        /// Sends an email indicating that the invitation acceptance failed.
        /// </summary>
        /// <param name="toName">The name of the recipient.</param>
        /// <param name="toEmail">The email address of the recipient.</param>
        /// <param name="fromName">The name of the sender's organization.</param>
        /// <param name="fromEmail">The email address of the sender.</param>
        /// <param name="errorMessage">The error message describing the failure.</param>
        /// <param name="callbackUrl">The callback URL for the email.</param>
        /// <returns>A task that represents the asynchronous send operation result.</returns>
        public async Task<IBaseResult> SendInvitationAcceptanceFailedEmailAsync(string toName, string toEmail, string fromName, string fromEmail, string errorMessage, string callbackUrl)
        {
            return await _emailTemplateSender.SendGeneralEmailAsync(
                new EmailDetails(toName, toEmail, $"Hi {toName}, ", $"{fromName} : An error occurred", fromEmail),
                $"Thank you for accepting {fromName}'s Invitation",
                $"Hi {toName}",
                $"The following error occurred while accepting this invitation: {errorMessage}. Please click on the link to resend the invitation.",
                "Resend Invitation",
                callbackUrl
            );
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
                $"Forgot Notification from {_configuration["ApplicationConfiguration:AppliactionName"]} {_configuration["ApplicationConfiguration:SchoolTypeName"]}",
                $"{_configuration["ApplicationConfiguration:AppliactionName"]} {_configuration["ApplicationConfiguration:SchoolTypeName"]} Administration",
                $"{_configuration["ApplicationConfiguration:DoNotReplyEmailAddress"]}"
            );

            // Use the specialized forgotPassword.htm template for building the email.
            var emailResult = await _emailTemplateSender.SendForgotPasswordEmailAsync(
                emailDetails,
                logoUrl,
                caption,
                logoLink,
                buttonUrl,
                $"{_configuration["ApplicationConfiguration:AppliactionName"]} {_configuration["ApplicationConfiguration:SchoolTypeName"]}",
                new TemplateInfo(
                    AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assem => assem.GetName().Name == "NeuralTech.Base"),
                    "NeuralTech.Base.Mailing.TemplateSender.Templates.forgotPassword.htm"
                )
            );

            if (!emailResult.Succeeded)
                return await Result.FailAsync(emailResult.Messages);

            return await Result.SuccessAsync("Forgot Password Email successfully sent");
        }
    }
}

