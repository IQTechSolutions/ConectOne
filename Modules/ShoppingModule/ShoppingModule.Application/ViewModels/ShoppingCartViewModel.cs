namespace ShoppingModule.Application.ViewModels
{
    /// <summary>
    /// Represents the view model for a shopping cart, containing details about the items, discounts,  and associated
    /// address information.
    /// </summary>
    /// <remarks>This class is used to encapsulate the state of a shopping cart, including the items it
    /// contains,  any applied discounts, and address-related details such as country, province, and postal code.  It
    /// also provides properties for calculating totals, such as the subtotal, VAT, and total discount.</remarks>
    public class ShoppingCartViewModel
    {
        /// <summary>
        /// Gets or sets the collection of items in the shopping cart.
        /// </summary>
        public ICollection<CartItemViewModel> CartItems { get; set; } = new List<CartItemViewModel>();

        /// <summary>
        /// Gets or sets the coupon code associated with a discount or promotion.
        /// </summary>
        public string CouponCode { get; set; }

        /// <summary>
        /// Gets or sets the name of the country associated with this entity.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the province associated with the entity.
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// Gets or sets the name of the city associated with the current entity.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the name of the suburb associated with the address.
        /// </summary>
        public string Suburb { get; set; }

        /// <summary>
        /// Gets or sets the postal code associated with the address.
        /// </summary>
        public string PostalCode { get; set; }       

        /// <summary>
        /// Gets or sets the discount percentage to be applied to the total price.
        /// </summary>
        public double Discount { get; set; } = 0;

        /// <summary>
        /// Gets or sets the total number of items.
        /// </summary>
        public double ItemCount { get; set; }

        /// <summary>
        /// Gets or sets the subtotal amount for the current transaction.
        /// </summary>
        public double SubTotal { get; set; }

        /// <summary>
        /// Gets or sets the value-added tax (VAT) percentage.
        /// </summary>
        public double Vat { get; set; }

        /// <summary>
        /// Gets or sets the discount percentage applied to the product.
        /// </summary>
        public double ProductDiscount { get; set; }

        /// <summary>
        /// Gets or sets the total discount applied to the current transaction.
        /// </summary>
        public double TotalDiscount { get; set; }

        /// <summary>
        /// Gets or sets the total value represented as a double.
        /// </summary>
        public double Total { get; set; }
    }
}
