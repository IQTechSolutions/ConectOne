using ConectOne.Domain.ResultWrappers;
using SchoolsModule.Domain.DataTransferObjects;

namespace SchoolsModule.Domain.Interfaces.Parents
{
    /// <summary>
    /// Defines the contract for commands related to Parent entities,
    /// including creation, updates, deletion, relationship management,
    /// chat group creation, and data export.
    /// </summary>
    public interface IParentCommandService
    {
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
