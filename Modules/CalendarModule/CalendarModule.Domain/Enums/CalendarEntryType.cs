using System.ComponentModel;

namespace CalendarModule.Domain.Enums
{
    /// <summary>
    /// Specifies the type of a calendar entry.
    /// </summary>
    /// <remarks>This enumeration is used to differentiate between various types of entries in a calendar, 
    /// such as appointments, events, and recurring entries. Use this type to categorize and handle  calendar entries
    /// appropriately.</remarks>
    public enum CalendarEntryType
    {
        /// <summary>
        /// Represents an appointment in the system.
        /// </summary>
        /// <remarks>This enumeration value is used to identify an appointment entity.  It can be utilized
        /// in scenarios where appointments need to be categorized or processed.</remarks>
        [Description("Appointment")] Appointment,

        /// <summary>
        /// Represents an event in the system.
        /// </summary>
        /// <remarks>This enumeration value is used to identify an event. It can be utilized in scenarios
        /// where  event-specific logic or categorization is required.</remarks>
        [Description("Event")] Event,

        /// <summary>
        /// Represents a recurring schedule or event type.
        /// </summary>
        /// <remarks>This enumeration value is typically used to indicate that an operation, task, or
        /// event occurs repeatedly over a defined interval or schedule.</remarks>
        [Description("Recurring")] Recurring,

        /// <summary>
        /// Represents a recurring event or schedule.
        /// </summary>
        /// <remarks>This enumeration value is used to indicate that an event or process occurs repeatedly
        /// over a defined interval.</remarks>
        [Description("Recurring")] Google
    }
}
