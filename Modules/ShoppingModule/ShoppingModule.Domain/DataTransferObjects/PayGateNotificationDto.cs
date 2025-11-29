using ShoppingModule.Domain.Enums;

namespace ShoppingModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents the data transfer object (DTO) for a notification received from the PayGate payment gateway.
    /// </summary>
    /// <remarks>This DTO encapsulates the details of a payment transaction notification, including
    /// identifiers,  transaction status, payment method, and additional metadata. It is typically used to process and 
    /// validate notifications sent by PayGate after a payment event.</remarks>
    public class PayGateNotificationDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the payment gateway.
        /// </summary>
        public string PAYGATE_ID { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the payment request.
        /// </summary>
        public string PAY_REQUEST_ID { get; set; }

        /// <summary>
        /// Gets or sets the reference identifier associated with the current object.
        /// </summary>
        public string REFERENCE { get; set; }

        /// <summary>
        /// Gets or sets the current status of the payment transaction.
        /// </summary>
        public PaymentStatus TRANSACTION_STATUS { get; set; }

        /// <summary>
        /// Gets or sets the result code of the payment operation.
        /// </summary>
        public PaymentResult RESULT_CODE { get; set; }

        /// <summary>
        /// Gets or sets the authorization code used for authentication or verification purposes.
        /// </summary>
        public string AUTH_CODE { get; set; }

        /// <summary>
        /// Gets or sets the currency code associated with the transaction.
        /// </summary>
        public string CURRENCY { get; set; }

        /// <summary>
        /// Gets or sets the amount value.  
        /// </summary>
        public int AMOUNT { get; set; }

        /// <summary>
        /// Gets or sets the description of the result.
        /// </summary>
        public string RESULT_DESC { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for a transaction.
        /// </summary>
        public string TRANSACTION_ID { get; set; }

        /// <summary>
        /// Gets or sets the risk indicator associated with the current context.
        /// </summary>
        public string RISK_INDICATOR { get; set; }

        /// <summary>
        /// Gets or sets the payment method associated with the transaction.
        /// </summary>
        public string PAY_METHOD { get; set; }

        /// <summary>
        /// Gets or sets the detailed description of the payment method.
        /// </summary>
        public string PAY_METHOD_DETAIL { get; set; }

        /// <summary>
        /// Gets or sets the value associated with the USER1 property.
        /// </summary>
        public string USER1 { get; set; }

        /// <summary>
        /// Gets or sets the value associated with the USER2 property.
        /// </summary>
        public string USER2 { get; set; }

        /// <summary>
        /// Gets or sets an additional user-defined string value.
        /// </summary>
        public string USER3 { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the vault.
        /// </summary>
        public string VAULT_ID { get; set; }

        /// <summary>
        /// Gets or sets the first data field associated with the PayVault system.
        /// </summary>
        public string PAYVAULT_DATA_1 { get; set; }

        /// <summary>
        /// Gets or sets the second data field associated with the PayVault system.
        /// </summary>
        public string PAYVAULT_DATA_2 { get; set; }

        /// <summary>
        /// Gets or sets the checksum value used to verify the integrity of data.
        /// </summary>
        public string CHECKSUM { get; set; }
    }
}
