using ConectOne.Domain.Extensions;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using IdentityModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Application.RestServices
{
    /// <summary>
    /// Provides methods for managing school classes and related operations through RESTful API calls.
    /// </summary>
    /// <remarks>This service interacts with a REST API to perform operations such as retrieving school
    /// classes,  creating or updating school class records, managing notifications, and exporting attendance data.  It
    /// implements the <see cref="ISchoolClassService"/> interface and relies on an <see cref="IBaseHttpProvider"/>  for
    /// HTTP communication.</remarks>
    /// <param name="provider"></param>
    public class SchoolClassRestService(IBaseHttpProvider provider) : ISchoolClassService
    {
        /// <summary>
        /// Retrieves a collection of all school classes.
        /// </summary>
        /// <remarks>This method fetches all available school classes from the underlying data provider.
        /// The caller can use the <paramref name="cancellationToken"/> to cancel the operation if needed.</remarks>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// containing an enumerable collection of <see cref="SchoolClassDto"/> objects representing the school classes.</returns>
        public async Task<IBaseResult<IEnumerable<SchoolClassDto>>> AllSchoolClassesAsync(CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<SchoolClassDto>>("schoolClasses/all");
            return result;
        }

        /// <summary>
        /// Retrieves a paginated list of school classes based on the specified page parameters.
        /// </summary>
        /// <remarks>This method sends a request to the "schoolClasses/pagedschoolClasses" endpoint to
        /// retrieve the data. Ensure that the <paramref name="pageParameters"/> object is properly configured to avoid
        /// invalid requests.</remarks>
        /// <param name="pageParameters">The parameters that define the pagination and filtering options for the school classes.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a collection of <see cref="SchoolClassDto"/> objects and
        /// pagination metadata.</returns>
        public async Task<PaginatedResult<SchoolClassDto>> PagedSchoolClassesAsync(SchoolClassPageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetPagedAsync<SchoolClassDto, SchoolClassPageParameters>("schoolClasses/pagedschoolClasses", pageParameters);
            return result;
        }

        /// <summary>
        /// Retrieves a school class by its unique identifier.
        /// </summary>
        /// <remarks>This method uses the underlying provider to fetch the school class data. The behavior
        /// of the returned result depends on the implementation of the provider.</remarks>
        /// <param name="schoolClassId">The unique identifier of the school class to retrieve. Cannot be null or empty.</param>
        /// <param name="trackChanges">A value indicating whether to track changes to the retrieved entity. Defaults to <see langword="false"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the retrieved <see cref="SchoolClassDto"/>. If the school class is not found, the result may
        /// indicate an error or contain a null value, depending on the implementation.</returns>
        public async Task<IBaseResult<SchoolClassDto>> SchoolClassAsync(string schoolClassId, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<SchoolClassDto>($"schoolClasses/{schoolClassId}");
            return result;
        }

        /// <summary>
        /// Creates or updates a school class asynchronously.
        /// </summary>
        /// <remarks>This method sends the provided <paramref name="schoolClass"/> to the underlying
        /// provider for creation or update. Ensure that the <paramref name="schoolClass"/> contains valid data before
        /// calling this method.</remarks>
        /// <param name="schoolClass">The data transfer object representing the school class to be created or updated.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> CreateAsync(SchoolClassDto schoolClass, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync($"schoolClasses", schoolClass);
            return result;
        }

        /// <summary>
        /// Updates the specified school class asynchronously.
        /// </summary>
        /// <remarks>This method sends the provided <paramref name="schoolClass"/> to the underlying
        /// provider for updating. Ensure that the <paramref name="schoolClass"/> contains valid data before calling
        /// this method.</remarks>
        /// <param name="schoolClass">The data transfer object representing the school class to be updated.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateAsync(SchoolClassDto schoolClass, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"schoolClasses", schoolClass);
            return result;
        }

        /// <summary>
        /// Deletes a school class with the specified identifier asynchronously.
        /// </summary>
        /// <remarks>This method sends a delete request to the underlying provider to remove the specified
        /// school class.  Ensure that the <paramref name="schoolClassId"/> corresponds to an existing school
        /// class.</remarks>
        /// <param name="schoolClassId">The unique identifier of the school class to delete. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the delete operation.</returns>
        public async Task<IBaseResult> DeleteAsync(string schoolClassId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"schoolClasses", schoolClassId);
            return result;
        }

        /// <summary>
        /// Retrieves a paginated list of recipients for school class notifications.
        /// </summary>
        /// <remarks>This method sends a request to retrieve a list of recipients based on the specified
        /// pagination and filtering parameters. The result includes the recipients who are eligible for school class
        /// notifications.</remarks>
        /// <param name="learnerPageParameters">The pagination and filtering parameters to apply when retrieving the notification list.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// of <see cref="IEnumerable{RecipientDto}"/> representing the recipients for school class notifications.</returns>
        public async Task<IBaseResult<IEnumerable<RecipientDto>>> SchoolClassNotificationList(LearnerPageParameters learnerPageParameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<RecipientDto>>($"schoolClasses/notificationList?{learnerPageParameters.GetQueryString()}");
            return result;
        }

        /// <summary>
        /// Creates a chat group for a specified school class and adds a member to the group.
        /// </summary>
        /// <remarks>This method sends a request to create a chat group for the specified school class and
        /// adds the specified member to the group. Ensure that the provided identifiers are valid and that the caller
        /// has the necessary permissions to perform this operation.</remarks>
        /// <param name="schoolClassId">The unique identifier of the school class for which the chat group is being created.</param>
        /// <param name="groupMemberId">The identifier of the member to be added to the newly created chat group.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>An <see cref="IBaseResult{T}"/> containing the unique identifier of the created chat group as a string.</returns>
        public async Task<IBaseResult<string>> CreateSchoolClassChatGroupAsync(string schoolClassId, string groupMemberId, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<string, string>($"schoolClasses/{schoolClassId}", groupMemberId);
            return result;
        }

        /// <summary>
        /// Exports attendance data for groups that are yet to be completed.
        /// </summary>
        /// <remarks>This method sends the export request to the server and returns the result of the
        /// operation. Ensure that the <paramref name="request"/> object is properly populated before calling this
        /// method.</remarks>
        /// <param name="request">The request object containing the parameters for the export operation.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a string representing the
        /// outcome of the export operation.</returns>
        public async Task<IBaseResult<string>> ExportAttendanceGroupToBeCompleted(ExportAttendanceRequest request, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<string, ExportAttendanceRequest>($"exportAttendance/toBeCompleted", request);
            return result;
        }
    }
}
