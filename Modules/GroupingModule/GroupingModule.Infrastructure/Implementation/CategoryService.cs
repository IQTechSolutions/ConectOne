using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using FilingModule.Domain.Entities;
using FilingModule.Domain.RequestFeatures;
using GroupingModule.Domain.DataTransferObjects;
using GroupingModule.Domain.Entities;
using GroupingModule.Domain.Interfaces;
using GroupingModule.Domain.RequestFeatures;
using GroupingModule.Domain.Specifications;
using Microsoft.EntityFrameworkCore;

namespace GroupingModule.Infrastructure.Implementation
{
    /// <summary>
    /// Service implementation for managing categories.
    /// Provides methods for CRUD operations and category-specific queries.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    public class CategoryService<T>(IRepository<Category<T>, string> categoryRepository, IRepository<EntityCategory<T>, string> entityCategoryRepository, IRepository<EntityImage<Category<T>, string>, string> imageRepo) : ICategoryService<T> where T : IAuditableEntity<string>, new()
    {
        #region Methods

        /// <summary>
        /// Asynchronously retrieves the count of categories based on the specified pagination and filtering parameters.
        /// </summary>
        /// <remarks>This method queries the repository for categories that match the specified parameters
        /// and returns the count of matching categories. If the operation fails, the result will contain error
        /// messages.</remarks>
        /// <param name="categoryPageParameters">The parameters used to define pagination and filtering for the category query.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// where <c>T</c> is an <see cref="int"/> representing the count of categories that match the specified
        /// parameters.</returns>
        public async Task<IBaseResult<int>> CategoryCountAsync(CategoryPageParameters categoryPageParameters, CancellationToken cancellationToken = default)
        {
            var result = await categoryRepository.ListAsync(new DefaultCategorySpec<T>(categoryPageParameters), false, cancellationToken);
            if (!result.Succeeded) return await Result<int>.FailAsync(result.Messages);
            return await Result<int>.SuccessAsync(result.Data.Count);
        }

        /// <summary>
        /// Retrieves a collection of categories, optionally filtered by a parent category ID.
        /// </summary>
        /// <remarks>The returned categories are ordered in descending order based on the number of their
        /// subcategories.</remarks>
        /// <param name="parentId">The optional ID of the parent category. If specified, only categories under the given parent will be
        /// included. If null, all top-level categories are retrieved.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation will terminate early if the token is canceled.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// where <c>T</c> is an <see cref="IEnumerable{T}"/> of <see cref="CategoryDto"/> objects. The result indicates
        /// whether the operation succeeded and includes the retrieved categories if successful.</returns>
        public async Task<IBaseResult<IEnumerable<CategoryDto>>> CategoriesAsync(string? parentId = null, CancellationToken cancellationToken = default)
        {
            var result = await categoryRepository.ListAsync(new CategoryListSpec<T>(parentId), false, cancellationToken);

            if (!result.Succeeded) return await Result<IEnumerable<CategoryDto>>.FailAsync(result.Messages);

            var response = result.Data.OrderByDescending(c => c.SubCategories.Count());
            return await Result<IEnumerable<CategoryDto>>.SuccessAsync(response.Select(c => CategoryDto.ToCategoryDto(c)));
        }
        
        /// <summary>
        /// Retrieves all bottom-level categories that do not have any subcategories.
        /// </summary>
        /// <remarks>A bottom-level category is defined as a category that does not have any
        /// subcategories. The method filters out deleted categories and includes related data such as images, entity
        /// collections, and subcategories in the query.</remarks>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// where <c>T</c> is an <see cref="IEnumerable{T}"/> of <see cref="CategoryDto"/> objects representing the
        /// bottom-level categories. If the operation fails, the result contains error messages.</returns>
        public async Task<IBaseResult<IEnumerable<CategoryDto>>> AllBottomLevelCategoriesAsync(string? parentId = null, CancellationToken cancellationToken = default)
        {
            var result = await categoryRepository.ListAsync(new CategoryListSpec<T>(parentId), false, cancellationToken);

            if (!result.Succeeded) return await Result<IEnumerable<CategoryDto>>.FailAsync(result.Messages);

            var pp = result.Data.Where(c => !c.SubCategories.Any()).Select(c => CategoryDto.ToCategoryDto(c));
            return await Result<IEnumerable<CategoryDto>>.SuccessAsync(pp);
        }

        /// <summary>
        /// Retrieves a collection of categories associated with a specific entity.
        /// </summary>
        /// <remarks>This method retrieves categories that are not marked as deleted and are associated
        /// with the specified entity. The categories include their related images, if any. The result is distinct and
        /// mapped to <see cref="CategoryDto"/> objects for easier consumption.</remarks>
        /// <param name="entityId">The unique identifier of the entity for which categories are to be retrieved. If <see langword="null"/>, all
        /// categories will be considered.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation will terminate early if the token is canceled.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// where <c>T</c> is an <see cref="IEnumerable{T}"/> of <see cref="CategoryDto"/> objects representing the
        /// categories associated with the specified entity. If the operation fails, the result will include error
        /// messages.</returns>
        public async Task<IBaseResult<IEnumerable<CategoryDto>>> EntityCategoriesAsync(string? entityId = null, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<EntityCategory<T>>(c => !c.IsDeleted && c.EntityId == entityId);
            spec.AddInclude(c => c.Include(category => category.Category).ThenInclude(category => category.Images).ThenInclude(c => c.Image));
            
            var entityCategoryResult = await entityCategoryRepository.ListAsync(spec, false, cancellationToken);

            if (!entityCategoryResult.Succeeded) return await Result<IEnumerable<CategoryDto>>.FailAsync(entityCategoryResult.Messages);

            var response = entityCategoryResult.Data.Distinct().Select(c => CategoryDto.ToCategoryDto(c.Category)).ToList();
            return await Result<IEnumerable<CategoryDto>>.SuccessAsync(response);
        }

        /// <summary>
        /// Retrieves a paginated list of categories based on the specified parameters.
        /// </summary>
        /// <remarks>The method supports filtering categories by name or description using the <see
        /// cref="CategoryPageParameters.SearchText"/> property. The search is case-insensitive and trims any leading or
        /// trailing whitespace from the search text.</remarks>
        /// <param name="categoryPageParameters">The parameters that define the pagination and filtering options, such as page number, page size, and search
        /// text.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a list of <see cref="CategoryDto"/> objects that match the
        /// specified parameters. If no categories are found, the result will indicate failure.</returns>
        public async Task<PaginatedResult<CategoryDto>> PagedCategoriesAsync(CategoryPageParameters categoryPageParameters, CancellationToken cancellationToken = default)
        {
            var spec = new DefaultCategorySpec<T>(categoryPageParameters);
            spec.AddInclude(c => c.Include(g => g.SubCategories).ThenInclude(b => b.SubCategories));
            spec.AddInclude(c => c.Include(g => g.Images).ThenInclude(b => b.Image));

            var result = await categoryRepository.ListAsync(spec, false, cancellationToken);

            if (!result.Succeeded) return PaginatedResult<CategoryDto>.Failure(result.Messages);

            var response = result.Data.ToList();
            if (!string.IsNullOrEmpty(categoryPageParameters.SearchText))
            {
                var lowerCaseTerm = categoryPageParameters.SearchText.Trim().ToLower();
                response = response.Where(c => c.Name.ToLower().Contains(lowerCaseTerm) || c.Description is not null &&  c.Description.ToLower().Contains(lowerCaseTerm)).ToList();
            }

            return PaginatedResult<CategoryDto>.Success(response.Select(c => CategoryDto.ToCategoryDto(c)).ToList(), response.Count(), categoryPageParameters.PageNr, categoryPageParameters.PageSize);
        }

        /// <summary>
        /// Retrieves a category by its unique identifier, including related images, subcategories, and associated
        /// entities.
        /// </summary>
        /// <remarks>This method retrieves the category along with its associated images, subcategories,
        /// and non-deleted entities. If no category matching the specified <paramref name="categoryId"/> is found, the
        /// result will indicate failure.</remarks>
        /// <param name="categoryId">The unique identifier of the category to retrieve. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// with a <see cref="CategoryDto"/> representing the category if found, or an error message if the category
        /// does not exist.</returns>
        public async Task<IBaseResult<CategoryDto>> CategoryAsync(string categoryId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Category<T>>(c => c.Id == categoryId);
            spec.AddInclude(c => c.Include(category => category.Images).ThenInclude(category => category.Image));
            spec.AddInclude(c => c.Include(category => category.EntityCollection.Where(t => !t.IsDeleted)));
            spec.AddInclude(c => c.Include(category => category.SubCategories).ThenInclude(category => category.Images).ThenInclude(category => category.Image));
            
            var result = await categoryRepository.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<CategoryDto>.FailAsync(result.Messages);

            if (result.Data is not null)
                return await Result<CategoryDto>.SuccessAsync(CategoryDto.ToCategoryDto(result.Data));
            return await Result<CategoryDto>.FailAsync($"No category matching id '{categoryId}' was found in the database");
        }

        /// <summary>
        /// Creates a new category asynchronously and returns the result.
        /// </summary>
        /// <remarks>This method creates a new category based on the provided <paramref name="category"/>
        /// object. If the operation is successful, the created category is returned as a <see cref="CategoryDto"/>. If
        /// the operation fails, the result will include error messages describing the failure.</remarks>
        /// <param name="category">The category data transfer object containing the details of the category to create.</param>
        /// <param name="cancellation">An optional <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// where <c>T</c> is <see cref="CategoryDto"/>. The result indicates whether the operation succeeded and, if
        /// successful, includes the created category.</returns>
        public async Task<IBaseResult<CategoryDto>> CreateCategoryAsync(CategoryDto category, CancellationToken cancellationToken = default)
        {
            var categoryEntity = category.ToCategory<T>();
            await categoryRepository.CreateAsync(categoryEntity, cancellationToken);

            var saveResult = await categoryRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result<CategoryDto>.FailAsync(saveResult.Messages);

            var categoryToReturn = CategoryDto.ToCategoryDto(categoryEntity);
            return await Result<CategoryDto>.SuccessAsync(categoryToReturn);
        }

        /// <summary>
        /// Updates an existing category in the database with the provided details.
        /// </summary>
        /// <remarks>This method retrieves the category matching the specified <see
        /// cref="CategoryDto.CategoryId"/> from the database, updates its properties with the values provided in the
        /// <paramref name="category"/> parameter, and saves the changes. If no matching category is found, the
        /// operation fails with an appropriate error message.</remarks>
        /// <param name="category">The <see cref="CategoryDto"/> containing the updated details of the category. The <see
        /// cref="CategoryDto.CategoryId"/> property must match the ID of an existing category.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation. Optional; defaults to <see
        /// cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation. If the update is successful, the result contains a success
        /// message. If the update fails, the result contains error messages.</returns>
        public async Task<IBaseResult> UpdateCategoryAsync(CategoryDto category, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Category<T>>(c => c.Id == category.CategoryId);
            spec.AddInclude(c => c.Include(category => category.Images).ThenInclude(category => category.Image));
            spec.AddInclude(c => c.Include(category => category.EntityCollection.Where(t => !t.IsDeleted)));
            spec.AddInclude(c => c.Include(category => category.SubCategories).ThenInclude(category => category.Images).ThenInclude(category => category.Image));


            var result = await categoryRepository.FirstOrDefaultAsync(spec, true, cancellationToken);
            if (!result.Succeeded) return await Result.FailAsync(result.Messages);

            if (result.Data is null) return await Result<CategoryDto>.FailAsync($"No category matching id '{category.CategoryId}' was found in the database");

            result.Data.Name = category.Name;
            result.Data.Description = category.Description;
            result.Data.Featured = category.Featured;
            result.Data.Active = category.Active;
            result.Data.DisplayCategoryInMainManu = category.DisplayCategoryInMainMenu;
            result.Data.DisplayAsSliderItem = category.DisplayAsSliderItem;
            result.Data.Description = category.Description;
            result.Data.WebTags = category.WebTags;

            categoryRepository.Update(result.Data);

            var saveResult = await categoryRepository.SaveAsync(cancellationToken);
            if (saveResult.Succeeded)
            {
                return await Result.SuccessAsync($"{result.Data.Name} was successfully updated");
            }
            return await Result.FailAsync(saveResult.Messages);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IBaseResult> DeleteCategoryAsync(string categoryId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Category<T>>(c => c.Id == categoryId);
            spec.AddInclude(c => c.Include(category => category.SubCategories));

            var result = await categoryRepository.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result.FailAsync(result.Messages);

            if (result.Data is null) return await Result<CategoryDto>.FailAsync($"No category matching id '{categoryId}' was found in the database");

            result.Data.IsDeleted = true;

            categoryRepository.Update(result.Data);

            if (result.Data.HasSubCategories)
            {
                foreach (var subCateogry in result.Data.SubCategories)
                {
                    subCateogry.IsDeleted = true;
                }
            }

            var saveResult = await categoryRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);
            return await Result.SuccessAsync($"{result.Data.Name} was successfully removed");
        }

        /// <summary>
        /// Creates a new entity-category association.
        /// </summary>
        /// <param name="categoryId">The ID of the category.</param>
        /// <param name="entityId">The ID of the entity.</param>
        /// <returns>A task that represents the asynchronous operation. The task result indicates the success or failure of the operation.</returns>
        public async Task<IBaseResult> CreateEntityCategoryAsync(string categoryId, string entityId, CancellationToken cancellationToken = default)
        {
            var entityCategory = new EntityCategory<T>(entityId, categoryId);
            await entityCategoryRepository.CreateAsync(entityCategory, cancellationToken);
            var saveResult = await entityCategoryRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);
            return await Result.SuccessAsync($"EntityCategory was successfully created");
        }

        /// <summary>
        /// Removes an entity-category association.
        /// </summary>
        /// <param name="categoryId">The ID of the category.</param>
        /// <param name="entityId">The ID of the entity.</param>
        /// <returns>A task that represents the asynchronous operation. The task result indicates the success or failure of the operation.</returns>
        public async Task<IBaseResult> RemoveEntityCategoryAsync(string categoryId, string entityId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<EntityCategory<T>>(c => c.EntityId == entityId && c.CategoryId == categoryId);
            spec.AddInclude(c => c.Include(c => c.Category).ThenInclude(c => c.Images).ThenInclude(c => c.Image));

            var entityCategoryResult = await entityCategoryRepository.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!entityCategoryResult.Succeeded) return await Result.FailAsync(entityCategoryResult.Messages);

            if (entityCategoryResult.Data is null) return await Result<CategoryDto>.FailAsync($"No category matching id '{categoryId}' was found in the database");

            entityCategoryRepository.Delete(entityCategoryResult.Data);

            var saveResult = await entityCategoryRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync($"{entityCategoryResult.Data.Category.Name} was successfully removed");
        }

        #endregion

        #region Images

        /// <summary>
        /// Adds a new image record that links an existing file in the media store
        /// (<see cref="AddEntityImageRequest.ImageId"/>) with a specific entity
        /// (<see cref="AddEntityImageRequest.EntityId"/>).
        /// </summary>
        /// <param name="request">
        /// The DTO carrying the target entity identifier, the image identifier,
        /// a <c>Selector</c> flag describing the image’s purpose (e.g. <c>Main</c>,
        /// <c>Thumbnail</c>), and the desired display <c>Order</c>.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by the caller to abort the operation
        /// before it completes.
        /// </param>
        /// <returns>
        /// <see cref="IBaseResult"/> indicating:
        /// • <c>Success</c> – the link was written to the repository and persisted, or  
        /// • <c>Failure</c> – one of the repository operations failed (in which case
        ///   the aggregated error messages are bubbled back to the caller).
        /// </returns>
        /// <remarks>
        /// The method performs two repository calls—<c>CreateAsync</c> followed by
        /// <c>SaveAsync</c>.  It short-circuits on failure so that no partial state
        /// is persisted.
        /// </remarks>
        public async Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default)
        {
            var image = new EntityImage<Category<T>, string>(request.ImageId, request.EntityId)
            {
                Selector = request.Selector,
                Order = request.Order
            };

            var addResult = await imageRepo.CreateAsync(image, cancellationToken);
            if (!addResult.Succeeded)
                return await Result.FailAsync(addResult.Messages);

            var saveResult = await imageRepo.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded)
                return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync();
        }

        /// <summary>
        /// Removes the link between an entity and an image previously added via
        /// <see cref="AddImage"/>.
        /// </summary>
        /// <param name="imageId">
        /// The primary-key of the <see cref="EntityImage{TEntity,TKey}"/> record to delete.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by the caller to abort the operation
        /// before it completes.
        /// </param>
        /// <returns>
        /// <see cref="IBaseResult"/> indicating success or failure, mirroring the
        /// repository results.  On failure the aggregated repository error messages are
        /// returned to the caller.
        /// </returns>
        /// <remarks>
        /// Just like <see cref="AddImage"/>, this method executes two discrete repository
        /// calls—<c>DeleteAsync</c> followed by <c>SaveAsync</c>—and short-circuits on
        /// failure to avoid inconsistent state.
        /// </remarks>
        public async Task<IBaseResult> RemoveImage(string imageId, CancellationToken cancellationToken = default)
        {
            var deleteResult = await imageRepo.DeleteAsync(imageId, cancellationToken);
            if (!deleteResult.Succeeded)
                return await Result.FailAsync(deleteResult.Messages);

            var saveResult = await imageRepo.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded)
                return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync();
        }

        #endregion
    }
}
