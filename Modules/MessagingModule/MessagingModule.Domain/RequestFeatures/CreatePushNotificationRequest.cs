using IdentityModule.Domain.DataTransferObjects;
using MessagingModule.Domain.DataTransferObjects;

namespace MessagingModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents a request to create (or send) a push notification 
    /// to a list of users. It bundles user information along with 
    /// the <see cref="NotificationDto"/> that holds the message details.
    /// </summary>
    public class CreatePushNotificationRequest
    {
        /// <summary>
        /// Parameterless constructor allowing for manual assignment of properties.
        /// </summary>
        public CreatePushNotificationRequest() { }

        /// <summary>
        /// Overloaded constructor that initializes the request with the specified user list 
        /// and notification content.
        /// </summary>
        /// <param name="users">A collection of user info objects (e.g., recipients).</param>
        /// <param name="notification">The notification data describing message type, title, etc.</param>
        public CreatePushNotificationRequest(List<RecipientDto> users, NotificationDto notification)
        {
            Users = users;
            Notification = notification;
        }

        /// <summary>
        /// A list of users who will receive the push notification.
        /// </summary>
        public List<RecipientDto> Users { get; set; } = [];

        /// <summary>
        /// Describes the message content, type, and any additional metadata 
        /// for the push notification.
        /// </summary>
        public NotificationDto Notification { get; set; } = null!;
    }

    /// <summary>
    /// Represents a request to create a blog post notification.
    /// </summary>
    /// <param name="BlogPostId">The identity of the blogpost that requires the notification</param>
    /// <param name="Users">A list of users the notification should be sent to</param>
    public record CreateBlogPostNotificationRequest(string BlogPostId, List<RecipientDto> Users);
}