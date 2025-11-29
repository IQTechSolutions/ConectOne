using System.ComponentModel;

namespace ShoppingModule.Domain.Enums
{
    /// <summary>
    /// Represents the various statuses an order can have during its lifecycle.
    /// </summary>
    /// <remarks>The <see cref="OrderStatus"/> enumeration defines the possible states of an order,  from the
    /// initial payment stage to completion or cancellation. These statuses can  be used to track and manage the
    /// progress of an order in an e-commerce or order  processing system.</remarks>
    public enum OrderStatus
    {
        /// <summary>
        /// Indicates that a payment is pending and has not yet been completed.
        /// </summary>
        [Description("Payment Pending")] PaymentPending,

        /// <summary>
        /// Represents the state where an operation is currently being processed.
        /// </summary>
        /// <remarks>This enumeration value is typically used to indicate that a task or operation is in
        /// progress.</remarks>
        [Description("Processing")] Processing,

        /// <summary>
        /// Indicates that the item is ready to be collected.
        /// </summary>
        /// <remarks>This status is typically used to signify that the item has completed all necessary
        /// processing and is now available for collection or pickup.</remarks>
        [Description("Ready for Collection")] ReadyForCollection,

        /// <summary>
        /// Represents the state of an order that has been shipped to the customer.
        /// </summary>
        [Description("Shipped")] Shipped,

        /// <summary>
        /// Represents the state of an item that has been successfully delivered to its destination.
        /// </summary>
        [Description("Delivered")] Delivered,

        /// <summary>
        /// Represents the completed state of an operation or process.
        /// </summary>
        [Description("Completed")] Completed,

        /// <summary>
        /// Represents a cancel action or operation.
        /// </summary>
        /// <remarks>This enumeration value is typically used to indicate a cancellation state or to
        /// trigger a cancel operation.</remarks>
        [Description("Cancel")] Cancel
    }
}