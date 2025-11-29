using ConectOne.Domain.ResultWrappers;
using IdentityModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Domain.Interfaces
{
    /// <summary>
    /// Interface for managing <see cref="SchoolClass"/> entities, including creation, update, deletion, 
    /// retrieval, and generation of recipient lists for notifications.
    /// </summary>
    public interface ISchoolClassService
    {
        /// <summary>
        /// Retrieves all school classes from the data store.
        /// </summary>
        /// <param name="cancellationToken">Token for cancelling the asynchronous operation.</param>
        /// <returns>A result containing a collection of <see cref="SchoolClass"/> entities.</returns>
        Task<IBaseResult<IEnumerable<SchoolClassDto>>> AllSchoolClassesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a paginated list of school classes.
        /// </summary>
        /// <param name="pageParameters">Paging parameters including page number and page size.</param>
        /// <param name="cancellationToken">Token for cancelling the asynchronous operation.</param>
        /// <returns>A paginated result of <see cref="SchoolClassDto"/> objects.</returns>
        Task<PaginatedResult<SchoolClassDto>> PagedSchoolClassesAsync(SchoolClassPageParameters pageParameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a specific school class by its unique identifier.
        /// </summary>
        /// <param name="schoolClassId">The ID of the school class.</param>
        /// <param name="trackChanges">Whether to enable change tracking in the database context.</param>
        /// <param name="cancellationToken">Token for cancelling the asynchronous operation.</param>
        /// <returns>A result containing the requested <see cref="SchoolClassDto"/> or error messages.</returns>
        Task<IBaseResult<SchoolClassDto>> SchoolClassAsync(string schoolClassId, bool trackChanges = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new school class using the provided data.
        /// </summary>
        /// <param name="schoolClass">The DTO containing the school class data.</param>
        /// <param name="cancellationToken">Token for cancelling the asynchronous operation.</param>
        /// <returns>A result indicating success or failure of the creation process.</returns>
        Task<IBaseResult> CreateAsync(SchoolClassDto schoolClass, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing school class with new values.
        /// </summary>
        /// <param name="schoolClass">The DTO containing updated school class data.</param>
        /// <param name="cancellationToken">Token for cancelling the asynchronous operation.</param>
        /// <returns>A result indicating the outcome of the update process.</returns>
        Task<IBaseResult> UpdateAsync(SchoolClassDto schoolClass, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a school class by its unique identifier.
        /// </summary>
        /// <param name="schoolClassId">The ID of the school class to delete.</param>
        /// <param name="cancellationToken">Token for cancelling the asynchronous operation.</param>
        /// <returns>A result indicating the outcome of the deletion process.</returns>
        Task<IBaseResult> DeleteAsync(string schoolClassId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a list of recipients (learners, parents, teachers) related to the specified school class for notification purposes.
        /// </summary>
        /// <param name="learnerPageParameters">Parameters including class, grade, and age range filters.</param>
        /// <param name="cancellationToken">Token for cancelling the asynchronous operation.</param>
        /// <returns>A result containing the notification recipients or error messages.</returns>
        Task<IBaseResult<IEnumerable<RecipientDto>>> SchoolClassNotificationList(LearnerPageParameters learnerPageParameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a chat group for a specified school class.
        /// </summary>
        /// <remarks>This method creates a new chat group associated with the specified school class and
        /// adds the specified member to the group. Ensure that the provided identifiers are valid and that the caller
        /// has the necessary permissions to create the group.</remarks>
        /// <param name="schoolClassId">The unique identifier of the school class for which the chat group is being created.  This value cannot be
        /// null or empty.</param>
        /// <param name="groupMemberId">The unique identifier of the member who will be added to the chat group.  This value cannot be null or
        /// empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the unique identifier of the
        /// created chat group.</returns>
        Task<IBaseResult<string>> CreateSchoolClassChatGroupAsync(string schoolClassId, string groupMemberId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Exports attendance data for a group that is marked as "to be completed."
        /// </summary>
        /// <remarks>This method initiates an asynchronous export process for attendance data. The caller
        /// can use the returned task to monitor the operation's completion. Ensure that the provided <paramref
        /// name="request"/> contains valid and complete information for the export.</remarks>
        /// <param name="request">The request containing the parameters for the export operation, including group details and export options.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation will terminate early if the token is canceled.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a string representing the
        /// location or identifier of the exported data.</returns>
        Task<IBaseResult<string>> ExportAttendanceGroupToBeCompleted(ExportAttendanceRequest request, CancellationToken cancellationToken = default);
    }

}
