using System.ComponentModel;

namespace PayFast
{
    /// <summary>
    /// Specifies the type of subscription for a scheduled operation or report.
    /// </summary>
    /// <remarks>Use this enumeration to distinguish between standard scheduled subscriptions and ad hoc
    /// (on-demand) executions. The value determines how the system processes and manages the subscription.</remarks>
    public enum SubscriptionType
    {
        /// <summary>
        /// Represents a subscription payment type.
        /// </summary>
        [Description("Subscription")] Subscription = 1,

        /// <summary>
        /// Specifies an ad hoc mode, typically used for temporary or on-demand operations.
        /// </summary>
        [Description("Ad Hoc")] AdHoc = 2
    }
}
