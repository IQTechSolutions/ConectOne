using System.ComponentModel.DataAnnotations.Schema;
using AccomodationModule.Domain.Enums;
using ConectOne.Domain.Entities;
using IdentityModule.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents a booking entity, including details such as booking reference, guest information, dates, and associated entities.
    /// This class is used to manage booking data within the application.
    /// </summary>
    public class Booking : EntityBase<int>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the unique booking reference number.
        /// </summary>
        public string BookingReferenceNr { get; set; } = null!;

        /// <summary>
        /// Gets or sets the name of the booking.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the contact details for the booking.
        /// </summary>
        public string? Contacts { get; set; }

        /// <summary>
        /// Gets or sets the phone number associated with the booking.
        /// </summary>
        public string? PhoneNr { get; set; }

        /// <summary>
        /// Gets or sets the email address associated with the booking.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the website associated with the booking.
        /// </summary>
        public string? Website { get; set; }

        /// <summary>
        /// Gets or sets the address associated with the booking.
        /// </summary>
        public string? Adress { get; set; }

        /// <summary>
        /// Gets or sets the latitude of the booking location.
        /// </summary>
        public string? Lat { get; set; }

        /// <summary>
        /// Gets or sets the longitude of the booking location.
        /// </summary>
        public string? Lng { get; set; }

        /// <summary>
        /// Gets or sets the directions to the booking location.
        /// </summary>
        public string? Directions { get; set; }

        /// <summary>
        /// Gets or sets the payment instructions for the booking.
        /// </summary>
        public string? PaymentInsturctions { get; set; }

        /// <summary>
        /// Gets or sets the start date of the booking.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of the booking.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the quantity of rooms booked.
        /// </summary>
        public double RoomQty { get; set; }

        /// <summary>
        /// Gets or sets the rate ID associated with the booking.
        /// </summary>
        public int RateId { get; set; }

        /// <summary>
        /// Gets or sets the description of the rate associated with the booking.
        /// </summary>
        public string? RateDescription { get; set; }

        /// <summary>
        /// Gets or sets the number of adults in the booking.
        /// </summary>
        public int Adults { get; set; }

        /// <summary>
        /// Gets or sets the number of children in the booking.
        /// </summary>
        public int Children { get; set; }

        /// <summary>
        /// Gets or sets the number of infants in the booking.
        /// </summary>
        public int Infants { get; set; }

        /// <summary>
        /// Gets or sets the cancellation ID associated with the booking.
        /// </summary>
        public int CancellationId { get; set; }

        /// <summary>
        /// Gets or sets the booking status, which is dynamically determined based on the end date.
        /// </summary>
        private BookingStatus _bookingStatus = BookingStatus.Pending;

        /// <summary>
        /// Gets or sets the current status of the booking.
        /// </summary>
        public BookingStatus BookingStatus
        {
            get
            {
                if (EndDate > DateTime.Now)
                    return _bookingStatus;
                return BookingStatus.Completed;
            }
            set
            {
                _bookingStatus = value;
            }
        }

        #endregion

        #region Navigation Properties

        /// <summary>
        /// Gets or sets the foreign key reference to the associated room.
        /// </summary>
        [ForeignKey(nameof(Room))]
        public int? RoomId { get; set; }

        /// <summary>
        /// Gets or sets the associated room entity.
        /// </summary>
        public Room? Room { get; set; }

        /// <summary>
        /// Gets or sets the foreign key reference to the associated package.
        /// </summary>
        [ForeignKey(nameof(Package))]
        public int? PackageId { get; set; }

        /// <summary>
        /// Gets or sets the associated package entity.
        /// </summary>
        public LodgingPackage? Package { get; set; }

        /// <summary>
        /// Gets or sets the foreign key reference to the associated lodging.
        /// </summary>
        [ForeignKey(nameof(Lodging))]
        public string? LodgingId { get; set; }

        /// <summary>
        /// Gets or sets the associated lodging entity.
        /// </summary>
        public Lodging? Lodging { get; set; }

        /// <summary>
        /// Gets or sets the foreign key reference to the associated user.
        /// </summary>
        [ForeignKey(nameof(User))]
        public string? UserId { get; set; }

        /// <summary>
        /// Gets or sets the associated user entity.
        /// </summary>
        public ApplicationUser? User { get; set; }

        /// <summary>
        /// Gets or sets the foreign key reference to the associated order.
        /// </summary>
        [ForeignKey(nameof(Order))]
        public string? OrderNr { get; set; }

        /// <summary>
        /// Gets or sets the associated order entity.
        /// </summary>
        public Order? Order { get; set; }

        #endregion
    }
}
