namespace AccomodationModule.Domain.DataTransferObjects
{
	/// <summary>
	/// Represents a mapping between a lodging, a voucher, and a room.
	/// </summary>
	/// <remarks>This data transfer object is used to associate a lodging identifier, a voucher identifier,  and a
	/// room identifier. It is typically used in scenarios where vouchers are assigned to  specific lodgings and
	/// rooms.</remarks>
	public record VoucherMappingDto
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="VoucherMappingDto"/> class.
		/// </summary>
		public VoucherMappingDto() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="VoucherMappingDto"/> class, representing the mapping between a
		/// lodging, voucher, and room.
		/// </summary>
		/// <param name="lodgingId">The unique identifier of the lodging associated with the voucher.</param>
		/// <param name="voucherId">The unique identifier of the voucher.</param>
		/// <param name="roomId">The unique identifier of the room associated with the voucher.</param>
		public VoucherMappingDto(string lodgingId, int voucherId, string roomId)
        {
            LodgingId = lodgingId;
			VoucherId = voucherId;
			RoomId = roomId;
		}

        #endregion

		/// <summary>
		/// Gets the unique identifier for the lodging.
		/// </summary>
        public string LodgingId { get; init; } = null!;

		/// <summary>
		/// Gets the unique identifier for the voucher.
		/// </summary>
        public int VoucherId { get; init; }

		/// <summary>
		/// Gets the unique identifier for the room.
		/// </summary>
        public string RoomId { get; init; } = null!;
    }
}
