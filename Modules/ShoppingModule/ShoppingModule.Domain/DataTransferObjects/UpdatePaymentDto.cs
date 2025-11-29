using ShoppingModule.Domain.Enums;

namespace ShoppingModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents the data transfer object used to update the status of a payment.
    /// </summary>
    /// <remarks>This DTO is typically used to convey information about a payment update, including the unique
    /// identifier of the payment request and the new payment status. It is designed to be immutable  after
    /// initialization.</remarks>
    public record UpdatePaymentDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdatePaymentDto"/> class.
        /// </summary>
        public UpdatePaymentDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdatePaymentDto"/> class with the specified payment request ID
        /// and payment status.
        /// </summary>
        /// <param name="payRequestId">The unique identifier for the payment request. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="paymentStatus">The status of the payment, indicating its current state.</param>
        public UpdatePaymentDto(string payRequestId, PaymentStatus paymentStatus) 
        {
            PayRequestId = payRequestId;
            PaymentStatus = paymentStatus;
        }

        /// <summary>
        /// Gets the unique identifier for the payment request.
        /// </summary>
        public string PayRequestId { get; init; }

        /// <summary>
        /// Gets the payment status of the transaction.
        /// </summary>
        public PaymentStatus PaymentStatus { get; init; }
    }
}