using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccomodationModule.Infrastructure.Controllers
{
    /// <summary>
    /// Provides endpoints for managing packages associated with lodgings.
    /// </summary>
    /// <remarks>This controller handles operations such as retrieving, creating, updating, and deleting
    /// packages. It interacts with the <see cref="IPackageService"/> to perform the necessary business logic.</remarks>
    [Route("api/lodgings"),ApiController]
    public class LodgingPackageController(ILodgingPackageService packageService) : ControllerBase
    {
        /// <summary>
        /// Retrieves all package account types associated with the specified lodging.
        /// </summary>
        /// <remarks>This method calls the service layer to fetch all package account types for the given
        /// lodging ID. Ensure that the <paramref name="lodgingId"/> corresponds to a valid lodging in the
        /// system.</remarks>
        /// <param name="lodgingId">The unique identifier of the lodging for which to retrieve package account types. Cannot be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> containing the collection of package account types associated with the
        /// specified lodging. The response is returned with an HTTP 200 status code if successful.</returns>
        [HttpGet("{lodgingId}/packages")]
        public async Task<IActionResult> Packages(string lodgingId)
        {
            var newPackage = await packageService.AllPackageAccountTypesAsync(lodgingId);
            return Ok(newPackage);
        }

        /// <summary>
        /// Retrieves the details of a product package based on the specified package ID.
        /// </summary>
        /// <remarks>This method uses the <c>packageService</c> to asynchronously retrieve the package
        /// details. Ensure that the <paramref name="packageId"/> corresponds to a valid package in the
        /// system.</remarks>
        /// <param name="packageId">The unique identifier of the package to retrieve.</param>
        /// <returns>An <see cref="IActionResult"/> containing the details of the requested package.  Returns a 200 OK response
        /// with the package details if the package is found.</returns>
        [HttpGet("packages/{packageId}")]
        public async Task<IActionResult> Package(int packageId)
        {
            var newPackage = await packageService.ProductPackageAsync(packageId);
            return Ok(newPackage);
        }

        /// <summary>
        /// Creates a new product package based on the provided package details.
        /// </summary>
        /// <remarks>This method processes the provided package details and creates a new product package
        /// using the underlying service. The result is returned as an HTTP 200 OK response containing the created
        /// package.</remarks>
        /// <param name="package">The package details to create, provided as a <see cref="PackageDto"/> object. Cannot be null.</param>
        /// <returns>An <see cref="IActionResult"/> containing the newly created package details.</returns>
        [HttpPut("createPackage")]
        public async Task<IActionResult> CreatePackage([FromBody] LodgingPackageDto package)
        {
            var newPackage = await packageService.CreateProductPackageAsync(package);
            return Ok(newPackage);
        }

        /// <summary>
        /// Updates an existing package with the provided details.
        /// </summary>
        /// <remarks>The <paramref name="package"/> parameter must include all necessary information for
        /// the update,  such as the package's unique identifier and any fields to be modified.  Ensure that the
        /// provided data adheres to the expected format and constraints.</remarks>
        /// <param name="package">The package details to update, including its identifier and updated properties.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the update operation.  Typically, this includes the
        /// updated package details or a status indicating the success of the operation.</returns>
        [HttpPost("updatePackage")]
        public async Task<IActionResult> EditPackage([FromBody] LodgingPackageDto package)
        {
            var result = await packageService.UpdateProductPackageAsync(package);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a package with the specified identifier.
        /// </summary>
        /// <remarks>This method invokes the package service to remove the package asynchronously.  Ensure
        /// that the <paramref name="packageId"/> corresponds to an existing package.</remarks>
        /// <param name="packageId">The unique identifier of the package to delete.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.  Typically, this will be an HTTP 200
        /// response with the result of the deletion.</returns>
        [HttpDelete("deletePackage/{packageId}")]
        public async Task<IActionResult> DeletePackage(int packageId)
        {
            var result = await packageService.RemovePackageAsync(packageId);
            return Ok(result);
        }
    }
}
