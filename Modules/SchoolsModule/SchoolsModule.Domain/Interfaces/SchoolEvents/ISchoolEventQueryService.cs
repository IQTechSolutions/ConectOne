using CalendarModule.Domain.DataTransferObjects;
using ConectOne.Domain.ResultWrappers;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Domain.Interfaces.SchoolEvents
{
    /// <summary>
    /// Provides querying capabilities for school events, including pagination, filtering,
    /// calendar views, view tracking, and detailed event retrieval.
    /// </summary>
    public interface ISchoolEventQueryService
    {
        /// <summary>
        /// Retrieves paginated school events for mobile or web app usage,
        /// filtered based on the current user's role (e.g., parent or teacher).
        /// </summary>
        Task<PaginatedResult<SchoolEventDto>> PagedSchoolEventsForAppAsync(SchoolEventPageParameters pageParameters, bool trackChanges = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a paginated list of school events for app ticket sales.
        /// </summary>
        /// <remarks>Use this method to retrieve school events in a paginated format, which is useful for
        /// scenarios  such as displaying events in a user interface with paging support. The <paramref
        /// name="trackChanges"/>  parameter determines whether the retrieved entities are tracked for changes in the
        /// underlying data context.</remarks>
        /// <param name="pageParameters">The parameters specifying the pagination and filtering options for the school events.</param>
        /// <param name="trackChanges">A value indicating whether to track changes to the retrieved entities.  Set to <see langword="true"/> to
        /// enable change tracking; otherwise, <see langword="false"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a collection of <see cref="SchoolEventDto"/> objects  that
        /// match the specified pagination and filtering criteria.</returns>
        Task<PaginatedResult<SchoolEventDto>> PagedSchoolEventsForAppForTicketSalesAsync(SchoolEventPageParameters pageParameters, bool trackChanges = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves paginated school events based on activity status (future/past/today) and category.
        /// </summary>
        Task<PaginatedResult<SchoolEventDto>> PagedSchoolEventsAsync(SchoolEventPageParameters pageParameters, bool trackChanges = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves paginated events with minimal data and no included relationships for performance.
        /// </summary>
        Task<PaginatedResult<SchoolEventDto>> PagedSchoolEventsMinimalAsync(SchoolEventPageParameters pageParameters, bool trackChanges = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves paginated school events relevant to a specific parent (based on their children’s participation).
        /// </summary>
        Task<PaginatedResult<SchoolEventDto>> PagedSchoolEventsForParentAsync(SchoolEventPageParameters pageParameters, bool trackChanges = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves paginated school events where a specific learner is a participant.
        /// </summary>
        Task<PaginatedResult<SchoolEventDto>> PagedSchoolEventsForLearnerAsync(SchoolEventPageParameters pageParameters, bool trackChanges = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a list of school events formatted as calendar entries, filtered by date and user context.
        /// </summary>
        Task<IBaseResult<IEnumerable<CalendarEntryDto>>> PagedSchoolEventsForAppCalendarAsync(SchoolEventPageParameters pageParameters, bool trackChanges = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a collection of ticket types associated with a specific school event.
        /// </summary>
        /// <remarks>The returned collection includes ticket types that match the specified <paramref
        /// name="eventTicketTypeId"/>. If no matching ticket types are found, the collection will be empty.</remarks>
        /// <param name="eventTicketTypeId">The identifier of the event ticket type to filter the results. This value cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// containing an enumerable collection of <see cref="SchoolEventTicketTypeDto"/> objects.</returns>
        Task<IBaseResult<IEnumerable<SchoolEventTicketTypeDto>>> SchoolEventTicketTypes(string eventTicketTypeId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the details of a specific ticket type based on its unique identifier.
        /// </summary>
        /// <remarks>Use this method to fetch detailed information about a specific ticket type, such as
        /// its name, description, or associated event. Ensure that the <paramref name="ticketTypeId"/> provided
        /// corresponds to a valid ticket type in the system.</remarks>
        /// <param name="ticketTypeId">The unique identifier of the ticket type to retrieve. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping the <see cref="SchoolEventTicketTypeDto"/> for the specified ticket type.</returns>
        Task<IBaseResult<SchoolEventTicketTypeDto>> TicketType(string ticketTypeId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the total count of future (upcoming) school events.
        /// </summary>
        Task<IBaseResult<int>> EventCount(CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves the number of times a specific user has viewed a specific event.
        /// </summary>
        Task<IBaseResult<int>> EventViews(string eventId, string userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the detailed information of a specific event, including all related entities.
        /// </summary>
        Task<IBaseResult<SchoolEventDto>> SchoolEventAsync(string schoolEventId, bool trackChanges = false, CancellationToken cancellationToken = default);
    }

}
