using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Enums;
using ShoppingModule.Domain.Enums;

namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Data Transfer Object (DTO) for vacation booking information.
    /// This DTO is used to transfer vacation booking data between different layers of the application.
    /// </summary>
    public record VacationBookingDto
    {
        #region Constructors

        /// <summary>
        /// Default constructor for creating a new instance of <see cref="VacationBookingDto"/>.
        /// </summary>
        public VacationBookingDto() { }

        /// <summary>
        /// Initializes a new instance of <see cref="VacationBookingDto"/> using a <see cref="VacationBooking"/>.
        /// </summary>
        /// <param name="booking">The booking entity containing booking details.</param>
        public VacationBookingDto(VacationBooking booking)
        {
            BookingId = booking.Id;
            StartDate = booking.StartDate;
            EndDate = booking.EndDate;
            ReferenceNr = booking.ReferenceNr;
            PaymentMethod = booking.PaymentMethod;
            ReservationType = booking.ReservationType;
            BookingStatus = booking.BookingStatus;
            AmountDueIncl = booking.AmountDueIncl;
            UserId = booking.UserId;
            VacationId = booking.VacationId;
            Guests = booking.Guests.Select(g => new VacationGuestInfoDto(g)).ToList();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the booking ID.
        /// </summary>
        public string? BookingId { get; set; }

        /// <summary>
        /// Gets or sets the start date of the booking.
        /// </summary>
        public DateTime StartDate { get; init; }

        /// <summary>
        /// Gets or sets the end date of the booking.
        /// </summary>
        public DateTime EndDate { get; init; }

        /// <summary>
        /// Gets or sets the reference number for the booking.
        /// </summary>
        public string? ReferenceNr { get; init; }

        /// <summary>
        /// Gets or sets the payment method for the booking.
        /// </summary>
        public PaymentMethod PaymentMethod { get; init; }

        /// <summary>
        /// Gets or sets the reservation type for the booking.
        /// </summary>
        public ReservationType ReservationType { get; init; }

        /// <summary>
        /// Gets or sets the booking status.
        /// </summary>
        public BookingStatus BookingStatus { get; init; } = BookingStatus.Pending;

        /// <summary>
        /// Gets or sets the amount due including taxes and fees.
        /// </summary>
        public double AmountDueIncl { get; init; }

        /// <summary>
        /// Gets or sets the ID of the user associated with the booking.
        /// </summary>
        public string? UserId { get; init; }

        /// <summary>
        /// Gets or sets the ID of the associated vacation.
        /// </summary>
        public string? VacationId { get; init; }

        #endregion

        #region Collections

        /// <summary>
        /// Gets or sets the list of guests associated with the booking.
        /// </summary>
        public List<VacationGuestInfoDto> Guests { get; init; } = new();

        #endregion
    }
}
