namespace AccomodationModule.Domain.DataTransferObjects
{
	/// <summary>
	/// Represents the data transfer object used to cancel a booking.
	/// </summary>
	/// <remarks>This class encapsulates the identifiers required to process a booking cancellation. It is typically
	/// used in API requests or service calls to specify the booking and cancellation details.</remarks>
	public class CancelBookingDto
	{
		/// <summary>
		/// Gets or sets the unique identifier for a booking.
		/// </summary>
		public int BookingId { get; set; }

		/// <summary>
		/// Gets or sets the unique identifier for a cancellation operation.
		/// </summary>
		public int CancellationId { get; set; }
	}
}
