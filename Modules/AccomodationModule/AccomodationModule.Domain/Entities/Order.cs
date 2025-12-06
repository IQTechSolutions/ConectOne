using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents an order containing customer details, financial information, and associated bookings and vouchers.
    /// </summary>
    /// <remarks>The <see cref="Order"/> class encapsulates information about a customer's order, including
    /// personal details,  financial calculations such as subtotals, VAT, discounts, and the total amount due. It also
    /// tracks associated  bookings and vouchers applied to the order.</remarks>
    public class Order : EntityBase<string>
    {
        /// <summary>
        /// Gets or sets the first name of the individual.
        /// </summary>
        public string FirstName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the last name of a person.
        /// </summary>
        public string LastName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the email address associated with the user.
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// Gets or sets the phone number associated with the entity.
        /// </summary>
        public string PhoneNr { get; set; } = null!;

        /// <summary>
        /// Gets or sets the subtotal amount excluding taxes.
        /// </summary>
        public double SubTotalExcl { get; set; }

        /// <summary>
        /// Gets or sets the VAT (Value Added Tax) percentage.
        /// </summary>
        public double Vat { get; set; }

        /// <summary>
        /// Gets or sets the subtotal amount, including applicable taxes.
        /// </summary>
        public double SubTotalIncl { get; set; }

        /// <summary>
        /// Gets or sets the discount percentage applied to the total price.
        /// </summary>
        public double Discount { get; set; }

        /// <summary>
        /// Gets or sets the total amount due for a transaction.
        /// </summary>
        public double TotalDue { get; set; }

        /// <summary>
        /// Gets or sets the collection of bookings.
        /// </summary>
        public List<Booking> Bookings { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of vouchers associated with the user.
        /// </summary>
        public List<UserVoucher> Vouchers { get; set; } = [];

    }
}