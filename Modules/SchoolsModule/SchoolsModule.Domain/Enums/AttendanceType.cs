using System.ComponentModel;

namespace SchoolsModule.Domain.Enums
{
    /// <summary>
    /// Represents the type of attendance for an event or activity.
    /// </summary>
    /// <remarks>This enumeration is used to specify whether the attendance is related to a class or a
    /// sport.</remarks>
    public enum AttendanceType
    {
        /// <summary>
        /// Represents the classification type for an entity.
        /// </summary>
        /// <remarks>This enumeration value is used to specify that the entity is classified as a
        /// class.</remarks>
        [Description("Class")] Class = 0,

        /// <summary>
        /// Represents a group of activities categorized under the "Activity Group."
        /// </summary>
        /// <remarks>This enumeration value is used to identify activities that belong to the "Activity
        /// Group" category.</remarks>
        [Description("Activity Group")] ActivityGroup = 1,

        /// <summary>
        /// Represents a group of individuals participating in an event activity.
        /// </summary>
        /// <remarks>This enumeration value is used to categorize participants or teams involved in
        /// event-related activities.</remarks>
        [Description("Event Activity Group")] EventTeam = 2,

        /// <summary>
        /// Represents the transport mechanism for events to a specified destination.
        /// </summary>
        /// <remarks>This enumeration value is used to indicate that events are transported to a target
        /// destination.</remarks>
        [Description("Event Transport To")] EventTransportTo = 3,

        /// <summary>
        /// Represents the source of an event transport.
        /// </summary>
        /// <remarks>This enumeration value is used to specify the origin of an event transport in the
        /// system.</remarks>
        [Description("Event Transport From")] EventTransportFrom = 4
    }
}
