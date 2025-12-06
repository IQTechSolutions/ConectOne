using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.RequestFeatures;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Application.RestServices
{
    /// <summary>
    /// Provides RESTful operations for managing vacation contact information, including retrieving, creating, updating,
    /// and deleting records.
    /// </summary>
    /// <remarks>This service is designed to handle vacation-related contact information through a set of
    /// asynchronous methods.  It supports paginated retrieval of data, fetching individual records, and performing CRUD
    /// operations.</remarks>
    /// <param name="provider"></param>
    public class VacationContactUsInfoRestService(IBaseHttpProvider provider) : IVacationContactUsInfoService
    {
        /// <summary>
        /// Retrieves a paginated list of vacation contact information records.
        /// </summary>
        /// <remarks>This method queries the data source for vacation contact information and returns the
        /// results in a paginated format. Use the <paramref name="pageParameters"/> to specify the desired page and
        /// size of the result set.</remarks>
        /// <param name="pageParameters">The pagination parameters, including page number and page size, used to control the subset of data returned.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a collection of <see cref="VacationContactUsInfoDto"/> objects
        /// and pagination metadata.</returns>
        public async Task<PaginatedResult<VacationContactUsInfoDto>> PagedVacationContactUsInfoListAsync(RequestParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetPagedAsync<VacationContactUsInfoDto, RequestParameters>("vacationContactUsInfos", pageParameters);
            return result;
        }

        /// <summary>
        /// Retrieves a list of vacation contact information records asynchronously.
        /// </summary>
        /// <remarks>This method fetches vacation contact information records from the underlying data
        /// provider. The returned result encapsulates the data and any associated metadata.</remarks>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// of <see cref="IEnumerable{T}"/> containing <see cref="VacationContactUsInfoDto"/> objects.</returns>
        public async Task<IBaseResult<IEnumerable<VacationContactUsInfoDto>>> VacationContactUsInfoListAsync(CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<VacationContactUsInfoDto>>("vacationContactUsInfos");
            return result;
        }

        /// <summary>
        /// Retrieves vacation contact information based on the specified identifier.
        /// </summary>
        /// <remarks>This method fetches vacation contact information from the underlying data provider.
        /// Ensure that the <paramref name="vacationContactUsInfoId"/> corresponds to a valid record in the data
        /// source.</remarks>
        /// <param name="vacationContactUsInfoId">The unique identifier of the vacation contact information to retrieve. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping the <see cref="VacationContactUsInfoDto"/> for the specified identifier.</returns>
        public async Task<IBaseResult<VacationContactUsInfoDto>> VacationContactUsInfoAsync(string vacationContactUsInfoId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<VacationContactUsInfoDto>($"vacationContactUsInfos/{vacationContactUsInfoId}");
            return result;
        }

        /// <summary>
        /// Creates or updates vacation contact information asynchronously.
        /// </summary>
        /// <remarks>This method sends the provided vacation contact information to the underlying data
        /// provider for creation or update. Ensure that the <paramref name="dto"/> contains valid data before calling
        /// this method.</remarks>
        /// <param name="dto">The data transfer object containing the vacation contact information to be created or updated.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> CreateVacationContactUsInfoAsync(VacationContactUsInfoDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync($"vacationContactUsInfos", dto);
            return result;
        }

        /// <summary>
        /// Updates the vacation contact information asynchronously.
        /// </summary>
        /// <remarks>This method sends the provided vacation contact information to the underlying service
        /// for updating. Ensure that the <paramref name="dto"/> contains valid data before calling this
        /// method.</remarks>
        /// <param name="dto">The data transfer object containing the vacation contact information to be updated.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateVacationContactUsInfoAsync(VacationContactUsInfoDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"vacationContactUsInfos", dto);
            return result;
        }

        /// <summary>
        /// Removes the vacation contact information associated with the specified review ID.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to delete vacation contact
        /// information. Ensure that the  <paramref name="reviewId"/> provided is valid and corresponds to an existing
        /// record.</remarks>
        /// <param name="reviewId">The unique identifier of the review whose vacation contact information is to be removed.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveVacationContactUsInfoAsync(string reviewId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"vacationContactUsInfos", reviewId);
            return result;
        }
    }
}
