using Microsoft.AspNetCore.Mvc;
using ShoppingModule.Domain.DataTransferObjects;
using ShoppingModule.Domain.Interfaces;

namespace ShoppingModule.Infrastructure.Controllers;

/// <summary>
/// Provides API endpoints for managing shopping cart operations, including retrieving, adding, removing, and migrating
/// shopping cart items.
/// </summary>
/// <remarks>This controller handles HTTP requests related to shopping cart functionality. It interacts with the
/// <see cref="IShoppingCartService"/> to perform operations such as retrieving cart items, adding items to the cart,
/// removing items, emptying the cart, and migrating cart data. Each action corresponds to a specific HTTP method and
/// route.</remarks>
/// <param name="service"></param>
[Route("api/shopping"), ApiController]
public class ShoppingCartController(IShoppingCartService service) : ControllerBase
{
    /// <summary>
    /// Retrieves the items in the shopping cart with the specified identifier.
    /// </summary>
    /// <remarks>This method returns an HTTP 200 response with the shopping cart items if the operation is
    /// successful. If the specified <paramref name="shoppingCartId"/> does not correspond to an existing cart,  the
    /// response may indicate an error, such as HTTP 404 (Not Found).</remarks>
    /// <param name="shoppingCartId">The unique identifier of the shopping cart to retrieve.</param>
    /// <returns>An <see cref="IActionResult"/> containing the shopping cart items if the cart exists;  otherwise, an appropriate
    /// HTTP response indicating the error.</returns>
	[HttpGet("cart/{shoppingCartId}", Name = "GetCartItems")]
    public async Task<IActionResult> GetCart(string shoppingCartId)
    {
        var result = await service.GetShoppingCartAsync(shoppingCartId);
        return Ok(result);
    }

    /// <summary>
    /// Adds an item to the shopping cart.
    /// </summary>
    /// <remarks>This method processes the provided shopping cart item and adds it to the user's shopping
    /// cart.  Ensure that the <paramref name="cartItem"/> contains valid data before calling this method.</remarks>
    /// <param name="cartItem">The item to add to the shopping cart, including its details such as product ID, quantity, and any other required
    /// information.</param>
    /// <returns>An <see cref="IActionResult"/> containing the result of the operation. Typically, this includes the updated
    /// shopping cart or a status indicating success or failure.</returns>
	[HttpPut("cart/add")]
    public async Task<IActionResult> AddCartItem([FromBody] ShoppingCartItemAddUpdateDto cartItem)
    {
        var result = await service.AddShoppingCartItemAsync(cartItem, HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Removes a specific item from the shopping cart.
    /// </summary>
    /// <remarks>This method calls the underlying service to remove the specified product from the shopping
    /// cart.  Ensure that both <paramref name="shoppingCartId"/> and <paramref name="productId"/> are valid and
    /// correspond to existing entities.</remarks>
    /// <param name="shoppingCartId">The unique identifier of the shopping cart from which the item will be removed. Cannot be null or empty.</param>
    /// <param name="productId">The unique identifier of the product to remove from the shopping cart. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Typically returns an HTTP 200 OK response
    /// with the operation result.</returns>
	[HttpDelete("cart/remove/{shoppingCartId}/{productId}", Name = "RemoveCartItem")]
    public async Task<IActionResult> RemoveCartItem(string shoppingCartId, string productId)
    {
        var response = await service.RemoveShoppingCartItemAsync(shoppingCartId, productId);
        return Ok(response);
    }

    /// <summary>
    /// Migrates the contents of an existing shopping cart to a new cart.
    /// </summary>
    /// <remarks>This operation transfers all items from the specified shopping cart to a new cart identified
    /// by <paramref name="newCartId"/>. Ensure that both <paramref name="shoppingCartId"/> and <paramref
    /// name="newCartId"/> are valid and not null or empty.</remarks>
    /// <param name="shoppingCartId">The identifier of the shopping cart to be migrated.</param>
    /// <param name="newCartId">The identifier of the new shopping cart to which the contents will be migrated.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the migration operation.  Returns <see langword="Ok"/>
    /// with the migration result if the operation is successful.</returns>
	[HttpPost("cart/migrate/{shoppingCartId}", Name = "MigrateCart")]
    public async Task<IActionResult> MigrateCart(string shoppingCartId, [FromBody] string newCartId)
    {
        return Ok(await service.MigrateAsync(shoppingCartId, newCartId));
    }

    /// <summary>
    /// Empties the shopping cart associated with the specified shopping cart ID.
    /// </summary>
    /// <remarks>This operation removes all items from the specified shopping cart. The method is asynchronous
    /// and should be awaited.</remarks>
    /// <param name="shoppingCartId">The unique identifier of the shopping cart to be emptied. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns <see cref="OkObjectResult"/> if
    /// the cart is successfully emptied.</returns>
	[HttpDelete("cart/empty/{shoppingCartId}", Name = "EmptyCartAsync")]
    public async Task<IActionResult> EmptyCart(string shoppingCartId)
    {        
        return Ok(await service.EmptyAsync(shoppingCartId));
    }

    /// <summary>
    /// Processes the specified shopping cart and returns the result of the operation.
    /// </summary>
    /// <remarks>This method invokes the service layer to process the shopping cart identified by <paramref
    /// name="shoppingCartId"/>. Ensure that the provided shopping cart ID is valid and corresponds to an existing
    /// cart.</remarks>
    /// <param name="shoppingCartId">The unique identifier of the shopping cart to process. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> containing the result of the processing operation. Typically, this will be an
    /// HTTP 200 OK response with the processing result.</returns>
    [HttpPost("cart/process/{shoppingCartId}")]
    public async Task<IActionResult> ProcessCart(string shoppingCartId)
    {
        return Ok(await service.ProcessAsync(shoppingCartId));
    }

    /// <summary>
    /// Removes all quantities of a specific product from the specified shopping cart.
    /// </summary>
    /// <remarks>This operation removes all instances of the specified product from the shopping cart.  If the
    /// product does not exist in the cart, the operation has no effect.</remarks>
    /// <param name="shoppingCartId">The unique identifier of the shopping cart from which the product will be removed.</param>
    /// <param name="productId">The unique identifier of the product to remove from the shopping cart.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.  Typically returns an HTTP 200 OK
    /// response if the operation is successful.</returns>
	[HttpDelete("cart/remove/{shoppingCartId}/{productId}/all", Name = "RemoveCartItemAll")]
    public async Task<IActionResult> RemoveCartItemAll(string shoppingCartId, string productId)
    {        
        return Ok(await service.RemoveShoppingCartItemAsync(shoppingCartId, productId, true));
    }

    /// <summary>
    /// Removes all shopping cart items for the specified user.
    /// </summary>
    /// <remarks>This action deletes all items in the shopping cart for the specified user.  Ensure that the
    /// <paramref name="userId"/> is valid and corresponds to an existing user.</remarks>
    /// <param name="userId">The unique identifier of the user whose shopping cart items are to be removed. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.  Returns <see cref="OkObjectResult"/> if
    /// the operation is successful.</returns>
    [HttpDelete("cart/removeforuser/{userId}")]
    public async Task<IActionResult> RemoveCartForSpecificUser(string userId)
    {
        return Ok(await service.RemoveShoppingCartItemsForUserAsync(userId));
    }
}
