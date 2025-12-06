namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a data transfer object for a voucher associated with a specific room.
    /// </summary>
    /// <remarks>This type is typically used to encapsulate information about the relationship between a
    /// voucher and a room, including identifiers and optional details about the room itself.</remarks>
    public record VoucherRoomDto 
    {
        /// <summary>
        /// Gets or sets the identifier for the voucher room.
        /// </summary>
        public string? VoucherRoomId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for a voucher.
        /// </summary>
        public int VoucherId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the room.
        /// </summary>
        public int RoomId { get; set;}

        /// <summary>
        /// Gets or sets the room associated with the current context.
        /// </summary>
        public RoomDto? Room { get; set; }

    }
}
