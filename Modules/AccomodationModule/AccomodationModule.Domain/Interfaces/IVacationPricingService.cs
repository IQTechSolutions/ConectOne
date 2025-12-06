using AccomodationModule.Domain.DataTransferObjects;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Domain.Interfaces
{
    /// <summary>
    /// Implements the vacation pricing service, providing methods to manage vacation pricing data.
    /// </summary>
    public interface IVacationPricingService
    {
        /// <summary>
        /// Retrieves pricing information for a specific vacation package.
        /// </summary>
        /// <remarks>Use this method to fetch detailed pricing information for a vacation package,
        /// including any applicable discounts or fees. The result may include multiple pricing items if the vacation
        /// package has variable pricing options.</remarks>
        /// <param name="vacationId">The unique identifier of the vacation package. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// with a collection of <see cref="VacationPricingItemDto"/> objects representing the pricing details for the
        /// vacation package.</returns>
        Task<IBaseResult<IEnumerable<VacationPricingItemDto>>> VacationPricesAsync(string vacationId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a vacation price flight
        /// </summary>
        /// <param name="vacationPricingId">The identity of the vacation price to retrieve</param>
        /// <returns>A result indicating the success of the operation.</returns>
        Task<IBaseResult<VacationPricingItemDto>> VacationPriceAsync(string vacationPricingId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new vacation price.
        /// </summary>
        /// <param name="dto">The data transfer object containing the vacation price data.</param>
        /// <returns>A result indicating the success of the operation.</returns>
        Task<IBaseResult> CreateVacationPriceAsync(VacationPricingItemDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update a new vacation price.
        /// </summary>
        /// <param name="dto">The data transfer object containing the vacation price data.</param>
        /// <returns>A result indicating the success of the operation.</returns>
        Task<IBaseResult> UpdateVacationPriceAsync(VacationPricingItemDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new vacation pricing display type info section.
        /// </summary>
        /// <param name="dto">The data transfer object containing the vacation pricing display type info section data.</param>
        /// <returns>A result indicating the success of the operation.</returns>
        Task<IBaseResult> UpdateVacationPriceDisplayOrderAsync(VacationPricingItemGroupUpdateRequest dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a vacation price.
        /// </summary>
        /// <param name="vacationPricingItemId">The ID of the vacation price to remove.</param>
        /// <returns>A result indicating the success of the operation.</returns>
        Task<IBaseResult> RemoveVacationPriceAsync(string vacationPricingItemId, CancellationToken cancellationToken = default);

        #region MyRegion

        /// <summary>
        /// Retrieves a collection of vacation pricing groups asynchronously.
        /// </summary>
        /// <remarks>Use this method to fetch pricing group data for vacations. The result may include
        /// multiple pricing groups, or an empty collection if no groups are available. Ensure proper handling of the
        /// <see cref="IBaseResult{T}"/> to check for success or failure of the operation.</remarks>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation will be canceled if the token is triggered.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with an enumerable collection of <see cref="VacationPriceGroupDto"/> objects representing the vacation
        /// pricing groups.</returns>
        Task<IBaseResult<IEnumerable<VacationPriceGroupDto>>> VacationPricingGroupsAsync(string vacationId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new vacation pricing group asynchronously.
        /// </summary>
        /// <param name="dto">The data transfer object containing the details of the vacation pricing group to create.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with the created <see cref="VacationPriceGroupDto"/> if the operation is successful.</returns>
        Task<IBaseResult<VacationPriceGroupDto>> CreateVacationPricingGroupAsync(VacationPriceGroupDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing vacation pricing group with the provided data.
        /// </summary>
        /// <param name="dto">The data transfer object containing the updated details of the vacation pricing group.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with the updated <see cref="VacationPriceGroupDto"/> if the operation is successful.</returns>
        Task<IBaseResult<VacationPriceGroupDto>> UpdateVacationPricingGroupAsync(VacationPriceGroupDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a vacation pricing group identified by the specified group ID.
        /// </summary>
        /// <remarks>Use this method to remove a vacation pricing group from the system. Ensure that the
        /// specified  <paramref name="groupId"/> corresponds to an existing group. The operation is asynchronous and 
        /// can be canceled by passing a cancellation token.</remarks>
        /// <param name="groupId">The unique identifier of the vacation pricing group to be removed.  This value cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the success or failure of the operation.</returns>
        Task<IBaseResult> RemoveVacationPricingGroupAsync(string groupId, CancellationToken cancellationToken = default);

        #endregion

        #region Payment Schedule Entry

        /// <summary>
        /// Retrieves all payment schedule entries for a specific vacation.
        /// </summary>
        /// <param name="vacationId">The ID of the vacation.</param>
        /// <returns>A result containing a collection of payment schedule entries.</returns>
        Task<IBaseResult<IEnumerable<PaymentRuleDto>>> PaymentScheduleEntriesAsync(string vacationId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new vacation payment schedule entry.
        /// </summary>
        /// <param name="dto">The data transfer object containing the vacation price data.</param>
        /// <returns>A result indicating the success of the operation.</returns>
        Task<IBaseResult> CreatePaymentScheduleEntryAsync(PaymentRuleDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing Payment Schedule Entry.
        /// </summary>
        /// <param name="dto">The data transfer object containing the vacation Payment Schedule Entry type info section data.</param>
        /// <returns>A result indicating the success of the operation.</returns>
        Task<IBaseResult> UpdatePaymentScheduleEntryAsync(PaymentRuleDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a Payment Schedule Entry.
        /// </summary>
        /// <param name="paymentRuleId">The ID of the Payment Schedule Entry to remove.</param>
        /// <returns>A result indicating the success of the operation.</returns>
        Task<IBaseResult> RemovePaymentScheduleEntryAsync(string paymentRuleId, CancellationToken cancellationToken = default);

        #endregion
    }
}
