using Microsoft.AspNetCore.SignalR;
using IdentityModule.Domain.Extensions;

namespace IdentityModule.Infrastructure.Implimentation
{
    /// <summary>
    /// Provides a custom implementation for retrieving a unique user identifier from a SignalR connection context.
    /// </summary>
    /// <remarks>This class is typically used to customize how user IDs are determined for SignalR
    /// connections, which can affect how messages are routed to users. Implementations may extract user identifiers
    /// from claims, headers, or other sources within the connection context. Thread safety and user identity
    /// consistency depend on the underlying logic of the user ID retrieval.</remarks>
    public class CustomUserIdProvider : IUserIdProvider
    {
        /// <summary>
        /// Retrieves the unique user identifier associated with the specified connection.
        /// </summary>
        /// <param name="connection">The connection context representing the user whose identifier is to be retrieved. Cannot be null.</param>
        /// <returns>A string containing the unique identifier for the user associated with the connection.</returns>
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User.GetUserId();
        }
    }
}
