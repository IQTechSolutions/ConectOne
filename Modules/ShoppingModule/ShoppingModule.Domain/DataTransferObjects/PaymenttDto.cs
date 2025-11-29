using ShoppingModule.Domain.Enums;

namespace ShoppingModule.Domain.DataTransferObjects
{
	/// <summary>
	/// The data transfer object that transfers information about a payment
	/// </summary>
    public record PaymenttDto
    {
        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public PaymenttDto() { }

        /// <summary>
        /// Default Cosntructor
        /// </summary>
        /// <param name="salesOrderId">The identity of the sales order this payment is allocated to</param>
        /// <param name="paymentDate">The date this payment was made</param>
        /// <param name="paymentReference">The reference number on which the payment was made</param>
        /// <param name="amount">The ammount of the payment made</param>
        /// <param name="notes">Any additional information that should accompany the payment</param>
        /// <param name="paymentMethod">The payment method used to create this payment</param>
        /// <param name="receiptNr">The receipt number allocated to this payment</param>
        public PaymenttDto(string? salesOrderId, DateTime paymentDate, string paymentReference, double amount, string notes, PaymentMethod paymentMethod, PaymentStatus paymentStatus, int? receiptNr = null)
        {
            ReceiptNr = receiptNr;
            SalesOrderId = salesOrderId;
            PaymentDate = paymentDate;
            Reference = paymentReference;
            Notes = notes;
            Ammount = amount;
            PaymentStatus = paymentStatus;
            PaymentMethod = paymentMethod;
        }

        #endregion

        /// <summary>
        /// The receipt number allocated to this payment
        /// </summary>
        public int? ReceiptNr { get; init; }

        /// <summary>
        /// The date this payment was made
        /// </summary>
        public DateTime PaymentDate { get; init; } = DateTime.Now;

        /// <summary>
        /// The identity of the sales order this payment is allocated to
        /// </summary>
        public string? SalesOrderId { get; init; }

        /// <summary>
        /// The reference number on which the payment was made
        /// </summary>
        public string Reference { get; init; }

        /// <summary>
        /// Any additional information that should accompany the payment
        /// </summary>
        public string Notes { get; init; }

        /// <summary>
        /// The ammount of the payment made
        /// </summary>
        public double Ammount { get; init; }

        /// <summary>
        /// The payment method used to create this payment
        /// </summary>
        public PaymentMethod PaymentMethod { get; init; }

        /// <summary>
        /// The current payment status of the featured payment
        /// </summary>
        public PaymentStatus PaymentStatus { get; init; }
    }
}
