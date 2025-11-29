using System.ComponentModel;

namespace CalendarModule.Domain.Enums
{
    /// <summary>
    /// Specifies the recurrence pattern for a scheduled event or operation.
    /// </summary>
    /// <remarks>This enumeration defines the frequency at which an event or operation recurs.  Use the
    /// appropriate value to indicate the desired recurrence interval, such as  daily, weekly, or yearly. The <see
    /// cref="DescriptionAttribute"/> applied to each  value provides a human-readable description of the recurrence
    /// pattern.</remarks>
    public enum Recurrence
    {
        /// <summary>
        /// Represents a state where no specific value or option is selected.
        /// </summary>
        [Description("None")]
        None = 0,

        /// <summary>
        /// Represents an hourly recurrence frequency.
        /// </summary>
        /// <remarks>This enumeration value is typically used to specify that an operation or event should
        /// occur on an hourly basis.</remarks>
        [Description("Hourly")]
        Hourly = 1,

        /// <summary>
        /// Represents a daily recurrence pattern.
        /// </summary>
        [Description("Daily")]
        Daily = 2,

        /// <summary>
        /// Represents a weekly recurrence pattern.
        /// </summary>
        [Description("Weekly")]
        Weekly = 3,

        /// <summary>
        /// Represents a fortnightly schedule.
        /// </summary>
        [Description("Fort Nightly")]
        FortNightly = 4,

        /// <summary>
        /// Represents a monthly recurrence pattern.
        /// </summary>
        /// <remarks>This value is typically used to specify that an operation or setting applies on a
        /// monthly basis.</remarks>
        [Description("Monthly")]
        Monthly = 5,

        /// <summary>
        /// Represents a yearly recurrence frequency.
        /// </summary>
        /// <remarks>This value is typically used to specify that an event or operation recurs on a yearly
        /// basis.</remarks>
        [Description("Yearly")]
        Yearly = 6
    }
}
