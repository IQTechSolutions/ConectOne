using BloggingModule.Domain.Interfaces;
using ConectOne.Domain.Extensions;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.RequestFeatures;
using GroupingModule.Domain.DataTransferObjects;
using GroupingModule.Domain.RequestFeatures;

namespace BloggingModule.Application.RestServices
{
    /// <summary>
    /// Provides REST-based operations for managing blog post categories, including retrieval, creation, updating,
    /// deletion, and association of categories with entities and images.
    /// </summary>
    /// <param name="provider">The HTTP provider used to perform REST API requests for category operations. Cannot be null.</param>
    public class BlogPostCategoryRestService(IBaseHttpProvider provider) : IBlogPostCategoryService
    {
        /// <summary>
        /// Asynchronously retrieves the total number of categories that match the specified page parameters.
        /// </summary>
        /// <param name="categoryPageParameters">The parameters used to filter and paginate the category results. Cannot be null.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an object with the total count
        /// of categories matching the specified parameters.</returns>
        public async Task<IBaseResult<int>> CategoryCountAsync(CategoryPageParameters categoryPageParameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<int>($"blog/categories/count/{categoryPageParameters.GetQueryString()}");
            return result;
        }

        /// <summary>
        /// Asynchronously retrieves a collection of blog categories, optionally filtered by a parent category
        /// identifier.
        /// </summary>
        /// <param name="parentId">The identifier of the parent category to filter by. If null, retrieves all top-level categories.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with a collection of <see cref="CategoryDto"/> objects representing the categories.</returns>
        public async Task<IBaseResult<IEnumerable<CategoryDto>>> CategoriesAsync(string? parentId = null, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<CategoryDto>>($"blog/categories/{parentId}");
            return result;
        }

        /// <summary>
        /// Retrieves the collection of categories associated with the specified department.
        /// </summary>
        /// <param name="departmentId">The unique identifier of the department for which to retrieve categories. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with a collection of <see cref="CategoryDto"/> objects for the specified department. The collection is empty
        /// if the department has no categories.</returns>
        public async Task<IBaseResult<IEnumerable<CategoryDto>>> DepartmentCategories(string departmentId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<CategoryDto>>($"blog/categories/byDepartment/{departmentId}");
            return result;
        }

        /// <summary>
        /// Asynchronously retrieves all bottom-level blog categories that are descendants of the specified parent
        /// category.
        /// </summary>
        /// <param name="parentId">The identifier of the parent category. If null, retrieves all bottom-level categories across all parent
        /// categories.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a result object with a
        /// collection of bottom-level category data transfer objects. The collection is empty if no categories are
        /// found.</returns>
        public async Task<IBaseResult<IEnumerable<CategoryDto>>> AllBottomLevelCategoriesAsync(string? parentId = null, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<CategoryDto>>($"blog/categories/bottomlevel/{parentId}");
            return result;
        }

        /// <summary>
        /// Asynchronously retrieves the collection of categories associated with a specified entity.
        /// </summary>
        /// <param name="categoryId">The identifier of the entity for which to retrieve associated categories. Can be null to indicate no
        /// specific entity.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with a collection of <see cref="CategoryDto"/> objects representing the categories associated with the
        /// specified entity.</returns>
        public async Task<IBaseResult<IEnumerable<CategoryDto>>> EntityCategoriesAsync(string? categoryId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<CategoryDto>>($"blog/categories/entrycategories/{categoryId}");
            return result;
        }

        /// <summary>
        /// Retrieves a paginated list of blog categories based on the specified paging and filtering parameters.
        /// </summary>
        /// <param name="categoryPageParameters">The parameters that define paging, sorting, and filtering options for the category list. Cannot be null.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a paginated result of category
        /// data transfer objects. The result may be empty if no categories match the specified parameters.</returns>
        public async Task<PaginatedResult<CategoryDto>> PagedCategoriesAsync(CategoryPageParameters categoryPageParameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetPagedAsync<CategoryDto, CategoryPageParameters>($"blog/categories/paged", categoryPageParameters);
            return result;
        }

        /// <summary>
        /// Retrieves the details of a blog category asynchronously by its unique identifier.
        /// </summary>
        /// <param name="categoryId">The unique identifier of the category to retrieve. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="IBaseResult{CategoryDto}"/> with the details of the specified category if found.</returns>
        public async Task<IBaseResult<CategoryDto>> CategoryAsync(string categoryId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<CategoryDto>($"blog/categories/{categoryId}");
            return result;
        }

        /// <summary>
        /// Creates a new blog category asynchronously using the specified category data.
        /// </summary>
        /// <param name="category">The category data to create. Cannot be null.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="IBaseResult{CategoryDto}"/> with the details of the created category.</returns>
        public async Task<IBaseResult<CategoryDto>> CreateCategoryAsync(CategoryDto category, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<CategoryDto, CategoryDto>("blog/categories", category);
            return result;
        }

        /// <summary>
        /// Updates an existing blog category asynchronously using the specified category data.
        /// </summary>
        /// <param name="category">The category data to update. Must not be null.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateCategoryAsync(CategoryDto category, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<CategoryDto, CategoryDto>("blog/categories", category);
            return result;
        }

        /// <summary>
        /// Deletes the blog category with the specified identifier asynchronously.
        /// </summary>
        /// <param name="categoryId">The unique identifier of the category to delete. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the delete operation.</param>
        /// <returns>A task that represents the asynchronous delete operation. The task result contains an object indicating the
        /// outcome of the operation.</returns>
        public async Task<IBaseResult> DeleteCategoryAsync(string categoryId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync("blog/categories", categoryId);
            return result;
        }

        /// <summary>
        /// Creates an association between a category and an entity asynchronously.
        /// </summary>
        /// <param name="categoryId">The unique identifier of the category to associate with the entity. Cannot be null or empty.</param>
        /// <param name="entityId">The unique identifier of the entity to associate with the category. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the association request.</returns>
        public async Task<IBaseResult> CreateEntityCategoryAsync(string categoryId, string entityId, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"blog/categories/entitycategory/{categoryId}/{entityId}");
            return result;
        }

        /// <summary>
        /// Asynchronously removes the association between the specified entity and category.
        /// </summary>
        /// <param name="categoryId">The unique identifier of the category from which the entity association will be removed. Cannot be null or
        /// empty.</param>
        /// <param name="entityId">The unique identifier of the entity to disassociate from the category. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the removal operation.</returns>
        public async Task<IBaseResult> RemoveEntityCategoryAsync(string categoryId, string entityId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"blog/categories/entitycategory/{categoryId}", entityId);
            return result;
        }

        /// <summary>
        /// Adds an image to the specified entity using the provided request data.
        /// </summary>
        /// <param name="request">The request containing the image data and associated entity information. Cannot be null.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the image addition.</returns>
        public async Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"blog/categories/addImage", request);
            return result;
        }

        /// <summary>
        /// Removes the image associated with the specified image identifier.
        /// </summary>
        /// <param name="imageId">The unique identifier of the image to remove. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the remove operation.</returns>
        public async Task<IBaseResult> RemoveImage(string imageId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"blog/categories/deleteImage", imageId);
            return result;
        }
    }
}
