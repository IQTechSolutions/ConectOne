using ConectOne.Domain.Extensions;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using MessagingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.Interfaces;
using MessagingModule.Domain.RequestFeatures;

namespace MessagingModule.Application.RestServices
{
    /// <summary>
    /// Provides REST-based operations for managing and retrieving messages, including support for pagination,
    /// notifications, and message state changes.
    /// </summary>
    /// <remarks>This service implements the IMessageService interface and is intended for use in applications
    /// that interact with a remote message API over HTTP. All methods are asynchronous and return results wrapped in
    /// standard result types to indicate success or failure. Thread safety depends on the underlying HTTP provider
    /// implementation.</remarks>
    /// <param name="provider">The HTTP provider used to perform REST API requests for message operations. Must not be null.</param>
    public sealed class MessageRestService(IBaseHttpProvider provider) : IMessageService
    {
        /// <summary>
        /// Retrieves a paginated list of messages based on the specified paging and filtering parameters.
        /// </summary>
        /// <param name="args">The parameters that define paging, sorting, and filtering options for the message query. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a paginated result of message
        /// data transfer objects that match the specified parameters.</returns>
        public async Task<PaginatedResult<MessageDto>> PagedMessagesAsync(MessagePageParameters args)
        {
            var result = await provider.GetPagedAsync<MessageDto, MessagePageParameters>($"messages/paged", args);
            return result;
        }

        /// <summary>
        /// Retrieves a paginated list of notification messages based on the specified paging and filtering parameters.
        /// </summary>
        /// <param name="args">The parameters that define paging, filtering, and sorting options for retrieving notification messages.
        /// Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a paginated result of
        /// notification message data transfer objects that match the specified parameters. The result may be empty if
        /// no messages are found.</returns>
        public async Task<PaginatedResult<MessageDto>> PagedNotificationMessagesAsync(MessagePageParameters args)
        {
            var result = await provider.GetPagedAsync<MessageDto, MessagePageParameters>($"messages/bynotification", args);
            return result;
        }

        /// <summary>
        /// Retrieves a notification message based on the specified message page parameters.
        /// </summary>
        /// <param name="args">The parameters used to filter and identify the notification message to retrieve. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="IBaseResult{MessageDto}"/> with the notification message data if found; otherwise, the result may
        /// indicate an error or that no message was found.</returns>
        public async Task<IBaseResult<MessageDto>> NotificationMessageAsync(MessagePageParameters args)
        {
            var result = await provider.GetAsync<MessageDto>($"messages/bynotification/message/{args.GetQueryString()}");
            return result;
        }

        /// <summary>
        /// Asynchronously retrieves all unread messages for the specified receiver.
        /// </summary>
        /// <param name="receiverId">The unique identifier of the user whose unread messages are to be retrieved. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with a collection of <see cref="MessageDto"/> objects representing the unread messages for the specified
        /// receiver. The collection is empty if there are no unread messages.</returns>
        public async Task<IBaseResult<IEnumerable<MessageDto>>> GetUnreadMessagesAsync(string receiverId)
        {
            var result = await provider.GetAsync<IEnumerable<MessageDto>>($"messages/unread/{receiverId}");
            return result;
        }

        /// <summary>
        /// Asynchronously retrieves all unsent messages for the specified receiver.
        /// </summary>
        /// <param name="receiverId">The unique identifier of the receiver whose unsent messages are to be retrieved. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with a collection of <see cref="MessageDto"/> objects representing the unsent messages for the specified
        /// receiver. The collection will be empty if there are no unsent messages.</returns>
        public async Task<IBaseResult<IEnumerable<MessageDto>>> GetUnSentMessagesAsync(string receiverId)
        {
            var result = await provider.GetAsync<IEnumerable<MessageDto>>($"messages/unsent/{receiverId}");
            return result;
        }

        /// <summary>
        /// Asynchronously retrieves a paginated list of messages received by the specified user.
        /// </summary>
        /// <param name="receiverId">The unique identifier of the user whose received messages are to be retrieved. Cannot be null or empty.</param>
        /// <param name="messageListPageParameters">The pagination and filtering parameters to apply when retrieving the messages. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a paginated result of received
        /// messages for the specified user. The result may be empty if no messages are found.</returns>
        public async Task<PaginatedResult<MessageDto>> GetReceivedMessagesAsync(string receiverId, MessagePageParameters messageListPageParameters)
        {
            var result = await provider.GetPagedAsync<MessageDto, MessagePageParameters>($"messages/received/{receiverId}", messageListPageParameters);
            return result;
        }

        /// <summary>
        /// Retrieves a paginated list of messages sent by the specified sender.
        /// </summary>
        /// <param name="senderId">The unique identifier of the user whose sent messages are to be retrieved. Cannot be null or empty.</param>
        /// <param name="messageListPageParameters">The pagination and filtering parameters to apply to the sent messages list. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a paginated result of sent
        /// messages as <see cref="MessageDto"/> objects. The result may be empty if no messages are found.</returns>
        public async Task<PaginatedResult<MessageDto>> GetSentMessagesAsync(string senderId, MessagePageParameters messageListPageParameters)
        {
            var result = await provider.GetPagedAsync<MessageDto, MessagePageParameters>($"messages/sent/{senderId}", messageListPageParameters);
            return result;
        }

        /// <summary>
        /// Asynchronously retrieves a message by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the message to retrieve. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="IBaseResult{MessageDto}"/> with the requested message data if found; otherwise, an appropriate result
        /// indicating the outcome.</returns>
        public async Task<IBaseResult<MessageDto>> GetMessageAsync(string id)
        {
            var result = await provider.GetAsync<MessageDto>($"messages/{id}");
            return result;
        }

        /// <summary>
        /// Retrieves the specified message and marks it as read asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the message to retrieve and mark as read. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="IBaseResult{MessageDto}"/> with the message data if found; otherwise, an appropriate result indicating
        /// the outcome.</returns>
        public async Task<IBaseResult<MessageDto>> GetMessageAndMarkAsReadAsync(string id)
        {
            var result = await provider.GetAsync<MessageDto>($"messages/{id}/markasread");
            return result;
        }

        /// <summary>
        /// Creates a new message asynchronously using the specified message data.
        /// </summary>
        /// <param name="messageDto">The message data to create. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="IBaseResult{MessageDto}"/> with the created message data if successful.</returns>
        public async Task<IBaseResult<MessageDto>> AddMessageAsync(MessageDto messageDto)
        {
            var result = await provider.PutAsync<MessageDto, MessageDto>($"messages/createMessage", messageDto);
            return result;
        }

        /// <summary>
        /// Updates an existing message asynchronously using the specified message data.
        /// </summary>
        /// <param name="messageDto">The message data to update. Must not be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateMessageAsync(MessageDto messageDto) 
        {
            var result = await provider.PostAsync($"messages/updateMessage", messageDto);
            return result;
        }

        /// <summary>
        /// Asynchronously deletes the message with the specified identifier.
        /// </summary>
        /// <param name="messageId">The unique identifier of the message to delete. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous delete operation. The task result contains an object indicating the
        /// outcome of the delete request.</returns>
        public async Task<IBaseResult> DeleteMessageAsync(string messageId)
        {
            var result = await provider.DeleteAsync($"messages/deleteMessage", messageId);
            return result;
        }

        /// <summary>
        /// Asynchronously deletes the specified document link associated with a message.
        /// </summary>
        /// <param name="messageId">The unique identifier of the message from which the document link will be deleted. Cannot be null or empty.</param>
        /// <param name="documentLink">The document link to delete from the specified message. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the delete operation.</returns>
        public async Task<IBaseResult> DeleteMessageLinkAsync(string messageId, string documentLink)
        {
            var result = await provider.DeleteAsync($"messages/{messageId}/documents", documentLink);
            return result;
        }
    }
}