using CalendarModule.Domain.DataTransferObjects;
using CalendarModule.Domain.Enums;
using IdentityModule.Domain.DataTransferObjects;

namespace CalendarModule.Application.ViewModels
{
    /// <summary>
    /// Represents the view model for a calendar entry, containing details such as
    /// the event's ID, name, start date and time, and end date and time.
    /// </summary>
    public class CalendarEntryViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarEntryViewModel"/> class.
        /// </summary>
        public CalendarEntryViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarEntryViewModel"/> class using the specified data
        /// transfer object.
        /// </summary>
        /// <remarks>If the <paramref name="dto"/> contains a null or empty <c>Id</c>, a new GUID is
        /// generated and assigned as the <c>Id</c>.</remarks>
        /// <param name="dto">The data transfer object containing the calendar entry details. Cannot be null.</param>
        public CalendarEntryViewModel(CalendarEntryDto dto)
        {
            Id = string.IsNullOrEmpty(dto.Id) ? Guid.NewGuid().ToString() : dto.Id;
            Name = dto.Name;
            StartDate = dto.StartDate;
            StartTime = dto.StartTime;
            Color = dto.Color;
            EndDate = dto.EndDate;
            EndTime = dto.EndTime;
            Url = dto.Url;
            FullDayEvent = dto.FullDayEvent;
            AudienceType = dto.AudienceType;
            InvitedUsers = dto.InvitedUsers;
            InvitedRoles = dto.InvitedRoles;
        }

        #endregion

        /// <summary>
        /// Gets or sets the unique identifier for the calendar entry.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the name of the calendar entry.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the color associated with the object.
        /// </summary>
        public string Color { get; set; } = "#1e90ff";

        /// <summary>
        /// Gets or sets the start date of the calendar entry.
        /// </summary>
        public DateTime? StartDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the start time of the calendar entry.
        /// </summary>
        public TimeSpan? StartTime { get; set; } = new TimeSpan(8, 0, 0);

        /// <summary>
        /// Gets or sets the end date of the calendar entry.
        /// </summary>
        public DateTime? EndDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the end time of the calendar entry.
        /// </summary>
        public TimeSpan? EndTime { get; set; } = new TimeSpan(8, 30, 0);

        /// <summary>
        /// Gets or sets a value indicating whether the event spans the entire day.
        /// </summary>
        public bool FullDayEvent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current item is expanded.
        /// </summary>
        public bool Expanded { get; set; }

        /// <summary>
        /// Gets or sets the URL associated with the current instance.
        /// </summary>
        /// <remarks>Ensure that the value assigned to this property is a well-formed URI to avoid
        /// potential errors during usage.</remarks>
        public string Url { get; set; }

        /// <summary>
        /// Determines who can view the appointment.
        /// </summary>
        public AppointmentAudienceType AudienceType { get; set; } = AppointmentAudienceType.Public;

        /// <summary>
        /// Collection of invited user identifiers.
        /// </summary>
        public IEnumerable<UserInfoDto> InvitedUsers { get; set; } 

        /// <summary>
        /// Collection of invited role identifiers.
        /// </summary>
        public IEnumerable<RoleDto> InvitedRoles { get; set; }
    }
}
