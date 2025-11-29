namespace CalendarModule.Domain.Enums
{
    /// <summary>
    /// Describes the visibility and invitation strategy for a calendar appointment.
    /// </summary>
    public enum AppointmentAudienceType
    {
        /// <summary>
        /// The appointment is visible to everyone without restrictions.
        /// </summary>
        Public = 0,

        /// <summary>
        /// Only the explicitly invited users may access the appointment details.
        /// </summary>
        SpecificUsers = 1,

        /// <summary>
        /// The appointment is shared with members of the selected roles.
        /// </summary>
        SpecificRoles = 2
    }
}
