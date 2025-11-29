using System.ComponentModel;

namespace ShoppingModule.Domain.Enums
{
    /// <summary>
    /// The payment result that is returned by Pay Gate
    /// </summary>
    public enum PaymentResult
    {
        /// <summary>
        /// Represents a state or condition where no specific value or action is defined.
        /// </summary>
        /// <remarks>This enumeration value is typically used as a default or placeholder to indicate the
        /// absence of a meaningful value.</remarks>
        [Description("None")] None = 900000,

        /// <summary>
        /// Represents the "Call for Approval" status.
        /// </summary>
        /// <remarks>This status is typically used to indicate that an approval process needs to be
        /// initiated or is pending.</remarks>
        [Description("Call for Approval")] CallForApproval = 900001,

        /// <summary>
        /// Represents an error code indicating that the card has expired.
        /// </summary>
        /// <remarks>This error code is typically used in scenarios where a payment or validation process
        /// fails  due to the expiration of the card. Ensure that the card's expiration date is checked before 
        /// proceeding with the operation.</remarks>
        [Description("Card Expired")] CardExpired = 900002,

        /// <summary>
        /// Represents an error code indicating that an operation failed due to insufficient funds.
        /// </summary>
        /// <remarks>This error code is typically used in financial or transactional systems to signal
        /// that the  requested operation could not be completed because the account balance or available funds  were
        /// insufficient.</remarks>
        [Description("Insufficient Funds")] InsufficientFunds = 900003,

        /// <summary>
        /// Represents an error indicating that the provided card number is invalid.
        /// </summary>
        /// <remarks>This error code is typically used in scenarios where a card number fails validation
        /// checks, such as incorrect formatting or failing a checksum validation.</remarks>
        [Description("Invalid Card Number")] InvalidCardNumber = 900004,

        /// <summary>
        /// Represents the timeout duration for the bank interface operation.
        /// </summary>
        /// <remarks>This value is used to specify the timeout setting for operations involving the bank
        /// interface. Ensure that the timeout value is configured appropriately to avoid unexpected behavior during
        /// long-running operations.</remarks>
        [Description("Bank Interface Timeout")] BankInterfaceTimeOut = 900005,

        /// <summary>
        /// Represents an error code indicating that the card is invalid.
        /// </summary>
        /// <remarks>This error code is typically used to signify that the provided card information is
        /// not valid  or does not meet the required criteria for the operation.</remarks>
        [Description("Invalid Card")] InvalidCard = 900006,

        /// <summary>
        /// Represents a status indicating that the operation or request was declined.
        /// </summary>
        /// <remarks>This enumeration value is typically used to signify that an action was explicitly
        /// rejected or not approved.</remarks>
        [Description("Declined")] Declined = 900007,

        /// <summary>
        /// Represents the status code for a lost card.
        /// </summary>
        /// <remarks>This status code is used to indicate that a card has been reported as lost.</remarks>
        [Description("Lost Card")] LostCard = 900009,

        /// <summary>
        /// Represents an error code indicating that the card length is invalid.
        /// </summary>
        /// <remarks>This error code is typically used to signal that the provided card number does not
        /// meet the expected length requirements. Ensure that the card number adheres to the specified length
        /// constraints before processing.</remarks>
        [Description("Invalid Card Lenght")] InvalidCardLenght = 900010,

        /// <summary>
        /// Represents a status code indicating suspected fraudulent activity.
        /// </summary>
        /// <remarks>This status code is typically used to flag transactions or activities that are
        /// identified as potentially fraudulent.</remarks>
        [Description("Suspected Fraud")] SuspectedFraud = 900011,

        /// <summary>
        /// Represents the status code indicating that the card has been reported as stolen.
        /// </summary>
        /// <remarks>This status code is typically used in scenarios where a cardholder has reported their
        /// card as stolen,  and the system needs to reflect this state for security or operational purposes.</remarks>
        [Description("Card Reported as Stolen")] CardReportedAsStolen = 900012,

        /// <summary>
        /// Represents a restricted card type.
        /// </summary>
        /// <remarks>This enumeration value is used to identify cards that are subject to specific
        /// restrictions.</remarks>
        [Description("Restricted Card")] RestrictedCard = 900013,

        /// <summary>
        /// Represents the status code for excessive card usage.
        /// </summary>
        /// <remarks>This status code indicates that a card has been used excessively, potentially
        /// triggering a limit or restriction.</remarks>
        [Description("Excessive Card Usage")] ExcessiveCardUsage = 900014,

        /// <summary>
        /// Represents the status code indicating that the card is blacklisted.
        /// </summary>
        /// <remarks>This status code is used to signify that a card has been flagged as blacklisted and
        /// cannot be used for transactions.</remarks>
        [Description("Card Black Listed")] CardBlacklisted = 900015,

        /// <summary>
        /// Represents an error code indicating that user authentication has failed.
        /// </summary>
        /// <remarks>This error code is typically used to signify that the authentication process for a
        /// user was unsuccessful. Ensure that the provided credentials are correct and meet the authentication
        /// requirements.</remarks>
        [Description("User Authentication Failed")] UserAuthenticationFailed = 900207,

        /// <summary>
        /// Indicates that user authentication was declined.
        /// </summary>
        /// <remarks>This value is typically used to represent a scenario where a user's authentication
        /// attempt  was explicitly rejected, such as due to invalid credentials or a failed verification
        /// process.</remarks>
        [Description("User Authentication Declined")] UserAuthenticationDeclined = 990020,

        /// <summary>
        /// Represents the error code for a 3D Secure lookup timeout.
        /// </summary>
        /// <remarks>This error code indicates that the 3D Secure lookup process exceeded the allowed time
        /// limit. It is typically used in scenarios where a timeout occurs during the authentication process.</remarks>
        [Description("3D Secure Lookup Timeout")] SecureLookupTimeout = 900210,

        /// <summary>
        /// Represents an error code indicating that the provided expiry date is invalid.
        /// </summary>
        /// <remarks>This error code is typically used to signal that an expiry date does not meet the
        /// required format,  is out of range, or fails validation checks.</remarks>
        [Description("Invalid Expiry Date")] InvalidExpiryDate = 991001,

        /// <summary>
        /// Represents an error code indicating that the specified amount is invalid.
        /// </summary>
        /// <remarks>This error code is typically used to signal that a provided amount does not meet the
        /// required criteria,  such as being negative or exceeding allowed limits. Ensure that the amount is validated
        /// before proceeding.</remarks>
        [Description("Invalid Amount")] InvalidAmount = 991002,

        /// <summary>
        /// Represents the status code indicating that authorization has been successfully completed.
        /// </summary>
        /// <remarks>This status code is typically used to signify that the authorization process has
        /// finished successfully.</remarks>
        [Description("Authorization Done")] AuthorizationDone = 990017,

        /// <summary>
        /// Represents an unexpected authentication result during the first phase of the authentication process.
        /// </summary>
        /// <remarks>This value is typically used to indicate an error or unexpected condition encountered
        /// during the initial phase of authentication.</remarks>
        [Description("Unexpected Authentication Result (Phase1)")] UnexpectedAuthenticationResultPhase1 = 900205,

        /// <summary>
        /// Represents an unexpected authentication result during the second phase of the authentication process.
        /// </summary>
        /// <remarks>This value is typically used to indicate an error or unexpected condition encountered
        /// during the second phase of authentication.</remarks>
        [Description("Unexpected Authentication Result (Phase2)")] UnexpectedAuthenticationResultPhase2 = 900206,

        /// <summary>
        /// Represents an error that occurs when an insertion operation into the database fails.
        /// </summary>
        /// <remarks>This error code indicates that the system was unable to insert the specified data
        /// into the database. It is typically used to identify and handle database insertion failures in the
        /// application.</remarks>
        [Description("Could not Insert into Database")] CouldNotInsertIntoDatabase = 990001,

        /// <summary>
        /// Represents an error code indicating that the bank is not available.
        /// </summary>
        /// <remarks>This error code is typically used to signify that the bank service is currently
        /// unavailable. It may occur due to maintenance, connectivity issues, or other temporary disruptions.</remarks>
        [Description("Bank Not Available")] BankNotAvailable = 990022,

        /// <summary>
        /// Represents an error that occurs during the processing of a transaction.
        /// </summary>
        /// <remarks>This error code indicates that a transaction could not be completed due to an
        /// unspecified issue. It is typically used in scenarios where the transaction processing system encounters an
        /// error that prevents successful completion.</remarks>
        [Description("Error Processing Transaction")] ErrorProcessingTransaction = 990053,

        /// <summary>
        /// Represents the error code for a failed transaction verification during phase 2 of the process.
        /// </summary>
        /// <remarks>This error code indicates that the transaction verification process was unsuccessful
        /// in the second phase.  It is typically used to identify and handle specific failure scenarios in multi-phase
        /// transaction workflows.</remarks>
        [Description("Transaction Verification Failed")] TransactionVerificationFailedPhase2 = 900209,

        /// <summary>
        /// Represents an error code indicating that the specified Pay Vault scope is invalid.
        /// </summary>
        /// <remarks>This error code is typically used to signal that an operation involving a Pay Vault
        /// has failed due to an invalid or unsupported scope being provided.</remarks>
        [Description("Invalid Pay Vault Scope")] InvalidPayVaultScope = 900019,

        /// <summary>
        /// Represents an error that occurs while processing a batch transaction.
        /// </summary>
        /// <remarks>This error code indicates that an issue was encountered during the processing of a
        /// batch transaction.  It is typically used to identify and handle errors specific to batch transaction
        /// workflows.</remarks>
        [Description("Error Processing Batch Transaction")] ErrorProcessingBatchTransaction = 990013,

        /// <summary>
        /// Represents the error code for a duplicate transaction detection scenario.
        /// </summary>
        /// <remarks>This error code is used to indicate that a transaction has been identified as a
        /// duplicate. It can be used in scenarios where duplicate transaction validation is required.</remarks>
        [Description("Duplicate Transaction Detected")] DuplicateTransactionDetected = 990024,

        /// <summary>
        /// Represents the status code for a transaction that has been cancelled.
        /// </summary>
        /// <remarks>This status code indicates that the transaction was explicitly cancelled and is no
        /// longer active.</remarks>
        [Description("Transaction Cancelled")] TransactionCancelled = 990028
    }
}
