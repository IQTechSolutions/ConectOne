using ConectOne.Domain.Entities;

namespace IdentityModule.Domain.Entities
{
    /// <summary>
    /// Represents the application settings for a user, including preferences for displaying personal information and
    /// receiving communications such as notifications, newsletters, and messages.
    /// </summary>
    /// <remarks>This class provides a set of configurable options that allow users to control the visibility
    /// of their personal information (e.g., job title, phone number, email address) and their preferences for receiving
    /// various types of communications (e.g., notifications, newsletters, messages, and emails).</remarks>
    public class UserAppSettings : EntityBase<string>
    {
        /// <summary>
        /// Gets or sets a value indicating whether the job title should be displayed.
        /// </summary>
        public bool ShowJobTitle { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the phone number should be displayed.
        /// </summary>
        public bool ShowPhoneNr { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the email address should be displayed.
        /// </summary>
        public bool ShowEmailAddress { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the user will receive notifications.
        /// </summary>
        public bool ReceiveNotifications { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the user has opted to receive newsletters.
        /// </summary>
        public bool ReceiveNewsletters { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the system should receive messages.
        /// </summary>
        public bool ReceiveMessages { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the user has opted to receive emails.
        /// </summary>
        public bool ReceiveEmails { get; set; } = true;

        /// <summary>
        /// Returns a string representation of the user application settings.
        /// </summary>
        /// <returns>A string that represents the user application settings.</returns>
        public override string ToString()
        {
            return "User Application Settings";
        }
    }
}
