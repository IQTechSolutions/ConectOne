using BeneficiariesModule.Domain.DataTransferObjects;
using BeneficiariesModule.Domain.RequestFeatures;
using ConectOne.Domain.ResultWrappers;

namespace BeneficiariesModule.Domain.Interfaces
{
    /// <summary>
    /// Defines a contract for managing beneficiaries and their related operations.
    /// </summary>
    /// <remarks>This interface provides methods for retrieving, creating, updating, and deleting
    /// beneficiaries,  as well as managing user-specific beneficiaries and ambassador relationships. It supports 
    /// paginated results for beneficiary queries and ensures consistent handling of operations  through standardized
    /// result types.</remarks>
    public interface IBeneficiaryService
    {
        /// <summary>
        /// Retrieves a paginated list of beneficiaries based on the specified page parameters.
        /// </summary>
        /// <param name="pageParameters">The parameters that define the pagination settings, such as page number and page size.</param>
        /// <returns>A <see cref="PaginatedResult{BeneficiaryDto}"/> containing the paginated list of beneficiaries. The result
        /// includes metadata such as total count and current page information.</returns>
        Task<PaginatedResult<BeneficiaryDto>> PagedBeneficiariesAsync(BeneficiaryPageParameters pageParameters);

        /// <summary>
        /// Retrieves a paginated list of user beneficiaries based on the specified paging parameters.
        /// </summary>
        /// <param name="pageParameters">The parameters that define the pagination settings, including page number and page size. Must not be <see
        /// langword="null"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a collection of <see cref="BeneficiaryDto"/> objects
        /// representing the user beneficiaries for the specified page. The result includes pagination metadata such as
        /// total count and current page.</returns>
        Task<PaginatedResult<BeneficiaryDto>> PagedUserBeneficiariesAsync(BeneficiaryPageParameters pageParameters);

        /// <summary>
        /// Retrieves beneficiary details based on the specified beneficiary ID.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch beneficiary information.
        /// Ensure that  <benificiaryId/> is a valid positive integer before calling this method.</remarks>
        /// <param name="benificiaryId">The unique identifier of the beneficiary to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <IBaseResult{T}/> object
        /// with the beneficiary details encapsulated in a  <BeneficiaryDto/>. If the beneficiary is not found, the
        /// result may indicate an error or empty data.</returns>
        Task<IBaseResult<BeneficiaryDto>> BeneficiaryAsync(int benificiaryId);

        /// <summary>
        /// Creates a new beneficiary record asynchronously.
        /// </summary>
        /// <remarks>Use this method to create a new beneficiary record in the system. Ensure that the 
        /// <paramref name="benificiary"/> object contains valid data before calling this method.</remarks>
        /// <param name="benificiary">The beneficiary data transfer object containing the details of the beneficiary to be created. Cannot be <see
        /// langword="null"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult{T}"/>
        /// object with the created <see cref="BeneficiaryDto"/> if the operation succeeds.</returns>
        Task<IBaseResult<BeneficiaryDto>> CreateAsync(BeneficiaryDto benificiary);

        /// <summary>
        /// Updates the specified beneficiary's information asynchronously.
        /// </summary>
        /// <remarks>This method performs an update operation on the beneficiary's data. Ensure that the
        /// provided  <see cref="BeneficiaryDto"/> contains valid and complete information before calling this
        /// method.</remarks>
        /// <param name="benificiary">The <see cref="BeneficiaryDto"/> object containing the updated information for the beneficiary. This
        /// parameter cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// object with the updated <see cref="BeneficiaryDto"/> data if the operation succeeds.</returns>
        Task<IBaseResult<BeneficiaryDto>> UpdateAsync(BeneficiaryDto benificiary);

        /// <summary>
        /// Deletes a beneficiary with the specified ID.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to delete a beneficiary. Ensure that
        /// the provided ID corresponds to an existing beneficiary.</remarks>
        /// <param name="benificiaryId">The unique identifier of the beneficiary to delete. Must be a positive integer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a string indicating the outcome
        /// of the deletion.</returns>
        Task<IBaseResult<string>> DeleteBeneficiaryAsync(int benificiaryId);

        /// <summary>
        /// Creates a new user beneficiary asynchronously.
        /// </summary>
        /// <remarks>Ensure that the <paramref name="benificiary"/> object contains valid data before
        /// calling this method. The operation may fail if required fields are missing or invalid.</remarks>
        /// <param name="benificiary">The data transfer object containing the details of the beneficiary to be created. This parameter cannot be
        /// null and must include all required fields for beneficiary creation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
        Task<IBaseResult> CreateUserBeneficiaryAsync(UserBeneficiaryCreationDto benificiary);

        /// <summary>
        /// Deletes a beneficiary associated with a specific user.
        /// </summary>
        /// <remarks>Ensure that both <paramref name="userId"/> and <paramref name="benificiaryId"/> are
        /// valid identifiers  before calling this method. The operation may fail if the specified beneficiary does not
        /// exist.</remarks>
        /// <param name="userId">The unique identifier of the user whose beneficiary is to be deleted. Cannot be null or empty.</param>
        /// <param name="benificiaryId">The unique identifier of the beneficiary to be deleted. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the success or failure of the operation.</returns>
        Task<IBaseResult> DeleteUserBeneficiaryAsync(string userId, string benificiaryId );

        #region Ambassador

        /// <summary>
        /// Retrieves information about a beneficiary ambassador based on the specified ambassador ID.
        /// </summary>
        /// <remarks>Use this method to retrieve detailed information about a specific ambassador
        /// associated with a beneficiary. Ensure that the provided <paramref name="ambassadorId"/> is valid and
        /// corresponds to an existing ambassador.</remarks>
        /// <param name="ambassadorId">The unique identifier of the ambassador. This parameter cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the ambassador's details encapsulated in an <see cref="AmbassadorDto"/>.</returns>
        Task<IBaseResult<AmbassadorDto>> BeneficiaryAmbassadorAsync(string ambassadorId);

        /// <summary>
        /// Adds or updates an ambassador for the specified beneficiary.
        /// </summary>
        /// <remarks>Use this method to associate an ambassador with a beneficiary or update the
        /// ambassador's details if they already exist. Ensure that the <paramref name="beneficiaryId"/> corresponds to
        /// a valid beneficiary and that the <paramref name="dto"/> contains valid data.</remarks>
        /// <param name="beneficiaryId">The unique identifier of the beneficiary for whom the ambassador is being added or updated. Cannot be null
        /// or empty.</param>
        /// <param name="dto">The data transfer object containing the ambassador's details. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
        Task<IBaseResult> AddUpdateBeneficiaryAmbassadorAsync(string beneficiaryId, AmbassadorDto dto);

        #endregion
    }
}
