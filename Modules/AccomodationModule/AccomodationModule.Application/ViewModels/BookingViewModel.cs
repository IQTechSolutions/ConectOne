using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Enums;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// Represents a view model for booking information, providing details about a booking such as  reference ID,
    /// contact information, dates, room quantity, and guest details.
    /// </summary>
    /// <remarks>This class is designed to encapsulate booking-related data for use in presentation layers or 
    /// data transfer scenarios. It includes properties for essential booking details, such as  start and end dates,
    /// guest counts, and payment instructions. Additionally, it provides  methods for converting the view model into a
    /// domain booking object.</remarks>
    public class BookingViewModel
    {
        #region Constructors
              
        /// <summary>
        /// Initializes a new instance of the <see cref="BookingViewModel"/> class.
        /// </summary>
        /// <remarks>This constructor creates a default instance of the <see cref="BookingViewModel"/>
        /// class. Use this constructor when no initial data is required for the booking view model.</remarks>
        public BookingViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BookingViewModel"/> class using the specified booking data.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see cref="BookingDto"/> to the
        /// corresponding properties of the <see cref="BookingViewModel"/>. It is intended to create a view model
        /// representation of a booking for use in presentation layers.</remarks>
        /// <param name="booking">The booking data transfer object containing details about the booking.</param>
		public BookingViewModel(BookingDto booking)
		{
            BookingReferenceId = booking.BookingReferenceNr;
            Name = booking.Name;
            Contacts = booking.Contacts;
            PhoneNr = booking.PhoneNr;
            Email = booking.Email;
            Website = booking.Website;
            Adress = booking.Address;
            Lat = booking.Lat;
            Lng = booking.Lng;
            Directions = booking.Directions;
            PaymentInsturctions = booking.PaymentInstructions;
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

        /// <summary>
        /// Gets or sets the unique identifier for a booking.
        /// </summary>
		public int BookingId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for a booking reference.
        /// </summary>
        public string BookingReferenceId { get; set; }

        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
		public string Name { get; set; }

        /// <summary>
        /// Gets or sets the contact information associated with the entity.
        /// </summary>
        public string? Contacts { get; set; }

        /// <summary>
        /// Gets or sets the phone number associated with the entity.
        /// </summary>
        public string? PhoneNr { get; set; }

        /// <summary>
        /// Gets or sets the email address associated with the user.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the URL of the website associated with the entity.
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// Gets or sets the address associated with the entity.
        /// </summary>
        public string? Adress { get; set; }

        /// <summary>
        /// Gets or sets the latitude value of a geographic location.
        /// </summary>
        public string? Lat { get; set; }

        /// <summary>
        /// Gets or sets the language code associated with the current context.
        /// </summary>
        public string? Lng { get; set; }

        /// <summary>
        /// Gets or sets the directions for completing a task or navigating a route.
        /// </summary>
        public string? Directions { get; set; }

        /// <summary>
        /// Gets or sets the payment instructions associated with the transaction.
        /// </summary>
        public string? PaymentInsturctions { get; set; }

        /// <summary>
        /// Gets or sets the start date of the event or process.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of the event or time period.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the quantity of rooms.
        /// </summary>
        public double RoomQty { get; set; }

        /// <summary>
        /// Gets or sets the number of adults associated with the current context.
        /// </summary>
        public int Adults { get; set; }

        /// <summary>
        /// Gets or sets the number of children associated with the entity.
        /// </summary>
        public int Children { get; set; }

        /// <summary>
        /// Gets or sets the number of infants included in the booking.
        /// </summary>
        public int Infants { get; set; }

        /// <summary>
        /// Gets or sets the current status of the booking.
        /// </summary>
        public BookingStatus BookingStatus { get; set; } = BookingStatus.Pending;

        /// <summary>
        /// Gets or sets the description of the rate associated with the current context.
        /// </summary>
        public string? RateDescription { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the rate.
        /// </summary>
        public int RateId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the room.
        /// </summary>
        public int? RoomId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the package.
        /// </summary>
        public int? PackageId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for a partner.
        /// </summary>
        public string? UniquePartnerId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for a lodging entity.
        /// </summary>
        public string? LodgingId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// Converts the current instance to a <see cref="Booking"/> object.
        /// </summary>
        /// <remarks>This method creates a new <see cref="Booking"/> instance and populates its properties
        /// using the corresponding values from the current object. The conversion ensures that all relevant details,
        /// such as contact information, location, and booking specifics, are transferred to the resulting <see
        /// cref="Booking"/> object.</remarks>
        /// <returns>A <see cref="Booking"/> object containing the data from the current instance.</returns>
        public Booking ToBooking()
        {
            return new Booking()
            {
                Name = this.Name,
                Contacts = this.Contacts,
                PhoneNr = this.PhoneNr,
                Email = this.Email,
                Website = this.Website,
                Adress = this.Adress,
                Lat = this.Lat.ToString(),
                Lng = this.Lng.ToString(),
                Directions = this.Directions,
                PaymentInsturctions = this.PaymentInsturctions,
                StartDate = this.StartDate,
                EndDate = this.StartDate,
                RoomQty = this.RoomQty,
                Adults = this.Adults,
                Children = this.Children,
                Infants = this.Infants,
                BookingStatus = this.BookingStatus,
                RoomId = this.RoomId,
                LodgingId = this.LodgingId,
                PackageId = this.PackageId,
                UserId = this.UserId
            };
        }
    }
}
