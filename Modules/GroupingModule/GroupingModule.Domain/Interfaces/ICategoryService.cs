using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.RequestFeatures;
using GroupingModule.Domain.DataTransferObjects;
using GroupingModule.Domain.RequestFeatures;

namespace GroupingModule.Domain.Interfaces
{
    /// <summary>
    /// Interface for category service operations.
    /// Provides methods for managing categories, including CRUD operations and category-specific queries.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface ICategoryService<TEntity> where TEntity : IAuditableEntity<string>
    {
        #region Category Count

        /// <summary>
        /// Gets the count of categories based on the specified parameters.
        /// </summary>
        /// <param name="categoryPageParameters">The parameters for filtering categories.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the count of categories.</returns>
        Task<IBaseResult<int>> CategoryCountAsync(CategoryPageParameters categoryPageParameters, CancellationToken cancellationToken = default);

        #endregion

        #region Category Retrieval

        /// <summary>
        /// Gets a list of categories, optionally filtered by parent ID.
        /// </summary>
        /// <param name="parentId">The ID of the parent category, if any.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of categories.</returns>
        Task<IBaseResult<IEnumerable<CategoryDto>>> CategoriesAsync(string? parentId = null, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets a list of all bottom-level categories.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of bottom-level categories.</returns>
        Task<IBaseResult<IEnumerable<CategoryDto>>> AllBottomLevelCategoriesAsync(string? parentId = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a list of categories associated with a specific entity.
        /// </summary>
        /// <param name="categoryId">The ID of the category.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of entity categories.</returns>
        Task<IBaseResult<IEnumerable<CategoryDto>>> EntityCategoriesAsync(string? categoryId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a paginated list of categories based on the specified parameters.
        /// </summary>
        /// <param name="categoryPageParameters">The parameters for filtering and paging categories.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the paginated list of categories.</returns>
        Task<PaginatedResult<CategoryDto>> PagedCategoriesAsync(CategoryPageParameters categoryPageParameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a specific category by its ID.
        /// </summary>
        /// <param name="categoryId">The ID of the category.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the category.</returns>
        Task<IBaseResult<CategoryDto>> CategoryAsync(string categoryId, CancellationToken cancellationToken = default);

        #endregion

        #region Category Management

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="category">The category to create.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created category.</returns>
        Task<IBaseResult<CategoryDto>> CreateCategoryAsync(CategoryDto category, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="category">The category to update.</param>
        /// <returns>A task that represents the asynchronous operation. The task result indicates the success or failure of the operation.</returns>
        Task<IBaseResult> UpdateCategoryAsync(CategoryDto category, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a category by its ID.
        /// </summary>
        /// <param name="categoryId">The ID of the category to delete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result indicates the success or failure of the operation.</returns>
        Task<IBaseResult> DeleteCategoryAsync(string categoryId, CancellationToken cancellationToken = default);

        #endregion

        #region Entity Category Management

        /// <summary>
        /// Creates a new entity-category association.
        /// </summary>
        /// <param name="categoryId">The ID of the category.</param>
        /// <param name="entityId">The ID of the entity.</param>
        /// <returns>A task that represents the asynchronous operation. The task result indicates the success or failure of the operation.</returns>
        Task<IBaseResult> CreateEntityCategoryAsync(string categoryId, string entityId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes an entity-category association.
        /// </summary>
        /// <param name="categoryId">The ID of the category.</param>
        /// <param name="entityId">The ID of the entity.</param>
        /// <returns>A task that represents the asynchronous operation. The task result indicates the success or failure of the operation.</returns>
        Task<IBaseResult> RemoveEntityCategoryAsync(string categoryId, string entityId, CancellationToken cancellationToken = default);

        #endregion

        /// <summary>
        /// Adds an image to the specified entity.
        /// </summary>
        /// <remarks>The operation may fail if the request is invalid or if the entity does not exist.
        /// Ensure that the <paramref name="request"/> parameter is properly populated before calling this
        /// method.</remarks>
        /// <param name="request">The request containing the details of the entity and the image to be added.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes an image with the specified identifier.
        /// </summary>
        /// <param name="imageId">The unique identifier of the image to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        Task<IBaseResult> RemoveImage(string imageId, CancellationToken cancellationToken = default);
    }
}
