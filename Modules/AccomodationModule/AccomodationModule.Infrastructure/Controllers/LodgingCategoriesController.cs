using AccomodationModule.Domain.Entities;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using FilingModule.Domain.Entities;
using FilingModule.Domain.RequestFeatures;
using GroupingModule.Domain.DataTransferObjects;
using GroupingModule.Domain.Entities;
using GroupingModule.Domain.Interfaces;
using GroupingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Mvc;

namespace AccomodationModule.Infrastructure.Controllers
{
    /// <summary>
    /// Provides endpoints for managing lodging categories and their associations with entities.
    /// </summary>
    /// <remarks>This controller includes operations for retrieving, creating, updating, and deleting lodging
    /// categories,  as well as managing associations between categories and entities. It supports paginated queries, 
    /// hierarchical category structures, and category-specific operations.</remarks>
    /// <param name="service"></param>
    [Route("api/lodgings/categories"), ApiController]
    public class LodgingCategoriesController(ICategoryService<Lodging>  categoryService, IRepository<EntityImage<Category<Lodging>, string>, string> imageRepository) : ControllerBase
    {
        /// <summary>
        /// Retrieves the total count of categories based on the specified query parameters.
        /// </summary>
        /// <param name="parameters">The query parameters used to filter and paginate the category count. Must not be null.</param>
        /// <returns>An <see cref="IActionResult"/> containing the total count of categories that match the specified parameters.</returns>
        [HttpGet("count")] public async Task<IActionResult> GetCountAsync([FromQuery] CategoryPageParameters parameters)
        {
            var result = await categoryService.CategoryCountAsync(parameters);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a paginated list of categories based on the specified query parameters.
        /// </summary>
        /// <remarks>This endpoint supports pagination and optional filtering to retrieve a subset of categories.
        /// Ensure that the <paramref name="parameters"/> object contains valid values for page number and page
        /// size.</remarks>
        /// <param name="parameters">The pagination and filtering parameters used to retrieve the categories. This includes page number, page size,
        /// and any additional filters.</param>
        /// <returns>An <see cref="IActionResult"/> containing the paginated list of categories. The result is returned with an HTTP
        /// 200 status code if successful.</returns>
        [HttpGet("paged")] public async Task<IActionResult> GetPagedAsync([FromQuery] CategoryPageParameters parameters)
        {
            var result = await categoryService.PagedCategoriesAsync(parameters);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves all categories asynchronously.
        /// </summary>
        /// <remarks>This method is intended to be used in HTTP GET requests to the "all" endpoint.</remarks>
        /// <returns>An <see cref="IActionResult"/> containing a collection of categories.  The response is serialized as an HTTP 200
        /// OK result with the collection in the response body.</returns>
        [HttpGet("all")] public async Task<IActionResult> GetAllAsync()
        {
            var result = await categoryService.CategoriesAsync(null, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves all categories asynchronously.
        /// </summary>
        /// <remarks>This method is intended to be used in HTTP GET requests to the "all" endpoint.</remarks>
        /// <returns>An <see cref="IActionResult"/> containing a collection of categories.  The response is serialized as an HTTP 200
        /// OK result with the collection in the response body.</returns>
        [HttpGet("all/{parentId}")] public async Task<IActionResult> GetChildrenAsync(string parentId)
        {
            var result = await categoryService.CategoriesAsync(parentId, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves all bottom-level activity categories.
        /// </summary>
        /// <returns>A list of all bottom-level activity categories.</returns>
        [HttpGet("bottomlevel")] public async Task<IActionResult> GetAllBottomLevelCategories()
        {
            var result = await categoryService.AllBottomLevelCategoriesAsync();
            return Ok(result);
        }

        /// <summary>
        /// Retrieves all bottom-level activity categories associated with the specified parent category.
        /// </summary>
        /// <remarks>This method calls the underlying service to fetch the bottom-level categories and returns the
        /// result as an HTTP 200 OK response. Ensure that <paramref name="parentId"/> is a valid identifier for an existing
        /// parent category.</remarks>
        /// <param name="parentId">The unique identifier of the parent category whose bottom-level activity categories are to be retrieved.</param>
        /// <returns>An <see cref="IActionResult"/> containing a collection of bottom-level activity categories if found;  otherwise,
        /// an appropriate HTTP response indicating the result of the operation.</returns>
        [HttpGet("bottomlevel/{parentId}")] public async Task<IActionResult> GetAllBottomLevelCategories(string parentId)
        {
            var result = await categoryService.AllBottomLevelCategoriesAsync(parentId);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves the category details for the specified category ID.
        /// </summary>
        /// <remarks>This method sends an HTTP GET request to retrieve the category details associated with the
        /// provided <paramref name="categoryId"/>. Ensure that the <paramref name="categoryId"/> is valid and corresponds
        /// to an existing category.</remarks>
        /// <param name="categoryId">The unique identifier of the category to retrieve. Cannot be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> containing the category details if found, or an appropriate HTTP response if not.</returns>
        [HttpGet("{categoryId}")] public async Task<IActionResult> GetByIdAsync(string categoryId)
        {
            var result = await categoryService.CategoryAsync(categoryId);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new category based on the provided data.
        /// </summary>
        /// <param name="model">The data transfer object containing the details of the category to create. Cannot be null.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.  Returns an HTTP 200 OK response with the
        /// created category details if the operation is successful.</returns>
        [HttpPut] public async Task<IActionResult> CreateAsync([FromBody] CategoryDto model)
        {
            var result = await categoryService.CreateCategoryAsync(model);
            return Ok(result);
        }

        /// <summary>
        /// Updates an existing category with the provided data.
        /// </summary>
        /// <remarks>The <paramref name="model"/> parameter must include valid category data. Ensure that the
        /// category identifier exists in the system before calling this method.</remarks>
        /// <param name="model">The category data to update, including the category's identifier and updated properties.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the update operation. Typically, this will be an HTTP
        /// 200 OK response with the updated category data.</returns>
        [HttpPost] public async Task<IActionResult> UpdateAsync([FromBody] CategoryDto model)
        {
            var result = await categoryService.UpdateCategoryAsync(model);
            return Ok(result);
        }

        /// <summary>
        /// Deletes the category with the specified identifier.
        /// </summary>
        /// <remarks>This method invokes the <c>DeleteCategoryAsync</c> method of the category service to perform
        /// the deletion. Ensure that the <paramref name="categoryId"/> corresponds to an existing category.</remarks>
        /// <param name="categoryId">The unique identifier of the category to delete. Cannot be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.  Typically returns an HTTP 200 OK
        /// response with the result of the deletion.</returns>
        [HttpDelete("{categoryId}")] public async Task<IActionResult> DeleteAsync(string categoryId)
        {
            var result = await categoryService.DeleteCategoryAsync(categoryId);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves categories associated with a specific entity.
        /// </summary>
        /// <param name="entityId">The ID of the entity.</param>
        /// <returns>A list of categories associated with the entity.</returns>
        [HttpGet("entitycategories/{entityId}")] public async Task<IActionResult> GetEntityCategoriesAsync(string entityId)
        {
            var result = await categoryService.EntityCategoriesAsync(entityId);
            return Ok(result);
        }

        /// <summary>
        /// Adds a category to an entity.
        /// </summary>
        /// <param name="categoryId">The ID of the category.</param>
        /// <param name="entityId">The ID of the entity.</param>
        /// <returns>A result indicating the success of the addition.</returns>
        [HttpPost("entitycategory/{categoryId}/{entityId}")] public async Task<IActionResult> AddEntityCategoryAsync(string categoryId, string entityId)
        {
            var result = await categoryService.CreateEntityCategoryAsync(categoryId, entityId);
            return Ok(result);
        }

        /// <summary>
        /// Removes a category from an entity.
        /// </summary>
        /// <param name="categoryId">The ID of the category.</param>
        /// <param name="entityId">The ID of the entity.</param>
        /// <returns>A result indicating the success of the removal.</returns>
        [HttpDelete("entitycategory/{categoryId}/{entityId}")] public async Task<IActionResult> RemoveEntityCategoryAsync(string categoryId, string entityId)
        {
            var result = await categoryService.RemoveEntityCategoryAsync(categoryId, entityId);
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
            var image = new EntityImage<Category<Lodging>, string>(request.ImageId, request.EntityId) { Selector = request.Selector, Order = request.Order };

            var addResult = await imageRepository.CreateAsync(image, cancellationToken);
            if (!addResult.Succeeded) return await Result.FailAsync(addResult.Messages);

            var saveResult = await imageRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync();
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
            var addResult = await imageRepository.DeleteAsync(imageId, cancellationToken);
            if (!addResult.Succeeded) return await Result.FailAsync(addResult.Messages);

            var saveResult = await imageRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync();
        }

        #endregion
    }
}
