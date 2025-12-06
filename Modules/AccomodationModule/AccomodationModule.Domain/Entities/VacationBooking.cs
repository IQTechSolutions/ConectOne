using System.ComponentModel.DataAnnotations.Schema;
using AccomodationModule.Domain.Enums;
using ConectOne.Domain.Entities;
using ShoppingModule.Domain.Enums;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents a booking for a vacation, including details such as dates, payment method, and associated guests.
    /// </summary>
    public class VacationBooking : EntityBase<string>
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the start date of the vacation booking.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of the vacation booking.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the reference number for the vacation booking.
        /// </summary>
        public string? ReferenceNr { get; set; }

        /// <summary>
        /// Gets or sets the payment method for the vacation booking.
        /// </summary>
        public PaymentMethod PaymentMethod { get; set; }

        /// <summary>
        /// Gets or sets the reservation type for the vacation booking.
        /// </summary>
        public ReservationType ReservationType { get; set; }

        /// <summary>
        /// Gets or sets the booking status.
        /// </summary>
        public BookingStatus BookingStatus { get; set; } = BookingStatus.Pending;

        /// <summary>
        /// Gets or sets the amount due excluding taxes and fees.
        /// </summary>
        public double AmountDueExcl { get; set; }

        /// <summary>
        /// Gets or sets the amount due including taxes and fees.
        /// </summary>
        public double AmountDueIncl { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user associated with the vacation booking.
        /// </summary>
        public string? UserId { get; set; }

        #endregion

        #region One-To-Many Relationships

        /// <summary>
        /// Gets or sets the ID of the associated vacation.
        /// </summary>
        [ForeignKey(nameof(Vacation))]
        public string? VacationId { get; set; }

        /// <summary>
        /// Gets or sets the associated vacation.
        /// </summary>
        public Vacation? Vacation { get; set; }

        #endregion

        #region Many-To-One Relationships

        /// <summary>
        /// Gets or sets the collection of guests associated with the vacation booking.
        /// </summary>
        public ICollection<VacationGuestInfo> Guests { get; set; } = [];

        #endregion
    }
}
