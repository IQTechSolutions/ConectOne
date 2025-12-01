using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.RequestFeatures;
using MessagingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.Interfaces;

namespace MessagingModule.Application.RestServices
{
    /// <summary>
    /// Provides chat-related operations, including message management and media handling, via RESTful HTTP endpoints.
    /// </summary>
    /// <remarks>This service implements chat functionality such as sending messages, retrieving recent
    /// messages, and managing images, documents, and videos associated with chats. All operations are performed
    /// asynchronously and require a valid HTTP provider for communication with the backend API.</remarks>
    /// <param name="provider">The HTTP provider used to perform REST API requests for chat and media operations. Cannot be null.</param>
    public class ChatRestService(IBaseHttpProvider provider) : IChatService
    {
        /// <summary>
        /// Asynchronously saves a new chat message to the data store.
        /// </summary>
        /// <param name="message">The chat message creation request containing the message details to be saved. Cannot be null.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the save operation.</param>
        /// <returns>A task that represents the asynchronous save operation. The task result contains an <see
        /// cref="IBaseResult"/> indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> SaveMessageAsync(ChatMessageDto message, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync($"chat", message);
            return result;
        }

        /// <summary>
        /// Asynchronously retrieves the most recent chat messages for the specified group.
        /// </summary>
        /// <param name="groupId">The unique identifier of the chat group for which to retrieve messages. Cannot be null or empty.</param>
        /// <param name="maxCount">The maximum number of recent messages to retrieve. Must be greater than zero. The default is 50.</param>
        /// <param name="cancellation">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with a list of <see cref="ChatMessageDto"/> objects representing the recent messages. The list may be empty
        /// if there are no messages.</returns>
        public async Task<IBaseResult<List<ChatMessageDto>>> GetRecentMessagesAsync(string groupId, int maxCount = 50, CancellationToken cancellation = default)
        {
            var result = await provider.GetAsync<List<ChatMessageDto>>($"chat/{groupId}");
            return result;
        }

        /// <summary>
        /// Marks the specified message as read asynchronously.
        /// </summary>
        /// <param name="messageId">The unique identifier of the message to mark as read. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an object indicating the outcome
        /// of the operation.</returns>
        public async Task<IBaseResult> MarkMessageAsReadAsync(string messageId, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync("messages/{messageId}/read");
            return result;
        }
        
        #region Images

        /// <summary>
        /// Adds a new image to the system using the specified request data.
        /// </summary>
        /// <param name="request">An object containing the details of the image to add. Cannot be null.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the add image operation.</returns>
        public async Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync("images/addImage", request);
            return result;
        }

        /// <summary>
        /// Deletes the image with the specified identifier.
        /// </summary>
        /// <param name="imageId">The unique identifier of the image to remove. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the delete operation.</returns>
        public async Task<IBaseResult> RemoveImage(string imageId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"images/deleteImage", imageId);
            return result;
        }

        #endregion

        #region Documents

        /// <summary>
        /// Adds a new document image to the system asynchronously.
        /// </summary>
        /// <param name="request">The request containing the document image data and associated metadata to be added. Cannot be null.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the add operation.</returns>
        public async Task<IBaseResult> AddDocument(AddEntityImageRequest request, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync("images/addDocument", request);
            return result;
        }

        /// <summary>
        /// Asynchronously removes the document with the specified identifier.
        /// </summary>
        /// <param name="documentId">The unique identifier of the document to remove. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the removal operation.</returns>
        public async Task<IBaseResult> RemoveDocument(string documentId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"images/deleteDocument", documentId);
            return result;
        }

        #endregion

        #region Videos

        /// <summary>
        /// Adds a new video to the system using the specified request data.
        /// </summary>
        /// <param name="request">An object containing the details of the video to add. Cannot be null.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the add operation.</returns>
        public async Task<IBaseResult> AddVideo(AddEntityImageRequest request, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync("images/addVideo", request);
            return result;
        }

        /// <summary>
        /// Deletes the video associated with the specified video identifier.
        /// </summary>
        /// <param name="videoId">The unique identifier of the video to remove. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the delete operation.</returns>
        public async Task<IBaseResult> RemoveVideo(string videoId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"images/deleteVideo", videoId);
            return result;
        }

        #endregion
    }
}
