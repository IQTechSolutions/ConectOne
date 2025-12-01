using ConectOne.Domain.Extensions;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.SchoolEvents;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Application.RestServices.SchoolEvents
{
    /// <summary>
    /// Provides methods for managing school event permissions, including retrieving, granting, and revoking permissions
    /// for team members associated with school events.
    /// </summary>
    /// <remarks>This service acts as a REST client for interacting with the underlying data provider to
    /// manage permissions related to school events. It includes operations for retrieving parent permissions, granting
    /// or revoking team member permissions, and querying permissions based on specific request arguments.</remarks>
    /// <param name="provider"></param>
    public class SchoolEventPermissionRestService(IBaseHttpProvider provider) : ISchoolEventPermissionService
    {
        /// <summary>
        /// Retrieves all parent permissions associated with the specified participating activity group.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch parent permissions from the
        /// underlying data provider. Ensure that the <paramref name="participatingActivityGroupId"/> is valid and
        /// corresponds to an existing activity group.</remarks>
        /// <param name="participatingActivityGroupId">The unique identifier of the participating activity group for which to retrieve parent permissions.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// containing an enumerable collection of <see cref="ParentPermissionDto"/> objects representing the parent
        /// permissions.</returns>
        public async Task<IBaseResult<IEnumerable<ParentPermissionDto>>> GetAllParentPermissions(string participatingActivityGroupId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<ParentPermissionDto>>($"schoolevents/teamMembers/permissions/count/{participatingActivityGroupId}");
            return result;
        }

        /// <summary>
        /// Sends a request to grant consent for team member permissions.
        /// </summary>
        /// <remarks>This method sends a POST request to the endpoint
        /// "schoolevents/teamMembers/permissions" with the specified parameters. Ensure that the provided parameters
        /// are valid and meet the requirements of the API.</remarks>
        /// <param name="parameters">The parameters specifying the team member permissions to be granted. This cannot be <see langword="null"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response string indicating
        /// the outcome of the consent operation.</returns>
        public async Task<IBaseResult<string>> GiveConsent(TeamMemberPermissionsParams parameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<string, TeamMemberPermissionsParams>($"schoolevents/teamMembers/permissions", parameters);
            return result;
        }

        /// <summary>
        /// Revokes the consent of a team member for specific permissions.
        /// </summary>
        /// <remarks>This method sends a request to revoke the specified permissions for the given team
        /// member. Ensure that the  <paramref name="parameters"/> object contains valid data before calling this
        /// method.</remarks>
        /// <param name="parameters">The parameters specifying the team member and the permissions to be revoked. This object must not be <see
        /// langword="null"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RetractConsent(TeamMemberPermissionsParams parameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<string, TeamMemberPermissionsParams>($"schoolevents/teamMembers/permissions/remove", parameters);
            return result;
        }

        /// <summary>
        /// Retrieves a list of school event permissions based on the specified request arguments.
        /// </summary>
        /// <remarks>The method sends a request to the underlying provider to retrieve the permissions
        /// list.  Ensure that the <paramref name="args"/> parameter is properly configured to generate the  desired
        /// query string.</remarks>
        /// <param name="args">The request arguments used to filter and customize the permissions list.</param>
        /// <param name="trackChanges">A value indicating whether to track changes to the retrieved data.  Defaults to <see langword="false"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of  <see
        /// cref="SchoolEventPermissionsDto"/> objects representing the permissions for school events.</returns>
        public async Task<IBaseResult<List<SchoolEventPermissionsDto>>> SchoolEventPermissionsListAsync(SchoolEventPermissionsRequestArgs args, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<List<SchoolEventPermissionsDto>>($"schoolevents/teamMembers/permissions/list?{args.GetQueryString()}");
            return result;
        }

        /// <summary>
        /// Retrieves the permissions for a school event based on the specified request arguments.
        /// </summary>
        /// <remarks>This method sends a request to retrieve permissions for team members associated with
        /// a school event. The <paramref name="args"/> parameter must be provided and should include the necessary
        /// query parameters.</remarks>
        /// <param name="args">The request arguments containing the parameters used to filter and retrieve the school event permissions.</param>
        /// <param name="trackChanges">A boolean value indicating whether to track changes to the retrieved data. The default value is <see
        /// langword="false"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the permissions data for the school event.</returns>
        public async Task<IBaseResult<SchoolEventPermissionsDto>> SchoolEventPermissionsAsync(SchoolEventPermissionsRequestArgs args, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<SchoolEventPermissionsDto>($"teamMembers/permissions?{args.GetQueryString()}");
            return result;
        }
    }
}
