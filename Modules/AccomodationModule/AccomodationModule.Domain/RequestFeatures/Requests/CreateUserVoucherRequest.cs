namespace AccomodationModule.Domain.Arguments.Requests
{
    /// <summary>
    /// Represents a request to create a user voucher, including the user, voucher, and room identifiers.
    /// </summary>
    /// <remarks>This record is used to encapsulate the data required to associate a voucher with a user in a
    /// specific room. It provides immutable properties for the user ID, voucher ID, and room ID.</remarks>
    public record CreateUserVoucherRequest
    {
        /// <summary>
        /// Gets the unique identifier for the user.
        /// </summary>
        public string UserId { get; init; } = null!;

        /// <summary>
        /// Gets the unique identifier for the voucher.
        /// </summary>
        public int VoucherId { get; init; } 

        /// <summary>
        /// Gets the unique identifier for the room.
        /// </summary>
        public int RoomId { get; init; }
    }
}
