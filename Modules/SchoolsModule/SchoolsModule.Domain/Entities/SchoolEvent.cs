using CalendarModule.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using MessagingModule.Domain.Entities;

namespace SchoolsModule.Domain.Entities
{
    /// <summary>
    /// Represents a school event with associated details, including location, participation, consents, and related
    /// documents.
    /// </summary>
    /// <remarks>This class extends the <see cref="Event{TEntity}"/> type to provide additional properties and
    /// collections specific to school events. It includes details such as the event's location, participation
    /// requirements, consents, and associated messages or documents.</remarks>
    /// <typeparam name="TEntity">The type of the entity associated with the event, typically representing the context or domain object related to
    /// the event.</typeparam>
    public class SchoolEvent<TEntity> : Event<TEntity>
    {
        /// <summary>
        /// Gets or sets the address associated with the entity.
        /// </summary>
        /// <remarks>The value can be <see langword="null"/> if no address is specified.</remarks>
        [StringLength(5000)] public string? Address { get; set; } = null!;

        /// <summary>
        /// Gets or sets the Google Maps link associated with the entity.
        /// </summary>
        /// <remarks>The URL should be a valid Google Maps link. Ensure the length does not exceed 5000
        /// characters.</remarks>
        [StringLength(5000)] public string? GoogleMapLink { get; set; } = null!;

        /// <summary>
        /// Gets or sets the latitude coordinate of a geographic location.
        /// </summary>
        public double Lat { get; set; }

        /// <summary>
        /// Gets or sets the longitude coordinate of a geographic location.
        /// </summary>
        public double Lng { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is published.
        /// </summary>
        public bool Published { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the event is designated as a home event.
        /// </summary>
        public bool HomeEvent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user consent is required for transport operations.
        /// </summary>
        public bool TransportConsentRequired { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether consent is required for attendance.
        /// </summary>
        public bool AttendanceConsentRequired { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether attendance should be recorded.
        /// </summary>
        public bool TakeAttendance { get; set; }

        /// <summary>
        /// Gets or sets the message content.
        /// </summary>
        /// <remarks>If the message exceeds the maximum length, an exception may be thrown when validating
        /// the property.</remarks>
        [StringLength(10000)] public string? Message { get; set; }

        /// <summary>
        /// Gets or sets a collection of document links as a single string.
        /// </summary>
        /// <remarks>The maximum length of the string is 10,000 characters. Ensure that the links are
        /// properly formatted and delimited to allow for correct parsing or usage.</remarks>
        [StringLength(10000)] public string? DocumentLinks { get; set; }

        /// <summary>
        /// Gets or sets the collection of ticket types available for the school event.
        /// </summary>
        public ICollection<SchoolEventTicketType> TicketTypes { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of activity groups participating in the current context.
        /// </summary>
        public ICollection<ParticipatingActivityGroup> ParticipatingActivityGroups { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of categories that are participating in the activity group.
        /// </summary>
        public ICollection<ParticipatingActivityGroupCategory> ParticipatingCategories { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of event consents provided by parents.
        /// </summary>
        public virtual ICollection<ParentPermission> EventConsents { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of messages associated with the current context.
        /// </summary>
        public ICollection<Message> Messages { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of views associated with the school event.
        /// </summary>
        public ICollection<SchoolEventViews> Views { get; set; } = [];
    }
}
