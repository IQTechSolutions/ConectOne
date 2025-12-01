using System.ComponentModel.DataAnnotations;
using ShoppingModule.Domain.DataTransferObjects;

namespace ShoppingModule.Application.ViewModels
{
	/// <summary>
	/// Represents a view model for a coupon, encapsulating its details and state.
	/// </summary>
	/// <remarks>This class provides properties to manage and display coupon information, including its code, 
	/// discount percentage, activation status, and deactivation date. It also includes a read-only  property to determine
	/// whether the coupon is currently active based on its activation status  and deactivation date.</remarks>
	public class CouponViewModel
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="CouponViewModel"/> class.
		/// </summary>
		public CouponViewModel() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="CouponViewModel"/> class using the specified coupon data.
		/// </summary>
		/// <remarks>This constructor maps the properties of the provided <see cref="CouponDto"/> to the corresponding
		/// properties of the <see cref="CouponViewModel"/> instance.</remarks>
		/// <param name="coupon">The data transfer object containing coupon details. Must not be <see langword="null"/>.</param>
		public CouponViewModel(CouponDto coupon)
		{
			CouponId = coupon.CouponId;
			Code = coupon.Code;
			DiscountPercentage = coupon.DiscountPercentage;
			DeActivationDate = coupon.DeActivationDate;
			Active = coupon.Active;
		}

		#endregion

		/// <summary>
		/// Gets or sets the identifier of the coupon associated with the current operation.
		/// </summary>
		public string? CouponId { get; set; }

		/// <summary>
		/// Gets or sets the coupon code associated with the discount.
		/// </summary>
		[MaxLength(500, ErrorMessage = "Maximum lenght for the coupon code is 500 characters")]
		[Required(ErrorMessage = "The coupon code is required")]
		public string Code { get; set; }

		/// <summary>
		/// Gets or sets the discount percentage to be applied to the total price.
		/// </summary>
		public double DiscountPercentage { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the entity was deactivated.
		/// </summary>
		public DateTime? DeActivationDate { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the entity is active.
		/// </summary>
		public bool Active { get; set; } = true;


		#region Read Only

		/// <summary>
		/// Gets a value indicating whether the entity is currently active.
		/// </summary>
		public bool IsActive
		{
			get
			{
				if (Active)
				{
					if (DeActivationDate.HasValue && DeActivationDate.Value.Date > DateTime.Now.Date)
					{
						return true;
					}
				}
				return false;

			}
		}

		#endregion

        #region Methods

		/// <summary>
		/// Converts the current coupon entity to a data transfer object (DTO).
		/// </summary>
		/// <returns>A <see cref="CouponDto"/> instance containing the coupon's identifier, code, discount percentage, deactivation
		/// date, and active status.</returns>
        public CouponDto ToDto()
		{
			return new CouponDto
			{
				CouponId = this.CouponId,
				Code = this.Code,
				DiscountPercentage = this.DiscountPercentage,
				DeActivationDate = this.DeActivationDate,
				Active = this.Active
			};
        }

        #endregion
    }
}
