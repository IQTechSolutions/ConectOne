using CalendarModule.Domain.DataTransferObjects;
using CalendarModule.Domain.Enums;
using ConectOne.Domain.Extensions;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using GroupingModule.Domain.Entities;
using MessagingModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.Interfaces.SchoolEvents;
using SchoolsModule.Domain.RequestFeatures;
using SchoolsModule.Domain.Specifications;

namespace SchoolsModule.Infrastructure.Implementation.SchoolEvents
{
    /// <summary>
    /// Provides query services for retrieving and managing school events.
    /// </summary>
    /// <remarks>This service offers a variety of methods to retrieve school events based on different
    /// criteria, such as pagination, user roles (e.g., parent, teacher, learner), and event-specific filters. The
    /// results are typically returned in a paginated format, and additional metadata such as total counts and page
    /// information is included. The service also supports operations like tracking event views and retrieving
    /// calendar-friendly event data.</remarks>
    /// <param name="schoolsModuleRepoManager"></param>
    /// <param name="notificationRepository"></param>
    public class SchoolEventQueryService(ISchoolsModuleRepoManager schoolsModuleRepoManager, IRepository<SchoolEventTicketType, string> schoolEventTicketTypes, IRepository<Notification, string> notificationRepository) : ISchoolEventQueryService
    {
        /// <summary>
        /// Retrieves a paginated list of school events based on the specified parameters.
        /// </summary>
        /// <remarks>The method filters events based on the provided <paramref name="pageParameters"/>. If
        /// a parent ID is specified, events associated with that parent are retrieved. If a teacher ID is specified,
        /// events associated with that teacher are retrieved. If neither is provided, an empty result is returned.  The
        /// returned events are distinct by their IDs and ordered by their start dates.</remarks>
        /// <param name="pageParameters">The parameters for pagination and filtering, including optional identifiers for a parent or teacher.</param>
        /// <param name="trackChanges">Indicates whether to track changes to the retrieved entities. Defaults to <see langword="false"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a list of <see cref="SchoolEventDto"/> objects that match the
        /// specified criteria, along with pagination metadata.</returns>
        public async Task<PaginatedResult<SchoolEventDto>> PagedSchoolEventsForAppAsync(SchoolEventPageParameters pageParameters, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            List<SchoolEvent<Category<ActivityGroup>>> response;

            if (!string.IsNullOrEmpty(pageParameters.ParentId))
                response = await GetEventsForParentAsync(pageParameters, cancellationToken);
            else if (!string.IsNullOrEmpty(pageParameters.TeacherId))
                response = await GetEventsForTeacherAsync(pageParameters, cancellationToken);
            else
                response = new();

            var distinctEvents = response.DistinctBy(e => e.Id).OrderBy(e => e.StartDate).ToList();
            var eventDtos = distinctEvents.Select(SchoolEventDto.Create).ToList();

            return PaginatedResult<SchoolEventDto>.Success(eventDtos, distinctEvents.Count, pageParameters.PageNr, pageParameters.PageSize);
        }

        /// <summary>
        /// Retrieves a paginated list of school events for ticket sales based on the specified parameters.
        /// </summary>
        /// <remarks>This method uses the provided pagination parameters to query school events
        /// specifically for ticket sales. The returned result includes the paginated data, the total count of items,
        /// and the current page information.</remarks>
        /// <param name="pageParameters">The parameters that define the pagination and filtering criteria for the school events.</param>
        /// <param name="trackChanges">A boolean value indicating whether to track changes to the retrieved entities. Defaults to <see
        /// langword="false"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a list of <see cref="SchoolEventDto"/> objects that match the
        /// specified criteria. If no events are found, the result will indicate failure with appropriate messages.</returns>
        public async Task<PaginatedResult<SchoolEventDto>> PagedSchoolEventsForAppForTicketSalesAsync(SchoolEventPageParameters pageParameters, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var spec = new SchoolEventForTicketSalesSpecification(pageParameters);
            var result = await schoolsModuleRepoManager.SchoolEvents.ListAsync(spec, false, cancellationToken);
            if(!result.Succeeded) return PaginatedResult<SchoolEventDto>.Failure(result.Messages);
            var eventDtos = result.Data.Select(SchoolEventDto.Create).ToList();

            return PaginatedResult<SchoolEventDto>.Success(eventDtos, eventDtos.Count, pageParameters.PageNr, pageParameters.PageSize);
        }

        /// <summary>
        /// Retrieves future events, filtered optionally by category and activity flags.
        /// </summary>
        public async Task<PaginatedResult<SchoolEventDto>> PagedSchoolEventsAsync(SchoolEventPageParameters pageParameters, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var spec = new PagedSchoolEventsSpecification(pageParameters);
            spec.AddInclude(c => c.Include(g => g.ParticipatingCategories));
            
            var result = await schoolsModuleRepoManager.SchoolEvents.ListAsync(spec, trackChanges, cancellationToken);
            if (!result.Succeeded) return PaginatedResult<SchoolEventDto>.Failure(result.Messages);

            var response = result.Data.Select(SchoolEventDto.Create).ToList();
            
            response = pageParameters.Archived ? response.OrderByDescending(e => e.StartDate).ToList() : response.OrderBy(e => e.StartDate).ToList();

            return PaginatedResult<SchoolEventDto>.Success(response, response.Count, pageParameters.PageNr, pageParameters.PageSize);
        }

        /// <summary>
        /// Returns events with minimal data and no eager-loading includes.
        /// </summary>
        public async Task<PaginatedResult<SchoolEventDto>> PagedSchoolEventsMinimalAsync(SchoolEventPageParameters pageParameters, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var result = await schoolsModuleRepoManager.SchoolEvents.ListAsync(trackChanges: trackChanges, cancellationToken);
            if (!result.Succeeded) return PaginatedResult<SchoolEventDto>.Failure(result.Messages);

            var filtered = result.Data
                .Select(SchoolEventDto.Create)
                .ToList();

            if (pageParameters.Archived)
            {
                filtered = filtered
                    .Where(e => e.Published && DateTimeExtensions.IsPast(e.StartDate))
                    .OrderByDescending(e => e.StartDate)
                    .ToList();
            }
            else if (pageParameters.Active)
            {
                filtered = filtered
                    .Where(e => e.Published && DateTimeExtensions.IsToday(e.StartDate))
                    .OrderBy(e => e.StartDate)
                    .ToList();
            }
            else 
            {
                filtered = filtered
                    .Where(e => e.Published && DateTimeExtensions.IsToday(e.StartDate) || DateTimeExtensions.IsFuture(e.StartDate))
                    .OrderBy(e => e.StartDate)
                    .ToList();
            }

            return PaginatedResult<SchoolEventDto>.Success(filtered, filtered.Count, pageParameters.PageNr, pageParameters.PageSize);
        }

        /// <summary>
        /// Gets paged school events that a parent's children are participating in.
        /// </summary>
        public async Task<PaginatedResult<SchoolEventDto>> PagedSchoolEventsForParentAsync(SchoolEventPageParameters pageParameters, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(pageParameters.ParentId))
                return PaginatedResult<SchoolEventDto>.Failure(["Parent Id is required"]);

            var learnerResult = await schoolsModuleRepoManager.LearnerParents.ListAsync(new LearnerParentsForParentSpecification(pageParameters.ParentId), trackChanges, cancellationToken);
            if (!learnerResult.Succeeded) return PaginatedResult<SchoolEventDto>.Failure(learnerResult.Messages);

            var learnerIds = learnerResult.Data.Select(lp => lp.LearnerId).ToHashSet();
            var eventResult = await schoolsModuleRepoManager.SchoolEvents.ListAsync(new RelevantSchoolEventsSpecification(pageParameters.Archived), trackChanges, cancellationToken);
            if (!eventResult.Succeeded) return PaginatedResult<SchoolEventDto>.Failure(eventResult.Messages);

            var filtered = eventResult.Data
                .Where(e => e.ParticipatingActivityGroups.Any(g => g.ParticipatingTeamMembers.Any(m => learnerIds.Contains(m.TeamMemberId))))
                .Where(e => string.IsNullOrEmpty(pageParameters.CategoryId) || e.ParticipatingCategories.Any(c => c.ActivityGroupCategoryId == pageParameters.CategoryId))
                .OrderBy(e => e.CreatedOn)
                .Select(SchoolEventDto.Create)
                .ToList();

            return PaginatedResult<SchoolEventDto>.Success(filtered, filtered.Count, pageParameters.PageNr, pageParameters.PageSize);
        }

        /// <summary>
        /// Gets events that a specific learner is participating in.
        /// </summary>
        public async Task<PaginatedResult<SchoolEventDto>> PagedSchoolEventsForLearnerAsync(SchoolEventPageParameters pageParameters, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(pageParameters.LearnerId))
                return PaginatedResult<SchoolEventDto>.Failure(["Learner Id is required"]);

            var result = await schoolsModuleRepoManager.SchoolEvents.ListAsync(new UpcomingSchoolEventsWithIncludesSpecification(), trackChanges, cancellationToken);
            if (!result.Succeeded) return PaginatedResult<SchoolEventDto>.Failure(result.Messages);

            var events = result.Data
                .Where(e => e.ParticipatingActivityGroups.Any(g => g.ParticipatingTeamMembers.Any(m => m.TeamMemberId == pageParameters.LearnerId)))
                .Where(e => string.IsNullOrEmpty(pageParameters.CategoryId) || e.ParticipatingCategories.Any(c => c.ActivityGroupCategoryId == pageParameters.CategoryId))
                .OrderBy(e => e.CreatedOn)
                .Select(SchoolEventDto.Create)
                .ToList();

            return PaginatedResult<SchoolEventDto>.Success(events, events.Count, pageParameters.PageNr, pageParameters.PageSize);
        }

        /// <summary>
        /// Provides a calendar-friendly view of events for the app user based on learner or parent participation.
        /// </summary>
        public async Task<IBaseResult<IEnumerable<CalendarEntryDto>>> PagedSchoolEventsForAppCalendarAsync(SchoolEventPageParameters pageParameters, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var spec = new SchoolEventsWithCalendarIncludesSpecification();

            if (pageParameters.StartDate is not null)
            {
                DateTime.TryParse(pageParameters.StartDate, out var parsedStartDate);
                spec = new SchoolEventsWithCalendarIncludesSpecification(parsedStartDate);
            }
            
            var eventsResult = await schoolsModuleRepoManager.SchoolEvents.ListAsync(spec, trackChanges, cancellationToken);
            if (!eventsResult.Succeeded) return await Result<IEnumerable<CalendarEntryDto>>.FailAsync(eventsResult.Messages);

            var allEvents = eventsResult.Data;
            var learnerEvents = new List<SchoolEvent<Category<ActivityGroup>>>();

            if (!string.IsNullOrEmpty(pageParameters.LearnerId))
            {
                learnerEvents = allEvents
                    .Where(e => e.ParticipatingActivityGroups.Any(g => g.ParticipatingTeamMembers.Any(m => m.TeamMemberId == pageParameters.LearnerId)))
                    .ToList();
            }

            if (!string.IsNullOrEmpty(pageParameters.ParentId))
            {
                var parentSpec = new LambdaSpec<Parent>(p => p.Id == pageParameters.ParentId);
                var parentResult = await schoolsModuleRepoManager.Parents.FirstOrDefaultAsync(parentSpec, true, cancellationToken);
                if (parentResult.Succeeded && parentResult.Data is not null)
                {
                    var learnerIds = parentResult.Data.Learners.Select(l => l.LearnerId).ToHashSet();
                    learnerEvents.AddRange(
                        allEvents.Where(e => e.ParticipatingActivityGroups.Any(g => g.ParticipatingTeamMembers.Any(m => learnerIds.Contains(m.TeamMemberId))))
                    );
                }
            }

            var googleEvents = allEvents.Where(e => e.Description == "Google Calendar Event");
            var combined = learnerEvents.Concat(googleEvents).DistinctBy(e => e.Id).OrderBy(e => e.StartDate).ToList();

            var dtoList = combined.Select(e => new CalendarEntryDto(e.Id, e.Heading, e.StartDate, e.StartTime, e.EndDate, e.EndTime, CalendarEntryType.Event, "", false)).ToList();

            return await Result<IEnumerable<CalendarEntryDto>>.SuccessAsync(dtoList);
        }
        
        /// <summary>
        /// Retrieves a collection of ticket types associated with a specific school event.
        /// </summary>
        /// <remarks>Use this method to retrieve ticket type details for a specific school event. The
        /// result includes  information such as ticket type names, prices, and availability.</remarks>
        /// <param name="eventTicketTypeId">The unique identifier of the event ticket type. This parameter cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  IBaseResult{T} object, where
        /// T is an IEnumerable{T} of  SchoolEventTicketTypeDto objects representing the ticket types for the specified
        /// event.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IBaseResult<IEnumerable<SchoolEventTicketTypeDto>>> SchoolEventTicketTypes(string eventTicketTypeId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<SchoolEventTicketType>(c => c.EventId == eventTicketTypeId);

            var result = await schoolEventTicketTypes.ListAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<IEnumerable<SchoolEventTicketTypeDto>>.FailAsync(result.Messages);
            var dtoList = result.Data.Select(t => new SchoolEventTicketTypeDto(t)).ToList();
            return await Result<IEnumerable<SchoolEventTicketTypeDto>>.SuccessAsync(dtoList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticketTypeId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IBaseResult<SchoolEventTicketTypeDto>> TicketType(string ticketTypeId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<SchoolEventTicketType>(c => c.Id == ticketTypeId);

            var result = await schoolEventTicketTypes.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<SchoolEventTicketTypeDto>.FailAsync(result.Messages);

            if(result.Data == null) return await Result<SchoolEventTicketTypeDto>.FailAsync($"No ticket type found with ID '{ticketTypeId}'.");
            
            var dto = new SchoolEventTicketTypeDto(result.Data);
            return await Result<SchoolEventTicketTypeDto>.SuccessAsync(dto);
        }

        /// <summary>
        /// Tracks how many times a user has viewed a specific event.
        /// </summary>
        public async Task<IBaseResult<int>> EventViews(string eventId, string userId, CancellationToken cancellationToken = default)
        {
            var result = await schoolsModuleRepoManager.SchoolEventViews.ListAsync(new LambdaSpec<SchoolEventViews>(c => c.Id == eventId && c.UserId == userId), false, cancellationToken);
            if (!result.Succeeded) return await Result<int>.FailAsync(result.Messages);
            return await Result<int>.SuccessAsync(result.Data.Count());
        }

        /// <summary>
        /// Retrieves the count of upcoming events.
        /// </summary>
        public async Task<IBaseResult<int>> EventCount(CancellationToken cancellationToken)
        {
            var result = await schoolsModuleRepoManager.SchoolEvents.CountAsync(new LambdaSpec<SchoolEvent<Category<ActivityGroup>>>(c => c.StartDate.Date >= DateTime.Now.Date), cancellationToken);
            if (!result.Succeeded) return await Result<int>.FailAsync(result.Messages);
            return await Result<int>.SuccessAsync(result.Data);
        }

        /// <summary>
        /// Retrieves a full representation of a school event and marks related notifications as read.
        /// </summary>
        public async Task<IBaseResult<SchoolEventDto>> SchoolEventAsync(string schoolEventId, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(schoolEventId))
                    return await Result<SchoolEventDto>.FailAsync("SchoolEventId is required.");

                var eventResult = await schoolsModuleRepoManager.SchoolEvents.FirstOrDefaultAsync(new SingleEventSpecification(schoolEventId), trackChanges, cancellationToken);
                if (eventResult.Data == null)
                    return await Result<SchoolEventDto>.FailAsync($"No event found with ID '{schoolEventId}'.");

                var dto = SchoolEventDto.Create(eventResult.Data);

                return await Result<SchoolEventDto>.SuccessAsync(dto);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        #region Helper Methods

        /// <summary>
        /// Retrieves a list of upcoming published school events relevant to a specific parent,
        /// identified by their email address, filtered optionally by category.
        /// </summary>
        /// <param name="pageParameters">The parameters containing the parent's email and optional category filter.</param>
        /// <param name="cancellationToken">Optional token to cancel the asynchronous operation.</param>
        /// <returns>A list of matching <see cref="SchoolEvent{Category}"/> instances.</returns>
        private async Task<List<SchoolEvent<Category<ActivityGroup>>>> GetEventsForParentAsync(SchoolEventPageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var response = new List<SchoolEvent<Category<ActivityGroup>>>();
            var parentResult = await schoolsModuleRepoManager.Parents.ListAsync(new ParentByEmailWithLearnersSpec(pageParameters.UserEmailAddress), false, cancellationToken);
            var parent = parentResult.Data.FirstOrDefault();
            if (parent is null) return response;

            foreach (var learner in parent.Learners)
            {
                var groupsResult = await schoolsModuleRepoManager.ParticipatingActivityGroups.ListAsync(new ParticipatingGroupsByLearnerIdSpec(learner.LearnerId!), false, cancellationToken);
                var events = groupsResult.Data
                    .Where(g => g.Event != null && g.Event.Published && g.Event.StartDate >= DateTime.Now &&
                                (string.IsNullOrEmpty(pageParameters.CategoryId) || g.Event.ParticipatingCategories.Any(c => c.ActivityGroupCategoryId == pageParameters.CategoryId)))
                    .Select(g => g.Event!)
                    .ToList();

                response.AddRange(events);
            }

            return response;
        }

        /// <summary>
        /// Retrieves a list of upcoming published school events relevant to a specific teacher,
        /// identified by their email address, filtered optionally by category.
        /// </summary>
        /// <param name="pageParameters">The parameters containing the teacher's email and optional category filter.</param>
        /// <param name="cancellationToken">Optional token to cancel the asynchronous operation.</param>
        /// <returns>A list of matching <see cref="SchoolEvent{Category}"/> instances.</returns>
        private async Task<List<SchoolEvent<Category<ActivityGroup>>>> GetEventsForTeacherAsync(SchoolEventPageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var response = new List<SchoolEvent<Category<ActivityGroup>>>();
            var groupsResult = await schoolsModuleRepoManager.ParticipatingActivityGroups.ListAsync(new ParticipatingGroupsByTeacherEmailSpec(pageParameters.UserEmailAddress), false, cancellationToken);
            var events = groupsResult.Data
                .Where(g => g.Event != null && g.Event.Published && g.Event.StartDate >= DateTime.Now &&
                            (string.IsNullOrEmpty(pageParameters.CategoryId) || g.Event.ParticipatingCategories.Any(c => c.ActivityGroupCategoryId == pageParameters.CategoryId)))
                .Select(g => g.Event!)
                .ToList();

            response.AddRange(events);
            return response;
        }

        #endregion
    }
}
