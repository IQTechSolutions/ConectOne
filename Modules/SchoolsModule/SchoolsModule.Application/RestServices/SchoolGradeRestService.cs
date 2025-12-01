using ConectOne.Domain.Extensions;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using IdentityModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Application.RestServices
{
    /// <summary>
    /// Provides methods for managing school grade data through RESTful API calls.
    /// </summary>
    /// <remarks>This service interacts with a REST API to perform operations such as retrieving, creating,
    /// updating,  and deleting school grade records. It also supports paginated queries and fetching notification lists
    /// related to school grades.</remarks>
    /// <param name="provider"></param>
    public class SchoolGradeRestService(IBaseHttpProvider provider) : ISchoolGradeService
    {
        /// <summary>
        /// Retrieves all school grades asynchronously.
        /// </summary>
        /// <remarks>This method fetches all available school grades from the underlying data source. The
        /// operation can be canceled by passing a cancellation token.</remarks>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// containing an enumerable collection of <see cref="SchoolGradeDto"/> objects.</returns>
        public async Task<IBaseResult<IEnumerable<SchoolGradeDto>>> AllSchoolGradesAsync(CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<SchoolGradeDto>>("schoolGrades/all");
            return result;
        }

        /// <summary>
        /// Retrieves a paginated list of school grades based on the specified page parameters.
        /// </summary>
        /// <remarks>This method queries the data source for school grades and returns the results in a
        /// paginated format. The <paramref name="pageParameters"/> parameter allows the caller to specify the page
        /// size, page number, and any additional filtering criteria.</remarks>
        /// <param name="pageParameters">The parameters that define the pagination and filtering options for the school grades.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing the paginated list of <see cref="SchoolGradeDto"/> objects.</returns>
        public async Task<PaginatedResult<SchoolGradeDto>> PagedSchoolGradesAsync(SchoolGradePageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetPagedAsync<SchoolGradeDto, SchoolGradePageParameters>("schoolGrades/pagedschoolGrades", pageParameters);
            return result;
        }

        /// <summary>
        /// Retrieves the details of a school grade by its unique identifier.
        /// </summary>
        /// <remarks>This method retrieves the school grade details from the underlying data provider. The
        /// <paramref name="trackChanges"/> parameter determines whether the retrieved entity is tracked for
        /// changes.</remarks>
        /// <param name="schoolGradeId">The unique identifier of the school grade to retrieve. This value cannot be <see langword="null"/> or empty.</param>
        /// <param name="trackChanges">A value indicating whether to track changes to the retrieved entity. The default is <see langword="false"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that wraps the <see cref="SchoolGradeDto"/> for the specified school grade.</returns>
        public async Task<IBaseResult<SchoolGradeDto>> SchoolGradeAsync(string schoolGradeId, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<SchoolGradeDto>($"schoolGrades/{schoolGradeId}");
            return result;
        }

        /// <summary>
        /// Creates or updates a school grade asynchronously.
        /// </summary>
        /// <remarks>This method sends the provided <paramref name="schoolGrade"/> to the underlying data
        /// provider for creation  or update. Ensure that the <paramref name="schoolGrade"/> object contains valid data
        /// before calling this method.</remarks>
        /// <param name="schoolGrade">The data transfer object representing the school grade to be created or updated.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> CreateAsync(SchoolGradeDto schoolGrade, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync($"schoolGrades", schoolGrade);
            return result;
        }

        /// <summary>
        /// Updates the school grade information asynchronously.
        /// </summary>
        /// <remarks>This method sends the provided <paramref name="schoolGrade"/> to the underlying
        /// provider for updating. Ensure that the <paramref name="schoolGrade"/> contains valid data before calling
        /// this method.</remarks>
        /// <param name="schoolGrade">The data transfer object containing the school grade information to be updated.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateAsync(SchoolGradeDto schoolGrade, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"schoolGrades", schoolGrade);
            return result;
        }

        /// <summary>
        /// Deletes a school grade with the specified identifier asynchronously.
        /// </summary>
        /// <remarks>This method sends a delete request to the underlying provider to remove the specified
        /// school grade. Ensure that the <paramref name="schoolGradeId"/> corresponds to an existing school
        /// grade.</remarks>
        /// <param name="schoolGradeId">The unique identifier of the school grade to delete.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the delete operation.</returns>
        public async Task<IBaseResult> DeleteAsync(string schoolGradeId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"schoolGrades", schoolGradeId);
            return result;
        }

        /// <summary>
        /// Retrieves a paginated list of recipients for school grade notifications.
        /// </summary>
        /// <remarks>The method sends a request to retrieve recipients based on the specified pagination
        /// and filtering criteria. Ensure that <paramref name="learnerPageParameters"/> is properly configured to avoid
        /// unexpected results.</remarks>
        /// <param name="learnerPageParameters">The pagination and filtering parameters to apply when retrieving the list of recipients.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// containing an enumerable collection of <see cref="RecipientDto"/> objects representing the recipients.</returns>
        public async Task<IBaseResult<IEnumerable<RecipientDto>>> SchoolGradeNotificationList(LearnerPageParameters learnerPageParameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<RecipientDto>>($"schoolGrades/notificationList?{learnerPageParameters.GetQueryString()}");
            return result;
        }
    }
}
