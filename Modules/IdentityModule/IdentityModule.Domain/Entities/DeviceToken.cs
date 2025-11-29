using ConectOne.Domain.Entities;

namespace IdentityModule.Domain.Entities
{
    /// <summary>
    /// Represents a device token used for push notifications or other device-specific actions.
    /// </summary>
    /// <param name="userId">The identity of the user this token belongs to</param>
    /// <param name="deviceTokenContent">The token content</param>
    public class DeviceToken(string userId, string deviceTokenContent) : EntityBase<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceToken"/> class with default values.
        /// </summary>
        public string UserId { get; set; } = userId;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceToken"/> class with specified user ID and device token.
        /// </summary>
        public string DeviceTokenContent { get; set; } = deviceTokenContent;

        /// <summary>
        /// Returns a string representation of the <see cref="DeviceToken"/> object.
        /// </summary>
        public override string ToString()
        {
            return "Device Token";
        }
    }
}