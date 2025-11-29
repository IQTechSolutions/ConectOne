using ConectOne.Domain.Extensions;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using FilingModule.Domain.Entities;
using MessagingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.Interfaces;
using MessagingModule.Domain.RequestFeatures;
using MessagingModule.Domain.Specifications;
using Microsoft.EntityFrameworkCore;
using Message = MessagingModule.Domain.Entities.Message;
using Notification = MessagingModule.Domain.Entities.Notification;

namespace MessagingModule.Infrastructure.Implementation
{
    /// <summary>
    /// Service implementation for managing messages and notifications.
    /// </summary>
    public sealed class MessageService : IMessageService
    {
        private readonly IRepository<Message, string> _messagingRepo;
        private readonly IRepository<Notification, string> _notificationsRepo;
        private readonly IRepository<EntityDocument<Message, string>, string> _documentFileRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageService"/> class.
        /// </summary>
        /// <param name="messagingRepo">The repository for messages.</param>
        /// <param name="notificationsRepo">The repository for notifications.</param>
        /// <param name="documentFileRepo">The repository for document files.</param>
        public MessageService(IRepository<Message, string> messagingRepo, IRepository<Notification, string> notificationsRepo, IRepository<EntityDocument<Message, string>, string> documentFileRepo)
        {
            _messagingRepo = messagingRepo;
            _notificationsRepo = notificationsRepo;
            _documentFileRepo = documentFileRepo;
        }

        #region Public Methods

        /// <summary>
        /// Retrieves a paginated list of messages based on the specified parameters.
        /// </summary>
        /// <param name="args">The parameters for pagination and filtering.</param>
        /// <returns>A paginated result containing the messages.</returns>
        public async Task<PaginatedResult<MessageDto>> PagedMessagesAsync(MessagePageParameters args)
        {
            var listSpec = new MessageListSpecification(args);
            var countSpec = new MessageListSpecification(args, applyPaging: false);

            var listResult = await _messagingRepo.ListAsync(listSpec, false);
            if (!listResult.Succeeded)
                return PaginatedResult<MessageDto>.Failure(listResult.Messages);

            var countResult = await _messagingRepo.CountAsync(countSpec);
            if (!countResult.Succeeded)
                return PaginatedResult<MessageDto>.Failure(countResult.Messages);

            var response = listResult.Data.Select(m => new MessageDto(m)).ToList();

            return PaginatedResult<MessageDto>.Success(response, countResult.Data, args.PageNr, args.PageSize);
        }

        /// <summary>
        /// Retrieves a paginated list of notification messages based on the specified parameters.
        /// </summary>
        /// <param name="args">The parameters for pagination and filtering.</param>
        /// <returns>A paginated result containing the notification messages.</returns>
        public async Task<PaginatedResult<MessageDto>> PagedNotificationMessagesAsync(MessagePageParameters args)
        {
            var result = _notificationsRepo.FindByCondition(c => c.ReceiverId.Equals(args.ReceiverId), false);
            if (result.Succeeded)
            {
                //var data = await result.Data.OrderByDescending(c => c.CreatedOn).Include(c => c.Message)
                //    .Where(c => c.Message!.MessageType == args.MessageType)
                //    .ToListAsync();

                //var response = data.Select(message => new MessageDto(message) { NotificationId = message.Id, ReceiverId = message.ReceiverId});

                //return PaginatedResult<MessageDto>.Success(response.ToList(), response.Count(), args.PageNr, args.PageSize);

                return PaginatedResult<MessageDto>.Success(new List<MessageDto>(), 0, args.PageNr, args.PageSize);
            }
            return PaginatedResult<MessageDto>.Failure(result.Messages);
        }

        /// <summary>
        /// Retrieves a specific notification message based on the specified parameters.
        /// </summary>
        /// <param name="args">The parameters for retrieving the notification message.</param>
        /// <returns>A result containing the notification message.</returns>
        public async Task<IBaseResult<MessageDto>> NotificationMessageAsync(MessagePageParameters args)
        {
            var result = _notificationsRepo.FindByCondition(c => c.Id.Equals(args.NotificationId), false);

            if (!result.Succeeded) return await Result<MessageDto>.FailAsync(result.Messages);

            var data = await result.Data.FirstOrDefaultAsync();

            if (data is null)
                return await Result<MessageDto>.FailAsync($"No notification with id matching '{args.NotificationId}' was found in the database");

            data.OpenedDate = DateTime.Now;
            _notificationsRepo.Update(data);

            var saveNotificationResponse = await _notificationsRepo.SaveAsync();
            if (!saveNotificationResponse.Succeeded)
                return await Result<MessageDto>.FailAsync(saveNotificationResponse.Messages);

            var response = new MessageDto();

            //if (messageResult.Data.Any())
            //{
            //    var message = await messageResult.Data.FirstOrDefaultAsync();
            //    message.ReadTime = DateTime.Now;

            //    var saveMessageResponse = await messagingRepo.SaveAsync();
            //    if (!saveMessageResponse.Succeeded)
            //        return await Result<MessageDto>.FailAsync(result.Messages);

            //    response = new MessageDto(message)
            //    {
            //        NotificationId = data.Id,
            //        CreatedTime = message.CreatedOn,
            //        ReceiverId = data.ReceiverId,
            //        ReceiverName = data.ReceiverName,
            //        DeliveredTime = message.CreatedOn,
            //        ReadTime = message.ReadTime
            //    };
            //}

            return await Result<MessageDto>.SuccessAsync(response);
        }

        /// <summary>
        /// Retrieves a list of unread messages for a specific receiver.
        /// </summary>
        /// <param name="receiverId">The ID of the receiver.</param>
        /// <returns>A result containing the unread messages.</returns>
        public async Task<IBaseResult<IEnumerable<MessageDto>>> GetUnreadMessagesAsync(string receiverId)
        {
            var spec = new LambdaSpec<Message>(c => c.ReceiverId == receiverId && !c.Read);
            var result = await _messagingRepo.ListAsync(spec, false);
            if (!result.Succeeded)
                return await Result<IEnumerable<MessageDto>>.FailAsync(result.Messages);

            return Result<IEnumerable<MessageDto>>.Success(result.Data.Select(message => new MessageDto(message)
            {
                MessageReceivedTimeString = message.DeliveredTime!.Value.DisplayDateTimeIntervalString(),
                MessageModifiedTimeString = message.LastModifiedOn!.Value.DisplayDateTimeIntervalString(),
                MessageReadTimeString = message.ReadTime!.Value.DisplayDateTimeIntervalString()
            }));
        }

        /// <summary>
        /// Retrieves a list of unsent messages for a specific receiver.
        /// </summary>
        /// <param name="receiverId">The ID of the receiver.</param>
        /// <returns>A result containing the unsent messages.</returns>
        public async Task<IBaseResult<IEnumerable<MessageDto>>> GetUnSentMessagesAsync(string receiverId)
        {
            var spec = new LambdaSpec<Message>(c => c.ReceiverId == receiverId && !c.Delivered);
            var result = await _messagingRepo.ListAsync(spec, false);
            if (!result.Succeeded)
                return await Result<IEnumerable<MessageDto>>.FailAsync(result.Messages);

            return Result<IEnumerable<MessageDto>>.Success(result.Data.Select(message => new MessageDto(message)
            {
                MessageReceivedTimeString = message.DeliveredTime!.Value.DisplayDateTimeIntervalString(),
                MessageModifiedTimeString = message.LastModifiedOn!.Value.DisplayDateTimeIntervalString(),
                MessageReadTimeString = message.ReadTime!.Value.DisplayDateTimeIntervalString()
            }));
        }

        /// <summary>
        /// Retrieves a paginated list of received messages for a specific receiver based on the specified parameters.
        /// </summary>
        /// <param name="receiverId">The ID of the receiver.</param>
        /// <param name="messageListPageParameters">The parameters for pagination and filtering.</param>
        /// <returns>A paginated result containing the received messages.</returns>
        public async Task<PaginatedResult<MessageDto>> GetReceivedMessagesAsync(string receiverId, MessagePageParameters messageListPageParameters)
        {
            var listSpec = new MessageListSpecification(messageListPageParameters, receiverId: receiverId);
            var countSpec = new MessageListSpecification(messageListPageParameters, receiverId: receiverId, applyPaging: false);

            var listResult = await _messagingRepo.ListAsync(listSpec, false);
            if (!listResult.Succeeded)
                return PaginatedResult<MessageDto>.Failure(listResult.Messages);

            var countResult = await _messagingRepo.CountAsync(countSpec);
            if (!countResult.Succeeded)
                return PaginatedResult<MessageDto>.Failure(countResult.Messages);

            return PaginatedResult<MessageDto>.Success(listResult.Data.Select(message => new MessageDto(message)
            {
                MessageReceivedTimeString = message.DeliveredTime!.Value.DisplayDateTimeIntervalString(),
                MessageModifiedTimeString = message.LastModifiedOn!.Value.DisplayDateTimeIntervalString(),
                MessageReadTimeString = message.ReadTime!.Value.DisplayDateTimeIntervalString()
            }).ToList(), countResult.Data, messageListPageParameters.PageNr, messageListPageParameters.PageSize);
        }

        /// <summary>
        /// Retrieves a paginated list of sent messages for a specific sender based on the specified parameters.
        /// </summary>
        /// <param name="senderId">The ID of the sender.</param>
        /// <param name="messageListPageParameters">The parameters for pagination and filtering.</param>
        /// <returns>A paginated result containing the sent messages.</returns>
        public async Task<PaginatedResult<MessageDto>> GetSentMessagesAsync(string senderId, MessagePageParameters messageListPageParameters)
        {
            var listSpec = new MessageListSpecification(messageListPageParameters, senderId: senderId);
            var countSpec = new MessageListSpecification(messageListPageParameters, senderId: senderId, applyPaging: false);

            var listResult = await _messagingRepo.ListAsync(listSpec, false);
            if (!listResult.Succeeded)
                return PaginatedResult<MessageDto>.Failure(listResult.Messages);

            var countResult = await _messagingRepo.CountAsync(countSpec);
            if (!countResult.Succeeded)
                return PaginatedResult<MessageDto>.Failure(countResult.Messages);

            return PaginatedResult<MessageDto>.Success(listResult.Data.Select(message => new MessageDto(message)
            {
                MessageReceivedTimeString = message.DeliveredTime!.Value.DisplayDateTimeIntervalString(),
                MessageModifiedTimeString = message.LastModifiedOn!.Value.DisplayDateTimeIntervalString(),
                MessageReadTimeString = message.ReadTime!.Value.DisplayDateTimeIntervalString()
            }).ToList(), countResult.Data, messageListPageParameters.PageNr, messageListPageParameters.PageSize);
        }

        /// <summary>
        /// Retrieves a specific message by its ID.
        /// </summary>
        /// <param name="id">The ID of the message.</param>
        /// <returns>A result containing the message.</returns>
        public async Task<IBaseResult<MessageDto>> GetMessageAsync(string id)
        {
            try
            {
                var spec = new MessageByIdSpecification(id, includeDocuments: true);
                var result = await _messagingRepo.FirstOrDefaultAsync(spec, false);
                if (!result.Succeeded || result.Data is null)
                    return await Result<MessageDto>.FailAsync(result.Messages.Any() ? result.Messages : [$"No message with id matching '{id}' was found in the database"]);

                var notificationsResult = _notificationsRepo.FindByCondition(c => c.EntityId!.Equals(id), true);
                if (notificationsResult.Succeeded)
                {
                    foreach (var notification in notificationsResult.Data)
                    {
                        notification.OpenedDate = DateTime.Now;
                        _notificationsRepo.Update(notification);
                    }
                }

                await _notificationsRepo.SaveAsync();

                var message = result.Data;
                return await Result<MessageDto>.SuccessAsync(new MessageDto(message)
                {
                    MessageReceivedTimeString = message.DeliveredTime?.DisplayDateTimeIntervalString(),
                    MessageModifiedTimeString = message.LastModifiedOn?.DisplayDateTimeIntervalString(),
                    MessageReadTimeString = message.ReadTime?.DisplayDateTimeIntervalString()
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Retrieves a specific message by its ID and marks it as read.
        /// </summary>
        /// <param name="id">The ID of the message.</param>
        /// <returns>A result containing the message.</returns>
        public async Task<IBaseResult<MessageDto>> GetMessageAndMarkAsReadAsync(string id)
        {
            var spec = new MessageByIdSpecification(id, includeDocuments: true);
            var result = await _messagingRepo.FirstOrDefaultAsync(spec, false);
            if (!result.Succeeded || result.Data is null)
                return await Result<MessageDto>.FailAsync(result.Messages.Any() ? result.Messages : [$"No message with id matching '{id}' was found in the database"]);

            var message = result.Data;
            return await Result<MessageDto>.SuccessAsync(new MessageDto(message)
            {
                MessageReceivedTimeString = message.DeliveredTime?.DisplayDateTimeIntervalString(),
                MessageModifiedTimeString = message.LastModifiedOn?.DisplayDateTimeIntervalString(),
                MessageReadTimeString = message.ReadTime?.DisplayDateTimeIntervalString()
            });
        }

        /// <summary>
        /// Adds a new message.
        /// </summary>
        /// <param name="messageDto">The message to add.</param>
        /// <returns>A result indicating the success or failure of the add operation.</returns>
        public async Task<IBaseResult<MessageDto>> AddMessageAsync(MessageDto messageDto)
        {
            try
            {
                var newMessage = messageDto.CreateMessage();

                foreach (var document in messageDto.Documents)
                {
                    if (!string.IsNullOrEmpty(document.Url) && document.Url.IsBase64String())
                    {

                        var path = Path.Combine("StaticFiles", "messages", "images");
                        var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", path);
                        if (!Directory.Exists(rootPath))
                            Directory.CreateDirectory(rootPath);

                        for (int i = 1; ; ++i)
                        {
                            if (!File.Exists(path))
                                break;
                            document.FileName = document.FileName + "(" + i + ")";
                        }

                        await File.WriteAllBytesAsync(Path.Combine(rootPath, document.FileName),
                            Convert.FromBase64String(document.Url));

                        //var resulteee = new DocumentFile<Message>
                        //{
                        //    Title = document.FileName,
                        //    FileName = document.FileName,
                        //    Description = "Message document file attachment",
                        //    IsPublic = true,
                        //    EntityId = newMessage.Id,
                        //    DisplayName = document.DisplayName,
                        //    ContentType = document.ContentType,
                        //    Size = document.FileSize,
                        //    RelativePath = Path.Combine(path, document.FileName),
                        //    FolderPath = rootPath
                        //};


                        //newMessage.Documents.Add(resulteee);
                    }
                }


                var result = await _messagingRepo.CreateAsync(newMessage);
                if (!result.Succeeded) return await Result<MessageDto>.FailAsync(result.Messages);

                var saveResult = await _messagingRepo.SaveAsync();
                if (!saveResult.Succeeded) return await Result<MessageDto>.FailAsync(saveResult.Messages);


                return await Result<MessageDto>.SuccessAsync(new MessageDto(newMessage));
            }
            catch (Exception e)
            {
                return await Result<MessageDto>.FailAsync(e.Message);
            }
            
        }

        /// <summary>
        /// Updates an existing message.
        /// </summary>
        /// <param name="messageDto">The message to update.</param>
        /// <returns>A result indicating the success or failure of the update operation.</returns>
        public async Task<IBaseResult> UpdateMessageAsync(MessageDto messageDto) 
        {
            var messageResult = _messagingRepo.FindByCondition(c => c.Id == messageDto.MessageId, false);
            if (messageResult.Succeeded)
            {
                var message = await messageResult.Data.FirstOrDefaultAsync();

                message.Subject = messageDto.Subject;
                message.ShortDescription = messageDto.ShortDescription;
                message.Description = messageDto.Message;
                message.DocumentLinks = string.Join(";", messageDto.DocumentLinks);

                _messagingRepo.Update(message);

                foreach (var bannerImage in messageDto.Documents)
                {
                    if (bannerImage.Url.IsBase64String())
                    {
                        var path = Path.Combine("StaticFiles", "messages", "images");
                        var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", path);
                        if (!Directory.Exists(rootPath))
                            Directory.CreateDirectory(rootPath);

                        for (int i = 1;; ++i)
                        {
                            if (!File.Exists(path))
                                break;
                            bannerImage.FileName = bannerImage.FileName + "(" + i + ")";
                        }

                        await File.WriteAllBytesAsync(Path.Combine(rootPath, bannerImage.FileName), Convert.FromBase64String(bannerImage.Url));

                        //await _documentFileRepo.CreateAsync(
                        //    new DocumentFile<Message>
                        //    {
                        //        Title   = bannerImage.DisplayName,
                        //        FileName = bannerImage.DisplayName,
                        //        Description = "Message document file attachment",
                        //        IsPublic = true,
                        //        EntityId = message.Id,
                        //        DisplayName = bannerImage.DisplayName,
                        //        ContentType = bannerImage.ContentType,
                        //        Size = bannerImage.FileSize,
                        //        RelativePath = rootPath,
                        //        FolderPath = path
                        //    });
                    }
                }

                var notifications = _notificationsRepo.FindByCondition(c => c.MessageId.Equals(message.Id), false);
                if (notifications.Succeeded)
                {
                    foreach (var notification in notifications.Data)
                    {
                        notification.Title = messageDto.Subject;
                        notification.ShortDescription = messageDto.ShortDescription;
                        notification.Description = messageDto.Message;

                        _notificationsRepo.Update(notification);
                    }
                }

                var saveResult = await _messagingRepo.SaveAsync();
                if (saveResult.Succeeded)
                {
                    return await Result<MessageDto>.SuccessAsync("Message successfully updated");
                }
                return await Result<MessageDto>.FailAsync(saveResult.Messages);
            }
            return await Result<MessageDto>.FailAsync(messageResult.Messages);
        }

        /// <summary>
        /// Deletes a message by its ID.
        /// </summary>
        /// <param name="messageId">The ID of the message to delete.</param>
        /// <returns>A result indicating the success or failure of the delete operation.</returns>
        public async Task<IBaseResult> DeleteMessageAsync(string messageId)
        {
            var notificationsResult = _notificationsRepo.FindByCondition(c => c.EntityId == messageId, true);
            if (notificationsResult.Succeeded)
            {
                foreach (var notification in notificationsResult.Data)
                {
                    _notificationsRepo.Delete(notification);
                }
            }

            var messageResult = _messagingRepo.FindByCondition(c => c.Id.Equals(messageId), false);
            if (!messageResult.Succeeded) return await Result.FailAsync(messageResult.Messages);

            var message = await messageResult.Data.Include(c => c.Documents).FirstOrDefaultAsync();
            foreach (var document in message.Documents)
            {
                _documentFileRepo.Delete(document);
            }
            var deleteResult = _messagingRepo.Delete(message);
            if (!deleteResult.Succeeded) return await Result<MessageDto>.FailAsync(deleteResult.Messages);

            var saveResult = await _messagingRepo.SaveAsync();
            if (!saveResult.Succeeded) return await Result<MessageDto>.FailAsync(saveResult.Messages);

            return await Result<MessageDto>.SuccessAsync("Message removed successfully");
        }

        /// <summary>
        /// Deletes a document link from a message by its ID.
        /// </summary>
        /// <param name="messageId">The ID of the message.</param>
        /// <param name="documentLink">The document link to delete.</param>
        /// <returns>A result indicating the success or failure of the delete operation.</returns>
        public async Task<IBaseResult> DeleteMessageLinkAsync(string messageId, string documentLink)
        {
            var documentFileResult = _messagingRepo.FindByCondition(c => c.Id.Equals(messageId), false);
            if (!documentFileResult.Succeeded) return await Result.FailAsync(documentFileResult.Messages);
            var mdda = documentFileResult.Data.FirstOrDefault();

            List<string> documentLinks = mdda.DocumentLinks.Split(",").ToList();
            documentLinks.Remove(documentLink);

            mdda.DocumentLinks = string.Join(",", documentLinks);

            _messagingRepo.Update(mdda);

            var saveResult = await _messagingRepo.SaveAsync();
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result<MessageDto>.SuccessAsync("Document Link Removed Successfully");
        }

        #endregion
    }
}