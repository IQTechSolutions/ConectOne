using ConectOne.Domain.ResultWrappers;
using ConectOne.Infrastructure.Interfaces;
using IdentityModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Domain.Interfaces
{
    /// <summary>
    /// Defines query operations for retrieving parent-related data from the SchoolsEnterprise application.
    /// </summary>
    /// <remarks>
    /// This interface forms part of the read-only service layer in line with CQRS principles, and should only
    /// expose non-mutating methods that query the domain.
    /// </remarks>
    public interface IParentService : IContactInfoService<Parent>
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

        /// <summary>
        /// Creates a new Parent entity from a DTO and saves it to the database.
        /// </summary>
        /// <param name="parent">DTO containing parent data.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>
        /// A success result with the created <see cref="ParentDto"/> 
        /// or a failure result with error messages.
        /// </returns>
        Task<IBaseResult<ParentDto>> CreateAsync(ParentDto parent, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing Parent entity with new data.
        /// </summary>
        /// <param name="parent">DTO containing updated parent data.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A result indicating success or failure of the update.</returns>
        Task<IBaseResult> UpdateAsync(ParentDto parent, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates only the profile details of an existing Parent entity.
        /// </summary>
        /// <param name="parent">DTO containing updated profile data.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A result indicating the success or failure of the profile update.</returns>
        Task<IBaseResult> UpdateProfileAsync(ParentDto parent, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a parent and all associated relationships, such as learners and consents.
        /// </summary>
        /// <param name="parentId">The ID of the parent to be removed.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A result indicating the success or failure of the deletion.</returns>
        Task<IBaseResult> RemoveAsync(string parentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a chat group for a parent and adds a specified user as a group member.
        /// </summary>
        /// <param name="parentId">The ID of the parent for whom the group is created.</param>
        /// <param name="groupMemberId">The ID of the user to be added to the group.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>
        /// A result containing the group ID if creation succeeds,
        /// or an error message if it fails.
        /// </returns>
        Task<IBaseResult<string>> CreateParentChatGroupAsync(string parentId, string groupMemberId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Establishes a relationship between a parent and a learner.
        /// </summary>
        /// <param name="parentId">ID of the parent.</param>
        /// <param name="learnerId">ID of the learner.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A result indicating success or failure of the relationship creation.</returns>
        Task<IBaseResult> CreateParentLearnerAsync(string parentId, string learnerId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes the relationship between a parent and a learner.
        /// </summary>
        /// <param name="parentId">ID of the parent.</param>
        /// <param name="learnerId">ID of the learner.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A result indicating success or failure of the relationship removal.</returns>
        Task<IBaseResult> RemoveParentLearnerAsync(string parentId, string learnerId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Exports the list of parents along with their contact and learner information to an Excel file.
        /// </summary>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>
        /// A result containing the path or content reference to the exported Excel file,
        /// or an error message if the operation fails.
        /// </returns>
        Task<IBaseResult<string>> ExportParents(CancellationToken cancellationToken = default);
    }
}
