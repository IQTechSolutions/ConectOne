using System.ComponentModel;

namespace PayFast
{
    /// <summary>
    /// Specifies the frequency at which billing occurs for a subscription or service.
    /// </summary>
    /// <remarks>Use this enumeration to indicate how often a customer is billed. The values represent common
    /// billing intervals, such as monthly, quarterly, biannual, or annual cycles.</remarks>
    public enum PayfastBillingFrequency
    {
        /// <summary>
        /// Represents a monthly interval or frequency.
        /// </summary>
        [Description("Monthly")] Monthly = 3,

        /// <summary>
        /// Represents a quarterly recurrence or interval, typically corresponding to a period of three months.
        /// </summary>
        [Description("Quarterly")] Quarterly = 4,

        /// <summary>
        /// Represents a biannual frequency, occurring twice per year.
        /// </summary>
        [Description("Bi-Annual")] Biannual = 5,

        /// <summary>
        /// Represents an annual interval or frequency.
        /// </summary>
        [Description("Annual")] Annual = 6
    }
}
