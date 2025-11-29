using ConectOne.Domain.ResultWrappers;
using ShoppingModule.Domain.DataTransferObjects;
using ShoppingModule.Domain.RequestFeatures;

namespace ShoppingModule.Domain.Interfaces
{
    /// <summary>
    /// Defines the contract for managing sales orders and their associated payments.
    /// </summary>
    /// <remarks>This interface provides methods for retrieving, creating, and updating sales orders, as well
    /// as managing payments associated with those orders. It supports operations such as paginated retrieval of sales
    /// orders, updating order statuses, creating payments, and handling payment notifications. Implementations of this
    /// interface are expected to enforce business rules and validations related to sales order and payment
    /// management.</remarks>
    public interface ISalesOrderService
    {
        /// <summary>
        /// Retrieves a paginated list of sales orders based on the specified parameters.
        /// </summary>
        /// <remarks>Use this method to retrieve sales orders in a paginated format, which is useful for
        /// scenarios  where large datasets need to be displayed incrementally. Ensure that the <paramref
        /// name="parameters"/>  object is properly configured to avoid invalid pagination requests.</remarks>
        /// <param name="parameters">The parameters used to define the pagination and filtering options for the sales orders. This includes page
        /// size, page number, and any additional filtering criteria.</param>
        /// <returns>A tuple containing the paginated list of sales orders and metadata about the pagination. The first item in
        /// the tuple is an <see cref="IEnumerable{T}"/> of <see cref="SalesOrderDto"/>  representing the sales orders
        /// for the requested page. The second item is a <see cref="PagedListMetaData"/>  object that provides details
        /// about the pagination, such as total items, total pages, and the current page.</returns>
        Task<PaginatedResult<SalesOrderDto>> PagedSalesOrdersAsync(SalesOrderPageParameters parameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the details of a sales order asynchronously.
        /// </summary>
        /// <param name="salesOrderId">The unique identifier of the sales order to retrieve. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a  <see cref="SalesOrderDto"/>
        /// object with the details of the specified sales order.</returns>
        Task<IBaseResult<SalesOrderDto>> SalesOrderAsync(string salesOrderId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new sales order based on the provided details.
        /// </summary>
        /// <param name="newOrder">The details of the sales order to create, including customer information and order items.</param>
        /// <returns>A <see cref="SalesOrderDto"/> representing the created sales order, including its unique identifier and
        /// other relevant details.</returns>
        Task<IBaseResult<SalesOrderDto>> CreateSalesOrderAsync(SalesOrderDto newOrder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the status of a sales order and returns the updated sales order details.
        /// </summary>
        /// <param name="update">An object containing the sales order ID and the new status to apply.</param>
        /// <returns>A <see cref="SalesOrderDto"/> representing the updated sales order.  The returned object includes the
        /// updated status and other relevant details.</returns>
        Task<IBaseResult<SalesOrderDto>> ChangeOrderStatusAsync(UpdateSalesOrderStatusDto update, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new payment asynchronously.
        /// </summary>
        /// <remarks>This method processes the provided payment details and returns the created payment
        /// object. Ensure that the <paramref name="payment"/> parameter contains valid data before calling this
        /// method.</remarks>
        /// <param name="payment">The payment details to be created. Must not be null.</param>
        /// <returns>A <see cref="PaymenttDto"/> representing the created payment.</returns>
        Task<IBaseResult<PaymenttDto>> CreatePaymentAsync(PaymenttDto payment, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all payment details associated with the specified sales order.
        /// </summary>
        /// <param name="salesOrderId">The unique identifier of the sales order for which payments are to be retrieved. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of <see
        /// cref="PaymenttDto"/> objects representing the payments for the specified sales order.</returns>
        Task<IBaseResult<IEnumerable<PaymenttDto>>> AllSalesOrderPayments(string salesOrderId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves payment details for a sales order using the specified PayGate request ID.
        /// </summary>
        /// <remarks>This method is used to retrieve payment information for a sales order based on the 
        /// PayGate request ID. Ensure that the provided ID is valid and corresponds to an existing  payment
        /// record.</remarks>
        /// <param name="paygateReqestId">The unique identifier of the PayGate request. This value cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a  <see cref="PaymenttDto"/>
        /// object with the payment details associated with the specified  PayGate request ID.</returns>
        Task<IBaseResult<PaymenttDto>> SalesOrderPaymentViaPayGateRequestId(string paygateReqestId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the payment status based on the provided payment details.
        /// </summary>
        /// <param name="dto">An object containing the payment details and the new status to be applied.  The object must not be null.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<IBaseResult> ChangePaymentStatusAsync(UpdatePaymentDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Processes a payment notification received from the payment gateway.
        /// </summary>
        /// <param name="dto">The payment notification data containing details about the transaction.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<IBaseResult> PaymentNotificationAsync(PayGateNotificationDto dto, CancellationToken cancellationToken = default);
    }
}
