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
    /// Provides RESTful operations for managing teacher data, including retrieval, creation, updating, and deletion of
    /// teacher records.
    /// </summary>
    /// <remarks>This service acts as an abstraction over HTTP-based operations for interacting with
    /// teacher-related resources.  It supports both paginated and non-paginated retrieval of teacher data, as well as
    /// operations for checking teacher existence,  retrieving teachers by specific criteria (e.g., email), and managing
    /// teacher notifications.</remarks>
    /// <param name="provider"></param>
    public class TeacherRestService(IBaseHttpProvider provider) : ITeacherService
    {
        /// <summary>
        /// Retrieves a collection of all teachers.
        /// </summary>
        /// <remarks>The operation fetches all teacher data from the underlying data source. The result
        /// may be empty  if no teachers are available. Ensure the cancellationToken is properly managed  to avoid
        /// unintentional cancellation of the operation.</remarks>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  IBaseResult{T} containing an
        /// enumerable collection of TeacherDto  objects representing the teachers.</returns>
        public async Task<IBaseResult<IEnumerable<TeacherDto>>> AllTeachersAsync(CancellationToken cancellationToken)
        {
            var result = await provider.GetAsync<IEnumerable<TeacherDto>>("teachers/all");
            return result;
        }

        /// <summary>
        /// Retrieves a paginated list of teachers based on the specified page parameters.
        /// </summary>
        /// <remarks>This method fetches teacher data from a remote source using the specified pagination
        /// parameters. The <paramref name="trackChanges"/> parameter determines whether the retrieved entities are
        /// tracked for changes, which may impact performance.</remarks>
        /// <param name="pageParameters">The parameters that define the pagination settings, such as page number and page size.</param>
        /// <param name="trackChanges">A value indicating whether to track changes to the retrieved entities. Defaults to <see langword="false"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a collection of <see cref="TeacherDto"/> objects and
        /// pagination metadata.</returns>
        public async Task<PaginatedResult<TeacherDto>> PagedTeachersAsync(TeacherPageParameters pageParameters, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetPagedAsync<TeacherDto, TeacherPageParameters>("teachers/pagedteachers", pageParameters);
            return result;
        }

        /// <summary>
        /// Retrieves a teacher's details asynchronously based on the specified teacher ID.
        /// </summary>
        /// <remarks>This method retrieves the teacher's details from the underlying data provider. If the
        /// teacher ID does not exist, the result may indicate an error or an empty response, depending on the
        /// implementation of the data provider.</remarks>
        /// <param name="teacherId">The unique identifier of the teacher to retrieve. This value cannot be <see langword="null"/> or empty.</param>
        /// <param name="trackChanges">A value indicating whether to track changes to the retrieved teacher entity. The default is <see
        /// langword="false"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that wraps the <see cref="TeacherDto"/> for the specified teacher.</returns>
        public async Task<IBaseResult<TeacherDto>> TeacherAsync(string teacherId, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<TeacherDto>($"teachers/{teacherId}");
            return result;
        }

        /// <summary>
        /// Retrieves a teacher's information based on their email address.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to retrieve teacher data from the
        /// underlying provider. Ensure that the provided email address is valid and corresponds to an existing teacher
        /// in the system.</remarks>
        /// <param name="emailAddress">The email address of the teacher to retrieve. This value cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the teacher's information as a <see cref="TeacherDto"/>. If no teacher is found, the result may
        /// indicate this.</returns>
        public async Task<IBaseResult<TeacherDto>> TeacherByEmailAsync(string emailAddress, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<TeacherDto>($"teachers/byemail/{emailAddress}");
            return result;
        }

        /// <summary>
        /// Determines whether a teacher with the specified email address exists.
        /// </summary>
        /// <remarks>The method sends a request to the underlying provider to verify the existence of a
        /// teacher based on the provided email address.</remarks>
        /// <param name="emailAddress">The email address of the teacher to check for existence. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// where <c>T</c> is a <see cref="string"/> indicating the result of the existence check.</returns>
        public async Task<IBaseResult<string>> TeacherExist(string emailAddress, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<string>($"teachers/exist/{emailAddress}");
            return result;
        }

        /// <summary>
        /// Creates a new teacher record asynchronously.
        /// </summary>
        /// <param name="teacher">The <see cref="TeacherDto"/> object containing the details of the teacher to create.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// containing the created <see cref="TeacherDto"/> object.</returns>
        public async Task<IBaseResult<TeacherDto>> CreateAsync(TeacherDto teacher, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<TeacherDto, TeacherDto>($"teachers", teacher);
            return result;
        }

        /// <summary>
        /// Updates the teacher information asynchronously.
        /// </summary>
        /// <param name="teacher">The <see cref="TeacherDto"/> object containing the updated teacher information.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation. The default value is <see
        /// cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateAsync(TeacherDto teacher, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"teachers", teacher);
            return result;
        }

        /// <summary>
        /// Removes a teacher with the specified identifier asynchronously.
        /// </summary>
        /// <remarks>This method sends a delete request to the underlying provider to remove the teacher.
        /// Ensure the <paramref name="teacherId"/> corresponds to an existing teacher.</remarks>
        /// <param name="teacherId">The unique identifier of the teacher to be removed. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the removal operation.</returns>
        public async Task<IBaseResult> RemoveAsync(string teacherId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"teachers", teacherId);
            return result;
        }

        /// <summary>
        /// Retrieves a paginated list of teacher notification recipients.
        /// </summary>
        /// <remarks>The method sends a request to the underlying data provider to retrieve the
        /// notification recipients based on the specified pagination parameters. The result includes the recipients as
        /// a collection of <see cref="RecipientDto"/> objects.</remarks>
        /// <param name="pageParameters">The pagination parameters, including page number and page size, used to filter the results.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// of <see cref="IEnumerable{RecipientDto}"/> representing the list of notification recipients.</returns>
        public async Task<IBaseResult<IEnumerable<RecipientDto>>> TeachersNotificationList(TeacherPageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<RecipientDto>>($"teachers/notificationList?{pageParameters.GetQueryString()}");
            return result;
        }
    }
}
