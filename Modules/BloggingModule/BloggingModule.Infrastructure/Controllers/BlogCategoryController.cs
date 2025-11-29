using BloggingModule.Domain.Entities;
using BloggingModule.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using FilingModule.Domain.Entities;
using FilingModule.Domain.RequestFeatures;
using GroupingModule.Domain.DataTransferObjects;
using GroupingModule.Domain.Entities;
using GroupingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloggingModule.Infrastructure.Controllers
{
    /// <summary>
    /// The BlogCategoryController handles API requests related to blog categories.
    /// It provides endpoints for creating, updating, deleting, and retrieving blog categories.
    /// </summary>
    [Route("api/blog/categories"), ApiController, Authorize(AuthenticationSchemes = "Bearer")]
    public class BlogCategoryController(IBlogPostCategoryService blogPostCategoryService, IRepository<EntityImage<Category<BlogPost>, string>, string> imageRepository) : ControllerBase
    {
        /// <summary>
        /// Retrieves the total number of categories that match the specified filtering and paging parameters.
        /// </summary>
        /// <param name="parameters">The parameters used to filter and page the category results. Cannot be null.</param>
        /// <returns>An <see cref="IActionResult"/> containing the total count of categories that match the specified criteria.</returns>
        [HttpGet("count")] public async Task<IActionResult> GetCountAsync([FromQuery] CategoryPageParameters parameters)
        {
            var result = await blogPostCategoryService.CategoryCountAsync(parameters);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a paginated list of blog categories based on the provided parameters.
        /// </summary>
        /// <param name="categoryPageParameters">The parameters for pagination and filtering.</param>
        /// <returns>A paginated list of blog categories.</returns>
        [HttpGet("paged")] public async Task<IActionResult> GetPagedBlogCategories([FromQuery] CategoryPageParameters categoryPageParameters)
        {
            var categories = await blogPostCategoryService.PagedCategoriesAsync(categoryPageParameters);
            return Ok(categories);
        }

        /// <summary>
        /// Retrieves all blog categories.
        /// </summary>
        /// <returns>A list of all blog categories.</returns>
        [HttpGet("all")] public async Task<IActionResult> GetAllBlogCategories()
        {
            var categoryEntries = await blogPostCategoryService.CategoriesAsync();
            return Ok(categoryEntries);
        }

        /// <summary>
        /// Retrieves all activity categories that do not have any child categories.
        /// </summary>
        /// <remarks>Bottom-level activity categories are those that do not serve as parents to any other
        /// categories. This endpoint is typically used to display selectable categories to end users.</remarks>
        /// <returns>An <see cref="IActionResult"/> containing a collection of bottom-level activity categories. The result is
        /// returned with an HTTP 200 status code.</returns>
        [HttpGet("bottomlevel")] public async Task<IActionResult> GetAllBottomLevelActivityCategories()
        {
            var result = await blogPostCategoryService.AllBottomLevelCategoriesAsync();
            return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpGet("bottomlevel/{parentId}")] public async Task<IActionResult> GetAllBottomLevelActivityCategories(string parentId)
        {
            var result = await blogPostCategoryService.AllBottomLevelCategoriesAsync(parentId);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves blog categories, optionally filtered by parent category ID.
        /// </summary>
        /// <param name="parentId">The ID of the parent category to filter by.</param>
        /// <returns>A list of blog categories.</returns>
        [HttpGet] public async Task<IActionResult> GetBlogCategories(string? parentId = null)
        {
            var categoryEntries = await blogPostCategoryService.CategoriesAsync(parentId);
            return Ok(categoryEntries);
        }

        /// <summary>
        /// Retrieves a specific blog category by its ID.
        /// </summary>
        /// <param name="categoryId">The ID of the category to retrieve.</param>
        /// <returns>The blog category with the specified ID.</returns>
        [HttpGet("{categoryId}")] public async Task<IActionResult> GetBlogCategory(string categoryId)
        {
            var category = await blogPostCategoryService.CategoryAsync(categoryId);
            return Ok(category);
        }

        /// <summary>
        /// Creates a new blog category.
        /// </summary>
        /// <param name="model">The category data to create.</param>
        /// <returns>The created blog category.</returns>
        [HttpPut] public async Task<IActionResult> CreateBlogCategory([FromBody] CategoryDto model)
        {
            var createdCategory = await blogPostCategoryService.CreateCategoryAsync(model);
            return Ok(createdCategory);
        }

        /// <summary>
        /// Updates an existing blog category.
        /// </summary>
        /// <param name="model">The category data to update.</param>
        /// <returns>The result of the update operation.</returns>
        [HttpPost] public async Task<IActionResult> UpdateBlogCategory([FromBody] CategoryDto model)
        {
            var result = await blogPostCategoryService.UpdateCategoryAsync(model);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a specific blog category by its ID.
        /// </summary>
        /// <param name="categoryId">The ID of the category to delete.</param>
        /// <returns>The result of the delete operation.</returns>
        [HttpDelete("{categoryId}")] public async Task<IActionResult> RemoveBlogCategory(string categoryId)
        {
            var result = await blogPostCategoryService.DeleteCategoryAsync(categoryId);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves the categories associated with a specific entity.
        /// </summary>
        /// <param name="entityId">The ID of the entity to retrieve categories for.</param>
        /// <returns>A list of categories associated with the specified entity.</returns>
        [HttpGet("entrycategories/{entityId}")] public async Task<IActionResult> GetEntityCategoriesAsync(string entityId)
        {
            var categoryEntries = await blogPostCategoryService.EntityCategoriesAsync(entityId);
            return Ok(categoryEntries);
        }

        /// <summary>
        /// Adds a category to a specific entity.
        /// </summary>
        /// <param name="categoryId">The ID of the category to add.</param>
        /// <param name="entityId">The ID of the entity to add the category to.</param>
        /// <returns>No content.</returns>
        [HttpPost("entitycategory")] public async Task<IActionResult> AddBlogEntityCategoryAsync(string categoryId, string entityId)
        {
            await blogPostCategoryService.CreateEntityCategoryAsync(categoryId, entityId);
            return NoContent();
        }

        /// <summary>
        /// Removes a category from a specific entity.
        /// </summary>
        /// <param name="categoryId">The ID of the category to remove.</param>
        /// <param name="entityId">The ID of the entity to remove the category from.</param>
        /// <returns>No content.</returns>
        [HttpDelete("entitycategory")] public async Task<IActionResult> RemoveBlogEntityCategoryAsync(string categoryId, string entityId)
        {
            await blogPostCategoryService.RemoveEntityCategoryAsync(categoryId, entityId);
            return NoContent();
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
            var image = new EntityImage<Category<BlogPost>, string>(request.ImageId, request.EntityId) { Selector = request.Selector, Order = request.Order };

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
