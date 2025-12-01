using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Mvc;
using ProductsModule.Domain.DataTransferObjects;
using ProductsModule.Domain.Interfaces;
using ProductsModule.Domain.RequestFeatures;

namespace ProductsModule.Infrastructure.Controllers;

/// <summary>
/// API endpoints for managing products.
/// </summary>
[Route("api/products"), ApiController]
public class ProductController(IProductService service) : ControllerBase
{
    /// <summary>
    /// Retrieves a paginated list of products including pricing details.
    /// </summary>
    /// <param name="parameters">The filtering, sorting, and pagination parameters for retrieving products.</param>
    /// <returns>An IActionResult containing a paginated collection of products with pricing information.</returns>
    [HttpGet] public async Task<IActionResult> GetPagedAsync([FromQuery] ProductsParameters parameters)
    {
        var result = await service.PagedPricedProductsAsync(parameters, false);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a complete list of products including pricing information using paginated parameters.
    /// </summary>
    /// <param name="parameters">The pagination and optional filtering parameters to retrieve all products.</param>
    /// <returns>An IActionResult containing a collection of products with corresponding pricing details.</returns>
    [HttpGet("all")] public async Task<IActionResult> GetAllAsync([FromQuery] ProductsParameters parameters)
    {
        var result = await service.AllPricedProductsAsync(parameters, false);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves detailed pricing information for a specific product identified by its ID.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <returns>An IActionResult containing the full pricing details of the specified product.</returns>
    [HttpGet("{productId}")] public async Task<IActionResult> GetByIdAsync(string productId)
    {
        var result = await service.PricedProductAsync(productId, false);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new product with the provided product creation details.
    /// </summary>
    /// <param name="product">A ProductCreationDto containing the data required to create a new product.</param>
    /// <returns>An IActionResult containing the created product details upon successful creation.</returns>
    [HttpPut] public async Task<IActionResult> CreateAsync([FromBody] ProductDto product)
    {
        var result = await service.CreateAsync(product);
        return Ok(result);
    }

    /// <summary>
    /// Updates an existing product with the provided product edition details.
    /// </summary>
    /// <param name="product">A ProductEditionDto containing updated product information.</param>
    /// <returns>An IActionResult containing the updated product details if the update is successful.</returns>
    [HttpPost] public async Task<IActionResult> UpdateAsync([FromBody] ProductDto product)
    {
        var result = await service.UpdateAsync(product);
        return Ok(result);
    }

    /// <summary>
    /// Deletes a product identified by the specified product ID.
    /// </summary>
    /// <param name="productId">The unique identifier of the product to delete.</param>
    /// <returns>An IActionResult indicating the success or failure of the deletion operation.</returns>
    [HttpDelete("{productId}")] public async Task<IActionResult> DeleteAsync(string productId)
    {
        var result = await service.DeleteAsync(productId);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a list of popular products based on predefined popularity criteria.
    /// </summary>
    /// <returns>An IActionResult containing a collection of popular products along with their pricing details.</returns>
    [HttpGet("popular/{count}")] public async Task<IActionResult> PopularAsync(int count)
    {
        var result = await service.PopularProductsAsync(count);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves products that belong to a specified category along with their pricing details.
    /// </summary>
    /// <param name="categoryId">The unique identifier of the product category.</param>
    /// <returns>An IActionResult containing a collection of products in the specified category with pricing information.</returns>
    [HttpGet("category/{categoryId}")] public async Task<IActionResult> CategoryAsync(string categoryId)
    {
        var result = await service.CategoryPricedProductsAsync(categoryId);
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

    #region Attributes

    /// <summary>
    /// Retrieves all attributes associated with the specified product.
    /// </summary>
    /// <remarks>This method sends an HTTP GET request to retrieve the attributes of a product identified by
    /// <paramref name="productId"/>. If the product does not exist or has no attributes, the response will contain an
    /// empty collection.</remarks>
    /// <param name="productId">The unique identifier of the product whose attributes are to be retrieved.</param>
    /// <returns>An <see cref="IActionResult"/> containing a collection of attributes for the specified product. The response is
    /// returned with an HTTP 200 status code if successful.</returns>
    [HttpGet("attributes/all/{productId}")] public async Task<IActionResult> GetAllAttributes(string productId)
    {
        var result = await service.GetAllAttributes(productId, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves the attribute associated with the specified attribute ID.
    /// </summary>
    /// <remarks>This method sends an HTTP GET request to retrieve the attribute details. The operation
    /// respects the cancellation token provided by the HTTP context.</remarks>
    /// <param name="attributeId">The unique identifier of the attribute to retrieve. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> containing the attribute data if found, or an appropriate HTTP status code if
    /// not.</returns>
    [HttpGet("attributes/{attributeId}")]
    public async Task<IActionResult> GetAttribute(string attributeId)
    {
        var result = await service.GetAttribute(attributeId, HttpContext.RequestAborted);
        return Ok(result);
    }

    #endregion

    #region Metadata

    /// <summary>
    /// Retrieves all metadata entries associated with a specific product.
    /// </summary>
    /// <param name="productId">The unique identifier of the product for which metadata is requested.</param>
    /// <returns>
    /// An IActionResult that contains an IBaseResult with an enumerable collection of ProductMetadataDto objects.
    /// On success, the result includes detailed metadata information for the specified product.
    /// </returns>
    [HttpGet("metadata/{productId}")]
    public async Task<IActionResult> GetMetadataAsync(string productId)
    {
        var result = await service.GetMetadata(productId, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new metadata record for a product.
    /// </summary>
    /// <param name="dto">A ProductMetadataDto containing the key, value, and associated product details to be created.</param>
    /// <returns>
    /// An IActionResult that contains an IBaseResult with the newly created ProductMetadataDto.
    /// This endpoint maps the provided DTO to a new metadata entity and persists it in the data store.
    /// </returns>
    [HttpPut("metadata")]
    public async Task<IActionResult> CreateMetadataAsync([FromBody] ProductMetadataDto dto)
    {
        var result = await service.CreateMetadata(dto, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Updates an existing metadata record for a product.
    /// </summary>
    /// <param name="dto">A ProductMetadataDto containing the updated metadata details for the product.</param>
    /// <returns>
    /// An IActionResult that contains an IBaseResult indicating the success or failure of the update operation.
    /// The endpoint updates the existing metadata entity with the new data submitted.
    /// </returns>
    [HttpPost("metadata")]
    public async Task<IActionResult> UpdateMetadataAsync([FromBody] ProductMetadataDto dto)
    {
        var result = await service.UpdateMetadata(dto, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Deletes a metadata record associated with a specific product.
    /// </summary>
    /// <param name="productId">The unique identifier of the product whose metadata should be deleted.</param>
    /// <returns>
    /// An IActionResult that contains an IBaseResult indicating the outcome of the deletion operation.
    /// The deletion is performed using the product identifier and observes the request cancellation token.
    /// </returns>
    [HttpDelete("metadata/{productId}")]
    public async Task<IActionResult> DeleteMetadataAsync(string productId)
    {
        var result = await service.DeleteAsync(productId, HttpContext.RequestAborted);
        return Ok(result);
    }

    #endregion
}
