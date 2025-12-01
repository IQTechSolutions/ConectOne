using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Application.RestServices
{
    /// <summary>
    /// Provides methods for managing age group data through RESTful API calls.
    /// </summary>
    /// <remarks>This service interacts with a REST API to perform CRUD operations on age group data.  It
    /// supports retrieving all age groups, paginated results, and individual age group details,  as well as creating,
    /// updating, and deleting age groups.</remarks>
    /// <param name="provider"></param>
    public class AgeGroupRestService(IBaseHttpProvider provider) : IAgeGroupService
    {
        /// <summary>
        /// Retrieves all age groups asynchronously.
        /// </summary>
        /// <remarks>This method fetches all available age groups from the underlying data provider. The
        /// operation  can be canceled by passing a cancellation token.</remarks>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  IBaseResult{T} of
        /// IEnumerable{T} containing AgeGroupDto  objects representing the age groups.</returns>
        public async Task<IBaseResult<IEnumerable<AgeGroupDto>>> AllAgeGroupsAsync(CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<AgeGroupDto>>("agegroups/all");
            return result;
        }

        /// <summary>
        /// Retrieves a paginated list of age groups based on the specified paging parameters.
        /// </summary>
        /// <remarks>This method communicates with an external data provider to fetch the paginated
        /// results. Ensure that the <paramref name="pageParameters"/> are valid to avoid unexpected behavior.</remarks>
        /// <param name="pageParameters">The parameters that define the paging options, such as page number and page size.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a collection of <see cref="AgeGroupDto"/> objects and metadata
        /// about the pagination, such as total count and current page.</returns>
        public async Task<PaginatedResult<AgeGroupDto>> PagedAgeGroupsAsync(AgeGroupPageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetPagedAsync<AgeGroupDto, AgeGroupPageParameters>("agegroups/pagedagegroups", pageParameters);
            return result;
        }

        /// <summary>
        /// Retrieves the details of an age group based on the specified identifier.
        /// </summary>
        /// <remarks>The <paramref name="trackChanges"/> parameter determines whether the retrieved data
        /// is tracked for changes, which may affect performance and memory usage. Use <paramref
        /// name="cancellationToken"/> to cancel the operation if needed.</remarks>
        /// <param name="ageGroupId">The unique identifier of the age group to retrieve. This value cannot be <see langword="null"/> or empty.</param>
        /// <param name="trackChanges">A value indicating whether to track changes to the retrieved data. The default is <see langword="false"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that wraps the <see cref="AgeGroupDto"/> for the specified age group.</returns>
        public async Task<IBaseResult<AgeGroupDto>> AgeGroupAsync(string ageGroupId, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<AgeGroupDto>($"agegroups/{ageGroupId}");
            return result;
        }

        /// <summary>
        /// Creates or updates an age group asynchronously.
        /// </summary>
        /// <remarks>This method sends the provided <paramref name="ageGroup"/> to the underlying provider
        /// for creation or update.  Ensure that the <paramref name="ageGroup"/> contains valid data before calling this
        /// method.</remarks>
        /// <param name="ageGroup">The data transfer object representing the age group to be created or updated.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> CreateAsync(AgeGroupDto ageGroup, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync($"agegroups", ageGroup);
            return result;
        }

        /// <summary>
        /// Updates an existing age group asynchronously.
        /// </summary>
        /// <remarks>This method sends the updated age group data to the underlying provider. Ensure that
        /// the  <paramref name="ageGroup"/> parameter contains valid data before calling this method.</remarks>
        /// <param name="ageGroup">The <see cref="AgeGroupDto"/> object containing the updated age group data.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation. The default value is <see
        /// cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateAsync(AgeGroupDto ageGroup, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync($"agegroups", ageGroup);
            return result;
        }

        /// <summary>
        /// Deletes an age group with the specified identifier.
        /// </summary>
        /// <remarks>This method sends a delete request to the underlying provider to remove the specified
        /// age group. Ensure that the  <paramref name="ageGroupId"/> corresponds to an existing age group to avoid
        /// unexpected results.</remarks>
        /// <param name="ageGroupId">The unique identifier of the age group to delete. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous delete operation. The task result contains an <see
        /// cref="IBaseResult"/>  indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> DeleteAsync(string ageGroupId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"agegroups", ageGroupId);
            return result;
        }
    }
}
