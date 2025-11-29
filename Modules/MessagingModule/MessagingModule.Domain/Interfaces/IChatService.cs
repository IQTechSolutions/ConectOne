using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.RequestFeatures;
using MessagingModule.Domain.DataTransferObjects;

namespace MessagingModule.Domain.Interfaces
{
    /// <summary>
    /// Defines the contract for chat message operations such as saving, retrieving, and archiving.
    /// </summary>
    public interface IChatService
    {
        /// <summary>
        /// Asynchronously saves a new chat message to the data store.
        /// </summary>
        /// <param name="message">The chat message creation request containing the message content and associated metadata to be saved. Cannot
        /// be null.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the save operation.</param>
        /// <returns>A task that represents the asynchronous save operation. The task result contains an <see
        /// cref="IBaseResult"/> indicating the outcome of the operation.</returns>
        Task<IBaseResult> SaveMessageAsync(ChatMessageDto message, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the most recent messages from a specific chat group.
        /// Archived messages are excluded from the result.
        /// </summary>
        /// <param name="groupId">The ID of the chat group to retrieve messages from.</param>
        /// <param name="maxCount">The maximum number of recent messages to retrieve. Defaults to 50.</param>
        /// <param name="cancellationToken">Optional token to cancel the asynchronous operation.</param>
        /// <returns>
        /// A result containing a list of recent chat messages, or an error message if the operation fails.
        /// </returns>
        Task<IBaseResult<List<ChatMessageDto>>> GetRecentMessagesAsync(string groupId, int maxCount = 50, CancellationToken cancellationToken = default);

        /// <summary>
        /// Marks a chat message as read on the server.
        /// </summary>
        /// <param name="messageId">The ID of the message to mark as read.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        Task<IBaseResult> MarkMessageAsReadAsync(string messageId, CancellationToken cancellationToken = default);

        #region Documents

        /// <summary>
        /// Adds an image to the specified entity.
        /// </summary>
        /// <remarks>This method creates an association between an image and an entity, using the provided
        /// details in the request.  The operation involves creating the image record and saving the changes to the
        /// repository. If either step fails,  the method returns a failure result with the corresponding error
        /// messages.</remarks>
        /// <param name="request">The request containing the image details, including the image ID, entity ID, selector, and order.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
        Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes an image with the specified identifier from the repository.
        /// </summary>
        /// <remarks>This method first attempts to delete the image from the repository. If the deletion
        /// succeeds,  it then saves the changes to the repository. If either operation fails, the method returns a
        /// failure result  with the associated error messages.</remarks>
        /// <param name="imageId">The unique identifier of the image to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation. Defaults to <see
        /// langword="default"/> if not provided.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the success or failure of the operation. If the operation fails, the result includes error
        /// messages.</returns>
        Task<IBaseResult> RemoveImage(string imageId, CancellationToken cancellationToken = default);

        #endregion

        #region Documents

        /// <summary>
        /// Adds a document associated with an entity to the repository.
        /// </summary>
        /// <remarks>This method attempts to add a document to the repository and save the changes. If the
        /// operation fails at any step,  the returned result will contain the failure messages.</remarks>
        /// <param name="request">The request containing the details of the document to add, including the document's ID and the associated
        /// entity's ID.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
        Task<IBaseResult> AddDocument(AddEntityImageRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a document with the specified identifier from the repository.
        /// </summary>
        /// <remarks>This method performs two operations: it deletes the document from the repository and
        /// then saves the changes. If either operation fails, the method returns a failure result with the associated
        /// error messages.</remarks>
        /// <param name="documentId">The unique identifier of the document to be removed. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation. If the operation fails, the result includes error
        /// messages.</returns>
        Task<IBaseResult> RemoveDocument(string documentId, CancellationToken cancellationToken = default);

        #endregion

        #region Videos

        /// <summary>
        /// Adds a video associated with a specific entity.
        /// </summary>
        /// <remarks>This method creates a new video entry in the repository and saves the changes. If the
        /// operation fails at any step, the returned result will contain the failure messages.</remarks>
        /// <param name="request">The request containing the video ID and the associated entity ID.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
        Task<IBaseResult> AddVideo(AddEntityImageRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a video with the specified identifier from the repository.
        /// </summary>
        /// <remarks>This method first attempts to delete the video from the repository. If the deletion
        /// succeeds, it then attempts to save the changes. If either operation fails, the method returns a failure
        /// result with the associated error messages.</remarks>
        /// <param name="videoId">The unique identifier of the video to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation. If the operation fails, the result includes error
        /// messages.</returns>
        Task<IBaseResult> RemoveVideo(string videoId, CancellationToken cancellationToken = default);

        #endregion
    }

}
