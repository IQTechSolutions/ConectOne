using System.ComponentModel;

namespace ProductsModule.Domain.Enums
{
    /// <summary>
    /// Specifies the frequency at which billing occurs for a service or product.
    /// </summary>
    /// <remarks>This enumeration defines various billing intervals, ranging from one-time payments to
    /// recurring  payments at different time intervals. Use this enum to indicate the desired billing frequency  when
    /// configuring a billing plan or subscription.</remarks>
    public enum BillingFrequency
    {
        /// <summary>
        /// Represents a state where no specific value or option is selected.
        /// </summary>
        [Description("None")] None = 0,

        /// <summary>
        /// Represents a one-time operation or event.
        /// </summary>
        [Description("Once Off")] OnceOff = 1,

        /// <summary>
        /// Represents an hourly recurrence frequency.
        /// </summary>
        /// <remarks>This value is typically used to specify that an operation or event should occur every
        /// hour.</remarks>
        [Description("Hourly")] Hourly = 2,

        /// <summary>
        /// Represents a daily recurrence pattern.
        /// </summary>
        /// <remarks>This value is typically used to specify that an operation or event occurs on a daily
        /// basis.</remarks>
        [Description("Daily")] Daily = 3,

        /// <summary>
        /// Represents a weekly recurrence pattern.
        /// </summary>
        [Description("Weekly")] Weekly = 4,

        /// <summary>
        /// Represents a monthly recurrence pattern.
        /// </summary>
        /// <remarks>This value is typically used to specify that an operation or event occurs on a
        /// monthly basis.</remarks>
        [Description("Monthly")] Monthly = 6,

        /// <summary>
        /// Represents a quarterly recurrence pattern.
        /// </summary>
        [Description("Quarterly")] Quarterly = 7,

        /// <summary>
        /// Represents a yearly recurrence pattern.
        /// </summary>
        /// <remarks>This value is typically used to specify that an operation or event occurs on a yearly
        /// basis.</remarks>
        [Description("Yearly")] Yearly = 8,

        /// <summary>
        /// Represents the "Contact Us" page or section in the application.
        /// </summary>
        [Description("Contac tUs")] ContactUs = 9
    }
}
