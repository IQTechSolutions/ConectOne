using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using ShoppingModule.Domain.DataTransferObjects;
using ShoppingModule.Domain.Entities;
using ShoppingModule.Domain.Interfaces;

namespace ShoppingModule.Infrastructure.Implementation
{
    /// <summary>
    /// Provides functionality for managing and applying coupons within the system.
    /// </summary>
    /// <remarks>The <see cref="CouponService"/> class offers methods to create, retrieve, update, delete, and
    /// apply coupons. It interacts with the underlying repository to perform operations on coupon data and ensures that
    /// the results are returned in a consistent format. This service is designed to handle business logic related to
    /// coupons, including validation and mapping to DTOs.</remarks>
    /// <param name="repository"></param>
    public sealed class CouponService(IRepository<Coupon, string> repository, IRepository<ShoppingCartCoupon, string> shoppingCartCouponRepo) : ICouponService
    {
        /// <summary>
        /// Retrieves a list of available coupons.
        /// </summary>
        /// <remarks>This method fetches all available coupons and maps them to a collection of <see
        /// cref="CouponDto"/> objects. The returned result indicates whether the operation was successful and provides
        /// the corresponding data or error messages.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// where T is an <see cref="IEnumerable{T}"/> of <see cref="CouponDto"/> objects. If the operation succeeds,
        /// the result contains the list of coupons; otherwise, it contains error messages.</returns>
        public async Task<IBaseResult<IEnumerable<CouponDto>>> CouponsAsync(CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Coupon>(c => true);

            var coupons = await repository.ListAsync(spec, false, cancellationToken);         
            if(coupons.Succeeded)
            {
                return await Result<IEnumerable<CouponDto>>.SuccessAsync(coupons.Data.Select(c => new CouponDto(c)));
            }
            return await Result<IEnumerable<CouponDto>>.FailAsync(coupons.Messages);
        }

        /// <summary>
        /// Retrieves a coupon by its identifier and returns the result as a <see cref="CouponDto"/>.
        /// </summary>
        /// <remarks>This method retrieves the coupon data from the repository and maps it to a <see
        /// cref="CouponDto"/>. If the coupon retrieval fails, the result will include the failure messages.</remarks>
        /// <param name="couponId">The unique identifier of the coupon to retrieve. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// where <c>T</c> is <see cref="CouponDto"/>. If the operation succeeds, the result contains the coupon data;
        /// otherwise, it contains error messages.</returns>
        public async Task<IBaseResult<CouponDto>> CouponAsync(string couponId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Coupon>(c => c.Id == couponId);
            var coupon = await repository.FirstOrDefaultAsync(spec, true, cancellationToken);
            if (coupon.Succeeded)
            {
                return await Result<CouponDto>.SuccessAsync(new CouponDto(coupon.Data));
            }
            return await Result<CouponDto>.FailAsync(coupon.Messages);
        }

        /// <summary>
        /// Retrieves a coupon by its unique code.
        /// </summary>
        /// <remarks>This method retrieves a coupon based on the provided code and returns the result
        /// wrapped in  an <IBaseResult{T}>. If the coupon is found, the result will include the coupon  details;
        /// otherwise, it will include failure messages.</remarks>
        /// <param name="code">The unique code of the coupon to retrieve. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <IBaseResult{T}> of type
        /// <CouponDto>. If the operation succeeds,  the result contains the coupon data; otherwise, it contains error
        /// messages.</returns>
        public async Task<IBaseResult<CouponDto>> CouponByCodeAsync(string code, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Coupon>(c => c.Code == code);
            var coupon = await repository.FirstOrDefaultAsync(spec, true, cancellationToken);

            if (coupon.Succeeded)
                return await Result<CouponDto>.SuccessAsync(new CouponDto(coupon.Data));
            return await Result<CouponDto>.FailAsync(coupon.Messages);
        }

        /// <summary>
        /// Creates a new coupon and saves it to the repository asynchronously.
        /// </summary>
        /// <remarks>This method attempts to create a coupon using the provided <CouponDto> and  persists
        /// it to the repository. If an exception occurs during the operation, the method  returns a failure result
        /// containing the exception message.</remarks>
        /// <param name="coupon">The data transfer object representing the coupon to be created.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <IBaseResult{T}/> object,
        /// which includes the created <CouponDto>  if the operation is successful, or an error message if it fails.</returns>
        public async Task<IBaseResult<CouponDto>> CreateCouponAsync(CouponDto coupon, CancellationToken cancellationToken = default)
        {
            var newCoupon = coupon.CreateCoupon();
            var result = await repository.CreateAsync(newCoupon, cancellationToken);
            if(result.Succeeded)
                return await Result<CouponDto>.SuccessAsync(new CouponDto(newCoupon));
            return await Result<CouponDto>.FailAsync(result.Messages);
        }

        /// <summary>
        /// Applies a coupon to the specified shopping cart.
        /// </summary>
        /// <remarks>This method attempts to retrieve the coupon by its code and, if successful,
        /// associates it with the specified shopping cart. The operation will fail if the coupon code is invalid or if
        /// any other issue occurs during the process.</remarks>
        /// <param name="model">An <see cref="ApplyCouponDto"/> object containing the coupon code and the shopping cart ID to which the
        /// coupon should be applied.</param>
        /// <returns>An <see cref="IBaseResult"/> indicating the outcome of the operation.  Returns a success result if the
        /// coupon is successfully applied, or a failure result with error messages if the operation fails.</returns>
        public async Task<IBaseResult> ApplyCouponAsync(ApplyCouponDto model, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Coupon>(c => c.Code == model.CouponCode);
            var coupon = await repository.FirstOrDefaultAsync(spec, true, cancellationToken);

            await shoppingCartCouponRepo.CreateAsync(new ShoppingCartCoupon() { CouponId = coupon.Data.Id, ShoppingCartId = model.ShoppingCartId }, cancellationToken);
            var result = await repository.SaveAsync(cancellationToken);
            if(result.Succeeded)
                return await Result.SuccessAsync("Coupon was succesfully applied");
            return await Result.FailAsync(coupon.Messages);
        }

        /// <summary>
        /// Removes a coupon from the specified shopping cart.
        /// </summary>
        /// <remarks>This method attempts to remove a coupon associated with the specified shopping cart. 
        /// If the operation fails, the returned result will contain the error message.</remarks>
        /// <param name="model">An object containing the details of the shopping cart and coupon to be removed.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the success or failure of the operation. On success, the result includes a success message;  on
        /// failure, it includes an error message.</returns>
        public async Task<IBaseResult> RemoveCouponAsync(ApplyCouponDto model, CancellationToken cancellationToken = default)
        {
            await repository.DeleteAsync(model.ShoppingCartId, cancellationToken);
            var result = await repository.SaveAsync(cancellationToken);
            if(result.Succeeded)
                return await Result.SuccessAsync("Coupon was successfully removed");
            return Result.Fail(result.Messages);
        }

        /// <summary>
        /// Updates an existing coupon with the specified details.
        /// </summary>
        /// <remarks>This method updates the coupon's code, discount percentage, deactivation date, and
        /// active status.  Ensure that the <see cref="CouponDto.CouponId"/> provided corresponds to an existing coupon
        /// in the repository.</remarks>
        /// <param name="coupon">The <see cref="CouponDto"/> containing the updated coupon details. The <see cref="CouponDto.CouponId"/> must
        /// correspond to an existing coupon.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// of type <see cref="CouponDto"/>: <list type="bullet"> <item><description>A successful result containing the
        /// updated coupon if the operation completes successfully.</description></item> <item><description>A failed
        /// result with an error message if the operation encounters an error.</description></item> </list></returns>
        public async Task<IBaseResult<CouponDto>> UpdateCouponAsync(CouponDto couponDto, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Coupon>(c => c.Id == couponDto.CouponId);
            var coupon = await repository.FirstOrDefaultAsync(spec, true, cancellationToken);

            coupon.Data.Code = coupon.Data.Id;
            coupon.Data.DiscountPercentage = coupon.Data.DiscountPercentage;
            coupon.Data.DeActivationDate = coupon.Data.DeActivationDate;
            coupon.Data.Active = coupon.Data.Active;

            repository.Update(coupon.Data);
            var saveResult = await repository.SaveAsync(cancellationToken);
            if(saveResult.Succeeded)
                return await Result<CouponDto>.SuccessAsync(new CouponDto(coupon.Data));
            return await Result<CouponDto>.FailAsync(saveResult.Messages);
        }

        /// <summary>
        /// Deletes a coupon asynchronously based on the specified coupon ID.
        /// </summary>
        /// <remarks>This method attempts to delete a coupon identified by the provided <paramref
        /// name="couponId"/>.  If the coupon is found and successfully removed, the operation succeeds with a success
        /// message.  Otherwise, the operation fails, and the result contains the relevant error messages.</remarks>
        /// <param name="couponId">The unique identifier of the coupon to be deleted. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the success or failure of the operation. If successful, the result includes a success message; 
        /// otherwise, it contains error messages describing the failure.</returns>
        public async Task<IBaseResult> DeleteAsync(string couponId, CancellationToken cancellationToken = default)
        {
            await shoppingCartCouponRepo.DeleteAsync(couponId, cancellationToken);

            var result = await repository.SaveAsync(cancellationToken);
            if (result.Succeeded)
                return await Result.SuccessAsync("Coupon was successfully removed");
            return await Result.FailAsync(result.Messages);
        }
    }
}
