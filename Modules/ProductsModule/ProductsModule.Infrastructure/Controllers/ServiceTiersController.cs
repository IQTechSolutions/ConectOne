using ConectOne.Domain.RequestFeatures;
using Microsoft.AspNetCore.Mvc;
using ProductsModule.Domain.DataTransferObjects;
using ProductsModule.Domain.Interfaces;

namespace ProductsModule.Infrastructure.Controllers;

/// <summary>
/// Provides endpoints for managing service tiers, including retrieving, creating, updating, and deleting service tier
/// data.
/// </summary>
/// <remarks>This controller handles HTTP requests related to service tiers, such as retrieving paged or all
/// service tiers,  fetching a specific service tier by ID, and performing CRUD operations. It interacts with the
/// underlying  service layer to process requests and return appropriate responses.</remarks>
/// <param name="service"></param>
[Route("api/service-tiers"), ApiController]
public class ServiceTiersController(IServiceTierService service) : ControllerBase
{
    /// <summary>
    /// Retrieves a paginated list of service tiers based on the specified request parameters.
    /// </summary>
    /// <remarks>This endpoint supports pagination and filtering through the <paramref name="parameters"/>
    /// object. Ensure that the provided parameters are valid to avoid unexpected results.</remarks>
    /// <param name="parameters">The query parameters used to define the pagination and filtering options.  This includes properties such as page
    /// number, page size, and any additional filters.</param>
    /// <returns>An <see cref="IActionResult"/> containing the paginated list of service tiers.  The result is returned with an
    /// HTTP 200 status code if successful.</returns>
    [HttpGet("paged")] public async Task<IActionResult> GetPagedAsync([FromQuery] RequestParameters parameters)
    {
        var result = await service.PagedServiceTiersAsync(parameters, false);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves all available service tiers.
    /// </summary>
    /// <remarks>This method returns a collection of service tiers, which represent the different levels of
    /// service  available in the system. The result is returned as an HTTP 200 OK response containing the
    /// data.</remarks>
    /// <returns>An <see cref="IActionResult"/> containing an HTTP 200 OK response with the collection of service tiers.</returns>
    [HttpGet("all")] public async Task<IActionResult> GetAllAsync()
    {
        var result = await service.AllServiceTiersAsync();
        return Ok(result);
    }
    
    /// <summary>
    /// Retrieves all available service tiers.
    /// </summary>
    /// <remarks>This method returns a collection of service tiers, which represent the different levels of
    /// service  available in the system. The result is returned as an HTTP 200 OK response containing the
    /// data.</remarks>
    /// <returns>An <see cref="IActionResult"/> containing an HTTP 200 OK response with the collection of service tiers.</returns>
    [HttpGet("roles/{roleId}")]
    public async Task<IActionResult> GetAllForRoleAsync(string roleId)
    {
        var result = await service.AllEntityServiceTiersAsync(roleId);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves the details of a service tier by its unique identifier.
    /// </summary>
    /// <remarks>This method returns an HTTP 200 OK response with the service tier details if the operation is
    /// successful.  If the specified <paramref name="serviceTierId"/> does not exist, an appropriate HTTP error
    /// response is returned.</remarks>
    /// <param name="serviceTierId">The unique identifier of the service tier to retrieve. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> containing the details of the service tier if found, or an appropriate HTTP
    /// status code if not.</returns>
    [HttpGet("{serviceTierId}")] public async Task<IActionResult> GetByIdAsync(string serviceTierId)
    {
        var result = await service.ServiceTierAsync(serviceTierId);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new service tier asynchronously.
    /// </summary>
    /// <remarks>The method expects a valid <see cref="ServiceTierDto"/> object in the request body. Ensure
    /// that all required fields in the DTO are populated before calling this method.</remarks>
    /// <param name="serviceTier">The service tier data to be created. This parameter must not be <see langword="null"/>.</param>
    /// <returns>An <see cref="IActionResult"/> containing the result of the operation. Typically, this will be an HTTP 200 OK
    /// response with the created service tier data.</returns>
    [HttpPut] public async Task<IActionResult> CreateAsync([FromBody] ServiceTierDto serviceTier)
    {
        var result = await service.CreateAsync(serviceTier);
        return Ok(result);
    }

    /// <summary>
    /// Updates the specified service tier asynchronously.
    /// </summary>
    /// <remarks>The <paramref name="serviceTier"/> parameter must contain valid data for the update operation
    /// to succeed.</remarks>
    /// <param name="serviceTier">The service tier data to be updated. This must be provided in the request body.</param>
    /// <returns>An <see cref="IActionResult"/> containing the result of the update operation.  Typically, this will be an HTTP
    /// 200 OK response with the update result.</returns>
    [HttpPost] public async Task<IActionResult> UpdateAsync([FromBody] ServiceTierDto serviceTier)
    {
        var result = await service.UpdateAsync(serviceTier);
        return Ok(result);
    }

    /// <summary>
    /// Deletes the specified service tier asynchronously.
    /// </summary>
    /// <remarks>This method performs an asynchronous operation to delete a service tier identified by the
    /// provided ID. Ensure that the specified ID corresponds to an existing service tier before calling this
    /// method.</remarks>
    /// <param name="erviceTierId">The unique identifier of the service tier to delete. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Typically, this will be an HTTP 200 OK
    /// response containing the result of the deletion.</returns>
    [HttpDelete("{serviceTierId}")] public async Task<IActionResult> DeleteAsync(string serviceTierId)
    {
        var result = await service.DeleteAsync(serviceTierId);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new service tier for the specified service.
    /// </summary>
    /// <remarks>This method is invoked via an HTTP PUT request to the endpoint 
    /// "services/{serviceId}/{serviceTierId}". Ensure that both <paramref name="serviceId"/>  and <paramref
    /// name="serviceTierId"/> are valid and non-null.</remarks>
    /// <param name="serviceId">The unique identifier of the service for which the tier is being created.</param>
    /// <param name="serviceTierId">The unique identifier of the service tier to be created.</param>
    /// <returns>An <see cref="IActionResult"/> containing the result of the operation.  Returns an HTTP 200 OK response with the
    /// result if the operation is successful.</returns>
    [HttpPut("services/{serviceId}/{serviceTierId}")]
    public async Task<IActionResult> CreateServiceTierServiceAsync(string serviceId, string serviceTierId)
    {
        var result = await service.CreateServiceTierServiceAsync(serviceId, serviceTierId);
        return Ok(result);
    }

    /// <summary>
    /// Deletes the specified service tier service.
    /// </summary>
    /// <remarks>The specified <paramref name="serviceTierServiceId"/> must correspond to an existing service
    /// tier service.</remarks>
    /// <param name="serviceTierServiceId">The unique identifier of the service tier service to delete.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.  Typically, this will be an HTTP 200 OK
    /// response containing the result of the deletion.</returns>
    [HttpDelete("services/{serviceId}/{serviceTierId}")]
    public async Task<IActionResult> RemoveServiceTierServiceAsync(string serviceId, string serviceTierId)
    {
        var result = await service.RemoveServiceTierServiceAsync(serviceId, serviceTierId);
        return Ok(result);
    }
}
