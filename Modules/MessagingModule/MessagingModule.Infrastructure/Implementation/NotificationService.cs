using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using MessagingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.Interfaces;
using MessagingModule.Domain.RequestFeatures;
using MessagingModule.Domain.Specifications;
using Notification = MessagingModule.Domain.Entities.Notification;

namespace MessagingModule.Infrastructure.Implementation
{
    /// <summary>
    /// The NotificationService handles CRUD and retrieval operations related to 
    /// notifications (e.g., fetching paginated results, reading/unreading, 
    /// marking as sent). It integrates with the underlying notification repository 
    /// and transforms data into domain-appropriate DTOs. 
    /// </summary>
    public sealed class NotificationService(IRepository<Notification, string> notificationRepo) : INotificationService
    {
        /// <summary>
        /// Retrieves a paginated list of notifications based on the provided 
        /// <see cref="NotificationPageParameters"/>. Supports filtering by 
        /// receiver ID and/or message type.
        /// </summary>
        /// <param name="args">Paging and filtering parameters for notifications.</param>
        /// <returns>A <see cref="NotificationDto"/> of <see cref="PaginatedResult{T}"/> objects.</returns>
        public async Task<PaginatedResult<NotificationDto>> PagedNotificationsAsync(NotificationPageParameters args)
        {
            var pageSpec = new PagedNotificationsSpecification(args);

            var listResult = await notificationRepo.ListAsync(pageSpec, false);
            if (!listResult.Succeeded)
                return PaginatedResult<NotificationDto>.Failure(listResult.Messages);

            var notifications = listResult.Data.Select(n => new NotificationDto(n)).OrderByDescending(c => c.Created).ToList();

            return PaginatedResult<NotificationDto>.Success(
                notifications,
                notifications.Count,
                args.PageNr,
                args.PageSize
            );
        }

        /// <summary>
        /// Retrieves all unread notifications for a given user (via <see cref="ReceiverId"/>).
        /// </summary>
        /// <param name="args">Contains the receiver ID to filter and optional message type.</param>
        /// <returns>A successful or failed <see cref="IBaseResult{T}"/> of unread <see cref="NotificationDto"/> objects.</returns>
        public async Task<IBaseResult<IEnumerable<NotificationDto>>> UnreadNotificationsAsync(NotificationPageParameters args)
        {
            var spec = new UnreadNotificationsSpecification(args);
            var result = await notificationRepo.ListAsync(spec, false);
            if (!result.Succeeded)
                return await Result<IEnumerable<NotificationDto>>.FailAsync(result.Messages);

            return await Result<IEnumerable<NotificationDto>>.SuccessAsync(
                result.Data.Select(notification => new NotificationDto(notification))
            );
        }

        /// <summary>
        /// Retrieves the count of unread notifications for a given user, with an optional message type filter.
        /// </summary>
        /// <param name="args">The user’s receiver ID and an optional message type.</param>
        /// <returns>A successful or failed <see cref="IBaseResult{T}"/> containing the count of unread notifications.</returns>
        public async Task<IBaseResult<int>> UnreadNotificationCountAsync(NotificationPageParameters args)
        {
            var spec = new UnreadNotificationsSpecification(args);
            var countResult = await notificationRepo.CountAsync(spec);
            return countResult.Succeeded
                ? await Result<int>.SuccessAsync(countResult.Data)
                : await Result<int>.FailAsync(countResult.Messages);
        }

        /// <summary>
        /// Retrieves a specific notification by its ID, marking it as opened/read if found.
        /// </summary>
        /// <param name="notificationId">The ID of the notification to retrieve.</param>
        /// <returns>
        /// A <see cref="IBaseResult{NotificationDto}"/> with the details of the notification,
        /// or an error if not found.
        /// </returns>
        public async Task<IBaseResult<NotificationDto>> NotificationAsync(string notificationId)
        {
            var spec = new NotificationByIdSpecification(notificationId);
            var result = await notificationRepo.FirstOrDefaultAsync(spec, false);
            if (!result.Succeeded)
                return await Result<NotificationDto>.FailAsync(result.Messages);

            var response = result.Data;
            if (response is null)
                return await Result<NotificationDto>.FailAsync($"No notification with id matching '{notificationId}' was found in the database");

            // Mark the notification as read.
            response.OpenedDate = DateTime.UtcNow;
            notificationRepo.Update(response);

            var saveNotificationResponse = await notificationRepo.SaveAsync();
            if (!saveNotificationResponse.Succeeded)
                return await Result<NotificationDto>.FailAsync(saveNotificationResponse.Messages);

            return await Result<NotificationDto>.SuccessAsync(new NotificationDto(response));
        }

        /// <summary>
        /// Marks a specific notification as read (updating the <see cref="OpenedDate"/>).
        /// </summary>
        /// <param name="notificationId">The ID of the notification to mark as read.</param>
        /// <returns>
        /// A <see cref="IBaseResult{NotificationDto}"/> with the updated notification or an error if failed.
        /// </returns>
        public async Task<IBaseResult<NotificationDto>> MarkAsReadAsync(string notificationId)
        {
            var spec = new NotificationByIdSpecification(notificationId);
            var result = await notificationRepo.FirstOrDefaultAsync(spec, true);
            if (!result.Succeeded)
                return await Result<NotificationDto>.FailAsync(result.Messages);

            var response = result.Data;
            if (response is null)
                return await Result<NotificationDto>.FailAsync($"No notification with id matching '{notificationId}' was found in the database");

            // Mark as read
            response.OpenedDate = DateTime.UtcNow;
            notificationRepo.Update(response);

            var saveResult = await notificationRepo.SaveAsync();
            if (!saveResult.Succeeded)
                return await Result<NotificationDto>.FailAsync(saveResult.Messages);

            return await Result<NotificationDto>.SuccessAsync(new NotificationDto(response));
        }

        /// <summary>
        /// Adds a new notification to the system, converting from <see cref="NotificationDto"/>
        /// to the <see cref="Notification"/> entity.
        /// </summary>
        /// <param name="message">The notification data.</param>
        /// <returns>
        /// A <see cref="IBaseResult{NotificationDto}"/> with the created notification 
        /// or an error message if creation failed.
        /// </returns>
        public async Task<IBaseResult<NotificationDto>> AddNotificationAsync(NotificationDto? message)
        {
            if (message == null)
                return await Result<NotificationDto>.FailAsync("Message cannot be null");

            var result = await notificationRepo.CreateAsync(message.ToNotification());
            if (!result.Succeeded) return await Result<NotificationDto>.FailAsync(result.Messages);

            var saveResult = await notificationRepo.SaveAsync();
            if (!saveResult.Succeeded) return await Result<NotificationDto>.FailAsync(saveResult.Messages);

            return await Result<NotificationDto>.SuccessAsync(new NotificationDto(result.Data));
        }

        /// <summary>
        /// Removes a notification by its ID from the database.
        /// </summary>
        /// <param name="notificationId">ID of the notification to remove.</param>
        /// <returns>
        /// A <see cref="IBaseResult"/> indicating success or failure of the operation.
        /// </returns>
        public async Task<IBaseResult> RemoveNotificationAsync(string notificationId)
        {
            var result = await notificationRepo.DeleteAsync(notificationId);
            if (!result.Succeeded) return await Result<NotificationDto>.FailAsync(result.Messages);

            var saveResult = await notificationRepo.SaveAsync();
            if (!saveResult.Succeeded) return await Result<NotificationDto>.FailAsync(saveResult.Messages);

            return await Result<NotificationDto>.SuccessAsync("Notification was successfully removed");
        }
    }
}
