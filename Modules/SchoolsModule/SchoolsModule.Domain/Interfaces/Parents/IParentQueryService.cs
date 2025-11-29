using ConectOne.Domain.ResultWrappers;
using IdentityModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Domain.Interfaces.Parents
{
    /// <summary>
    /// Defines query operations for retrieving parent-related data from the SchoolsEnterprise application.
    /// </summary>
    /// <remarks>
    /// This interface forms part of the read-only service layer in line with CQRS principles, and should only
    /// expose non-mutating methods that query the domain.
    /// </remarks>
    public interface IParentQueryService
    {
        /// <summary>
        /// Retrieves all parents or a specific parent if an identifier is provided.
        /// </summary>
        /// <param name="parentId">
        /// Optional identifier to retrieve a specific parent. If <c>null</c> or empty, all parents are returned.
        /// </param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>
        /// A result containing a collection of <see cref="Parent"/> instances or error messages if the operation fails.
        /// </returns>
        Task<IBaseResult<IEnumerable<ParentDto>>> AllParentsAsync(string? parentId = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a paginated list of parents based on the provided paging parameters.
        /// </summary>
        /// <param name="pageParameters">The pagination and filtering parameters to use in the query.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>
        /// A paginated result of <see cref="ParentDto"/> instances, or error messages if the operation fails.
        /// </returns>
        Task<PaginatedResult<ParentDto>> PagedParentsAsync(ParentPageParameters pageParameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the total number of parent records in the system.
        /// </summary>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>
        /// A result containing the count of <see cref="Parent"/> records or error messages if the operation fails.
        /// </returns>
        Task<IBaseResult<int>> ParentCount(CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a specific parent by their unique identifier and optionally tracks changes.
        /// </summary>
        /// <param name="parentId">The unique identifier of the parent.</param>
        /// <param name="trackChanges">
        /// Whether the returned entity should be tracked by the underlying data context for change detection.
        /// </param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>
        /// A result containing the mapped <see cref="ParentDto"/> or error messages if the operation fails.
        /// </returns>
        Task<IBaseResult<ParentDto>> ParentAsync(string parentId, bool trackChanges, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a parent using their associated email address.
        /// </summary>
        /// <param name="emailAddress">The email address of the parent to find.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>
        /// A result containing the corresponding <see cref="ParentDto"/> or error messages if the operation fails.
        /// </returns>
        Task<IBaseResult<ParentDto>> ParentByEmailAsync(string emailAddress, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all learners associated with a given parent identifier.
        /// </summary>
        /// <param name="parentId">The identifier of the parent.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>
        /// A result containing a list of <see cref="LearnerDto"/> instances or error messages if the operation fails.
        /// </returns>
        Task<IBaseResult<List<LearnerDto>>> ParentLearnersAsync(string parentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a list of users (in <see cref="UserInfoDto"/>) suitable for receiving notifications.
        /// Optionally filtered via <see cref="ParentPageParameters"/> for narrower queries.
        /// </summary>
        /// <param name="pageParameters">Contains paging or filter info, e.g., a <c>ParentId</c>.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>An enumeration of user info objects for notification distribution.</returns>
        Task<IBaseResult<IEnumerable<RecipientDto>>> ParentsNotificationList(ParentPageParameters pageParameters, CancellationToken cancellationToken = default);
    }
}
