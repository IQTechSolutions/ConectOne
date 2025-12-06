using AccomodationModule.Domain.DataTransferObjects;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Domain.Interfaces;

/// <summary>
/// Service contract for managing gifts associated with vacations.
/// </summary>
public interface IGiftService
{
    /// <summary>
    /// Retrieves a list of available gifts.
    /// </summary>
    /// <remarks>The returned collection may be empty if no gifts are available. Ensure to check the result's
    /// status and handle any potential errors as indicated by the <see cref="IBaseResult"/>
    /// implementation.</remarks>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation will terminate early if the token is canceled.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> with
    /// an enumerable collection of <see cref="GiftDto"/> objects representing the available gifts.</returns>
    Task<IBaseResult<IEnumerable<GiftDto>>> GiftListAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a single gift by its identifier.
    /// </summary>
    /// <param name="giftId">The ID of the gift to retrieve.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A result containing the requested <see cref="GiftDto"/>.</returns>
    Task<IBaseResult<GiftDto>> GiftAsync(string giftId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new gift.
    /// </summary>
    /// <param name="dto">The gift data.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<IBaseResult> CreateGiftAsync(GiftDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing gift.
    /// </summary>
    /// <param name="dto">The updated gift data.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<IBaseResult> UpdateGiftAsync(GiftDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a gift by its identifier.
    /// </summary>
    /// <param name="giftId">The ID of the gift to remove.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<IBaseResult> RemoveGiftAsync(string giftId, CancellationToken cancellationToken = default);
}
