using ConectOne.Domain.ResultWrappers;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using GoogleCalendar.Auth;
using GoogleCalendar.Contracts;

namespace GoogleCalendar.Service
{
    /// <summary>
    /// This service provides various methods to interact with the Google Calendar API,
    /// including creating events, retrieving events within a specific date range or all events,
    /// and fetching an individual event by its ID. It uses <see cref="Authenticator"/> for authentication
    /// and Googles official <see cref="CalendarService"/> to perform the API calls.
    /// </summary>
    public class GoogleCalendarService : IGoogleCalendarService
    {
        // A reference to the Google Calendar API service client.
        private readonly CalendarService _calendarService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleCalendarService"/> class,
        /// authenticating with Google and setting up the CalendarService with an application name.
        /// </summary>
        public GoogleCalendarService()
        {
            // Authenticate with Google using the specified scope for calendar access.
            var initializer = Authenticator.Authenticate("https://www.googleapis.com/auth/calendar");

            // Configure the CalendarService with the authenticated initializer and a custom application name.
            var init = new BaseClientService.Initializer
            {
                HttpClientInitializer = initializer,
                ApplicationName = "Google Calendar",
            };

            _calendarService = new CalendarService(init);
        }

        /// <summary>
        /// Creates a new event in the specified Google Calendar.
        /// </summary>
        /// <param name="newEvent">The event details to be created.</param>
        /// <param name="calendarId">The ID of the calendar where the event will be added.</param>
        /// <returns>An <see cref="Event"/> object representing the newly created event.</returns>
        public async Task<Event> CreateEventAsync(Event newEvent, string calendarId)
        {
            // Insert the event into the given calendar and execute the API call asynchronously.
            return await _calendarService.Events.Insert(newEvent, calendarId).ExecuteAsync();
        }

        /// <summary>
        /// Retrieves a single event from the specified calendar by its ID.
        /// </summary>
        /// <param name="eventId">The ID of the event to retrieve.</param>
        /// <param name="calendarId">The ID of the calendar that contains the event.</param>
        /// <returns>An <see cref="Event"/> object representing the fetched event, or null if not found.</returns>
        public async Task<Event> GetEventAsync(string eventId, string calendarId)
        {
            // Send a request to fetch the event details by ID, then return it.
            return await _calendarService.Events.Get(calendarId, eventId).ExecuteAsync();
        }

        /// <summary>
        /// Retrieves events between <paramref name="startDate"/> and <paramref name="endDate"/>
        /// from the calendar. Results are fetched page by page and the API
        /// call is executed asynchronously.
        /// </summary>
        /// <param name="startDate">The start date for filtering events.</param>
        /// <param name="endDate">The end date for filtering events.</param>
        /// <returns>An <see cref="IBaseResult{T}"/> containing the events if successful, or error details otherwise.</returns>
        public async Task<IBaseResult<IList<Event>>> GetEvents(DateTime startDate, DateTime endDate)
        {
            try
            {
                // The pageToken helps iterate through paginated results.
                string pageToken = null;
                var eventsList = new List<Event>();

                do
                {
                    // Prepare the request to retrieve events from a specified calendar.
                    var eventsRequest = _calendarService.Events.List("evercalender@gmail.com");
                    eventsRequest.TimeMin = startDate;
                    eventsRequest.TimeMax = endDate;
                    eventsRequest.ShowDeleted = false;
                    eventsRequest.SingleEvents = true;
                    eventsRequest.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
                    eventsRequest.MaxResults = 250;
                    eventsRequest.PageToken = pageToken;

                    // Execute the request asynchronously and collect results.
                    var response = await eventsRequest.ExecuteAsync();
                    eventsList.AddRange(response.Items);

                    // Update pageToken to retrieve subsequent pages.
                    pageToken = response.NextPageToken;
                }
                while (pageToken != null);

                // Wrap the result in a success object using your Result wrapper.
                return await Result<IList<Event>>.SuccessAsync(eventsList);
            }
            catch (Exception e)
            {
                // If something goes wrong, wrap the exception message in a failure result.
                Console.WriteLine(e);
                return await Result<IList<Event>>.FailAsync(e.Message);
            }
        }

        /// <summary>
        /// Retrieves all events from the specified calendar without any date filtering.
        /// </summary>
        /// <returns>An <see cref="IBaseResult{T}"/> containing a list of <see cref="Event"/> if successful, or an error if something goes wrong.</returns>
        public async Task<IBaseResult<IList<Event>>> AllEventsAsync()
        {
            try
            {
                string pageToken = null;
                var eventsList = new List<Event>();

                do
                {
                    // Prepare a request to list events from the calendar.
                    var eventsRequest = _calendarService.Events.List("evercalender@gmail.com");
                    eventsRequest.MaxResults = 250;
                    eventsRequest.PageToken = pageToken;

                    // Execute the request and filter out items missing date or summary.
                    var response = eventsRequest.Execute();
                    eventsList.AddRange(response.Items
                        .Where(c => !string.IsNullOrEmpty(c.Start?.Date) && !string.IsNullOrEmpty(c.Summary)));

                    // Update the page token for subsequent pages.
                    pageToken = response.NextPageToken;
                }
                while (pageToken != null);

                // Return a success result with the list of events.
                return await Result<IList<Event>>.SuccessAsync(eventsList);
            }
            catch (Exception e)
            {
                // In case of errors, log them and return a failure result.
                Console.WriteLine(e);
                return await Result<IList<Event>>.FailAsync(e.Message);
            }
        }
    }
}
