using Microsoft.AspNetCore.SignalR;
using SchoolsEnterprise.Base.Constants;

namespace MessagingModule.Infrastructure.Hubs
{

    /// <summary>
    /// The <see cref="NotificationsHub"/> manages the real-time push notification flow between the
    /// Schools Enterprise back-end and connected SignalR clients.  It exposes hub methods that can
    /// be invoked by the web API or other server-side components to broadcast notification payloads
    /// to specific users.
    /// </summary>
    public class NotificationsHub : Hub
    {
        /// <summary>
        /// Sends a push notification to a specific user, which can be displayed
        /// in real-time (e.g., a toast notification) in the client's UI.
        /// </summary>
        /// <param name="message">The content of the push notification.</param>
        /// <param name="receiverUserId">The user who should receive the notification.</param>
        /// <param name="senderUserId">The user who triggered the notification.</param>
        public async Task SendPushNotification(List<string> userIds, string type, string title, string message, string url)
        {
            await Clients.Users(userIds).SendAsync(ApplicationConstants.SignalR.SendPushNotification, type, title, message, url);
        }
    }
}
