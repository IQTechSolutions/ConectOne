using Microsoft.AspNetCore.Mvc;
using ShoppingModule.Domain.DataTransferObjects;
using ShoppingModule.Domain.Interfaces;
using ShoppingModule.Domain.RequestFeatures;

namespace ShoppingModule.Infrastructure.Controllers
{
    /// <summary>
    /// Provides endpoints for managing sales orders and their associated payments.
    /// </summary>
    /// <remarks>This controller handles operations related to sales orders, including retrieving paginated
    /// sales orders,  fetching individual sales orders, creating new sales orders, updating their statuses, and
    /// managing payments  associated with sales orders. It interacts with the <see cref="ISalesOrderService"/> to
    /// perform these operations.  The controller supports the following operations: - Retrieving paginated sales
    /// orders. - Fetching a specific sales order by its identifier. - Adding a new sales order. - Creating payments for
    /// sales orders. - Changing the status of sales orders and payments. - Handling payment notifications. - Retrieving
    /// payments for a specific sales order or by PayGate request ID.  All endpoints return appropriate HTTP status
    /// codes and responses based on the operation's outcome.</remarks>
    /// <param name="salesOrderService"></param>
    [Route("api/salesorders"), ApiController]
    public class SalesOrderController(ISalesOrderService salesOrderService) : ControllerBase
    {
        /// <summary>
        /// Retrieves a paginated list of sales orders based on the specified parameters.
        /// </summary>
        /// <remarks>The "X-Pagination" header in the response contains metadata about the pagination,
        /// such as the total number of items and pages.</remarks>
        /// <param name="parameters">The pagination and filtering parameters for retrieving sales orders.</param>
        /// <returns>An <see cref="IActionResult"/> containing the paginated list of sales orders. The response includes
        /// pagination metadata in the "X-Pagination" header.</returns>
        [HttpGet] public async Task<IActionResult> GetPagedSalesOrders([FromQuery] SalesOrderPageParameters parameters)
        {
            var result = await salesOrderService.PagedSalesOrdersAsync(parameters);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a sales order by its unique identifier.
        /// </summary>
        /// <remarks>This method returns an HTTP 200 OK response with the sales order details if the
        /// specified identifier exists. If the identifier is invalid or the sales order is not found, an appropriate
        /// HTTP error response is returned.</remarks>
        /// <param name="id">The unique identifier of the sales order to retrieve. Cannot be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> containing the sales order details if found, or an appropriate HTTP response
        /// if not.</returns>
        [HttpGet("{id}", Name = "GetSalesOrder")] public async Task<IActionResult> GetSalesOrder(string id)
        {
            var customer = await salesOrderService.SalesOrderAsync(id);
            return Ok(customer);
        }

        /// <summary>
        /// Creates a new sales order and returns the result.
        /// </summary>
        /// <remarks>The method processes the provided sales order data and delegates the creation to the
        /// sales order service.  Ensure that the <paramref name="newOrder"/> parameter contains valid data to avoid
        /// validation errors.</remarks>
        /// <param name="newOrder">The details of the sales order to create. This must include all required fields for a valid sales order.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation. Typically, this will include the
        /// created sales order details.</returns>
        [HttpPut] public async Task<IActionResult> AddSalesOrder([FromBody] SalesOrderDto newOrder)
        {
            var result = await salesOrderService.CreateSalesOrderAsync(newOrder);
            return Ok(result);
        }

        /// <summary>
        /// Creates a payment for a sales order.
        /// </summary>
        /// <remarks>This method processes the payment details provided in the request body and delegates
        /// the  creation of the payment to the underlying sales order service. The result is returned as  an HTTP
        /// response.</remarks>
        /// <param name="payment">The payment details provided as a <see cref="PaymenttDto"/> object.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the payment creation operation. Typically, this will
        /// include the created payment details or a status indicating the outcome.</returns>
        [HttpPut("createPayment")] public async Task<IActionResult> CreateSalesOrderPayment([FromBody] PaymenttDto payment)
        {
            var result = await salesOrderService.CreatePaymentAsync(payment);
            return Ok(result);
        }

        /// <summary>
        /// Updates the status of a sales order.
        /// </summary>
        /// <remarks>This method is an HTTP PUT endpoint that processes a request to change the status of
        /// a sales order. The provided <paramref name="orderdetails"/> must include valid sales order information and a
        /// new status.</remarks>
        /// <param name="orderdetails">An object containing the details of the sales order and the new status to be applied.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation. Typically, this will include the
        /// updated sales order status or an appropriate HTTP response.</returns>
        [HttpPost("changeStatus")] public async Task<IActionResult> ChangeOrderStatus([FromBody] UpdateSalesOrderStatusDto orderdetails)
        {
            var result = await salesOrderService.ChangeOrderStatusAsync(orderdetails);
            return Ok(result);
        }

        /// <summary>
        /// Updates the payment status of a sales order.
        /// </summary>
        /// <remarks>This method is an HTTP PUT endpoint that processes a request to change the payment
        /// status  of a sales order. The provided <paramref name="dto"/> must contain valid data for the operation  to
        /// succeed.</remarks>
        /// <param name="dto">An object containing the details required to update the payment status.  This must include the sales order
        /// identifier and the new payment status.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.  Returns <see cref="OkResult"/> if
        /// the payment status was successfully updated.</returns>
        [HttpPost("payments/changeStatus")] public async Task<IActionResult> ChangePaymentStatus([FromBody] UpdatePaymentDto dto)
        {
            await salesOrderService.ChangePaymentStatusAsync(dto);
            return Ok();
        }

        /// <summary>
        /// Handles payment notifications from the payment gateway.
        /// </summary>
        /// <remarks>This endpoint is intended to be called by the payment gateway to notify the system of
        /// payment events.  Ensure that the payload in <paramref name="dto"/> matches the expected structure and
        /// contains valid data.</remarks>
        /// <param name="dto">The payment notification data received from the payment gateway.  This must include all required fields for
        /// processing the notification.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.  Typically returns an HTTP 200 OK
        /// response if the notification is processed successfully.</returns>
        [HttpPut("payments/notification")] public async Task<IActionResult> PaymentNotification([FromBody] PayGateNotificationDto dto)
        {
            await salesOrderService.PaymentNotificationAsync(dto);
            return Ok();
        }

        /// <summary>
        /// Retrieves all payment details associated with the specified sales order.
        /// </summary>
        /// <remarks>This method is an HTTP GET endpoint and is accessible via the route
        /// "payments/{salesOrderId}". Ensure that the <paramref name="salesOrderId"/> is a valid and existing sales
        /// order identifier.</remarks>
        /// <param name="salesOrderId">The unique identifier of the sales order for which payment details are to be retrieved.</param>
        /// <returns>An <see cref="IActionResult"/> containing the payment details for the specified sales order. Returns an HTTP
        /// 200 OK response with the payment details if successful.</returns>
        [HttpGet("payments/{salesOrderId}", Name = "GetSalesOrderPayments")]
        public async Task<IActionResult> GetSalesOrderPayments(string salesOrderId)
        {
            var customer = await salesOrderService.AllSalesOrderPayments(salesOrderId);
            return Ok(customer);
        }

        /// <summary>
        /// Retrieves the sales order payment details associated with the specified PayGate request ID.
        /// </summary>
        /// <remarks>This method returns an HTTP 200 OK response with the payment details if the PayGate
        /// request ID is valid and a matching payment exists. If no payment is found, an appropriate HTTP error
        /// response is returned.</remarks>
        /// <param name="paygateReqestId">The unique identifier of the PayGate request. This value is used to locate the corresponding sales order
        /// payment.</param>
        /// <returns>An <see cref="IActionResult"/> containing the payment details if found, or an appropriate HTTP response if
        /// not.</returns>
        [HttpGet("payments/paygate/{paygateReqestId}", Name = "GetSalesOrderPaymentViaPaygateRequestId")]
        public async Task<IActionResult> GetSalesOrderPaymentViaPaygateRequestId(string paygateReqestId)
        {
            var payment = await salesOrderService.SalesOrderPaymentViaPayGateRequestId(paygateReqestId);
            return Ok(payment);
        }
    }
}
