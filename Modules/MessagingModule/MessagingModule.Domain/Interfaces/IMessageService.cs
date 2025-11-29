using ConectOne.Domain.ResultWrappers;
using MessagingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.RequestFeatures;

namespace MessagingModule.Domain.Interfaces
{
    /// <summary>
    /// Defines the contract for message-related operations, including retrieval, creation, updating, and deletion of messages.
    /// </summary>
    public interface IMessageService
    {
        #region Message Retrieval

        /// <summary>
        /// Retrieves a paginated list of messages based on the provided parameters.
        /// </summary>
        /// <param name="args">The parameters for pagination and filtering.</param>
        /// <returns>A paginated result containing a list of messages.</returns>
        Task<PaginatedResult<MessageDto>> PagedMessagesAsync(MessagePageParameters args);

        /// <summary>
        /// Retrieves a paginated list of notification messages based on the provided parameters.
        /// </summary>
        /// <param name="args">The parameters for pagination and filtering.</param>
        /// <returns>A paginated result containing a list of notification messages.</returns>
        Task<PaginatedResult<MessageDto>> PagedNotificationMessagesAsync(MessagePageParameters args);

        /// <summary>
        /// Retrieves a notification message based on the provided parameters.
        /// </summary>
        /// <param name="args">The parameters for retrieving the notification message.</param>
        /// <returns>A result containing the notification message.</returns>
        Task<IBaseResult<MessageDto>> NotificationMessageAsync(MessagePageParameters args);

        /// <summary>
        /// Retrieves a list of unread messages for a specific receiver.
        /// </summary>
        /// <param name="receiverId">The ID of the receiver.</param>
        /// <returns>A result containing a list of unread messages.</returns>
        Task<IBaseResult<IEnumerable<MessageDto>>> GetUnreadMessagesAsync(string receiverId);

        /// <summary>
        /// Retrieves a list of unsent messages for a specific receiver.
        /// </summary>
        /// <param name="receiverId">The ID of the receiver.</param>
        /// <returns>A result containing a list of unsent messages.</returns>
        Task<IBaseResult<IEnumerable<MessageDto>>> GetUnSentMessagesAsync(string receiverId);

        /// <summary>
        /// Retrieves a paginated list of received messages for a specific receiver.
        /// </summary>
        /// <param name="receiverId">The ID of the receiver.</param>
        /// <param name="messageListPageParameters">The parameters for pagination and filtering.</param>
        /// <returns>A paginated result containing a list of received messages.</returns>
        Task<PaginatedResult<MessageDto>> GetReceivedMessagesAsync(string receiverId, MessagePageParameters messageListPageParameters);

        /// <summary>
        /// Retrieves a paginated list of sent messages for a specific sender.
        /// </summary>
        /// <param name="senderId">The ID of the sender.</param>
        /// <param name="messageListPageParameters">The parameters for pagination and filtering.</param>
        /// <returns>A paginated result containing a list of sent messages.</returns>
        Task<PaginatedResult<MessageDto>> GetSentMessagesAsync(string senderId, MessagePageParameters messageListPageParameters);

        /// <summary>
        /// Retrieves a message by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the message.</param>
        /// <returns>A result containing the message.</returns>
        Task<IBaseResult<MessageDto>> GetMessageAsync(string id);

        /// <summary>
        /// Retrieves a message by its unique identifier and marks it as read.
        /// </summary>
        /// <param name="id">The unique identifier of the message.</param>
        /// <returns>A result containing the message.</returns>
        Task<IBaseResult<MessageDto>> GetMessageAndMarkAsReadAsync(string id);

        #endregion

        #region Message Management

        /// <summary>
        /// Adds a new message.
        /// </summary>
        /// <param name="message">The message to add.</param>
        /// <returns>A result containing the added message.</returns>
        Task<IBaseResult<MessageDto>> AddMessageAsync(MessageDto message);

        /// <summary>
        /// Updates an existing message.
        /// </summary>
        /// <param name="message">The message to update.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        Task<IBaseResult> UpdateMessageAsync(MessageDto message);

        /// <summary>
        /// Deletes a message by its unique identifier.
        /// </summary>
        /// <param name="messageId">The unique identifier of the message to delete.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        Task<IBaseResult> DeleteMessageAsync(string messageId);

        /// <summary>
        /// Deletes a document link associated with a message.
        /// </summary>
        /// <param name="messageId">The unique identifier of the message.</param>
        /// <param name="documentlink">The document link to delete.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        Task<IBaseResult> DeleteMessageLinkAsync(string messageId, string documentlink);

        #endregion
    }
}

