using CalendarModule.Domain.Enums;
using IdentityModule.Domain.DataTransferObjects;

namespace CalendarModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for a calendar entry.
    /// This DTO is used to transfer calendar entry data between different layers of the application.
    /// </summary>
    /// <param name="Id">The unique identifier of the calendar entry.</param>
    /// <param name="Name">The name or title of the calendar entry.</param>
    /// <param name="StartDate">The start date of the calendar entry.</param>
    /// <param name="StartTime">The start time of the calendar entry.</param>
    /// <param name="EndDate">The end date of the calendar entry.</param>
    /// <param name="EndTime">The end time of the calendar entry.</param>
    /// <param name="CalendarEntryType">The type of the calendar entry (e.g., Appointment, Event, Recurring).</param>
    /// <param name="Url">The URL associated with the calendar entry.</param>
    /// <param name="FullDayEvent">Flag to indicate if the event spans the entire day.</param>
    public record CalendarEntryDto(string Id, string Name, DateTime? StartDate, TimeSpan? StartTime, DateTime? EndDate, TimeSpan? EndTime, CalendarEntryType CalendarEntryType, string Url, bool FullDayEvent)
    {
        /// <summary>
        /// Gets or sets the color associated with the object.
        /// </summary>
        public string Color { get; set; } = "#1e90ff";

        /// <summary>
        /// Gets or sets the description of the object.
        /// </summary>
        public string? Description { get; set; } 

        /// <summary>
        /// Controls who may access the appointment.
        /// </summary>
        public AppointmentAudienceType AudienceType { get; init; } = AppointmentAudienceType.Public;

        /// <summary>
        /// The identifiers of users invited to the appointment.
        /// </summary>
        public IReadOnlyCollection<UserInfoDto> InvitedUsers { get; init; } = Array.Empty<UserInfoDto>();

        /// <summary>
        /// The identifiers of roles invited to the appointment.
        /// </summary>
        public IReadOnlyCollection<RoleDto> InvitedRoles { get; init; } = Array.Empty<RoleDto>();
    }
}
