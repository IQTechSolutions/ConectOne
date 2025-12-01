using ConectOne.Domain.Interfaces;
using ConectOne.Domain.RequestFeatures;
using ConectOne.Domain.ResultWrappers;
using ProductsModule.Domain.DataTransferObjects;
using ProductsModule.Domain.Interfaces;

namespace ProductsModule.Application.RestServices
{
    /// <summary>
    /// Provides methods for managing service tiers, including retrieving, creating, updating, and deleting service
    /// tiers,  as well as associating and disassociating service tiers with services.
    /// </summary>
    /// <remarks>This class acts as a REST-based service layer for interacting with service tier data. It uses
    /// an HTTP provider  to perform operations such as fetching paginated results, retrieving all service tiers, and
    /// managing individual  service tiers. The methods in this class are asynchronous and support cancellation tokens
    /// for cooperative cancellation.</remarks>
    /// <param name="provider"></param>
    public class ServiceTierRestService(IBaseHttpProvider provider) : IServiceTierService
    {
        /// <summary>
        /// Retrieves a paginated list of service tiers based on the specified request parameters.
        /// </summary>
        /// <remarks>The method sends a request to the "service-tiers/paged" endpoint to retrieve the
        /// data.  Ensure that the <paramref name="pageParameters"/> are correctly configured to avoid invalid
        /// requests.</remarks>
        /// <param name="pageParameters">The parameters that define the pagination and filtering options for the request.</param>
        /// <param name="trackChanges">A value indicating whether to track changes to the retrieved entities.  Set to <see langword="true"/> to
        /// enable change tracking; otherwise, <see langword="false"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing the paginated list of <see cref="ServiceTierDto"/> objects.</returns>
        public async Task<PaginatedResult<ServiceTierDto>> PagedServiceTiersAsync(RequestParameters pageParameters, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetPagedAsync<ServiceTierDto, RequestParameters>("service-tiers/paged", pageParameters);
            return result;
        }

        /// <summary>
        /// Retrieves all available service tiers asynchronously.
        /// </summary>
        /// <remarks>This method fetches the service tiers from the underlying provider. The result
        /// includes all service tiers currently available in the system.</remarks>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// of <see cref="IEnumerable{T}"/> containing the available service tiers.</returns>
        public async Task<IBaseResult<IEnumerable<ServiceTierDto>>> AllServiceTiersAsync(CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<ServiceTierDto>>("service-tiers/all");
            return result;
        }

        /// <summary>
        /// Retrieves all service tiers associated with the specified role.
        /// </summary>
        /// <remarks>This method communicates with an external provider to fetch the service tiers. Ensure
        /// that the <paramref name="roleId"/> corresponds to a valid role in the system. The operation may be canceled
        /// by passing a cancellation token.</remarks>
        /// <param name="roleId">The unique identifier of the role for which to retrieve service tiers. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// containing an enumerable collection of <see cref="ServiceTierDto"/> objects representing the service tiers.</returns>
        public async Task<IBaseResult<IEnumerable<ServiceTierDto>>> AllEntityServiceTiersAsync(string roleId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<ServiceTierDto>>($"service-tiers/roles/{roleId}");
            return result;
        }

        /// <summary>
        /// Retrieves the details of a specific service tier asynchronously.
        /// </summary>
        /// <remarks>This method sends a request to retrieve the details of the specified service tier.
        /// Ensure that the <paramref name="serviceTierId"/> is valid and corresponds to an existing service
        /// tier.</remarks>
        /// <param name="serviceTierId">The unique identifier of the service tier to retrieve.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that wraps the <see cref="ServiceTierDto"/> representing the service tier details.</returns>
        public async Task<IBaseResult<ServiceTierDto>> ServiceTierAsync(string serviceTierId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<ServiceTierDto>($"service-tiers/{serviceTierId}");
            return result;
        }

        /// <summary>
        /// Creates a new service tier asynchronously.
        /// </summary>
        /// <remarks>This method sends the provided <paramref name="serviceTier"/> data to the underlying
        /// service for creation. Ensure that the <paramref name="serviceTier"/> object contains valid data before
        /// calling this method.</remarks>
        /// <param name="serviceTier">The service tier data to be created.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the identifier of the created
        /// service tier.</returns>
        public async Task<IBaseResult<string>> CreateAsync(ServiceTierDto serviceTier, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<string, ServiceTierDto>($"service-tiers", serviceTier);
            return result;
        }

        /// <summary>
        /// Updates the service tier by sending the specified data to the service.
        /// </summary>
        /// <remarks>This method sends the provided <paramref name="serviceTier"/> data to the service
        /// endpoint for updating the service tier. Ensure that the <paramref name="serviceTier"/> object contains valid
        /// data before calling this method.</remarks>
        /// <param name="serviceTier">The data transfer object representing the service tier to be updated.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response from the service as
        /// a string.</returns>
        public async Task<IBaseResult<string>> UpdateAsync(ServiceTierDto serviceTier, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<string, ServiceTierDto>($"service-tiers", serviceTier);
            return result;
        }

        /// <summary>
        /// Deletes a service tier with the specified identifier.
        /// </summary>
        /// <remarks>The operation is performed asynchronously. The caller can use the returned task to
        /// await the completion of the delete operation. Ensure that the <paramref name="serviceTierId"/> corresponds
        /// to an existing service tier; otherwise, the operation may fail.</remarks>
        /// <param name="serviceTierId">The unique identifier of the service tier to delete.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with a string indicating the outcome of the delete operation.</returns>
        public async Task<IBaseResult<string>> DeleteAsync(string serviceTierId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync<string>($"service-tiers", serviceTierId);
            return result;
        }

        /// <summary>
        /// Creates a service tier for the specified service asynchronously.
        /// </summary>
        /// <remarks>This method sends a PUT request to create a service tier for the specified service.
        /// Ensure that the <paramref name="serviceId"/> and <paramref name="serviceTierId"/> are valid and correspond
        /// to existing entities.</remarks>
        /// <param name="serviceId">The unique identifier of the service for which the service tier is being created.</param>
        /// <param name="serviceTierId">The unique identifier of the service tier to be created.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> CreateServiceTierServiceAsync(string serviceId, string serviceTierId, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync($"service-tiers/{serviceId}/{serviceTierId}", "");
            return result;
        }

        /// <summary>
        /// Removes the association between a service and a service tier.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to remove the specified service tier
        /// from the given service. Ensure that both <paramref name="serviceId"/> and <paramref name="serviceTierId"/>
        /// are valid and exist in the system.</remarks>
        /// <param name="serviceId">The unique identifier of the service to be disassociated.</param>
        /// <param name="serviceTierId">The unique identifier of the service tier to be disassociated.</param>
        /// <param name="cancellationToken">An optional token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveServiceTierServiceAsync(string serviceId, string serviceTierId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync<string>($"service-tiers/services/{serviceId}/{serviceTierId}", "");
            return result;
        }
    }
}
