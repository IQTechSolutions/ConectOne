using ConectOne.Domain.ResultWrappers;
using ConectOne.Infrastructure.Interfaces;
using IdentityModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Domain.Interfaces
{
    /// <summary>
    /// Interface for managing Teacher-related operations.
    /// </summary>
    public interface ITeacherService : IContactInfoService<Teacher>
    {
        /// <summary>
        /// Retrieves all teachers asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of Teacher entities.</returns>
        Task<IBaseResult<IEnumerable<TeacherDto>>> AllTeachersAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a paginated list of teachers asynchronously.
        /// </summary>
        /// <param name="pageParameters">The pagination parameters.</param>
        /// <param name="trackChanges">Indicates whether to track changes.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a paginated result of TeacherDto entities.</returns>
        Task<PaginatedResult<TeacherDto>> PagedTeachersAsync(TeacherPageParameters pageParameters, bool trackChanges = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a teacher by ID asynchronously.
        /// </summary>
        /// <param name="teacherId">The ID of the teacher.</param>
        /// <param name="trackChanges">Indicates whether to track changes.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a TeacherDto entity.</returns>
        Task<IBaseResult<TeacherDto>> TeacherAsync(string teacherId, bool trackChanges = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a teacher by email address asynchronously.
        /// </summary>
        /// <param name="emailAddress">The email address of the teacher.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a TeacherDto entity.</returns>
        Task<IBaseResult<TeacherDto>> TeacherByEmailAsync(string emailAddress, CancellationToken cancellationToken = default);

        Task<IBaseResult<string>> TeacherExist(string emailAddress, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new teacher asynchronously.
        /// </summary>
        /// <param name="teacher">The teacher data transfer object.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a TeacherDto entity.</returns>
        Task<IBaseResult<TeacherDto>> CreateAsync(TeacherDto teacher, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing teacher asynchronously.
        /// </summary>
        /// <param name="teacher">The teacher data transfer object.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<IBaseResult> UpdateAsync(TeacherDto teacher, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a teacher by ID asynchronously.
        /// </summary>
        /// <param name="teacherId">The ID of the teacher.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<IBaseResult> RemoveAsync(string teacherId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a list of teachers for notifications asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of UserInfoDto entities.</returns>
        Task<IBaseResult<IEnumerable<RecipientDto>>> TeachersNotificationList(TeacherPageParameters pageParameters, CancellationToken cancellationToken = default);
    }
}
