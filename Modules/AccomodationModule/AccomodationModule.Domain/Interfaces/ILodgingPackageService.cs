using AccomodationModule.Domain.DataTransferObjects;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Domain.Interfaces
{
    /// <summary>
    /// Interface for managing package-related operations.
    /// </summary>
	public interface ILodgingPackageService
    {
        /// <summary>
        /// Retrieves all package account types associated with the specified lodging.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch package account types for the
        /// specified lodging. Ensure that the <paramref name="lodgingId"/> is valid and corresponds to an existing
        /// lodging.</remarks>
        /// <param name="lodgingId">The unique identifier of the lodging for which package account types are to be retrieved. Must not be <see
        /// langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// with a collection of <see cref="PackageDto"/> objects representing the package account types.</returns>
        Task<IBaseResult<IEnumerable<LodgingPackageDto>>> AllPackageAccountTypesAsync(string lodgingId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the details of a product package based on the specified package ID.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch package details. Ensure that
        /// the provided <paramref name="packageId"/> is valid and corresponds to an existing package.</remarks>
        /// <param name="packageId">The unique identifier of the package to retrieve. Must be a positive integer.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the package details encapsulated in a <see cref="PackageDto"/>. If the package is not found, the
        /// result may indicate an error or an empty state.</returns>
        Task<IBaseResult<LodgingPackageDto>> ProductPackageAsync(int packageId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new product package asynchronously.
        /// </summary>
        /// <remarks>This method performs the creation of a product package and returns the result
        /// encapsulated in an <see cref="IBaseResult{T}"/>. The caller can use the result to determine the success or
        /// failure of the operation.</remarks>
        /// <param name="package">The package details to be created. Must not be <see langword="null"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the created <see cref="PackageDto"/> if the operation succeeds.</returns>
        Task<IBaseResult<LodgingPackageDto>> CreateProductPackageAsync(LodgingPackageDto package, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the details of an existing product package asynchronously.
        /// </summary>
        /// <remarks>Use this method to update the properties of an existing product package. Ensure that
        /// the provided  <paramref name="package"/> object contains valid data before calling this method.</remarks>
        /// <param name="package">The <see cref="PackageDto"/> object containing the updated package details. This parameter must not be <see
        /// langword="null"/>.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation. Defaults to <see
        /// cref="CancellationToken.None"/> if not provided.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// with the updated <see cref="PackageDto"/> object.</returns>
        Task<IBaseResult<LodgingPackageDto>> UpdateProductPackageAsync(LodgingPackageDto package, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a package with the specified identifier asynchronously.
        /// </summary>
        /// <remarks>This method performs the removal operation asynchronously. Ensure that the package
        /// with the specified  identifier exists before calling this method to avoid unexpected results.</remarks>
        /// <param name="id">The unique identifier of the package to be removed. Must be a positive integer.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. The default value is <see langword="default"/>,  which
        /// indicates that no cancellation is requested.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the removal operation.</returns>
        Task<IBaseResult> RemovePackageAsync(int id, CancellationToken cancellationToken = default);
    }
}
