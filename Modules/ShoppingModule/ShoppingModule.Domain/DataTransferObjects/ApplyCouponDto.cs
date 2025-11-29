namespace ShoppingModule.Domain.DataTransferObjects
{
    /// <summary>
    /// The data transfer object used to transfer information about a coupon to be applied <see cref="ApplyCouponDto"/>
    /// </summary>
    public record ApplyCouponDto
	{
		/// <summary>
		/// The code of the coupon code to be applied
		/// </summary>
		public string? CouponCode { get; init; }
		
		/// <summary>
		/// The shoppling cart id the coupon should be applied to
		/// </summary>
		public string? ShoppingCartId { get; init; }
	}
}
