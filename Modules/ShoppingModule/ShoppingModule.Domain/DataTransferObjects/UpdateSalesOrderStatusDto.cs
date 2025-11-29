using ShoppingModule.Domain.Enums;

namespace ShoppingModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents the data transfer object used to update the status of a sales order.
    /// </summary>
    /// <remarks>This DTO encapsulates the sales order identifier and the new status to be applied.  It is
    /// typically used in operations where the status of an existing sales order needs to be modified.</remarks>
    public record UpdateSalesOrderStatusDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateSalesOrderStatusDto"/> class.
        /// </summary>
        public UpdateSalesOrderStatusDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateSalesOrderStatusDto"/> class with the specified sales
        /// order ID and order status.
        /// </summary>
        /// <param name="salesOrderId">The unique identifier of the sales order to be updated. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="orderStatus">The new status to assign to the sales order.</param>
        public UpdateSalesOrderStatusDto(string salesOrderId, OrderStatus orderStatus) 
        { 
            SalesOrderId = salesOrderId;
            SalesOrderStatus = orderStatus;
        }

        /// <summary>
        /// Gets the unique identifier for the sales order.
        /// </summary>
        public string SalesOrderId { get; init; }

        /// <summary>
        /// Gets the status of the sales order.
        /// </summary>
        public OrderStatus SalesOrderStatus { get; init; }
    }
}
