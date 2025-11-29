using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using FilingModule.Domain.Entities;
using FilingModule.Domain.RequestFeatures;
using GroupingModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ProductsModule.Domain.DataTransferObjects;
using ProductsModule.Domain.Entities;
using ProductsModule.Domain.Interfaces;
using ProductsModule.Domain.RequestFeatures;

namespace ProductsModule.Infrastructure.Implementations
{
    /// <summary>
    /// Provides methods for managing services, including retrieval, creation, updating, and deletion of services, as
    /// well as operations related to service categories and associated images.
    /// </summary>
    /// <remarks>This class serves as the implementation of the <see cref="IServiceService"/> interface,
    /// offering a variety of operations for managing services and their related entities. It supports asynchronous
    /// operations for retrieving paginated lists of services, managing service categories, handling images, and
    /// performing CRUD operations on services. The methods in this class return results wrapped in standardized result
    /// types, such as <see cref="IBaseResult"/> and <see cref="PaginatedResult{T}"/>, to indicate success or failure
    /// and provide relevant data or error messages.</remarks>
    public sealed class ServiceService(IRepository<OfferedService, string> serviceRepository, IRepository<EntityImage<OfferedService, string>, string> imageRepository, 
        IRepository<EntityVideo<OfferedService, string>, string> videoRepository, IRepository<Category<OfferedService>, string> categoryRepo, 
        IProductCategoryService productCategoryService) : IServiceService
    {
        /// <summary>
        /// Retrieves a paginated list of services based on the specified parameters.
        /// </summary>
        /// <remarks>This method retrieves services along with their associated pricing and images. The
        /// results are paginated based on the provided <paramref name="serviceParameters"/>. If the operation does not
        /// succeed, the returned <see cref="PaginatedResult{T}"/> will indicate failure and include the relevant error
        /// messages.</remarks>
        /// <param name="serviceParameters">The parameters that define the pagination and filtering options, such as page number and page size.</param>
        /// <param name="trackChanges">A boolean value indicating whether to track changes to the retrieved entities. Set to <see langword="true"/>
        /// to enable change tracking; otherwise, <see langword="false"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a collection of <see cref="ServiceDto"/> objects, the total
        /// count of items, and pagination metadata. If the operation fails, the result will contain error messages.</returns>
        public async Task<PaginatedResult<ServiceDto>> PagedServicesAsync(ServiceParameters serviceParameters, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<OfferedService>(c => true);
            spec.AddInclude(c => c.Include(g => g.Images).ThenInclude(g => g.Image));

            var result = await serviceRepository.ListAsync(spec, trackChanges, cancellationToken);
            if (!result.Succeeded)
                return PaginatedResult<ServiceDto>.Failure(result.Messages);

            return PaginatedResult<ServiceDto>.Success(result.Data.Select(c => new ServiceDto(c)).ToList(), result.Data.Count, serviceParameters.PageNr, serviceParameters.PageSize);
        }

        /// <summary>
        /// Retrieves a collection of services based on the specified parameters.
        /// </summary>
        /// <remarks>This method retrieves services along with their associated pricing and images. The
        /// returned data is mapped  to <see cref="ServiceDto"/> objects for easier consumption. If the operation fails,
        /// the result will indicate  the failure and include relevant error messages.</remarks>
        /// <param name="pageParameters">The parameters used to filter and paginate the services. This includes options such as page size and page
        /// number.</param>
        /// <param name="trackChanges">A boolean value indicating whether to track changes to the retrieved entities.  Set to <see
        /// langword="true"/> to enable change tracking; otherwise, <see langword="false"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// containing an enumerable collection of <see cref="ServiceDto"/> objects. If the operation fails, the result 
        /// includes error messages.</returns>
        public async Task<IBaseResult<IEnumerable<ServiceDto>>> AllServicesAsync(ServiceParameters pageParameters, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<OfferedService>(c => true);
            spec.AddInclude(c => c.Include(g => g.Images).ThenInclude(c => c.Image));

            var result = await serviceRepository.ListAsync(spec,false, cancellationToken);
            if (!result.Succeeded) return await Result<IEnumerable<ServiceDto>>.FailAsync(result.Messages);
            return await Result<IEnumerable<ServiceDto>>.SuccessAsync(result.Data.Select(c => new ServiceDto(c)));
        }

        /// <summary>
        /// Retrieves a collection of services associated with the specified category.
        /// </summary>
        /// <remarks>This method retrieves services for a given category and maps them to <see
        /// cref="ServiceDto"/> objects. If the operation is unsuccessful, the result will include the failure
        /// messages.</remarks>
        /// <param name="categoryId">The unique identifier of the category for which to retrieve services. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// where <c>T</c> is an <see cref="IEnumerable{T}"/> of <see cref="ServiceDto"/> objects representing the
        /// services in the specified category. If the operation fails, the result contains error messages.</returns>
        public async Task<IBaseResult<IEnumerable<ServiceDto>>> CategoryServicesAsync(string categoryId, CancellationToken cancellationToken = default)
        {
            var productList = await GetCategorySedrvicesAsync(categoryId, cancellationToken);
            if (!productList.Succeeded) return await Result<IEnumerable<ServiceDto>>.FailAsync(productList.Messages);

            return await Result<IEnumerable<ServiceDto>>.SuccessAsync(productList.Data.Select(c => new ServiceDto(c)));
        }

        /// <summary>
        /// Retrieves a service by its identifier, including related categories, images, and pricing information.
        /// </summary>
        /// <remarks>This method retrieves a service entity from the database based on the specified
        /// <paramref name="serviceId"/>.  The retrieved service includes its associated categories, images, and pricing
        /// details. If no service matches  the provided identifier, the method returns a failure result with an
        /// appropriate error message.</remarks>
        /// <param name="serviceId">The unique identifier of the service to retrieve.</param>
        /// <param name="trackChanges">A value indicating whether to track changes to the retrieved entities.  Set to <see langword="true"/> to
        /// enable change tracking; otherwise, <see langword="false"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// where <c>T</c> is <see cref="ServiceDto"/>. The result indicates whether the operation succeeded and, if so,
        /// provides the service data. If no matching service is found, the result contains an error message.</returns>
        public async Task<IBaseResult<ServiceDto>> ServiceAsync(string serviceId, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<OfferedService>(c => c.Id == serviceId);
            spec.AddInclude(g => g.Include(p => p.Categories).ThenInclude(v => v.Category));
            spec.AddInclude(g => g.Include(p => p.Images).ThenInclude(v => v.Image));

            var result = await serviceRepository.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<ServiceDto>.FailAsync(result.Messages);
            if (result.Data == null)
                return await Result<ServiceDto>.FailAsync($"No service with id matching '{serviceId}' was found in the database");

            return await Result<ServiceDto>.SuccessAsync(new ServiceDto(result.Data));
        }

        /// <summary>
        /// Creates a new service asynchronously and persists it to the repository.
        /// </summary>
        /// <remarks>This method performs the following operations: <list type="bullet"> <item>Creates a
        /// new service in the repository based on the provided <paramref name="service"/>.</item> <item>Populates the
        /// pricing details of the created service.</item> <item>Associates the service with the specified
        /// categories.</item> <item>Persists the changes to the repository.</item> </list> If the operation fails at
        /// any step, the result will indicate failure and include the relevant error messages.</remarks>
        /// <param name="service">The <see cref="ServiceDto"/> object containing the details of the service to create.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation. Defaults to <see
        /// cref="CancellationToken.None"/> if not provided.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// where <c>T</c> is <see cref="ServiceDto"/>. The result indicates whether the operation succeeded and, if
        /// successful, includes the created service details.</returns>
        public async Task<IBaseResult<ServiceDto>> CreateAsync(ServiceDto service, CancellationToken cancellationToken = default)
        {
            var dService = new OfferedService(service);

            var result = await serviceRepository.CreateAsync(dService, cancellationToken);
            if (!result.Succeeded) return await Result<ServiceDto>.FailAsync(result.Messages);

            foreach (var category in service.Categories)
            {
                await productCategoryService.CreateEntityCategoryAsync(category.CategoryId, service.Id);
            }

            var saveResult = await serviceRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result<ServiceDto>.FailAsync(saveResult.Messages);
            return await Result<ServiceDto>.SuccessAsync("Service was successfully removed");
        }

        /// <summary>
        /// Updates an existing service in the database with the provided details.
        /// </summary>
        /// <remarks>This method performs the following steps: <list type="number"> <item>Checks if a
        /// service with the specified ID exists in the database.</item> <item>If the service is found, updates its
        /// properties with the values from the provided <paramref name="service"/>.</item> <item>Saves the changes to
        /// the database.</item> <item>Returns a success result containing the updated service details, or a failure
        /// result with error messages if the operation fails.</item> </list></remarks>
        /// <param name="service">The <see cref="ServiceDto"/> containing the updated details of the service. The <see cref="ServiceDto.Id"/>
        /// must match the ID of an existing service in the database.</param>
        /// <param name="cancellationToken">An optional <see cref="CancellationToken"/> to observe while waiting for the operation to complete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// of type <see cref="ServiceDto"/>. If the update is successful, the result contains the updated service
        /// details. If the update fails, the result contains error messages describing the failure.</returns>
        public async Task<IBaseResult<ServiceDto>> UpdateAsync(ServiceDto service, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<OfferedService>(c => c.Id == service.Id);
            var result = await serviceRepository.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<ServiceDto>.FailAsync(result.Messages);

            if (result.Data == null)
                return await Result<ServiceDto>.FailAsync($"No service with id matching '{service.Id}' was found in the database");

            result.Data.Name = service.Name;
            result.Data.DisplayName= service.DisplayName;
            result.Data.ShortDescription = service.ShortDescription;
            result.Data.Description = service.Description;
            result.Data.BillingFrequency = service.BillingFrequency;
            result.Data.ServiceFrequency = service.ServiceFrequency;
            result.Data.PriceTableItem = service.PriceTableItem;
            result.Data.Featured = service.Featured;
            result.Data.Active = service.Active;
            result.Data.Tags = service.Tags;
            result.Data.Price = service.Price;
            result.Data.DoNotDisplayInCatalogs = service.DoNotDisplayInCatalogs;

            var saveResult = await serviceRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result<ServiceDto>.FailAsync(saveResult.Messages);
            return await Result<ServiceDto>.SuccessAsync(new ServiceDto(result.Data));
        }

        /// <summary>
        /// Deletes the specified service by its identifier asynchronously.
        /// </summary>
        /// <remarks>This method performs the deletion of a service and ensures that the changes are saved
        /// to the repository. If the deletion or save operation fails, the result will indicate the failure with
        /// appropriate messages.</remarks>
        /// <param name="serviceId">The unique identifier of the service to delete. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation. The default value is <see
        /// cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation. If the operation succeeds, the result contains a success message.
        /// If it fails, the result contains error messages.</returns>
        public async Task<IBaseResult> DeleteAsync(string serviceId, CancellationToken cancellationToken = default)
        {
            var result = await serviceRepository.DeleteAsync(serviceId, cancellationToken);
            if (!result.Succeeded) return await Result.FailAsync(result.Messages);

            var saveResult = await serviceRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);
            return await Result.SuccessAsync("Service was successfully removed");
        }

        #region Images

        /// <summary>
        /// Adds an image to the specified entity.
        /// </summary>
        /// <remarks>This method creates a new image entity and attempts to add it to the repository. It
        /// then saves the changes to the repository. If the operation fails at any step, the method returns a failure
        /// result with the associated error messages.</remarks>
        /// <param name="request">The request containing the image details and the entity to which the image will be added.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
        public async Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default)
        {
            var image = new EntityImage<OfferedService, string>(request.ImageId, request.EntityId) { Selector = request.Selector, Order = request.Order };

            var addResult = await imageRepository.CreateAsync(image, cancellationToken);
            if (!addResult.Succeeded) return await Result.FailAsync(addResult.Messages);

            var saveResult = await imageRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync();
        }

        /// <summary>
        /// Removes an image identified by the specified image ID from the repository.
        /// </summary>
        /// <remarks>This method attempts to delete the image from the repository and then save the
        /// changes. If either operation fails, the method returns a failure result with the associated error
        /// messages.</remarks>
        /// <param name="imageId">The unique identifier of the image to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
        public async Task<IBaseResult> RemoveImage(string imageId, CancellationToken cancellationToken = default)
        {
            var addResult = await imageRepository.DeleteAsync(imageId, cancellationToken);
            if (!addResult.Succeeded) return await Result.FailAsync(addResult.Messages);

            var saveResult = await imageRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync();
        }

        #endregion

        #region Videos

        /// <summary>
        /// Adds a video associated with a specific entity to the repository.
        /// </summary>
        /// <remarks>This method attempts to add a video to the repository and save the changes. If the
        /// operation fails at any step, the result will contain the failure messages.</remarks>
        /// <param name="request">The request containing the video details, including the video ID and the associated entity ID.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> AddVideo(AddEntityVideoRequest request, CancellationToken cancellationToken = default)
        {
            var image = new EntityVideo<OfferedService, string> { VideoId = request.VideoId, EntityId = request.EntityId };

            var addResult = await videoRepository.CreateAsync(image, cancellationToken);
            if (!addResult.Succeeded) return await Result.FailAsync(addResult.Messages);

            var saveResult = await videoRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync();
        }

        /// <summary>
        /// Removes a video with the specified identifier from the repository.
        /// </summary>
        /// <remarks>This method performs two operations: it deletes the video from the repository and
        /// then saves the changes. If either operation fails, the method returns a failure result containing the
        /// associated error messages.</remarks>
        /// <param name="videoId">The unique identifier of the video to be removed. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation. The default value is <see
        /// cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation. If the operation succeeds, the result will indicate success. If the
        /// operation fails, the result will contain error messages.</returns>
        public async Task<IBaseResult> RemoveVideo(string videoId, CancellationToken cancellationToken = default)
        {
            var addResult = await videoRepository.DeleteAsync(videoId, cancellationToken);
            if (!addResult.Succeeded) return await Result.FailAsync(addResult.Messages);

            var saveResult = await videoRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

            return await Result.SuccessAsync();
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Retrieves a collection of products for a specified category or all products if no category is specified.
        /// </summary>
        /// <remarks>If a <paramref name="categoryId"/> is provided, the method retrieves products
        /// specific to that category. If no category is specified, the method retrieves all products that are not
        /// marked as "Do Not Display in Catalogs." The returned products include related data such as images and
        /// pricing.</remarks>
        /// <param name="categoryId">The identifier of the category for which to retrieve products. If <see langword="null"/> or empty, all
        /// products are retrieved.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with a collection of <see cref="Product"/> objects. If the operation fails, the result contains error
        /// messages.</returns>
        public async Task<IBaseResult<IEnumerable<OfferedService>>> GetCategorySedrvicesAsync(string? categoryId = null, CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrEmpty(categoryId))
            {
                var services = new List<OfferedService>();
                var result = (await PopulateServiceItemsList(categoryId, services, cancellationToken));
                if (result.Succeeded)
                {
                    services.AddRange(result.Data);
                    return await Result<IEnumerable<OfferedService>>.SuccessAsync(services.Distinct());
                }
            }
            var categoryNullResult = serviceRepository.FindAll();
            if (categoryNullResult.Succeeded)
            {
                var response = categoryNullResult.Data.Include(c => c.Images);
            }
            return await Result<IEnumerable<OfferedService>>.FailAsync(categoryNullResult.Messages);
        }

        /// <summary>
        /// Populates a list of products by recursively traversing a product category hierarchy.
        /// </summary>
        /// <remarks>This method retrieves the specified category and its subcategories from the database,
        /// including their associated entities, and adds the entities to the provided list of products. If the category
        /// has subcategories, the method is called recursively for each subcategory. The method returns a failure
        /// result if the category is not found or if an error occurs during the operation.</remarks>
        /// <param name="categoryId">The unique identifier of the product category to start the traversal from.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <param name="services">A list of products to populate with the entities found in the specified category and its subcategories.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with a collection of <see cref="Product"/> objects. If the operation succeeds, the result contains the
        /// populated list of products; otherwise, it contains error messages.</returns>
        public async Task<IBaseResult<IEnumerable<OfferedService>>> PopulateServiceItemsList(string categoryId, List<OfferedService> services, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Category<OfferedService>>(c => c.Id == categoryId);
            spec.AddInclude(c => c.Include(g => g.SubCategories));
            spec.AddInclude(c => c.Include(g => g.EntityCollection).ThenInclude(c => c.Entity));
            spec.AddInclude(c => c.Include(g => g.EntityCollection).ThenInclude(c => c.Entity).ThenInclude(c => c.Images).ThenInclude(c => c.Image));

            var result = await categoryRepo.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<IEnumerable<OfferedService>>.FailAsync(result.Messages);
            if (result.Data.SubCategories.Any())
            {
                foreach (var item in result.Data.SubCategories)
                {
                    services = (await PopulateServiceItemsList(item.Id, services)).Data.ToList();
                }
            }

            foreach (var item in result.Data.EntityCollection)
            {
                services.Add(item.Entity);
            }
            return await Result<IEnumerable<OfferedService>>.SuccessAsync(services);
        }

        #endregion
    }
}
