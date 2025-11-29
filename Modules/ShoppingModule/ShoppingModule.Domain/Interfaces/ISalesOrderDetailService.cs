using ConectOne.Domain.ResultWrappers;
using ShoppingModule.Domain.DataTransferObjects;

namespace ShoppingModule.Domain.Interfaces
{
    /// <summary>
    /// Defines the contract for managing sales order details, including retrieving and creating sales order details.
    /// </summary>
    /// <remarks>This service provides methods to retrieve sales order details for a specific sales order and
    /// to create new sales order details. Implementations of this interface should ensure thread safety and proper
    /// validation of input parameters.</remarks>
    public interface ISalesOrderDetailService
    {
        /// <summary>
        /// Asynchronously retrieves the details of a sales order based on the specified sales order ID.
        /// </summary>
        /// <param name="salesOrderId">The unique identifier of the sales order to retrieve details for. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of  <see
        /// cref="SalesOrderDetailDto"/> objects representing the details of the specified sales order. If no details
        /// are found, the collection will be empty.</returns>
        Task<IBaseResult<IEnumerable<SalesOrderDetailDto>>> GetSalesOrderDetailsAsync(string salesOrderId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new sales order detail based on the provided data.
        /// </summary>
        /// <param name="salesOrderDetail">The data used to create the sales order detail. This must include all required fields.</param>
        /// <returns>A <see cref="SalesOrderDetailDto"/> representing the created sales order detail.</returns>
        Task<IBaseResult> CreateSalesOrderDetailAsync(SalesOrderDetailDto salesOrderDetail, CancellationToken cancellationToken = default);
    }
}
