using ConectOne.Domain.DataTransferObjects;
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

        /// <summary>
        /// Retrieves all contact numbers associated with the specified parent identifier.
        /// </summary>
        /// <param name="parentId">The unique identifier of the parent whose contact numbers are to be retrieved. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with a collection of <see cref="ContactNumberDto"/> objects representing the contact numbers for the
        /// specified parent. The collection will be empty if no contact numbers are found.</returns>
        public async Task<IBaseResult<IEnumerable<ContactNumberDto>>> AllContactNumbers(string parentId)
        {
            var result = await provider.GetAsync<IEnumerable<ContactNumberDto>>($"teachers/{parentId}/contactNumbers");
            return result;
        }

        /// <summary>
        /// Retrieves the contact number details for the specified contact number identifier.
        /// </summary>
        /// <param name="contactNumberId">The unique identifier of the contact number to retrieve. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="IBaseResult{ContactNumberDto}"/> with the contact number details if found; otherwise, an appropriate
        /// result indicating the outcome.</returns>
        public async Task<IBaseResult<ContactNumberDto>> ContactNumberAsync(string contactNumberId)
        {
            var result = await provider.GetAsync<ContactNumberDto>($"teachers/contactNumbers/{contactNumberId}");
            return result;
        }

        /// <summary>
        /// Creates a new contact number record using the specified contact number details.
        /// </summary>
        /// <param name="contactNumber">The contact number information to create. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="IBaseResult{ContactNumberDto}"/> with the created contact number details if successful.</returns>
        public async Task<IBaseResult<ContactNumberDto>> CreateContactNumber(ContactNumberDto contactNumber)
        {
            var result = await provider.PutAsync<ContactNumberDto, ContactNumberDto>($"teachers/contactNumbers", contactNumber);
            return result;
        }

        /// <summary>
        /// Updates the contact number information for a parent asynchronously.
        /// </summary>
        /// <param name="contactNr">The contact number data to update. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateContactNumberAsync(ContactNumberDto contactNr)
        {
            var result = await provider.PostAsync<ContactNumberDto, ContactNumberDto>($"teachers/contactNumbers", contactNr);
            return result;
        }

        /// <summary>
        /// Asynchronously deletes a contact number identified by the specified contact number ID.
        /// </summary>
        /// <param name="contactNrId">The unique identifier of the contact number to delete. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the delete operation.</returns>
        public async Task<IBaseResult> DeleteContactNumberAsync(string contactNrId)
        {
            var result = await provider.DeleteAsync($"teachers/contactNumbers", contactNrId);
            return result;
        }

        /// <summary>
        /// Asynchronously retrieves all email addresses associated with the specified parent identifier.
        /// </summary>
        /// <param name="parentId">The unique identifier of the parent entity for which to retrieve email addresses. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with a collection of <see cref="EmailAddressDto"/> objects representing the email addresses. The collection
        /// is empty if no email addresses are found.</returns>
        public async Task<IBaseResult<IEnumerable<EmailAddressDto>>> AllEmailAddressesAsync(string parentId)
        {
            var result = await provider.GetAsync<IEnumerable<EmailAddressDto>>($"teachers/{parentId}/emailAddresses");
            return result;
        }

        /// <summary>
        /// Retrieves the email address details for the specified email address identifier.
        /// </summary>
        /// <param name="emailAddressId">The unique identifier of the email address to retrieve. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="IBaseResult{EmailAddressDto}"/> with the email address details if found; otherwise, the result may
        /// indicate an error or that the email address does not exist.</returns>
        public async Task<IBaseResult<EmailAddressDto>> EmailAddressAsync(string emailAddressId)
        {
            var result = await provider.GetAsync<EmailAddressDto>($"teachers/emailAddresses/{emailAddressId}");
            return result;
        }

        /// <summary>
        /// Retrieves the email address details for the specified email address asynchronously.
        /// </summary>
        /// <param name="emailAddress">The email address to retrieve details for. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="IBaseResult{EmailAddressDto}"/> with the details of the specified email address if found; otherwise,
        /// the result may indicate failure or not found.</returns>
        public async Task<IBaseResult<EmailAddressDto>> EmailAddressByAddressAsync(string emailAddress)
        {
            var result = await provider.GetAsync<EmailAddressDto>($"teachers/emailAddresses/{emailAddress}");
            return result;
        }

        /// <summary>
        /// Creates a new email address entry asynchronously using the specified email address details.
        /// </summary>
        /// <param name="emailAddress">An <see cref="EmailAddressDto"/> object containing the details of the email address to create. Cannot be
        /// null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="IBaseResult{EmailAddressDto}"/> with the created email address details if successful.</returns>
        public async Task<IBaseResult<EmailAddressDto>> CreateEmailAddressAsync(EmailAddressDto emailAddress)
        {
            var result = await provider.PutAsync<EmailAddressDto, EmailAddressDto>($"teachers/emailAddresses", emailAddress);
            return result;
        }

        /// <summary>
        /// Updates an existing email address asynchronously using the provided email address data.
        /// </summary>
        /// <param name="emailAddress">An object containing the email address information to update. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateEmailAddressAsync(EmailAddressDto emailAddress)
        {
            var result = await provider.PostAsync<EmailAddressDto, EmailAddressDto>($"teachers/emailAddresses", emailAddress);
            return result;
        }

        /// <summary>
        /// Asynchronously deletes the email address with the specified identifier.
        /// </summary>
        /// <param name="emailAddressId">The unique identifier of the email address to delete. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an object indicating the outcome
        /// of the delete operation.</returns>
        public async Task<IBaseResult> DeleteEmailAddressAsync(string emailAddressId)
        {
            var result = await provider.DeleteAsync($"teachers/emailAddresses", emailAddressId);
            return result;
        }
    }
}
