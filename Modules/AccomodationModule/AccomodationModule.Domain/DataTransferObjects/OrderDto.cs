using AccomodationModule.Domain.Entities;

namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents an Order Data Transfer Object (DTO) used for transferring order-related data between
    /// different layers of the application (e.g., from the database entity to the UI or API response).
    /// </summary>
    public class OrderDto
    {
        #region Constructors

        /// <summary>
        /// Default constructor required for serialization or manual property setting.
        /// Initializes a new, empty OrderDto.
        /// </summary>
        public OrderDto() { }

        /// <summary>
        /// Constructs an OrderDto from a given Order entity.
        /// Maps the properties of the Order entity to this DTO, including its bookings.
        /// </summary>
        /// <param name="order">The Order entity from which to construct the DTO.</param>
        public OrderDto(Order order)
        {
            // Basic customer-related information
            Id = order.Id;
            FirstName = order.FirstName;
            LastName = order.LastName;
            Email = order.Email;
            PhoneNr = order.PhoneNr;

            // Financial details
            SubTotalExcl = order.SubTotalExcl;
            Vat = order.Vat;
            SubTotalIncl = order.SubTotalIncl;
            Discount = order.Discount;
            TotalDue = order.TotalDue;

            // Convert the bookings from the Order entity to BookingDto instances
            Bookings = order.Bookings.Select(c => new BookingDto(c)).ToList();

            // Note: The Vouchers property is not populated here as the original constructor
            // did not account for them. This ensures backward compatibility, but you may need 
            // to handle it separately if required.
        }

        /// <summary>
        /// Constructs an OrderDto with all detailed parameters set.
        /// This constructor is useful when you have all data readily available and want to create an OrderDto in one go.
        /// </summary>
        /// <param name="id">A unique identifier (usually a string guid) for the order.</param>
        /// <param name="firstName">The first name of the person placing the order.</param>
        /// <param name="lastName">The last name of the person placing the order.</param>
        /// <param name="email">The email address associated with the order (often the customer's email).</param>
        /// <param name="phoneNr">The phone number of the customer placing the order.</param>
        /// <param name="subTotalExcl">The subtotal amount before VAT is applied.</param>
        /// <param name="vat">The VAT (Value Added Tax) amount applied to the order.</param>
        /// <param name="subTotalIncl">The subtotal amount including VAT (subTotalExcl + vat).</param>
        /// <param name="discount">The total discount applied to the order, if any.</param>
        /// <param name="totalDue">The final amount due after all calculations (subtotal incl VAT minus discounts).</param>
        /// <param name="bookings">A list of bookings associated with this order.</param>
        /// <param name="vouchers">A list of vouchers associated with this order.</param>
        public OrderDto(string id, string firstName, string lastName, string email, string phoneNr, double subTotalExcl, double vat, double subTotalIncl, double discount, double totalDue, List<BookingDto> bookings, List<UserVoucherDto> vouchers)
        {
            Id = id;

            // Customer details
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNr = phoneNr;

            // Financial details
            SubTotalExcl = subTotalExcl;
            Vat = vat;
            SubTotalIncl = subTotalIncl;
            Discount = discount;
            TotalDue = totalDue;

            // Related entities
            Bookings = bookings;
            Vouchers = vouchers;
        }

        #endregion

        /// <summary>
        /// Unique identifier for the order, often a GUID represented as a string.
        /// </summary>
        public string Id { get; init; } = null!;

        /// <summary>
        /// The first name of the customer placing the order.
        /// </summary>
        public string FirstName { get; set; } = null!;

        /// <summary>
        /// The last name of the customer placing the order.
        /// </summary>
        public string LastName { get; set; } = null!;

        /// <summary>
        /// The email address associated with the order, typically the customer's email.
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// The phone number of the customer placing the order.
        /// </summary>
        public string PhoneNr { get; set; } = null!;

        /// <summary>
        /// The subtotal amount before applying VAT.
        /// </summary>
        public double SubTotalExcl { get; init; }

        /// <summary>
        /// The amount of VAT applied to the order.
        /// </summary>
        public double Vat { get; init; }

        /// <summary>
        /// The subtotal amount including VAT (SubtotalExcl + Vat).
        /// </summary>
        public double SubTotalIncl { get; init; }

        /// <summary>
        /// The discount amount (if any) applied to the order.
        /// </summary>
        public double Discount { get; init; }

        /// <summary>
        /// The final amount the customer must pay after all discounts and VAT have been applied.
        /// </summary>
        public double TotalDue { get; init; }

        /// <summary>
        /// A list of bookings associated with this order. Each booking includes details like lodging, room, and stay dates.
        /// </summary>
        public List<BookingDto> Bookings { get; init; } = [];

        /// <summary>
        /// A list of user vouchers associated with this order, representing voucher-based deals or promotional items.
        /// </summary>
        public List<UserVoucherDto> Vouchers { get; init; } = [];

        /// <summary>
        /// Converts the current OrderDto back into an Order domain entity, suitable for persistence in the database or further processing by business logic.
        /// </summary>
        /// <returns>An Order entity reflecting the data in this DTO.</returns>
        public Order ToOrder()
        {
            return new Order()
            {
                Id = this.Id,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Email = this.Email,
                PhoneNr = this.PhoneNr,

                SubTotalExcl = this.SubTotalExcl,
                Vat = this.Vat,
                SubTotalIncl = this.SubTotalIncl,
                Discount = this.Discount,
                TotalDue = this.TotalDue,

                // Convert each BookingDto back to a Booking entity
                Bookings = this.Bookings?.Select(c => c.ToBooking())?.ToList(),

                // Convert each UserVoucherDto to a UserVoucher entity for persistence
                Vouchers = Vouchers?.Select(c => new UserVoucher
                {
                    Id = c.UserVoucherId,
                    UserId = c.UserId,
                    RoomId = c.Room.RoomTypeId!.Value,   // Assumes RoomTypeId is not null
                    VoucherId = c.Voucher.VoucherId!.Value // Assumes VoucherId is not null
                })?.ToList()
            };
        }
    }
}
