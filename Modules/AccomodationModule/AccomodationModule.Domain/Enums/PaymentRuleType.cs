using System.ComponentModel;

namespace AccomodationModule.Domain.Enums
{
    /// <summary>
    /// Specifies the type of payment rule used to calculate charges or deposits.
    /// </summary>
    /// <remarks>This enumeration defines the available payment rule types, which determine how payment
    /// amounts are calculated. Use <see cref="PercentageOfTotalCharge"/> for rules based on a percentage of the total
    /// charge,  <see cref="PercentageOfDeposit"/> for rules based on a percentage of the deposit,  and <see
    /// cref="FixedAmount"/> for rules that specify a fixed payment amount.</remarks>
    public enum PaymentRuleType
    {
        /// <summary>
        /// The payment rule is a percentage of the total charge.
        /// </summary>
        [Description("% of Total Charge")] PercentageOfTotalCharge,

        /// <summary>
        /// The payment rule is a percentage of the deposit.
        /// </summary>
        [Description("% of Deposit")] PercentageOfDeposit,

        /// <summary>
        /// The payment rule is a fixed amount.
        /// </summary>
        [Description("Fixed Amount")] FixedAmount
    }
}
