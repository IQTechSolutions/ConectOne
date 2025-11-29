using ConectOne.Domain.ResultWrappers;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using GoogleCalendar.Auth;
using IdentityModule.Domain.DataTransferObjects;
using MessagingModule.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.SchoolEvents;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Infrastructure.Controllers
{
    /// <summary>
    /// Controller to handle various School Event endpoints, including listing, paging, retrieving, creating,
    /// updating, and deleting events, as well as interaction with Google Calendar and associated permissions.
    /// </summary>
    [Route("api/schoolevents"), ApiController, Authorize(AuthenticationSchemes = "Bearer")]
    public class SchoolEventController(ISchoolEventCategoryService schoolEventCategoryService, ISchoolEventQueryService schoolEventQueryService, 
        ISchoolEventCommandService schoolEventCommandService, ISchoolEventPermissionService schoolEventPermissionService, 
        ISchoolEventExportService schoolEventExportService, ISchoolEventNotificationService schoolEventNotificationService) : ControllerBase
    {
        
        /// <summary>
        /// Fetches paged school events for the application with filtering and pagination parameters.
        /// </summary>
        [HttpGet("pagedevents/app")] public async Task<IActionResult> PagedSchoolEventsForAppAsync([FromQuery] SchoolEventPageParameters pageParameters)
        {
            var result = await schoolEventQueryService.PagedSchoolEventsForAppAsync(pageParameters);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a paginated list of school events for app ticket sales.
        /// </summary>
        /// <remarks>This endpoint is designed to support app-based ticket sales by providing a paginated
        /// view of school events. Ensure that the <paramref name="pageParameters"/> object contains valid values for
        /// pagination.</remarks>
        /// <param name="pageParameters">The pagination parameters, including page number and page size, used to filter the results.</param>
        /// <returns>An <see cref="IActionResult"/> containing the paginated list of school events. The response includes
        /// metadata such as total count and pagination details.</returns>
        [HttpGet("pagedevents/app/tickets/sale")] public async Task<IActionResult> PagedSchoolEventsForAppTicketSalesAsync([FromQuery] SchoolEventPageParameters pageParameters)
        {
            var result = await schoolEventQueryService.PagedSchoolEventsForAppAsync(pageParameters);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves paged categories of school event activities/groups for the application.
        /// </summary>
        [HttpGet("pagedcategories/app")] public async Task<IActionResult> PagedSchoolEventActivityGroupCategoriesForAppAsync([FromQuery] SchoolEventPageParameters pageParameters)
        {
            var result = await schoolEventCategoryService.PagedSchoolEventActivityGroupCategoriesForAppAsync(pageParameters);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves events from a specified Google Calendar and returns them.
        /// </summary>
        [HttpGet("googlecalendar/events")] public async Task<IActionResult> GoogleCalendarEventsAsync()
        {
            try
            {
                // Authenticate with Google API using a custom Authenticator
                var initializer = Authenticator.Authenticate("https://www.googleapis.com/auth/calendar");

                // Initialize a new CalendarService instance with the authenticated credentials
                var init = new BaseClientService.Initializer()
                {
                    HttpClientInitializer = initializer,
                    ApplicationName = $"Google Calendar",
                };
                var calendarService = new CalendarService(init);

                // Prepare a list to collect all calendar events
                var googleCalendarItems = new List<Event>();

                // Some calendars may paginate results. We'll loop until we get all events.
                string pageToken = string.Empty;
                do
                {
                    // Create a request to list events from the given calendar address
                    var eventsRequest = calendarService.Events.List("evercalender@gmail.com");
                    eventsRequest.MaxResults = 250;
                    eventsRequest.PageToken = pageToken;

                    // Execute the request and fetch the results
                    var gooleCalendarItemsResult = eventsRequest.Execute();

                    // Add the retrieved events to the master list
                    googleCalendarItems.AddRange(gooleCalendarItemsResult.Items);

                    // If there's another page of events, store the token and repeat
                    pageToken = gooleCalendarItemsResult.NextPageToken;
                }
                while (pageToken != null);

                // Return a success result containing all the events
                return Ok(await Result<List<Event>>.SuccessAsync(googleCalendarItems));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Returns paged school events in a format suitable for displaying in a calendar application.
        /// </summary>
        [HttpGet("all/app/calendar")] public async Task<IActionResult> PagedSchoolEventsForAppCalendarAsync([FromQuery] SchoolEventPageParameters pageParameters)
        {
            var result = await schoolEventQueryService.PagedSchoolEventsForAppCalendarAsync(pageParameters);
            return Ok(result);
        }

        /// <summary>
        /// Fetches paged school events with the given parameters (general endpoint).
        /// </summary>
        [HttpGet("pagedevents")] public async Task<IActionResult> PagedSchoolEventsAsync([FromQuery] SchoolEventPageParameters pageParameters)
        {
            var result = await schoolEventQueryService.PagedSchoolEventsAsync(pageParameters);
            return Ok(result);
        }

        /// <summary>
        /// Fetches paged school events that represent historical data.
        /// </summary>
        [HttpGet("pagedevents/history")] public async Task<IActionResult> PagedSchoolEventsHistoryAsync([FromQuery] SchoolEventPageParameters pageParameters)
        {
            var result = await schoolEventQueryService.PagedSchoolEventsAsync(pageParameters);
            return Ok(result);
        }

        /// <summary>
        /// Fetches paged school events specifically for parents, filtered by parent criteria.
        /// </summary>
        [HttpGet("pagedevents/byparent")] public async Task<IActionResult> PagedSchoolEventsForParentAsync([FromQuery] SchoolEventPageParameters pageParameters)
        {
            var result = await schoolEventQueryService.PagedSchoolEventsForParentAsync(pageParameters);
            return Ok(result);
        }

        /// <summary>
        /// Historical events endpoint for parents.
        /// </summary>
        [HttpGet("pagedevents/byparent/history")] public async Task<IActionResult> PagedSchoolEventsHistoryForParentAsync([FromQuery] SchoolEventPageParameters pageParameters)
        {
            var result = await schoolEventQueryService.PagedSchoolEventsForParentAsync(pageParameters);
            return Ok(result);
        }

        /// <summary>
        /// Fetches paged school events specifically for a particular learner.
        /// </summary>
        [HttpGet("pagedevents/bylearner")] public async Task<IActionResult> PagedSchoolEventsForLearnerAsync([FromQuery] SchoolEventPageParameters pageParameters)
        {
            var result = await schoolEventQueryService.PagedSchoolEventsForLearnerAsync(pageParameters);
            return Ok(result);
        }

        /// <summary>
        /// Historical events endpoint for learners.
        /// </summary>
        [HttpGet("pagedevents/bylearner/history")] public async Task<IActionResult> PagedSchoolEventsHistoryForLearnerAsync([FromQuery] SchoolEventPageParameters pageParameters)
        {
            var result = await schoolEventQueryService.PagedSchoolEventsForLearnerAsync(pageParameters);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves the count of all events.
        /// </summary>
        [HttpGet("count")] public async Task<IActionResult> UserCount()
        {
            var result = await schoolEventQueryService.EventCount(HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves paged school events with minimal info (perhaps for a lightweight display).
        /// </summary>
        [HttpGet("pagedevents/minimalinfo")] public async Task<IActionResult> PagedSchoolEventsMinimalAsync([FromQuery] SchoolEventPageParameters pageParameters)
        {
            var result = await schoolEventQueryService.PagedSchoolEventsMinimalAsync(pageParameters);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves minimal information about a specific event by its ID.
        /// </summary>
        [HttpGet("{eventId}/minimalinfo")] public async Task<IActionResult> EventMinimalAsync(string eventId)
        {
            var result = await schoolEventQueryService.SchoolEventAsync(eventId);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves detailed information about a specific event by its ID.
        /// </summary>
        [HttpGet("{eventId}")] public async Task<IActionResult> EventAsync(string eventId)
        {
            try
            {
                var result = await schoolEventQueryService.SchoolEventAsync(eventId);
                return Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           
        }

        /// <summary>
        /// Creates a new school event.
        /// </summary>
        [HttpPut] public async Task<IActionResult> Create([FromBody] SchoolEventDto schoolClass)
        {
            var result = await schoolEventCommandService.Create(schoolClass, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Updates an existing school event asynchronously.
        /// </summary>
        [HttpPost] public async Task<IActionResult> UpdateAsync([FromBody] SchoolEventDto schoolClass)
        {
            var result = await schoolEventCommandService.UpdateAsync(schoolClass, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a school event by its ID.
        /// </summary>
        [HttpDelete("{schoolClassId}")] public async Task<IActionResult> DeleteAsync(string schoolClassId)
        {
            var result = await schoolEventCommandService.DeleteAsync(schoolClassId, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves the count of school event permissions for the specified participating activity group.
        /// </summary>
        /// <remarks>The method returns an HTTP 200 OK response with the permissions count if the
        /// operation is successful. Ensure that the <paramref name="participatingActivityGroupId"/> corresponds to a
        /// valid activity group.</remarks>
        /// <param name="participatingActivityGroupId">The identifier of the participating activity group for which to retrieve the permissions count. This
        /// parameter cannot be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> containing the count of permissions as the response payload.</returns>
        [HttpGet("teamMembers/permissions/count")]
        public async Task<IActionResult> SchoolEventPermissionsCountAsync(string participatingActivityGroupId)
        {
            var result = await schoolEventPermissionService.GetAllParentPermissions(participatingActivityGroupId);
            return Ok(result);
        }

        /// <summary>
        /// Lists the permissions for team members related to certain school events.
        /// </summary>
        [HttpGet("teamMembers/permissions/list")] public async Task<IActionResult> SchoolEventPermissionsListAsync([FromQuery] SchoolEventPermissionsRequestArgs args)
        {
            var result = await schoolEventPermissionService.SchoolEventPermissionsListAsync(args);
            return Ok(result);
        }

        /// <summary>
        /// Gets the permissions for team members for a specific event scenario.
        /// </summary>
        [HttpGet("teamMembers/permissions")] public async Task<IActionResult> SchoolEventPermissionsAsync([FromQuery] SchoolEventPermissionsRequestArgs args)
        {
            var result = await schoolEventPermissionService.SchoolEventPermissionsAsync(args);
            return Ok(result);
        }

        /// <summary>
        /// Assigns or gives permission to team members for a specific event scenario.
        /// </summary>
        [HttpPost("teamMembers/permissions")] public async Task<IActionResult> GivePermission([FromBody] TeamMemberPermissionsParams parameters)
        {
            var result = await schoolEventPermissionService.GiveConsent(parameters);
            return Ok(result);
        }

        /// <summary>
        /// Removes or retracts previously given permissions from team members.
        /// </summary>
        [HttpPost("teamMembers/permissions/remove")] public async Task<IActionResult> RetractConsent([FromBody] TeamMemberPermissionsParams parameters)
        {
            var result = await schoolEventPermissionService.RetractConsent(parameters);
            return Ok(result);
        }

        /// <summary>
        /// Tracks views (maybe impressions or page hits) of a particular event by a specific user.
        /// </summary>
        [HttpGet("views/{eventId}/{userId}")] public async Task<IActionResult> EventViews(string eventId, string userId)
        {
            var result = await schoolEventQueryService.EventViews(eventId, userId);
            return Ok(result);
        }

        #region SchoolEvent Ticket Types

        /// <summary>
        /// Retrieves the ticket types available for a specific school event.
        /// </summary>
        /// <remarks>This method is an HTTP GET endpoint that maps to the route "/ticketstypes/{eventId}".
        /// Ensure that <paramref name="eventId"/> corresponds to a valid school event.</remarks>
        /// <param name="eventId">The unique identifier of the school event for which ticket types are requested. Cannot be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> containing the ticket types for the specified event.  The result is returned
        /// as an HTTP 200 OK response with the ticket types data, or an appropriate error response if the request
        /// fails.</returns>
        [HttpGet("/ticketstypes/{eventId}")] public async Task<IActionResult> SchoolEventsTicketTypesAsync(string eventId)
        {
            var result = await schoolEventQueryService.SchoolEventTicketTypes(eventId);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves the ticket type details for a specific school event based on the provided type identifier.
        /// </summary>
        /// <remarks>This method is an HTTP GET endpoint that fetches ticket type information for school
        /// events. The result is returned as an HTTP response with a status code indicating success or
        /// failure.</remarks>
        /// <param name="typeId">The unique identifier of the ticket type to retrieve. Cannot be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> containing the ticket type details if found, or an appropriate HTTP status
        /// code if not.</returns>
        [HttpGet("/ticketstypes/type/{typeId}")] public async Task<IActionResult> SchoolEventsTicketTypeAsync(string typeId)
        {
            var result = await schoolEventQueryService.TicketType(typeId);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new ticket type for a school event.
        /// </summary>
        /// <remarks>This method processes the provided ticket type details and delegates the creation to
        /// the underlying service.  The operation respects the cancellation token provided by the HTTP
        /// context.</remarks>
        /// <param name="ticketType">The ticket type details to be created, provided as a <see cref="SchoolEventTicketTypeDto"/> object.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation. Typically, this will be an HTTP 200
        /// OK response with the created ticket type details.</returns>
        [HttpPut("ticketstypes")] public async Task<IActionResult> CreateTicketType([FromBody] SchoolEventTicketTypeDto ticketType)
        {
            var result = await schoolEventCommandService.CreateTicketTypeAsync(ticketType, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Updates the details of a school event ticket type.
        /// </summary>
        /// <remarks>This method processes an HTTP POST request to update the details of a ticket type for
        /// a school event. The operation is performed asynchronously and respects the cancellation token provided by
        /// the HTTP context.</remarks>
        /// <param name="ticketType">The updated ticket type details, provided as a <see cref="SchoolEventTicketTypeDto"/> object.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the update operation.</returns>
        [HttpPost("ticketTypes")] public async Task<IActionResult> UpdateTicketType([FromBody] SchoolEventTicketTypeDto ticketType)
        {
            var result = await schoolEventCommandService.UpdateTicketTypeAsync(ticketType, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a ticket type with the specified identifier.
        /// </summary>
        /// <remarks>This operation is performed asynchronously. The request may be canceled using the 
        /// <see cref="HttpContext.RequestAborted"/> cancellation token.</remarks>
        /// <param name="ticketTypeId">The unique identifier of the ticket type to delete.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.  Typically, this will be an HTTP 200
        /// OK response containing the result of the deletion.</returns>
        [HttpDelete("ticketstypes/{ticketTypeId}")] public async Task<IActionResult> DeleteTicketType(string ticketTypeId)
        {
            var result = await schoolEventCommandService.DeleteTicketTypeAsync(ticketTypeId, HttpContext.RequestAborted);
            return Ok(result);
        }

        #endregion

        #region ActivityGroup TeamMembers

        /// <summary>
        /// Retrieves team members participating in a specific activity group of an event.
        /// </summary>
        [HttpGet("{eventId}/activityGroup/{activityGroupId}")] public async Task<IActionResult> GetParticipatingActivityGroupTeamMembers(string eventId, string activityGroupId)
        {
            var result = await schoolEventCategoryService.ParticipatingActivityGroupTeamMembers(eventId, activityGroupId);
            return Ok(result);
        }

        #endregion

        #region Activity Groups

        /// <summary>
        /// Retrieves all activity groups participating in a given event.
        /// </summary>
        [HttpGet("{eventId}/participatingActivityGroups")] public async Task<IActionResult> ParticipatingActivityGroups(string eventId)
        {
            var result = await schoolEventCategoryService.ParticipatingActivityGroups(eventId);
            return Ok(result);
        }

        #endregion

        #region ActivityGroup Categories

        /// <summary>
        /// Retrieves all activity group categories participating in a given event.
        /// </summary>
        [HttpGet("{eventId}/participatingActivityGroupCategories")] public async Task<IActionResult> ParticipatingActivityGroupCategories(string eventId)
        {
            var result = await schoolEventCategoryService.ParticipatingActivityGroupCategories(eventId);
            return Ok(result);
        }

        #endregion

        #region Notifications

        /// <summary>
        /// Retrieves a list of users who should be notified when a specific school event (identified by <paramref name="schoolEventId"/>) is created.
        /// </summary>
        /// <param name="schoolEventId">The unique ID of the school event for which notification recipients should be fetched.</param>
        /// <returns>A list of <see cref="UserInfoDto"/> representing the users to be notified about this event.</returns>
        [HttpGet("notificationList/eventCreated/{schoolEventId}")] public async Task<IActionResult> EventCreatedNotificationList(string schoolEventId)
        {
            //var result = await schoolEventService.EventCreatedNotificationList(schoolEventId);
            //if (result.Succeeded)
            //{
            //    return Ok(await Result<IEnumerable<RecipientDto>>.SuccessAsync(result.Data));
            //}
            // return Ok(result);
            return Ok(await Result.SuccessAsync());
        }

        /// <summary>
        /// Retrieves a list of users who should be notified when a specific school event (identified by <paramref name="schoolEventId"/>) is created.
        /// </summary>
        /// <param name="schoolEventId">The unique ID of the school event for which notification recipients should be fetched.</param>
        /// <returns>A list of <see cref="UserInfoDto"/> representing the users to be notified about this event.</returns>
        [HttpGet("notificationList/{schoolEventId}")] public async Task<IActionResult> EventNotificationList(string schoolEventId)
        {
            var result = await schoolEventNotificationService.EventNotificationList(schoolEventId);
            if (result.Succeeded)
            {
                return Ok(await Result<IEnumerable<RecipientDto>>.SuccessAsync(result.Data));
            }
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a list of recipients for event notifications based on the specified event ID and consent type.
        /// </summary>
        /// <remarks>This method calls the underlying service to fetch the notification recipients based
        /// on the provided parameters. Ensure that the <paramref name="schoolEventId"/> is valid and that the <paramref
        /// name="consentType"/> corresponds to a supported consent type.</remarks>
        /// <param name="schoolEventId">The unique identifier of the school event for which notifications are being retrieved.</param>
        /// <param name="consentType">The type of consent used to filter the recipients. This determines the notification permissions.</param>
        /// <returns>An <see cref="IActionResult"/> containing a successful result with a collection of <see
        /// cref="RecipientDto"/> objects if the operation succeeds, or an appropriate result indicating the failure.</returns>
        [HttpGet("notificationList/permissions/{schoolEventId}/{consentType}")]
        public async Task<IActionResult> EventNotificationList(string schoolEventId, ConsentTypes consentType)
        {
            var result = await schoolEventNotificationService.EventPermissionNotificationList(consentType, schoolEventId);
            if (result.Succeeded)
            {
                return Ok(await Result<IEnumerable<RecipientDto>>.SuccessAsync(result.Data));
            }
            return Ok(result);
        }


        #endregion

        #region Import/Export

        /// <summary>
        /// Exports event data (likely in a certain file format or structure).
        /// </summary>
        [HttpGet("export")] public async Task<IActionResult> Export([FromQuery] SchoolEventPageParameters parameters)
        {
            var result = await schoolEventExportService.ExportEvents(parameters);
            return Ok(result);
        }

        /// <summary>
        /// Exports attendance details for a given event by its ID.
        /// </summary>
        [HttpGet("exportAttendance/{eventId}")] public async Task<IActionResult> ExportAttendance(string eventId)
        {
            var result = await schoolEventExportService.ExportEventConsents(eventId, ConsentTypes.Attendance, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Exports transport details for a given event by its ID.
        /// </summary>
        [HttpGet("exportTransport/{eventId}")] public async Task<IActionResult> ExportTransport(string eventId)
        {
            var result = await schoolEventExportService.ExportEventConsents(eventId, ConsentTypes.Transport, HttpContext.RequestAborted);
            return Ok(result);
        }

        #endregion
    }
}
