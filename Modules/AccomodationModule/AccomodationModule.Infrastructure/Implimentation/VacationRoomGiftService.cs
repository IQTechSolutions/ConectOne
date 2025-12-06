using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Interfaces;
using AccomodationModule.Domain.RequestFeatures;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;

namespace AccomodationModule.Infrastructure.Implimentation
{
    /// <summary>
    /// Implements the vacation room gift service, providing methods to manage vacation room gift data.
    /// </summary>
    public class VacationRoomGiftService(IAccomodationRepositoryManager accomodationRepositoryManager) : IVacationRoomGiftService
    {
        /// <summary>
        /// Retrieves a list of room gifts associated with a vacation or vacation extension.
        /// </summary>
        /// <remarks>This method queries the repository for room gifts based on the provided vacation or
        /// vacation extension identifiers. If no valid identifiers are provided, the operation will fail. The returned
        /// result indicates whether the operation succeeded and provides the corresponding data or error
        /// messages.</remarks>
        /// <param name="args">The parameters specifying the vacation or vacation extension for which to retrieve room gifts. The <see
        /// cref="VacationIntervalPageParameters.VacationId"/> or <see
        /// cref="VacationIntervalPageParameters.VacationExtensionId"/> must be provided to identify the relevant
        /// vacation context.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>An asynchronous operation that returns a result containing a collection of <see cref="RoomGiftDto"/> objects
        /// representing the room gifts. If the operation fails, the result will include error messages.</returns>
        public async Task<IBaseResult<IEnumerable<RoomGiftDto>>> VacationRoomGiftListAsync(VacationIntervalPageParameters args, CancellationToken cancellationToken = default)
        {
            IBaseResult<List<RoomGift>>? result = null;

            if (string.IsNullOrEmpty(args.VacationId))
            {
                var spec = new LambdaSpec<RoomGift>(c => c.VacationId == args.VacationId);
                result = await accomodationRepositoryManager.Gifts.ListAsync(spec, false, cancellationToken);
            }

            if (result == null) return await Result<IEnumerable<RoomGiftDto>>.FailAsync("There was an error initiating the method, please check your arguments");
            {
                if (result != null && result.Succeeded)
                    return await Result<IEnumerable<RoomGiftDto>>.SuccessAsync(result.Data.Select(c => new RoomGiftDto(c)));
            }
            return await Result<IEnumerable<RoomGiftDto>>.FailAsync(result.Messages);
        }

        /// <summary>
        /// Get a vacation room gift
        /// </summary>
        /// <param name="vacationPricingId">The identity of the vacation room gift to retrieve</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A result indicating the success of the operation.</returns>
        public async Task<IBaseResult<RoomGiftDto>> VacationRoomGiftAsync(string vacationRoomGiftId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<RoomGift>(c => c.Id == vacationRoomGiftId);
            var vacationIntervalResult = await accomodationRepositoryManager.Gifts.FirstOrDefaultAsync(spec, false, cancellationToken);

            if (vacationIntervalResult.Succeeded)
                return await Result<RoomGiftDto>.SuccessAsync(new RoomGiftDto(vacationIntervalResult.Data));
            return await Result<RoomGiftDto>.FailAsync(vacationIntervalResult.Messages);
        }

        /// <summary>
        /// Creates a new vacation room gift.
        /// </summary>
        /// <param name="dto">The data transfer object containing the vacation room gift data.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A result indicating the success of the operation.</returns>
        public async Task<IBaseResult> CreateVacationRoomGiftAsync(RoomGiftDto dto, CancellationToken cancellationToken = default)
        {
            var vacationRoomGift = dto.ToVacationRoomGift();
            await accomodationRepositoryManager.Gifts.CreateAsync(vacationRoomGift, cancellationToken);

            // Save the changes
            var saveResult = await accomodationRepositoryManager.Gifts.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            // Return success message
            return await Result.SuccessAsync($"Vacation gift was created successfully");
        }

        /// <summary>
        /// Updates the vacation room gift information for a specific vacation.
        /// </summary>
        /// <param name="dto">The data transfer object containing the vacation data, including room gift information.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A result indicating the success of the operation.</returns>
        public async Task<IBaseResult> UpdateVacationRoomGiftAsync(RoomGiftDto dto, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<RoomGift>(c => c.Id == dto.RoomGiftId);

            var vacationRoomGiftResult = await accomodationRepositoryManager.Gifts.FirstOrDefaultAsync(spec, true, cancellationToken);
            if (!vacationRoomGiftResult.Succeeded) return await Result.FailAsync(vacationRoomGiftResult.Messages);

            dto.UpdateVacationRoomGiftValues(vacationRoomGiftResult.Data);

            var updateResult = accomodationRepositoryManager.Gifts.Update(vacationRoomGiftResult.Data);
            if (!updateResult.Succeeded) return await Result.FailAsync(updateResult.Messages);

            var saveResult = await accomodationRepositoryManager.Gifts.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync($"Images updated successfully");
        }

        /// <summary>
        /// Removes a vacation room gift.
        /// </summary>
        /// <param name="vacationRoomGiftId">The ID of the vacation room gift to remove.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A result indicating the success of the operation.</returns>
        public async Task<IBaseResult> RemoveVacationRoomGiftAsync(string vacationRoomGiftId, CancellationToken cancellationToken = default)
        {
            var result = await accomodationRepositoryManager.Gifts.DeleteAsync(vacationRoomGiftId, cancellationToken);
            if (!result.Succeeded) await Result.FailAsync(result.Messages);

            return await Result.SuccessAsync($"Vacation room gift with id '{vacationRoomGiftId}' was successfully removed");
        }
    }
}
