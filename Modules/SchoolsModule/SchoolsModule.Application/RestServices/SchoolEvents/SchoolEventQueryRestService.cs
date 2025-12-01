using CalendarModule.Domain.DataTransferObjects;
using ConectOne.Domain.Extensions;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.SchoolEvents;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Application.RestServices.SchoolEvents
{
    /// <summary>
    /// Provides methods for querying and retrieving school event data from a RESTful API.
    /// </summary>
    /// <remarks>This service is designed to interact with a backend API to fetch paginated results,
    /// individual event details,  and other related data for school events. It supports various filtering options, such
    /// as retrieving events  for specific users (e.g., parents or learners) and fetching minimal or detailed event
    /// information.</remarks>
    /// <param name="provider"></param>
    public class SchoolEventQueryRestService(IBaseHttpProvider provider) : ISchoolEventQueryService
    {
        /// <summary>
        /// Retrieves a paginated list of school events for the application.
        /// </summary>
        /// <remarks>This method fetches school events specifically for application use, based on the
        /// provided pagination parameters. The <paramref name="trackChanges"/> parameter determines whether the
        /// retrieved entities are tracked for changes.</remarks>
        /// <param name="pageParameters">The parameters specifying the pagination and filtering options for the school events.</param>
        /// <param name="trackChanges">A value indicating whether to track changes to the retrieved entities.  The default is <see
        /// langword="false"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing the paginated list of <see cref="SchoolEventDto"/> objects.</returns>
        public async Task<PaginatedResult<SchoolEventDto>> PagedSchoolEventsForAppAsync(SchoolEventPageParameters pageParameters, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetPagedAsync<SchoolEventDto, SchoolEventPageParameters>("schoolevents/pagedevents/app", pageParameters);
            return result;
        }

        /// <summary>
        /// Retrieves a paginated list of school events for the application.
        /// </summary>
        /// <remarks>This method is typically used to retrieve school events in a paginated format for
        /// application-level  consumption. The <paramref name="trackChanges"/> parameter is useful when working with
        /// change tracking  in Entity Framework or similar frameworks.</remarks>
        /// <param name="pageParameters">The parameters specifying the pagination details, such as page number and page size.</param>
        /// <param name="trackChanges">A value indicating whether to track changes to the retrieved entities.  Defaults to <see langword="false"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a collection of <see cref="SchoolEventDto"/> objects  and
        /// pagination metadata.</returns>
        public async Task<PaginatedResult<SchoolEventDto>> PagedSchoolEventsForAppForTicketSalesAsync(SchoolEventPageParameters pageParameters, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetPagedAsync<SchoolEventDto, SchoolEventPageParameters>("schoolevents/pagedevents/app/tickets/sale", pageParameters);
            return result;
        }

        /// <summary>
        /// Retrieves a paginated list of school events based on the specified parameters.
        /// </summary>
        /// <remarks>This method communicates with the underlying data provider to fetch the paginated
        /// results. The <paramref name="trackChanges"/> parameter is typically used to determine whether the retrieved
        /// entities should be tracked for changes in the current context.</remarks>
        /// <param name="pageParameters">The parameters that define the pagination and filtering options for the school events.</param>
        /// <param name="trackChanges">A value indicating whether to track changes to the retrieved entities. Defaults to <see langword="false"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a collection of <see cref="SchoolEventDto"/> objects that
        /// match the specified pagination and filtering criteria.</returns>
        public async Task<PaginatedResult<SchoolEventDto>> PagedSchoolEventsAsync(SchoolEventPageParameters pageParameters, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetPagedAsync<SchoolEventDto, SchoolEventPageParameters>("schoolevents/pagedevents", pageParameters);
            return result;
        }

        /// <summary>
        /// Retrieves a paginated list of school events with minimal information.
        /// </summary>
        /// <remarks>This method fetches school events with minimal details, suitable for scenarios where
        /// only basic  information is required. Use the <paramref name="pageParameters"/> to specify the page size, 
        /// page number, and any filtering criteria.</remarks>
        /// <param name="pageParameters">The parameters that define the pagination and filtering options for the school events.</param>
        /// <param name="trackChanges">A value indicating whether to track changes to the retrieved entities.  Defaults to <see langword="false"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a paginated list of <see cref="SchoolEventDto"/> objects.</returns>
        public async Task<PaginatedResult<SchoolEventDto>> PagedSchoolEventsMinimalAsync(SchoolEventPageParameters pageParameters, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetPagedAsync<SchoolEventDto, SchoolEventPageParameters>("schoolevents/pagedevents/minimalinfo", pageParameters);
            return result;
        }

        /// <summary>
        /// Retrieves a paginated list of school events associated with a parent.
        /// </summary>
        /// <remarks>This method fetches school events specifically associated with a parent, based on the
        /// provided  pagination parameters. The results are returned in a paginated format, allowing efficient handling
        /// of large datasets.</remarks>
        /// <param name="pageParameters">The parameters specifying the pagination and filtering options for the school events.</param>
        /// <param name="trackChanges">A value indicating whether to track changes to the retrieved entities.  Defaults to <see langword="false"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a collection of <see cref="SchoolEventDto"/> objects  that
        /// represent the paginated school events.</returns>
        public async Task<PaginatedResult<SchoolEventDto>> PagedSchoolEventsForParentAsync(SchoolEventPageParameters pageParameters, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetPagedAsync<SchoolEventDto, SchoolEventPageParameters>("schoolevents/pagedevents/byparent", pageParameters);
            return result;
        }

        /// <summary>
        /// Retrieves a paginated list of school events for a learner based on the specified parameters.
        /// </summary>
        /// <remarks>This method retrieves school events associated with a learner and supports pagination
        /// to limit the number of results returned in a single call. Use the <paramref name="pageParameters"/> to
        /// specify the page size and other filtering criteria.</remarks>
        /// <param name="pageParameters">The parameters that define the pagination and filtering options for the school events.</param>
        /// <param name="trackChanges">A value indicating whether to track changes to the retrieved entities. Defaults to <see langword="false"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing the paginated list of <see cref="SchoolEventDto"/> objects.</returns>
        public async Task<PaginatedResult<SchoolEventDto>> PagedSchoolEventsForLearnerAsync(SchoolEventPageParameters pageParameters, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetPagedAsync<SchoolEventDto, SchoolEventPageParameters>("schoolevents/pagedevents/bylearner", pageParameters);
            return result;
        }

        /// <summary>
        /// Retrieves a paginated list of school events for the application calendar.
        /// </summary>
        /// <remarks>This method retrieves school events formatted for use in the application calendar.
        /// The pagination parameters control the size and position of the data returned.</remarks>
        /// <param name="pageParameters">The parameters for pagination, including page size and page number.</param>
        /// <param name="trackChanges">A value indicating whether to track changes to the retrieved entities. Defaults to <see langword="false"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// of <see cref="IEnumerable{T}"/> containing the paginated collection of <see cref="CalendarEntryDto"/>
        /// objects.</returns>
        public async Task<IBaseResult<IEnumerable<CalendarEntryDto>>> PagedSchoolEventsForAppCalendarAsync(SchoolEventPageParameters pageParameters, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<CalendarEntryDto>>($"schoolevents/all/app/calendar?{pageParameters.GetQueryString()}");
            return result;
        }
        
        /// <summary>
        /// Retrieves a collection of ticket types associated with a specific school event.
        /// </summary>
        /// <remarks>This method fetches ticket type information for a specific school event by its
        /// identifier. Ensure that the provided <paramref name="eventTicketTypeId"/> corresponds to a valid
        /// event.</remarks>
        /// <param name="eventTicketTypeId">The unique identifier of the event ticket type to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  IBaseResult{T} of
        /// IEnumerable{T} containing  SchoolEventTicketTypeDto objects representing the ticket types.</returns>
        public async Task<IBaseResult<IEnumerable<SchoolEventTicketTypeDto>>> SchoolEventTicketTypes(string eventTicketTypeId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<SchoolEventTicketTypeDto>>($"schoolevents/ticketstypes/{eventTicketTypeId}");
            return result;
        }

        /// <summary>
        /// Retrieves the details of a specific ticket type based on its unique identifier.
        /// </summary>
        /// <remarks>Use this method to retrieve detailed information about a specific ticket type for a
        /// school event. Ensure that the <paramref name="ticketTypeId"/> is valid and corresponds to an existing ticket
        /// type.</remarks>
        /// <param name="ticketTypeId">The unique identifier of the ticket type to retrieve. This value cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping the <see cref="SchoolEventTicketTypeDto"/> for the specified ticket type.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IBaseResult<SchoolEventTicketTypeDto>> TicketType(string ticketTypeId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<SchoolEventTicketTypeDto>($"schoolevents/ticketstypes/type/{ticketTypeId}");
            return result;
        }

        /// <summary>
        /// Retrieves the total count of school events.
        /// </summary>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Passing a canceled token will result in the task being
        /// canceled.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// where <c>T</c> is an <see cref="int"/> representing the total number of school events.</returns>
        public async Task<IBaseResult<int>> EventCount(CancellationToken cancellationToken)
        {
            var result = await provider.GetAsync<int>($"schoolevents/count");
            return result;
        }

        /// <summary>
        /// Retrieves the number of views for a specific event by a specific user.
        /// </summary>
        /// <remarks>This method asynchronously retrieves the view count for a specific event and user
        /// from the underlying data provider.</remarks>
        /// <param name="eventId">The unique identifier of the event whose views are being retrieved.</param>
        /// <param name="userId">The unique identifier of the user associated with the event views.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>An <see cref="IBaseResult{T}"/> containing the number of views for the specified event and user.</returns>
        public async Task<IBaseResult<int>> EventViews(string eventId, string userId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<int>($"schoolevents/views/{eventId}/{userId}");
            return result;
        }

        /// <summary>
        /// Retrieves a school event by its unique identifier.
        /// </summary>
        /// <remarks>This method retrieves the details of a school event from the data provider. The
        /// caller can optionally specify whether to track changes to the retrieved entity.</remarks>
        /// <param name="schoolEventId">The unique identifier of the school event to retrieve. This value cannot be null or empty.</param>
        /// <param name="trackChanges">A boolean value indicating whether to track changes to the retrieved entity. Defaults to <see
        /// langword="false"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// of type <see cref="SchoolEventDto"/> representing the school event.</returns>
        public async Task<IBaseResult<SchoolEventDto>> SchoolEventAsync(string schoolEventId, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<SchoolEventDto>($"schoolevents/{schoolEventId}");
            return result;
        }
    }
}
