using System.Collections.Concurrent;
using ConectOne.Domain.Mailing.Entities;

namespace ConectOne.Domain.Mailing.Services
{
    /// <summary>
    /// Maintains a thread-safe queue of email messages to be processed by a background service.
    /// </summary>
    public class EmailQueue
    {
        private readonly ConcurrentQueue<EmailDetails> _emails = new();

        /// <summary>
        /// Adds an email message to the end of the queue.
        /// </summary>
        /// <param name="message">The email message to enqueue.</param>
        public void Enqueue(EmailDetails message)
        {
            _emails.Enqueue(message);
        }

        /// <summary>
        /// Attempts to remove and return an email message from the front of the queue.
        /// </summary>
        /// <param name="message">
        /// When this method returns, contains the object removed from the 
        /// front of the queue, or the default value of <see cref="EmailDetails"/> 
        /// if the queue is empty.
        /// </param>
        /// <returns>
        /// <c>true</c> if an element was removed and returned successfully; 
        /// <c>false</c> if the queue is empty.
        /// </returns>
        public bool TryDequeue(out EmailDetails message)
        {
            return _emails.TryDequeue(out message);
        }
    }
}
