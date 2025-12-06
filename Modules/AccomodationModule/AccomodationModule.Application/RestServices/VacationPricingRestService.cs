using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Application.RestServices
{
    /// <summary>
    /// Provides methods for managing vacation pricing, pricing groups, and payment schedules.
    /// </summary>
    /// <remarks>This service acts as a REST-based interface for interacting with vacation pricing data,
    /// including retrieving, creating, updating, and deleting vacation pricing items, pricing groups, and payment
    /// schedule entries. It communicates with an underlying HTTP provider to perform these operations.</remarks>
    public class VacationPricingRestService(IBaseHttpProvider provider) : IVacationPricingService
    {
        /// <summary>
        /// Retrieves the pricing details for a specific vacation.
        /// </summary>
        /// <remarks>This method fetches pricing details for a vacation by its unique identifier. The
        /// caller can use the <paramref name="cancellationToken"/> to cancel the operation if needed.</remarks>
        /// <param name="vacationId">The unique identifier of the vacation for which pricing details are requested. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// of <see cref="IEnumerable{VacationPricingItemDto}"/> representing the pricing details for the specified
        /// vacation.</returns>
        public async Task<IBaseResult<IEnumerable<VacationPricingItemDto>>> VacationPricesAsync(string vacationId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<VacationPricingItemDto>>($"vacations/vacationPrices/{vacationId}");
            return result;
        }

        /// <summary>
        /// Retrieves the pricing details for a specific vacation package.
        /// </summary>
        /// <remarks>This method sends a request to the underlying provider to fetch the pricing details
        /// for the specified vacation package. Ensure that the <paramref name="vacationPricingId"/> is valid and
        /// corresponds to an existing vacation pricing item.</remarks>
        /// <param name="vacationPricingId">The unique identifier of the vacation pricing item to retrieve. This value cannot be <see langword="null"/>
        /// or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping a <see cref="VacationPricingItemDto"/> with the pricing details for the specified vacation
        /// package.</returns>
        public async Task<IBaseResult<VacationPricingItemDto>> VacationPriceAsync(string vacationPricingId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<VacationPricingItemDto>($"vacations/vacationPrices/price/{vacationPricingId}");
            return result;
        }

        /// <summary>
        /// Creates or updates a vacation pricing item asynchronously.
        /// </summary>
        /// <remarks>This method sends the provided vacation pricing details to the server for creation or
        /// update. Ensure that the <paramref name="dto"/> parameter contains valid data before calling this
        /// method.</remarks>
        /// <param name="dto">The data transfer object containing the vacation pricing details to be created or updated.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> CreateVacationPriceAsync(VacationPricingItemDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<VacationPricingItemDto, VacationPricingItemDto>($"vacations/vacationPrices", dto);
            return result;
        }

        /// <summary>
        /// Updates the vacation price using the provided pricing details.
        /// </summary>
        /// <remarks>This method sends the provided vacation pricing details to the server for updating.
        /// Ensure that the <paramref name="dto"/> parameter contains valid data before calling this method.</remarks>
        /// <param name="dto">The data transfer object containing the vacation pricing details to be updated.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateVacationPriceAsync(VacationPricingItemDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"vacations/vacationPrices", dto);
            return result;
        }
        
        /// <summary>
        /// Updates the display order of vacation pricing items based on the provided update request.
        /// </summary>
        /// <remarks>This method sends the update request to the underlying provider to modify the display
        /// order of vacation pricing items. Ensure that the <paramref name="dto"/> parameter contains valid data before
        /// calling this method.</remarks>
        /// <param name="dto">The request object containing the updated display order information for vacation pricing items.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateVacationPriceDisplayOrderAsync(VacationPricingItemGroupUpdateRequest dto,
            CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"vacations/vacationPrices/updateDisplayOrder", dto);
            return result;
        }

        /// <summary>
        /// Removes a vacation pricing item asynchronously.
        /// </summary>
        /// <remarks>This method sends a request to delete the specified vacation pricing item. Ensure
        /// that the  <paramref name="vacationPricingItemId"/> corresponds to an existing item.</remarks>
        /// <param name="vacationPricingItemId">The unique identifier of the vacation pricing item to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveVacationPriceAsync(string vacationPricingItemId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"vacations/vacationPrices", vacationPricingItemId);
            return result;
        }

        /// <summary>
        /// Retrieves the pricing groups associated with a specific vacation.
        /// </summary>
        /// <remarks>This method communicates with an external provider to fetch the pricing groups for
        /// the specified vacation. Ensure that the <paramref name="vacationId"/> is valid and corresponds to an
        /// existing vacation.</remarks>
        /// <param name="vacationId">The unique identifier of the vacation for which pricing groups are requested. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// containing an enumerable collection of <see cref="VacationPriceGroupDto"/> objects representing the pricing
        /// groups.</returns>
        public async Task<IBaseResult<IEnumerable<VacationPriceGroupDto>>> VacationPricingGroupsAsync(string vacationId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<VacationPriceGroupDto>>($"vacations/vacationPrices/groups/{vacationId}");
            return result;
        }

        /// <summary>
        /// Creates or updates a vacation pricing group asynchronously.
        /// </summary>
        /// <remarks>This method sends a PUT request to the "vacations/vacationPrices/groups" endpoint to
        /// create or update the specified vacation pricing group.</remarks>
        /// <param name="dto">The data transfer object containing the details of the vacation pricing group to create or update.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the created or updated <see cref="VacationPriceGroupDto"/>.</returns>
        public async Task<IBaseResult<VacationPriceGroupDto>> CreateVacationPricingGroupAsync(VacationPriceGroupDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<VacationPriceGroupDto, VacationPriceGroupDto>($"vacations/vacationPrices/groups", dto);
            return result;
        }

        public async Task<IBaseResult<VacationPriceGroupDto>> UpdateVacationPricingGroupAsync(VacationPriceGroupDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<VacationPriceGroupDto, VacationPriceGroupDto>($"vacations/vacationPrices/groups", dto);
            return result;
        }

        /// <summary>
        /// Removes a vacation pricing group identified by the specified group ID.
        /// </summary>
        /// <remarks>This method sends a request to delete the specified vacation pricing group. Ensure
        /// that the <paramref name="groupId"/> corresponds to an existing group.</remarks>
        /// <param name="groupId">The unique identifier of the vacation pricing group to remove. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveVacationPricingGroupAsync(string groupId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"vacations/vacationPrices/groups", groupId);
            return result;
        }

        /// <summary>
        /// Retrieves the payment schedule entries for a specified vacation.
        /// </summary>
        /// <remarks>This method sends a request to retrieve payment rules associated with the specified
        /// vacation. Ensure that the <paramref name="vacationId"/> is valid and corresponds to an existing
        /// vacation.</remarks>
        /// <param name="vacationId">The unique identifier of the vacation for which payment schedule entries are requested.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// of <see cref="IEnumerable{T}"/> containing <see cref="PaymentRuleDto"/> objects that represent the payment
        /// schedule entries.</returns>
        public async Task<IBaseResult<IEnumerable<PaymentRuleDto>>> PaymentScheduleEntriesAsync(string vacationId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<PaymentRuleDto>>($"vacations/vacationPrices/paymentRules/{vacationId}");
            return result;
        }

        /// <summary>
        /// Creates a new payment schedule entry based on the provided payment rule data.
        /// </summary>
        /// <remarks>This method sends the payment rule data to the underlying provider to create a
        /// payment schedule entry. Ensure that the <paramref name="dto"/> contains valid data before calling this
        /// method.</remarks>
        /// <param name="dto">The data transfer object containing the payment rule details to be used for creating the payment schedule
        /// entry.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> CreatePaymentScheduleEntryAsync(PaymentRuleDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<PaymentRuleDto, PaymentRuleDto>($"vacations/vacationPrices/paymentRule", dto);
            return result;
        }

        /// <summary>
        /// Updates a payment schedule entry with the specified details.
        /// </summary>
        /// <remarks>This method sends the provided payment schedule entry details to the server for
        /// updating. Ensure that the <paramref name="dto"/> parameter contains valid data.</remarks>
        /// <param name="dto">The <see cref="PaymentRuleDto"/> containing the payment schedule entry details to update.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation. The default value is <see
        /// cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdatePaymentScheduleEntryAsync(PaymentRuleDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<PaymentRuleDto, PaymentRuleDto>($"vacations/vacationPrices/paymentRule", dto);
            return result;
        }

        /// <summary>
        /// Removes a payment schedule entry identified by the specified payment rule ID.
        /// </summary>
        /// <remarks>This method sends a request to delete the payment schedule entry associated with the
        /// specified <paramref name="paymentRuleId"/>. Ensure the provided ID is valid and corresponds to an existing
        /// entry.</remarks>
        /// <param name="paymentRuleId">The unique identifier of the payment rule to be removed. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemovePaymentScheduleEntryAsync(string paymentRuleId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"vacations/vacationPrices/paymentRule", paymentRuleId);
            return result;
        }
    }
}
