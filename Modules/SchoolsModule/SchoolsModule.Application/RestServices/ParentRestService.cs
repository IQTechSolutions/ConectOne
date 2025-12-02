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
    /// Provides methods for querying parent-related data from a RESTful API.
    /// </summary>
    /// <remarks>This service is designed to interact with a REST API to retrieve and manage parent-related
    /// information. It supports operations such as retrieving all parents, paginated parent data, parent details by ID
    /// or email, and associated learners or notification lists. The service relies on an HTTP provider to perform the
    /// underlying API calls.</remarks>
    /// <param name="provider"></param>
    public class ParentRestService(IBaseHttpProvider provider) : IParentService
    {
        /// <summary>
        /// Retrieves a collection of parent records asynchronously.
        /// </summary>
        /// <remarks>The method fetches parent records from the underlying data provider. If no <paramref
        /// name="parentId"/> is specified, all available parent records are returned. Use the <paramref
        /// name="cancellationToken"/> to cancel the operation if needed.</remarks>
        /// <param name="parentId">An optional identifier to filter the parent records. If <see langword="null"/>, all parent records are
        /// retrieved.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// wrapping an <see cref="IEnumerable{T}"/> of <see cref="ParentDto"/> objects.</returns>
        public async Task<IBaseResult<IEnumerable<ParentDto>>> AllParentsAsync(string? parentId = null, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<ParentDto>>("parents/all");
            return result;
        }

        /// <summary>
        /// Retrieves a paginated list of parent records based on the specified page parameters.
        /// </summary>
        /// <remarks>This method communicates with the underlying data provider to fetch the paginated
        /// data. Ensure that the <paramref name="pageParameters"/> are properly configured to avoid invalid
        /// requests.</remarks>
        /// <param name="pageParameters">The parameters that define the pagination settings, such as page number and page size.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{ParentDto}"/> containing the paginated list of parent records.</returns>
        public async Task<PaginatedResult<ParentDto>> PagedParentsAsync(ParentPageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetPagedAsync<ParentDto, ParentPageParameters>("parents/pagedparents", pageParameters);
            return result;
        }

        /// <summary>
        /// Retrieves the total count of parent entities.
        /// </summary>
        /// <remarks>The operation sends a request to the underlying provider to fetch the count of parent
        /// entities.  The result encapsulates the count and any additional metadata provided by the <IBaseResult{T}>
        /// implementation.</remarks>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <IBaseResult{T}> where <T>
        /// is an <int> representing the total count of parent entities.</returns>
        public async Task<IBaseResult<int>> ParentCount(CancellationToken cancellationToken)
        {
            var result = await provider.GetAsync<int>("parents/count");
            return result;
        }

        /// <summary>
        /// Retrieves a parent entity by its identifier.
        /// </summary>
        /// <remarks>The <paramref name="trackChanges"/> parameter determines whether the retrieved entity
        /// is tracked  for changes in the underlying data context. This can affect performance and should be used 
        /// appropriately based on the application's requirements.</remarks>
        /// <param name="parentId">The unique identifier of the parent entity to retrieve. Cannot be null or empty.</param>
        /// <param name="trackChanges">A value indicating whether to track changes to the retrieved entity.  If <see langword="true"/>, the entity
        /// will be tracked; otherwise, it will not be tracked.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult{T}"/>
        /// of type <see cref="ParentDto"/> representing the retrieved parent entity.</returns>
        public async Task<IBaseResult<ParentDto>> ParentAsync(string parentId, bool trackChanges, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<ParentDto>($"parents/{parentId}");
            return result;
        }

        /// <summary>
        /// Retrieves a parent record based on the specified email address.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to retrieve a parent record using the
        /// provided email address. Ensure that the email address is valid and properly formatted. The operation may
        /// return an empty result if no parent is associated with the specified email address.</remarks>
        /// <param name="emailAddress">The email address of the parent to retrieve. This value cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="IBaseResult{ParentDto}"/> object with the parent details if found, or an appropriate result indicating
        /// the outcome.</returns>
        public async Task<IBaseResult<ParentDto>> ParentByEmailAsync(string emailAddress, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<ParentDto>($"parents/byemail/{emailAddress}");
            return result;
        }

        /// <summary>
        /// Retrieves a list of learners associated with the specified parent.
        /// </summary>
        /// <remarks>This method sends a request to the underlying data provider to retrieve the learners
        /// associated with the specified parent. Ensure that the <paramref name="parentId"/> is valid and corresponds
        /// to an existing parent in the system.</remarks>
        /// <param name="parentId">The unique identifier of the parent whose learners are to be retrieved. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that holds a list of <see cref="LearnerDto"/> objects representing the learners associated with the
        /// parent.</returns>
        public async Task<IBaseResult<List<LearnerDto>>> ParentLearnersAsync(string parentId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<List<LearnerDto>>($"parents/parentlearners/{parentId}");
            return result;
        }

        /// <summary>
        /// Retrieves a paginated list of parent notification recipients.
        /// </summary>
        /// <remarks>The method sends a request to retrieve the notification recipients for parents based
        /// on the specified pagination parameters. Ensure that <paramref name="pageParameters"/> is properly configured
        /// to avoid unexpected results.</remarks>
        /// <param name="pageParameters">The pagination parameters, including page number and page size, used to filter the results.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// of <see cref="IEnumerable{RecipientDto}"/> representing the list of parent notification recipients.</returns>
        public async Task<IBaseResult<IEnumerable<RecipientDto>>> ParentsNotificationList(ParentPageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<RecipientDto>>($"parents/notificationList?{pageParameters.GetQueryString()}");
            return result;
        }

        /// <summary>
        /// Creates or updates a parent entity asynchronously.
        /// </summary>
        /// <param name="parent">The parent entity to create or update.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  IBaseResult{T} object with
        /// the created or updated ParentDto.</returns>
        public async Task<IBaseResult<ParentDto>> CreateAsync(ParentDto parent, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<ParentDto, ParentDto>("parents", parent);
            return result;
        }

        /// <summary>
        /// Updates the parent entity asynchronously.
        /// </summary>
        /// <remarks>This method sends the provided <paramref name="parent"/> data to the underlying
        /// provider for updating. Ensure that the <paramref name="parent"/> object is properly populated before calling
        /// this method.</remarks>
        /// <param name="parent">The parent entity to be updated, represented as a <see cref="ParentDto"/> object.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateAsync(ParentDto parent, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync("parents", parent);
            return result;
        }

        /// <summary>
        /// Updates the profile information for a parent asynchronously.
        /// </summary>
        /// <param name="parent">The parent profile data to be updated. Cannot be <see langword="null"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateProfileAsync(ParentDto parent, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync("parents/updateprofile", parent);
            return result;
        }

        /// <summary>
        /// Removes a parent entity asynchronously based on the specified identifier.
        /// </summary>
        /// <param name="parentId">The unique identifier of the parent entity to be removed. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the removal operation.</returns>
        public async Task<IBaseResult> RemoveAsync(string parentId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync("parents", parentId);
            return result;
        }

        /// <summary>
        /// Creates a parent chat group and associates it with the specified parent ID.
        /// </summary>
        /// <remarks>This method sends a request to create a new chat group for the specified parent and
        /// adds the specified group member to it. Ensure that the provided identifiers are valid and that the operation
        /// is not canceled via the <paramref name="cancellationToken"/>.</remarks>
        /// <param name="parentId">The unique identifier of the parent for whom the chat group is being created. Cannot be null or empty.</param>
        /// <param name="groupMemberId">The identifier of the group member to be added to the chat group. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the unique identifier of the
        /// created chat group.</returns>
        public async Task<IBaseResult<string>> CreateParentChatGroupAsync(string parentId, string groupMemberId, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<string, string>($"parent/chats/{parentId}", groupMemberId);
            return result;
        }

        /// <summary>
        /// Creates a parent-learner association asynchronously.
        /// </summary>
        /// <remarks>This method sends a request to associate a learner with a parent. Ensure that both
        /// the parent and learner  identifiers are valid and exist in the system before calling this method.</remarks>
        /// <param name="parentId">The unique identifier of the parent.</param>
        /// <param name="learnerId">The unique identifier of the learner to associate with the parent.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> CreateParentLearnerAsync(string parentId, string learnerId, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<string>($"parent/learners/{parentId}/create", learnerId);
            return result;
        }

        /// <summary>
        /// Removes the association between a parent and a learner asynchronously.
        /// </summary>
        /// <remarks>This method removes the relationship between a parent and a learner in the system.
        /// Ensure that both the parent and learner identifiers are valid and exist in the system before calling this
        /// method.</remarks>
        /// <param name="parentId">The unique identifier of the parent whose association is to be removed. Cannot be <see langword="null"/> or
        /// empty.</param>
        /// <param name="learnerId">The unique identifier of the learner whose association with the parent is to be removed. Cannot be <see
        /// langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveParentLearnerAsync(string parentId, string learnerId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync("parents", parentId);
            return result;
        }

        /// <summary>
        /// Exports parent data and retrieves the result as a string.
        /// </summary>
        /// <remarks>This method sends a request to export parent data and returns the result. The
        /// operation is asynchronous and can be canceled using the provided <paramref
        /// name="cancellationToken"/>.</remarks>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the exported parent data as a string.</returns>
        public async Task<IBaseResult<string>> ExportParents(CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<string>("parents/export");
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
            var result = await provider.GetAsync<IEnumerable<ContactNumberDto>>($"parents/{parentId}/contactNumbers");
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
            var result = await provider.GetAsync<ContactNumberDto>($"parents/contactNumbers/{contactNumberId}");
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
            var result = await provider.PutAsync<ContactNumberDto, ContactNumberDto>($"parents/contactNumbers", contactNumber);
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
            var result = await provider.PostAsync<ContactNumberDto, ContactNumberDto>($"parents/contactNumbers", contactNr);
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
            var result = await provider.DeleteAsync($"parents/contactNumbers", contactNrId);
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
            var result = await provider.GetAsync<IEnumerable<EmailAddressDto>>($"parents/{parentId}/emailAddresses");
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
            var result = await provider.GetAsync<EmailAddressDto>($"parents/emailAddresses/{emailAddressId}");
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
            var result = await provider.GetAsync<EmailAddressDto>($"parents/emailAddresses/{emailAddress}");
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
            var result = await provider.PutAsync<EmailAddressDto, EmailAddressDto>($"parents/emailAddresses", emailAddress);
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
            var result = await provider.PostAsync<EmailAddressDto, EmailAddressDto>($"parents/emailAddresses", emailAddress);
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
            var result = await provider.DeleteAsync($"parents/emailAddresses", emailAddressId);
            return result;
        }
    }
}
