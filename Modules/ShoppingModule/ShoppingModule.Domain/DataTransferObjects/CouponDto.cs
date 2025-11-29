using ShoppingModule.Domain.Entities;

namespace ShoppingModule.Domain.DataTransferObjects
{
	/// <summary>
	/// Represents a data transfer object (DTO) for a coupon, encapsulating its key properties and providing methods for
	/// conversion to domain models.
	/// </summary>
	/// <remarks>This DTO is designed to facilitate the transfer of coupon data between different layers of the
	/// application, such as between the domain model and the presentation layer. It includes properties for the coupon's
	/// unique identifier, code, discount percentage, activation status, and deactivation date.</remarks>
    public record CouponDto
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="CouponDto"/> class.
		/// </summary>
		public CouponDto() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="CouponDto"/> class using the specified <see cref="Coupon"/> object.
		/// </summary>
		/// <param name="coupon">The <see cref="Coupon"/> object containing the data to initialize the DTO. Cannot be <see langword="null"/>.</param>
		public CouponDto(Coupon coupon)
		{
			CouponId = coupon.Id;
			Code = coupon.Code;
			DiscountPercentage = coupon.DiscountPercentage;
			Active = coupon.Active;
			DeActivationDate = coupon.DeActivationDate;
		}

		#endregion

		/// <summary>
		/// Gets the unique identifier of the coupon associated with the current operation.
		/// </summary>
		public string? CouponId { get; init; }

		/// <summary>
		/// Gets the unique code associated with this instance.
		/// </summary>
		public string Code { get; init; }

		/// <summary>
		/// Gets the discount percentage to be applied to the total price.
		/// </summary>
		public double DiscountPercentage { get; init; }

		/// <summary>
		/// Gets or sets the date and time when the entity was deactivated.
		/// </summary>
        public DateTime? DeActivationDate { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the entity is active.
		/// </summary>
        public bool Active { get; set; }

        #region Methods

		/// <summary>
		/// Creates a new <see cref="Coupon"/> instance with the current object's properties.
		/// </summary>
		/// <remarks>The returned <see cref="Coupon"/> object will have its properties set to match the current
		/// instance,  including <see cref="Id"/>, <see cref="Code"/>, <see cref="DiscountPercentage"/>, <see cref="Active"/>,
		/// and <see cref="DeActivationDate"/>.</remarks>
		/// <returns>A <see cref="Coupon"/> object initialized with the values of the current instance's properties.</returns>
        public Coupon CreateCoupon()
		{
			return new Coupon()
			{
				Id = this.CouponId,
				Code = this.Code,
				DiscountPercentage = this.DiscountPercentage,
				Active = this.Active,
				DeActivationDate = this.DeActivationDate
			};
		}

		#endregion

		
	}
}
