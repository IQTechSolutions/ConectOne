using ConectOne.Domain.ResultWrappers;
using ShoppingModule.Domain.DataTransferObjects;

namespace ShoppingModule.Domain.Interfaces
{
    /// <summary>
    /// Defines the operations for managing shopping carts, including retrieving, adding, removing,  and migrating
    /// shopping cart items.
    /// </summary>
    /// <remarks>This service provides methods to interact with shopping carts, such as retrieving the current
    /// state of a shopping cart, adding or removing items, and performing operations like emptying  or migrating
    /// shopping carts. Each method returns a result encapsulated in an <see cref="IBaseResult"/>  to indicate the
    /// success or failure of the operation, along with any relevant data or error information.</remarks>
    public interface IShoppingCartService
    {
        /// <summary>
        /// Retrieves the shopping cart associated with the specified identifier.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch the shopping cart details.
        /// Ensure that the provided <paramref name="shoppingCartId"/> is valid and corresponds to an existing
        /// cart.</remarks>
        /// <param name="shoppingCartId">The unique identifier of the shopping cart to retrieve. Must not be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping a <see cref="ShoppingCartDto"/> that represents the shopping cart data. If the shopping cart
        /// is not found, the result may indicate an error or an empty cart, depending on the implementation.</returns>
        Task<IBaseResult<ShoppingCartDto>> GetShoppingCartAsync(string shoppingCartId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a new item to the shopping cart asynchronously.
        /// </summary>
        /// <param name="addUpdateModel">The data transfer object containing the details of the item to add to the shopping cart.  This must include
        /// valid product information and quantity.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <IBaseResult{T}> object
        /// wrapping a <ShoppingCartItemDto>  that represents the added shopping cart item.</returns>
        Task<IBaseResult<ShoppingCartItemDto>> AddShoppingCartItemAsync(ShoppingCartItemAddUpdateDto addUpdateModel, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a new item to the shopping cart asynchronously.
        /// </summary>
        /// <param name="addUpdateModel">The data transfer object containing the details of the item to add to the shopping cart.  This must include
        /// valid product information and quantity.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <IBaseResult{T}> object
        /// wrapping a <ShoppingCartItemDto>  that represents the added shopping cart item.</returns>
        Task<IBaseResult<ShoppingCartItemDto>> AddShoppingCartItemAsync(ShoppingCartItemDto addUpdateModel, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes an item from the shopping cart.
        /// </summary>
        /// <remarks>Use this method to remove a specific product from a shopping cart. If the all
        /// parameter is set to  true, all quantities of the specified product will be removed; otherwise, only one
        /// quantity will be removed.</remarks>
        /// <param name="shoppingCartId">The unique identifier of the shopping cart from which the item will be removed.</param>
        /// <param name="productId">The unique identifier of the product to remove from the shopping cart.</param>
        /// <param name="all">A value indicating whether all instances of the product should be removed.  true to remove all instances of
        /// the product; otherwise, false to remove only one instance.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an IBaseResult  indicating the
        /// success or failure of the operation.</returns>
        Task<IBaseResult> RemoveShoppingCartItemAsync(string shoppingCartId, string productId, bool all = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes all shopping cart items associated with the specified user.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to clear the shopping cart for the
        /// specified user.  Ensure that the <paramref name="userId"/> is valid and corresponds to an existing
        /// user.</remarks>
        /// <param name="userId">The unique identifier of the user whose shopping cart items are to be removed. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the success or failure of the operation.</returns>
        Task<IBaseResult> RemoveShoppingCartItemsForUserAsync(string userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Migrates the contents of an existing shopping cart to a new shopping cart.
        /// </summary>
        /// <param name="shoppingCartId">The unique identifier of the shopping cart to be migrated. Cannot be null or empty.</param>
        /// <param name="newCartId">The unique identifier of the new shopping cart to which the contents will be migrated. Cannot be null or
        /// empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the migration operation.</returns>
        Task<IBaseResult> MigrateAsync(string shoppingCartId, string newCartId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Processes the specified shopping cart by removing all items and saving the changes.
        /// </summary>
        /// <param name="shoppingCartId">The unique identifier of the shopping cart to process. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
        Task<IBaseResult> ProcessAsync(string shoppingCartId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Empties the contents of the specified shopping cart asynchronously.
        /// </summary>
        /// <param name="shoppingCartId">The unique identifier of the shopping cart to be emptied. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the success or failure of the operation.</returns>
        Task<IBaseResult> EmptyAsync(string shoppingCartId, CancellationToken cancellationToken = default);
    }
}
