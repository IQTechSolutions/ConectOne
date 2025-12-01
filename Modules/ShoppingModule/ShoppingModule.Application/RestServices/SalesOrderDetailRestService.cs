using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ShoppingModule.Domain.DataTransferObjects;
using ShoppingModule.Domain.Interfaces;

namespace ShoppingModule.Application.RestServices
{
    /// <summary>
    /// Provides REST-based operations for managing sales order details.
    /// </summary>
    /// <remarks>This service allows clients to retrieve and create sales order details using HTTP-based
    /// endpoints. It implements the <see cref="ISalesOrderDetailService"/> interface to ensure consistent behavior
    /// across different implementations.</remarks>
    /// <param name="provider"></param>
    public class SalesOrderDetailRestService(IBaseHttpProvider provider) : ISalesOrderDetailService
    {
        /// <summary>
        /// Retrieves the details of a specific sales order asynchronously.
        /// </summary>
        /// <remarks>This method communicates with the underlying data provider to fetch the sales order
        /// details. Ensure that the  <paramref name="salesOrderId"/> corresponds to a valid sales order in the
        /// system.</remarks>
        /// <param name="salesOrderId">The unique identifier of the sales order whose details are to be retrieved. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// containing an enumerable collection of <see cref="SalesOrderDetailDto"/> objects representing the sales
        /// order details.</returns>
        public async Task<IBaseResult<IEnumerable<SalesOrderDetailDto>>> GetSalesOrderDetailsAsync(string salesOrderId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<SalesOrderDetailDto>>($"salesorders/details/{salesOrderId}");
            return result;
        }

        /// <summary>
        /// Creates a new sales order detail asynchronously.
        /// </summary>
        /// <remarks>This method sends a POST request to the "salesorders/details" endpoint to create the
        /// specified sales order detail. Ensure that the provided <paramref name="salesOrderDetail"/> contains valid
        /// data to avoid errors.</remarks>
        /// <param name="salesOrderDetail">The sales order detail to be created. This must contain the necessary information for the sales order
        /// detail.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that includes the created <see cref="SalesOrderDetailDto"/> if the operation is successful.</returns>
        public async Task<IBaseResult> CreateSalesOrderDetailAsync(SalesOrderDetailDto salesOrderDetail, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync("salesorders/details", salesOrderDetail);
            return result;
        }
    }
}
