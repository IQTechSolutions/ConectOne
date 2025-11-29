using ConectOne.Domain.Mailing.Entities;
using ConectOne.Domain.ResultWrappers;

namespace ConectOne.Domain.Mailing.Interfaces
{
    /// <summary>
    /// Defines the contract for sending emails (synchronous or asynchronous).
    /// Implementations should return status information via 
    /// <see cref="IBaseResult"/> indicating success or failure.
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Sends an email synchronously using the provided 
        /// <see cref="EmailDetails"/>.
        /// </summary>
        /// <param name="details">Encapsulates the data necessary to send an email (e.g., recipient info, subject, body).</param>
        /// <returns>
        /// An <see cref="IBaseResult"/> indicating success or failure, 
        /// along with any relevant messages or error details.
        /// </returns>
        IBaseResult SendEmail(EmailDetails details);

        /// <summary>
        /// Sends an email asynchronously using the provided 
        /// <see cref="EmailDetails"/>.
        /// </summary>
        /// <param name="details">Encapsulates the data necessary to send an email (e.g., recipient info, subject, body).</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> wrapping an <see cref="IBaseResult"/>. 
        /// This allows awaiting the outcome of the operation and handling any results or errors accordingly.
        /// </returns>
        Task<IBaseResult> SendEmailAsync(EmailDetails details);
    }
}