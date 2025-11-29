using ConectOne.Domain.RequestFeatures;
using ConectOne.Domain.ResultWrappers;
using ProductsModule.Domain.DataTransferObjects;

namespace ProductsModule.Domain.Interfaces
{
    /// <summary>
    /// Defines a contract for managing service tiers, including operations for retrieving, creating, updating, and
    /// deleting service tier data.
    /// </summary>
    /// <remarks>This interface provides methods to interact with service tier data, supporting both paginated
    /// and non-paginated retrieval. It also includes functionality for creating, updating, and deleting service tiers.
    /// The methods are asynchronous and support cancellation tokens for cooperative cancellation.</remarks>
    public interface IServiceTierService
    {
        /// <summary>
        /// Retrieves a paginated list of service tiers based on the specified request parameters.
        /// </summary>
        /// <remarks>The <paramref name="pageParameters"/> parameter must be provided to specify the
        /// pagination details,  such as the page number and page size. If no service tiers are found, the result will
        /// contain an empty collection.</remarks>
        /// <param name="pageParameters">The parameters that define the pagination and filtering options for the request.</param>
        /// <param name="trackChanges">A value indicating whether to track changes to the retrieved entities.  Set to <see langword="true"/> to
        /// enable change tracking; otherwise, <see langword="false"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a  <see
        /// cref="PaginatedResult{T}"/> of <see cref="ServiceTierDto"/> objects representing the service tiers.</returns>
        Task<PaginatedResult<ServiceTierDto>> PagedServiceTiersAsync(RequestParameters pageParameters, bool trackChanges = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all available service tiers.
        /// </summary>
        /// <remarks>This method retrieves a collection of service tiers, which may include metadata or
        /// configuration details about each tier. The operation supports cancellation via the provided <paramref
        /// name="cancellationToken"/>.</remarks>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// containing an enumerable collection of <see cref="ServiceTierDto"/> objects representing the service tiers.</returns>
        Task<IBaseResult<IEnumerable<ServiceTierDto>>> AllServiceTiersAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all service tiers associated with the specified role.
        /// </summary>
        /// <remarks>This method is typically used to retrieve service tier information for a specific
        /// role in the system.  The result may vary depending on the role's configuration and associated service
        /// tiers.</remarks>
        /// <param name="roleId">The unique identifier of the role for which to retrieve service tiers. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// containing an enumerable collection of <see cref="ServiceTierDto"/> objects representing the service tiers.</returns>
        Task<IBaseResult<IEnumerable<ServiceTierDto>>> AllEntityServiceTiersAsync(string roleId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the details of a specific service tier based on the provided identifier.
        /// </summary>
        /// <param name="serviceTierId">The unique identifier of the service tier to retrieve. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping the <see cref="ServiceTierDto"/> for the specified service tier.</returns>
        Task<IBaseResult<ServiceTierDto>> ServiceTierAsync(string serviceTierId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new service tier asynchronously.
        /// </summary>
        /// <remarks>The operation may fail if the provided service tier data is invalid or if the system
        /// is in an inconsistent state. Ensure that the <paramref name="serviceTier"/> object contains all required
        /// fields before calling this method.</remarks>
        /// <param name="serviceTier">The data transfer object containing the details of the service tier to create.</param>
        /// <param name="cancellationToken">An optional token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with the identifier of the created service tier as a string.</returns>
        Task<IBaseResult<string>> CreateAsync(ServiceTierDto serviceTier, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the specified service tier asynchronously.
        /// </summary>
        /// <remarks>The operation may involve validation of the provided service tier data. Ensure that
        /// the data is complete and adheres to the expected format before calling this method.</remarks>
        /// <param name="serviceTier">The service tier data to be updated. Cannot be <see langword="null"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with a string indicating the outcome of the update operation.</returns>
        Task<IBaseResult<string>> UpdateAsync(ServiceTierDto serviceTier, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the specified service tier asynchronously.
        /// </summary>
        /// <param name="serviceTierId">The unique identifier of the service tier to delete. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a string indicating the outcome
        /// of the deletion.</returns>
        Task<IBaseResult<string>> DeleteAsync(string serviceTierId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a service tier for the specified service asynchronously.
        /// </summary>
        /// <param name="serviceId">The unique identifier of the service for which the tier is being created. Cannot be null or empty.</param>
        /// <param name="serviceTierId">The unique identifier of the service tier to create. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        Task<IBaseResult> CreateServiceTierServiceAsync(string serviceId, string serviceTierId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes the association between a service and a service tier asynchronously.
        /// </summary>
        /// <remarks>This method removes the link between a specific service and a service tier. Ensure
        /// that both  <paramref name="serviceId"/> and <paramref name="serviceTierId"/> are valid and exist in the
        /// system  before calling this method. The operation is performed asynchronously and can be canceled using  the
        /// provided <paramref name="cancellationToken"/>.</remarks>
        /// <param name="serviceId">The unique identifier of the service to be disassociated.</param>
        /// <param name="serviceTierId">The unique identifier of the service tier to be disassociated.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        Task<IBaseResult> RemoveServiceTierServiceAsync(string serviceId, string serviceTierId, CancellationToken cancellationToken = default);
    }
}