using ConectOne.Domain.DataTransferObjects;
using ConectOne.Domain.Entities;
using ConectOne.Domain.Enums;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using LocationModule.Domain.Interfaces;

namespace LocationModule.Infrastructure.Implementation
{
    /// <summary>
    /// Provides functionality for managing addresses associated with a specific entity type.
    /// </summary>
    /// <remarks>This service allows for creating, retrieving, updating, and deleting addresses, as well as
    /// retrieving addresses based on specific criteria such as parent entity ID, route ID, or address type. It relies
    /// on a repository pattern for data access and logging for operational insights.</remarks>
    /// <typeparam name="TEntity">The type of the entity associated with the addresses. Must implement <see cref="IAuditableEntity"/>.</typeparam>
    /// <param name="repository"></param>
    /// <param name="logger"></param>
    public sealed class AddressService<TEntity>(IRepository<Address<TEntity>, string> repository) : IAddressService<TEntity> where TEntity : IAuditableEntity<TEntity>
    {
        private readonly IRepository<Address<TEntity>, string> _repository = repository;

        /// <summary>
        /// Retrieves all addresses associated with the specified parent entity, optionally filtered by route ID.
        /// </summary>
        /// <remarks>This method retrieves addresses associated with a specific parent entity. If a
        /// <paramref name="routId"/> is provided, the result is filtered to include only addresses matching the
        /// specified route ID. The <paramref name="trackChanges"/> parameter determines whether the retrieved entities
        /// are tracked for changes in the underlying data store.</remarks>
        /// <param name="parentId">The unique identifier of the parent entity whose addresses are to be retrieved. Cannot be null or empty.</param>
        /// <param name="routId">An optional route ID to filter the addresses. If null or empty, no filtering by route ID is applied.</param>
        /// <param name="trackChanges">A boolean value indicating whether to track changes to the retrieved entities. Defaults to <see
        /// langword="false"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// with a collection of <see cref="AddressDto"/> objects if the operation succeeds, or error messages if it
        /// fails.</returns>
        public async Task<IBaseResult<IEnumerable<AddressDto>>> GetAllAddressesAsync(string parentId, string? routId, bool trackChanges = false)
        {
            var result = _repository.FindByCondition(c => c.EntityId.Equals(parentId), false);
            if (result.Succeeded)
            {
                var response = result.Data.Select(c => new AddressDto(c)).ToList();

                if(!string.IsNullOrEmpty(routId))
                {
                    response = response.Where(c => c.RouteId == routId).ToList();
                }

                return Result<IEnumerable<AddressDto>>.Success(response);
            }
            return Result<IEnumerable<AddressDto>>.Fail(result.Messages);
        }

        /// <summary>
        /// Retrieves all addresses of a specified type for a given parent entity.
        /// </summary>
        /// <remarks>This method queries the repository for addresses associated with the specified parent
        /// entity and address type. The result includes only addresses that match the specified criteria. If no
        /// matching addresses are found, the result will indicate failure with appropriate error messages.</remarks>
        /// <param name="parentId">The unique identifier of the parent entity whose addresses are to be retrieved. Cannot be null or empty.</param>
        /// <param name="addressType">The type of addresses to retrieve. Must be a valid <see cref="AddressType"/> value.</param>
        /// <param name="trackChanges">A boolean value indicating whether to track changes to the retrieved entities.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// where T is an <see cref="IEnumerable{T}"/> of <see cref="AddressDto"/> objects. If the operation succeeds,
        /// the result contains the list of addresses; otherwise, it contains error messages.</returns>
        public async Task<IBaseResult<IEnumerable<AddressDto>>> GetAllAddressesByTypeAsync(string parentId, AddressType addressType, bool trackChanges)
        {
            var result = _repository.FindByCondition(c => c.EntityId.Equals(parentId) && c.AddressType.Equals(addressType), false);
            if (result.Succeeded)
            {
                var response = result.Data.Select(c => new AddressDto(c)).ToList();
                return Result<IEnumerable<AddressDto>>.Success(response);
            }
            return Result<IEnumerable<AddressDto>>.Fail(result.Messages);
        }

        /// <summary>
        /// Retrieves all addresses associated with the specified route.
        /// </summary>
        /// <remarks>The method queries the underlying data source for addresses linked to the provided
        /// route identifier.  If the operation is successful, the result includes a collection of address data transfer
        /// objects.  Otherwise, the result contains error messages describing the failure.</remarks>
        /// <param name="routeId">The unique identifier of the route for which to retrieve addresses. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// with a collection of <see cref="AddressDto"/> objects representing the addresses associated with the
        /// specified route.  If the operation fails, the result contains error messages.</returns>
        public async Task<IBaseResult<IEnumerable<AddressDto>>> GetAllRouteAddressesAsync(string routeId)
        {
            var result = _repository.FindByCondition(c => c.RouteId.Equals(routeId), false);
            if (result.Succeeded)
            {
                var response = result.Data.Select(c => new AddressDto(c)).ToList();
                return Result<IEnumerable<AddressDto>>.Success(response);
            }
            return Result<IEnumerable<AddressDto>>.Fail(result.Messages);
        }

        /// <summary>
        /// Retrieves all route addresses associated with a specific entity and route.
        /// </summary>
        /// <remarks>This method queries the repository for route addresses that match the specified
        /// entity and route identifiers. The returned result indicates whether the operation succeeded and provides the
        /// corresponding data or error messages.</remarks>
        /// <param name="parentId">The unique identifier of the parent entity whose route addresses are to be retrieved. Cannot be null or
        /// empty.</param>
        /// <param name="routeId">The unique identifier of the route for which addresses are to be retrieved. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// where <c>T</c> is an <see cref="IEnumerable{T}"/> of <see cref="AddressDto"/> objects. If the operation
        /// succeeds, the result contains the list of route addresses; otherwise, it contains error messages.</returns>
        public async Task<IBaseResult<IEnumerable<AddressDto>>> GetAllEntityRouteAddressesAsync(string parentId, string routeId)
        {
            var result = _repository.FindByCondition(c => c.RouteId.Equals(routeId) && c.EntityId == parentId, false);
            if (result.Succeeded)
            {
                var response = result.Data.Select(c => new AddressDto(c)).ToList();
                return Result<IEnumerable<AddressDto>>.Success(response);
            }
            return Result<IEnumerable<AddressDto>>.Fail(result.Messages);
        }

        /// <summary>
        /// Retrieves an address by its unique identifier.
        /// </summary>
        /// <remarks>If no address is found with the specified <paramref name="id"/>, the result will
        /// indicate failure  with an appropriate error message.</remarks>
        /// <param name="id">The unique identifier of the address to retrieve. Cannot be null or empty.</param>
        /// <param name="trackChanges">A boolean value indicating whether to track changes to the retrieved entity.  Set to <see langword="true"/>
        /// to enable change tracking; otherwise, <see langword="false"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult{T}"/>
        /// of type <see cref="AddressDto"/>. If the operation succeeds,  the result contains the address data;
        /// otherwise, it contains error messages.</returns>
        public async Task<IBaseResult<AddressDto>> GetAddressAsync(string id, bool trackChanges)
        {
            var result = _repository.FindByCondition(c => c.Id.Equals(id), false);
            if (result.Succeeded)
            {
                var response = result.Data.FirstOrDefault();
                if (response == null)
                    return Result<AddressDto>.Fail($"No address with id matching '{id}' was found in the database");

                return Result<AddressDto>.Success(new AddressDto(response));
            }
            return Result<AddressDto>.Fail(result.Messages);
        }

        /// <summary>
        /// Creates a new address associated with the specified parent entity.
        /// </summary>
        /// <remarks>This method creates a new address entity based on the provided <paramref name="dto"/>
        /// and associates it with the specified parent entity identified by <paramref name="parentId"/>. The operation
        /// involves persisting the entity to the underlying data store. If the creation or save operation fails, the
        /// result will contain the corresponding error messages.</remarks>
        /// <param name="parentId">The unique identifier of the parent entity to associate the address with. Cannot be null or empty.</param>
        /// <param name="dto">The data transfer object containing the details of the address to be created. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with the created <see cref="AddressDto"/> if the operation succeeds, or error messages if it fails.</returns>
        public async Task<IBaseResult<AddressDto>> CreateAddress(string parentId, AddressDto dto)
        {
            var addressEntity = dto.ToAddress<TEntity>();
            addressEntity.EntityId = parentId;

            var result = await _repository.CreateAsync(addressEntity);
            if (!result.Succeeded) return await Result<AddressDto>.FailAsync(result.Messages);

            var saveResult = await _repository.SaveAsync();
            if (!saveResult.Succeeded) return await Result<AddressDto>.FailAsync(saveResult.Messages);

            return await Result<AddressDto>.SuccessAsync(dto);
        }

        /// <summary>
        /// Updates an existing address in the database with the provided details.
        /// </summary>
        /// <remarks>The method performs the following steps: <list type="number"> <item>
        /// <description>Finds the address in the database using the <see cref="AddressDto.AddressId"/>
        /// property.</description> </item> <item> <description>Updates the address fields with the values provided in
        /// the <paramref name="address"/> parameter.</description> </item> <item> <description>Saves the changes to the
        /// database.</description> </item> </list> If the address is not found, or if saving the changes fails, the
        /// method returns a failure result with appropriate error messages.</remarks>
        /// <param name="address">An <see cref="AddressDto"/> object containing the updated address details.  The <see
        /// cref="AddressDto.AddressId"/> property must match the ID of an existing address in the database.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is an <see cref="IBaseResult"/>: <list
        /// type="bullet"> <item> <description> If the update is successful, the result contains a success message.
        /// </description> </item> <item> <description> If the update fails, the result contains error messages
        /// describing the failure. </description> </item> </list></returns>
        public async Task<IBaseResult> UpdateAddressAsync(AddressDto address)
        {
            var result = _repository.FindByCondition(c => c.Id.Equals(address.AddressId), false);
            if (result.Succeeded)
            {
                var response = result.Data.FirstOrDefault();
                if (response == null) return await Result<AddressDto>.FailAsync($"No address with id matching '{address.AddressId}' was found in the database");

                response.UnitNumber = address.UnitNumber;
                response.Complex = address.Complex;
                response.StreetNumber = address.StreetNumber;
                response.StreetName = address.StreetName;
                response.Suburb = address.Suburb;
                response.PostalCode = address.PostalCode;
                response.City = address.City;
                response.Province = address.Province;
                response.Country = address.Country;
                response.Latitude = address.Latitude;
                response.Longitude = address.Longitude;
                response.Default = address.Default;
                response.RouteId = address.RouteId;

                var saveResult = await _repository.SaveAsync();
                if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);
                return await Result.SuccessAsync("Address was successfully updated");
            }
            return await Result.FailAsync(result.Messages);
        }

        /// <summary>
        /// Deletes an address with the specified identifier asynchronously.
        /// </summary>
        /// <remarks>This method attempts to locate the address in the database using the provided
        /// identifier. If the address  is found, it is deleted, and the changes are saved to the database. If the
        /// address is not found or the  operation fails, the result will indicate the failure with appropriate error
        /// messages.</remarks>
        /// <param name="addressId">The unique identifier of the address to delete. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the success or failure of the operation. If successful, the result includes a success message; 
        /// otherwise, it includes error messages describing the failure.</returns>
        public async Task<IBaseResult> DeleteAddressAsync(string addressId)
        {
            var result = _repository.FindByCondition(c => c.Id.Equals(addressId));
            if (!result.Succeeded) return await Result.FailAsync(result.Messages);
            var response = result.Data.FirstOrDefault();
            if (response == null)
                return await Result<AddressDto>.FailAsync($"No address with id matching '{addressId}' was found in the database");

            _repository.Delete(response);

            var saveResult = await _repository.SaveAsync();
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);
            return await Result.SuccessAsync("Address was successfully removed");
        }

    }
}