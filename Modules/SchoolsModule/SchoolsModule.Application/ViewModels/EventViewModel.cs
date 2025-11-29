using System.ComponentModel.DataAnnotations;
using FilingModule.Domain.DataTransferObjects;
using GroupingModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.DataTransferObjects;

namespace SchoolsModule.Application.ViewModels
{
    /// <summary>
    /// ViewModel for representing an event.
    /// This ViewModel is used for binding event data in the UI.
    /// </summary>
    public class EventViewModel
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public EventViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventViewModel"/> class from a <see cref="SchoolEventDto"/>.
        /// </summary>
        /// <param name="dto">The data transfer object containing event information.</param>
        public EventViewModel(SchoolEventDto dto)
        {
            EventId = dto.EventId;
            EntityId = dto.EntityId;
            Name = dto.Name;
            Details = dto.Details;
            Address = dto.Address;
            GoogleMapLink = dto.GoogleMapLink;
            StartDate = dto.StartDate;
            EndDate = dto.EndDate;
            HomeEvent = dto.HomeEvent;
            Published = dto.Published;
            AttendanceConsentRequired = dto.AttendanceConsentRequired;
            TransportPermissionRequired = dto.TransportPermissionRequired;
            SelectedActivityCategories = dto.ParticipatingCategories.ToHashSet();
            SelectedActivityGroups = dto.ParticipatingTeams.ToList();
            AnyOutstandingAttendanceConsents = dto.AnyOutStandingAttendancePermissions;
            AnyOutstandingTransportPermissions = dto.AnyOutStandingTransportPermissions;
            Documents = dto.Documents.Select(c => new DocumentDto() { FileName = c.FileName, Size = c.Size, Url = c.RelativePath }).ToList(); ;
            DocumentLinks = dto.DocumentLinks;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the event ID.
        /// </summary>
        public string EventId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the entity ID.
        /// </summary>
        public string? EntityId { get; set; }

        /// <summary>
        /// Gets or sets the cover image URL.
        /// </summary>
        public string? CoverImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the name of the event.
        /// </summary>
        [Required] public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the details of the event.
        /// </summary>
        [Required] public string? Details { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether the event is a home event.
        /// </summary>
        public bool HomeEvent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the event is published.
        /// </summary>
        public bool Published { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether attendance consent is required for the event.
        /// </summary>
        public bool AttendanceConsentRequired { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether transport permission is required for the event.
        /// </summary>
        public bool TransportPermissionRequired { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there are any outstanding attendance consents.
        /// </summary>
        public bool AnyOutstandingAttendanceConsents { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there are any outstanding transport permissions.
        /// </summary>
        public bool AnyOutstandingTransportPermissions { get; set; }

        /// <summary>
        /// Gets or sets the address of the event.
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Gets or sets the Google Map link for the event location.
        /// </summary>
        public string? GoogleMapLink { get; set; }

        /// <summary>
        /// Gets or sets the start date of the event.
        /// </summary>
        public DateTime StartDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the end date of the event.
        /// </summary>
        public DateTime EndDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the selected activity groups for the event.
        /// </summary>
        public List<ActivityGroupDto?> SelectedActivityGroups { get; set; } = new();

        /// <summary>
        /// Gets or sets the selected activity categories for the event.
        /// </summary>
        public HashSet<CategoryDto> SelectedActivityCategories { get; set; } = new();

        /// <summary>
        /// Gets or sets the collection of documents associated with the event.
        /// </summary>
        public ICollection<DocumentDto> Documents { get; set; } = new List<DocumentDto>();

        /// <summary>
        /// Gets or sets the collection of document links associated with the event.
        /// </summary>
        public ICollection<string> DocumentLinks { get; set; } = new List<string>();

        #endregion
    }
}
