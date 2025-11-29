using ConectOne.Domain.Extensions;
using ConectOne.Domain.ResultWrappers;
using Google.Apis.Calendar.v3.Data;

namespace GoogleCalendar.Providers
{
    /// <summary>
    /// Provides access to Google Calendar events using an HTTP client.
    /// </summary>
    /// <remarks>This provider is intended to be used for retrieving calendar event data from a Google
    /// Calendar-compatible API. The class requires an initialized and configured HttpClient instance, which should be
    /// managed by the caller. Thread safety depends on the usage of the provided HttpClient instance.</remarks>
    public class GoogleCalendarProvider : IGoogleCalendarProvider 
    {
        private readonly HttpClient _client;

        /// <summary>
        /// Initializes a new instance of the GoogleCalendarProvider class using the specified HTTP client for API
        /// requests.
        /// </summary>
        /// <remarks>The provided HttpClient is configured with a 30-second timeout and cleared default
        /// request headers. The caller is responsible for managing the lifetime of the HttpClient instance.</remarks>
        /// <param name="client">The HttpClient instance used to send requests to the Google Calendar API. Must not be null.</param>
        public GoogleCalendarProvider(HttpClient client) 
        {
            this._client = client;
            this._client.Timeout = new TimeSpan(0, 0, 30);
            this._client.DefaultRequestHeaders.Clear();
        }

        /// <summary>
        /// Retrieves a list of calendar events occurring within the specified date range.
        /// </summary>
        /// <param name="startDate">The start date and time of the range for which to retrieve events. Events starting on or after this date are
        /// included.</param>
        /// <param name="endDate">The end date and time of the range for which to retrieve events. Events ending on or before this date are
        /// included.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="IBaseResult{IList{Event}}"/> with the list of events in the specified date range. The list is empty if
        /// no events are found.</returns>
        public async Task<IBaseResult<IList<Event>>> GetCalendarEvents(DateTime startDate, DateTime endDate)
        {
            var result = await _client.GetAsync($"googlecalendar/events/{startDate.ToFileTimeUtc()}/{endDate.ToFileTimeUtc()}");
            return await result.ToResultAsync<IList<Event>>();
        }
    }
}
