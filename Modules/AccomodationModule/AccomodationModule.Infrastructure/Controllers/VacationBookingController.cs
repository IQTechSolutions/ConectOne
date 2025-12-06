using AccomodationModule.Domain.Arguments;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using AccomodationModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Mvc;

namespace AccomodationModule.Infrastructure.Controllers
{
    /// <summary>
    /// API controller for managing vacation bookings.
    /// Provides endpoints for creating and retrieving vacation bookings.
    /// </summary>
    [Route("api/vacationBookings"), ApiController]
    public class VacationBookingController(IVacationBookingService vacationBookingService) : ControllerBase
    {
        #region Methods

        /// <summary>
        /// Retrieves a paginated list of vacation bookings based on the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters for pagination and filtering.</param>
        /// <returns>An <see cref="IActionResult"/> containing the paginated list of vacation bookings.</returns>
        [HttpGet] public async Task<IActionResult> PagedVacationBookingsAsync(VacationBookingPageParams parameters)
        {
            var vacationBookings = await vacationBookingService.PagedAsync(parameters, HttpContext.RequestAborted);
            return Ok(vacationBookings);
        }

        [HttpGet("all")]
        public async Task<IActionResult> AllVacationBookingsAsync(VacationBookingPageParams parameters)
        {
            var vacationBookings = await vacationBookingService.GetAllAsync(HttpContext.RequestAborted);
            return Ok(vacationBookings);
        }

        /// <summary>
        /// Retrieves the details of a vacation booking by its unique identifier.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch the vacation booking details
        /// from the service. The result is returned as an HTTP 200 OK response if the booking is found.</remarks>
        /// <param name="vacationBookingId">The unique identifier of the vacation booking to retrieve. This value cannot be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> containing the details of the vacation booking if found, or an appropriate
        /// HTTP response if not.</returns>
        [HttpGet("{vacationBookingId}")]
        public async Task<IActionResult> VacationBookingAsync(string vacationBookingId)
        {
            var vacationBookings = await vacationBookingService.GetByIdAsync(vacationBookingId, HttpContext.RequestAborted);
            return Ok(vacationBookings);
        }

        /// <summary>
        /// Creates a new vacation booking based on the provided details.
        /// </summary>
        /// <remarks>This method processes the provided vacation booking details and adds them to the
        /// system.  The operation respects the cancellation token provided by the HTTP context.</remarks>
        /// <param name="vacationBooking">The vacation booking details to be created. This parameter must not be <see langword="null"/>.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation. Typically, this will be an HTTP 200
        /// response with the created booking details.</returns>
        [HttpPut] public async Task<IActionResult> CreateVacationBookingAsync([FromBody] VacationBookingDto vacationBooking)
        {
            var result = await vacationBookingService.AddAsync(vacationBooking, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new vacation booking.
        /// </summary>
        /// <param name="vacationBooking">The vacation booking DTO containing booking details.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPost] public async Task<IActionResult> UpdateVacationBookingAsync([FromBody] VacationBookingDto vacationBooking)
        {
            var result = await vacationBookingService.AddAsync(vacationBooking, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Deletes an existing vacation booking.
        /// </summary>
        /// <remarks>The operation is asynchronous and respects the cancellation token provided by the
        /// HTTP context. Ensure that the <paramref name="vacationBooking"/> parameter contains valid data for the
        /// booking to be deleted.</remarks>
        /// <param name="vacationBooking">The vacation booking to be deleted. This must be provided as a <see cref="VacationBookingDto"/> object
        /// containing the details of the booking to remove.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Typically, this will be an HTTP 200
        /// OK response if the deletion is successful.</returns>
        [HttpDelete] public async Task<IActionResult> DeleteVacationBookingAsync(string id)
        {
            var result = await vacationBookingService.DeleteAsync(id, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Marks a vacation booking as active.
        /// </summary>
        /// <param name="vacationBookingId">The ID of the vacation booking to mark as active.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPost("markAsActive/{vacationBookingId}")]
        public async Task<IActionResult> MarkAsReadAsync(string vacationBookingId)
        {
            var result = await vacationBookingService.MarkAsActiveAsync(vacationBookingId, HttpContext.RequestAborted);
            return Ok(result);
        }

        #endregion
    }
}
