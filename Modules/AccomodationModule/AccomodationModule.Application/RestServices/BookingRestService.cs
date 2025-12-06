using AccomodationModule.Domain.Arguments;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.Extensions;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Application.RestServices
{
    /// <summary>
    /// Provides a REST-based implementation of the <see cref="IBookingService"/> interface for managing bookings and
    /// orders.
    /// </summary>
    /// <remarks>This service communicates with an underlying HTTP provider to perform operations such as
    /// retrieving, creating, updating, and canceling bookings and orders. It supports asynchronous operations and
    /// allows for cancellation through <see cref="CancellationToken"/>.  Ensure that the provided <see
    /// cref="IBaseHttpProvider"/> is properly configured before using this service.</remarks>
    /// <param name="provider"></param>
    public class BookingRestService(IBaseHttpProvider provider) : IBookingService
    {
        /// <summary>
        /// Asynchronously retrieves the total count of bookings based on the specified parameters.
        /// </summary>
        /// <remarks>The method constructs a query string from the provided <paramref
        /// name="pageParameters"/> and sends a request to retrieve the count of bookings. Ensure that the <paramref
        /// name="pageParameters"/> object is properly initialized before calling this method.</remarks>
        /// <param name="pageParameters">The parameters used to filter and refine the booking count query.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// with the total count of bookings matching the specified parameters.</returns>
        public async Task<IBaseResult<int>> BookingsCountAsync(BookingParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<int>($"bookings/count?{pageParameters.GetQueryString()}");
            return result;
        }

        /// <summary>
        /// Retrieves a paginated list of bookings based on the specified parameters.
        /// </summary>
        /// <remarks>This method queries the bookings endpoint and returns the results in a paginated
        /// format. Use the <paramref name="pageParameters"/> to specify the page size, page number, and any additional
        /// filtering criteria.</remarks>
        /// <param name="pageParameters">The parameters that define the pagination and filtering options for the bookings.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a collection of <see cref="BookingDto"/> objects and
        /// pagination metadata.</returns>
        public async Task<PaginatedResult<BookingDto>> PagedBookingsAsync(BookingParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetPagedAsync<BookingDto, BookingParameters>($"bookings", pageParameters);
            return result;
        }

        /// <summary>
        /// Retrieves the details of a booking asynchronously based on the specified booking ID.
        /// </summary>
        /// <remarks>This method sends a request to retrieve booking details from the underlying provider.
        /// Ensure that the <paramref name="bookingId"/> corresponds to a valid booking in the system.</remarks>
        /// <param name="bookingId">The unique identifier of the booking to retrieve. This value cannot be null or empty.</param>
        /// <param name="cancellationToken">An optional token to monitor for cancellation requests. The default value is <see
        /// cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that wraps the <see cref="BookingDto"/> containing the booking details.</returns>
        public async Task<IBaseResult<BookingDto>> BookingAsync(string bookingId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<BookingDto>($"bookings/details/{bookingId}");
            return result;
        }

        /// <summary>
        /// Retrieves a list of bookings associated with the specified order number.
        /// </summary>
        /// <remarks>This method sends a request to the underlying provider to retrieve booking data.
        /// Ensure that the <paramref name="orderNr"/> is valid and corresponds to an existing order in the
        /// system.</remarks>
        /// <param name="orderNr">The order number used to identify the bookings. This value cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// of <see cref="IEnumerable{BookingDto}"/> representing the bookings associated with the specified order
        /// number. If no bookings are found, the result may contain an empty collection.</returns>
        public async Task<IBaseResult<IEnumerable<BookingDto>>> BookingsOrder(string orderNr, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<BookingDto>>($"bookings/bookingList/{orderNr}");
            return result;
        }

        /// <summary>
        /// Completes the specified bookings order.
        /// </summary>
        /// <remarks>This method sends a request to complete the specified bookings order. Ensure that the
        /// provided <paramref name="orderNr"/> corresponds to a valid and existing order.</remarks>
        /// <param name="orderNr">The unique identifier of the order to complete. This value cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> CompleteBookingsOrder(string orderNr, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"bookings/complete/{orderNr}");
            return result;
        }

        /// <summary>
        /// Creates a new booking using the provided booking details.
        /// </summary>
        /// <remarks>This method sends the booking details to the underlying provider to create a new
        /// booking. Ensure that the  <paramref name="booking"/> object contains all required fields before calling this
        /// method.</remarks>
        /// <param name="booking">The booking details to be created. This parameter cannot be <see langword="null"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// representing the outcome of the booking creation.</returns>
        public async Task<IBaseResult> CreateBooking(BookingDto booking, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"bookings", booking);
            return result;
        }

        /// <summary>
        /// Cancels an existing booking based on the provided cancellation details.
        /// </summary>
        /// <remarks>This method sends a cancellation request to the booking provider. Ensure that the
        /// <paramref name="cancellation"/>  object contains all required information for the cancellation to
        /// succeed.</remarks>
        /// <param name="cancellation">An object containing the details of the booking to be canceled.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the cancellation request.</returns>
        public async Task<IBaseResult> CancelBooking(CancelBookingDto cancellation, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"bookings/cancel", cancellation);
            return result;
        }

        /// <summary>
        /// Retrieves the details of an order asynchronously based on the specified order number.
        /// </summary>
        /// <remarks>This method communicates with an external provider to retrieve the order details.
        /// Ensure that the <paramref name="orderNr"/> is valid and corresponds to an existing order. The operation may
        /// be canceled by passing a cancellation token.</remarks>
        /// <param name="orderNr">The unique identifier of the order to retrieve. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping the <see cref="OrderDto"/> for the specified order.</returns>
        public async Task<IBaseResult<OrderDto>> OrderAsync(string orderNr, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<OrderDto>($"bookings/order/{orderNr}");
            return result;
        }

        /// <summary>
        /// Creates a new order based on the provided order details.
        /// </summary>
        /// <remarks>This method sends the order details to the underlying provider to create the order.
        /// Ensure that the  <paramref name="model"/> parameter contains valid data before calling this
        /// method.</remarks>
        /// <param name="model">The order details to be used for creating the order.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the order creation process.</returns>
        public async Task<IBaseResult> CreateOrder(OrderDto model, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync($"bookings/order", model);
            return result;
        }
    }
}
