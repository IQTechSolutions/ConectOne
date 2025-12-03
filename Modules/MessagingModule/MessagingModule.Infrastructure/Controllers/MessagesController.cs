using MessagingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.Interfaces;
using MessagingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessagingModule.Infrastructure.Controllers
{
    /// <summary>
    /// API controller for managing messages.
    /// Provides endpoints for retrieving, creating, updating, and deleting messages.
    /// </summary>
    [Route("api/messages"), ApiController, Authorize(AuthenticationSchemes = "Bearer")]
    public class MessagesController(IMessageService messagingService) : ControllerBase
    {
        #region Message Retrieval

        /// <summary>
        /// Retrieves a paginated list of messages based on the provided parameters.
        /// </summary>
        /// <param name="args">The parameters for pagination and filtering.</param>
        /// <returns>A paginated list of messages.</returns>
        [HttpGet("paged")]  public async Task<IActionResult> PagedMessages([FromQuery] MessagePageParameters args)
        {
            var notifications = await messagingService.PagedMessagesAsync(args);
            return Ok(notifications);
        }

        /// <summary>
        /// Retrieves a paginated list of notification messages based on the provided parameters.
        /// </summary>
        /// <param name="args">The parameters for pagination and filtering.</param>
        /// <returns>A paginated list of notification messages.</returns>
        [HttpGet("bynotification")]  public async Task<IActionResult> PagedNotificationMessages([FromQuery] MessagePageParameters args)
        {
            var notifications = await messagingService.PagedNotificationMessagesAsync(args);
            return Ok(notifications);
        }

        /// <summary>
        /// Retrieves a notification message based on the provided parameters.
        /// </summary>
        /// <param name="args">The parameters for retrieving the notification message.</param>
        /// <returns>The notification message.</returns>
        [HttpGet("bynotification/message")]  public async Task<IActionResult> NotificationMessage([FromQuery] MessagePageParameters args)
        {
            var notifications = await messagingService.NotificationMessageAsync(args);
            return Ok(notifications);
        }

        /// <summary>
        /// Retrieves a list of unread messages for a specific receiver.
        /// </summary>
        /// <param name="receiverId">The ID of the receiver.</param>
        /// <returns>A list of unread messages.</returns>
        [HttpGet("unread/{receiverId}")]  public async Task<IActionResult> GetUnreadMessages(string receiverId)
        {
            var messages = await messagingService.GetUnreadMessagesAsync(receiverId);
            return Ok(messages);
        }

        /// <summary>
        /// Retrieves a list of unsent messages for a specific receiver.
        /// </summary>
        /// <param name="receiverId">The ID of the receiver.</param>
        /// <param name="messagePageListParams">The parameters for pagination and filtering.</param>
        /// <returns>A list of unsent messages.</returns>
        [HttpGet("unsent/{receiverId}")]  public async Task<IActionResult> GetUnsentMessages(string receiverId, [FromQuery] MessagePageParameters messagePageListParams)
        {
            var messages = await messagingService.GetUnSentMessagesAsync(receiverId);
            return Ok(messages);
        }

        /// <summary>
        /// Retrieves a paginated list of received messages for a specific receiver.
        /// </summary>
        /// <param name="receiverId">The ID of the receiver.</param>
        /// <param name="messagePageListParams">The parameters for pagination and filtering.</param>
        /// <returns>A paginated list of received messages.</returns>
        [HttpGet("received/{receiverId}")]  public async Task<IActionResult> GetReceivedMessages(string receiverId, [FromQuery] MessagePageParameters messagePageListParams)
        {
            var messages = await messagingService.GetReceivedMessagesAsync(receiverId, messagePageListParams);
            return Ok(messages);
        }

        /// <summary>
        /// Retrieves a paginated list of sent messages for a specific sender.
        /// </summary>
        /// <param name="senderId">The ID of the sender.</param>
        /// <param name="messagePageListParams">The parameters for pagination and filtering.</param>
        /// <returns>A paginated list of sent messages.</returns>
        [HttpGet("sent/{senderId}")] public async Task<IActionResult> GetSentMessages(string senderId, [FromQuery] MessagePageParameters messagePageListParams)
        {
            var messages = await messagingService.GetSentMessagesAsync(senderId, messagePageListParams);
            return Ok(messages);
        }

        /// <summary>
        /// Retrieves a message by its unique identifier.
        /// </summary>
        /// <param name="messageId">The unique identifier of the message.</param>
        /// <returns>The message with the specified ID.</returns>
        [HttpGet("{messageId}")] public async Task<IActionResult> GetMessage(string messageId)
        {
            var message = await messagingService.GetMessageAsync(messageId);
            return Ok(message);
        }

        /// <summary>
        /// Retrieves a message by its unique identifier and marks it as read.
        /// </summary>
        /// <param name="messageId">The unique identifier of the message.</param>
        /// <returns>The message with the specified ID.</returns>
        [HttpGet("{messageId}/markasread")] public async Task<IActionResult> MarkAsMessage(string messageId)
        {
            var message = await messagingService.GetMessageAndMarkAsReadAsync(messageId);
            return Ok(message);
        }

        #endregion

        #region Message Management

        /// <summary>
        /// Creates a new message.
        /// </summary>
        /// <param name="messageDto">The message to create.</param>
        /// <returns>The created message.</returns>
        [HttpPut("createMessage")] public async Task<IActionResult> CreateMessage([FromBody] MessageDto messageDto)
        {
            var messageResult = await messagingService.AddMessageAsync(messageDto);
            return Ok(messageResult);
        }

        /// <summary>
        /// Updates an existing message.
        /// </summary>
        /// <param name="messageDto">The message to update.</param>
        /// <returns>The updated message.</returns>
        [HttpPost("updateMessage")] public async Task<IActionResult> UpdateMessage([FromBody] MessageDto messageDto)
        {
            var messageResult = await messagingService.UpdateMessageAsync(messageDto);
            return Ok(messageResult);
        }

        /// <summary>
        /// Deletes a message by its unique identifier.
        /// </summary>
        /// <param name="messageId">The unique identifier of the message to delete.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        [HttpDelete("deleteMessage/{messageId}")] public async Task<IActionResult> DeleteMessage(string messageId)
        {
            var messageResult = await messagingService.DeleteMessageAsync(messageId);
            return Ok(messageResult);
        }

        /// <summary>
        /// Deletes a document link associated with a message.
        /// </summary>
        /// <param name="messageId">The unique identifier of the message.</param>
        /// <param name="documentlink">The document link to delete.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        [HttpDelete("{messageId}/documents/{documentId}")] public async Task<IActionResult> DeleteMessageLinkAsync(string messageId, string documentlink)
        {
            var messageResult = await messagingService.DeleteMessageLinkAsync(messageId, documentlink);
            return Ok(messageResult);
        }

        #endregion
    }
}
