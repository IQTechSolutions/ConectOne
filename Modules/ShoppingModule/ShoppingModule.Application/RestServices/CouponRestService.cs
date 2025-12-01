using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ShoppingModule.Domain.DataTransferObjects;
using ShoppingModule.Domain.Interfaces;

namespace ShoppingModule.Application.RestServices
{
    /// <summary>
    /// Provides methods for managing and interacting with coupons in a RESTful service.
    /// </summary>
    /// <remarks>This service allows clients to perform operations such as retrieving, creating, updating, and
    /// deleting coupons,  as well as applying and removing coupons. It communicates with the underlying HTTP provider
    /// to execute these operations.</remarks>
    /// <param name="provider"></param>
    public class CouponRestService(IBaseHttpProvider provider) : ICouponService
    {
        /// <summary>
        /// Retrieves a collection of available coupons asynchronously.
        /// </summary>
        /// <remarks>This method fetches the list of coupons from the underlying provider. If no coupons
        /// are available, the result may contain an empty collection.</remarks>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// containing an enumerable collection of <see cref="CouponDto"/> objects.</returns>
        public async Task<IBaseResult<IEnumerable<CouponDto>>> CouponsAsync(CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<CouponDto>>("coupons");
            return result;
        }

        /// <summary>
        /// Retrieves the details of a coupon by its unique identifier.
        /// </summary>
        /// <remarks>This method uses the underlying provider to fetch the coupon details. Ensure that the
        /// <paramref name="couponId"/> corresponds to a valid coupon in the system. The operation may fail if the
        /// coupon does not exist or if there are connectivity issues.</remarks>
        /// <param name="couponId">The unique identifier of the coupon to retrieve. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the details of the coupon, or an error result if the operation fails.</returns>
        public async Task<IBaseResult<CouponDto>> CouponAsync(string couponId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<CouponDto>($"coupons/{couponId}");
            return result;
        }

        /// <summary>
        /// Creates a new coupon asynchronously.
        /// </summary>
        /// <remarks>This method sends the provided coupon data to the underlying provider for creation.
        /// Ensure that the  <paramref name="coupon"/> object contains all required fields before calling this
        /// method.</remarks>
        /// <param name="coupon">The <see cref="CouponDto"/> object containing the details of the coupon to be created.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation. This parameter is optional and
        /// defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// where <c>T</c> is <see cref="CouponDto"/>. The result includes the created coupon details if the operation
        /// succeeds.</returns>
        public async Task<IBaseResult<CouponDto>> CreateCouponAsync(CouponDto coupon, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<CouponDto, CouponDto>("coupons", coupon);
            return result;
        }

        /// <summary>
        /// Updates an existing coupon with the specified details.
        /// </summary>
        /// <remarks>This method sends the updated coupon details to the underlying provider for
        /// processing. Ensure that the <paramref name="coupon"/> object contains valid data before calling this
        /// method.</remarks>
        /// <param name="coupon">The <see cref="CouponDto"/> object containing the updated details of the coupon.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation. The default value is <see
        /// cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// object with the updated <see cref="CouponDto"/> if the operation is successful.</returns>
        public async Task<IBaseResult<CouponDto>> UpdateCouponAsync(CouponDto coupon, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<CouponDto, CouponDto>("coupons", coupon);
            return result;
        }

        /// <summary>
        /// Deletes a coupon asynchronously.
        /// </summary>
        /// <param name="couponId">The unique identifier of the coupon to delete. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the delete operation.</returns>
        public async Task<IBaseResult> DeleteAsync(string couponId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync("coupons", couponId);
            return result;
        }

        /// <summary>
        /// Retrieves a coupon by its unique code.
        /// </summary>
        /// <remarks>This method sends a request to retrieve a coupon based on the provided code. If the
        /// coupon does not exist, the result may indicate a failure or contain no data, depending on the implementation
        /// of the underlying provider.</remarks>
        /// <param name="code">The unique code of the coupon to retrieve. This value cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping the <see cref="CouponDto"/> if the coupon is found.</returns>
        public async Task<IBaseResult<CouponDto>> CouponByCodeAsync(string code, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<CouponDto>($"coupons/code/{code}");
            return result;
        }

        /// <summary>
        /// Applies a coupon to the current context asynchronously.
        /// </summary>
        /// <remarks>This method sends the coupon details to the underlying provider for processing.
        /// Ensure that the <paramref name="model"/> contains valid coupon information before calling this
        /// method.</remarks>
        /// <param name="model">The data transfer object containing the coupon details to be applied.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the coupon application.</returns>
        public async Task<IBaseResult> ApplyCouponAsync(ApplyCouponDto model, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"coupons/apply", model);
            return result;
        }

        /// <summary>
        /// Removes a coupon from the system asynchronously.
        /// </summary>
        /// <remarks>This method sends a request to remove the specified coupon. Ensure that the <paramref
        /// name="model"/> contains valid data before calling this method.</remarks>
        /// <param name="model">An object containing the details of the coupon to be removed.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveCouponAsync(ApplyCouponDto model, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"coupons/remove", model);
            return result;
        }
    }
}
