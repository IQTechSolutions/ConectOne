using FilingModule.Domain.DataTransferObjects;
using GroupingModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Domain.DataTransferObjects;

/// <summary>
/// Data Transfer Object (DTO) representing a school event, including metadata, scheduling, participation, and consent status.
/// </summary>
public record SchoolEventDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the event.
    /// </summary>
    public string EventId { get; init; }

    /// <summary>
    /// Gets or sets the display name of the event.
    /// </summary>
    public string Name { get; init; } = null!;

    /// <summary>
    /// Gets or sets a detailed description of the event.
    /// </summary>
    public string? Details { get; init; }

    /// <summary>
    /// Gets or sets the URL of the cover image associated with the item.
    /// </summary>
    public string? CoverImageUrl { get; set; }

    /// <summary>
    /// Gets or sets the physical address of the event.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Gets or sets a link to a Google Maps location for the event.
    /// </summary>
    public string? GoogleMapLink { get; set; }

    /// <summary>
    /// Gets or sets the scheduled start date of the event.
    /// </summary>
    public DateTime StartDate { get; init; }

    /// <summary>
    /// Gets or sets the scheduled end date of the event.
    /// </summary>
    public DateTime EndDate { get; init; }

    /// <summary>
    /// Indicates if the event has any outstanding consents required (transport or attendance).
    /// </summary>
    public bool AnyOutStandingPermissions => AnyOutStandingAttendancePermissions || AnyOutStandingTransportPermissions;

    /// <summary>
    /// Indicates if transport consents are still outstanding.
    /// </summary>
    public bool AnyOutStandingTransportPermissions { get; set; }

    /// <summary>
    /// Indicates if attendance consents are still outstanding.
    /// </summary>
    public bool AnyOutStandingAttendancePermissions { get; set; }

    /// <summary>
    /// Indicates if the event is hosted by the local entity (true = home event).
    /// </summary>
    public bool HomeEvent { get; init; }

    /// <summary>
    /// Indicates whether the event is published and visible to end users.
    /// </summary>
    public bool Published { get; init; }

    /// <summary>
    /// Indicates whether attendance consent is required for the event.
    /// </summary>
    public bool AttendanceConsentRequired { get; init; }

    /// <summary>
    /// Indicates whether transport permission is required for the event.
    /// </summary>
    public bool TransportPermissionRequired { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether tickets are available for sale.
    /// </summary>
    public bool TicketAvailableForSale { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether ticket sales should be advertised.
    /// </summary>
    public bool AdvertiseTicketSales { get; set; }

    /// <summary>
    /// Collection of consents already provided for this event.
    /// </summary>
    public ICollection<TeamMemberParentPermissionDto> ConsentsGiven { get; set; } = [];

    /// <summary>
    /// Entity ID associated with the event, typically referencing the school or organization.
    /// </summary>
    public string? EntityId { get; set; }

    /// <summary>
    /// List of participating activity groups (teams) in the event.
    /// </summary>
    public ICollection<ActivityGroupDto> ParticipatingTeams { get; set; } = [];

    /// <summary>
    /// List of activity categories involved in the event.
    /// </summary>
    public ICollection<CategoryDto> ParticipatingCategories { get; set; } = [];

    /// <summary>
    /// Gets or sets the collection of ticket types available for the school event.
    /// </summary>
    public ICollection<SchoolEventTicketTypeDto> TicketTypes { get; set; } = [];

    /// <summary>
    /// List of documents associated with the event.
    /// </summary>
    public ICollection<DocumentDto> Documents { get; set; } = [];

    /// <summary>
    /// Hyperlinks to external documents associated with the event.
    /// </summary>
    public ICollection<string> DocumentLinks { get; set; } = [];

    /// <summary>
    /// IDs of users who have viewed the event.
    /// </summary>
    public ICollection<string> UserViewedIds { get; set; } = [];

    /// <summary>
    /// Creates a <see cref="SchoolEvent{TEntity}"/> domain model from this DTO.
    /// </summary>
    /// <typeparam name="T">Generic type for the domain entity.</typeparam>
    /// <returns>A new instance of <see cref="SchoolEvent{T}"/> populated from the DTO.</returns>
    public SchoolEvent<T> Create<T>()
    {
        return new SchoolEvent<T>
        {
            Id = string.IsNullOrEmpty(EventId) ? Guid.NewGuid().ToString() : EventId,
            EntityId = EntityId,
            Heading = Name,
            Description = Details,
            Address = Address,
            GoogleMapLink = GoogleMapLink,
            StartDate = StartDate,
            EndDate = EndDate,
            HomeEvent = HomeEvent,
            Published = Published,
            AttendanceConsentRequired = AttendanceConsentRequired,
            TransportConsentRequired = TransportPermissionRequired,
            DocumentLinks = string.Join(";", DocumentLinks),
        };
    }

    /// <summary>
    /// Maps a domain <see cref="SchoolEvent{T}"/> instance to its corresponding DTO.
    /// </summary>
    /// <typeparam name="T">The entity type that backs the domain model.</typeparam>
    /// <param name="academicEvent">The domain entity to convert.</param>
    /// <returns>A populated <see cref="SchoolEventDto"/> instance.</returns>
    public static SchoolEventDto Create<T>(SchoolEvent<T> academicEvent)
    {
        return new SchoolEventDto
        {
            EventId = academicEvent.Id,
            EntityId = academicEvent.EntityId,
            Name = academicEvent.Heading,
            Details = academicEvent.Description,
            Address = academicEvent.Address,
            GoogleMapLink = academicEvent.GoogleMapLink,
            StartDate = academicEvent.StartDate,
            EndDate = academicEvent.EndDate,
            HomeEvent = academicEvent.HomeEvent,
            Published = academicEvent.Published,
            TransportPermissionRequired = academicEvent.TransportConsentRequired,
            AttendanceConsentRequired = academicEvent.AttendanceConsentRequired,
            DocumentLinks = !string.IsNullOrEmpty(academicEvent.DocumentLinks) ? academicEvent.DocumentLinks!.Split(";") : [],
            UserViewedIds = academicEvent.Views?.Select(c => c.UserId).ToList() ?? [],
            ParticipatingTeams = academicEvent.ParticipatingActivityGroups?
                .Where(c => c.ActivityGroup is not null)
                .Select(g => new ActivityGroupDto(g))
                .ToList() ?? [],
            ParticipatingCategories = academicEvent.ParticipatingCategories.Where(c => c.ActivityGroupCategory is not null)?
                .Select(g => CategoryDto.ToCategoryDto(g.ActivityGroupCategory!))
                .ToList() ?? [],
            AnyOutStandingAttendancePermissions = academicEvent.ParticipatingActivityGroups?
                .Where(c => c.ActivityGroup is not null)
                .Select(g => new ActivityGroupDto(g))
                .Any(c => c.AnyAttendanceConsentRequired) ?? false,
            AnyOutStandingTransportPermissions = academicEvent.ParticipatingActivityGroups?
                .Where(c => c.ActivityGroup is not null)
                .Select(g => new ActivityGroupDto(g))
                .Any(c => c.AnyTransportConsentRequired) ?? false
        };
    }
}
