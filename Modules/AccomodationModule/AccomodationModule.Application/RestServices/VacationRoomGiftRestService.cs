using AccomodationModule.Domain.Arguments;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using AccomodationModule.Domain.RequestFeatures;
using ConectOne.Domain.Extensions;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Application.RestServices
{
    /// <summary>
    /// Provides RESTful operations for managing vacation room gifts, including retrieving, creating, updating, and
    /// deleting room gifts.
    /// </summary>
    /// <remarks>This service interacts with a REST API to perform operations related to vacation room gifts.
    /// It supports asynchronous methods for retrieving lists of room gifts, fetching details of a specific gift,
    /// creating or updating gifts, and removing gifts.  Ensure that the provided parameters are valid and meet the
    /// requirements of the respective methods.</remarks>
    /// <param name="provider"></param>
    public class VacationRoomGiftRestService(IBaseHttpProvider provider) : IVacationRoomGiftService
    {
        /// <summary>
        /// Retrieves a list of room gifts associated with a vacation interval.
        /// </summary>
        /// <remarks>The returned list may be empty if no room gifts are associated with the specified
        /// vacation interval.</remarks>
        /// <param name="args">The parameters specifying the vacation interval and pagination details.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  IBaseResult{T} of
        /// IEnumerable{RoomGiftDto} representing the room gifts.</returns>
        public async Task<IBaseResult<IEnumerable<RoomGiftDto>>> VacationRoomGiftListAsync(VacationIntervalPageParameters args, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<RoomGiftDto>>($"vacations/vacationRoomGifts?{args.GetQueryString()}");
            return result;
        }

        /// <summary>
        /// Retrieves the details of a vacation room gift based on the specified identifier.
        /// </summary>
        /// <remarks>This method sends a request to retrieve the vacation room gift details from the
        /// provider. Ensure that the <paramref name="vacationRoomGiftId"/> is valid and corresponds to an existing
        /// vacation room gift.</remarks>
        /// <param name="vacationRoomGiftId">The unique identifier of the vacation room gift to retrieve. This value cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the details of the vacation room gift as a <see cref="RoomGiftDto"/>.</returns>
        public async Task<IBaseResult<RoomGiftDto>> VacationRoomGiftAsync(string vacationRoomGiftId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<RoomGiftDto>($"vacations/vacationRoomGifts/{vacationRoomGiftId}");
            return result;
        }

        /// <summary>
        /// Creates or updates a vacation room gift using the provided data transfer object (DTO).
        /// </summary>
        /// <remarks>This method sends a PUT request to the "vacations/vacationRoomGifts" endpoint with
        /// the provided DTO. Ensure that the <paramref name="dto"/> contains valid data before calling this
        /// method.</remarks>
        /// <param name="dto">The data transfer object containing the details of the vacation room gift to be created or updated.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> CreateVacationRoomGiftAsync(RoomGiftDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync($"vacations/vacationRoomGifts", dto);
            return result;
        }

        /// <summary>
        /// Updates the vacation room gift information asynchronously.
        /// </summary>
        /// <remarks>This method sends the updated vacation room gift details to the server. Ensure that
        /// the <paramref name="dto"/> parameter contains valid data before calling this method.</remarks>
        /// <param name="dto">The data transfer object containing the updated vacation room gift details.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateVacationRoomGiftAsync(RoomGiftDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"vacations/vacationRoomGifts", dto);
            return result;
        }

        /// <summary>
        /// Removes a vacation room gift identified by the specified ID.
        /// </summary>
        /// <remarks>This method sends a request to delete the specified vacation room gift. Ensure the
        /// provided ID is valid  and corresponds to an existing gift.</remarks>
        /// <param name="vacationRoomGiftId">The unique identifier of the vacation room gift to be removed. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveVacationRoomGiftAsync(string vacationRoomGiftId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"vacations/vacationRoomGifts", vacationRoomGiftId);
            return result;
        }
    }
}