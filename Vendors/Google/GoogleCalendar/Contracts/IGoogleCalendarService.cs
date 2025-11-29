using ConectOne.Domain.ResultWrappers;
using Google.Apis.Calendar.v3.Data;

namespace GoogleCalendar.Contracts
{
    /// <summary>
    /// Provides methods for interacting with the Google Calendar API.
    /// It includes fetching events within a date range or all events, 
    /// and creating or retrieving specific calendar events.
    /// </summary>
    public interface IGoogleCalendarService
    {
        /// <summary>
        /// Retrieves a list of calendar events within the specified date range.
        /// </summary>
        /// <param name="startDate">The start date for filtering events.</param>
        /// <param name="endDate">The end date for filtering events.</param>
        /// <returns>An <see cref="IBaseResult"/> containing a list of <see cref="Event"/> objects if successful.</returns>
        Task<IBaseResult<IList<Event>>> GetEvents(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Retrieves all events from the calendar, without any date filtering.
        /// </summary>
        /// <returns>An <see cref="IBaseResult{T}"/> with a list of <see cref="Event"/> objects if successful.</returns>
        Task<IBaseResult<IList<Event>>> AllEventsAsync();

        /// <summary>
        /// Creates a new event in the specified calendar.
        /// </summary>
        /// <param name="newEvent">The event to be created.</param>
        /// <param name="calendarId">The ID of the Google Calendar in which the event should be created.</param>
        /// <returns>The newly created <see cref="Event"/> object.</returns>
        Task<Event> CreateEventAsync(Event newEvent, string calendarId);

        /// <summary>
        /// Retrieves a specific event by its ID from the specified calendar.
        /// </summary>
        /// <param name="eventId">The ID of the event to retrieve.</param>
        /// <param name="calendarId">The ID of the Google Calendar to search for the event.</param>
        /// <returns>The requested <see cref="Event"/>, or null if it does not exist.</returns>
        Task<Event> GetEventAsync(string eventId, string calendarId);
    }
}
