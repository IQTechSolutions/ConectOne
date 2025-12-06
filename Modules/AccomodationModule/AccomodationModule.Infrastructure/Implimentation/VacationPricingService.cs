using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;

namespace AccomodationModule.Infrastructure.Implimentation
{
    /// <summary>
    /// Implements the vacation service, providing methods to manage vacation data.
    /// </summary>
    public class VacationPricingService(IAccomodationRepositoryManager accomodationRepositoryManager) : IVacationPricingService
    {
        /// <summary>
        /// Retrieves all vacation prices for a specific vacation.
        /// </summary>
        /// <param name="vacationId">The ID of the vacation.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result containing a collection of vacation pricing items.</returns>
        public async Task<IBaseResult<IEnumerable<VacationPricingItemDto>>> VacationPricesAsync(string vacationId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<VacationPrice>(c => c.VacationId == vacationId);    

            // Find all vacation prices for the specified vacation ID
            var vacationPriceResult = await accomodationRepositoryManager.VacationPrices.ListAsync(spec, false, cancellationToken);

            // If successful, return the vacation pricing items; otherwise, return failure
            if (vacationPriceResult.Succeeded)
                return await Result<IEnumerable<VacationPricingItemDto>>.SuccessAsync(vacationPriceResult.Data.Select(c => new VacationPricingItemDto(c)));
            return await Result<IEnumerable<VacationPricingItemDto>>.FailAsync(vacationPriceResult.Messages);
        }

        /// <summary>
        /// Get a vacation price
        /// </summary>
        /// <param name="vacationPricingId">The identity of the vacation price to retreive</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success of the operation.</returns>
        public async Task<IBaseResult<VacationPricingItemDto>> VacationPriceAsync(string vacationPricingId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<VacationPrice>(c => c.Id == vacationPricingId);

            // Find all vacation prices for the specified vacation ID
            var vacationPriceResult = await accomodationRepositoryManager.VacationPrices.FirstOrDefaultAsync(spec, false, cancellationToken);

            // If successful, return the vacation pricing items; otherwise, return failure
            if (vacationPriceResult.Succeeded)
                return await Result<VacationPricingItemDto>.SuccessAsync(new VacationPricingItemDto(vacationPriceResult.Data!));
            return await Result<VacationPricingItemDto>.FailAsync(vacationPriceResult.Messages);
        }

        /// <summary>
        /// Creates a new vacation price.
        /// </summary>
        /// <param name="dto">The data transfer object containing the vacation price data.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success of the operation.</returns>
        public async Task<IBaseResult> CreateVacationPriceAsync(VacationPricingItemDto dto, CancellationToken cancellationToken = default)
        {
            // Convert the DTO to a VacationPrice entity
            var vacation = dto.ToVacationPrice();

            // Create the vacation price
            await accomodationRepositoryManager.VacationPrices.CreateAsync(vacation, cancellationToken);

            // Save the changes
            var saveResult = await accomodationRepositoryManager.VacationPrices.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            // Return success message
            return await Result.SuccessAsync("Vacation price was created successfully");
        }

        /// <summary>
        /// Creates a new vacation pricing display type info section.
        /// </summary>
        /// <param name="dto">The data transfer object containing the vacation pricing display type info section data.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success of the operation.</returns>
        public async Task<IBaseResult> UpdateVacationPriceAsync(VacationPricingItemDto dto, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<VacationPrice>(c => c.Id == dto.VacationPriceItemId);

            var vacationPricingResult = await accomodationRepositoryManager.VacationPrices.FirstOrDefaultAsync(spec, true, cancellationToken);
            if (!vacationPricingResult.Succeeded) return await Result.FailAsync(vacationPricingResult.Messages);

            dto.UpdateVactionPricingValues(vacationPricingResult.Data!);

            var updateResult = accomodationRepositoryManager.VacationPrices.Update(vacationPricingResult.Data!);
            if (!updateResult.Succeeded) return await Result.FailAsync(updateResult.Messages);

            var saveResult = await accomodationRepositoryManager.VacationPrices.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync($"Vacation price updated successfully");
        }

        /// <summary>
        /// Creates a new vacation inclusion display type info section.
        /// </summary>
        /// <param name="dto">The data transfer object containing the vacation inclusion display type info section data.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success of the operation.</returns>
        public async Task<IBaseResult> UpdateVacationPriceDisplayOrderAsync(VacationPricingItemGroupUpdateRequest dto, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<VacationPrice>(c => c.VacationId == dto.VacationId);

            var collection = await accomodationRepositoryManager.VacationPrices.ListAsync(spec, false, cancellationToken);

            foreach (var item in collection.Data)
            {
                var itemToUpdate = dto.Items.FirstOrDefault(c => c.VacationPriceItemId == item.Id);
                if (itemToUpdate is not null)
                {
                    item.Order = itemToUpdate.Order;
                    item.Selector = itemToUpdate.Selector;
                }

                accomodationRepositoryManager.VacationPrices.Update(item);
            }

            var saveResult = await accomodationRepositoryManager.VacationPrices.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync($"Vacation pricing display type information section display order was updated successfully");
        }

        /// <summary>
        /// Removes a vacation price.
        /// </summary>
        /// <param name="vacationPricingItemId">The ID of the vacation price to remove.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success of the operation.</returns>
        public async Task<IBaseResult> RemoveVacationPriceAsync(string vacationPricingItemId, CancellationToken cancellationToken = default)
        {
            // Delete the vacation price
            var result = await accomodationRepositoryManager.VacationPrices.DeleteAsync(vacationPricingItemId, cancellationToken);
            if (!result.Succeeded) await Result.FailAsync(result.Messages);

            // Return success message
            return await Result.SuccessAsync($"Vacation Price with id '{vacationPricingItemId}' was successfully removed");
        }

        #region Pricing Groups

        public async Task<IBaseResult<IEnumerable<VacationPriceGroupDto>>> VacationPricingGroupsAsync(string vacationId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<VacationPriceGroup>(c => c.VacationId == vacationId);

            var result = await accomodationRepositoryManager.VacationPricingGroups.ListAsync(spec,false, cancellationToken);
            if (!result.Succeeded) return await Result<IEnumerable<VacationPriceGroupDto>>.FailAsync(result.Messages);
            return await Result< IEnumerable < VacationPriceGroupDto>>.SuccessAsync(result.Data.Select(c => new VacationPriceGroupDto(){Id = c.Id, Name = c.Name, VacationId = c.VacationId}));
        }

        /// <summary>
        /// Creates a new vacation pricing group and persists it to the data store.
        /// </summary>
        /// <remarks>This method creates a new vacation pricing group based on the provided <paramref
        /// name="dto"/> and saves it to the data store. If the operation fails, the result will include error messages
        /// indicating the reason for failure.</remarks>
        /// <param name="dto">The data transfer object containing the details of the vacation pricing group to create.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with the created <see cref="VacationPriceGroupDto"/> if the operation succeeds, or error messages if it
        /// fails.</returns>
        public async Task<IBaseResult<VacationPriceGroupDto>> CreateVacationPricingGroupAsync(VacationPriceGroupDto dto, CancellationToken cancellationToken = default)
        {
            // Convert the DTO to a VacationPriceGroup entity
            var vacationPriceGroup = new VacationPriceGroup(){ Id = dto.Id, Name = dto.Name, VacationId = dto.VacationId};
            // Create the vacation price group
            await accomodationRepositoryManager.VacationPricingGroups.CreateAsync(vacationPriceGroup, cancellationToken);
            // Save the changes
            var saveResult = await accomodationRepositoryManager.VacationPricingGroups.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result<VacationPriceGroupDto>.FailAsync(saveResult.Messages);
            // Return success message
            return await Result<VacationPriceGroupDto>.SuccessAsync(new VacationPriceGroupDto() { Id = vacationPriceGroup.Id, Name = vacationPriceGroup.Name, VacationId = vacationPriceGroup.VacationId });
        }

        /// <summary>
        /// Updates an existing vacation pricing group with the provided details.
        /// </summary>
        /// <remarks>This method updates the name of an existing vacation pricing group identified by the
        /// <see cref="VacationPriceGroupDto.Id"/>  property in the provided <paramref name="dto"/>. The changes are
        /// persisted to the underlying data store.  If the specified vacation pricing group does not exist, or if the
        /// update or save operation fails, the method  returns a failure result with appropriate error
        /// messages.</remarks>
        /// <param name="dto">The data transfer object containing the updated details of the vacation pricing group.  The <see
        /// cref="VacationPriceGroupDto.Id"/> property must match an existing vacation pricing group.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// where <c>T</c> is <see cref="VacationPriceGroupDto"/>. If the operation succeeds, the result contains the
        /// updated  vacation pricing group. If the operation fails, the result contains error messages.</returns>
        public async Task<IBaseResult<VacationPriceGroupDto>> UpdateVacationPricingGroupAsync(VacationPriceGroupDto dto, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<VacationPriceGroup>(c => c.Id == dto.Id);
            
            var result = await accomodationRepositoryManager.VacationPricingGroups.FirstOrDefaultAsync(spec, cancellationToken: cancellationToken);
            if(!result.Succeeded) return await Result<VacationPriceGroupDto>.FailAsync(result.Messages);

            result.Data!.Name = dto.Name;

            var updateResult = accomodationRepositoryManager.VacationPricingGroups.Update(result.Data);
            if (!updateResult.Succeeded) return await Result<VacationPriceGroupDto>.FailAsync(updateResult.Messages);

            // Save the changes
            var saveResult = await accomodationRepositoryManager.VacationPricingGroups.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result<VacationPriceGroupDto>.FailAsync(saveResult.Messages);

            // Return success message
            return await Result<VacationPriceGroupDto>.SuccessAsync(dto);
        }

        /// <summary>
        /// Removes a vacation pricing group identified by the specified group ID.
        /// </summary>
        /// <remarks>This method deletes the specified vacation pricing group and persists the changes to
        /// the underlying data store. If the operation fails, the result will include the failure messages.</remarks>
        /// <param name="groupId">The unique identifier of the vacation pricing group to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation. If successful, the result includes a success message;
        /// otherwise, it contains error messages.</returns>
        public async Task<IBaseResult> RemoveVacationPricingGroupAsync(string groupId, CancellationToken cancellationToken = default)
        {
            // Create the vacation price group
            await accomodationRepositoryManager.VacationPricingGroups.DeleteAsync(groupId, cancellationToken);
            // Save the changes
            var saveResult = await accomodationRepositoryManager.VacationPricingGroups.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result<VacationPriceGroupDto>.FailAsync(saveResult.Messages);
            // Return success message
            return await Result<VacationPriceGroupDto>.SuccessAsync("Pricing group successfully remove");
        }

        #endregion

        #region Payment Schedule Entry

        /// <summary>
        /// Retrieves all payment schedule entries for a specific vacation.
        /// </summary>
        /// <param name="vacationId">The ID of the vacation.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result containing a collection of payment schedule entries.</returns>
        public async Task<IBaseResult<IEnumerable<PaymentRuleDto>>> PaymentScheduleEntriesAsync(string vacationId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<PaymentRule>(c => c.VacationId == vacationId);

            // Find all vacation prices for the specified vacation ID
            var vacationPriceResult = await accomodationRepositoryManager.VacationPaymentRules.ListAsync(spec, false, cancellationToken);

            // If successful, return the vacation pricing items; otherwise, return failure
            if (vacationPriceResult.Succeeded)
                return await Result<IEnumerable<PaymentRuleDto>>.SuccessAsync(vacationPriceResult.Data.Select(c => new PaymentRuleDto(c)));
            return await Result<IEnumerable<PaymentRuleDto>>.FailAsync(vacationPriceResult.Messages);
        }

        /// <summary>
        /// Creates a new vacation payment schedule entry.
        /// </summary>
        /// <param name="dto">The data transfer object containing the vacation price data.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success of the operation.</returns>
        public async Task<IBaseResult> CreatePaymentScheduleEntryAsync(PaymentRuleDto dto, CancellationToken cancellationToken = default)
        {
            // Convert the DTO to a VacationPrice entity
            var vacation = dto.ToPaymentRule();

            // Create the vacation price
            await accomodationRepositoryManager.VacationPaymentRules.CreateAsync(vacation, cancellationToken);

            // Save the changes
            var saveResult = await accomodationRepositoryManager.VacationPaymentRules.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            // Return success message
            return await Result.SuccessAsync($"Vacation price was created successfully");
        }

        /// <summary>
        /// Updates an existing Payment Schedule Entry.
        /// </summary>
        /// <param name="dto">The data transfer object containing the vacation Payment Schedule Entry type info section data.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success of the operation.</returns>
        public async Task<IBaseResult> UpdatePaymentScheduleEntryAsync(PaymentRuleDto dto, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<PaymentRule>(c => c.Id == dto.PaymentRuleId);

            var vacationPricingResult = await accomodationRepositoryManager.VacationPaymentRules.FirstOrDefaultAsync(spec, true, cancellationToken);
            if (!vacationPricingResult.Succeeded) return await Result.FailAsync(vacationPricingResult.Messages);

            if (vacationPricingResult.Data is null) return await Result.FailAsync("No pricing item found in databasis");

            dto.UpdatePaymentRuleValues(vacationPricingResult.Data);

            var updateResult = accomodationRepositoryManager.VacationPaymentRules.Update(vacationPricingResult.Data);
            if (!updateResult.Succeeded) return await Result.FailAsync(updateResult.Messages);

            var saveResult = await accomodationRepositoryManager.VacationPaymentRules.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync($"Payment Schedule Entry updated successfully");
        }

        /// <summary>
        /// Removes a Payment Schedule Entry.
        /// </summary>
        /// <param name="paymentRuleId">The ID of the Payment Schedule Entry to remove.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success of the operation.</returns>
        public async Task<IBaseResult> RemovePaymentScheduleEntryAsync(string paymentRuleId, CancellationToken cancellationToken = default)
        {
            // Delete the vacation price
            var result = await accomodationRepositoryManager.VacationPaymentRules.DeleteAsync(paymentRuleId, cancellationToken);
            if (!result.Succeeded) await Result.FailAsync(result.Messages);

            // Return success message
            return await Result.SuccessAsync($"Payment Schedule Entry with id '{paymentRuleId}' was successfully removed");
        }

        #endregion
    }
}
