using ConectOne.Domain.RequestFeatures;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using Microsoft.EntityFrameworkCore;
using ProductsModule.Domain.DataTransferObjects;
using ProductsModule.Domain.Entities;
using ProductsModule.Domain.Interfaces;

namespace ProductsModule.Infrastructure.Implementations
{
    /// <summary>
    /// Provides methods for managing service tiers and their associations with services.
    /// </summary>
    /// <remarks>This class implements the <see cref="IServiceTierService"/> interface and provides
    /// functionality for creating, retrieving, updating, and deleting service tiers, as well as managing their
    /// relationships with services. It uses repository patterns to interact with the underlying data storage and
    /// ensures that operations are performed asynchronously. The methods in this class return results wrapped in
    /// standardized result types, such as <see cref="PaginatedResult{T}"/> and <see cref="repository"/>, to
    /// indicate success or failure along with any relevant data or error messages.</remarks>
    /// <param name="tierServiceRepository"></param>
    /// <param name="tierServiceRepository"></param>
    public sealed class ServiceTierService(IRepository<ServiceTier, string> repository, IRepository<TierService, string> tierServiceRepository) : IServiceTierService
    {
        /// <summary>
        /// Retrieves a paginated list of service tiers based on the specified request parameters.
        /// </summary>
        /// <remarks>This method uses a specification pattern to retrieve service tiers, including their
        /// associated tier services  and offered services. The returned data is mapped to DTOs for external
        /// consumption.</remarks>
        /// <param name="pageParameters">The pagination parameters, including the page number and page size, used to determine the subset of results
        /// to return.</param>
        /// <param name="trackChanges">A boolean value indicating whether to track changes to the retrieved entities.  Set to <see
        /// langword="true"/> to enable change tracking; otherwise, <see langword="false"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation will terminate early if the token is canceled.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a collection of <see cref="ServiceTierDto"/> objects 
        /// representing the service tiers, along with pagination metadata. If the operation fails, the result will 
        /// contain error messages.</returns>
        public async Task<PaginatedResult<ServiceTierDto>> PagedServiceTiersAsync(RequestParameters pageParameters, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<ServiceTier>(c => true);
            spec.AddInclude(g => g.Include(c => c.Images).ThenInclude(c => c.Image));
            spec.AddInclude(g => g.Include(c => c.TierServices).ThenInclude(c => c.OfferedService));

            var result = await repository.ListAsync(spec, trackChanges, cancellationToken);
            if (!result.Succeeded)
                return PaginatedResult<ServiceTierDto>.Failure(result.Messages);

            var dtoList = result.Data.Select(c => new ServiceTierDto(c)).ToList();
            return PaginatedResult<ServiceTierDto>.Success(dtoList, dtoList.Count, pageParameters.PageNr, pageParameters.PageSize);
        }

        /// <summary>
        /// Retrieves all available service tiers.
        /// </summary>
        /// <remarks>If the operation fails, the result will include failure messages. The returned
        /// collection will be empty if no service tiers are available.</remarks>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// containing a collection of <see cref="ServiceTierDto"/> objects representing the service tiers.</returns>
        public async Task<IBaseResult<IEnumerable<ServiceTierDto>>> AllServiceTiersAsync(CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<ServiceTier>(c => true);
            spec.AddInclude(g => g.Include(c => c.Images).ThenInclude(c => c.Image));
            spec.AddInclude(g => g.Include(c => c.TierServices).ThenInclude(c => c.OfferedService));

            var result = await repository.ListAsync(spec, false, cancellationToken);
            if (!result.Succeeded)
                return await Result<IEnumerable<ServiceTierDto>>.FailAsync(result.Messages);

            return await Result<IEnumerable<ServiceTierDto>>.SuccessAsync(result.Data.Select(c => new ServiceTierDto(c)));
        }

        /// <summary>
        /// Retrieves all service tiers associated with the specified role.
        /// </summary>
        /// <remarks>This method queries the repository for service tiers associated with the specified
        /// role and maps the results to <see cref="ServiceTierDto"/> objects. If the operation is unsuccessful, the
        /// result will include the failure messages.</remarks>
        /// <param name="roleId">The identifier of the role for which to retrieve service tiers. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// where <c>T</c> is an <see cref="IEnumerable{T}"/> of <see cref="ServiceTierDto"/> objects. If the operation
        /// fails, the result contains error messages.</returns>
        public async Task<IBaseResult<IEnumerable<ServiceTierDto>>> AllEntityServiceTiersAsync(string roleId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<ServiceTier>(c => c.RoleId == roleId);
            spec.AddInclude(g => g.Include(c => c.Images).ThenInclude(c => c.Image));
            spec.AddInclude(g => g.Include(c => c.TierServices).ThenInclude(c => c.OfferedService));

            var result = await repository.ListAsync(spec,false, cancellationToken);
            if (!result.Succeeded)
                return await Result<IEnumerable<ServiceTierDto>>.FailAsync(result.Messages);

            return await Result<IEnumerable<ServiceTierDto>>.SuccessAsync(result.Data.Select(c => new ServiceTierDto(c)));
        }

        /// <summary>
        /// Retrieves the service tier information for a specified supplier.
        /// </summary>
        /// <remarks>If no supplier with the specified <paramref name="supplierId"/> exists in the
        /// database, the result will indicate failure with an appropriate error message.</remarks>
        /// <param name="supplierId">The unique identifier of the supplier whose service tier information is to be retrieved.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// where <c>T</c> is <see cref="ServiceTierDto"/>. The result indicates whether the operation succeeded and, if
        /// successful, includes the service tier information for the specified supplier.</returns>
        public async Task<IBaseResult<ServiceTierDto>> ServiceTierAsync(string supplierId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<ServiceTier>(c => c.Id == supplierId);
            spec.AddInclude(g => g.Include(c => c.Images).ThenInclude(c => c.Image));
            spec.AddInclude(g => g.Include(c => c.TierServices).ThenInclude(c => c.OfferedService));

            var result = await repository.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!result.Succeeded)
                return await Result<ServiceTierDto>.FailAsync(result.Messages);

            if (result.Data is null)
                return await Result<ServiceTierDto>.FailAsync($"No supplier with id matching '{supplierId}' was found in the database");

            return await Result<ServiceTierDto>.SuccessAsync(new ServiceTierDto(result.Data));
        }

        /// <summary>
        /// Creates a new service tier asynchronously.
        /// </summary>
        /// <remarks>This method attempts to create a new service tier using the provided <paramref
        /// name="serviceTier"/> object. If the creation is successful, the result will include the ID of the newly
        /// created service tier. Otherwise, the result will contain error messages describing the failure.</remarks>
        /// <param name="serviceTier">The data transfer object representing the service tier to be created.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// where <c>T</c> is a <see cref="string"/> representing the ID of the created service tier if the operation
        /// succeeds, or error messages if it fails.</returns>
        public async Task<IBaseResult<string>> CreateAsync(ServiceTierDto serviceTier, CancellationToken cancellationToken = default)
        {
            var result = await repository.CreateAsync(serviceTier.ToServiceTier(), cancellationToken);
            if(!result.Succeeded) return await Result<string>.FailAsync(result.Messages);

            var saveResult = await repository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result<string>.FailAsync(saveResult.Messages);

            return await Result<string>.SuccessAsync(serviceTier.Id);
        }

        /// <summary>
        /// Updates an existing service tier in the database with the provided data.
        /// </summary>
        /// <remarks>This method performs the following steps: <list type="number">
        /// <item><description>Retrieves the existing service tier from the database using the ID provided in <paramref
        /// name="serviceTier"/>.</description></item> <item><description>Validates that the service tier exists and
        /// updates its properties with the provided data.</description></item> <item><description>Attempts to save the
        /// changes to the database.</description></item> </list> If the service tier does not exist, or if any step in
        /// the update process fails, the method returns a failure result with an appropriate error message.</remarks>
        /// <param name="serviceTier">The updated service tier data to apply. The <see cref="ServiceTierDto.Id"/> property must match an existing
        /// service tier in the database.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// where <c>T</c> is a <see cref="string"/>: <list type="bullet"> <item><description>The ID of the updated
        /// service tier if the operation succeeds.</description></item> <item><description>An error message if the
        /// operation fails.</description></item> </list></returns>
        public async Task<IBaseResult<string>> UpdateAsync(ServiceTierDto serviceTier, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<ServiceTier>(c => c.Id == serviceTier.Id);
            spec.AddInclude(g => g.Include(c => c.TierServices).ThenInclude(c => c.OfferedService));

            var result = await repository.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!result.Succeeded)
                return await Result<string>.FailAsync(result.Messages);

            var response = result.Data;
            if (response is null)
                return await Result<string>.FailAsync($"No supplier with id matching '{serviceTier.Id}' was found in the database");

            var toUpdate = result.Data;
            toUpdate = serviceTier.UpdateServiceTier(toUpdate);

            var updateResult = repository.Update(toUpdate);
            if (!updateResult.Succeeded)
                return await Result<string>.FailAsync(updateResult.Messages);

            var saveResult = await repository.SaveAsync(cancellationToken);
            if(!saveResult.Succeeded) return await Result<string>.FailAsync(saveResult.Messages);
            return await Result<string>.SuccessAsync(serviceTier.Id);
        }

        /// <summary>
        /// Deletes a service tier with the specified identifier from the database.
        /// </summary>
        /// <remarks>This method retrieves the service tier entity based on the provided identifier,
        /// including its related tier services and offered services. If the entity is found, it is deleted from the
        /// database. If the entity is not found or the deletion fails, the result will indicate the failure with
        /// appropriate messages.</remarks>
        /// <param name="serviceTierId">The unique identifier of the service tier to delete. This value cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation. The default value is <see
        /// cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// indicating the outcome of the operation. If successful, the result contains a success message. Otherwise, it
        /// contains error messages describing the failure.</returns>
        public async Task<IBaseResult<string>> DeleteAsync(string serviceTierId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<ServiceTier>(c => c.Id == serviceTierId);
            spec.AddInclude(g => g.Include(c => c.TierServices).ThenInclude(c => c.OfferedService));

            var result = await repository.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!result.Succeeded)
                return await Result<string>.FailAsync(result.Messages);

            var entity = result.Data;
            if (entity is null)
                return await Result<string>.FailAsync($"No supplier with id matching '{serviceTierId}' found in the database");

            repository.Delete(entity);
            var saveResult = await repository.SaveAsync(cancellationToken);
            if(saveResult.Succeeded)
            {
                return await Result<string>.SuccessAsync("Supplier successfully removed");
            }
            return await Result<string>.FailAsync(saveResult.Messages);
        }

        /// <summary>
        /// Creates a new service tier service asynchronously.
        /// </summary>
        /// <remarks>This method creates a new association between a service and a service tier. It
        /// performs the operation asynchronously and ensures that the changes are persisted to the repository. If the
        /// operation fails at any stage, an appropriate failure result is returned.</remarks>
        /// <param name="serviceId">The unique identifier of the service to associate with the service tier.</param>
        /// <param name="serviceTierId">The unique identifier of the service tier to be created.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation. If the operation fails, the result includes error
        /// messages.</returns>
        public async Task<IBaseResult> CreateServiceTierServiceAsync(string serviceId, string serviceTierId, CancellationToken cancellationToken = default)
        {
            var result = await tierServiceRepository.CreateAsync(new TierService(){ ServiceTierId = serviceTierId, OfferedServiceId = serviceId }, cancellationToken);
            if (!result.Succeeded) return await Result.FailAsync(result.Messages);

            var saveResult = await repository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result<string>.SuccessAsync();
        }

        /// <summary>
        /// Removes the association between a service and a service tier asynchronously.
        /// </summary>
        /// <remarks>This method removes the relationship between the specified service and service tier.
        /// If the association does not exist, the operation will fail and return the appropriate error
        /// messages.</remarks>
        /// <param name="serviceId">The unique identifier of the service to be disassociated.</param>
        /// <param name="serviceTierId">The unique identifier of the service tier to be disassociated.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation, along with any associated messages.</returns>
        public async Task<IBaseResult> RemoveServiceTierServiceAsync(string serviceId, string serviceTierId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<TierService>(c => c.ServiceTierId == serviceTierId && c.OfferedServiceId == serviceId);
            var result = await tierServiceRepository.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!result.Succeeded)
                return await Result.FailAsync(result.Messages);

            var updateResult = await tierServiceRepository.DeleteAsync(result.Data!.Id, cancellationToken);
            if (!updateResult.Succeeded)
                return await Result<string>.FailAsync(updateResult.Messages);

            var saveResult = await repository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result<string>.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync();
        }
    }
}