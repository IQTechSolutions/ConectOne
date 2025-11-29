using System.ComponentModel;

namespace SchoolsModule.Domain.Enums
{
    /// <summary>
    /// Represents the attendance status of an individual.
    /// </summary>
    /// <remarks>This enumeration is used to indicate whether an individual is present, absent, or late. It
    /// can be utilized in scenarios such as attendance tracking systems or scheduling applications.</remarks>
    public enum AttendanceStatus
    {
        /// <summary>
        /// Represents the state of being present.
        /// </summary>
        /// <remarks>This enumeration value is typically used to indicate that an entity or individual is
        /// currently available or in attendance.</remarks>
        [Description("Present")] Present,

        /// <summary>
        /// Represents a state where the entity is absent.
        /// </summary>
        [Description("Absent")] Absent,

        /// <summary>
        /// Represents a state or condition where an action or event occurs later than expected or scheduled.
        /// </summary>
        /// <remarks>This enumeration value is typically used to indicate a delay or tardiness in a
        /// process or event.</remarks>
        [Description("Late")] Late
    }
}
