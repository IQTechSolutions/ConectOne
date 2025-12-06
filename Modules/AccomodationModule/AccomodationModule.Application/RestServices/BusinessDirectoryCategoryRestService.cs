using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.Extensions;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.RequestFeatures;
using GroupingModule.Domain.DataTransferObjects;
using GroupingModule.Domain.RequestFeatures;

namespace AccomodationModule.Application.RestServices
{
    public class LodgingCategoryRestService(IBaseHttpProvider provider) : ILodgingCategoryService
    {
        /// <summary>
        /// Asynchronously retrieves the total count of categories based on the specified query parameters.
        /// </summary>
        /// <remarks>The method sends a request to the underlying provider to retrieve the category count.
        /// Ensure that the <paramref name="categoryPageParameters"/> object is properly configured to reflect the
        /// desired query.</remarks>
        /// <param name="categoryPageParameters">The parameters used to filter and paginate the category count query. Must not be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// with the total count of categories matching the specified parameters.</returns>
        public async Task<IBaseResult<int>> CategoryCountAsync(CategoryPageParameters categoryPageParameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<int>($"lodgings/categories/count/{categoryPageParameters.GetQueryString()}");
            return result;
        }

        /// <summary>
        /// Retrieves a collection of product categories, optionally filtered by a parent category ID.
        /// </summary>
        /// <remarks>The method fetches categories from the underlying data provider. If a parent category
        /// ID is provided, the result is scoped to categories under that parent; otherwise, all categories are
        /// returned.</remarks>
        /// <param name="parentId">The optional ID of the parent category. If specified, only categories under the given parent category are
        /// retrieved. If null or empty, all categories are retrieved.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation is canceled if the token is triggered.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// containing an enumerable collection of <see cref="CategoryDto"/> objects representing the retrieved
        /// categories.</returns>
        public async Task<IBaseResult<IEnumerable<CategoryDto>>> CategoriesAsync(string? parentId = null, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<CategoryDto>>(string.IsNullOrEmpty(parentId) ? "lodgings/categories/all" : $"lodgings/categories/all/{parentId}");
            return result;
        }

        /// <summary>
        /// Retrieves a collection of categories associated with the specified department.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch categories for a given
        /// department. Ensure that the <paramref name="departmentId"/> corresponds to a valid department in the
        /// system.</remarks>
        /// <param name="departmentId">The unique identifier of the department whose categories are to be retrieved. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// containing an enumerable collection of <see cref="CategoryDto"/> objects representing the categories of the
        /// specified department.</returns>
        public async Task<IBaseResult<IEnumerable<CategoryDto>>> DepartmentCategories(string departmentId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<CategoryDto>>($"lodgings/categories/byDepartment/{departmentId}");
            return result;
        }

        /// <summary>
        /// Retrieves all bottom-level categories, optionally filtered by a parent category ID.
        /// </summary>
        /// <remarks>Bottom-level categories are categories that do not have any child
        /// categories.</remarks>
        /// <param name="parentId">The ID of the parent category to filter the results. If <see langword="null"/> or empty,  all bottom-level
        /// categories are retrieved.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult{T}"/>
        /// containing an enumerable collection of <see cref="CategoryDto"/>  objects representing the bottom-level
        /// categories.</returns>
        public async Task<IBaseResult<IEnumerable<CategoryDto>>> AllBottomLevelCategoriesAsync(string? parentId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<CategoryDto>>(string.IsNullOrEmpty(parentId) ? "lodgings/categories/bottomlevel" : $"lodgings/categories/bottomlevel/{parentId}");
            return result;
        }

        /// <summary>
        /// Retrieves the categories associated with a specified entity.
        /// </summary>
        /// <remarks>If <paramref name="entityId"/> is <see langword="null"/>, the method retrieves
        /// categories for all entities.</remarks>
        /// <param name="entityId">The unique identifier of the entity whose categories are to be retrieved.  Can be <see langword="null"/> to
        /// retrieve categories for all entities.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// of <see cref="IEnumerable{T}"/> containing <see cref="CategoryDto"/> objects representing the categories 
        /// associated with the specified entity.</returns>
        public async Task<IBaseResult<IEnumerable<CategoryDto>>> EntityCategoriesAsync(string? entityId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<CategoryDto>>($"lodgings/categories/entitycategories/{entityId}");
            return result;
        }

        /// <summary>
        /// Retrieves a paginated list of categories based on the specified paging parameters.
        /// </summary>
        /// <remarks>This method sends a request to the "products/categories/paged" endpoint to retrieve
        /// the paginated data. Ensure that the <paramref name="categoryPageParameters"/> object is properly configured
        /// to avoid invalid requests.</remarks>
        /// <param name="categoryPageParameters">The parameters that define the paging and filtering options for the categories.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a collection of <see cref="CategoryDto"/> objects and metadata
        /// about the pagination.</returns>
        public async Task<PaginatedResult<CategoryDto>> PagedCategoriesAsync(CategoryPageParameters categoryPageParameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetPagedAsync<CategoryDto, CategoryPageParameters>("lodgings/categories/paged", categoryPageParameters);
            return result;
        }

        /// <summary>
        /// Retrieves the details of a specific product category asynchronously.
        /// </summary>
        /// <remarks>This method uses the underlying provider to fetch category details. Ensure that the
        /// <paramref name="categoryId"/> corresponds to a valid category in the system.</remarks>
        /// <param name="categoryId">The unique identifier of the category to retrieve. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the details of the category as a <see cref="CategoryDto"/>. If the category is not found, the
        /// result may indicate an error or an empty value, depending on the implementation of the provider.</returns>
        public async Task<IBaseResult<CategoryDto>> CategoryAsync(string categoryId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<CategoryDto>($"lodgings/categories/{categoryId}");
            return result;
        }

        /// <summary>
        /// Creates a new product category asynchronously.
        /// </summary>
        /// <remarks>This method sends a PUT request to the "products/categories" endpoint to create the
        /// specified category.</remarks>
        /// <param name="category">The category data to be created. Must not be <see langword="null"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that includes the created category data.</returns>
        public async Task<IBaseResult<CategoryDto>> CreateCategoryAsync(CategoryDto category, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<CategoryDto, CategoryDto>($"lodgings/categories", category);
            return result;
        }

        /// <summary>
        /// Updates an existing product category asynchronously.
        /// </summary>
        /// <remarks>This method sends the updated category details to the server and returns the result
        /// of the operation. Ensure that the <paramref name="category"/> parameter contains valid data before calling
        /// this method.</remarks>
        /// <param name="category">The <see cref="CategoryDto"/> object containing the updated category details.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateCategoryAsync(CategoryDto category, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<CategoryDto, CategoryDto>($"lodgings/categories", category);
            return result;
        }

        /// <summary>
        /// Deletes a product category with the specified identifier.
        /// </summary>
        /// <remarks>This method sends a request to delete the specified category. Ensure that the
        /// category ID is valid and exists before calling this method.</remarks>
        /// <param name="categoryId">The unique identifier of the category to delete. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>An <see cref="IBaseResult"/> representing the result of the delete operation. The result indicates whether
        /// the operation was successful.</returns>
        public async Task<IBaseResult> DeleteCategoryAsync(string categoryId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"lodgings/categories", categoryId);
            return result;
        }

        /// <summary>
        /// Creates an association between a category and an entity asynchronously.
        /// </summary>
        /// <remarks>This method sends a POST request to associate the specified category with the
        /// specified entity. Ensure that both <paramref name="categoryId"/> and <paramref name="entityId"/> are valid
        /// and exist in the system.</remarks>
        /// <param name="categoryId">The unique identifier of the category to associate with the entity. Cannot be null or empty.</param>
        /// <param name="entityId">The unique identifier of the entity to associate with the category. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> CreateEntityCategoryAsync(string categoryId, string entityId, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"lodgings/categories/entitycategory/{categoryId}/{entityId}");
            return result;
        }

        /// <summary>
        /// Removes the association between a category and an entity asynchronously.
        /// </summary>
        /// <remarks>This method sends a request to remove the specified category-entity association.
        /// Ensure that both  <paramref name="categoryId"/> and <paramref name="entityId"/> are valid and exist in the
        /// system.</remarks>
        /// <param name="categoryId">The unique identifier of the category to be disassociated.</param>
        /// <param name="entityId">The unique identifier of the entity to be disassociated from the category.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveEntityCategoryAsync(string categoryId, string entityId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"lodgings/categories/entitycategory/{categoryId}/{entityId}", "");
            return result;
        }

        /// <summary>
        /// Adds an image to a product category.
        /// </summary>
        /// <remarks>This method sends a request to add an image to a product category. Ensure that the
        /// <paramref name="request"/> contains valid data before calling this method. The operation is performed
        /// asynchronously.</remarks>
        /// <param name="request">The request containing the details of the image to be added, including the product category and image data.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"lodgings/categories/addImage", request);
            return result;
        }

        /// <summary>
        /// Removes an image associated with a product category.
        /// </summary>
        /// <remarks>This method sends a request to delete the specified image from the product category.
        /// Ensure that the  <paramref name="imageId"/> corresponds to a valid image before calling this
        /// method.</remarks>
        /// <param name="imageId">The unique identifier of the image to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveImage(string imageId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"lodgings/categories/deleteImage", imageId);
            return result;
        }
    }
}
