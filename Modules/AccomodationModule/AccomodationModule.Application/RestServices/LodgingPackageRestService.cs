using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Application.RestServices
{
    /// <summary>
    /// Provides methods for managing product packages, including retrieving, creating, updating, and deleting packages.
    /// </summary>
    /// <remarks>This service communicates with an external provider to perform operations related to product
    /// packages. It includes methods for retrieving package account types, fetching package details, creating new
    /// packages, updating existing packages, and removing packages. All operations are asynchronous and support
    /// cancellation through a <see cref="CancellationToken"/>.</remarks>
    /// <param name="provider"></param>
    public class LodgingPackageRestService(IBaseHttpProvider provider) : ILodgingPackageService
    {
        /// <summary>
        /// Retrieves all package account types associated with the specified lodging.
        /// </summary>
        /// <remarks>This method communicates with an external provider to fetch the package account
        /// types. Ensure that the <paramref name="lodgingId"/> is valid and corresponds to an existing lodging in the
        /// system.</remarks>
        /// <param name="lodgingId">The unique identifier of the lodging for which to retrieve package account types.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// containing an enumerable collection of <see cref="PackageDto"/> objects representing the package account
        /// types.</returns>
        public async Task<IBaseResult<IEnumerable<LodgingPackageDto>>> AllPackageAccountTypesAsync(string lodgingId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<LodgingPackageDto>>($"lodgings/{lodgingId}/packages");
            return result;
        }

        /// <summary>
        /// Retrieves the details of a product package by its unique identifier.
        /// </summary>
        /// <remarks>This method sends a request to retrieve the package details from the provider. If the
        /// package does not exist, the result may indicate an error or an empty response, depending on the provider's
        /// behavior.</remarks>
        /// <param name="packageId">The unique identifier of the package to retrieve. Must be a positive integer.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that wraps the <see cref="PackageDto"/> representing the package details.</returns>
        public async Task<IBaseResult<LodgingPackageDto>> ProductPackageAsync(int packageId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<LodgingPackageDto>($"lodgings/packages/{packageId}");
            return result;
        }

        /// <summary>
        /// Creates a new product package asynchronously.
        /// </summary>
        /// <remarks>This method sends a request to create a new product package using the provided
        /// package details. The operation is performed asynchronously and can be canceled using the provided <paramref
        /// name="cancellationToken"/>.</remarks>
        /// <param name="package">The package details to be created. Must not be <see langword="null"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the created <see cref="PackageDto"/> if the operation is successful.</returns>
        public async Task<IBaseResult<LodgingPackageDto>> CreateProductPackageAsync(LodgingPackageDto package, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<LodgingPackageDto, LodgingPackageDto>($"lodgings/createPackage", package);
            return result;
        }

        /// <summary>
        /// Updates the specified product package asynchronously.
        /// </summary>
        /// <remarks>This method sends a request to update the product package using the provided details.
        /// Ensure that the <paramref name="package"/> parameter contains valid data before calling this
        /// method.</remarks>
        /// <param name="package">The package details to be updated. Must not be <see langword="null"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the updated <see cref="PackageDto"/> if the operation is successful.</returns>
        public async Task<IBaseResult<LodgingPackageDto>> UpdateProductPackageAsync(LodgingPackageDto package, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<LodgingPackageDto, LodgingPackageDto>($"lodgings/updatePackage", package);
            return result;
        }

        /// <summary>
        /// Removes a package with the specified identifier asynchronously.
        /// </summary>
        /// <remarks>This method sends a request to delete the package identified by <paramref
        /// name="id"/>. Ensure that the specified package exists before calling this method. The operation may fail if
        /// the package cannot be found or if there are issues with the underlying provider.</remarks>
        /// <param name="id">The unique identifier of the package to be removed.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the removal operation.</returns>
        public async Task<IBaseResult> RemovePackageAsync(int id, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"lodgings/deletePackage/{id}","");
            return result;
        }
    }
}
