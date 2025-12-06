using AccomodationModule.Domain.DataTransferObjects;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Domain.Interfaces
{
    /// <summary>
    /// Implements the golfer package service, providing methods to manage golfer package data.
    /// </summary>
    public interface IGolferPackageService
    {
        /// <summary>
        /// Retrieves a list of golfer packages associated with the specified vacation.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch golfer packages. The caller
        /// can use the  <paramref name="cancellationToken"/> to cancel the operation if needed.</remarks>
        /// <param name="vacationId">The unique identifier of the vacation for which to retrieve golfer packages. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// containing an enumerable collection of <see cref="GolferPackageDto"/> objects representing the golfer
        /// packages.</returns>
        Task<IBaseResult<IEnumerable<GolferPackageDto>>> GolferPackageListAsync(string vacationId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the details of a golfer package based on the specified package ID.
        /// </summary>
        /// <remarks>Use this method to fetch detailed information about a specific golfer package. Ensure
        /// that the provided <paramref name="golferPackageId"/> is valid and corresponds to an existing
        /// package.</remarks>
        /// <param name="golferPackageId">The unique identifier of the golfer package to retrieve. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping the <see cref="GolferPackageDto"/> with the details of the golfer package.</returns>
        Task<IBaseResult<GolferPackageDto>> GolferPackageAsync(string golferPackageId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a golfer package based on the provided data transfer object (DTO).
        /// </summary>
        /// <remarks>The <paramref name="dto"/> parameter must contain all required fields for creating a
        /// golfer package.  If the operation is canceled via the <paramref name="cancellationToken"/>, the task will be
        /// marked as canceled.</remarks>
        /// <param name="dto">The data transfer object containing the details of the golfer package to create.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        Task<IBaseResult> CreateGolferPackageAsync(GolferPackageDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the golfer's package with the specified details.
        /// </summary>
        /// <remarks>The operation may fail if the provided data in <paramref name="dto"/> is invalid or
        /// if the update cannot be  completed due to system constraints. Ensure that the <paramref name="dto"/>
        /// contains valid and complete  information before calling this method.</remarks>
        /// <param name="dto">The data transfer object containing the updated package details for the golfer.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the update operation.</returns>
        Task<IBaseResult> UpdateGolferPackageAsync(GolferPackageDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a golfer package identified by the specified ID.
        /// </summary>
        /// <remarks>Use this method to remove a golfer package from the system. Ensure that the specified
        /// <paramref name="golferPackageId"/> corresponds to an existing package. The operation may fail if the package
        /// does not exist or if there are constraints preventing its removal.</remarks>
        /// <param name="golferPackageId">The unique identifier of the golfer package to remove. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        Task<IBaseResult> RemoveGolferPackageAsync(string golferPackageId, CancellationToken cancellationToken = default);
    }
}
