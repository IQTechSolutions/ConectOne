using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ShoppingModule.Domain.DataTransferObjects;
using ShoppingModule.Domain.Interfaces;
using ShoppingModule.Domain.RequestFeatures;

namespace ShoppingModule.Application.RestServices
{
    /// <summary>
    /// Provides a REST-based implementation of the <see cref="ISalesOrderService"/> interface for managing sales orders
    /// and related operations.
    /// </summary>
    /// <remarks>This service interacts with a REST API to perform operations such as retrieving paginated
    /// sales orders, fetching individual sales orders, creating new sales orders, updating order statuses, managing
    /// payments, and handling payment notifications. It uses an <see cref="IBaseHttpProvider"/> to perform HTTP
    /// requests and handle responses.</remarks>
    /// <param name="provider"></param>
    public class SalesOrderRestService(IBaseHttpProvider provider) : ISalesOrderService
    {
        /// <summary>
        /// Retrieves a paginated list of sales orders based on the specified parameters.
        /// </summary>
        /// <remarks>This method queries the sales orders endpoint and returns the results in a paginated
        /// format.  Ensure that the <paramref name="parameters"/> object is properly configured to avoid unexpected
        /// results.</remarks>
        /// <param name="parameters">The parameters used to define the pagination and filtering criteria for the sales orders.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing the paginated list of sales orders that match the specified
        /// criteria.</returns>
        public async Task<PaginatedResult<SalesOrderDto>> PagedSalesOrdersAsync(SalesOrderPageParameters parameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetPagedAsync<SalesOrderDto, SalesOrderPageParameters>("salesorders", parameters);
            return result;
        }

        /// <summary>
        /// Retrieves a sales order by its unique identifier.
        /// </summary>
        /// <remarks>This method asynchronously retrieves the details of a sales order using the provided
        /// identifier. If the sales order does not exist, the result may indicate an error or an empty response,
        /// depending on the implementation of the provider.</remarks>
        /// <param name="salesOrderId">The unique identifier of the sales order to retrieve. This value cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that wraps the <see cref="SalesOrderDto"/> for the specified sales order.</returns>
        public async Task<IBaseResult<SalesOrderDto>> SalesOrderAsync(string salesOrderId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<SalesOrderDto>($"salesorders/{salesOrderId}");
            return result;
        }

        /// <summary>
        /// Creates a new sales order asynchronously.
        /// </summary>
        /// <remarks>This method sends the provided sales order to the server for creation. Ensure that
        /// the <paramref name="newOrder"/> object contains all required fields as per the server's validation rules.
        /// The operation may be canceled by passing a cancellation token.</remarks>
        /// <param name="newOrder">The sales order to be created. This must contain all required fields for a valid sales order.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that includes the created sales order details if the operation is successful.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IBaseResult<SalesOrderDto>> CreateSalesOrderAsync(SalesOrderDto newOrder, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<SalesOrderDto, SalesOrderDto>($"salesorders", newOrder);
            return result; throw new NotImplementedException();
        }

        /// <summary>
        /// Updates the status of a sales order asynchronously.
        /// </summary>
        /// <remarks>This method sends a request to update the status of a sales order. Ensure that the
        /// provided <paramref name="update"/> object contains valid data, including the sales order ID and the desired
        /// status.</remarks>
        /// <param name="update">An object containing the sales order identifier and the new status to be applied.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that includes the updated sales order details.</returns>
        public async Task<IBaseResult<SalesOrderDto>> ChangeOrderStatusAsync(UpdateSalesOrderStatusDto update, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<SalesOrderDto, UpdateSalesOrderStatusDto>($"salesorders/changeStatus", update);
            return result;
        }

        /// <summary>
        /// Creates a payment for a sales order asynchronously.
        /// </summary>
        /// <remarks>This method sends a request to create a payment for a sales order and returns the
        /// result. Ensure that the provided <paramref name="payment"/> object contains valid data before calling this
        /// method.</remarks>
        /// <param name="payment">The payment details to be created, represented as a <see cref="PaymenttDto"/> object.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that wraps the created payment details as a <see cref="PaymenttDto"/>.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IBaseResult<PaymenttDto>> CreatePaymentAsync(PaymenttDto payment, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<PaymenttDto, PaymenttDto>($"salesorders/createPayment", payment);
            return result; throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves all payment details associated with the specified sales order.
        /// </summary>
        /// <remarks>This method asynchronously retrieves payment details for a specific sales order.
        /// Ensure that the  <paramref name="salesOrderId"/> is valid and corresponds to an existing sales
        /// order.</remarks>
        /// <param name="salesOrderId">The unique identifier of the sales order for which payment details are to be retrieved.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// containing an enumerable collection of <see cref="PaymenttDto"/> objects representing the payment details.</returns>
        public async Task<IBaseResult<IEnumerable<PaymenttDto>>> AllSalesOrderPayments(string salesOrderId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<PaymenttDto>>($"salesorders/payments/{salesOrderId}");
            return result;
        }

        /// <summary>
        /// Retrieves payment details for a sales order using the specified PayGate request ID.
        /// </summary>
        /// <remarks>This method communicates with the underlying provider to fetch payment details
        /// associated with the given PayGate request ID. Ensure that the provided ID is valid and corresponds to an
        /// existing payment record.</remarks>
        /// <param name="paygateReqestId">The unique identifier for the PayGate request. This value cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the payment details as a <see cref="PaymenttDto"/>. If no payment details are found, the result
        /// may indicate an empty or null value depending on the implementation.</returns>
        public async Task<IBaseResult<PaymenttDto>> SalesOrderPaymentViaPayGateRequestId(string paygateReqestId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<PaymenttDto>($"salesorders/payments/paygate/{paygateReqestId}");
            return result;
        }

        /// <summary>
        /// Updates the payment status of a sales order asynchronously.
        /// </summary>
        /// <remarks>This method sends a request to update the payment status of a sales order. Ensure
        /// that the <paramref name="dto"/> parameter contains valid data before calling this method.</remarks>
        /// <param name="dto">An object containing the payment status update details.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> ChangePaymentStatusAsync(UpdatePaymentDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"salesorders/payments/changeStatus", dto);
            return result;
        }

        /// <summary>
        /// Sends a payment notification to the payment gateway.
        /// </summary>
        /// <remarks>This method sends the payment notification details provided in the <paramref
        /// name="dto"/> to the payment gateway. Ensure that the <paramref name="dto"/> contains all required fields
        /// before calling this method.</remarks>
        /// <param name="dto">The data transfer object containing payment notification details.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the notification request.</returns>
        public async Task<IBaseResult> PaymentNotificationAsync(PayGateNotificationDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync($"salesorders/payments/notification", dto);
            return result;
        }
    }
}
