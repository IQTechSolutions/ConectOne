using ConectOne.Domain.Extensions;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using FilingModule.Domain.Entities;
using FilingModule.Domain.Enums;
using FilingModule.Domain.RequestFeatures;
using IdentityModule.Domain.DataTransferObjects;
using MessagingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.Entities;
using MessagingModule.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using NeuralTech.Base.Enums;

namespace MessagingModule.Infrastructure.Implementation
{
    /// <summary>
    /// Provides operations for managing chat messages, including saving, retrieving, and archiving.
    /// </summary>
    /// <param name="chatMessageRepo">Repository for accessing <see cref="ChatMessage"/> entities.</param>
    public class ChatService(IRepository<ChatMessage, string> chatMessageRepo, IRepository<ChatGroupMember, string> chatGroupMemberRepo, IPushNotificationService pushNotificationService, 
        IRepository<EntityImage<ChatMessage, string>, string> imageRepository, IRepository<EntityDocument<ChatMessage, string>, string> documentRepository, 
        IRepository<EntityVideo<ChatMessage, string>, string> videoRepository) : IChatService
    {
        /// <summary>
        /// Archives messages that are older than one year and are not already archived.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task ArchiveOldMessagesAsync()
        {
            var cutoff = DateTime.UtcNow.AddYears(-1);
            var spec = new LambdaSpec<ChatMessage>(m => m.Timestamp < cutoff && !m.IsArchived);

            var messages = await chatMessageRepo.ListAsync(spec, false);

            foreach (var msg in messages.Data)
                msg.IsArchived = true;

            await chatMessageRepo.SaveAsync();
        }

        /// <summary>
        /// Saves a chat message along with its associated images and files asynchronously.
        /// </summary>
        /// <remarks>This method processes the provided images and files by decoding their base64 content,
        /// saving them to the appropriate file system paths, and associating them with the chat message.  The message
        /// and its related data are then persisted to the repository.</remarks>
        /// <param name="message">The <see cref="ChatMessageCreationRequest"/> containing the details of the message to be saved,  including
        /// its text, associated images, and files.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        /// <returns>An <see cref="IBaseResult"/> indicating the success or failure of the operation.  If successful, the result
        /// contains a success message; otherwise, it contains error messages.</returns>
        public async Task<IBaseResult> SaveMessageAsync(ChatMessageDto message, CancellationToken cancellationToken = default)
        {
            var newMessage = new ChatMessage(message);

            await chatMessageRepo.CreateAsync(newMessage, cancellationToken);

            var result = await chatMessageRepo.SaveAsync(cancellationToken);
            if (!result.Succeeded) return await Result.FailAsync(result.Messages);

            var chatMemberSpec = new LambdaSpec<ChatGroupMember>(c => c.GroupId == message.ChatGroupId && c.UserId != message.SenderId);
            chatMemberSpec.AddInclude(c => c.Include(u => u.User).ThenInclude(c => c.UserInfo.Images));

            var chatMemberResult = await chatGroupMemberRepo.ListAsync(chatMemberSpec, false, cancellationToken);
            if (!chatMemberResult.Succeeded || chatMemberResult.Data == null || !chatMemberResult.Data.Any()) return await Result.FailAsync("No chat members found for notification.");

            var recipientList = chatMemberResult.Data.Select(c => new RecipientDto(c.Id, c.User.UserInfo.FirstName, c.User.UserInfo.LastName, new List<string>(), true, false, c.User.UserInfo.Images.FirstOrDefault(c => c.Image.ImageType == UploadType.Cover)?.Image.RelativePath ?? "_content/FilingModule.Blazor/images/profileImage128x128.png", MessageType.Learner)).ToList();
            var notification = new NotificationDto
            {
                EntityId = newMessage.Id,
                Title = $"You received a message from {newMessage.SenderDisplayName}",
                ShortDescription = @newMessage.Content.TruncateLongString(25),
                Message = @newMessage.Content,
                MessageType = MessageType.Parent,
                Created = DateTime.Now,
                NotificationUrl = $"/chats/{newMessage.ChatGroupId}"
            };

            await pushNotificationService.EnqueueNotificationsAsync(recipientList, notification);

            return await Result.SuccessAsync("Message saved successfully.");
        }

        /// <summary>
        /// Retrieves the most recent messages from a specified chat group, excluding archived ones.
        /// </summary>
        /// <param name="groupId">The ID of the chat group.</param>
        /// <param name="maxCount">The maximum number of messages to retrieve. Defaults to 50.</param>
        /// <param name="cancellation">Optional cancellation token.</param>
        /// <returns>
        /// A result containing a list of recent <see cref="ChatMessageDto"/> instances, or an error message.
        /// </returns>
        public async Task<IBaseResult<List<ChatMessageDto>>> GetRecentMessagesAsync(string groupId, int maxCount = 50, CancellationToken cancellation = default)
        {
            var spec = new LambdaSpec<ChatMessage>(m => m.ChatGroupId == groupId && !m.IsArchived);
            spec.AddInclude(c => c.Include(g => g.Images).ThenInclude(c => c.Image));
            spec.AddInclude(c => c.Include(g => g.Documents).ThenInclude(c => c.Document));
            spec.AddInclude(c => c.Include(g => g.Videos).ThenInclude(c => c.Video));

            var chatMessageResult = await chatMessageRepo.ListAsync(spec, false);

            if (!chatMessageResult.Succeeded)
                return await Result<List<ChatMessageDto>>.FailAsync(chatMessageResult.Messages);

            if (chatMessageResult.Data == null || !chatMessageResult.Data.Any())
                return await Result<List<ChatMessageDto>>.SuccessAsync(new List<ChatMessageDto>());

            var messages = chatMessageResult.Data.OrderByDescending(m => m.Timestamp).Take(maxCount).OrderBy(m => m.Timestamp); // oldest first for chat display

            return await Result<List<ChatMessageDto>>.SuccessAsync(messages.Select(m => new ChatMessageDto(m)).ToList());
        }

        /// <summary>
        /// Marks the specified message as read.
        /// </summary>
        public async Task<IBaseResult> MarkMessageAsReadAsync(string messageId, CancellationToken cancellationToken = default)
        {
            var messageResult = await chatMessageRepo.FirstOrDefaultAsync(new LambdaSpec<ChatMessage>(c => c.Id == messageId), cancellationToken: cancellationToken);
            if (!messageResult.Succeeded || messageResult.Data == null)
                return await Result.FailAsync(messageResult.Messages);

            var msg = messageResult.Data;
            msg.IsRead = true;
            msg.ReadTime = DateTime.UtcNow;

            var saveResult = await chatMessageRepo.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded)
                return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync();
        }
        
        #region Images

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
        public async Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default)
        {
            var image = new EntityImage<ChatMessage, string>(request.ImageId, request.EntityId) { Selector = request.Selector, Order = request.Order };

            var addResult = await imageRepository.CreateAsync(image, cancellationToken);
            if (!addResult.Succeeded) return await Result.FailAsync(addResult.Messages);

            var saveResult = await imageRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync();
        }

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
        public async Task<IBaseResult> RemoveImage(string imageId, CancellationToken cancellationToken = default)
        {
            var addResult = await imageRepository.DeleteAsync(imageId, cancellationToken);
            if (!addResult.Succeeded) return await Result.FailAsync(addResult.Messages);

            var saveResult = await imageRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync();
        }

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
        public async Task<IBaseResult> AddDocument(AddEntityImageRequest request, CancellationToken cancellationToken = default)
        {
            var document = new EntityDocument<ChatMessage, string> { DocumentId = request.ImageId, EntityId = request.EntityId };

            var addResult = await documentRepository.CreateAsync(document, cancellationToken);
            if (!addResult.Succeeded) return await Result.FailAsync(addResult.Messages);

            var saveResult = await documentRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync();
        }

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
        public async Task<IBaseResult> RemoveDocument(string documentId, CancellationToken cancellationToken = default)
        {
            var addResult = await documentRepository.DeleteAsync(documentId, cancellationToken);
            if (!addResult.Succeeded) return await Result.FailAsync(addResult.Messages);

            var saveResult = await documentRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync();
        }

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
        public async Task<IBaseResult> AddVideo(AddEntityImageRequest request, CancellationToken cancellationToken = default)
        {
            var video = new EntityVideo<ChatMessage, string> { VideoId = request.ImageId, EntityId = request.EntityId };

            var addResult = await videoRepository.CreateAsync(video, cancellationToken);
            if (!addResult.Succeeded) return await Result.FailAsync(addResult.Messages);

            var saveResult = await videoRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync();
        }

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
        public async Task<IBaseResult> RemoveVideo(string videoId, CancellationToken cancellationToken = default)
        {
            var addResult = await videoRepository.DeleteAsync(videoId, cancellationToken);
            if (!addResult.Succeeded) return await Result.FailAsync(addResult.Messages);

            var saveResult = await videoRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync();
        }

        #endregion
    }
}
