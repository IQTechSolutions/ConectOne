using ConectOne.Domain.Extensions;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using IdentityModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.Parents;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Application.RestServices.Parents
{
    /// <summary>
    /// Provides methods for querying parent-related data from a RESTful API.
    /// </summary>
    /// <remarks>This service is designed to interact with a REST API to retrieve and manage parent-related
    /// information. It supports operations such as retrieving all parents, paginated parent data, parent details by ID
    /// or email, and associated learners or notification lists. The service relies on an HTTP provider to perform the
    /// underlying API calls.</remarks>
    /// <param name="provider"></param>
    public class ParentQueryRestService(IBaseHttpProvider provider) : IParentQueryService
    {
        /// <summary>
        /// Retrieves a collection of parent records asynchronously.
        /// </summary>
        /// <remarks>The method fetches parent records from the underlying data provider. If no <paramref
        /// name="parentId"/> is specified, all available parent records are returned. Use the <paramref
        /// name="cancellationToken"/> to cancel the operation if needed.</remarks>
        /// <param name="parentId">An optional identifier to filter the parent records. If <see langword="null"/>, all parent records are
        /// retrieved.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// wrapping an <see cref="IEnumerable{T}"/> of <see cref="ParentDto"/> objects.</returns>
        public async Task<IBaseResult<IEnumerable<ParentDto>>> AllParentsAsync(string? parentId = null, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<ParentDto>>("parents/all");
            return result;
        }

        /// <summary>
        /// Retrieves a paginated list of parent records based on the specified page parameters.
        /// </summary>
        /// <remarks>This method communicates with the underlying data provider to fetch the paginated
        /// data. Ensure that the <paramref name="pageParameters"/> are properly configured to avoid invalid
        /// requests.</remarks>
        /// <param name="pageParameters">The parameters that define the pagination settings, such as page number and page size.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{ParentDto}"/> containing the paginated list of parent records.</returns>
        public async Task<PaginatedResult<ParentDto>> PagedParentsAsync(ParentPageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetPagedAsync<ParentDto, ParentPageParameters>("parents/pagedparents", pageParameters);
            return result;
        }

        /// <summary>
        /// Retrieves the total count of parent entities.
        /// </summary>
        /// <remarks>The operation sends a request to the underlying provider to fetch the count of parent
        /// entities.  The result encapsulates the count and any additional metadata provided by the <IBaseResult{T}>
        /// implementation.</remarks>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <IBaseResult{T}> where <T>
        /// is an <int> representing the total count of parent entities.</returns>
        public async Task<IBaseResult<int>> ParentCount(CancellationToken cancellationToken)
        {
            var result = await provider.GetAsync<int>("parents/count");
            return result;
        }

        /// <summary>
        /// Retrieves a parent entity by its identifier.
        /// </summary>
        /// <remarks>The <paramref name="trackChanges"/> parameter determines whether the retrieved entity
        /// is tracked  for changes in the underlying data context. This can affect performance and should be used 
        /// appropriately based on the application's requirements.</remarks>
        /// <param name="parentId">The unique identifier of the parent entity to retrieve. Cannot be null or empty.</param>
        /// <param name="trackChanges">A value indicating whether to track changes to the retrieved entity.  If <see langword="true"/>, the entity
        /// will be tracked; otherwise, it will not be tracked.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult{T}"/>
        /// of type <see cref="ParentDto"/> representing the retrieved parent entity.</returns>
        public async Task<IBaseResult<ParentDto>> ParentAsync(string parentId, bool trackChanges, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<ParentDto>($"parents/{parentId}");
            return result;
        }

        /// <summary>
        /// Retrieves a parent record based on the specified email address.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to retrieve a parent record using the
        /// provided email address. Ensure that the email address is valid and properly formatted. The operation may
        /// return an empty result if no parent is associated with the specified email address.</remarks>
        /// <param name="emailAddress">The email address of the parent to retrieve. This value cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="IBaseResult{ParentDto}"/> object with the parent details if found, or an appropriate result indicating
        /// the outcome.</returns>
        public async Task<IBaseResult<ParentDto>> ParentByEmailAsync(string emailAddress, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<ParentDto>($"parents/byemail/{emailAddress}");
            return result;
        }

        /// <summary>
        /// Retrieves a list of learners associated with the specified parent.
        /// </summary>
        /// <remarks>This method sends a request to the underlying data provider to retrieve the learners
        /// associated with the specified parent. Ensure that the <paramref name="parentId"/> is valid and corresponds
        /// to an existing parent in the system.</remarks>
        /// <param name="parentId">The unique identifier of the parent whose learners are to be retrieved. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that holds a list of <see cref="LearnerDto"/> objects representing the learners associated with the
        /// parent.</returns>
        public async Task<IBaseResult<List<LearnerDto>>> ParentLearnersAsync(string parentId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<List<LearnerDto>>($"parents/parentlearners/{parentId}");
            return result;
        }

        /// <summary>
        /// Retrieves a paginated list of parent notification recipients.
        /// </summary>
        /// <remarks>The method sends a request to retrieve the notification recipients for parents based
        /// on the specified pagination parameters. Ensure that <paramref name="pageParameters"/> is properly configured
        /// to avoid unexpected results.</remarks>
        /// <param name="pageParameters">The pagination parameters, including page number and page size, used to filter the results.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// of <see cref="IEnumerable{RecipientDto}"/> representing the list of parent notification recipients.</returns>
        public async Task<IBaseResult<IEnumerable<RecipientDto>>> ParentsNotificationList(ParentPageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<RecipientDto>>($"parents/notificationList?{pageParameters.GetQueryString()}");
            return result;
        }
    }
}
