using System.Globalization;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Enums;

namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Data Transfer Object (DTO) for booking information.
    /// This DTO is used to transfer booking data between different layers of the application.
    /// </summary>
    public record BookingDto
    {
        #region Constructors

        /// <summary>
        /// Default constructor for creating a new instance of <see cref="BookingDto"/>.
        /// </summary>
        public BookingDto() { }

        /// <summary>
        /// Initializes a new instance of <see cref="BookingDto"/> using a <see cref="LodgingDto"/>.
        /// </summary>
        /// <param name="lodging">The lodging DTO containing lodging details.</param>
        public BookingDto(LodgingDto lodging)
        {
            BookingReferenceNr = "";
            Name = lodging.Name;
            Contacts = lodging.Contacts;
            PhoneNr = lodging.PhoneNr;
            Email = lodging.Email;
            Website = lodging.Website;
            Address = lodging.Address;
            Lat = lodging.Lat.ToString(CultureInfo.InvariantCulture);
            Lng = lodging.Lng.ToString(CultureInfo.InvariantCulture);
            Directions = lodging.Directions;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="BookingDto"/> using a <see cref="Booking"/>.
        /// </summary>
        /// <param name="booking">The booking entity containing booking details.</param>
        public BookingDto(Booking booking)
        {
            BookingId = booking.Id.ToString();
            BookingReferenceNr = booking.BookingReferenceNr;
            OrderNr = booking.OrderNr;
            Name = booking.Name;
            Contacts = booking.Contacts;
            PhoneNr = booking.PhoneNr;
            Email = booking.Email;
            Website = booking.Website;
            Address = booking.Adress;
            Lat = booking.Lat;
            Lng = booking.Lng;
            Directions = booking.Directions;
            PaymentInstructions = booking.PaymentInsturctions;
            StartDate = booking.StartDate;
            EndDate = booking.StartDate;
            RoomQty = booking.RoomQty;
            Adults = booking.Adults;
            Children = booking.Children;
            Infants = booking.Infants;
            BookingStatus = booking.BookingStatus;
            RoomId = booking.RoomId;
            LodgingId = booking.LodgingId;
            PackageId = booking.PackageId;
            UserId = booking.UserId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the booking ID.
        /// </summary>
        public string? BookingId { get; set; }

        /// <summary>
        /// Gets or sets the booking reference number.
        /// </summary>
        public string BookingReferenceNr { get; set; } = null!;

        /// <summary>
        /// Gets or sets the order number.
        /// </summary>
        public string OrderNr { get; set; } = null!;

        /// <summary>
        /// Gets or sets the name associated with the booking.
        /// </summary>
        public string Name { get; init; } = null!;

        /// <summary>
        /// Gets or sets the contacts associated with the booking.
        /// </summary>
        public string? Contacts { get; init; }

        /// <summary>
        /// Gets or sets the phone number associated with the booking.
        /// </summary>
        public string? PhoneNr { get; init; }

        /// <summary>
        /// Gets or sets the email address associated with the booking.
        /// </summary>
        public string? Email { get; init; }

        /// <summary>
        /// Gets or sets the website associated with the booking.
        /// </summary>
        public string? Website { get; init; }

        /// <summary>
        /// Gets or sets the address associated with the booking.
        /// </summary>
        public string? Address { get; init; }

        /// <summary>
        /// Gets or sets the latitude of the booking location.
        /// </summary>
        public string? Lat { get; init; }

        /// <summary>
        /// Gets or sets the longitude of the booking location.
        /// </summary>
        public string? Lng { get; init; }

        /// <summary>
        /// Gets or sets the directions to the booking location.
        /// </summary>
        public string? Directions { get; init; }

        /// <summary>
        /// Gets or sets the payment instructions for the booking.
        /// </summary>
        public string? PaymentInstructions { get; init; }

        /// <summary>
        /// Gets or sets the start date of the booking.
        /// </summary>
        public DateTime StartDate { get; init; }

        /// <summary>
        /// Gets or sets the end date of the booking.
        /// </summary>
        public DateTime EndDate { get; init; }

        /// <summary>
        /// Gets the number of nights for the booking.
        /// </summary>
        public int Nights => (EndDate.Date - StartDate.Date).Days;

        /// <summary>
        /// Gets or sets the quantity of rooms booked.
        /// </summary>
        public double RoomQty { get; init; }

        /// <summary>
        /// Gets or sets the number of adults for the booking.
        /// </summary>
        public int Adults { get; init; }

        /// <summary>
        /// Gets or sets the number of children for the booking.
        /// </summary>
        public int Children { get; init; }

        /// <summary>
        /// Gets or sets the number of infants for the booking.
        /// </summary>
        public int Infants { get; init; }

        /// <summary>
        /// Gets or sets the booking status.
        /// </summary>
        public BookingStatus BookingStatus { get; init; } = BookingStatus.Pending;

        /// <summary>
        /// Gets or sets the rate description for the booking.
        /// </summary>
        public string? RateDescription { get; init; }

        /// <summary>
        /// Gets or sets the rate ID for the booking.
        /// </summary>
        public int RateId { get; set; }

        /// <summary>
        /// Gets or sets the room ID for the booking.
        /// </summary>
        public int? RoomId { get; init; }

        /// <summary>
        /// Gets or sets the package ID for the booking.
        /// </summary>
        public int? PackageId { get; init; }

        /// <summary>
        /// Gets or sets the unique partner ID for the booking.
        /// </summary>
        public string? UniquePartnerId { get; init; }

        /// <summary>
        /// Gets or sets the lodging ID for the booking.
        /// </summary>
        public string? LodgingId { get; init; }

        /// <summary>
        /// Gets or sets the user ID associated with the booking.
        /// </summary>
        public string? UserId { get; init; }

        /// <summary>
        /// Gets or sets the amount due excluding taxes and fees.
        /// </summary>
        public double AmountDueExcl { get; set; }

        /// <summary>
        /// Gets or sets the amount due including taxes and fees.
        /// </summary>
        public double AmountDueIncl { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the <see cref="BookingDto"/> to a <see cref="Booking"/> entity.
        /// </summary>
        /// <returns>A <see cref="Booking"/> entity with the same data.</returns>
        public Booking ToBooking()
        {
            return new Booking()
            {
                BookingReferenceNr = BookingReferenceNr,
                Name = Name,
                Contacts = Contacts,
                PhoneNr = PhoneNr,
                Email = Email,
                Website = Website,
                Adress = Address,
                Lat = Lat,
                Lng = Lng,
                Directions = Directions,
                PaymentInsturctions = PaymentInstructions,
                StartDate = StartDate,
                EndDate = StartDate,
                RoomQty = RoomQty,
                Adults = Adults,
                Children = Children,
                Infants = Infants,
                BookingStatus = BookingStatus,
                RoomId = RoomId,
                LodgingId = LodgingId,
                PackageId = PackageId,
                UserId = UserId
            };
        }

        #endregion
    }
}
