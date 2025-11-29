using ConectOne.Domain.ResultWrappers;
using IdentityModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Domain.Interfaces
{
    /// <summary>
    /// Interface defining operations for managing and retrieving <see cref="SchoolGrade"/> entities, 
    /// including support for notifications and CRUD functionality.
    /// </summary>
    public interface ISchoolGradeService
    {
        /// <summary>
        /// Retrieves all school grades from the data source.
        /// </summary>
        /// <param name="cancellationToken">A token for cancelling the asynchronous operation.</param>
        /// <returns>A result containing the list of school grades or error messages.</returns>
        Task<IBaseResult<IEnumerable<SchoolGradeDto>>> AllSchoolGradesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a paginated list of school grades.
        /// </summary>
        /// <param name="pageParameters">Paging parameters including page number and size.</param>
        /// <param name="cancellationToken">A token for cancelling the asynchronous operation.</param>
        /// <returns>A paginated result of school grade DTOs.</returns>
        Task<PaginatedResult<SchoolGradeDto>> PagedSchoolGradesAsync(SchoolGradePageParameters pageParameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a single school grade by its unique identifier.
        /// </summary>
        /// <param name="schoolGradeId">The ID of the school grade to retrieve.</param>
        /// <param name="trackChanges">Indicates whether the entity should be tracked by the DbContext.</param>
        /// <param name="cancellationToken">A token for cancelling the asynchronous operation.</param>
        /// <returns>A result containing the school grade DTO or error messages.</returns>
        Task<IBaseResult<SchoolGradeDto>> SchoolGradeAsync(string schoolGradeId, bool trackChanges = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new school grade.
        /// </summary>
        /// <param name="schoolGrade">The DTO containing data for the new school grade.</param>
        /// <param name="cancellationToken">A token for cancelling the asynchronous operation.</param>
        /// <returns>A result indicating success or failure of the creation operation.</returns>
        Task<IBaseResult> CreateAsync(SchoolGradeDto schoolGrade, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing school grade.
        /// </summary>
        /// <param name="schoolGrade">The DTO containing updated data for the school grade.</param>
        /// <param name="cancellationToken">A token for cancelling the asynchronous operation.</param>
        /// <returns>A result indicating success or failure of the update operation.</returns>
        Task<IBaseResult> UpdateAsync(SchoolGradeDto schoolGrade, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a school grade identified by its unique ID.
        /// </summary>
        /// <param name="schoolGradeId">The ID of the school grade to delete.</param>
        /// <param name="cancellationToken">A token for cancelling the asynchronous operation.</param>
        /// <returns>A result indicating success or failure of the deletion.</returns>
        Task<IBaseResult> DeleteAsync(string schoolGradeId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a list of notification recipients related to a school grade, including learners, parents, and teachers.
        /// </summary>
        /// <param name="learnerPageParameters">Parameters for filtering by age and grade.</param>
        /// <param name="cancellationToken">A token for cancelling the asynchronous operation.</param>
        /// <returns>A result containing the list of recipients or error messages.</returns>
        Task<IBaseResult<IEnumerable<RecipientDto>>> SchoolGradeNotificationList(LearnerPageParameters learnerPageParameters, CancellationToken cancellationToken = default);
    }
}
