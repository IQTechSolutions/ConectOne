using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Interfaces;
using AccomodationModule.Domain.RequestFeatures;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;

namespace AccomodationModule.Infrastructure.Implimentation
{
    /// <summary>
    /// Implements the vacation interval service, providing methods to manage vacation interval data.
    /// </summary>
    public class VacationIntervalService(IAccomodationRepositoryManager accomodationRepositoryManager) : IVacationIntervalService
    {
        /// <summary>
        /// Retrieves a list of vacation intervals based on the specified filtering parameters.
        /// </summary>
        /// <remarks>This method filters vacation intervals based on the provided <paramref name="args"/>.
        /// If both <c>VacationId</c> and <c>VacationExtensionId</c> are null or empty, the method will fail. Ensure
        /// that valid filtering parameters are provided to avoid errors.</remarks>
        /// <param name="args">The filtering parameters for the vacation intervals, including vacation ID and vacation extension ID.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>An asynchronous operation that returns a result containing a collection of <see cref="VacationIntervalDto"/>
        /// objects. If the operation fails, the result will include error messages.</returns>
        public async Task<IBaseResult<IEnumerable<VacationIntervalDto>>> VacationIntervalListAsync(VacationIntervalPageParameters args, CancellationToken cancellationToken = default)
        {
            IBaseResult<List<VacationInterval>>? result = null;

            if (string.IsNullOrEmpty(args.VacationId))
            {
                var spec = new LambdaSpec<VacationInterval>(c => c.VacationId!.Equals(args.VacationId));
                result = await accomodationRepositoryManager.VacationIntervals.ListAsync(spec, false, cancellationToken);
            }
                
            if (result == null) return await Result<IEnumerable<VacationIntervalDto>>.FailAsync("There was an error initiating the method, please check your arguments");
            {
                if (result != null && result.Succeeded)
                    return await Result<IEnumerable<VacationIntervalDto>>.SuccessAsync(result.Data.Select(c => new VacationIntervalDto(c)));
            }
            return await Result<IEnumerable<VacationIntervalDto>>.FailAsync(result.Messages);
        }

        /// <summary>
        /// Get a vacation interval
        /// </summary>
        /// <param name="vacationPricingId">The identity of the vacation interval to retrieve</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success of the operation.</returns>
        public async Task<IBaseResult<VacationIntervalDto>> VacationIntervalAsync(string vacationPricingId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<VacationInterval>(c => c.Id! == vacationPricingId);

            var vacationIntervalResult = await accomodationRepositoryManager.VacationIntervals.FirstOrDefaultAsync(spec, false, cancellationToken);

            if (vacationIntervalResult.Succeeded)
                return await Result<VacationIntervalDto>.SuccessAsync(new VacationIntervalDto(vacationIntervalResult.Data));
            return await Result<VacationIntervalDto>.FailAsync(vacationIntervalResult.Messages);
        }

        /// <summary>
        /// Creates a new vacation interval.
        /// </summary>
        /// <param name="dto">The data transfer object containing the vacation interval data.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success of the operation.</returns>
        public async Task<IBaseResult> CreateVacationIntervalAsync(VacationIntervalDto dto, CancellationToken cancellationToken = default)
        {
            var vacationInterval = dto.ToVacationInterval();
            await accomodationRepositoryManager.VacationIntervals.CreateAsync(vacationInterval, cancellationToken);

            // Save the changes
            var saveResult = await accomodationRepositoryManager.VacationIntervals.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            // Return success message
            return await Result.SuccessAsync($"Vacation interval was created successfully");
        }

        /// <summary>
        /// Updates the vacation interval information for a specific vacation.
        /// </summary>
        /// <param name="dto">The data transfer object containing the vacation data, including interval information.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success of the operation.</returns>
        public async Task<IBaseResult> UpdateVacationIntervalAsync(VacationIntervalDto dto, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<VacationInterval>(c => c.Id! == dto.VacationIntervalId);

            var vacationPricingResult = await accomodationRepositoryManager.VacationIntervals.FirstOrDefaultAsync(spec, true, cancellationToken);
            if (!vacationPricingResult.Succeeded) return await Result.FailAsync(vacationPricingResult.Messages);

            dto.UpdateVacationIntervalValues(vacationPricingResult.Data);

            var updateResult = accomodationRepositoryManager.VacationIntervals.Update(vacationPricingResult.Data);
            if (!updateResult.Succeeded) return await Result.FailAsync(updateResult.Messages);

            var saveResult = await accomodationRepositoryManager.VacationIntervals.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync($"Images updated successfully");
        }

        /// <summary>
        /// Removes a vacation interval.
        /// </summary>
        /// <param name="vacationIntervalId">The ID of the vacation price to remove.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A result indicating the success of the operation.</returns>
        public async Task<IBaseResult> RemoveVacationIntervalAsync(string vacationIntervalId, CancellationToken cancellationToken = default)
        {
            // Delete the vacation price
            var result = await accomodationRepositoryManager.VacationIntervals.DeleteAsync(vacationIntervalId, cancellationToken);
            if (!result.Succeeded) await Result.FailAsync(result.Messages);

            var saveResult = await accomodationRepositoryManager.VacationIntervals.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            // Return success message
            return await Result.SuccessAsync($"Vacation interval with id '{vacationIntervalId}' was successfully removed");
        }
    }
}
