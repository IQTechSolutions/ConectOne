using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;

namespace AccomodationModule.Infrastructure.Implimentation;

/// <summary>
/// Implements gift related operations.
/// </summary>
public class GiftService(IRepository<Gift, string> giftRepo) : IGiftService
{
    /// <summary>
    /// Retrieves a list of gifts as a collection of <see cref="GiftDto"/> objects.
    /// </summary>
    /// <remarks>This method queries the underlying data source for all available gifts and maps them to  <see
    /// cref="GiftDto"/> objects. The operation supports cancellation via the provided  <paramref
    /// name="cancellationToken"/>.</remarks>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult"/>
    /// where <c>T</c> is an <see cref="IEnumerable{T}"/> of <see cref="GiftDto"/>. If the operation succeeds, the
    /// result contains the list of gifts; otherwise, it contains error messages.</returns>
    public async Task<IBaseResult<IEnumerable<GiftDto>>> GiftListAsync(CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<Gift>(g => true);
        var result = await giftRepo.ListAsync(spec, false, cancellationToken);

        if (result.Succeeded)
            return await Result<IEnumerable<GiftDto>>.SuccessAsync(result.Data.Select(g => new GiftDto(g)));
        return await Result<IEnumerable<GiftDto>>.FailAsync(result.Messages);
    }

    /// <summary>
    /// Retrieves a gift by its unique identifier and returns the result as a data transfer object (DTO).
    /// </summary>
    /// <param name="giftId">The unique identifier of the gift to retrieve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an  IBaseResult{T} of type GiftDto.
    /// If the operation succeeds, the result  contains the gift data; otherwise, it contains error messages.</returns>
    public async Task<IBaseResult<GiftDto>> GiftAsync(string giftId, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<Gift>(g => g.Id == giftId);
        var result = await giftRepo.FirstOrDefaultAsync(spec, false, cancellationToken);

        if (result.Succeeded)
            return await Result<GiftDto>.SuccessAsync(new GiftDto(result.Data!));
        return await Result<GiftDto>.FailAsync(result.Messages);
    }

    /// <summary>
    /// Creates a new gift based on the provided data transfer object (DTO) and saves it to the repository.
    /// </summary>
    /// <remarks>This method performs the following steps: <list type="number"> <item>Converts the provided
    /// <paramref name="dto"/> into a gift entity.</item> <item>Attempts to create the gift in the repository.</item>
    /// <item>If the creation succeeds, attempts to save the changes to the repository.</item> <item>Returns a success
    /// result if all operations succeed, or a failure result with error messages if any step fails.</item>
    /// </list></remarks>
    /// <param name="dto">The data transfer object containing the details of the gift to be created.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the outcome of the operation. If successful, the result includes a success message; otherwise, it
    /// contains error messages describing the failure.</returns>
    public async Task<IBaseResult> CreateGiftAsync(GiftDto dto, CancellationToken cancellationToken = default)
    {
        var gift = dto.ToGift();
        var createResult = await giftRepo.CreateAsync(gift, cancellationToken);
        if (!createResult.Succeeded) return await Result.FailAsync(createResult.Messages);

        var saveResult = await giftRepo.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

        return await Result.SuccessAsync("Gift was created successfully");
    }

    /// <summary>
    /// Updates an existing gift with the specified values.
    /// </summary>
    /// <remarks>This method performs the following steps: <list type="number"> <item>Retrieves the gift
    /// specified by <paramref name="dto"/> using its <see cref="GiftDto.GiftId"/>.</item> <item>Updates the gift's
    /// values based on the provided <paramref name="dto"/>.</item> <item>Attempts to save the changes to the
    /// repository.</item> </list> If any step fails, the method returns a failure result with the corresponding error
    /// messages.</remarks>
    /// <param name="dto">The data transfer object containing the updated values for the gift. The <see cref="GiftDto.GiftId"/> property
    /// must correspond to an existing gift.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the operation. If the operation succeeds, the result contains a success
    /// message. If it fails, the result contains error messages.</returns>
    public async Task<IBaseResult> UpdateGiftAsync(GiftDto dto, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<Gift>(g => g.Id == dto.GiftId);
        var result = await giftRepo.FirstOrDefaultAsync(spec, true, cancellationToken);
        if (!result.Succeeded) return await Result.FailAsync(result.Messages);

        dto.UpdateGiftValues(result.Data!);
        var updateResult = giftRepo.Update(result.Data!);
        if (!updateResult.Succeeded) return await Result.FailAsync(updateResult.Messages);

        var saveResult = await giftRepo.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

        return await Result.SuccessAsync("Gift updated successfully");
    }

    /// <summary>
    /// Removes a gift with the specified identifier asynchronously.
    /// </summary>
    /// <remarks>If the operation is successful, the result will indicate success and include a message
    /// confirming the removal of the gift. If the operation fails, the result will indicate failure and include the
    /// relevant error messages.</remarks>
    /// <param name="giftId">The unique identifier of the gift to be removed. Cannot be <see langword="null"/> or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the outcome of the operation.</returns>
    public async Task<IBaseResult> RemoveGiftAsync(string giftId, CancellationToken cancellationToken = default)
    {
        var result = await giftRepo.DeleteAsync(giftId, cancellationToken);
        if (!result.Succeeded) await Result.FailAsync(result.Messages);

        return await Result.SuccessAsync($"Gift with id '{giftId}' was successfully removed");
    }
}
