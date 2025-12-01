using ConectOne.Domain.DataTransferObjects;
using ConectOne.Domain.Extensions;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Interfaces;
using IdentityModule.Domain.RequestFeatures;

namespace IdentityModule.Application.RestServices
{
    /// <summary>
    /// Provides a REST-based implementation of the <see cref="IUserService"/> interface for managing user accounts and
    /// related operations.
    /// </summary>
    /// <remarks>This service interacts with a RESTful API to perform various user-related operations, such as
    /// retrieving user information,  managing user accounts, and handling registration requests. It relies on an <see
    /// cref="IBaseHttpProvider"/> to send HTTP requests  and process responses. The methods in this service return
    /// results wrapped in <see cref="IBaseResult"/> to encapsulate the outcome  of the operations.</remarks>
    /// <param name="provider"></param>
    public class UserRestService(IBaseHttpProvider provider) : IUserService
    {
        /// <summary>
        /// Updates the privacy and usage terms acceptance timestamp for the specified user.
        /// </summary>
        /// <remarks>This method sends a request to update the acceptance timestamp for the privacy and
        /// usage terms associated with the specified user. Ensure the <paramref name="userId"/> is valid and not null
        /// before calling this method.</remarks>
        /// <param name="userId">The unique identifier of the user whose acceptance timestamp is being updated.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> SetPrivacyAndUsageTermsAcceptedTimeStamp(string userId)
        {
            var result = await provider.PostAsync($"account/setPrivacyAndUsageTermsAcceptedTimeStamp/{userId}");
            return result;
        }

        /// <summary>
        /// Removes a user with the specified user ID asynchronously.
        /// </summary>
        /// <remarks>This method sends a request to remove the user identified by the provided <paramref
        /// name="userId"/>. Ensure the user ID is valid and exists in the system before calling this method.</remarks>
        /// <param name="userId">The unique identifier of the user to be removed. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveUserAsync(string userId)
        {
            var result = await provider.DeleteAsync("account/users/removeUser", userId);
            return result;
        }

        /// <summary>
        /// Retrieves a paginated list of all users.
        /// </summary>
        /// <remarks>The method fetches user data from the underlying provider based on the specified
        /// pagination parameters. Ensure that <paramref name="pageParameters"/> contains valid values to avoid
        /// unexpected results.</remarks>
        /// <param name="pageParameters">The pagination parameters, including page number and page size, used to filter the user list.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// containing an enumerable collection of <see cref="UserInfoDto"/> objects representing the users.</returns>
        public async Task<IBaseResult<IEnumerable<UserInfoDto>>> AllUsers(UserPageParameters pageParameters)
        {
            var result = await provider.GetAsync<IEnumerable<UserInfoDto>>($"account/users/{pageParameters.GetQueryString()}");
            return result;
        }

        /// <summary>
        /// Retrieves a paginated list of user information based on the specified paging and filtering parameters.
        /// </summary>
        /// <param name="pageParameters">The parameters that define the paging, sorting, and filtering options for the user list. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see
        /// cref="PaginatedResult{UserInfoDto}"/> with the users matching the specified parameters. If no users are
        /// found, the result contains an empty collection.</returns>
        public async Task<PaginatedResult<UserInfoDto>> PagedUsers(UserPageParameters pageParameters)
        {
            var result = await provider.GetPagedAsync<UserInfoDto, UserPageParameters>($"account/users/paged", pageParameters);
            return result;
        }

        /// <summary>
        /// Retrieves the total number of users in the system.
        /// </summary>
        /// <remarks>This method asynchronously fetches the user count from the underlying data provider.
        /// The result represents the total number of users currently registered in the system.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <IBaseResult{T}> where
        /// <c>T</c> is an <int> representing the total user count.</returns>
        public async Task<IBaseResult<int>> UserCount()
        {
            var result = await provider.GetAsync<int>("account/users/count");
            return result;
        }

        /// <summary>
        /// Retrieves a paginated list of all users based on the specified page parameters.
        /// </summary>
        /// <remarks>This method sends a request to the underlying data provider to retrieve user
        /// information. Ensure that the <paramref name="pageParameters"/> object contains valid values to avoid
        /// unexpected results.</remarks>
        /// <param name="pageParameters">The pagination parameters, including page number and page size, used to filter the user list.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with an enumerable collection of <see cref="UserInfoDto"/> representing the users.</returns>
        public async Task<IBaseResult<IEnumerable<UserInfoDto>>> GetAllUsersAsync(UserPageParameters pageParameters)
        {
            var result = await provider.GetAsync<IEnumerable<UserInfoDto>>($"account/users/{pageParameters.GetQueryString()}");
            return result;
        }
        
        /// <summary>
        /// Adds or updates the address information for a specified user.
        /// </summary>
        /// <remarks>This method sends the address information to the server for the specified user.
        /// Ensure that the <paramref name="userId"/> corresponds to a valid user and that the <paramref name="dto"/>
        /// contains all required address fields.</remarks>
        /// <param name="userId">The unique identifier of the user whose address information is being added or updated. Cannot be null or
        /// empty.</param>
        /// <param name="dto">An <see cref="AddressDto"/> object containing the address details to be added or updated. Cannot be null.</param>
        /// <returns>An <see cref="IBaseResult"/> representing the result of the operation.</returns>
        public async Task<IBaseResult> AddUpdateUserInfoAddress(string userId, AddressDto dto)
        {
            var result = await provider.PostAsync($"account/users/{userId}/address/addupdate", dto);
            return result;
        }

        /// <summary>
        /// Retrieves user information for the specified user ID.
        /// </summary>
        /// <remarks>This method sends a request to retrieve user information based on the provided user
        /// ID.  Ensure that the user ID is valid and corresponds to an existing user.</remarks>
        /// <param name="userId">The unique identifier of the user whose information is to be retrieved. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// of type <see cref="UserInfoDto"/> with the user's information.</returns>
        public async Task<IBaseResult<UserInfoDto>> GetUserInfoAsync(string userId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<UserInfoDto>($"account/users/{userId}");
            return result;
        }

        /// <summary>
        /// Retrieves user information based on the specified email address.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch user details from the
        /// underlying data provider. Ensure that the email address provided is valid and properly formatted.</remarks>
        /// <param name="email">The email address of the user whose information is to be retrieved. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the user information as a <see cref="UserInfoDto"/>. If the user is not found, the result may
        /// indicate an error or an empty value.</returns>
        public async Task<IBaseResult<UserInfoDto>> GetUserInfoByEmailAsync(string email)
        {
            var result = await provider.GetAsync<UserInfoDto>($"account/users/email/{email}");
            return result;
        }

        /// <summary>
        /// Changes the connection status of a user.
        /// </summary>
        /// <remarks>This method sends a request to update the user's connection status. Ensure the
        /// <paramref name="userId"/> is valid and the caller has the necessary permissions to perform this
        /// operation.</remarks>
        /// <param name="userId">The unique identifier of the user whose connection status is to be changed.</param>
        /// <param name="status">The new connection status to set. <see langword="true"/> to enable the connection; otherwise, <see
        /// langword="false"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> ChangeConnectionStatus(string userId, bool status)
        {
            var result = await provider.PostAsync($"account/users/changeStatus/{userId}/{status}");
            return result;
        }

        /// <summary>
        /// Updates the user information asynchronously.
        /// </summary>
        /// <remarks>This method sends the updated user information to the server and returns the result
        /// of the operation. Ensure that the <paramref name="userInfo"/> object contains valid data before calling this
        /// method.</remarks>
        /// <param name="userInfo">An object containing the updated user information. This parameter cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateUserInfoAsync(UserInfoDto userInfo)
        {
            var result = await provider.PostAsync($"account/users/update", userInfo);
            return result;
        }

        /// <summary>
        /// Changes the status of a user asynchronously.
        /// </summary>
        /// <remarks>This method sends a request to change the status of the specified user. Ensure the
        /// <paramref name="userId"/> corresponds to a valid user in the system.</remarks>
        /// <param name="userId">The unique identifier of the user whose status is to be changed. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the status change operation.</returns>
        public async Task<IBaseResult> ChangeUserStatusAsync(string userId)
        {
            var result = await provider.PostAsync($"account/users/changestatus/{userId}");
            return result;
        }

        /// <summary>
        /// Changes the password for a user account asynchronously.
        /// </summary>
        /// <remarks>This method sends a request to the server to update the user's password. Ensure that
        /// the <paramref name="changePasswordRequest"/> contains valid and complete information before calling this
        /// method.</remarks>
        /// <param name="changePasswordRequest">An object containing the details required to change the user's password, including the current password and
        /// the new password.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the password change request.</returns>
        public async Task<IBaseResult> ChangeUserPasswordAsync(ChangePasswordRequest changePasswordRequest)
        {
            var result = await provider.PostAsync($"account/users/changepassword", changePasswordRequest);
            return result;
        }

        /// <summary>
        /// Accepts a user registration request and processes it asynchronously.
        /// </summary>
        /// <remarks>This method sends the registration request to the appropriate endpoint for
        /// processing. Ensure that the <paramref name="registrationRequest"/> contains valid and complete data before
        /// calling this method.</remarks>
        /// <param name="registrationRequest">The registration request to be accepted. This must contain the necessary details for processing the
        /// registration.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the registration acceptance.</returns>
        public async Task<IBaseResult> AcceptRegistrationAsync(AcceptRegistrationRequest registrationRequest)
        {
            var result = await provider.PostAsync($"account/users/registrations/accept", registrationRequest);
            return result;
        }

        /// <summary>
        /// Rejects a user registration request asynchronously.
        /// </summary>
        /// <remarks>This method sends a rejection request to the provider's API endpoint for user
        /// registrations. Ensure that the <paramref name="request"/> parameter contains valid data before calling this
        /// method.</remarks>
        /// <param name="request">The request containing the details of the registration to be rejected.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the rejection operation.</returns>
        public async Task<IBaseResult> RejectRegistrationAsync(RejectRegistrationRequest request)
        {
            var result = await provider.PostAsync($"account/users/registrations/reject", request);
            return result;
        }

        /// <summary>
        /// Retrieves a list of users who are subscribed to global notifications.
        /// </summary>
        /// <remarks>This method asynchronously fetches the list of recipients for global notifications
        /// from the underlying data provider. The returned list contains user details in the form of <see
        /// cref="RecipientDto"/> objects.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// wrapping an enumerable collection of <see cref="RecipientDto"/> objects representing the users subscribed to
        /// global notifications.</returns>
        public async Task<IBaseResult<IEnumerable<RecipientDto>>> GlobalNotificationsUserList()
        {
            var result = await provider.GetAsync<IEnumerable<RecipientDto>>($"account/users/notifications/global");
            return result;
        }

        /// <summary>
        /// Creates a new user information record asynchronously using the specified user data.
        /// </summary>
        /// <param name="userInfo">An object containing the user information to be created. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation, or <see langword="null"/> if the operation did not return a result.</returns>
        public async Task<IBaseResult?> CreateUserInfoAsync(UserInfoDto userInfo)
        {
            var result = await provider.PutAsync($"account/users/userInfo/create", userInfo);
            return result;
        }
    }
}
