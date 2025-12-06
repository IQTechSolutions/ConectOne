using AccomodationModule.Domain.DataTransferObjects;
using ConectOne.Domain.RequestFeatures;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Domain.Interfaces
{
    /// <summary>
    /// Defines a contract for managing vacation contact information, including retrieving, creating, updating, and
    /// removing records.
    /// </summary>
    /// <remarks>This service provides methods to interact with vacation contact information, such as
    /// retrieving a list of records,  fetching details for a specific record, and performing create, update, or delete
    /// operations.  All methods return results wrapped in an <see cref="IBaseResult"/> to indicate the operation's
    /// success or failure.</remarks>
    public interface IVacationContactUsInfoService
    {
        /// <summary>
        /// Retrieves a paginated list of vacation contact information based on the specified request parameters.
        /// </summary>
        /// <remarks>Use this method to retrieve vacation contact information in a paginated format.
        /// Ensure that the  <paramref name="pageParameters"/> are valid to avoid unexpected results.</remarks>
        /// <param name="pageParameters">The pagination parameters, including page number and page size, to control the paginated results.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a collection of <see cref="VacationContactUsInfoDto"/> objects
        /// and pagination metadata, such as total count and current page.</returns>
        Task<PaginatedResult<VacationContactUsInfoDto>> PagedVacationContactUsInfoListAsync(RequestParameters pageParameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a list of vacation contact information records.
        /// </summary>
        /// <remarks>This method is used to asynchronously fetch vacation contact information. The result
        /// includes details about each contact record. If no records are found, the enumerable collection in the result
        /// will be empty.</remarks>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// containing an enumerable collection of <see cref="VacationContactUsInfoDto"/> objects.</returns>
        Task<IBaseResult<IEnumerable<VacationContactUsInfoDto>>> VacationContactUsInfoListAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the contact information for a specific vacation inquiry.
        /// </summary>
        /// <param name="vacationContactUsInfoId">The unique identifier of the vacation contact information to retrieve. Cannot be <see langword="null"/> or
        /// empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping the <see cref="VacationContactUsInfoDto"/> for the specified identifier.</returns>
        Task<IBaseResult<VacationContactUsInfoDto>> VacationContactUsInfoAsync(string vacationContactUsInfoId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new vacation contact information entry asynchronously.
        /// </summary>
        /// <remarks>The <paramref name="dto"/> parameter must contain valid data for the vacation contact
        /// information. If the operation is canceled via the <paramref name="cancellationToken"/>, the task will be
        /// marked as canceled.</remarks>
        /// <param name="dto">The data transfer object containing the vacation contact information to be created.</param>
        /// <param name="cancellationToken">An optional token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        Task<IBaseResult> CreateVacationContactUsInfoAsync(VacationContactUsInfoDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the vacation contact information with the provided details.
        /// </summary>
        /// <remarks>Use this method to update the vacation contact information stored in the system.
        /// Ensure that the <paramref name="dto"/> parameter contains valid data before calling this method. The
        /// operation may be canceled by passing a cancellation token.</remarks>
        /// <param name="dto">The data transfer object containing the updated vacation contact information.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        Task<IBaseResult> UpdateVacationContactUsInfoAsync(VacationContactUsInfoDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes the vacation contact information associated with the specified review.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to remove vacation contact
        /// information.  Ensure that the <paramref name="reviewId"/> corresponds to an existing review before calling
        /// this method.</remarks>
        /// <param name="reviewId">The unique identifier of the review whose vacation contact information is to be removed. Cannot be null or
        /// empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        Task<IBaseResult> RemoveVacationContactUsInfoAsync(string reviewId, CancellationToken cancellationToken = default);
    }
}
