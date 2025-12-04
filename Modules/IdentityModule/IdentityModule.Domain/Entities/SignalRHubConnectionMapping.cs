namespace IdentityModule.Domain.Entities
{
    /// <summary>
    /// Provides thread-safe mapping between user identifiers and their associated SignalR connection identifiers,
    /// enabling management of multiple active connections per user.
    /// </summary>
    /// <remarks>This static class is intended for scenarios where users may have multiple concurrent SignalR
    /// connections, such as when accessing an application from multiple devices or browser tabs. All methods are
    /// thread-safe and can be called from multiple threads without additional synchronization.</remarks>
    public class SignalRHubConnectionMapping
    {
        private readonly Dictionary<string, HashSet<string>> _userConnections = new();

        /// <summary>
        /// Associates the specified connection ID with the given user ID, adding the connection to the user's
        /// collection of active connections.
        /// </summary>
        /// <remarks>If the user does not already have any connections, a new collection is created. This
        /// method is thread-safe.</remarks>
        /// <param name="userId">The unique identifier of the user to associate with the connection. Cannot be null.</param>
        /// <param name="connectionId">The identifier of the connection to add for the specified user. Cannot be null.</param>
        public void Add(string userId, string connectionId)
        {
            lock (_userConnections)
            {
                if (!_userConnections.TryGetValue(userId, out var connections))
                {
                    connections = new HashSet<string>();
                    _userConnections[userId] = connections;
                }

                connections.Add(connectionId);
            }
        }

        /// <summary>
        /// Removes the specified connection identifier from the collection of connections associated with the given
        /// user.
        /// </summary>
        /// <remarks>If the user has no remaining connections after removal, the user is also removed from
        /// the collection. This method is thread-safe.</remarks>
        /// <param name="userId">The unique identifier of the user whose connection is to be removed. Cannot be null.</param>
        /// <param name="connectionId">The identifier of the connection to remove from the user's collection. Cannot be null.</param>
        public void Remove(string userId, string connectionId)
        {
            lock (_userConnections)
            {
                if (!_userConnections.TryGetValue(userId, out var connections))
                    return;

                connections.RemoveWhere(c => c == connectionId);
                if (connections.Count == 0)
                    _userConnections.Remove(userId);
            }
        }

        /// <summary>
        /// Retrieves the collection of active connection identifiers associated with the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose connections are to be retrieved. Cannot be null.</param>
        /// <returns>An immutable collection of strings containing the connection identifiers for the specified user. Returns an
        /// empty collection if the user has no active connections.</returns>
        public IReadOnlyCollection<string> GetConnections(string userId)
        {
            lock (_userConnections)
            {
                return _userConnections.TryGetValue(userId, out var connections)
                    ? connections.ToList()
                    : Array.Empty<string>();
            }
        }
    }
}
