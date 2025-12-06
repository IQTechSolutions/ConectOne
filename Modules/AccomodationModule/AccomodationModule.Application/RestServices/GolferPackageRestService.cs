using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Application.RestServices
{
    /// <summary>
    /// Provides methods for managing golfer packages, including retrieving, creating, updating, and deleting packages.
    /// </summary>
    /// <remarks>This service interacts with an underlying HTTP provider to perform operations related to
    /// golfer packages. It supports asynchronous operations for retrieving lists of packages, fetching package details,
    /// creating or updating packages, and removing packages. Ensure that the provided identifiers and data transfer
    /// objects are valid before invoking these methods.</remarks>
    /// <param name="provider"></param>
    public class GolferPackageRestService(IBaseHttpProvider provider) : IGolferPackageService
    {
        /// <summary>
        /// Retrieves a list of golfer packages associated with the specified vacation.
        /// </summary>
        /// <remarks>This method fetches golfer packages from the underlying provider using the specified
        /// vacation ID.  Ensure that the <paramref name="vacationId"/> corresponds to a valid vacation.</remarks>
        /// <param name="vacationId">The unique identifier of the vacation for which to retrieve golfer packages.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// containing an enumerable collection of <see cref="GolferPackageDto"/> objects representing the golfer
        /// packages.</returns>
        public async Task<IBaseResult<IEnumerable<GolferPackageDto>>> GolferPackageListAsync(string vacationId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<GolferPackageDto>>($"vacations/golferPackages/{vacationId}");
            return result;
        }

        /// <summary>
        /// Retrieves the details of a golfer package based on the specified package ID.
        /// </summary>
        /// <remarks>This method sends a request to retrieve the details of a golfer package from the
        /// provider's API. Ensure that the <paramref name="golferPackageId"/> corresponds to a valid package.</remarks>
        /// <param name="golferPackageId">The unique identifier of the golfer package to retrieve. This value cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that wraps the details of the golfer package as a <see cref="GolferPackageDto"/>.</returns>
        public async Task<IBaseResult<GolferPackageDto>> GolferPackageAsync(string golferPackageId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<GolferPackageDto>($"vacations/golferPackages/details/{golferPackageId}");
            return result;
        }

        /// <summary>
        /// Creates or updates a golfer package asynchronously.
        /// </summary>
        /// <remarks>This method sends a PUT request to the "vacations/golferPackages" endpoint with the
        /// provided golfer package details.</remarks>
        /// <param name="dto">The data transfer object containing the details of the golfer package to create or update.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> CreateGolferPackageAsync(GolferPackageDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync($"vacations/golferPackages", dto);
            return result;
        }

        /// <summary>
        /// Updates a golfer package with the specified details.
        /// </summary>
        /// <remarks>This method sends the provided golfer package details to the server for updating.
        /// Ensure that the <paramref name="dto"/> parameter contains valid data before calling this method.</remarks>
        /// <param name="dto">The data transfer object containing the details of the golfer package to update.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateGolferPackageAsync(GolferPackageDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"vacations/golferPackages", dto);
            return result;
        }

        /// <summary>
        /// Removes a golfer package asynchronously.
        /// </summary>
        /// <param name="golferPackageId">The unique identifier of the golfer package to be removed. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveGolferPackageAsync(string golferPackageId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"vacations/golferPackages", golferPackageId);
            return result;
        }
    }
}
