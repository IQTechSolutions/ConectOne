using System.ComponentModel.DataAnnotations;
using ConectOne.Application.ViewModels;
using ConectOne.Domain.Enums;
using ShoppingModule.Domain.Enums;

namespace ShoppingModule.Application.ViewModels
{
    /// <summary>
    /// Represents the data required for a checkout process, including customer details, shipping and billing
    /// information, payment method, and cart items.
    /// </summary>
    /// <remarks>This view model is used to collect and validate user input during the checkout process. It
    /// includes properties for customer details (e.g., name, email, phone number), shipping and billing addresses,
    /// selected shipping and payment options, and the items in the shopping cart. Calculated properties provide summary
    /// information such as discounts, totals, and VAT.</remarks>
    public class CheckoutViewModel
    { 
        /// <summary>
        /// Gets or sets the unique identifier for the beneficiary.
        /// </summary>
        public string? BenificiaryId { get; set; }

        /// <summary>
        /// Gets or sets the first name of the individual.
        /// </summary>
        [Required] public string FirstName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the last name of the individual.
        /// </summary>
        [Required] public string LastName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        public string? CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the email address associated with the entity.
        /// </summary>
        [Required] public string Email { get; set; } = null!;

        /// <summary>
        /// Gets or sets the phone number associated with the entity.
        /// </summary>
        [Required] public string PhoneNr { get; set; } = null!;

        /// <summary>
        /// Gets or sets the coupon code associated with a discount or promotion.
        /// </summary>
        public string? CouponCode { get; set; }

        /// <summary>
        /// Gets or sets the discount percentage applied when a coupon is used.
        /// </summary>
        public double CouponDiscountPercentage { get; set; } = 0;

        public ShippingOption Shipping { get; set; } = new ShippingOption();

        /// <summary>
        /// Gets or sets the shipping option for the current order.
        /// </summary>
        public CardDetails CardDetails { get; set; } = new CardDetails();

        /// <summary>
        /// Gets or sets the list of available shipping options for an order.
        /// </summary>
        public List<ShippingOption> ShippingOptions { get; set; } = [];

        /// <summary>
        /// Gets or sets the shipping address associated with the order.
        /// </summary>
        public AddressViewModel? ShippingAddress { get; set; } = new AddressViewModel() { AddressType = AddressType.Shipping };

        /// <summary>
        /// Gets or sets the billing address associated with the current entity.
        /// </summary>
        public AddressViewModel? BillingAddress { get; set; } = new AddressViewModel() { AddressType = AddressType.Billing };

        /// <summary>
        /// Gets or sets a value indicating whether the billing address is the same as the shipping address.
        /// </summary>
        public bool BillingAddressSameAsShippingAddress { get; set; } = true;

        /// <summary>
        /// Gets or sets the payment method used for the transaction.
        /// </summary>
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;

        /// <summary>
        /// Gets or sets the comments associated with this entity.
        /// </summary>
        [DataType(DataType.MultilineText)] public string? Comments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has agreed to the privacy policy.
        /// </summary>
        public bool AgreeToPrivacyPolicy { get; set; }

        /// <summary>
        /// Gets or sets the collection of items in the shopping cart.
        /// </summary>
        public IEnumerable<CartItemViewModel> CartItems { get; set; } = new List<CartItemViewModel>();

        #region Read Only
               
        /// <summary>
        /// Gets the total discount applied to all items in the cart.
        /// </summary>
        public double ProductDiscount => CartItems.Sum(c => c.Qty* c.Discount);

        /// <summary>
        /// Gets the total discount applied, combining product-specific discounts and coupon discounts.
        /// </summary>
        public double TotalDiscount => ProductDiscount + CouponDiscount;

        /// <summary>
        /// Gets the total price of all items in the cart, excluding tax.
        /// </summary>
        public double TotalExcl => CartItems.Sum(c => c.Qty * c.PriceExcl);

        /// <summary>
        /// Gets the discount amount applied to the total, based on the coupon discount percentage.
        /// </summary>
        public double CouponDiscount => TotalExcl * (CouponDiscountPercentage / 100);

        /// <summary>
        /// Gets the subtotal amount, calculated as the total excluding taxes minus the total discount.
        /// </summary>
        public double SubTotal => TotalExcl - TotalDiscount;

        /// <summary>
        /// Gets the total value-added tax (VAT) for the current transaction.
        /// </summary>
        public double TotalVat => GrandTotal - SubTotal ;

        /// <summary>
        /// Gets the grand total value.
        /// </summary>
        public double GrandTotal => 0;

        #endregion
    }

    /// <summary>
    /// Represents a shipping option available for a delivery or collection service.
    /// </summary>
    /// <remarks>This class provides details about a specific shipping option, including its code, name,
    /// description,  applicable delivery and collection dates, and associated costs such as VAT and total
    /// amount.</remarks>
    public class ShippingOption
    {
        /// <summary>
        /// Gets or sets the code associated with this instance.
        /// </summary>
        public string Code { get; set; } = null!;

        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the description associated with the object.
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// Gets or sets the earliest allowable delivery date for an order.
        /// </summary>
        public DateTime DeliveryDateFrom { get; set; }

        /// <summary>
        /// Gets or sets the latest allowable delivery date for an order.
        /// </summary>
        public DateTime DeliveryDateTo { get; set; }

        /// <summary>
        /// Gets or sets the starting date for the collection period.
        /// </summary>
        public DateTime CollectionDateFrom { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the collection was performed.
        /// </summary>
        public DateTime CollectionDate { get; set; }

        /// <summary>
        /// Gets or sets the cutoff time for collection operations.
        /// </summary>
        public DateTime CollectionCutOffTime { get; set; }

        /// <summary>
        /// Gets or sets the value-added tax (VAT) percentage.
        /// </summary>
        public double Vat { get; set; }

        /// <summary>
        /// Gets or sets the amount associated with the operation.
        /// </summary>
        public double Ammount { get; set; }

    }

    /// <summary>
    /// Represents the details of a payment card, including card number, expiration date, and CVV.
    /// </summary>
    /// <remarks>This class is typically used to store and transfer payment card information securely. Ensure
    /// that sensitive data, such as the card number and CVV, is handled in compliance with  applicable security
    /// standards (e.g., PCI DSS).</remarks>
    public class CardDetails
    {
        /// <summary>
        /// Gets or sets the name of the cardholder associated with the payment method.
        /// </summary>
        public string CardHolderName { get; set; }

        /// <summary>
        /// Gets or sets the card number associated with the entity.
        /// </summary>
        public string CardNumber { get; set; } = null!;

        /// <summary>
        /// Gets or sets the expiry date of the item.
        /// </summary>
        public string ExpiryDate { get; set; } = null!;

        /// <summary>
        /// Gets or sets the card verification value (CVV) associated with a payment card.
        /// </summary>
        /// <remarks>The CVV is used to verify the authenticity of the card during transactions.  Ensure
        /// this value is handled securely and not stored in a persistent manner to comply with payment security
        /// standards.</remarks>
        public string Cvv { get; set; } = null!;
    }
}
