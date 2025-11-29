using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using Microsoft.EntityFrameworkCore;
using ShoppingModule.Domain.DataTransferObjects;
using ShoppingModule.Domain.Entities;
using ShoppingModule.Domain.Interfaces;
using ShoppingModule.Domain.RequestFeatures;

namespace ShoppingModule.Infrastructure.Implementation;

/// <summary>
/// Provides operations for managing sales orders and their associated payments.
/// </summary>
/// <remarks>This service offers functionality to retrieve, create, and update sales orders, as well as manage
/// payments associated with those orders. It supports paginated retrieval of sales orders, updating order statuses, 
/// creating payments, and handling payment notifications. The service relies on repository patterns for data  access
/// and ensures proper handling of related entities such as sales order details, addresses, and payments.</remarks>
/// <param name="repository"></param>
/// <param name="paymentRepo"></param>
/// <param name="salesOrderPaymentRepo"></param>
public class SalesOrderService(IRepository<SalesOrder, string> repository, IRepository<Payment, string> paymentRepo, IRepository<SalesOrderPayment, string> salesOrderPaymentRepo) : ISalesOrderService
{
    /// <summary>
    /// Retrieves a paginated list of sales orders based on the specified parameters.
    /// </summary>
    /// <remarks>This method uses the provided <paramref name="parameters"/> to determine the subset of sales
    /// orders to retrieve. The returned result includes metadata such as the total count of items, the current page
    /// number, and the page size.</remarks>
    /// <param name="parameters">The pagination and filtering parameters used to define the page number, page size, and any additional criteria
    /// for retrieving sales orders.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A <see cref="PaginatedResult{T}"/> containing a collection of <see cref="SalesOrderDto"/> objects representing
    /// the sales orders in the requested page. If the operation fails, the result will indicate failure with the
    /// corresponding error messages.</returns>
    public async Task<PaginatedResult<SalesOrderDto>> PagedSalesOrdersAsync(SalesOrderPageParameters parameters, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<SalesOrder>(c => c.OrderStatus == parameters.OrderStatus);
        spec.AddInclude(c => c.Include(g => g.Details).ThenInclude(r => r.MetaData));
        spec.AddInclude(c => c.Include(g => g.Details).ThenInclude(r => r.Product));
        spec.AddInclude(c => c.Include(g => g.UserInfo));

        var salesOrderResult = await repository.ListAsync(spec, false, cancellationToken);
        if (!salesOrderResult.Succeeded)
            return PaginatedResult<SalesOrderDto>.Failure(salesOrderResult.Messages);

        var page = salesOrderResult.Data.Select(c => new SalesOrderDto(c)).ToList();
        return PaginatedResult<SalesOrderDto>.Success(page, salesOrderResult.Data.Count, parameters.PageNr, parameters.PageSize);
    }

    /// <summary>
    /// Retrieves a sales order by its unique identifier, including its details, metadata, product information, and
    /// addresses.
    /// </summary>
    /// <remarks>The method performs a query to retrieve the sales order and its related data, including
    /// details, metadata, product information, and addresses. If the sales order is not found, the result will indicate
    /// failure with an appropriate message.</remarks>
    /// <param name="salesOrderId">The unique identifier of the sales order to retrieve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// where <c>T</c> is <see cref="SalesOrderDto"/>. If the operation succeeds, the result contains the sales order
    /// data; otherwise, it contains error messages.</returns>
    public async Task<IBaseResult<SalesOrderDto>> SalesOrderAsync(string salesOrderId, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<SalesOrder>(c => c.Id == salesOrderId);
        spec.AddInclude(c => c.Include(g => g.Details).ThenInclude(r => r.MetaData));
        spec.AddInclude(c => c.Include(g => g.Details).ThenInclude(r => r.Product));
        spec.AddInclude(c => c.Include(g => g.Addresses));


        var result = await repository.FirstOrDefaultAsync(spec, false, cancellationToken);
        if (!result.Succeeded) return await Result<SalesOrderDto>.FailAsync(result.Messages);
        if (result.Data == null) return await Result<SalesOrderDto>.FailAsync("Sales order not found");
        return await Result<SalesOrderDto>.SuccessAsync(new SalesOrderDto(result.Data));
    }

    /// <summary>
    /// Creates a new sales order asynchronously.
    /// </summary>
    /// <remarks>This method performs the following steps: <list type="number"> <item> Attempts to create a
    /// new sales order in the repository using the provided <paramref name="newOrder"/>. </item> <item> Saves the
    /// changes to the repository if the creation succeeds. </item> <item> Returns the created sales order if all
    /// operations succeed, or an error result if any step fails. </item> </list></remarks>
    /// <param name="newOrder">The details of the sales order to create.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> of
    /// type <see cref="SalesOrderDto"/>. If the operation succeeds, the result contains the created sales order. If the
    /// operation fails, the result contains error messages.</returns>
    public async Task<IBaseResult<SalesOrderDto>> CreateSalesOrderAsync(SalesOrderDto newOrder, CancellationToken cancellationToken = default)
    {
        var result = await repository.CreateAsync(new SalesOrder(newOrder), cancellationToken);
        if(!result.Succeeded) return await Result<SalesOrderDto>.FailAsync(result.Messages);
        
        var saveResult = await repository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded) return await Result<SalesOrderDto>.FailAsync(saveResult.Messages);
        
        return await SalesOrderAsync(result.Data.Id, cancellationToken);
    }

    /// <summary>
    /// Updates the status of a sales order and returns the updated sales order details.
    /// </summary>
    /// <remarks>This method retrieves the sales order by its ID, updates its status, and saves the changes to
    /// the repository.  If the sales order is not found or the update operation fails, the method returns a failure
    /// result with  appropriate error messages.</remarks>
    /// <param name="update">An object containing the sales order ID and the new status to apply.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an  IBaseResult{T} object with the
    /// updated sales order details if the operation succeeds,  or error messages if the operation fails.</returns>
    public async Task<IBaseResult<SalesOrderDto>> ChangeOrderStatusAsync(UpdateSalesOrderStatusDto update, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<SalesOrder>(c => c.Id == update.SalesOrderId);
        spec.AddInclude(c => c.Include(g => g.Details).ThenInclude(r => r.MetaData));
        spec.AddInclude(c => c.Include(g => g.Details).ThenInclude(r => r.Product));
        spec.AddInclude(c => c.Include(g => g.Addresses));


        var result = await repository.FirstOrDefaultAsync(spec, false, cancellationToken);
        if (!result.Succeeded) return await Result<SalesOrderDto>.FailAsync(result.Messages);
        if (result.Data == null) return await Result<SalesOrderDto>.FailAsync("Sales order not found");

        result.Data.OrderStatus = update.SalesOrderStatus;
        var updateResult = repository.Update(result.Data);
        if (!updateResult.Succeeded) return await Result<SalesOrderDto>.FailAsync(updateResult.Messages);
        var saveResult = await repository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded) return await Result<SalesOrderDto>.FailAsync(saveResult.Messages);

        return await SalesOrderAsync(result.Data.Id, cancellationToken);
    }

    /// <summary>
    /// Creates a new payment and associates it with a sales order.
    /// </summary>
    /// <remarks>This method performs the following operations: <list type="bullet"> <item> Creates a payment
    /// record in the repository. </item> <item> Associates the payment with a sales order by creating a corresponding
    /// sales order payment record. </item> <item> Saves the changes to the repository. </item> </list> If any of these
    /// operations fail, the method returns a failure result with the corresponding error messages.</remarks>
    /// <param name="payment">The payment details to be created, including the sales order ID and payment amount.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// object with the created payment details as a <see cref="PaymenttDto"/> if the operation succeeds; otherwise, it
    /// contains error messages describing the failure.</returns>
    public async Task<IBaseResult<PaymenttDto>> CreatePaymentAsync(PaymenttDto payment, CancellationToken cancellationToken = default)
    {
        var result = await paymentRepo.CreateAsync(new Payment(payment), cancellationToken);
        if (!result.Succeeded) return await Result<PaymenttDto>.FailAsync(result.Messages);

        var salesOrderPayment = new SalesOrderPayment() { 
            PaymentId = result.Data.Id, 
            SalesOrderId = payment.SalesOrderId, 
            AmmountAllocated = payment.Ammount 
        };

        var sopResult = await salesOrderPaymentRepo.CreateAsync(salesOrderPayment, cancellationToken);
        if (!sopResult.Succeeded) return await Result<PaymenttDto>.FailAsync(sopResult.Messages);

        var saveResult = await paymentRepo.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded) return await Result<PaymenttDto>.FailAsync(saveResult.Messages);

        return await Result<PaymenttDto>.SuccessAsync(new PaymenttDto(payment.SalesOrderId, result.Data.PaymentDate, result.Data.PaymentReference, result.Data.Amount, result.Data.Notes, result.Data.PaymentMethod, result.Data.PaymentStatus, result.Data.ReceiptNr));
    }

    /// <summary>
    /// Retrieves all payments associated with a specific sales order.
    /// </summary>
    /// <remarks>This method queries the repository for payments linked to the specified sales order ID and
    /// maps the results to a collection of <see cref="PaymenttDto"/> objects. If no payments are found or the operation
    /// fails, the result will indicate failure with appropriate error messages.</remarks>
    /// <param name="salesOrderId">The unique identifier of the sales order for which payments are to be retrieved.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// where <c>T</c> is an <see cref="IEnumerable{T}"/> of <see cref="PaymenttDto"/> objects representing the payments
    /// for the specified sales order. If the operation fails, the result contains error messages.</returns>
    public async Task<IBaseResult<IEnumerable<PaymenttDto>>> AllSalesOrderPayments(string salesOrderId, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<SalesOrderPayment>(c => c.SalesOrderId == salesOrderId);
        spec.AddInclude(c => c.Include(g => g.Payment));

        var result = await salesOrderPaymentRepo.ListAsync(spec, false, cancellationToken);
        if (!result.Succeeded) return await Result<IEnumerable<PaymenttDto>>.FailAsync(result.Messages);

        var payments = result.Data.Select(c => new PaymenttDto(c.SalesOrderId, c.Payment.PaymentDate, c.Payment.PaymentReference, c.Payment.Amount, c.Payment.Notes, c.Payment.PaymentMethod, c.Payment.PaymentStatus, c.Payment.ReceiptNr));
        return await Result<IEnumerable<PaymenttDto>>.SuccessAsync(payments);
    }

    /// <summary>
    /// Retrieves payment details associated with a sales order using the specified PayGate request ID.
    /// </summary>
    /// <remarks>This method searches for a payment record based on the provided PayGate request ID. If a
    /// matching payment is found, the associated sales order details and payment information are returned. If no
    /// payment is found, the result will indicate failure with an appropriate error message.</remarks>
    /// <param name="paygateReqestId">The unique identifier for the PayGate request used to locate the payment.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> of
    /// type <see cref="PaymenttDto"/> with the payment details if the operation succeeds; otherwise, an error result.</returns>
    public async Task<IBaseResult<PaymenttDto>> SalesOrderPaymentViaPayGateRequestId(string paygateReqestId, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<Payment>(c => c.PaymentReference == paygateReqestId);

        var result = await paymentRepo.FirstOrDefaultAsync(spec, false, cancellationToken);
        if (!result.Succeeded) return await Result<PaymenttDto>.FailAsync(result.Messages);
        if (result.Data == null) return await Result<PaymenttDto>.FailAsync("Payment not found");

        return await Result<PaymenttDto>.SuccessAsync(new PaymenttDto(result.Data.SalesOrders.FirstOrDefault()?.SalesOrderId, result.Data.PaymentDate, result.Data.PaymentReference, result.Data.Amount, result.Data.Notes, result.Data.PaymentMethod, result.Data.PaymentStatus, result.Data.ReceiptNr));
    }

    /// <summary>
    /// Updates the payment status of a payment record based on the provided payment details.
    /// </summary>
    /// <remarks>This method retrieves a payment record using the payment reference provided in <paramref
    /// name="dto"/>. If the payment record is found, its status is updated to the new status specified in <paramref
    /// name="dto"/>. The changes are then saved to the repository. If the payment record is not found or the save
    /// operation fails, the method returns a failure result with the appropriate error messages.</remarks>
    /// <param name="dto">An object containing the payment details, including the payment reference and the new payment status.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the operation.</returns>
    public async Task<IBaseResult> ChangePaymentStatusAsync(UpdatePaymentDto dto, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<Payment>(c => c.PaymentReference == dto.PayRequestId);

        var result = await paymentRepo.FirstOrDefaultAsync(spec, false, cancellationToken);
        if (!result.Succeeded) return await Result.FailAsync(result.Messages);
        if (result.Data == null) return await Result.FailAsync("Payment not found");

        result.Data.PaymentStatus = dto.PaymentStatus;

        paymentRepo.Update(result.Data);

        var saveResult = await paymentRepo.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);
        return await Result.SuccessAsync();
    }

    /// <summary>
    /// Processes a payment notification received from the payment gateway and updates the payment record accordingly.
    /// </summary>
    /// <remarks>This method updates the payment record in the repository based on the details provided in the
    /// notification. If the payment record is not found or the update operation fails, the method returns a failure
    /// result.</remarks>
    /// <param name="dto">The payment notification data transfer object containing details of the payment transaction.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the operation.</returns>
    public async Task<IBaseResult> PaymentNotificationAsync(PayGateNotificationDto dto, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<Payment>(c => c.PaymentReference == dto.PAY_REQUEST_ID);

        var result = await paymentRepo.FirstOrDefaultAsync(spec, false, cancellationToken);
        if (!result.Succeeded) return await Result.FailAsync(result.Messages);
        if (result.Data == null) return await Result.FailAsync("Payment not found");

        result.Data.PaymentStatus = dto.TRANSACTION_STATUS;
        result.Data.PaymentResult = dto.RESULT_CODE;
        result.Data.PaymentResultDescription = dto.RESULT_DESC;
        result.Data.AuthCode = dto.AUTH_CODE;
        result.Data.TransactionId = dto.TRANSACTION_ID;

        paymentRepo.Update(result.Data);
        var saveResult = await paymentRepo.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);
        return await Result.SuccessAsync();
    }
}