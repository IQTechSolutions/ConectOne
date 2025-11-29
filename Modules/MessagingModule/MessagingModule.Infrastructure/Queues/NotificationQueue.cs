using System.Collections.Concurrent;
using MessagingModule.Domain.RequestFeatures;

namespace MessagingModule.Infrastructure.Queues
{
    /// <summary>
    /// Represents a thread-safe queue for managing push notification requests.
    /// </summary>
    /// <remarks>This class provides methods to enqueue and dequeue push notification requests in a
    /// thread-safe manner. It is designed to handle concurrent access, ensuring safe operations in multi-threaded
    /// environments.</remarks>
    public class NotificationQueue
    {
        /// <summary>
        /// A thread-safe queue that stores email push notification requests to be processed.
        /// </summary>
        /// <remarks>This queue is used to manage pending <see cref="CreatePushNotificationRequest"/>
        /// objects in a  concurrent environment, ensuring thread safety during enqueue and dequeue
        /// operations.</remarks>
        private readonly ConcurrentQueue<CreatePushNotificationRequest> _emails = new();

        /// <summary>
        /// Adds a push notification request to the queue for processing.
        /// </summary>
        /// <remarks>The method adds the specified <see cref="CreatePushNotificationRequest"/> to an
        /// internal queue.  The queued requests will be processed in the order they are added (FIFO).</remarks>
        /// <param name="message">The push notification request to enqueue. This parameter cannot be null.</param>
        public void Enqueue(CreatePushNotificationRequest message)
        {
            _emails.Enqueue(message);
        }

        /// <summary>
        /// Attempts to remove and return the object at the beginning of the queue.
        /// </summary>
        /// <param name="message">When this method returns, contains the object removed from the queue if the operation was successful;
        /// otherwise, the default value for the type of the <paramref name="message"/> parameter.</param>
        /// <returns><see langword="true"/> if an object was successfully removed from the queue; otherwise, <see
        /// langword="false"/>.</returns>
        public bool TryDequeue(out CreatePushNotificationRequest message)
        {
            return _emails.TryDequeue(out message);
        }
    }
}
