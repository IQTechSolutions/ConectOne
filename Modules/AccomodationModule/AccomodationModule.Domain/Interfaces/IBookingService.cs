using AccomodationModule.Domain.Arguments;
using AccomodationModule.Domain.DataTransferObjects;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Domain.Interfaces
{
    /// <summary>
    /// Defines the contract for a service that manages booking operations.
    /// This interface provides methods for retrieving, creating, completing, and canceling bookings and orders.
    /// </summary>
    public interface IBookingService
    {
        /// <summary>
        /// Asynchronously retrieves the total count of bookings based on the specified parameters.
        /// </summary>
        /// <param name="pageParameters">The parameters used to filter and refine the booking count query. Cannot be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// with the total count of bookings matching the specified parameters.</returns>
        Task<IBaseResult<int>> BookingsCountAsync(BookingParameters pageParameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a paginated list of bookings based on the provided parameters.
        /// </summary>
        /// <param name="pageParameters">The parameters used for pagination and filtering.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>An <see cref="PaginatedResult{T}"/> containing the paginated list of <see cref="BookingDto"/>.</returns>
        Task<PaginatedResult<BookingDto>> PagedBookingsAsync(BookingParameters pageParameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a specific booking by its ID.
        /// </summary>
        /// <param name="bookingId">The ID of the booking to retrieve.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>An <see cref="IBaseResult{T}"/> containing the <see cref="BookingDto"/> for the specified booking ID.</returns>
        Task<IBaseResult<BookingDto>> BookingAsync(string bookingId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all bookings associated with a specific order number.
        /// </summary>
        /// <param name="orderNr">The order number to filter bookings by.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>An <see cref="IBaseResult{T}"/> containing a collection of <see cref="BookingDto"/> associated with the order number.</returns>
        Task<IBaseResult<IEnumerable<BookingDto>>> BookingsOrder(string orderNr, CancellationToken cancellationToken = default);

        /// <summary>
        /// Completes all bookings associated with a specific order number.
        /// </summary>
        /// <param name="orderNr">The order number to complete bookings for.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>An <see cref="IBaseResult"/> indicating the success or failure of the operation.</returns>
        Task<IBaseResult> CompleteBookingsOrder(string orderNr, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new booking.
        /// </summary>
        /// <param name="booking">The <see cref="BookingDto"/> containing the data for the new booking.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>An <see cref="IBaseResult"/> indicating the success or failure of the booking creation.</returns>
        Task<IBaseResult> CreateBooking(BookingDto booking, CancellationToken cancellationToken = default);

        /// <summary>
        /// Cancels an existing booking.
        /// </summary>
        /// <param name="cancellation">The <see cref="CancelBookingDto"/> containing the data required to cancel the booking.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>An <see cref="IBaseResult"/> indicating the success or failure of the cancellation.</returns>
        Task<IBaseResult> CancelBooking(CancelBookingDto cancellation, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a specific order by its order number.
        /// </summary>
        /// <param name="orderNr">The order number to retrieve.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>An <see cref="IBaseResult{T}"/> containing the <see cref="OrderDto"/> for the specified order number.</returns>
        Task<IBaseResult<OrderDto>> OrderAsync(string orderNr, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new order.
        /// </summary>
        /// <param name="model">The <see cref="OrderDto"/> containing the data for the new order.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>An <see cref="IBaseResult"/> indicating the success or failure of the order creation.</returns>
        Task<IBaseResult> CreateOrder(OrderDto model, CancellationToken cancellationToken = default);
    }
}