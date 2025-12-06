using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using AccomodationModule.Domain.RequestFeatures;
using ConectOne.Domain.Extensions;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Application.RestServices
{
    /// <summary>
    /// Provides methods for managing vacation intervals through RESTful API calls.
    /// </summary>
    /// <remarks>This service allows clients to perform CRUD operations on vacation intervals, including
    /// retrieving lists of intervals, fetching specific interval details, creating new intervals, updating existing
    /// intervals, and removing intervals.</remarks>
    /// <param name="provider"></param>
    public class VacationIntervalRestService(IBaseHttpProvider provider) : IVacationIntervalService
    {
        /// <summary>
        /// Retrieves a paginated list of vacation intervals based on the specified parameters.
        /// </summary>
        /// <param name="args">The parameters used to filter and paginate the vacation intervals.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  IBaseResult{T} of
        /// IEnumerable{VacationIntervalDto} representing  the retrieved vacation intervals.</returns>
        public async Task<IBaseResult<IEnumerable<VacationIntervalDto>>> VacationIntervalListAsync(VacationIntervalPageParameters args, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<VacationIntervalDto>>($"vacations/vacationIntervals?{args.GetQueryString()}");
            return result;
        }

        /// <summary>
        /// Retrieves the vacation interval details for the specified vacation pricing identifier.
        /// </summary>
        /// <remarks>This method sends a request to retrieve vacation interval details based on the
        /// provided vacation pricing identifier. Ensure that the <paramref name="vacationPricingId"/> is valid and
        /// corresponds to an existing vacation pricing entry.</remarks>
        /// <param name="vacationPricingId">The unique identifier for the vacation pricing. This value cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that wraps the <see cref="VacationIntervalDto"/> containing the vacation interval details.</returns>
        public async Task<IBaseResult<VacationIntervalDto>> VacationIntervalAsync(string vacationPricingId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<VacationIntervalDto>($"vacations/vacationIntervals/{vacationPricingId}");
            return result;
        }

        /// <summary>
        /// Creates a new vacation interval asynchronously.
        /// </summary>
        /// <remarks>This method sends a PUT request to the "vacations/vacationIntervals" endpoint to
        /// create the specified vacation interval.</remarks>
        /// <param name="dto">The data transfer object containing the details of the vacation interval to create.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> CreateVacationIntervalAsync(VacationIntervalDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync($"vacations/vacationIntervals", dto);
            return result;
        }

        /// <summary>
        /// Updates a vacation interval by sending the specified data to the server.
        /// </summary>
        /// <remarks>This method sends the provided vacation interval data to the server using a POST
        /// request. Ensure that the <paramref name="dto"/> parameter contains valid data before calling this
        /// method.</remarks>
        /// <param name="dto">The data transfer object containing the details of the vacation interval to update.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateVacationIntervalAsync(VacationIntervalDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"vacations/vacationIntervals", dto);
            return result;
        }

        /// <summary>
        /// Removes a vacation interval with the specified identifier.
        /// </summary>
        /// <remarks>This method sends a request to delete the specified vacation interval. Ensure that
        /// the <paramref name="vacationIntervalId"/> corresponds to an existing interval. The operation may fail if the
        /// identifier is invalid or the interval does not exist.</remarks>
        /// <param name="vacationIntervalId">The unique identifier of the vacation interval to remove.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveVacationIntervalAsync(string vacationIntervalId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"vacations/vacationIntervals", vacationIntervalId);
            return result;
        }
    }
}
