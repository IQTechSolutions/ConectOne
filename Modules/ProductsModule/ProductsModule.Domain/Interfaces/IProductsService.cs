using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.RequestFeatures;
using ProductsModule.Domain.DataTransferObjects;
using ProductsModule.Domain.RequestFeatures;

namespace ProductsModule.Domain.Interfaces
{
    /// <summary>
    /// Defines a contract for managing and retrieving product-related data, including pricing, inventory, categories,
    /// brands, and suppliers.
    /// </summary>
    /// <remarks>This interface provides methods for performing CRUD operations on products, retrieving
    /// product details,  managing product settings, and handling product-related data such as pricing, inventory,
    /// categories, and brands.  It supports both paginated and non-paginated queries, as well as operations that track
    /// changes for concurrency control.</remarks>
    public interface IProductService
    {
        /// <summary>
        /// Retrieves a paginated list of products with pricing information based on the specified parameters.
        /// </summary>
        /// <remarks>The method supports filtering, sorting, and pagination based on the provided
        /// <paramref name="productParameters"/>. If <paramref name="trackChanges"/> is set to <see langword="true"/>,
        /// the retrieved entities will be tracked  by the underlying data context, which may impact performance in
        /// high-load scenarios.</remarks>
        /// <param name="productParameters">The parameters used to filter, sort, and paginate the product list.</param>
        /// <param name="trackChanges">A value indicating whether to track changes to the retrieved entities.  Set to <see langword="true"/> to
        /// enable change tracking; otherwise, <see langword="false"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a collection of <see cref="ProductDto"/> objects  that match
        /// the specified parameters, along with pagination metadata.</returns>
        Task<PaginatedResult<ProductDto>> PagedPricedProductsAsync(ProductsParameters productParameters, bool trackChanges = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a collection of the most popular products.
        /// </summary>
        /// <remarks>The method returns up to <paramref name="selectionCount"/> products based on their
        /// popularity. If fewer products are available, the result will contain all available products.</remarks>
        /// <param name="selectionCount">The maximum number of popular products to retrieve. The default value is 12.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with an enumerable collection of <see cref="ProductDto"/> representing the most popular products.</returns>
        Task<IBaseResult<IEnumerable<ProductDto>>> PopularProductsAsync(int selectionCount = 12, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a paginated list of products that have pricing information.
        /// </summary>
        /// <remarks>The method supports pagination through the <paramref name="pageParameters"/>
        /// parameter.  If <paramref name="trackChanges"/> is set to <see langword="true"/>, the retrieved entities 
        /// will be tracked for changes, which may impact performance in high-load scenarios.</remarks>
        /// <param name="pageParameters">The parameters for pagination, including page size and page number.</param>
        /// <param name="trackChanges">A value indicating whether to track changes to the retrieved entities.  Set to <see langword="true"/> to
        /// enable change tracking; otherwise, <see langword="false"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> containing
        /// an enumerable collection of <see cref="ProductDto"/> objects with pricing information.</returns>
        Task<IBaseResult<IEnumerable<ProductDto>>> AllPricedProductsAsync(ProductsParameters pageParameters, bool trackChanges = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a collection of products within the specified category, including their pricing details.
        /// </summary>
        /// <remarks>The method fetches products along with their pricing details for the specified
        /// category. Ensure the <paramref name="categoryId"/> corresponds to a valid category in the system.</remarks>
        /// <param name="categoryId">The unique identifier of the category for which products are to be retrieved. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// containing an enumerable collection of <see cref="ProductDto"/> objects representing the products in the
        /// specified category.</returns>
        Task<IBaseResult<IEnumerable<ProductDto>>> CategoryPricedProductsAsync(string categoryId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves detailed information about a product, including its pricing, based on the specified product ID.
        /// </summary>
        /// <param name="productId">The unique identifier of the product to retrieve. Cannot be null or empty.</param>
        /// <param name="trackChanges">A value indicating whether to track changes to the retrieved product entity.  If <see langword="true"/>, the
        /// entity will be tracked; otherwise, it will not be tracked.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// object wrapping the <see cref="ProductFullInfoDto"/> with the product's detailed information.</returns>
        Task<IBaseResult<ProductFullInfoDto>> PricedProductAsync(string productId, bool trackChanges = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing product with the specified details.
        /// </summary>
        /// <param name="product">The product details to update. Must include a valid identifier.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that indicates the outcome of the operation and includes the updated product details if successful.</returns>
        Task<IBaseResult<ProductDto>> UpdateAsync(ProductDto product, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new product asynchronously.
        /// </summary>
        /// <remarks>The operation may fail if the provided product data is invalid or if a product with
        /// the same identifier already exists. Ensure that the product data meets all required validation criteria
        /// before calling this method.</remarks>
        /// <param name="product">The product data to create. Must not be <see langword="null"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that includes the created product data and operation status.</returns>
        Task<IBaseResult<ProductDto>> CreateAsync(ProductDto product, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the product with the specified identifier asynchronously.
        /// </summary>
        /// <param name="productId">The unique identifier of the product to delete. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the delete operation.</returns>
        Task<IBaseResult> DeleteAsync(string productId, CancellationToken cancellationToken = default);

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
        Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default);

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
        Task<IBaseResult> RemoveImage(string imageId, CancellationToken cancellationToken = default);

        #endregion

        #region Videos

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
        Task<IBaseResult> AddVideo(AddEntityVideoRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a video with the specified identifier.
        /// </summary>
        /// <param name="videoId">The unique identifier of the video to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        Task<IBaseResult> RemoveVideo(string videoId, CancellationToken cancellationToken = default);

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
        Task<IBaseResult<IEnumerable<ProductDto>>> GetAllAttributes(string productId, CancellationToken cancellationToken = default);

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
        Task<IBaseResult<ProductDto>> GetAttribute(string attributeId, CancellationToken cancellationToken = default);

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
        Task<IBaseResult<IEnumerable<ProductMetadataDto>>> GetMetadata(string productId, CancellationToken cancellationToken = default);

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
        Task<IBaseResult<ProductMetadataDto>> CreateMetadata(ProductMetadataDto dto, CancellationToken cancellationToken = default);

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
        Task<IBaseResult> UpdateMetadata(ProductMetadataDto dto, CancellationToken cancellationToken = default);

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
        Task<IBaseResult> RemoveMetadata(string metadataId, CancellationToken cancellationToken = default);

        #endregion
    }
}
