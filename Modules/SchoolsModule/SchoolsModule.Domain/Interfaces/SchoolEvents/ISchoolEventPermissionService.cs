using ConectOne.Domain.ResultWrappers;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Domain.Interfaces.SchoolEvents
{
    /// <summary>
    /// Defines operations for managing parent consents for school events,
    /// including granting, retracting, and retrieving consent statuses.
    /// </summary>
    public interface ISchoolEventPermissionService
    {
        /// <summary>
        /// Retrieves all parent permissions based on the specified request parameters.
        /// </summary>
        /// <remarks>Use this method to retrieve parent permissions based on specific criteria provided in
        /// the request parameters. The result includes a collection of permissions that match the criteria.</remarks>
        /// <param name="parameters">The request parameters used to filter and retrieve the parent permissions. This must not be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// with an enumerable collection of <see cref="ParentPermissionDto"/> objects representing the parent
        /// permissions.</returns>
        Task<IBaseResult<IEnumerable<ParentPermissionDto>>> GetAllParentPermissions(string participatingActivityGroupId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Grants a parent’s consent for a specific learner, event, and consent type.
        /// </summary>
        /// <param name="parameters">Details identifying the parent, learner, event, and consent type.</param>
        /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
        /// <returns>A result indicating success or failure of the operation.</returns>
        Task<IBaseResult<string>> GiveConsent(TeamMemberPermissionsParams parameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retracts a previously granted parent consent for a learner and event.
        /// </summary>
        /// <param name="parameters">Details identifying the parent, learner, event, and consent type.</param>
        /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
        /// <returns>A result indicating success or failure of the operation.</returns>
        Task<IBaseResult> RetractConsent(TeamMemberPermissionsParams parameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a list of consents for all learners linked to a parent for a specific event.
        /// </summary>
        /// <param name="args">The parent email and event ID used to filter the data.</param>
        /// <param name="trackChanges">Whether to track changes in the EF Core context.</param>
        /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
        /// <returns>A list of learner-specific consent statuses.</returns>
        Task<IBaseResult<List<SchoolEventPermissionsDto>>> SchoolEventPermissionsListAsync(SchoolEventPermissionsRequestArgs args, bool trackChanges = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the consent status for a specific learner for a specific event.
        /// </summary>
        /// <param name="args">The learner ID and event ID.</param>
        /// <param name="trackChanges">Whether to track changes in the EF Core context.</param>
        /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
        /// <returns>The learner's consent details for the event.</returns>
        Task<IBaseResult<SchoolEventPermissionsDto>> SchoolEventPermissionsAsync(SchoolEventPermissionsRequestArgs args, bool trackChanges = false, CancellationToken cancellationToken = default);
    }

}
