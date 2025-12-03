using FilingModule.Domain.RequestFeatures;
using MessagingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessagingModule.Infrastructure.Controllers
{
    /// <summary>
    /// Provides endpoints for managing chat messages, including retrieving recent messages and sending new ones.
    /// </summary>
    /// <remarks>This controller handles chat-related operations such as retrieving the most recent messages
    /// for a group and sending new chat messages. All endpoints require authentication using the Bearer token
    /// scheme.</remarks>
    /// <param name="chatService"></param>
    [Route("api/chats"), ApiController, Authorize(AuthenticationSchemes = "Bearer")]
    public class ChatController(IChatService chatService) : ControllerBase
    {
        /// <summary>
        /// Retrieves the most recent chat messages for a specified group.
        /// </summary>
        /// <remarks>This method retrieves up to 50 of the most recent messages for the specified group. 
        /// If the group does not exist or has no messages, the result will be an empty collection.</remarks>
        /// <param name="groupId">The unique identifier of the group whose messages are to be retrieved. Cannot be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> containing a collection of the most recent chat messages for the specified
        /// group.</returns>
        [HttpGet("messages/{groupId}")] public async Task<IActionResult> Chats(string groupId)
        {
            var result = await chatService.GetRecentMessagesAsync(groupId, 50, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Sends a chat message to the server and saves it asynchronously.
        /// </summary>
        /// <remarks>This method processes the provided chat message and saves it using the chat service. 
        /// The operation respects the cancellation token provided by the HTTP context.</remarks>
        /// <param name="message">The chat message to be sent, including its content and metadata.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation, typically an HTTP 200 response with
        /// the saved message details.</returns>
        [HttpPut] public async Task<IActionResult> SendMessage([FromBody] ChatMessageDto message)
        {
            var result = await chatService.SaveMessageAsync(message, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Marks a message as read.
        /// </summary>
        [HttpPost("messages/{messageId}/read")] public async Task<IActionResult> MarkAsRead(string messageId)
        {
            var result = await chatService.MarkMessageAsReadAsync(messageId, HttpContext.RequestAborted);
            return Ok(result);
        }

        #region Images

        /// <summary>
        /// Adds an image to a vacation entity.
        /// </summary>
        /// <remarks>This method processes the provided image data and associates it with the specified
        /// vacation entity. The operation is performed asynchronously and respects the cancellation token from the HTTP
        /// context.</remarks>
        /// <param name="dto">The request object containing the image data and associated entity information.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPost("addImage")] public async Task<IActionResult> AddVacationImage([FromBody] AddEntityImageRequest dto)
        {
            var result = await chatService.AddImage(dto, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a vacation image with the specified identifier.
        /// </summary>
        /// <remarks>This method removes a vacation image by delegating the operation to the underlying
        /// service.  The operation respects the cancellation token provided by the HTTP request.</remarks>
        /// <param name="imageId">The unique identifier of the image to be deleted. Cannot be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Typically returns an HTTP 200
        /// response with the result of the deletion.</returns>
        [HttpDelete("deleteImage/{imageId}/")] public async Task<IActionResult> RemoveVacationImage(string imageId)
        {
            var result = await chatService.RemoveImage(imageId, HttpContext.RequestAborted);
            return Ok(result);
        }

        #endregion

        #region Documents

        /// <summary>
        /// Adds a vacation document to the system.
        /// </summary>
        /// <remarks>This method processes the provided document details and adds the document
        /// asynchronously. The operation respects the cancellation token provided by the HTTP context.</remarks>
        /// <param name="dto">The request object containing the details of the document to be added.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPost("addDocument")] public async Task<IActionResult> AddVacationDocument([FromBody] AddEntityImageRequest dto)
        {
            var result = await chatService.AddDocument(dto, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a vacation document identified by the specified image ID.
        /// </summary>
        /// <remarks>This method removes a vacation document by delegating the operation to the underlying
        /// chat service. The operation respects the cancellation token provided by the HTTP context.</remarks>
        /// <param name="imageId">The unique identifier of the document to be deleted. Cannot be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns <see langword="Ok"/> with the
        /// result of the deletion.</returns>
        [HttpDelete("deleteDocument/{imageId}/")] public async Task<IActionResult> RemoveVacationDocument(string imageId)
        {
            var result = await chatService.RemoveDocument(imageId, HttpContext.RequestAborted);
            return Ok(result);
        }

        #endregion

        #region Videos

        /// <summary>
        /// Adds a vacation video to the system.
        /// </summary>
        /// <remarks>This method processes the provided video details and adds the video to the system. 
        /// The operation is asynchronous and respects the cancellation token provided by the HTTP context.</remarks>
        /// <param name="dto">The request object containing the video details to be added.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPost("addVideo")] public async Task<IActionResult> AddVacationVideo([FromBody] AddEntityImageRequest dto)
        {
            var result = await chatService.AddVideo(dto, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a vacation video with the specified identifier.
        /// </summary>
        /// <remarks>This method removes a vacation video by its identifier and returns an HTTP 200
        /// response with the result of the operation. If the video cannot be found or the operation fails, the response
        /// may vary depending on the implementation of the underlying service.</remarks>
        /// <param name="imageId">The unique identifier of the video to delete. Cannot be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpDelete("deleteVideo/{imageId}/")] public async Task<IActionResult> RemoveVacationVideo(string imageId)
        {
            var result = await chatService.RemoveVideo(imageId, HttpContext.RequestAborted);
            return Ok(result);
        }

        #endregion
    }
}
