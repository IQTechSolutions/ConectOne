using System.ComponentModel;

namespace ShoppingModule.Domain.Enums
{
    /// <summary>
    /// Represents the available payment methods for a transaction.
    /// </summary>
    /// <remarks>This enumeration defines the various payment methods that can be used in a transaction.  Use
    /// <see cref="PaymentMethod.None"/> to indicate no payment method has been selected.</remarks>
    public enum PaymentMethod
    {
        /// <summary>
        /// Represents the absence of a specific value or state.
        /// </summary>
        [Description("None")] None,

        /// <summary>
        /// Represents a payment method where transactions are completed using physical currency.
        /// </summary>
        [Description("Cash")] Cash,

        /// <summary>
        /// Represents a debit order transaction type.
        /// </summary>
        /// <remarks>A debit order is typically used for recurring payments, such as subscriptions or loan
        /// repayments.</remarks>
        [Description("Debit Order")] DebitOrder,

        /// <summary>
        /// Represents the Electronic Funds Transfer (EFT) payment method.
        /// </summary>
        /// <remarks>This enumeration value is typically used to indicate payments made via electronic
        /// transfer of funds.</remarks>
        [Description("Eft")] Eft,

        /// <summary>
        /// Represents a card in a collection, deck, or game.
        /// </summary>
        /// <remarks>This enumeration value is typically used to identify or categorize a card
        /// entity.</remarks>
        [Description("Card")] Card,

        /// <summary>
        /// Represents the Paygate payment method.
        /// </summary>
        /// <remarks>This enumeration value is used to indicate that the Paygate payment method is
        /// selected.</remarks>
        [Description("Paygate")] Paygate,

        /// <summary>
        /// Represents a digital wallet that can store and manage financial assets or information.
        /// </summary>
        /// <remarks>This class is typically used to encapsulate wallet-related functionality, such as
        /// managing balances,  processing transactions, or storing payment methods. The specific behavior and features
        /// of the wallet  depend on its implementation.</remarks>
        [Description("Wallet")] Wallet
    }
}
