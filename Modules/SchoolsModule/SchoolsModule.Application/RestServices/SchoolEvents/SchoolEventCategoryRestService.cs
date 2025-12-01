using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using GroupingModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.SchoolEvents;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Application.RestServices.SchoolEvents
{
    /// <summary>
    /// Provides REST-based operations for managing school event categories and their associated activity groups.
    /// </summary>
    /// <remarks>This service facilitates interaction with school event categories and activity groups through
    /// a REST API.  It includes methods for retrieving paginated lists of categories, fetching participating activity
    /// groups  and their members, and accessing specific categories or groups related to a school event.</remarks>
    /// <param name="provider"></param>
    public class SchoolEventCategoryRestService(IBaseHttpProvider provider) : ISchoolEventCategoryService
    {
        /// <summary>
        /// Retrieves a paginated list of school event activity group categories for the application.
        /// </summary>
        /// <remarks>This method is designed for use in application scenarios where paginated access to
        /// school event activity group categories is required. The <paramref name="trackChanges"/> parameter is useful
        /// for scenarios where entity state tracking is necessary, such as in an Entity Framework context.</remarks>
        /// <param name="pageParameters">The pagination parameters, including page size and page number, to control the paginated results.</param>
        /// <param name="trackChanges">A value indicating whether to track changes to the retrieved entities.  Set to <see langword="true"/> to
        /// enable change tracking; otherwise, <see langword="false"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing the paginated list of <see cref="CategoryDto"/> objects.</returns>
        public async Task<PaginatedResult<CategoryDto>> PagedSchoolEventActivityGroupCategoriesForAppAsync(SchoolEventPageParameters pageParameters, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetPagedAsync<CategoryDto, SchoolEventPageParameters>("schoolevents/pagedcategories/app", pageParameters);
            return result;
        }

        /// <summary>
        /// Retrieves the categories of activity groups participating in the specified event.
        /// </summary>
        /// <remarks>This method sends a request to retrieve the categories of activity groups associated
        /// with the specified event. Ensure that the <paramref name="eventId"/> corresponds to a valid event.</remarks>
        /// <param name="eventId">The unique identifier of the event for which to retrieve the participating activity group categories. Cannot
        /// be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// of <see cref="IEnumerable{T}"/> containing <see cref="CategoryDto"/> objects representing the participating
        /// activity group categories.</returns>
        public async Task<IBaseResult<IEnumerable<CategoryDto>>> ParticipatingActivityGroupCategories(string eventId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<CategoryDto>>($"schoolevents/{eventId}/participatingActivityGroupCategories");
            return result;
        }

        /// <summary>
        /// Retrieves the details of a participating activity group category for a specified event.
        /// </summary>
        /// <remarks>This method fetches the category details from the provider using the specified event
        /// and category identifiers. Ensure that the identifiers are valid and the cancellation token is used
        /// appropriately to handle operation cancellation.</remarks>
        /// <param name="eventId">The unique identifier of the event.</param>
        /// <param name="participatingActivityCategoryId">The unique identifier of the participating activity category.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the details of the participating activity group category as a <see cref="CategoryDto"/>.</returns>
        public async Task<IBaseResult<CategoryDto>> ParticipatingActivityGroupCategory(string eventId, string participatingActivityCategoryId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<CategoryDto>($"activities/categories/{eventId}/participating/{participatingActivityCategoryId}");
            return result;
        }

        /// <summary>
        /// Retrieves the activity groups participating in the specified event.
        /// </summary>
        /// <remarks>This method sends a request to retrieve the activity groups associated with the
        /// specified event. The result includes details about each activity group in the form of <see
        /// cref="ActivityGroupDto"/> objects.</remarks>
        /// <param name="eventId">The unique identifier of the event for which to retrieve participating activity groups.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// containing a collection of <see cref="ActivityGroupDto"/> objects representing the participating activity
        /// groups.</returns>
        public async Task<IBaseResult<IEnumerable<ActivityGroupDto>>> ParticipatingActivityGroups(string eventId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<ActivityGroupDto>>($"schoolevents/{eventId}/participatingActivityGroups");
            return result;
        }

        /// <summary>
        /// Retrieves a list of learners who are participating in the specified activity group within a school event.
        /// </summary>
        /// <remarks>This method communicates with the underlying data provider to fetch the list of
        /// learners. Ensure that the provided identifiers are valid and correspond to existing entities.</remarks>
        /// <param name="eventId">The unique identifier of the school event.</param>
        /// <param name="activityGroupId">The unique identifier of the activity group within the event.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with a list of <see cref="LearnerDto"/> representing the participating learners.</returns>
        public async Task<IBaseResult<List<LearnerDto>>> ParticipatingActivityGroupTeamMembers(string eventId, string activityGroupId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<List<LearnerDto>>($"schoolevents/activities/{eventId}/activityGroup/{activityGroupId}");
            return result;
        }
    }
}
