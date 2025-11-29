using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;

namespace CalendarModule.Domain.Entities
{
    /// <summary>
    /// Represents a single day (or timeslot) within a larger event.
    /// Some events span multiple days, and each day may have its own schedule or timeframe.
    /// The EventDay class allows breaking down an event into daily segments with specific start/end times.
    /// </summary>
    public class EventDay : EntityBase<string>
    {
        /// <summary>
        /// The specific date of this event day. Combined with StartTime and EndTime
        /// to represent when the event activities occur on this particular date.
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// The time on the given day when the event activities start.
        /// Helps define a schedule for the day's session of the event.
        /// </summary>
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// The time on the given day when the event activities end.
        /// This, along with StartTime, defines the daily segment of the event.
        /// </summary>
        public TimeSpan EndTime { get; set; }

        #region One-To-Many Relationships

        /// <summary>
        /// A foreign key linking this EventDay to a specific EventRegistration (i.e., a participant's registration).
        /// This association suggests that different days of the event may be tracked per registration or that
        /// registrations might have day-specific allocations or attendance records.
        /// </summary>
        [ForeignKey("EventRegistration")] public string? EventRegistrationId { get; set; }

        /// <summary>
        /// Navigation property referencing the associated EventRegistration entity.
        /// Through this, you can access registration details for the specific day of the event,
        /// potentially enabling logic like checking who attended on which days.
        /// </summary>
        public EventRegistration? EventRegistration { get; set; }

        #endregion
    }
}
