using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CalendarModule.Domain.Enums;
using ConectOne.Domain.Entities;
using FilingModule.Domain.Entities;
using GroupingModule.Domain.Entities;

// For EventRegistration, EventDay and other calendar-related entities
// For Recurrence enum
// For ImageFileCollection base class
// For Video<T> and other file-related entities
// For EntityCategory<T> and related grouping entities
// For Address<T> and location entities

// For ContactNumber<T> and related contact info entities

namespace CalendarModule.Domain.Entities
{
    /// <summary>
    /// Represents a calendar event that can be associated with a generic entity type (TEntity).
    /// Inherits from ImageFileCollection to manage image files related to the event.
    /// Supports recurrence, categorization, registration, and rich contact/location information.
    /// </summary>
    /// <typeparam name="TEntity">The entity type this event is associated with (e.g., a school, organization, etc.).</typeparam>
    public class Event<TEntity> : FileCollection<Event<TEntity>, string>
    {
        /// <summary>
        /// Indicates the recurrence pattern of the event (None, Daily, Weekly, etc.).
        /// Determines how often the event repeats on the calendar.
        /// </summary>
        public Recurrence RecurrenceRule { get; set; } = Recurrence.None;

        /// <summary>
        /// The main title or heading of the event. Required to ensure every event has a descriptive name.
        /// </summary>
        [Required] public string Heading { get; set; } = null!;

        /// <summary>
        /// An optional detailed description or notes about the event.
        /// Could be HTML, markdown, or plain text providing additional context to participants.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The starting date of the event. Defaults to today's date.
        /// </summary>
        [DataType(DataType.Date)] public DateTime StartDate { get; set; } = DateTime.Now.Date;

        /// <summary>
        /// The starting time of the event. Defaults to current time. 
        /// Used in combination with StartDate to determine the exact start timestamp.
        /// </summary>
        public TimeSpan StartTime { get; set; } = DateTime.Now.TimeOfDay;

        /// <summary>
        /// The ending date of the event. Defaults to today's date.
        /// May be the same day as StartDate for single-day events or a later date for multi-day events.
        /// </summary>
        [DataType(DataType.Date)] public DateTime EndDate { get; set; } = DateTime.Now.Date;

        /// <summary>
        /// The ending time of the event. Defaults to the end of the day (23:59:59).
        /// Used with EndDate to determine the exact end timestamp.
        /// </summary>
        public TimeSpan EndTime { get; set; } = new TimeSpan(0, 23, 59, 59);

        /// <summary>
        /// Indicates if the event should be featured or highlighted in UI listings.
        /// Could affect how the event is displayed (e.g., pinned at the top, special styling).
        /// </summary>
        public bool Featured { get; set; }

        /// <summary>
        /// Optional tags associated with this event, possibly for searching or categorizing events.
        /// Could be a comma-separated list of keywords.
        /// </summary>
        public string? Tags { get; set; }

        #region One-To-Many Relationships

        /// <summary>
        /// Foreign key linking this event to a particular TEntity.
        /// Represents a scenario where events belong to or are associated with another entity (e.g. a school, a venue).
        /// </summary>
        [ForeignKey(nameof(Entity))]
        public string? EntityId { get; set; }

        /// <summary>
        /// Navigation property for the associated entity. 
        /// This allows loading additional information about the entity related to this event.
        /// </summary>
        public TEntity? Entity { get; set; }

        /// <summary>
        /// Foreign key linking this event to a location/address.
        /// If provided, the event occurs at this location.
        /// </summary>
        [ForeignKey("Location")]
        public string? LocationId { get; set; }

        /// <summary>
        /// Navigation property for the event's physical location.
        /// Helps participants know where the event is taking place.
        /// </summary>
        public Address<Event<TEntity>>? Location { get; set; }

        #endregion

        #region Many-To-One Relationships

        /// <summary>
        /// A collection of contact numbers associated with this event.
        /// Could represent customer service lines, event organizers, or emergency contacts.
        /// </summary>
        public ICollection<ContactNumber<Event<TEntity>>> Contacts { get; set; } = new List<ContactNumber<Event<TEntity>>>();

        /// <summary>
        /// A collection of categories (EntityCategory) that this event belongs to.
        /// Allows grouping events by certain themes, types, or interest groups.
        /// </summary>
        public ICollection<EntityCategory<Event<TEntity>>> Categories { get; set; } = new List<EntityCategory<Event<TEntity>>>();

        /// <summary>
        /// A collection of registrations associated with this event.
        /// If the event requires attendees to register, these records track who has signed up.
        /// </summary>
        public virtual ICollection<EventRegistration> Registrations { get; set; } = new List<EventRegistration>();

        /// <summary>
        /// A collection of event days. Useful if the event spans multiple non-contiguous days or sessions,
        /// detailing the schedule or program for each day.
        /// </summary>
        public virtual ICollection<EventDay> EventDays { get; set; } = new List<EventDay>();

        #endregion
    }
}
