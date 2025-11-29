using ConectOne.Domain.Entities;

namespace ShoppingModule.Domain.Entities
{
	/// <summary>
	/// Represents a discount coupon that can be applied to reduce the cost of a purchase.
	/// </summary>
	/// <remarks>A coupon has a unique code, a discount percentage, and an optional deactivation date.  The <see
	/// cref="Active"/> property indicates whether the coupon is currently valid for use.</remarks>
    public class Coupon : EntityBase<string>
	{
		/// <summary>
		/// Gets or sets the code associated with this instance.
		/// </summary>
		public string Code { get; set; }

		/// <summary>
		/// Gets or sets the discount percentage to be applied to the total price.
		/// </summary>
		public double DiscountPercentage{ get; set; }

		/// <summary>
		/// Gets or sets the date and time when the entity was deactivated.
		/// </summary>
		public DateTime? DeActivationDate { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the entity is active.
		/// </summary>
        public bool Active 
		{
			get
			{
				if(DeActivationDate is null || DeActivationDate > DateTime.Now)
				{
					return active;
				}
				return false;
			}
			set 
			{
				active = value;
			} 
		}
		private bool active = true;
    }
}
