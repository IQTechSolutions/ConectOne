using Microsoft.AspNetCore.Mvc;
using ShoppingModule.Domain.DataTransferObjects;
using ShoppingModule.Domain.Interfaces;

namespace ShoppingModule.Infrastructure.Controllers;

/// <summary>
/// Provides endpoints for managing coupons, including retrieval, creation, updating, and deletion.
/// </summary>
/// <remarks>This controller handles operations related to coupons, such as retrieving all coupons, retrieving a
/// specific coupon by ID or code,  applying or removing a coupon, and creating, updating, or deleting coupons. All
/// endpoints return appropriate HTTP responses  based on the operation's result.</remarks>
/// <param name="service"></param>
[Route("api/coupons"), ApiController]
public class CouponController(ICouponService service) : ControllerBase
{
    /// <summary>
    /// Retrieves a list of available coupons.
    /// </summary>
    /// <remarks>This method asynchronously fetches coupon data and returns it in the HTTP response. The
    /// response contains the list of coupons in a format determined by the service layer.</remarks>
    /// <returns>An <see cref="IActionResult"/> containing the list of coupons.  Returns an HTTP 200 OK response with the coupon
    /// data if successful.</returns>
	[HttpGet] public async Task<IActionResult> Coupons()
    {
        var result = await service.CouponsAsync();
        return Ok(result);
    }

    /// <summary>
    /// Retrieves the details of a specific coupon by its unique identifier.
    /// </summary>
    /// <remarks>This method calls the underlying service to fetch the coupon details asynchronously.  The
    /// result is returned as an HTTP 200 response with the coupon data if successful.</remarks>
    /// <param name="couponId">The unique identifier of the coupon to retrieve. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> containing the coupon details if found, or an appropriate HTTP response if not.</returns>
	[HttpGet("{couponId}")] public async Task<IActionResult> Coupon(string couponId)
    {
        var result = await service.CouponAsync(couponId);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a coupon by its unique code.
    /// </summary>
    /// <remarks>This method calls the underlying service to fetch the coupon details associated with the
    /// specified code. Ensure the <paramref name="code"/> parameter is valid and corresponds to an existing
    /// coupon.</remarks>
    /// <param name="code">The unique code of the coupon to retrieve. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> containing the coupon details if found, or an appropriate HTTP response if not.</returns>
	[HttpGet("code/{code}")] public async Task<IActionResult> CouponByCode(string code)
    {
        var result = await service.CouponByCodeAsync(code);
        return Ok(result);
    }

    /// <summary>
    /// Applies a coupon to the current user's account or order.
    /// </summary>
    /// <remarks>This method processes the provided coupon code and applies it to the user's account or order 
    /// based on the business logic defined in the service layer. Ensure that the <paramref name="model"/>  contains
    /// valid data before calling this method.</remarks>
    /// <param name="model">The data transfer object containing the coupon code and any additional required information.</param>
    /// <returns>An <see cref="IActionResult"/> containing the result of the coupon application.  Typically, this includes a
    /// success status and any relevant response data.</returns>
    [HttpPost("apply")] public async Task<IActionResult> ApplyCoupon([FromBody] ApplyCouponDto model)
    {
        var response = await service.ApplyCouponAsync(model);
        return Ok(response);
    }

    /// <summary>
    /// Removes an applied coupon from the user's account or order.
    /// </summary>
    /// <remarks>This method is invoked via an HTTP POST request to the "remove" endpoint. Ensure that the 
    /// <paramref name="model"/> parameter contains valid coupon information before calling this method.</remarks>
    /// <param name="model">The data transfer object containing details of the coupon to be removed.</param>
    /// <returns>An <see cref="IActionResult"/> containing the result of the operation. Typically, this includes a success or
    /// failure response with relevant details.</returns>
    [HttpPost("remove")] public async Task<IActionResult> RemoveCoupon([FromBody] ApplyCouponDto model)
    {
        var response = await service.RemoveCouponAsync(model);
        return Ok(response);
    }

    /// <summary>
    /// Creates a new coupon based on the provided data.
    /// </summary>
    /// <remarks>This method processes an HTTP POST request to create a new coupon. The coupon details are
    /// provided in the request body.</remarks>
    /// <param name="coupon">The data transfer object containing the details of the coupon to be created. Cannot be null.</param>
    /// <returns>An <see cref="IActionResult"/> containing the created coupon if the operation is successful.</returns>
    [HttpPost] public async Task<IActionResult> CreateCoupon([FromBody] CouponDto coupon)
    {
        var createdCoupon = await service.CreateCouponAsync(coupon);
        return Ok(createdCoupon);
    }

    /// <summary>
    /// Updates an existing coupon with the provided details.
    /// </summary>
    /// <remarks>The provided <paramref name="couponDto"/> must contain valid data for the update to succeed.
    /// Ensure that all required fields are populated and meet the expected format.</remarks>
    /// <param name="couponDto">The data transfer object containing the updated details of the coupon.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the update operation.  Typically returns an HTTP 200 OK
    /// response with the updated coupon details.</returns>
    [HttpPut] public async Task<IActionResult> UpdateCoupon([FromBody] CouponDto couponDto)
    {
        var pp = await service.UpdateCouponAsync(couponDto);
        return Ok(pp);
    }

    /// <summary>
    /// Deletes a coupon with the specified identifier.
    /// </summary>
    /// <remarks>This method performs an HTTP DELETE operation to remove a coupon from the system.  Ensure
    /// that the <paramref name="couponId"/> corresponds to an existing coupon.</remarks>
    /// <param name="couponId">The unique identifier of the coupon to delete. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.  Returns an HTTP 200 OK response with the
    /// deleted coupon if the operation is successful.</returns>
    [HttpDelete("{couponId}")] public async Task<IActionResult> DeleteCoupon(string couponId)
    {
        var coupon = await service.DeleteAsync(couponId);
        return Ok(coupon);
    }
}
