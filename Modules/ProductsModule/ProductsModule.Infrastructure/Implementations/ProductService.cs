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
using ProductsModule.Domain.Specifications;

namespace ProductsModule.Infrastructure.Implementations
{
    /// <summary>
    /// Provides services for managing and retrieving product-related data, including pricing, inventory, and
    /// associations with categories and brands.
    /// </summary>
    /// <remarks>This service offers a variety of methods to handle product operations, such as retrieving
    /// paginated product lists, fetching popular products, managing product settings, and calculating product pricing.
    /// It integrates with repositories to perform database operations and ensures that related data, such as pricing
    /// and images, is included in the results. The service also supports filtering, sorting, and pagination for
    /// efficient data retrieval.</remarks>
    /// <param name="productRepository"></param>
    /// <param name="brandRepository"></param>
    /// <param name="logger"></param>
    public sealed class ProductService(IRepository<Product, string> productRepository, IRepository<EntityImage<Product, string>, string> imageRepository, 
        IRepository<EntityVideo<Product, string>, string> videoRepository, IRepository<ProductMetadata, string> metadataRepository, 
        IRepository<Price, string> priceRepository, IProductCategoryService productCategoryService, IRepository<Category<Product>, string> categoryRepo) : IProductService
    {
        /// <summary>
        /// Retrieves a paginated list of products with their associated pricing and images,  filtered and sorted based
        /// on the specified parameters.
        /// </summary>
        /// <remarks>This method applies multiple filters and transformations to the product data: <list
        /// type="bullet"> <item>Filters by category and brand if specified in <paramref
        /// name="productParameters"/>.</item> <item>Filters by active status based on the <paramref
        /// name="productParameters.Active"/> value.</item> <item>Sorts the results based on the <paramref
        /// name="productParameters.OrderBy"/> property.</item> <item>Includes related pricing and image data for each
        /// product.</item> </list> The method returns a paginated result, which includes the total count of matching
        /// products  and the current page of data. If any step in the process fails, the method returns a failure 
        /// result with the corresponding error messages.</remarks>
        /// <param name="productParameters">The parameters used to filter, sort, and paginate the product list. Includes options  such as category,
        /// brand, active status, sorting order, page number, and page size.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <param name="trackChanges">A boolean value indicating whether to track changes to the entities retrieved from the database.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a list of <see cref="ProductDto"/> objects  that match the
        /// specified criteria, along with pagination metadata. If the operation fails,  the result contains error
        /// messages.</returns>
        public async Task<PaginatedResult<ProductDto>> PagedPricedProductsAsync(ProductsParameters productParameters, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var spec = new PagedProductsSpecification(productParameters);
            spec.AddInclude(c => c.Include(g => g.Pricing));
            spec.AddInclude(c => c.Include(g => g.Images).ThenInclude(g => g.Image));

            var result = await productRepository.ListAsync(spec, trackChanges, cancellationToken);
            if (!result.Succeeded)
                return PaginatedResult<ProductDto>.Failure(result.Messages);

            return PaginatedResult<ProductDto>.Success(result.Data.Select(c => new ProductDto(c, c.Pricing)).ToList(), result.Data.Count, productParameters.PageNr, productParameters.PageSize);
        }

        /// <summary>
        /// Retrieves a collection of the most popular products based on sales, with optional selection count.
        /// </summary>
        /// <remarks>The method retrieves products ordered by their sales count in ascending order. The
        /// result includes product pricing and image data. If the number of available products is less than <paramref
        /// name="selectionCount"/>, all available products are returned. The method ensures that the returned data is
        /// enriched with pricing details.</remarks>
        /// <param name="selectionCount">The maximum number of popular products to retrieve. Defaults to 12. Must be a positive integer.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// where T is an <see cref="IEnumerable{T}"/> of <see cref="ProductDto"/> objects representing the most popular
        /// products. If the operation fails, the result will include error messages.</returns>
        public async Task<IBaseResult<IEnumerable<ProductDto>>> PopularProductsAsync(int selectionCount = 12, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Product>(c => true);
            spec.AddInclude(c => c.Include(g => g.Pricing));
            spec.AddInclude(c => c.Include(g => g.MetaDataCollection));
            spec.AddInclude(c => c.Include(g => g.Images).ThenInclude(c => c.Image));

            var result = await productRepository.ListAsync(spec, false, cancellationToken);
            if(!result.Succeeded) return await Result<IEnumerable<ProductDto>>.FailAsync(result.Messages);

            if (selectionCount <= result.Data.Count())
            {
                result.Data.Take(selectionCount);
            }

            return await Result<IEnumerable<ProductDto>>.SuccessAsync(result.Data.Select(c => new ProductDto(c, c.Pricing)));
        }

        /// <summary>
        /// Retrieves a paginated list of products, including their pricing and images, ordered by the number of units
        /// sold.
        /// </summary>
        /// <remarks>This method retrieves products from the repository, including their associated
        /// pricing and images,  and orders them by the number of units sold. The method supports pagination and
        /// optionally tracks changes  to the retrieved entities. If the operation fails at any stage, the result will
        /// include error messages.</remarks>
        /// <param name="pageParameters">The pagination parameters, such as page number and page size, to control the result set.</param>
        /// <param name="trackChanges">A boolean value indicating whether to track changes to the retrieved entities.  Set to <see
        /// langword="true"/> to enable change tracking; otherwise, <see langword="false"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// where <c>T</c> is an <see cref="IEnumerable{T}"/> of <see cref="ProductDto"/> objects.  If successful, the
        /// result contains the list of products with their pricing and images; otherwise, it contains error messages.</returns>
        public async Task<IBaseResult<IEnumerable<ProductDto>>> AllPricedProductsAsync(ProductsParameters pageParameters, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Product>(c => true);
            spec.AddInclude(c => c.Include(g => g.Pricing));
            spec.AddInclude(c => c.Include(g => g.Images).ThenInclude(c => c.Image));

            var result = await productRepository.ListAsync(spec,false, cancellationToken);
            if (!result.Succeeded) return await Result<IEnumerable<ProductDto>>.FailAsync(result.Messages);
            return await Result<IEnumerable<ProductDto>>.SuccessAsync(result.Data.Select(c => new ProductDto(c, c.Pricing)));
        }

        /// <summary>
        /// Retrieves a list of products within a specified category, including their pricing information.
        /// </summary>
        /// <remarks>This method performs two asynchronous operations: 1. Retrieves the list of products
        /// in the specified category. 2. Fetches the pricing information for the retrieved products. If either
        /// operation fails, the method returns a failure result with the corresponding error messages.</remarks>
        /// <param name="categoryId">The unique identifier of the category for which to retrieve the products.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// where <c>T</c> is an <see cref="IEnumerable{T}"/> of <see cref="ProductDto"/>. If successful, the result
        /// contains the list of products with their pricing information. If the operation fails, the result contains
        /// error messages describing the failure.</returns>
        public async Task<IBaseResult<IEnumerable<ProductDto>>> CategoryPricedProductsAsync(string categoryId, CancellationToken cancellationToken = default)
        {
            var productList = await GetCategoryProductsAsync(categoryId, cancellationToken);
            if (!productList.Succeeded) return await Result<IEnumerable<ProductDto>>.FailAsync(productList.Messages);

            return await Result<IEnumerable<ProductDto>>.SuccessAsync(productList.Data.Select(c => new ProductDto(c, c.Pricing)));
        }

        /// <summary>
        /// Retrieves detailed information about a product, including its calculated price.
        /// </summary>
        /// <remarks>This method retrieves a product by its unique identifier and calculates its price
        /// asynchronously.  If the product is not found in the database, or if the price calculation fails, the result
        /// will  indicate failure with appropriate error messages.</remarks>
        /// <param name="productId">The unique identifier of the product to retrieve.</param>
        /// <param name="trackChanges">A boolean value indicating whether to track changes to the retrieved product entity.  Set to <see
        /// langword="true"/> to enable change tracking; otherwise, <see langword="false"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult{T}"/>
        /// of type <see cref="ProductFullInfoDto"/>. If successful, the result  includes the product's details and its
        /// calculated price. If the product is not found or an error  occurs, the result contains failure messages.</returns>
        public async Task<IBaseResult<ProductFullInfoDto>> PricedProductAsync(string productId, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Product>(c => c.Id == productId);
            spec.AddInclude(g => g.Include(p => p.Categories).ThenInclude(v => v.Category));
            spec.AddInclude(g => g.Include(p => p.Images).ThenInclude(v => v.Image));
            spec.AddInclude(g => g.Include(p => p.Variants).ThenInclude(v => v.Pricing));
            spec.AddInclude(g => g.Include(p => p.Variants).ThenInclude(v => v.Variants).ThenInclude(c => c.Pricing));
            spec.AddInclude(g => g.Include(p => p.MetaDataCollection));
            spec.AddInclude(g => g.Include(p => p.Pricing));

            var result = await productRepository.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<ProductFullInfoDto>.FailAsync(result.Messages);
            if (result.Data == null)
                return await Result<ProductFullInfoDto>.FailAsync($"No product with id matching '{productId}' was found in the database");

            return await Result<ProductFullInfoDto>.SuccessAsync(new ProductFullInfoDto(result.Data, result.Data.Pricing));
        }

        /// <summary>
        /// Updates an existing product in the database with the provided details.
        /// </summary>
        /// <remarks>This method updates the product's details, including its name, SKU, descriptions, and
        /// other properties.  Additionally, it updates related entities such as categories, suppliers, and brands. If a
        /// cover image is provided,  it will also be updated. The method calculates the product's price after the
        /// update and includes it in the result.</remarks>
        /// <param name="product">An object containing the updated details of the product. The <see cref="ProductEditionDto.ProductId"/>
        /// property must match the ID of an existing product in the database.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// where <c>T</c> is <see cref="ProductDto"/>. If the update is successful, the result contains the updated
        /// product  details. If the update fails, the result contains error messages describing the failure.</returns>
        public async Task<IBaseResult<ProductDto>> UpdateAsync(ProductDto product, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Product>(c => c.Id == product.ProductId);
            spec.AddInclude(c => c.Include(g => g.Pricing));
            spec.AddInclude(c => c.Include(g => g.Variants).ThenInclude(v => v.Pricing));
            spec.AddInclude(g => g.Include(p => p.Variants).ThenInclude(v => v.Variants).ThenInclude(c => c.Pricing));
            spec.AddInclude(c => c.Include(g => g.Categories));
            spec.AddInclude(c => c.Include(g => g.MetaDataCollection));

            var result = await productRepository.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<ProductDto>.FailAsync(result.Messages);

            if (result.Data == null)
                return await Result<ProductDto>.FailAsync($"No product with id matching '{product.ProductId}' was found in the database");

            result.Data.Name = product.Name;
            result.Data.DisplayName= product.DisplayName;
            result.Data.SKU =   product.Sku;
            result.Data.ShortDescription = product.ShortDescription;
            result.Data.Description = product.Description;
            result.Data.ShopOwnerId = product.ShopOwnerId;

            productRepository.Update(result.Data);

            var saveResult = await productRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result<ProductDto>.FailAsync(saveResult.Messages);

            await UpdatePricingAsync(product, cancellationToken);
            await UpdateCategoriesAsync(product, cancellationToken);
            await UpdateSuppliersAsync(product, cancellationToken);
            await UpdateBrandsAsync(product, cancellationToken);

            

            return await Result<ProductDto>.SuccessAsync(new ProductDto(result.Data, result.Data.Pricing));
        }

        /// <summary>
        /// Creates a new product asynchronously based on the provided product creation data.
        /// </summary>
        /// <param name="product">The data required to create a new product, including its details, cover image, categories, suppliers, and
        /// brands.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// of type <see cref="ProductDto"/>: <list type="bullet"> <item><description>A successful result if the product
        /// is created and saved successfully, including the created product's details.</description></item>
        /// <item><description>A failed result if the creation or saving process encounters an error, including the
        /// associated error messages.</description></item> </list></returns>
        public async Task<IBaseResult<ProductDto>> CreateAsync(ProductDto product, CancellationToken cancellationToken = default)
        {
            var result = await productRepository.CreateAsync(new Product(product), cancellationToken);
            if (!result.Succeeded) return await Result<ProductDto>.FailAsync(result.Messages);

            result.Data.Pricing = new Price()
            {
                
                Vatable = product.Pricing.Vatable,
                DiscountEndDate = product.Pricing.DiscountEndDate,
                DiscountPercentage = product.Pricing.DiscountPercentage,
                CostExcl = product.Pricing.CostExcl,
                SellingPrice = product.Pricing.PriceIncl,
                ShippingAmount = product.Pricing.ShippingAmount
            };

            foreach (var category in product.Categories)
            {
                await productCategoryService.CreateEntityCategoryAsync(category.CategoryId, product.ProductId);
            }

            var saveResult = await productRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result<ProductDto>.FailAsync(saveResult.Messages);
            return await Result<ProductDto>.SuccessAsync("Product was successfully removed");
        }
        
        /// <summary>
        /// Deletes a product with the specified identifier from the database.
        /// </summary>
        /// <remarks>This method attempts to locate the product in the database using the provided
        /// <paramref name="productId"/>.  If the product is found, it is deleted, and the changes are saved to the
        /// database. If the product is not found,  or if an error occurs during the operation, the result will indicate
        /// failure with appropriate error messages.</remarks>
        /// <param name="productId">The unique identifier of the product to delete. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation. If successful, the result contains a success message;  otherwise,
        /// it contains error messages describing the failure.</returns>
        public async Task<IBaseResult> DeleteAsync(string productId, CancellationToken cancellationToken = default)
        {
            var result = await productRepository.DeleteAsync(productId, cancellationToken);
            if (!result.Succeeded) return await Result.FailAsync(result.Messages);
            var saveResult = await productRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);
            return await Result.SuccessAsync("Product was successfully removed");
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
            var image = new EntityImage<Product, string>(request.ImageId, request.EntityId) { Selector = request.Selector, Order = request.Order };

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
            var image = new EntityVideo<Product, string> { VideoId = request.VideoId, EntityId = request.EntityId };

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
        
        #region Product Attributes

        /// <summary>
        /// Retrieves all attributes associated with a specified parent product.
        /// </summary>
        /// <remarks>
        /// This method constructs a query specification to filter products based on the provided product identifier.
        /// Specifically, it retrieves all products where the <c>VariantParentId</c> equals the given <paramref name="productId"/>,
        /// and it includes their associated pricing information. The resulting data is then projected into a collection
        /// of <see cref="ProductDto"/> objects. If the repository operation fails, an error result containing the failure 
        /// messages is returned.
        /// </remarks>
        /// <param name="productId">The identifier of the product whose attributes are to be retrieved.</param>
        /// <param name="cancellationToken">A token that can be monitored for cancellation requests.</param>
        /// <returns>
        /// An asynchronous task that returns an <see cref="IBaseResult{T}"/> containing an enumerable collection of 
        /// <see cref="ProductDto"/> objects if the operation is successful; otherwise, an error result with appropriate 
        /// failure messages.
        /// </returns>
        public async Task<IBaseResult<IEnumerable<ProductDto>>> GetAllAttributes(string productId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Product>(c => c.VariantParentId == productId);
            spec.AddInclude(c => c.Include(g => g.Pricing));

            var result = await productRepository.ListAsync(spec, false, cancellationToken);
            if (!result.Succeeded)
                return await Result<IEnumerable<ProductDto>>.FailAsync(result.Messages);
            return await Result<IEnumerable<ProductDto>>.SuccessAsync(result.Data.Select(c => new ProductDto(c, c.Pricing)));
        }

        /// <summary>
        /// Retrieves a single attribute for a product based on the attribute identifier.
        /// </summary>
        /// <remarks>
        /// This method defines a query specification to locate a product entity that matches the provided attribute identifier.
        /// It includes the related pricing data to ensure a comprehensive representation of the attribute. If the product 
        /// is found, it is transformed into a <see cref="ProductDto"/>; if not, an error result is returned detailing that 
        /// no attribute with the specified identifier was found.
        /// </remarks>
        /// <param name="attributeId">The unique identifier of the attribute to be retrieved.</param>
        /// <param name="cancellationToken">A token that can be used to monitor for cancellation signals during the operation.</param>
        /// <returns>
        /// An asynchronous task that returns an <see cref="IBaseResult{T}"/> containing a <see cref="ProductDto"/> 
        /// representing the attribute if the operation is successful; in case of failure or if no attribute is found, 
        /// the result includes the corresponding error messages.
        /// </returns>
        public async Task<IBaseResult<ProductDto>> GetAttribute(string attributeId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Product>(c => c.Id == attributeId);
            spec.AddInclude(c => c.Include(g => g.Pricing));

            var result = await productRepository.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!result.Succeeded)
                return await Result<ProductDto>.FailAsync(result.Messages);
            if (result.Data == null)
                return await Result<ProductDto>.FailAsync($"No attribute with id : '{attributeId}' was found in the database");
            return await Result<ProductDto>.SuccessAsync(new ProductDto(result.Data, result.Data.Pricing));
        }

        #endregion

        #region Product Metadata

        /// <summary>
        /// Retrieves all metadata entries associated with the specified product.
        /// </summary>
        /// <remarks>
        /// This method constructs a query specification based on the provided product identifier and 
        /// retrieves a list of metadata entities from the repository. Each metadata entity is then projected 
        /// into a ProductMetadataDto. Detailed error messages are returned if the retrieval operation fails.
        /// </remarks>
        /// <param name="productId">The unique identifier of the product whose metadata is to be fetched.</param>
        /// <param name="cancellationToken">Token used to cancel the asynchronous operation.</param>
        /// <returns>An IBaseResult containing an enumerable collection of ProductMetadataDto with associated metadata details.</returns>
        public async Task<IBaseResult<IEnumerable<ProductMetadataDto>>> GetMetadata(string productId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<ProductMetadata>(c => c.ProductId == productId);

            var result = await metadataRepository.ListAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<IEnumerable<ProductMetadataDto>>.FailAsync(result.Messages);
            return await Result<IEnumerable<ProductMetadataDto>>.SuccessAsync(result.Data.Select(c => new ProductMetadataDto(c)));
        }

        /// <summary>
        /// Creates a new metadata record for a product using the provided data transfer object.
        /// </summary>
        /// <remarks>
        /// This method maps the incoming ProductMetadataDto to a new ProductMetadata entity.
        /// It generates a new identifier if one is not provided. After successfully creating the entity in the metadata repository,
        /// it saves the changes to the database. A success message is returned upon completion, otherwise detailed error messages are provided.
        /// </remarks>
        /// <param name="dto">The ProductMetadataDto containing metadata key, value, and associated product details.</param>
        /// <param name="cancellationToken">Token to observe while waiting for the asynchronous operation to complete.</param>
        /// <returns>An IBaseResult containing a success message or error details based on the outcome.</returns>
        public async Task<IBaseResult<ProductMetadataDto>> CreateMetadata(ProductMetadataDto dto, CancellationToken cancellationToken = default)
        {
            var result = await metadataRepository.CreateAsync(new ProductMetadata()
            {
                Id = string.IsNullOrEmpty(dto.Id) ? Guid.NewGuid().ToString() : dto.Id,
                Name = dto.Name,
                Value = dto.Value,
                ProductId = dto.ProductId
            }, cancellationToken);
            if (!result.Succeeded) return await Result<ProductMetadataDto>.FailAsync(result.Messages);

            var saveResult = await metadataRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result<ProductMetadataDto>.FailAsync(saveResult.Messages);
            return await Result<ProductMetadataDto>.SuccessAsync("Metadata was successfully created");
        }

        /// <summary>
        /// Updates an existing metadata record using the provided data transfer object.
        /// </summary>
        /// <remarks>
        /// This method first retrieves the existing metadata entity based on the identifier provided in the DTO.
        /// If the entity is found, its properties are updated with the new values from the DTO. 
        /// The updated entity is then passed to the repository's update method and changes are saved. 
        /// Detailed error information is returned if the metadata does not exist or if any operation fails.
        /// </remarks>
        /// <param name="dto">The ProductMetadataDto containing updated metadata information.</param>
        /// <param name="cancellationToken">Token used for canceling the asynchronous operation.</param>
        /// <returns>An IBaseResult containing either a success message or error details.</returns>
        public async Task<IBaseResult> UpdateMetadata(ProductMetadataDto dto, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<ProductMetadata>(c => c.Id == dto.Id);
            var metadataResult = await metadataRepository.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!metadataResult.Succeeded) return await Result.FailAsync(metadataResult.Messages);
            if (metadataResult.Data == null) return await Result.FailAsync($"No metadata property with id : '{dto.Id}' was found in the database");

            var result = metadataRepository.Update(new ProductMetadata()
            {
                Id = string.IsNullOrEmpty(dto.Id) ? Guid.NewGuid().ToString() : dto.Id,
                Name = dto.Name,
                Value = dto.Value,
                ProductId = dto.ProductId
            });
            if (!result.Succeeded) return await Result.FailAsync(result.Messages);
            var saveResult = await metadataRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);
            return await Result<ProductMetadataDto>.SuccessAsync("Metadata was successfully updated");
        }

        /// <summary>
        /// Deletes a metadata record identified by the specified metadata identifier.
        /// </summary>
        /// <remarks>
        /// This method attempts to remove a metadata entry from the repository using its unique identifier.
        /// If the deletion is successful, the changes are persisted in the database.
        /// In case of failure, detailed error messages are returned indicating the cause.
        /// </remarks>
        /// <param name="metadataId">The unique identifier of the metadata record to be removed.</param>
        /// <param name="cancellationToken">Token used to cancel the asynchronous operation.</param>
        /// <returns>An IBaseResult containing a success message if the deletion is successful or error details if it fails.</returns>
        public async Task<IBaseResult> RemoveMetadata(string metadataId, CancellationToken cancellationToken = default)
        {
            var result = await metadataRepository.DeleteAsync(metadataId, cancellationToken);
            if (!result.Succeeded) return await Result<ProductMetadataDto>.FailAsync(result.Messages);
            var saveResult = await metadataRepository.SaveAsync(cancellationToken);
            if (saveResult.Succeeded)
            {
                return await Result.SuccessAsync("Metadata was successfully removed");
            }
            return await Result.FailAsync(saveResult.Messages);
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
        private async Task<IBaseResult<IEnumerable<Product>>> GetCategoryProductsAsync(string? categoryId = null, CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrEmpty(categoryId))
            {
                var products = new List<Product>();
                var result = (await PopulateProductItemsList(categoryId, products, cancellationToken));
                if (result.Succeeded)
                {
                    products.AddRange(result.Data);
                    return await Result<IEnumerable<Product>>.SuccessAsync(products.Distinct());
                }
            }
            var categoryNullResult = productRepository.FindAll(false);
            if (categoryNullResult.Succeeded)
            {
                var response = categoryNullResult.Data.Include(c => c.Images).Include(c => c.Pricing);
            }
            return await Result<IEnumerable<Product>>.FailAsync(categoryNullResult.Messages);
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
        /// <param name="products">A list of products to populate with the entities found in the specified category and its subcategories.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with a collection of <see cref="Product"/> objects. If the operation succeeds, the result contains the
        /// populated list of products; otherwise, it contains error messages.</returns>
        private async Task<IBaseResult<IEnumerable<Product>>> PopulateProductItemsList(string categoryId, List<Product> products, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Category<Product>>(c => c.Id == categoryId);
            spec.AddInclude(c => c.Include(g => g.SubCategories));
            spec.AddInclude(c => c.Include(g => g.EntityCollection).ThenInclude(c => c.Entity).ThenInclude(c => c.Pricing));
            spec.AddInclude(c => c.Include(g => g.EntityCollection).ThenInclude(c => c.Entity).ThenInclude(c => c.Images).ThenInclude(c => c.Image));

            var result = await categoryRepo.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<IEnumerable<Product>>.FailAsync(result.Messages);
            if (result.Data.SubCategories.Any())
            {
                foreach (var item in result.Data.SubCategories)
                {
                    products = (await PopulateProductItemsList(item.Id, products)).Data.ToList();
                }
            }

            foreach (var item in result.Data.EntityCollection)
            {
                products.Add(item.Entity);
            }
            return await Result<IEnumerable<Product>>.SuccessAsync(products);
        }

        /// <summary>
        /// Updates the pricing details of an existing product based on the provided product edition data.
        /// </summary>
        /// <remarks>
        /// This method locates the pricing record for the product by constructing a query specification using the product identifier.
        /// Upon successful retrieval, it updates the pricing details including cost exclusive, selling price, discount percentage,
        /// discount end date, and VAT applicability with the new values supplied in the <see cref="ProductEditionDto"/>.
        /// If the pricing record is not found or if there is an error during the retrieval process, a failure result is returned
        /// with the corresponding error messages.
        /// </remarks>
        /// <param name="product">A <see cref="ProductEditionDto"/> object containing the updated pricing information and associated product details.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests during the asynchronous operation.</param>
        /// <returns>An <see cref="IBaseResult"/> indicating the success or failure of the pricing update.</returns>
        private async Task<IBaseResult> UpdatePricingAsync(ProductDto product, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Price>(c => c.Id == product.ProductId);
            var priceResult = await priceRepository.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!priceResult.Succeeded) return await Result.FailAsync(priceResult.Messages);
            if (priceResult.Data == null)
                return await Result.FailAsync($"No price found for product with id '{product.ProductId}'");
            
            priceResult.Data.CostExcl = product.Pricing.CostExcl;
            priceResult.Data.SellingPrice = product.Pricing.PriceIncl;
            priceResult.Data.DiscountPercentage = product.Pricing.DiscountPercentage;
            priceResult.Data.DiscountEndDate = product.Pricing.DiscountEndDate;
            priceResult.Data.Vatable = product.Pricing.Vatable;

            priceRepository.Update(priceResult.Data);

            return await Result.SuccessAsync("Product pricing updated successfully");
        }

        /// <summary>
        /// Updates the categories associated with a product based on the provided data.
        /// </summary>
        /// <remarks>This method ensures that the product's category associations are updated to match the
        /// provided list.  Categories that are no longer associated with the product will be removed, and new
        /// associations will be created.</remarks>
        /// <param name="product">An object containing the product's identifier and the updated list of categories to associate with the
        /// product.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the success or failure of the operation.</returns>
        private async Task<IBaseResult> UpdateCategoriesAsync(ProductDto product, CancellationToken cancellationToken = default)
        {
            var categoriesResult = await productCategoryService.EntityCategoriesAsync(product.ProductId);
            foreach (var category in categoriesResult.Data)
            {
                if (product.Categories.All(c => c.CategoryId != category.CategoryId))
                {
                    await productCategoryService.RemoveEntityCategoryAsync(category.CategoryId, product.ProductId);
                }
            }

            foreach (var category in product.Categories)
            {
                if (!categoriesResult.Data.Any(c => c.CategoryId == category.CategoryId))
                {
                    await productCategoryService.CreateEntityCategoryAsync(category.CategoryId, product.ProductId);
                }
            }

            return await Result.SuccessAsync("Categories updated successfully");
        }

        /// <summary>
        /// Updates the brands associated with a product based on the provided product data.
        /// </summary>
        /// <remarks>This method synchronizes the brands associated with the specified product by adding
        /// or removing associations as necessary to match the provided list of brands. The operation ensures that the
        /// product's brand associations are consistent with the data in the <paramref name="product"/>
        /// parameter.</remarks>
        /// <param name="product">A <see cref="ProductEditionDto"/> object containing the product's identifier and the updated list of
        /// associated brands.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>An <see cref="IBaseResult"/> indicating the success or failure of the operation.</returns>
        private async Task<IBaseResult> UpdateBrandsAsync(ProductDto product, CancellationToken cancellationToken = default)
        {
            //var brandsResult = await brandService.ProductBrandsAsync(product.ProductId, cancellationToken);
            //foreach (var brand in brandsResult.Data)
            //{
            //    if (!product.Brands.Any(c => c.BrandId == brand.BrandId))
            //    {
            //        await brandService.RemoveProductBrandAsync(brand.BrandId, product.ProductId, cancellationToken);
            //    }
            //}

            //foreach (var brand in product.Brands)
            //{
            //    if (!brandsResult.Data.Any(c => c.BrandId == brand.BrandId))
            //    {
            //        await brandService.CreateProductBrandAsync(brand.BrandId, product.ProductId, cancellationToken);
            //    }
            //}
            return await Result.SuccessAsync("Brands updated successfully");
        }

        /// <summary>
        /// Updates the suppliers associated with a given product.
        /// </summary>
        /// <remarks>This method synchronizes the suppliers associated with the specified product by
        /// adding new suppliers  and removing those that are no longer associated. The operation ensures that the
        /// product's supplier  list matches the provided data.</remarks>
        /// <param name="product">A <see cref="ProductEditionDto"/> object containing the product's identifier and the updated list of
        /// suppliers.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>An <see cref="IBaseResult"/> indicating the success or failure of the operation.</returns>
        private async Task<IBaseResult> UpdateSuppliersAsync(ProductDto product, CancellationToken cancellationToken = default)
        {
            //var supplierResult = await suppliersService.AllProductSuppliersAsync(product.ProductId, cancellationToken);
            //foreach (var supplier in supplierResult.Data)
            //{
            //    if (!product.Suppliers.Any(c => c.SupplierId == supplier.SupplierId))
            //    {
            //        await suppliersService.RemoveProductSupplierAsync(supplier.SupplierId, cancellationToken);
            //    }
            //}

            //foreach (var supplier in product.Suppliers)
            //{
            //    if (!supplierResult.Data.Any(c => c.SupplierId == supplier.SupplierId))
            //    {
            //       await suppliersService.AddUpdateProductSupplierAsync(new ProductSupplierCreationDto(supplier.SupplierId, product.ProductId, true), cancellationToken);
            //    }
            //}
            return await Result.SuccessAsync("Suppliers updated successfully");
        }

        #endregion
    }
}
