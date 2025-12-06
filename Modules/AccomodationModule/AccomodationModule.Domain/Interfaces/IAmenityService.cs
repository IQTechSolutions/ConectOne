using AccomodationModule.Domain.DataTransferObjects;
using ConectOne.Domain.RequestFeatures;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Domain.Interfaces
{
	/// <summary>
	/// Defines a contract for managing amenities and their associations with lodgings and rooms.
	/// </summary>
	/// <remarks>This service provides methods for retrieving, creating, updating, and deleting amenities,  as well
	/// as managing their relationships with lodgings and rooms. It supports paginated  retrieval of amenities and offers
	/// operations to associate or disassociate amenities with  specific lodgings or rooms.</remarks>
    public interface IAmenityService
    {
		/// <summary>
		/// Retrieves a paginated list of amenities based on the specified request parameters.
		/// </summary>
		/// <remarks>The method supports pagination and filtering as defined by the <paramref name="parameters"/>.
		/// Ensure that the parameters are valid to avoid unexpected results.</remarks>
		/// <param name="parameters">The parameters that define the pagination and filtering options for the request.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="PaginatedResult{T}"/> containing a collection of <see cref="AmenityDto"/> objects that match the
		/// specified criteria.</returns>
		Task<PaginatedResult<AmenityDto>> PagedAmenitiesAsync(RequestParameters parameters, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieves a collection of all available amenities.
		/// </summary>
		/// <remarks>The returned collection may be empty if no amenities are available. Ensure to check the result's
		/// status and handle any potential errors as indicated by the <see cref="IBaseResult{T}"/> implementation.</remarks>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
		/// wrapping an <see cref="IEnumerable{T}"/> of <see cref="AmenityDto"/> objects representing the amenities.</returns>
        Task<IBaseResult<IEnumerable<AmenityDto>>> AllAmenitiesAsync(CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieves a collection of amenities associated with the specified lodging.
		/// </summary>
		/// <remarks>This method performs an asynchronous operation to fetch the amenities for a given lodging.  The
		/// result includes details about each amenity, such as its name and description, encapsulated in <see
		/// cref="AmenityDto"/> objects.</remarks>
		/// <param name="lodgingId">The unique identifier of the lodging for which amenities are to be retrieved. Cannot be null or empty.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
		/// containing an enumerable collection of <see cref="AmenityDto"/> objects representing the amenities of the lodging.</returns>
        Task<IBaseResult<IEnumerable<AmenityDto>>> LodgingAmenitiesAsync(string lodgingId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieves the details of a specific amenity by its unique identifier.
		/// </summary>
		/// <param name="amenityId">The unique identifier of the amenity to retrieve.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> object
		/// wrapping the <see cref="AmenityDto"/> for the specified amenity.</returns>
		Task<IBaseResult<AmenityDto>> AmenityAsync(int amenityId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Creates a new amenity based on the provided data.
		/// </summary>
		/// <remarks>The operation may fail if the provided amenity data is invalid or if a conflict occurs during
		/// creation. Ensure that the <paramref name="amenity"/> object contains all required fields.</remarks>
		/// <param name="amenity">The data for the amenity to be created. Must not be <see langword="null"/>.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> object
		/// that includes the created amenity as an <see cref="AmenityDto"/> or details about the operation's outcome.</returns>
		Task<IBaseResult<AmenityDto>> CreateAmenity(AmenityDto amenity, CancellationToken cancellationToken = default);

		/// <summary>
		/// Updates the details of an existing amenity.
		/// </summary>
		/// <remarks>Use this method to update the details of an amenity in the system. Ensure that the provided <see
		/// cref="AmenityDto"/> contains valid data. The operation may fail if the amenity does not exist or if the provided
		/// data is invalid.</remarks>
		/// <param name="amenity">The <see cref="AmenityDto"/> object containing the updated details of the amenity. Cannot be <see
		/// langword="null"/>.</param>
		/// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation. The default value is <see
		/// cref="CancellationToken.None"/>.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
		/// indicating the outcome of the update operation.</returns>
		Task<IBaseResult<AmenityDto>> UpdateAmenity(AmenityDto amenity, CancellationToken cancellationToken = default);

		/// <summary>
		/// Removes an amenity with the specified identifier.
		/// </summary>
		/// <param name="amenityId">The unique identifier of the amenity to remove.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
		/// indicating the outcome of the operation.</returns>
		Task<IBaseResult> RemoveAmentity(int amenityId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Adds a lodging amenity item to the specified lodging.
		/// </summary>
		/// <param name="amentityId">The unique identifier of the amenity to be added.</param>
		/// <param name="lodgingId">The unique identifier of the lodging to which the amenity will be added. Cannot be null or empty.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
		/// indicating the outcome of the operation.</returns>
		Task<IBaseResult> AddLodgingAmenityItem(int amentityId, string lodgingId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Removes a lodging amenity item associated with the specified lodging.
		/// </summary>
		/// <remarks>The operation will fail if the specified amenity or lodging does not exist. Ensure that both
		/// identifiers  are valid before calling this method.</remarks>
		/// <param name="amentityId">The unique identifier of the amenity to be removed.</param>
		/// <param name="lodgingId">The unique identifier of the lodging from which the amenity will be removed.</param>
		/// <param name="cancellationToken">An optional token to monitor for cancellation requests.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
		/// indicating the outcome of the operation.</returns>
		Task<IBaseResult> RemoveLodgingAmenityItem(int amentityId, string lodgingId, CancellationToken cancellationToken = default);

        /// <summary>
		/// Associates a specified amenity with a room.
		/// </summary>
		/// <remarks>This method adds an amenity to a room, establishing a relationship between the two entities. 
		/// Ensure that both the amenity and room identifiers are valid and exist in the system.</remarks>
		/// <param name="amentityId">The unique identifier of the amenity to be added.</param>
		/// <param name="roomId">The unique identifier of the room to which the amenity will be added.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
		/// indicating the outcome of the operation.</returns>
		Task<IBaseResult> AddRoomAmenityItem(int amentityId, int roomId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Removes the specified amenity from the given room.
		/// </summary>
		/// <remarks>The operation will fail if the specified amenity does not exist in the room or if the identifiers
		/// are invalid.</remarks>
		/// <param name="amentityId">The unique identifier of the amenity to be removed.</param>
		/// <param name="roomId">The unique identifier of the room from which the amenity will be removed.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
		/// indicating the success or failure of the operation.</returns>
		Task<IBaseResult> RemoveRoomAmenityItem(int amentityId, int roomId, CancellationToken cancellationToken = default);
	}
}
