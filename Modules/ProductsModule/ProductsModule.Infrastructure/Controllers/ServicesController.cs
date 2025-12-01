using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Mvc;
using ProductsModule.Domain.DataTransferObjects;
using ProductsModule.Domain.Interfaces;
using ProductsModule.Domain.RequestFeatures;

namespace ProductsModule.Infrastructure.Controllers;

/// <summary>
/// Provides endpoints for managing services, including operations such as retrieving, creating, updating,  and deleting
/// services, as well as managing associated images.
/// </summary>
/// <remarks>This controller serves as the API layer for interacting with services and their related data. It
/// supports operations such as retrieving paginated lists of services, fetching service details by ID, creating and 
/// updating services, and deleting services. Additionally, it includes endpoints for managing images associated  with
/// services, such as adding and removing images.  The controller relies on an injected <see cref="IServiceService"/> to
/// perform the underlying business logic  and data access operations. All endpoints return appropriate HTTP responses,
/// including success and error  statuses, based on the outcome of the operations.</remarks>
/// <param name="service"></param>
[Route("api/services"), ApiController]
public class ServicesController(IServiceService service) : ControllerBase
{
    /// <summary>
    /// Retrieves a paginated list of services based on the specified query parameters.
    /// </summary>
    /// <remarks>The method processes the provided query parameters to retrieve the appropriate subset of
    /// services. Ensure that the <paramref name="parameters"/> object contains valid values for pagination and
    /// filtering.</remarks>
    /// <param name="parameters">The query parameters used to filter and paginate the results. This includes options such as page number, page
    /// size, and any filtering criteria.</param>
    /// <returns>An <see cref="IActionResult"/> containing the paginated list of services. The result is returned with an HTTP
    /// 200 OK status if successful.</returns>
    [HttpGet] public async Task<IActionResult> GetPagedAsync([FromQuery] ServiceParameters parameters)
    {
        var result = await service.PagedServicesAsync(parameters);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves the details of a service by its unique identifier.
    /// </summary>
    /// <remarks>This method performs an asynchronous operation to fetch the service details. The response is
    /// returned as an HTTP 200 OK  status with the service details if the operation is successful.</remarks>
    /// <param name="serviceId">The unique identifier of the service to retrieve. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> containing the service details if found, or an appropriate HTTP response if not.</returns>
    [HttpGet("{serviceId}")] public async Task<IActionResult> GetByIdAsync(string serviceId)
    {
        var result = await service.ServiceAsync(serviceId);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new resource based on the provided data transfer object (DTO).
    /// </summary>
    /// <param name="dto">The data transfer object containing the details of the resource to create. Cannot be null.</param>
    /// <returns>An <see cref="IActionResult"/> representing the result of the operation.  Typically returns an HTTP 200 OK
    /// response with the created resource.</returns>
    [HttpPut] public async Task<IActionResult> CreateAsync([FromBody] ServiceDto dto)
    {
        var result = await service.CreateAsync(dto);
        return Ok(result);
    }

    /// <summary>
    /// Updates the specified service entity asynchronously.
    /// </summary>
    /// <remarks>The <paramref name="dto"/> parameter must contain valid data for the update operation to
    /// succeed.</remarks>
    /// <param name="dto">The data transfer object containing the updated service information.</param>
    /// <returns>An <see cref="IActionResult"/> representing the result of the operation.  Typically, this is an HTTP 200 OK
    /// response containing the update result.</returns>
    [HttpPost] public async Task<IActionResult> UpdateAsync([FromBody] ServiceDto dto)
    {
        var result = await service.UpdateAsync(dto);
        return Ok(result);
    }

    /// <summary>
    /// Deletes the specified service asynchronously.
    /// </summary>
    /// <remarks>This method invokes the service layer to perform the deletion. Ensure that the <paramref
    /// name="serviceId"/> corresponds to an existing service.</remarks>
    /// <param name="serviceId">The unique identifier of the service to delete. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Typically returns an HTTP 200 OK response
    /// with the result of the deletion.</returns>
    [HttpDelete("{serviceId}")] public async Task<IActionResult> DeleteAsync(string serviceId)
    {
        var result = await service.DeleteAsync(serviceId);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a complete list of products including pricing information using paginated parameters.
    /// </summary>
    /// <param name="parameters">The pagination and optional filtering parameters to retrieve all products.</param>
    /// <returns>An IActionResult containing a collection of products with corresponding pricing details.</returns>
    [HttpGet("all")] public async Task<IActionResult> GetAllAsync([FromQuery] ServiceParameters parameters)
    {
        var result = await service.AllServicesAsync(parameters);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves products that belong to a specified category along with their pricing details.
    /// </summary>
    /// <param name="categoryId">The unique identifier of the product category.</param>
    /// <returns>An IActionResult containing a collection of products in the specified category with pricing information.</returns>
    [HttpGet("category/{categoryId}")] public async Task<IActionResult> CategoryAsync(string categoryId)
    {
        var result = await service.CategoryServicesAsync(categoryId);
        return Ok(result);
    }

    #region Images

    /// <summary>
    /// Adds an image to the specified entity with the provided details.
    /// </summary>
    /// <remarks>This method creates an association between an image and an entity, saving the image details
    /// to the repository. The operation will fail if the repository operations (create or save) are
    /// unsuccessful.</remarks>
    /// <param name="request">The request containing the image details, including the image ID, entity ID, selector, and order.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the operation. If the operation fails, the result includes error messages.</returns>
    [HttpPost("addImage")] public async Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default)
    {
        var addResult = await service.AddImage(request, cancellationToken);
        return addResult;
    }

    /// <summary>
    /// Removes an image with the specified identifier from the repository.
    /// </summary>
    /// <remarks>This method first attempts to delete the image from the repository. If the deletion succeeds,
    /// it then saves the changes to the repository. If either operation fails, the method returns a failure result with
    /// the associated error messages.</remarks>
    /// <param name="imageId">The unique identifier of the image to be removed. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the operation. If the operation fails, the result includes error messages.</returns>
    [HttpDelete("deleteImage/{imageId}/")] public async Task<IBaseResult> RemoveImage(string imageId, CancellationToken cancellationToken = default)
    {
        var addResult = await service.RemoveImage(imageId, cancellationToken);
        return addResult;
    }

    #endregion

    #region Videos

    /// <summary>
    /// Adds a video entity based on the provided request.
    /// </summary>
    /// <remarks>This method delegates the operation to the underlying service layer. Ensure that the
    /// <paramref name="request"/> contains valid data before invoking this method.</remarks>
    /// <param name="request">The request containing the details of the video to be added.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the outcome of the operation.</returns>
    [HttpPost("addVideo")]
    public async Task<IBaseResult> AddVideo(AddEntityVideoRequest request, CancellationToken cancellationToken = default)
    {
        var addResult = await service.AddVideo(request, cancellationToken);
        return addResult;
    }

    /// <summary>
    /// Deletes a video with the specified identifier.
    /// </summary>
    /// <remarks>This method sends an HTTP DELETE request to remove the video identified by <paramref
    /// name="videoId"/>. Ensure the video ID is valid and exists in the system before calling this method.</remarks>
    /// <param name="videoId">The unique identifier of the video to delete. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the outcome of the delete operation.</returns>
    [HttpDelete("deleteVideo/{videoId}/")]
    public async Task<IBaseResult> RemoveVideo(string videoId, CancellationToken cancellationToken = default)
    {
        var addResult = await service.RemoveVideo(videoId, cancellationToken);
        return addResult;
    }

    #endregion
}