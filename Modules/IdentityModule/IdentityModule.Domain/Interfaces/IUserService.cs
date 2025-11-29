using ConectOne.Domain.DataTransferObjects;
using ConectOne.Domain.ResultWrappers;
using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.RequestFeatures;

namespace IdentityModule.Domain.Interfaces
{
    /// <summary>
    /// Interface for user management operations.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Sets the timestamp for when a user accepted the privacy terms.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        Task<IBaseResult> SetPrivacyAndUsageTermsAcceptedTimeStamp(string userId);

        /// <summary>
        /// Removes a user from the system by ID.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>A result indicating success or failure.</returns>
        Task<IBaseResult> RemoveUserAsync(string userId);

        /// <summary>
        /// Retrieves all users with their info.
        /// </summary>
        Task<IBaseResult<IEnumerable<UserInfoDto>>> AllUsers(UserPageParameters pageParameters);

        /// <summary>
        /// Retrieves a paginated list of users based on the specified paging and filtering parameters.
        /// </summary>
        /// <param name="pageParameters">The parameters that define paging, sorting, and filtering options for the user list. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a paginated result of user
        /// information. The result may be empty if no users match the specified criteria.</returns>
        Task<PaginatedResult<UserInfoDto>> PagedUsers(UserPageParameters pageParameters);

        /// <summary>
        /// Gets the total active user count.
        /// </summary>
        Task<IBaseResult<int>> UserCount();

        /// <summary>
        /// Retrieves paged user information.
        /// </summary>
        Task<IBaseResult<IEnumerable<UserInfoDto>>> GetAllUsersAsync(UserPageParameters pageParameters);

        /// <summary>
        /// Asynchronously creates a new user record based on the provided user information.
        /// </summary>
        /// <param name="userInfo">An object containing the details of the user to create. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation, or <see langword="null"/> if the creation failed unexpectedly.</returns>
        Task<IBaseResult?> CreateUserInfoAsync(UserInfoDto userInfo);

        /// <summary>
        /// Adds or updates a user's address.
        /// </summary>
        Task<IBaseResult> AddUpdateUserInfoAddress(string userId, AddressDto dto);

        /// <summary>
        /// Gets user info by ID.
        /// </summary>
        Task<IBaseResult<UserInfoDto>> GetUserInfoAsync(string userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets user info by email.
        /// </summary>
        Task<IBaseResult<UserInfoDto>> GetUserInfoByEmailAsync(string email);

        /// <summary>
        /// Changes the connection status of a user.
        /// </summary>
        Task<IBaseResult> ChangeConnectionStatus(string userId, bool status);

        /// <summary>
        /// Updates existing user info.
        /// </summary>
        Task<IBaseResult> UpdateUserInfoAsync(UserInfoDto userInfo);

        /// <summary>
        /// Toggles a user's active status.
        /// </summary>
        Task<IBaseResult> ChangeUserStatusAsync(string userId);

        /// <summary>
        /// Changes a user's password.
        /// </summary>
        Task<IBaseResult> ChangeUserPasswordAsync(ChangePasswordRequest changePasswordRequest);

        /// <summary>
        /// Accepts a user's registration.
        /// </summary>
        Task<IBaseResult> AcceptRegistrationAsync(AcceptRegistrationRequest registrationRequest);

        /// <summary>
        /// Rejects a user's registration.
        /// </summary>
        Task<IBaseResult> RejectRegistrationAsync(RejectRegistrationRequest request);

        /// <summary>
        /// Retrieves a list of recipients eligible for global notifications.
        /// </summary>
        /// <remarks>The method returns a collection of RecipientDto objects representing users  who are
        /// configured to receive global notifications. The result is wrapped in an  IBaseResult{T} to provide
        /// additional metadata about the operation's success or failure.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  IBaseResult{T} with an
        /// enumerable collection of RecipientDto objects.  If no recipients are found, the collection will be empty.</returns>
        Task<IBaseResult<IEnumerable<RecipientDto>>> GlobalNotificationsUserList();
    }
}
