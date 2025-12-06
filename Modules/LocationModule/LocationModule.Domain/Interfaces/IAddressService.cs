using ConectOne.Domain.DataTransferObjects;
using ConectOne.Domain.Enums;
using ConectOne.Domain.ResultWrappers;

namespace LocationModule.Domain.Interfaces
{
    /// <summary>
    /// Defines a contract for managing address-related operations for a specific entity type.
    /// </summary>
    /// <remarks>This interface provides methods for retrieving, creating, updating, and deleting addresses, 
    /// as well as retrieving addresses by specific criteria such as parent entity, route, or address type.</remarks>
    /// <typeparam name="TEntity">The type of the entity associated with the addresses.</typeparam>
    public interface IAddressService<TEntity>
    {
        /// <summary>
        /// Retrieves all addresses associated with the specified parent identifier.
        /// </summary>
        /// <param name="parentId">The unique identifier of the parent entity whose addresses are to be retrieved. Cannot be null or empty.</param>
        /// <param name="routId">An optional route identifier to filter the addresses. If null, no route-based filtering is applied.</param>
        /// <param name="trackChanges">A boolean value indicating whether to track changes to the retrieved addresses.  <see langword="true"/> to
        /// enable change tracking; otherwise, <see langword="false"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// with a collection of <see cref="AddressDto"/> objects representing the retrieved addresses.</returns>
        Task<IBaseResult<IEnumerable<AddressDto>>> GetAllAddressesAsync(string parentId, string? routId = null, bool trackChanges = false);

        /// <summary>
        /// Retrieves all addresses of a specified type associated with a given parent identifier.
        /// </summary>
        /// <param name="parentId">The unique identifier of the parent entity for which addresses are being retrieved.  Cannot be <see
        /// langword="null"/> or empty.</param>
        /// <param name="addressType">The type of addresses to retrieve. Must be a valid <see cref="AddressType"/> value.</param>
        /// <param name="trackChanges">A value indicating whether the retrieved addresses should be tracked for changes.  <see langword="true"/> to
        /// enable change tracking; otherwise, <see langword="false"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult{T}"/>
        /// where <c>T</c> is an <see cref="IEnumerable{T}"/> of  <see cref="AddressDto"/> objects representing the
        /// retrieved addresses. If no addresses  are found, the result will contain an empty collection.</returns>
        Task<IBaseResult<IEnumerable<AddressDto>>> GetAllAddressesByTypeAsync(string parentId, AddressType addressType, bool trackChanges);

        /// <summary>
        /// Asynchronously retrieves all addresses associated with the specified route.
        /// </summary>
        /// <param name="routeId">The unique identifier of the route for which to retrieve addresses. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with a collection of <see cref="AddressDto"/> objects representing the addresses for the specified route.</returns>
        Task<IBaseResult<IEnumerable<AddressDto>>> GetAllRouteAddressesAsync(string routeId);

        /// <summary>
        /// Retrieves all route addresses associated with a specific entity and route.
        /// </summary>
        /// <remarks>This method is asynchronous and should be awaited. The returned collection will be
        /// empty if no addresses are associated with the specified entity and route.</remarks>
        /// <param name="parentId">The unique identifier of the parent entity. This value cannot be <see langword="null"/> or empty.</param>
        /// <param name="routeId">The unique identifier of the route. This value cannot be <see langword="null"/> or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with an enumerable collection of <see cref="AddressDto"/> objects representing the route addresses.</returns>
        Task<IBaseResult<IEnumerable<AddressDto>>> GetAllEntityRouteAddressesAsync(string parentId, string routeId);

        /// <summary>
        /// Asynchronously retrieves the address details for the specified identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the address to retrieve. Cannot be null or empty.</param>
        /// <param name="trackChanges">A boolean value indicating whether to track changes to the retrieved address entity. <see langword="true"/>
        /// to enable change tracking; otherwise, <see langword="false"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult{T}"/>
        /// of type <see cref="AddressDto"/> with the address details.</returns>
        Task<IBaseResult<AddressDto>> GetAddressAsync(string id, bool trackChanges);

        /// <summary>
        /// Creates a new address associated with the specified parent entity.
        /// </summary>
        /// <param name="parentId">The unique identifier of the parent entity to which the address will be associated. Cannot be null or empty.</param>
        /// <param name="dto">The data transfer object containing the details of the address to be created. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the created <see cref="AddressDto"/> if the operation is successful.</returns>
        Task<IBaseResult<AddressDto>> CreateAddress(string parentId, AddressDto dto);

        /// <summary>
        /// Updates the address information for a user asynchronously.
        /// </summary>
        /// <remarks>Ensure that the <paramref name="address"/> parameter contains all required fields and
        /// valid data  before calling this method. The operation may fail if the provided address is invalid or if
        /// there  are issues with the underlying data store.</remarks>
        /// <param name="address">The address details to update. Must not be null and should contain valid address information.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the success or failure of the update operation.</returns>
        Task<IBaseResult> UpdateAddressAsync(AddressDto address);

        /// <summary>
        /// Deletes the address associated with the specified address ID asynchronously.
        /// </summary>
        /// <remarks>Ensure that the provided <paramref name="addressId"/> corresponds to a valid address
        /// in the system.  The operation may fail if the address does not exist or if there are restrictions on its
        /// deletion.</remarks>
        /// <param name="addressId">The unique identifier of the address to delete. This value cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the success or failure of the operation.</returns>
        Task<IBaseResult> DeleteAddressAsync(string addressId);
    }
}
