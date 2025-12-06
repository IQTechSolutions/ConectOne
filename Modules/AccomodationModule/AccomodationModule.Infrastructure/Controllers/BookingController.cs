using AccomodationModule.Domain.Arguments;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccomodationModule.Infrastructure.Controllers
{
    /// <summary>
    /// Controller responsible for handling booking-related API requests.
    /// </summary>
    [Route($"api/bookings")]
    [ApiController]
    public class BookingController(IBookingService bookingService) : ControllerBase
    {
        /// <summary>
        /// Retrieves the total count of bookings that match the given filtering and paging parameters.
        /// Useful for pagination scenarios, where the client needs to know how many items are available in total.
        /// </summary>
        /// <param name="parameters">Query parameters that may include filters (e.g., date ranges, status) and pagination settings.</param>
        /// <returns>An <see cref="IActionResult"/> containing the total count of bookings as an integer.</returns>
        [HttpGet("count")] public async Task<IActionResult> BookingsCount([FromQuery] BookingParameters parameters)
        {
            var result = await bookingService.BookingsCountAsync(parameters, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a paged list of bookings that match the given parameters.
        /// The result may contain partial sets of bookings, useful for displaying pages of data in a UI.
        /// </summary>
        /// <param name="parameters">Query parameters such as page number, page size, and other optional filters.</param>
        /// <returns>A paged list of booking data, typically a DTO containing a collection of bookings and pagination info.</returns>
        [HttpGet] public async Task<IActionResult> PagedBookingsAsync([FromQuery] BookingParameters parameters)
        {
            var result = await bookingService.PagedBookingsAsync(parameters, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves detailed information about a single booking, identified by its booking ID.
        /// </summary>
        /// <param name="bookingId">The unique identifier for the booking to retrieve.</param>
        /// <returns>A detailed <see cref="BookingDto"/> containing full booking data.</returns>
        [HttpGet("details/{bookingId}")] public async Task<IActionResult> Booking(string bookingId)
        {
            var result = await bookingService.BookingAsync(bookingId, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves all bookings associated with a given order number.
        /// An order may contain multiple bookings.
        /// </summary>
        /// <param name="orderNr">The unique order number associated with one or more bookings.</param>
        /// <returns>A collection of bookings associated with the specified order number.</returns>
        [HttpGet("bookingList/{orderNr}")] public async Task<IActionResult> BookingsOrder(string orderNr)
        {
            var result = await bookingService.BookingsOrder(orderNr, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Marks an order (and its associated bookings) as complete, indicating that the booking process is finalized.
        /// This might be used after payment confirmation or manual verification steps.
        /// </summary>
        /// <param name="orderNr">The unique order number to complete.</param>
        /// <returns>A result indicating success or failure of the completion operation.</returns>
        [HttpPost("complete/{orderNr}")] public async Task<IActionResult> CompleteBookingsOrder(string orderNr)
        {
            return Ok(await bookingService.CompleteBookingsOrder(orderNr, HttpContext.RequestAborted));
        }

        /// <summary>
        /// Creates a new booking record in the system. Typically invoked after verifying availability and confirming details.
        /// </summary>
        /// <param name="booking">A <see cref="BookingDto"/> containing all details required to create a booking (e.g., user info, dates, lodging details).</param>
        /// <returns>The newly created booking details or a success/failure result.</returns>
        [HttpPut] public async Task<IActionResult> CreateBooking([FromBody] BookingDto booking)
        {
            return Ok(await bookingService.CreateBooking(booking, HttpContext.RequestAborted));
        }

        /// <summary>
        /// Cancels an existing booking. The operation may involve checking cancellation policies, fees, or other conditions.
        /// </summary>
        /// <param name="cancellation">A <see cref="CancelBookingDto"/> containing the booking ID and any necessary cancellation info (e.g., reason).</param>
        /// <returns>A result indicating the success or failure of the cancellation request.</returns>
        [HttpPost("cancel")] public async Task<IActionResult> CancelBooking([FromBody] CancelBookingDto cancellation)
        {
            return Ok(await bookingService.CancelBooking(cancellation, HttpContext.RequestAborted));
        }

        /// <summary>
        /// Retrieves the order details associated with a given order number.
        /// Orders encapsulate one or more bookings, payment info, and other order-level data.
        /// </summary>
        /// <param name="orderNr">The unique order number to query.</param>
        /// <returns>Information about the specified order, including associated bookings and payment details.</returns>
        [HttpGet("order/{orderNr}")] public async Task<IActionResult> Order(string orderNr)
        {
            var result = await bookingService.OrderAsync(orderNr, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new order in the system. This usually happens after selecting multiple bookings or vouchers
        /// and before making a payment. The order serves as a wrapper for all these items.
        /// </summary>
        /// <param name="model">An <see cref="OrderDto"/> containing information such as customer details, list of bookings, and total amounts.</param>
        /// <returns>A result indicating success or failure of creating the order. May also return the newly created order details.</returns>
        [HttpPut("order")] public async Task<IActionResult> CreateOrder([FromBody] OrderDto model)
        {
            return Ok(await bookingService.CreateOrder(model, HttpContext.RequestAborted));
        }
    }
}
