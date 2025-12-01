using ConectOne.Domain.Extensions;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.RequestFeatures;
using ProductsModule.Domain.DataTransferObjects;
using ProductsModule.Domain.Interfaces;
using ProductsModule.Domain.RequestFeatures;

namespace ProductsModule.Application.RestServices
{
    /// <summary>
    /// Provides a set of methods for managing products, including retrieving, creating, updating, and deleting
    /// products, as well as managing associated metadata, images, and videos.
    /// </summary>
    /// <remarks>This service acts as a REST-based interface for interacting with product-related data. It
    /// supports operations such as: <list type="bullet"> <item><description>Fetching paginated product lists with
    /// pricing information.</description></item> <item><description>Retrieving popular products, products by category,
    /// and detailed product information.</description></item> <item><description>Managing product metadata, including
    /// creation, updates, and deletion.</description></item> <item><description>Adding and removing images and videos
    /// associated with products.</description></item> </list> The service relies on an <see cref="IBaseHttpProvider"/>
    /// to perform HTTP operations and returns results wrapped in <see cref="IBaseResult"/> or <see
    /// cref="PaginatedResult{T}"/> objects. Cancellation tokens are supported for all asynchronous operations to allow
    /// for graceful cancellation.</remarks>
    /// <param name="provider"></param>
    public class ProductRestService(IBaseHttpProvider provider) : IProductService
    {
        /// <summary>
        /// Retrieves a paginated list of products with pricing information based on the specified parameters.
        /// </summary>
        /// <remarks>This method queries the product data source and returns a paginated result based on
        /// the provided <paramref name="productParameters"/>. The <paramref name="trackChanges"/> parameter determines
        /// whether the retrieved entities are tracked for changes, which may impact performance.</remarks>
        /// <param name="productParameters">The parameters used to filter, sort, and paginate the product list.</param>
        /// <param name="trackChanges">A value indicating whether to track changes to the retrieved entities.  Defaults to <see langword="false"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a paginated list of <see cref="ProductDto"/> objects.</returns>
        public async Task<PaginatedResult<ProductDto>> PagedPricedProductsAsync(ProductsParameters productParameters, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetPagedAsync<ProductDto, ProductsParameters>("products", productParameters);
            return result;
        }

        /// <summary>
        /// Retrieves a collection of the most popular products.
        /// </summary>
        /// <remarks>This method fetches the most popular products based on the specified selection count.
        /// The result includes product details in the form of <see cref="ProductDto"/> objects.</remarks>
        /// <param name="selectionCount">The number of popular products to retrieve. The default value is 12.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// of <see cref="IEnumerable{T}"/> containing the popular products.</returns>
        public async Task<IBaseResult<IEnumerable<ProductDto>>> PopularProductsAsync(int selectionCount = 12, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<ProductDto>>($"products/popular/{selectionCount}");
            return result;
        }

        /// <summary>
        /// Retrieves a collection of products that have associated pricing information.
        /// </summary>
        /// <remarks>The method fetches all products with pricing details based on the specified
        /// pagination and filtering parameters. If no products match the criteria, the result will contain an empty
        /// collection.</remarks>
        /// <param name="pageParameters">An object containing pagination and filtering parameters for the request.</param>
        /// <param name="trackChanges">A boolean value indicating whether to track changes to the retrieved entities. Defaults to <see
        /// langword="false"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// of <see cref="IEnumerable{T}"/> containing <see cref="ProductDto"/> objects representing the priced
        /// products.</returns>
        public async Task<IBaseResult<IEnumerable<ProductDto>>> AllPricedProductsAsync(ProductsParameters pageParameters, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<ProductDto>>($"products/all?{pageParameters.GetQueryString()}");
            return result;
        }

        /// <summary>
        /// Retrieves a collection of products within the specified category.
        /// </summary>
        /// <remarks>This method fetches products associated with the given category identifier. Ensure
        /// that the <paramref name="categoryId"/> corresponds to a valid category in the system. The operation may be
        /// canceled by passing a cancellation token.</remarks>
        /// <param name="categoryId">The unique identifier of the category for which to retrieve products.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// containing an enumerable collection of <see cref="ProductDto"/> objects representing the products in the
        /// specified category.</returns>
        public async Task<IBaseResult<IEnumerable<ProductDto>>> CategoryPricedProductsAsync(string categoryId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<ProductDto>>($"products/category/{categoryId}");
            return result;
        }

        /// <summary>
        /// Retrieves detailed information about a product, including its pricing, based on the specified product ID.
        /// </summary>
        /// <remarks>This method fetches product details from the underlying provider. The caller can
        /// optionally specify whether to track changes to the retrieved data and provide a cancellation token to cancel
        /// the operation if needed.</remarks>
        /// <param name="productId">The unique identifier of the product to retrieve. This value cannot be null or empty.</param>
        /// <param name="trackChanges">A boolean value indicating whether to track changes to the retrieved product data. The default value is <see
        /// langword="false"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping the <see cref="ProductFullInfoDto"/> with detailed product information.</returns>
        public async Task<IBaseResult<ProductFullInfoDto>> PricedProductAsync(string productId, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<ProductFullInfoDto>($"products/{productId}");
            return result;
        }

        /// <summary>
        /// Updates an existing product asynchronously.
        /// </summary>
        /// <remarks>This method sends an HTTP PUT request to update the product on the server. Ensure
        /// that the provided <paramref name="product"/> contains valid and complete data for the update
        /// operation.</remarks>
        /// <param name="product">The product data to update. The <see cref="ProductDto"/> must contain the updated information for the
        /// product.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the updated product data if the operation is successful.</returns>
        public async Task<IBaseResult<ProductDto>> UpdateAsync(ProductDto product, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<ProductDto, ProductDto>($"products", product);
            return result;
        }

        /// <summary>
        /// Creates a new product asynchronously.
        /// </summary>
        /// <param name="product">The product data to be created. Must not be <see langword="null"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// containing the created product data.</returns>
        public async Task<IBaseResult<ProductDto>> CreateAsync(ProductDto product, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<ProductDto, ProductDto>($"products", product);
            return result;
        }

        /// <summary>
        /// Deletes a product asynchronously based on the specified product ID.
        /// </summary>
        /// <remarks>This method sends a delete request to the underlying provider to remove the specified
        /// product. Ensure that the product ID is valid and exists before calling this method.</remarks>
        /// <param name="productId">The unique identifier of the product to delete. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the delete operation.</returns>
        public async Task<IBaseResult> DeleteAsync(string productId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"products", productId);
            return result;
        }

        /// <summary>
        /// Adds an image to an entity based on the provided request.
        /// </summary>
        /// <remarks>This method sends a request to add an image to an entity. Ensure that the <paramref
        /// name="request"/> contains valid data, such as the entity identifier and the image payload. The operation is
        /// asynchronous and can be canceled using the <paramref name="cancellationToken"/>.</remarks>
        /// <param name="request">The request containing the details of the image to be added, including the entity identifier and image data.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"products/addImage", request);
            return result;
        }

        /// <summary>
        /// Removes an image associated with the specified image identifier.
        /// </summary>
        /// <remarks>This method sends a request to delete the image identified by <paramref
        /// name="imageId"/>.  Ensure that the provided identifier corresponds to an existing image.</remarks>
        /// <param name="imageId">The unique identifier of the image to be removed. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveImage(string imageId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"products/deleteImage", imageId);
            return result;
        }

        /// <summary>
        /// Adds a video to an entity based on the provided request.
        /// </summary>
        /// <remarks>This method sends a request to add a video to an entity. Ensure that the <paramref
        /// name="request"/>  contains all required fields before calling this method.</remarks>
        /// <param name="request">The request containing the details of the video to be added.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> AddVideo(AddEntityVideoRequest request, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"products/addVideo", request);
            return result;
        }

        /// <summary>
        /// Removes a video associated with the specified video ID.
        /// </summary>
        /// <remarks>This method sends a request to delete the video identified by <paramref
        /// name="videoId"/>.  Ensure that the provided video ID is valid and corresponds to an existing
        /// video.</remarks>
        /// <param name="videoId">The unique identifier of the video to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveVideo(string videoId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"products/deleteVideo", videoId);
            return result;
        }

        /// <summary>
        /// Retrieves all attributes associated with the specified product.
        /// </summary>
        /// <remarks>This method sends an asynchronous request to retrieve all attributes for the
        /// specified product. Ensure that the <paramref name="productId"/> corresponds to a valid product in the
        /// system.</remarks>
        /// <param name="productId">The unique identifier of the product whose attributes are to be retrieved. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// containing an enumerable collection of <see cref="ProductDto"/> objects representing the product's
        /// attributes.</returns>
        public async Task<IBaseResult<IEnumerable<ProductDto>>> GetAllAttributes(string productId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<ProductDto>>($"products/attributes/all/{productId}");
            return result;
        }

        /// <summary>
        /// Retrieves the details of a product attribute by its unique identifier.
        /// </summary>
        /// <remarks>This method sends an HTTP GET request to retrieve the product attribute details from
        /// the provider. Ensure that the <paramref name="attributeId"/> corresponds to a valid product
        /// attribute.</remarks>
        /// <param name="attributeId">The unique identifier of the product attribute to retrieve. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that wraps the <see cref="ProductDto"/> representing the product attribute details.</returns>
        public async Task<IBaseResult<ProductDto>> GetAttribute(string attributeId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<ProductDto>($"products/attributes/{attributeId}");
            return result;
        }

        /// <summary>
        /// Retrieves metadata for a specified product.
        /// </summary>
        /// <remarks>This method asynchronously retrieves metadata for a product using the provided
        /// product identifier.  The metadata is returned as a collection of <see cref="ProductMetadataDto"/>
        /// objects.</remarks>
        /// <param name="productId">The unique identifier of the product for which metadata is being retrieved.  This value cannot be <see
        /// langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult{T}"/>
        /// of <see cref="IEnumerable{T}"/> containing the metadata for the specified product. If no metadata is found,
        /// the result may contain an empty collection.</returns>
        public async Task<IBaseResult<IEnumerable<ProductMetadataDto>>> GetMetadata(string productId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<ProductMetadataDto>>($"products/metadata/{productId}");
            return result;
        }

        /// <summary>
        /// Creates or updates product metadata by sending the specified data to the server.
        /// </summary>
        /// <remarks>This method sends a PUT request to the server to create or update product metadata. 
        /// Ensure that the dto parameter contains valid data before calling this method.</remarks>
        /// <param name="dto">The product metadata to create or update.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  IBaseResult{T} object that
        /// includes the created or updated product metadata.</returns>
        public async Task<IBaseResult<ProductMetadataDto>> CreateMetadata(ProductMetadataDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<ProductMetadataDto, ProductMetadataDto>($"products/metadata", dto);
            return result;
        }

        /// <summary>
        /// Updates the metadata for a product.
        /// </summary>
        /// <remarks>This method sends the provided product metadata to the server for updating. Ensure
        /// that the  <paramref name="dto"/> contains valid data before calling this method.</remarks>
        /// <param name="dto">The product metadata to be updated, represented as a <see cref="ProductMetadataDto"/> object.</param>
        /// <param name="cancellationToken">An optional <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateMetadata(ProductMetadataDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<ProductMetadataDto, ProductMetadataDto>($"products/metadata", dto);
            return result;
        }

        /// <summary>
        /// Removes metadata associated with the specified metadata identifier.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to delete metadata identified by
        /// <paramref name="metadataId"/>.  Ensure that the provided identifier corresponds to an existing metadata
        /// entry.</remarks>
        /// <param name="metadataId">The unique identifier of the metadata to be removed. This value cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveMetadata(string metadataId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"products/metadata", metadataId);
            return result;
        }
    }
}
