using GroupingModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.DataTransferObjects;

namespace SchoolsModule.Application.ViewModels
{
    /// <summary>
    /// ViewModel for representing a school event.
    /// This ViewModel is used for binding school event data in the UI.
    /// </summary>
    public class SchoolEventViewModel : EventViewModel
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SchoolEventViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchoolEventViewModel"/> class from a <see cref="SchoolEventDto"/>.
        /// </summary>
        /// <param name="dto">The data transfer object containing school event information.</param>
        public SchoolEventViewModel(SchoolEventDto dto) : base(dto)
        {
            SelectedActivityCategories = dto.ParticipatingCategories.ToHashSet();

            HomeEvent = dto.HomeEvent;
            AttendanceConsentRequired = dto.AttendanceConsentRequired;
            AnyOutstandingAttendanceConsents = dto.AnyOutStandingAttendancePermissions;
            AnyOutstandingTransportConsents = dto.AnyOutStandingTransportPermissions;
            TransportPermissionRequired = dto.TransportPermissionRequired;
            
            TicketTypes = dto.TicketTypes.ToList();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the event is a home event.
        /// </summary>
        public bool HomeEvent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether attendance consent is required for the event.
        /// </summary>
        public bool AttendanceConsentRequired { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there are any outstanding attendance consents.
        /// </summary>
        public bool AnyOutstandingAttendanceConsents { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether transport permission is required for the event.
        /// </summary>
        public bool TransportPermissionRequired { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there are any outstanding transport consents.
        /// </summary>
        public bool AnyOutstandingTransportConsents { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether attendance should be taken for the event.
        /// </summary>
        public bool TakeAttendance { get; set; }

        /// <summary>
        /// Gets or sets the collection of ticket types available for the school event.
        /// </summary>
        public List<SchoolEventTicketTypeDto> TicketTypes { get; set; } = [];

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current instance of the school event to its corresponding data transfer object (DTO).
        /// </summary>
        /// <remarks>The returned <see cref="SchoolEventDto"/> contains all relevant details of the school
        /// event,  including event metadata, participation details, and associated document links.  This method is
        /// typically used to prepare the event data for serialization or transfer between application layers.</remarks>
        /// <returns>A <see cref="SchoolEventDto"/> object that represents the current school event.</returns>
        public SchoolEventDto ToDto()
        {
            return new SchoolEventDto()
            {
                EventId = EventId,
                Name = Name,
                Details = Details,
                Address = Address,
                GoogleMapLink = GoogleMapLink,
                StartDate = StartDate,
                EndDate = EndDate,
                HomeEvent = HomeEvent,
                Published = Published,
                TransportPermissionRequired = TransportPermissionRequired,
                AttendanceConsentRequired = AttendanceConsentRequired,
                EntityId = EntityId,
                DocumentLinks = DocumentLinks,

                ParticipatingTeams = SelectedActivityGroups.Any() ? SelectedActivityGroups.ToList() : new List<ActivityGroupDto>(),
                ParticipatingCategories = SelectedActivityCategories.Any() ? SelectedActivityCategories.ToList() : new List<CategoryDto>(),
                TicketTypes = TicketTypes.Any() ? TicketTypes.ToList() : new List<SchoolEventTicketTypeDto>(),

            };
        }

        #endregion
    }
}
