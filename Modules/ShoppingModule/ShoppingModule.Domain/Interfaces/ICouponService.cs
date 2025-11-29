using ConectOne.Domain.ResultWrappers;
using ShoppingModule.Domain.DataTransferObjects;

namespace ShoppingModule.Domain.Interfaces
{
    /// <summary>
    /// Defines a contract for managing and interacting with coupons in the system.
    /// </summary>
    /// <remarks>This service provides methods for retrieving, creating, updating, deleting, and applying
    /// coupons. It is designed to handle operations related to coupon management, including fetching coupons by ID or
    /// code, and applying or removing coupons for specific use cases.</remarks>
    public interface ICouponService
    {
        /// <summary>
        /// Retrieves a collection of available coupons asynchronously.
        /// </summary>
        /// <remarks>The returned collection may be empty if no coupons are available. Ensure to check the
        /// result's status  and data before processing.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// with an enumerable collection of <see cref="CouponDto"/> objects representing the available coupons.</returns>
        Task<IBaseResult<IEnumerable<CouponDto>>> CouponsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the details of a coupon based on the provided coupon identifier.
        /// </summary>
        /// <remarks>Use this method to fetch information about a specific coupon, such as its discount
        /// value, expiration date, and applicable conditions. Ensure the <paramref name="couponId"/> is valid and
        /// corresponds to an existing coupon.</remarks>
        /// <param name="couponId">The unique identifier of the coupon to retrieve. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping a <see cref="CouponDto"/> with the coupon details, or an error if the operation fails.</returns>
        Task<IBaseResult<CouponDto>> CouponAsync(string couponId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new coupon asynchronously.
        /// </summary>
        /// <param name="coupon">The <see cref="CouponDto"/> object containing the details of the coupon to be created. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the created <see cref="CouponDto"/> if the operation is successful.</returns>
        Task<IBaseResult<CouponDto>> CreateCouponAsync(CouponDto coupon, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing coupon with the provided details.
        /// </summary>
        /// <remarks>Ensure that the <paramref name="coupon"/> object contains valid and complete data
        /// before calling this method. The operation may fail if the coupon does not exist or if the provided data
        /// violates business rules.</remarks>
        /// <param name="coupon">The <see cref="CouponDto"/> object containing the updated coupon details. Must not be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the updated <see cref="CouponDto"/> if the operation is successful.</returns>
        Task<IBaseResult<CouponDto>> UpdateCouponAsync(CouponDto coupon, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a coupon with the specified identifier asynchronously.
        /// </summary>
        /// <remarks>Use this method to remove a coupon from the system. Ensure that the provided
        /// <paramref name="couponId"/>  corresponds to an existing coupon. The operation is performed asynchronously
        /// and may involve network or  database interactions.</remarks>
        /// <param name="couponId">The unique identifier of the coupon to delete. This value cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the delete operation.</returns>
        Task<IBaseResult> DeleteAsync(string couponId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a coupon by its unique code.
        /// </summary>
        /// <param name="code">The unique code of the coupon to retrieve. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <IBaseResult{T}> of type
        /// <CouponDto> representing the coupon details  if the code is valid; otherwise, an appropriate error result.</returns>
        Task<IBaseResult<CouponDto>> CouponByCodeAsync(string code, CancellationToken cancellationToken = default);

        /// <summary>
        /// Applies a coupon to the current user's account or order.
        /// </summary>
        /// <remarks>Ensure that the <paramref name="model"/> contains all required fields before calling
        /// this method.  The result of the operation will provide details about whether the coupon was successfully
        /// applied  or if any errors occurred.</remarks>
        /// <param name="model">An object containing the details of the coupon to be applied, such as the coupon code and any additional
        /// required information.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the success or failure of the operation, along with any relevant messages or data.</returns>
        Task<IBaseResult> ApplyCouponAsync(ApplyCouponDto model, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a previously applied coupon from the system asynchronously.
        /// </summary>
        /// <remarks>Use this method to remove a coupon that was previously applied. Ensure that the
        /// <paramref name="model"/>  contains valid information about the coupon to be removed.</remarks>
        /// <param name="model">The data transfer object containing details of the coupon to be removed.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the success or failure of the operation.</returns>
        Task<IBaseResult> RemoveCouponAsync(ApplyCouponDto model, CancellationToken cancellationToken = default);
    }
}
