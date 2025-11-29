using ConectOne.Domain.ResultWrappers;
using MessagingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.RequestFeatures;

namespace MessagingModule.Domain.Interfaces
{
    /// <summary>
    /// Defines a contract for managing notifications, including retrieving, adding, updating, and removing
    /// notifications.
    /// </summary>
    /// <remarks>This service provides methods to handle notifications in a paginated manner, retrieve unread
    /// notifications,  mark notifications as read, and perform CRUD operations on individual notifications.  It is
    /// designed to support asynchronous operations for scalability and responsiveness.</remarks>
    public interface INotificationService
    {
        /// <summary>
        /// Retrieves a paginated list of notifications based on the specified parameters.
        /// </summary>
        /// <remarks>Use this method to retrieve notifications in a paginated format, which is useful for
        /// scenarios where the total number of notifications is large and needs to be displayed in smaller
        /// chunks.</remarks>
        /// <param name="args">The parameters that define the pagination and filtering criteria for the notifications. This includes page
        /// size, page number, and any additional filters.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a collection of <see cref="NotificationDto"/> objects and
        /// metadata about the pagination, such as the total number of items and pages.</returns>
        Task<PaginatedResult<NotificationDto>> PagedNotificationsAsync(NotificationPageParameters args);

        /// <summary>
        /// Retrieves a paginated list of unread notifications based on the specified parameters.
        /// </summary>
        /// <param name="args">The parameters for pagination and filtering of unread notifications.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <IBaseResult{T}/> object
        /// with an enumerable collection of <NotificationDto>  representing the unread notifications.</returns>
        Task<IBaseResult<IEnumerable<NotificationDto>>> UnreadNotificationsAsync(NotificationPageParameters args);

        /// <summary>
        /// Asynchronously retrieves the count of unread notifications based on the specified parameters.
        /// </summary>
        /// <param name="args">The parameters used to filter and paginate the unread notifications. Cannot be <see langword="null"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// where <c>T</c> is an <see cref="int"/> representing the count of unread notifications.</returns>
        Task<IBaseResult<int>> UnreadNotificationCountAsync(NotificationPageParameters args);

        /// <summary>
        /// Retrieves the details of a notification based on the specified notification ID.
        /// </summary>
        /// <remarks>Use this method to fetch notification details for a given ID. Ensure the provided ID
        /// is valid and corresponds to an existing notification. The result will include any relevant metadata or error
        /// information.</remarks>
        /// <param name="notificationId">The unique identifier of the notification to retrieve. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the notification details encapsulated in a <see cref="NotificationDto"/>. If the notification is
        /// not found, the result may indicate an error or a null value, depending on the implementation.</returns>
        Task<IBaseResult<NotificationDto>> NotificationAsync(string notificationId);

        /// <summary>
        /// Marks the specified notification as read.
        /// </summary>
        /// <remarks>Use this method to update the read status of a notification. The caller must ensure 
        /// that the <paramref name="notificationId"/> corresponds to a valid notification.</remarks>
        /// <param name="notificationId">The unique identifier of the notification to mark as read.  This value cannot be <see langword="null"/> or
        /// empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult{T}"/>
        /// object with a <see cref="NotificationDto"/> representing  the updated notification.</returns>
        Task<IBaseResult<NotificationDto>> MarkAsReadAsync(string notificationId);

        /// <summary>
        /// Adds a new notification asynchronously.
        /// </summary>
        /// <param name="message">The notification message to be added. Must not be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult{T}"/>
        /// object with the added <see cref="NotificationDto"/>  if the operation is successful.</returns>
        Task<IBaseResult<NotificationDto>> AddNotificationAsync(NotificationDto message);

        /// <summary>
        /// Removes a notification with the specified identifier asynchronously.
        /// </summary>
        /// <remarks>Use this method to remove a notification by its unique identifier. Ensure that the
        /// provided  <paramref name="notificationId"/> corresponds to an existing notification.</remarks>
        /// <param name="notificationId">The unique identifier of the notification to be removed. Must not be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the success or failure of the operation.</returns>
        Task<IBaseResult> RemoveNotificationAsync(string notificationId);
    }
}
