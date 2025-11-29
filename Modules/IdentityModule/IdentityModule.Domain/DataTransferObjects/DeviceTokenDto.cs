using IdentityModule.Domain.Entities;

namespace IdentityModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for device tokens, 
    /// which are used to identify devices for push notifications or other purposes.
    /// </summary>
    public record DeviceTokenDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceTokenDto"/> class with default values.
        /// </summary>
        public DeviceTokenDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceTokenDto"/> class with specified user ID and device token.
        /// </summary>
        /// <param name="userId">The unique identifier of the user associated with the device token.</param>
        /// <param name="deviceTokenId">The content of the device token.</param>
        public DeviceTokenDto(string userId, string deviceTokenId)
        {
            DeviceTokenId = Guid.NewGuid().ToString();
            UserId = userId;
            DeviceToken = deviceTokenId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceTokenDto"/> class using a <see cref="DeviceToken"/> entity.
        /// </summary>
        /// <param name="token">The <see cref="DeviceToken"/> entity containing the device token details.</param>
        public DeviceTokenDto(DeviceToken token)
        {
            DeviceTokenId = token.Id;
            UserId = token.UserId;
            DeviceToken = token.DeviceTokenContent;
        }

        /// <summary>
        /// Gets the unique identifier of the device token.
        /// </summary>
        public string? DeviceTokenId { get; init; }

        /// <summary>
        /// Gets the unique identifier of the user associated with the device token.
        /// </summary>
        public string? UserId { get; init; }

        /// <summary>
        /// Gets the content of the device token.
        /// </summary>
        public string? DeviceToken { get; init; } 
    }
}
