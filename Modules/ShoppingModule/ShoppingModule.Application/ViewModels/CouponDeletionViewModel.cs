namespace ShoppingModule.Application.ViewModels
{
    /// <summary>
    /// Represents the data required to delete a coupon.
    /// </summary>
    /// <remarks>This view model is typically used to pass the identifier of a coupon that needs to be deleted
    /// in an operation.</remarks>
    public class CouponDeletionViewModel 
    {
        /// <summary>
        /// Gets or sets the unique identifier for the coupon.
        /// </summary>
        public string CouponId { get; set; }
    }
}
