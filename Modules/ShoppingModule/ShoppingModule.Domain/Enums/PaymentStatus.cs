using System.ComponentModel;

namespace ShoppingModule.Domain.Enums
{
    /// <summary>
    /// Represents the status of a payment in a transaction lifecycle.
    /// </summary>
    /// <remarks>This enumeration provides a set of predefined statuses that describe the outcome or current
    /// state of a payment. The values range from initial states, such as <see cref="NotDone"/>, to final states, such
    /// as <see cref="Accepted"/> or <see cref="Declined"/>. Special statuses, such as <see cref="CheckSumError"/> or
    /// <see cref="NoPaymentRconceliation"/>, indicate specific error conditions.</remarks>
    public enum PaymentStatus
    {
        /// <summary>
        /// Represents the state where the task or operation has not been completed.
        /// </summary>
        [Description("Not Done")] NotDone = 0,

        /// <summary>
        /// Represents the status of an operation that has been accepted for processing.
        /// </summary>
        [Description("Accepted")] Accepted = 1,

        /// <summary>
        /// Represents a state where the operation or request has been declined.
        /// </summary>
        [Description("Declined")] Declined = 2,

        /// <summary>
        /// Represents a state where the operation has been cancelled.
        /// </summary>
        [Description("Cancelled")] Cancelled = 3,

        /// <summary>
        /// Indicates that the operation was cancelled by the user.
        /// </summary>
        [Description("User Cancelled")] UserCancelled = 4,

        /// <summary>
        /// Indicates that the payment has been received and processed by the payment gateway.
        /// </summary>
        [Description("Received by PayGate")] ReceivedbyPayGate = 5,

        /// <summary>
        /// Represents an unknown or unspecified state.
        /// </summary>
        /// <remarks>This value is typically used as a default or placeholder when the actual state is not
        /// known or cannot be determined.</remarks>
        [Description("Unknown")] Unknown = 6,

        /// <summary>
        /// Represents a settlement that has been voided.
        /// </summary>
        /// <remarks>This status indicates that the settlement process was canceled or
        /// invalidated.</remarks>
        [Description("Settlement Voided")] SettlementVoided = 7,

        /// <summary>
        /// Represents an error code indicating a checksum error.
        /// </summary>
        /// <remarks>This error code is typically used to signify that a checksum validation has failed, 
        /// which may indicate data corruption or transmission errors.</remarks>
        [Description("Check Sum Error")] CheckSumError = 901,

        /// <summary>
        /// Represents the status code for scenarios where no payment reconciliation is performed.
        /// </summary>
        /// <remarks>This status code indicates that payment reconciliation is not applicable or has not
        /// been executed. It can be used to identify cases where payment processing does not require
        /// reconciliation.</remarks>
        [Description("No Payment Rconceliation")] NoPaymentRconceliation = 902
    }
}
