using AccomodationModule.Domain.Arguments;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Enums;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using Microsoft.EntityFrameworkCore;

namespace AccomodationModule.Infrastructure.Implimentation;

/// <summary>
/// Represents the service layer responsible for handling booking-related operations,
/// including retrieving bookings, creating or updating bookings, managing orders, and applying filters.
/// This service interacts with an accommodation repository to perform database operations.
/// </summary>
public class BookingService(IAccomodationRepositoryManager accommodationRepo) : IBookingService
{
    /// <summary>
    /// Asynchronously retrieves the count of bookings that match the specified filtering criteria.
    /// </summary>
    /// <remarks>This method retrieves all bookings from the repository and filters them based on the criteria
    /// specified in <paramref name="pageParameters"/>. If no filtering criteria are provided, all bookings are included
    /// in the count.</remarks>
    /// <param name="pageParameters">The parameters used to filter bookings, including optional booking status.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
    /// <returns>An <see cref="IBaseResult"/> containing the count of bookings that match the specified criteria. If the
    /// operation fails, the result will include error messages.</returns>
    public async Task<IBaseResult<int>> BookingsCountAsync(BookingParameters pageParameters, CancellationToken cancellationToken = default)
    {
        var spec = pageParameters.BookingStatus is not null
            ? new LambdaSpec<Booking>(c => c.BookingStatus == pageParameters.BookingStatus)
            : new LambdaSpec<Booking>(c => true);

        // Attempt to retrieve all bookings
        var result = await accommodationRepo.Bookings.ListAsync(trackChanges: false, cancellationToken);
        if (!result.Succeeded) return await Result<int>.FailAsync(result.Messages);

        // Return the count of filtered bookings
        return await Result<int>.SuccessAsync(result.Data.Count);
    }

    /// <summary>
    /// Retrieves a paginated list of bookings based on the specified parameters.
    /// This allows clients to request a subset (page) of bookings, potentially filtered by search text, user ID, or booking status.
    /// </summary>
    /// <param name="pageParameters">
    /// The pagination and filtering parameters, including page number, page size, search text, user ID, and optional booking status.
    /// </param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
    /// <returns>
    /// A <see cref="PaginatedResult{BookingDto}"/> containing a list of matching bookings 
    /// for the requested page and pagination information, or failure messages if retrieval fails.
    /// </returns>
    public async Task<PaginatedResult<BookingDto>> PagedBookingsAsync(BookingParameters pageParameters, CancellationToken cancellationToken = default)
    {
        var spec =  new LambdaSpec<Booking>(c => true);
        spec.AddInclude(g => g.Include(c => c.Lodging));
        spec.AddInclude(g => g.Include(c => c.User));
        spec.AddInclude(g => g.Include(c => c.Room).ThenInclude(c => c.Package));

        // Retrieve all bookings from the repository
        var result = await accommodationRepo.Bookings.ListAsync(spec, trackChanges: false, cancellationToken);
        if (result.Succeeded)
        {
            // Include related entities like Lodging, Room, Package, User for richer booking details
            var response = result.Data;

            // Optional filtering by search text (e.g., guest name)
            if (!string.IsNullOrEmpty(pageParameters.SearchText))
            {
                response = response.Where(c => c.Name.Contains(pageParameters.SearchText, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            // Optional filtering by user ID
            if (!string.IsNullOrEmpty(pageParameters.UserId))
            {
                response = response.Where(c => c.UserId == pageParameters.UserId).ToList();
            }

            // Optional filtering by booking status
            if (pageParameters.BookingStatus is not null)
            {
                response = response.Where(c => c.BookingStatus == pageParameters.BookingStatus).ToList();
            }

            // Convert bookings to DTOs and return a paginated result
            var bookingsDto = response.Select(booking => new BookingDto(booking)).ToList();
            return PaginatedResult<BookingDto>.Success(bookingsDto, response.Count(), pageParameters.PageNr, pageParameters.PageSize);
        }

        // If retrieval failed, return a failure with messages
        return PaginatedResult<BookingDto>.Failure(result.Messages);
    }

    /// <summary>
    /// Retrieves a single booking by its unique booking ID.
    /// </summary>
    /// <param name="bookingId">The unique identifier of the booking to retrieve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
    /// <returns>
    /// An <see cref="IBaseResult{BookingDto}"/> containing the booking details if found, 
    /// or a failure result if no matching booking is found or if retrieval fails.
    /// </returns>
    public async Task<IBaseResult<BookingDto>> BookingAsync(string bookingId, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<Booking>(c => c.Id.ToString() == bookingId);

        // Find booking by bookingId
        var result = await accommodationRepo.Bookings.FirstOrDefaultAsync(spec, false, cancellationToken);
        if (!result.Succeeded) return await Result<BookingDto>.FailAsync(result.Messages);

        if (result.Data == null)
            return await Result<BookingDto>.FailAsync($"No booking with id '{bookingId}' found in the database");

        // Convert to DTO and return success
        return await Result<BookingDto>.SuccessAsync(new BookingDto(result.Data));
    }

    /// <summary>
    /// Retrieves all bookings associated with a particular order number.
    /// An order can contain multiple bookings, so this returns a collection.
    /// </summary>
    /// <param name="orderNr">The order number for which to retrieve bookings.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
    /// <returns>
    /// An <see cref="IBaseResult{IEnumerable{BookingDto}}"/> containing all associated bookings, 
    /// or a failure result if retrieval fails.
    /// </returns>
    public async Task<IBaseResult<IEnumerable<BookingDto>>> BookingsOrder(string orderNr, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<Booking>(c => c.OrderNr == orderNr);

        var result = await accommodationRepo.Bookings.ListAsync(spec, false, cancellationToken);
        if (!result.Succeeded) return await Result<IEnumerable<BookingDto>>.FailAsync(result.Messages);

        var bookings = result.Data.Select(c => new BookingDto(c));
        return await Result<IEnumerable<BookingDto>>.SuccessAsync(bookings);
    }

    /// <summary>
    /// Marks all bookings associated with a given order number as completed (e.g., changing their status to Active).
    /// This might be used after the order is fully paid or confirmed.
    /// </summary>
    /// <param name="orderNr">The order number identifying which bookings to complete.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
    /// <returns>An <see cref="IBaseResult"/> indicating success or failure.</returns>
    public async Task<IBaseResult> CompleteBookingsOrder(string orderNr, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<Booking>(c => c.OrderNr == orderNr);

        var result = await accommodationRepo.Bookings.ListAsync(spec, false, cancellationToken);
        if (!result.Succeeded) return await Result.FailAsync(result.Messages);

        // Update booking statuses
        foreach (var booking in result.Data.ToList())
        {
            booking.BookingStatus = BookingStatus.Active;
            accommodationRepo.Bookings.Update(booking);
        }

        // Save changes to the database
        var saveResult = await accommodationRepo.Bookings.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

        return await Result.SuccessAsync("Booking order successfully completed");
    }

    /// <summary>
    /// Creates a new booking record based on the provided <see cref="BookingDto"/>.
    /// Typically invoked after validating availability and confirming booking details.
    /// </summary>
    /// <param name="model">The <see cref="BookingDto"/> containing the necessary booking details.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
    /// <returns>
    /// An <see cref="IBaseResult"/> indicating success if the booking was created successfully, or failure if the operation fails.
    /// </returns>
    public async Task<IBaseResult> CreateBooking(BookingDto model, CancellationToken cancellationToken = default)
    {
        // Convert DTO to domain entity and create it in the database
        var booking = model.ToBooking();
        await accommodationRepo.Bookings.CreateAsync(booking, cancellationToken);

        // Attempt to save changes
        var saveResult = await accommodationRepo.Bookings.SaveAsync(cancellationToken);
        if (saveResult.Succeeded)
        {
            return await Result.SuccessAsync($"{booking.Name} successfully updated");
        }
        return await Result.FailAsync(saveResult.Messages);
    }

    /// <summary>
    /// Cancels an existing booking based on the provided cancellation details.
    /// </summary>
    /// <param name="cancellation">The <see cref="CancelBookingDto"/> containing the booking reference and optional cancellation reasons.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
    /// <returns>
    /// An <see cref="IBaseResult"/> indicating success if the booking was canceled successfully, or failure otherwise.
    /// </returns>
    public async Task<IBaseResult> CancelBooking(CancelBookingDto cancellation, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<Booking>(c => c.BookingReferenceNr == cancellation.BookingId.ToString());

        // Find the booking by its booking reference number
        var result = await accommodationRepo.Bookings.FirstOrDefaultAsync(spec, true, cancellationToken);
        if (!result.Succeeded) return await Result.FailAsync(result.Messages);

        if (result.Data == null)
            return await Result.FailAsync($"No booking found with reference '{cancellation.BookingId}'");

        // Apply the cancellation details and update the booking
        result.Data.CancellationId = cancellation.CancellationId;
        accommodationRepo.Bookings.Update(result.Data);

        // Attempt to save changes
        var saveResult = await accommodationRepo.Bookings.SaveAsync(cancellationToken);
        if (saveResult.Succeeded)
        {
            return await Result.SuccessAsync($"{result.Data.Name} successfully cancelled");
        }
        return await Result.FailAsync(saveResult.Messages);
    }

    #region Orders

    /// <summary>
    /// Retrieves details of a particular order, including its associated bookings.
    /// This provides a broader view of what the user purchased or booked as part of a single order.
    /// </summary>
    /// <param name="orderNr">The unique order number to retrieve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
    /// <returns>
    /// An <see cref="IBaseResult{OrderDto}"/> containing order details and associated bookings if found,
    /// or failure messages if not found or if retrieval fails.
    /// </returns>
    public async Task<IBaseResult<OrderDto>> OrderAsync(string orderNr, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<Order>(c => c.Id == orderNr);
        spec.AddInclude(c => c.Include(o => o.Bookings).ThenInclude(b => b.Lodging));

        var result = await accommodationRepo.Orders.FirstOrDefaultAsync(spec, false, cancellationToken);
        if (!result.Succeeded) return await Result<OrderDto>.FailAsync(result.Messages);

        if (result.Data == null)
            return await Result<OrderDto>.FailAsync($"No order with number '{orderNr}' found.");

        return await Result<OrderDto>.SuccessAsync(new OrderDto(result.Data));
    }

    /// <summary>
    /// Creates a new order in the system based on the provided <see cref="OrderDto"/>.
    /// Typically called after finalizing the selection of bookings or vouchers and before completing the transaction.
    /// </summary>
    /// <param name="model">The <see cref="OrderDto"/> containing details about the order, including associated bookings.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
    /// <returns>
    /// An <see cref="IBaseResult"/> indicating success if the order was created successfully,
    /// or failure if the operation fails.
    /// </returns>
    public async Task<IBaseResult> CreateOrder(OrderDto model, CancellationToken cancellationToken = default)
    {
        try
        {
       
            // Convert DTO to domain entity and create it
            var order = model.ToOrder();
            await accommodationRepo.Orders.CreateAsync(order, cancellationToken);

            // Attempt to save the new order
            var saveResult = await accommodationRepo.Bookings.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);
        
            // Live bookings: send manual confirmation emails and show a thank you page.
            //if (order.Bookings.Any())
            //{
            //    var manualBookingConformation = await _emailSender.SendManualBookingConformationEmailAsync(
            //        order,
            //        $"{model.FirstName} {model.LastName}",
            //        model.Email,
            //        "Kwagga Admin",
            //        "noreply@kwaggatravel.co.za",
            //        $"https://kwaggatravel.co.za/bookings/order/{order.Id}"
            //    );

            //    var manualBookingConformationAdmin = await _emailSender.SendManualBookingConformationAdminEmailAsync(
            //        order,
            //        "Kwagga Admin",
            //        "ivanrossouw2@gmail.com",
            //        "Kwagga Admin",
            //        "noreply@kwaggatravel.co.za",
            //        $"https://kwaggatravel.co.za/bookings/order/{order.Id}"
            //    );
            //}

            //if (order.Vouchers.Any())
            //{
            //    var voucherConformationMailResult = await _emailSender.SendVoucherConformationEmailAsync(
            //        order,
            //        $"{model.FirstName} {model.LastName}",
            //        model.Email,
            //        model.PhoneNr,
            //        "Kwagga Admin",
            //        "noreply@kwaggatravel.co.za",
            //        $"https://kwaggatravel.co.za/bookings/order/{order.Id}"
            //    );
            //}


            return await Result.SuccessAsync("Order created successfully updated");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    #endregion
}