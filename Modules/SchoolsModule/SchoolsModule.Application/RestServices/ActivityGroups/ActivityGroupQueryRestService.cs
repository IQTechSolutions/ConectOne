using ConectOne.Domain.Extensions;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.ActivityGroups;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Application.RestServices.ActivityGroups
{
    /// <summary>
    /// Provides methods for querying activity groups and their related data through RESTful endpoints.
    /// </summary>
    /// <remarks>This service is designed to interact with activity group-related resources, offering
    /// functionality such as retrieving paginated lists of activity groups, fetching specific activity group details, 
    /// and querying team members associated with activity groups or events. All methods are asynchronous  and support
    /// cancellation tokens for managing long-running operations.</remarks>
    /// <param name="provider"></param>
    public class ActivityGroupQueryRestService(IBaseHttpProvider provider) : IActivityGroupQueryService
    {
        /// <summary>
        /// Retrieves a paginated list of all activity groups.
        /// </summary>
        /// <remarks>The method fetches all activity groups based on the provided pagination parameters.
        /// If no activity groups are available, the result will contain an empty collection.</remarks>
        /// <param name="pageParameters">The pagination parameters, including page number and page size, to control the result set.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// containing an enumerable collection of <see cref="ActivityGroupDto"/> objects.</returns>
        public async Task<IBaseResult<IEnumerable<ActivityGroupDto>>> AllActivityGroupsAsync(ActivityGroupPageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<ActivityGroupDto>>($"activitygroups/all?{pageParameters.GetQueryString()}");
            return result;
        }

        /// <summary>
        /// Retrieves a paginated list of activity groups based on the specified page parameters.
        /// </summary>
        /// <remarks>This method queries the data source for activity groups and returns the results in a
        /// paginated format. The <paramref name="pageParameters"/> parameter allows the caller to specify the page
        /// size, page number,  and any additional filtering criteria.</remarks>
        /// <param name="pageParameters">The parameters that define the pagination and filtering options for the activity groups.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing the paginated list of <see cref="ActivityGroupDto"/> objects.</returns>
        public async Task<PaginatedResult<ActivityGroupDto>> PagedActivityGroupsAsync(ActivityGroupPageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetPagedAsync<ActivityGroupDto, ActivityGroupPageParameters>("activitygroups/pagedactivitygroups", pageParameters);
            return result;
        }

        /// <summary>
        /// Retrieves a paginated list of activity groups for a specific event.
        /// </summary>
        /// <remarks>This method fetches activity groups associated with an event based on the provided
        /// pagination parameters. Use the <paramref name="pageParameters"/> to specify the page size, page number, and
        /// any additional filters.</remarks>
        /// <param name="pageParameters">The parameters that define the pagination and filtering options for the activity groups.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing the paginated list of <see cref="ActivityGroupDto"/> objects.</returns>
        public async Task<PaginatedResult<ActivityGroupDto>> PagedActivityGroupsForEventAsync(ActivityGroupPageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetPagedAsync<ActivityGroupDto, ActivityGroupPageParameters>("activitygroups/pagedactivitygroupsforevent", pageParameters);
            return result;
        }

        /// <summary>
        /// Retrieves a paginated list of activity groups associated with participating event categories.
        /// </summary>
        /// <remarks>This method queries the activity groups that are linked to event categories in which
        /// the user is participating. The pagination and filtering options are determined by the <paramref
        /// name="pageParameters"/> argument.</remarks>
        /// <param name="pageParameters">The parameters specifying the pagination and filtering options for the activity groups.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{ActivityGroupDto}"/> containing the paginated activity groups that match the
        /// specified criteria.</returns>
        public async Task<PaginatedResult<ActivityGroupDto>> PagedActivityGroupsForParticipatingEventCategories(ActivityGroupPageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetPagedAsync<ActivityGroupDto, ActivityGroupPageParameters>("activitygroups/pagedActivityGroupsForParticipatingEventCategories", pageParameters);
            return result;
        }

        /// <summary>
        /// Retrieves the details of an activity group by its unique identifier.
        /// </summary>
        /// <remarks>This method asynchronously retrieves the details of an activity group from the
        /// underlying provider. Ensure that the <paramref name="activityGroupId"/> corresponds to a valid activity
        /// group.</remarks>
        /// <param name="activityGroupId">The unique identifier of the activity group to retrieve. This value cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that wraps the <see cref="ActivityGroupDto"/> representing the activity group details.</returns>
        public async Task<IBaseResult<ActivityGroupDto>> ActivityGroupAsync(string activityGroupId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<ActivityGroupDto>($"activitygroups/{activityGroupId}");
            return result;
        }

        /// <summary>
        /// Retrieves a paginated list of learners associated with an activity group.
        /// </summary>
        /// <remarks>The method fetches learners based on the provided pagination parameters. The result
        /// includes metadata  such as the total count of learners and the current page index.</remarks>
        /// <param name="pageParameters">The parameters specifying the pagination and filtering options for the learners.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing the paginated list of learners.</returns>
        public async Task<PaginatedResult<LearnerDto>> PagedActivityGroupTeamMembersAsync(LearnerPageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetPagedAsync<LearnerDto, LearnerPageParameters>("learners/pagedactivitygrouplearners", pageParameters);
            return result;
        }

        /// <summary>
        /// Retrieves a paginated list of learners associated with an event activity group team.
        /// </summary>
        /// <remarks>This method queries the underlying data source to retrieve learners in a paginated
        /// format.  Ensure that <paramref name="pageParameters"/> specifies valid pagination values to avoid
        /// errors.</remarks>
        /// <param name="pageParameters">The pagination parameters, including page size and page number, used to control the paginated result set.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a collection of <see cref="LearnerDto"/> objects  representing
        /// the learners in the specified event activity group team.</returns>
        public async Task<PaginatedResult<LearnerDto>> PagedEventActivityGroupTeamMembersAsync(LearnerPageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetPagedAsync<LearnerDto, LearnerPageParameters>("learners/pagedeventactivitygrouplearners", pageParameters);
            return result;
        }

        /// <summary>
        /// Retrieves a collection of team members associated with an activity group.
        /// </summary>
        /// <remarks>This method sends a request to the underlying data provider to retrieve the team
        /// members based on the specified query parameters. Ensure that <paramref name="args"/> contains valid
        /// filtering and pagination values.</remarks>
        /// <param name="args">The parameters used to filter and paginate the list of team members.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// containing an enumerable collection of <see cref="LearnerDto"/> objects representing the team members.</returns>
        public async Task<IBaseResult<IEnumerable<LearnerDto>>> ActivityGroupTeamMembersAsync(LearnerPageParameters args, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<LearnerDto>>($"activitygroups/teamMembers?{args.GetQueryString()}");
            return result;
        }
    }
}
