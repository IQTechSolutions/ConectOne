using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ShoppingModule.Domain.DataTransferObjects;
using ShoppingModule.Domain.Interfaces;

namespace ShoppingModule.Application.RestServices
{
    /// <summary>
    /// Provides methods for managing shopping cart operations via RESTful HTTP requests.
    /// </summary>
    /// <remarks>This service allows clients to perform various shopping cart operations, such as retrieving a
    /// shopping cart,  adding or removing items, migrating carts, and processing or emptying a cart. It communicates
    /// with a backend  REST API using the provided <see cref="IBaseHttpProvider"/>.</remarks>
    /// <param name="provider"></param>
    public class ShoppingCartRestService(IBaseHttpProvider provider) : IShoppingCartService
    {
        /// <summary>
        /// Retrieves the shopping cart details for the specified shopping cart ID.
        /// </summary>
        /// <param name="shoppingCartId">The unique identifier of the shopping cart to retrieve.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// object wrapping the <see cref="ShoppingCartDto"/> for the specified shopping cart ID.</returns>
        public async Task<IBaseResult<ShoppingCartDto>> GetShoppingCartAsync(string shoppingCartId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<ShoppingCartDto>($"shopping/cart/{shoppingCartId}");
            return result;
        }

        /// <summary>
        /// Adds an item to the shopping cart asynchronously.
        /// </summary>
        /// <remarks>This method sends a request to the underlying provider to add the specified item to
        /// the shopping cart. Ensure that the <paramref name="addUpdateModel"/> contains valid data before calling this
        /// method.</remarks>
        /// <param name="addUpdateModel">The data transfer object containing the details of the item to add to the shopping cart.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that includes the added shopping cart item as a <see cref="ShoppingCartItemDto"/>.</returns>
        public async Task<IBaseResult<ShoppingCartItemDto>> AddShoppingCartItemAsync(ShoppingCartItemAddUpdateDto addUpdateModel, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<ShoppingCartItemDto, ShoppingCartItemAddUpdateDto>($"shopping/cart/add", addUpdateModel);
            return result;
        }

        /// <summary>
        /// Adds or updates an item in the shopping cart asynchronously.
        /// </summary>
        /// <remarks>This method sends a request to add or update the specified item in the shopping cart.
        /// If the item already exists, its details will be updated; otherwise, it will be added as a new
        /// item.</remarks>
        /// <param name="addUpdateModel">The shopping cart item to add or update. Must contain valid item details.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the updated <see cref="ShoppingCartItemDto"/> if the operation is successful.</returns>
        public async Task<IBaseResult<ShoppingCartItemDto>> AddShoppingCartItemAsync(ShoppingCartItemDto addUpdateModel, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<ShoppingCartItemDto, ShoppingCartItemDto>($"shopping/cart/add", addUpdateModel);
            return result;
        }

        /// <summary>
        /// Removes an item from the shopping cart.
        /// </summary>
        /// <remarks>Use this method to remove a specific product from a shopping cart. If the <paramref
        /// name="all"/> parameter is set to  <see langword="true"/>, all quantities of the product will be removed;
        /// otherwise, only one unit will be removed.</remarks>
        /// <param name="shoppingCartId">The unique identifier of the shopping cart from which the item will be removed.</param>
        /// <param name="productId">The unique identifier of the product to remove from the shopping cart.</param>
        /// <param name="all">A value indicating whether all quantities of the specified product should be removed.  <see
        /// langword="true"/> to remove all quantities of the product; otherwise, <see langword="false"/> to remove only
        /// one unit.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation will be canceled if the token is triggered.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the removal operation.</returns>
        public async Task<IBaseResult> RemoveShoppingCartItemAsync(string shoppingCartId, string productId, bool all = false, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"shopping/cart/remove/{shoppingCartId}", productId);
            return result;
        }

        /// <summary>
        /// Removes all shopping cart items associated with the specified user.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to remove all items from the shopping
        /// cart for the specified user. The operation is idempotent; if the user has no items in their cart, the method
        /// completes successfully without making changes.</remarks>
        /// <param name="userId">The unique identifier of the user whose shopping cart items are to be removed. Cannot be <see
        /// langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveShoppingCartItemsForUserAsync(string userId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"shopping/cart/removeforuser", userId);
            return result;
        }

        /// <summary>
        /// Migrates the contents of an existing shopping cart to a new shopping cart.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to migrate the contents of one
        /// shopping cart to another.  Ensure that both <paramref name="shoppingCartId"/> and <paramref
        /// name="newCartId"/> are valid and unique  identifiers for shopping carts. The operation may fail if the
        /// specified shopping cart does not exist or  if the migration cannot be completed due to other
        /// constraints.</remarks>
        /// <param name="shoppingCartId">The identifier of the shopping cart to be migrated.</param>
        /// <param name="newCartId">The identifier of the new shopping cart to which the contents will be migrated.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the migration operation.</returns>
        public async Task<IBaseResult> MigrateAsync(string shoppingCartId, string newCartId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"shopping/cart/migrate/{shoppingCartId}", newCartId);
            return result;
        }

        /// <summary>
        /// Processes the specified shopping cart asynchronously.
        /// </summary>
        /// <remarks>This method sends a request to process the specified shopping cart. Ensure that the
        /// <paramref name="shoppingCartId"/> corresponds to a valid shopping cart. The operation may be canceled by
        /// passing a cancellation token.</remarks>
        /// <param name="shoppingCartId">The unique identifier of the shopping cart to process. This value cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// representing the outcome of the shopping cart processing.</returns>
        public async Task<IBaseResult> ProcessAsync(string shoppingCartId, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"shopping/cart/process/{shoppingCartId}");
            return result;
        }

        /// <summary>
        /// Empties the specified shopping cart asynchronously.
        /// </summary>
        /// <remarks>This method sends a request to empty the shopping cart identified by <paramref
        /// name="shoppingCartId"/>.  Ensure the provided shopping cart ID is valid and corresponds to an existing
        /// cart.</remarks>
        /// <param name="shoppingCartId">The unique identifier of the shopping cart to be emptied. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> EmptyAsync(string shoppingCartId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"shopping/cart/empty", shoppingCartId);
            return result;
        }
    }
}
