using ConectOne.Domain.Extensions;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Interfaces;

namespace IdentityModule.Application.RestServices
{
    /// <summary>
    /// Provides REST-based operations for managing device tokens associated with user accounts.
    /// </summary>
    /// <param name="provider">The HTTP provider used to send requests to the underlying REST API.</param>
    public class TokenRestService(IBaseHttpProvider provider) : ITokenService
    {
        /// <summary>
        /// Retrieves the device tokens associated with the specified user IDs asynchronously.
        /// </summary>
        /// <param name="userIds">A list of user IDs for which to retrieve device tokens. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// with a list of <see cref="DeviceTokenDto"/> objects corresponding to the specified user IDs. The list will
        /// be empty if no device tokens are found.</returns>
        public async Task<IBaseResult<List<DeviceTokenDto>>> DeviceTokensAsync(List<string> userIds)
        {
            var result = await provider.GetAsync<List<DeviceTokenDto>>($"account/deviceTokens?{userIds.GetQueryString()}");
            return result;
        }

        /// <summary>
        /// Creates or updates a device token for the current account asynchronously.
        /// </summary>
        /// <param name="token">An object containing the device token information to be registered or updated. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> CreateDeviceTokenAsync(DeviceTokenDto token)
        {
            var result = await provider.PutAsync("account/createDeviceToken", token);
            return result;
        }

        /// <summary>
        /// Asynchronously removes a device token associated with a user account.
        /// </summary>
        /// <param name="token">An object containing the user identifier and device token to be removed. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an object indicating the outcome
        /// of the removal operation.</returns>
        public async Task<IBaseResult> RemoveDeviceTokenAsync(DeviceTokenDto token)
        {
            var result = await provider.DeleteAsync($"account/removeDeviceToken/{token.UserId}/{token.DeviceToken}", "");
            return result;
        }
    }
}
