using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;

namespace ShoppingModule.Domain.Entities
{
    /// <summary>
    /// Represents a payment allocation for a sales order, linking a specific payment to a sales order and tracking the
    /// amount allocated from the payment.
    /// </summary>
    /// <remarks>This class is used to associate a payment with a sales order and specify the portion of the
    /// payment that has been allocated to the sales order. It includes references to both the sales order and the
    /// payment entities.</remarks>
    public class SalesOrderPayment : EntityBase<string>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SalesOrderPayment"/> class.
        /// </summary>
        /// <remarks>This constructor creates a default instance of the <see cref="SalesOrderPayment"/>
        /// class with no properties initialized. Use this constructor when you intend to set properties manually after
        /// instantiation.</remarks>
        public SalesOrderPayment() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SalesOrderPayment"/> class with the specified sales order ID,
        /// payment ID, and allocated amount.
        /// </summary>
        /// <param name="salesOrderId">The unique identifier of the sales order associated with this payment. Cannot be <see langword="null"/> or
        /// empty.</param>
        /// <param name="paymentId">The unique identifier of the payment. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="ammount">The amount allocated from the payment to the sales order. Must be a non-negative value.</param>
        public SalesOrderPayment(string salesOrderId, string paymentId, double ammount)
        {
            SalesOrderId = salesOrderId;
            PaymentId = paymentId;
            AmmountAllocated = ammount;
        }

        #endregion

        /// <summary>
        /// Gets or sets the amount allocated for the specified purpose.
        /// </summary>
        [DisplayName("Ammount Allocated")]
        public double AmmountAllocated { get; set; }  

        /// <summary>
        /// Gets or sets the identifier for the associated sales order.
        /// </summary>
        [ForeignKey(nameof(SalesOrder))]
        public string SalesOrderId { get; set; }    

        /// <summary>
        /// Gets or sets the sales order associated with the current operation.
        /// </summary>
        public SalesOrder SalesOrder { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the associated payment.
        /// </summary>
        [ForeignKey(nameof(Payment))]
        public string PaymentId { get; set; }

        /// <summary>
        /// Gets or sets the payment details associated with the transaction.
        /// </summary>
        public Payment Payment { get; set; }
    }
}
