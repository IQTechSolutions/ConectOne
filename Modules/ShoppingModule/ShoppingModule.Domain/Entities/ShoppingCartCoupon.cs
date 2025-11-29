using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;

namespace ShoppingModule.Domain.Entities
{
    /// <summary>
    /// Represents a coupon associated with a shopping cart.
    /// </summary>
    /// <remarks>This class links a specific coupon to a shopping cart, allowing the application to track and
    /// apply discounts or promotions to the cart. It includes references to both the shopping cart and the coupon
    /// entities.</remarks>
    public class ShoppingCartCoupon : EntityBase<string>
	{
        /// <summary>
        /// Gets or sets the unique identifier for the shopping cart.
        /// </summary>
		public string ShoppingCartId { get; set; }

        /// <summary>
        /// Gets or sets the shopping cart associated with this coupon.
        /// </summary>
		[ForeignKey(nameof(Coupon))] public string CouponId { get; set; }

        /// <summary>
        /// Gets or sets the coupon associated with the current transaction.
        /// </summary>
		public Coupon Coupon { get; set; }

        /// <summary>
        /// Returns a string representation of the shopping cart coupon.
        /// </summary>
        /// <returns>A string that represents the shopping cart coupon.</returns>
        public override string ToString()
        {
            return $"Shoppping Cart Coupon";
        }
    }
}
