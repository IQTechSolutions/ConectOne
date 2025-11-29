using ConectOne.Domain.ResultWrappers;
using IdentityModule.Domain.DataTransferObjects;

namespace IdentityModule.Domain.Interfaces
{
    /// <summary>
    /// Interface for operations related to JWT and device tokens.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Retrieves device tokens for multiple users.
        /// </summary>
        /// <param name="userIds">List of user IDs.</param>
        /// <returns>A result containing device tokens.</returns>
        Task<IBaseResult<List<DeviceTokenDto>>> DeviceTokensAsync(List<string> userIds);

        /// <summary>
        /// Creates a new device token for a user.
        /// </summary>
        /// <param name="token">Device token details.</param>
        /// <returns>A result indicating success or failure.</returns>
        Task<IBaseResult> CreateDeviceTokenAsync(DeviceTokenDto token);

        /// <summary>
        /// Removes a device token from a user.
        /// </summary>
        /// <param name="token">The device token to remove.</param>
        /// <returns>A result indicating success or failure.</returns>
        Task<IBaseResult> RemoveDeviceTokenAsync(DeviceTokenDto token);
    }
}
