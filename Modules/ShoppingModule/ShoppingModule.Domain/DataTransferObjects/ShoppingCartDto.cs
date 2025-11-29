namespace ShoppingModule.Domain.DataTransferObjects
{    
    /// <summary>
    /// Represents a shopping cart containing items, discounts, and calculated totals.
    /// </summary>
    /// <remarks>This class provides a data transfer object (DTO) for a shopping cart, including details about
    /// the  items in the cart, applied coupon, and various calculated totals such as subtotal, VAT, discounts,  and the
    /// final total. It is designed to encapsulate the state of a shopping cart for use in  e-commerce applications or
    /// similar systems.</remarks>
    public class ShoppingCartDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the shopping cart.
        /// </summary>
        public string ShoppingCartId { get; set; }

        /// <summary>
        /// Gets or sets the coupon associated with the current transaction.
        /// </summary>
        public CouponDto Coupon { get; set; }

        /// <summary>
        /// Gets or sets the collection of items in the shopping cart.
        /// </summary>
        public ICollection<ShoppingCartItemDto> Items { get; set; } = new List<ShoppingCartItemDto>();

        #region Read Only   

        /// <summary>
        /// Gets the total count of items by summing their quantities.
        /// </summary>
        public double ItemCount => Items.Sum(c => c.Qty);

        /// <summary>
        /// Gets the subtotal of all items, calculated as the sum of the quantity multiplied by the price (excluding
        /// tax) for each item.
        /// </summary>
        public double SubTotal => Items.Sum(c => c.Qty * c.PriceExcl);

        /// <summary>
        /// Gets the total VAT (Value Added Tax) for all items in the collection.
        /// </summary>
        public double Vat => Items.Sum(c => c.Qty* c.Vat);

        /// <summary>
        /// Gets the total discount applied to all items in the collection.
        /// </summary>
        public double ProductDiscount => Items.Sum(c => c.Qty* c.Discount);

        /// <summary>
        /// Gets the discount amount applied to the subtotal based on the current coupon.
        /// </summary>
        public double CouponDiscount 
        {
            get
            {
                if (Coupon is not null)
                    return SubTotal * (Coupon.DiscountPercentage/100);
                return 0;
            }            
        }

        /// <summary>
        /// Gets the total discount applied to the purchase.
        /// </summary>
        public double TotalDiscount => ProductDiscount + CouponDiscount;

        /// <summary>
        /// Gets the total amount after applying VAT and discounts, rounded to two decimal places.
        /// </summary>
        public double Total => Math.Round(SubTotal + Vat - TotalDiscount, 2);

        #endregion
    }
}
