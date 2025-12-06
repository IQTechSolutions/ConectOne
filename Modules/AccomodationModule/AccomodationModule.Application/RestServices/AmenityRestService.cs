using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.RequestFeatures;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Application.RestServices
{
    /// <summary>
    /// Provides a set of methods for managing amenities, including retrieving, creating, updating,  and deleting
    /// amenities, as well as associating amenities with lodgings and rooms.
    /// </summary>
    /// <remarks>This service acts as a REST client for interacting with the "amenities" endpoint and related 
    /// operations. It supports asynchronous operations and cancellation tokens for improved responsiveness  and
    /// control. The service is designed to handle common amenity-related tasks, such as retrieving  paginated results,
    /// managing associations with lodgings and rooms, and performing CRUD operations  on amenities.</remarks>
    /// <param name="provider"></param>
    public class AmenityRestService(IBaseHttpProvider provider) : IAmenityService
    {
        /// <summary>
        /// Retrieves a paginated list of amenities based on the specified request parameters.
        /// </summary>
        /// <remarks>This method asynchronously retrieves data from the "amenities" endpoint and supports
        /// cancellation through the provided <paramref name="cancellationToken"/>.</remarks>
        /// <param name="parameters">The parameters that define the pagination and filtering options for the request.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a collection of <see cref="AmenityDto"/> objects and
        /// pagination metadata.</returns>
        public async Task<PaginatedResult<AmenityDto>> PagedAmenitiesAsync(RequestParameters parameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetPagedAsync<AmenityDto, RequestParameters>("amenities", parameters);
            return result;
        }

        /// <summary>
        /// Retrieves a collection of all available amenities.
        /// </summary>
        /// <remarks>This method fetches all amenities from the underlying data provider. The caller can
        /// use the <paramref name="cancellationToken"/> to cancel the operation if needed.</remarks>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// wrapping an <see cref="IEnumerable{T}"/> of <see cref="AmenityDto"/> objects representing the amenities.</returns>
        public async Task<IBaseResult<IEnumerable<AmenityDto>>> AllAmenitiesAsync(CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<AmenityDto>>("amenities/all");
            return result;
        }

        /// <summary>
        /// Retrieves a collection of amenities associated with the specified lodging.
        /// </summary>
        /// <remarks>This method asynchronously retrieves the amenities for the specified lodging by
        /// making a call to the underlying provider. Ensure that the <paramref name="lodgingId"/> is valid and
        /// corresponds to an existing lodging.</remarks>
        /// <param name="lodgingId">The unique identifier of the lodging for which to retrieve amenities.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// containing an enumerable collection of <see cref="AmenityDto"/> objects representing the amenities.</returns>
        public async Task<IBaseResult<IEnumerable<AmenityDto>>> LodgingAmenitiesAsync(string lodgingId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<AmenityDto>>($"amenities/children/{lodgingId}");
            return result;
        }

        /// <summary>
        /// Retrieves the details of an amenity by its unique identifier.
        /// </summary>
        /// <remarks>This method sends a request to retrieve the amenity details from the underlying data
        /// provider. Ensure that the <paramref name="amenityId"/> corresponds to a valid amenity.</remarks>
        /// <param name="amenityId">The unique identifier of the amenity to retrieve.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the details of the amenity as an <see cref="AmenityDto"/>.</returns>
        public async Task<IBaseResult<AmenityDto>> AmenityAsync(int amenityId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<AmenityDto>($"amenities/{amenityId}");
            return result;
        }

        /// <summary>
        /// Creates a new amenity by sending the provided data to the server.
        /// </summary>
        /// <remarks>This method sends a PUT request to the server to create the specified amenity. Ensure
        /// that the provided <paramref name="amenity"/> contains valid data as required by the server.</remarks>
        /// <param name="amenity">The amenity data to be created. This must not be <see langword="null"/>.</param>
        /// <param name="cancellationToken">An optional token to monitor for cancellation requests. The default value is <see
        /// cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that includes the created amenity data.</returns>
        public async Task<IBaseResult<AmenityDto>> CreateAmenity(AmenityDto amenity, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<AmenityDto, AmenityDto>($"amenities", amenity);
            return result;
        }

        /// <summary>
        /// Updates an existing amenity with the specified details.
        /// </summary>
        /// <remarks>This method sends a POST request to update the amenity details. Ensure that the
        /// provided <paramref name="amenity"/> object contains valid data before calling this method.</remarks>
        /// <param name="amenity">The <see cref="AmenityDto"/> object containing the updated details of the amenity.</param>
        /// <param name="cancellationToken">An optional <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult<AmenityDto>> UpdateAmenity(AmenityDto amenity, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<AmenityDto, AmenityDto>($"amenities", amenity);
            return result;
        }

        /// <summary>
        /// Removes an amenity with the specified identifier.
        /// </summary>
        /// <remarks>This method sends a delete request to the underlying provider to remove the specified
        /// amenity. Ensure that the <paramref name="amenityId"/> corresponds to an existing amenity.</remarks>
        /// <param name="amenityId">The unique identifier of the amenity to be removed.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the removal operation.</returns>
        public async Task<IBaseResult> RemoveAmentity(int amenityId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"amenities/{amenityId}", "");
            return result;
        }

        /// <summary>
        /// Associates an amenity with a lodging entity.
        /// </summary>
        /// <remarks>This method sends a request to associate the specified amenity with the given lodging
        /// entity. Ensure that both the <paramref name="amentityId"/> and <paramref name="lodgingId"/> are valid and
        /// exist in the system.</remarks>
        /// <param name="amentityId">The unique identifier of the amenity to be added.</param>
        /// <param name="lodgingId">The unique identifier of the lodging entity to which the amenity will be associated.</param>
        /// <param name="cancellationToken">An optional token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> AddLodgingAmenityItem(int amentityId, string lodgingId, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"addEntityAmenity", new AddOwnerAmenityModalViewModel(){ AmenityId = amentityId, LodgingId = lodgingId});
            return result;
        }

        /// <summary>
        /// Removes a lodging amenity item associated with the specified amenity ID and lodging ID.
        /// </summary>
        /// <remarks>This method sends a request to remove the specified amenity from the lodging. Ensure
        /// that the provided  <paramref name="amentityId"/> and <paramref name="lodgingId"/> are valid and exist in the
        /// system.</remarks>
        /// <param name="amentityId">The unique identifier of the amenity to be removed.</param>
        /// <param name="lodgingId">The unique identifier of the lodging from which the amenity will be removed.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveLodgingAmenityItem(int amentityId, string lodgingId, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"lodgings/children/{amentityId}/{lodgingId}", "");
            return result;
        }

        /// <summary>
        /// Associates an amenity with a specific room.
        /// </summary>
        /// <remarks>This method sends a request to associate the specified amenity with the specified
        /// room.  Ensure that both the amenity and room identifiers are valid before calling this method.</remarks>
        /// <param name="amentityId">The unique identifier of the amenity to be added.</param>
        /// <param name="roomId">The unique identifier of the room to which the amenity will be added.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> AddRoomAmenityItem(int amentityId, int roomId, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"addEntityAmenity", new AddOwnerAmenityModalViewModel() { AmenityId = amentityId, RoomId = roomId.ToString() });
            return result;
        }

        /// <summary>
        /// Removes the specified amenity from the specified room.
        /// </summary>
        /// <remarks>This method sends a request to remove the specified amenity from the specified room.
        /// Ensure that the  provided identifiers are valid and that the operation is permitted for the current
        /// user.</remarks>
        /// <param name="amentityId">The unique identifier of the amenity to be removed.</param>
        /// <param name="roomId">The unique identifier of the room from which the amenity will be removed.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveRoomAmenityItem(int amentityId, int roomId, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"rooms/children/{amentityId}/{roomId}", "");
            return result;
        }
    }
}
