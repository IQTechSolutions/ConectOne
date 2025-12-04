using IdentityModule.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(AuthenticationSchemes = "Bearer")] public class NotificationsHub : Hub
    {
        /// <summary>
        /// Initializes a new instance of the NotificationsHub class using the specified SignalR hub connection mapping.
        /// </summary>
        /// <param name="signalRHubConnectionMapping">The SignalRHubConnectionMapping instance that manages the mapping between user identifiers and SignalR
        /// connection IDs. Cannot be null.</param>
        public NotificationsHub(SignalRHubConnectionMapping signalRHubConnectionMapping)
        {
            SignalRHubConnectionMapping = signalRHubConnectionMapping;
        }

        /// <summary>
        /// Gets the mapping of SignalR hub connections associated with the current instance.
        /// </summary>
        private SignalRHubConnectionMapping SignalRHubConnectionMapping { get; }

        /// <summary>
        /// Called when a new connection is established with the hub.
        /// </summary>
        /// <remarks>This method adds the connected user's identifier and connection ID to the connection
        /// mapping. Override this method to perform additional actions when a client connects. Always call the base
        /// implementation to ensure the connection is properly established.</remarks>
        /// <returns>A task that represents the asynchronous connect operation.</returns>
        public override Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier ?? Context.ConnectionId;
            SignalRHubConnectionMapping.Add(userId, Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        /// <summary>
        /// Handles the event when a client disconnects from the hub.
        /// </summary>
        /// <remarks>This method is called by the SignalR framework when a client disconnects, either
        /// intentionally or due to a network failure. Override this method to perform custom logic when a connection is
        /// terminated.</remarks>
        /// <param name="exception">The exception that occurred during the disconnect operation, if any; otherwise, null.</param>
        /// <returns>A task that represents the asynchronous disconnect operation.</returns>
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.UserIdentifier ?? Context.ConnectionId;
            SignalRHubConnectionMapping.Remove(userId, Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Marks a push notification as read for a specified user and notification type.
        /// </summary>
        /// <param name="userId">The unique identifier of the user for whom the notification is being marked as read. Cannot be null or
        /// empty.</param>
        /// <param name="type">The type or category of the push notification to be marked as read. Cannot be null or empty.</param>
        /// <param name="notificationId">The unique identifier of the notification to mark as read. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task ReadPushNotification(string userId, string type, string notificationId)
        {
            await Clients.Users(userId).SendAsync(ApplicationConstants.SignalR.ReadPushNotification, type, notificationId);
        }
    }
}
