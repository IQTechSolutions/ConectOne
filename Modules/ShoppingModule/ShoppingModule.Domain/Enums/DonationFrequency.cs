using System.ComponentModel;

namespace ShoppingModule.Domain.Enums
{
    /// <summary>
    /// Specifies the frequency at which a donation is made.
    /// </summary>
    /// <remarks>This enumeration is used to define recurring donation intervals or a one-time
    /// donation.</remarks>
    public enum DonationFrequency
    {
        /// <summary>
        /// Represents a one-time operation or event that occurs only once.
        /// </summary>
        /// <remarks>This type is typically used to indicate or manage operations that are executed a
        /// single time  and are not intended to be repeated.</remarks>
        [Description("Once Off")] OnceOff,

        /// <summary>
        /// Represents a monthly recurrence pattern.
        /// </summary>
        [Description("Monthly")] Monthly,

        /// <summary>
        /// Represents the quarterly frequency option for a specific operation or setting.
        /// </summary>
        /// <remarks>This value is typically used to indicate that an operation or setting occurs or
        /// applies every quarter.</remarks>
        [Description("Quarterly")] Quarterly,

        /// <summary>
        /// Represents an annual frequency, typically used to indicate that an operation or event occurs once per year.
        /// </summary>
        /// <remarks>This value is commonly used in scenarios where yearly scheduling or recurrence is
        /// required.</remarks>
        [Description("Annually")] Annually
    }
}
