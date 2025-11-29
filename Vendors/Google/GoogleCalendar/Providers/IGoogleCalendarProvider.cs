using ConectOne.Domain.ResultWrappers;
using Google.Apis.Calendar.v3.Data;

namespace GoogleCalendar.Providers
{
    /// <summary>
    /// Defines a contract for retrieving events from a Google Calendar within a specified date range.
    /// </summary>
    public interface IGoogleCalendarProvider
    {
        /// <summary>
        /// Asynchronously retrieves a list of calendar events occurring within the specified date range.
        /// </summary>
        /// <param name="startDate">The start date and time of the range for which to retrieve events. Events starting on or after this date are
        /// included.</param>
        /// <param name="endDate">The end date and time of the range for which to retrieve events. Events ending on or before this date are
        /// included.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="IBaseResult{IList{Event}}"/> with a list of events within the specified date range. The list is empty
        /// if no events are found.</returns>
        Task<IBaseResult<IList<Event>>> GetCalendarEvents(DateTime startDate, DateTime endDate);
    }
}
