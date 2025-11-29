using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using Microsoft.EntityFrameworkCore;
using ProductsModule.Domain.Interfaces;
using ShoppingModule.Domain.DataTransferObjects;
using ShoppingModule.Domain.Entities;
using ShoppingModule.Domain.Interfaces;

namespace ShoppingModule.Infrastructure.Implementation
{
    /// <summary>
    /// Provides functionality for managing shopping carts, including retrieving, adding, removing,  processing, and
    /// migrating shopping cart items.
    /// </summary>
    /// <remarks>This service is designed to handle operations related to shopping carts, such as retrieving 
    /// cart details, adding or removing items, processing the cart, and migrating cart data.  It interacts with a
    /// repository to perform data access operations and ensures that the  appropriate results are returned to the
    /// caller, including success or failure messages.</remarks>
    /// <param name="repository"></param>
    public sealed class ShoppingCartService(IRepository<ShoppingCartItem, string> repository, IRepository<ShoppingCartCoupon, string> couponRepository,
        IProductService productService) : IShoppingCartService
    {
        /// <summary>
        /// Retrieves the shopping cart details for the specified shopping cart identifier.
        /// </summary>
        /// <remarks>This method retrieves the items in the shopping cart and maps them to a
        /// <ShoppingCartDto>  object. If the shopping cart contains no items, the returned DTO will have an empty item
        /// list.  In case of failure, the result will include error messages describing the issue.</remarks>
        /// <param name="shoppingCartId">The unique identifier of the shopping cart to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <IBaseResult{T}> of type
        /// <ShoppingCartDto>. If successful, the result  contains the shopping cart details; otherwise, it contains
        /// error messages.</returns>
        public async Task<IBaseResult<ShoppingCartDto>> GetShoppingCartAsync(string shoppingCartId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<ShoppingCartItem>(c => c.ShoppingCartId == shoppingCartId);
            spec.AddInclude(c => c.Include(g => g.Metadata));

            var cartItemsResult = await repository.ListAsync(spec, false, cancellationToken);
            if (cartItemsResult.Succeeded)
            {
                var dto = new ShoppingCartDto();

                if (cartItemsResult.Data.Any())
                {
                    foreach (var cartItem in cartItemsResult.Data)
                    {
                        dto.Items.Add(new ShoppingCartItemDto(cartItem));
                    }
                }
                return await Result<ShoppingCartDto>.SuccessAsync(dto);
            }
            return await Result<ShoppingCartDto>.FailAsync(cartItemsResult.Messages);
        }

        /// <summary>
        /// Adds a new item to the shopping cart asynchronously.
        /// </summary>
        /// <remarks>This method attempts to add a new item to the shopping cart. If the operation is
        /// successful, the result will include the added item. If the operation fails, the result will include error
        /// messages describing the failure.</remarks>
        /// <param name="addUpdateModel">The data transfer object representing the shopping cart item to be added.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with a <see cref="ShoppingCartItemDto"/> representing the added shopping cart item if the operation
        /// succeeds, or error messages if it fails.</returns>
        public async Task<IBaseResult<ShoppingCartItemDto>> AddShoppingCartItemAsync(ShoppingCartItemAddUpdateDto addUpdateModel, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<ShoppingCartItem>(c => c.ShoppingCartId == addUpdateModel.ShoppingCartId && c.ProductId == addUpdateModel.ProductId);

            var cartItem = await repository.FirstOrDefaultAsync(spec, true, cancellationToken);
            if (!cartItem.Succeeded) return await Result<ShoppingCartItemDto>.FailAsync(cartItem.Messages);
            if (cartItem.Data is null)
            {
                var shoppingCartItemResponse = await repository.CreateAsync(addUpdateModel.ToShoppingCartItem(), cancellationToken);

                if (!shoppingCartItemResponse.Succeeded) return await Result<ShoppingCartItemDto>.FailAsync(shoppingCartItemResponse.Messages);
                var result = await repository.SaveAsync(cancellationToken);
                if (!result.Succeeded) return await Result<ShoppingCartItemDto>.FailAsync(result.Messages);
                return await Result<ShoppingCartItemDto>.SuccessAsync(new ShoppingCartItemDto(shoppingCartItemResponse.Data));
            }

            cartItem.Data.Qty = cartItem.Data.Qty + addUpdateModel.Qty;

            if (addUpdateModel.Metadata.Any())
            {
                foreach (var metadataDto in addUpdateModel.Metadata.Where(m => !string.IsNullOrWhiteSpace(m.Name)))
                {
                    var existingMetadata = cartItem.Data.Metadata.FirstOrDefault(m => m.Name == metadataDto.Name);
                    if (existingMetadata is null)
                    {
                        var metadata = new ShoppingCartItemMetadata(metadataDto)
                        {
                            ShoppingCartItemId = cartItem.Data.Id
                        };
                        cartItem.Data.Metadata.Add(metadata);
                    }
                    else
                    {
                        existingMetadata.Value = metadataDto.Value;
                    }
                }
            }

            var updateResponse = repository.Update(cartItem.Data);
            var saveResult = await repository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result<ShoppingCartItemDto>.FailAsync(saveResult.Messages);
            if (!updateResponse.Succeeded) return await Result<ShoppingCartItemDto>.FailAsync(updateResponse.Messages);
            return await Result<ShoppingCartItemDto>.SuccessAsync(new ShoppingCartItemDto(updateResponse.Data));
        }

        /// <summary>
        /// Adds a new item to the shopping cart asynchronously.
        /// </summary>
        /// <remarks>This method attempts to add a new item to the shopping cart. If the operation is
        /// successful, the item is persisted  to the repository and returned as part of the result. If the operation
        /// fails, the result will include the relevant  error messages.</remarks>
        /// <param name="addUpdateModel">The data transfer object representing the shopping cart item to be added.  This must contain valid item
        /// details.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// where <c>T</c> is <see cref="ShoppingCartItemDto"/>. If the operation succeeds, the result contains the
        /// added  shopping cart item. If the operation fails, the result contains error messages.</returns>
        public async Task<IBaseResult<ShoppingCartItemDto>> AddShoppingCartItemAsync(ShoppingCartItemDto addUpdateModel, CancellationToken cancellationToken = default)
        {
            var shoppingCartItemResponse = await repository.CreateAsync(addUpdateModel.ToShoppingCartItem(), cancellationToken);

            if (shoppingCartItemResponse.Succeeded)
            {
                var result = await repository.SaveAsync(cancellationToken);
                if (!result.Succeeded)
                    return await Result<ShoppingCartItemDto>.FailAsync(result.Messages);

                return await Result<ShoppingCartItemDto>.SuccessAsync(new ShoppingCartItemDto(shoppingCartItemResponse.Data));
            }
            return await Result<ShoppingCartItemDto>.FailAsync(shoppingCartItemResponse.Messages);
        }

        /// <summary>
        /// Removes an item from the shopping cart.
        /// </summary>
        /// <remarks>If the specified product is not found in the shopping cart, the method completes
        /// successfully without making changes.</remarks>
        /// <param name="shoppingCartId">The unique identifier of the shopping cart from which the item will be removed.</param>
        /// <param name="productId">The unique identifier of the product to be removed from the shopping cart.</param>
        /// <param name="all">A boolean value indicating whether all instances of the product should be removed.  If <see
        /// langword="true"/>, all instances of the product are removed; otherwise, only one instance is removed.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the success or failure of the operation.</returns>
        public async Task<IBaseResult> RemoveShoppingCartItemAsync(string shoppingCartId, string productId, bool all = false, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<ShoppingCartItem>(c => c.ShoppingCartId == shoppingCartId && c.ProductId == productId);

            var cartItem = await repository.FirstOrDefaultAsync(spec, true, cancellationToken);
            if (!cartItem.Succeeded) return await Result<ShoppingCartItemDto>.FailAsync(cartItem.Messages);
            if (cartItem.Data is null) return await Result<ShoppingCartItemDto>.SuccessAsync("Shopping Cart Item not found");

            repository.Delete(cartItem.Data);

            var result = await repository.SaveAsync(cancellationToken);
            if (!result.Succeeded) return await Result<ShoppingCartItemDto>.FailAsync(result.Messages);
            return await Result<ShoppingCartItemDto>.SuccessAsync("Shopping Cart Item successfully removed");
            
        }

        /// <summary>
        /// Removes all shopping cart items associated with the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose shopping cart items are to be removed. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation. If the operation succeeds, the result includes a success
        /// message. If it fails, the result includes the failure messages.</returns>
        public async Task<IBaseResult> RemoveShoppingCartItemsForUserAsync(string userId, CancellationToken cancellationToken = default)
        {
            try
            {
                var spec = new LambdaSpec<ShoppingCartItem>(c => c.ParentId == userId);
                var products = await repository.ListAsync(spec, true, cancellationToken);
                foreach (var product in products.Data)
                {
                    repository.Delete(product);
                }

                var result = await repository.SaveAsync(cancellationToken);
                if (!result.Succeeded) return await Result<ShoppingCartItemDto>.FailAsync(result.Messages);

                return await Result<ShoppingCartItemDto>.SuccessAsync("Shopping Cart Item successfully removed");
            }
            catch (Exception e)
            {
                return await Result.FailAsync(e.Message);
            };
        }

        /// <summary>
        /// Processes the specified shopping cart by removing all items and saving the changes.
        /// </summary>
        /// <param name="shoppingCartId">The unique identifier of the shopping cart to process. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation. On success, the result includes a success message. On
        /// failure, the result includes an error message.</returns>
        public async Task<IBaseResult> ProcessAsync(string shoppingCartId, CancellationToken cancellationToken = default)
        {
            try
            {
                var spec = new LambdaSpec<ShoppingCartItem>(c => c.ShoppingCartId == shoppingCartId);
                var cartItems = await repository.ListAsync(spec, true, cancellationToken);

                if (!cartItems.Succeeded)
                    return await Result.FailAsync(cartItems.Messages);

                foreach (var item in cartItems.Data)
                {
                    //var inventoryResult = await productService.ProductInventorySettingsAsync(item.ProductId, cancellationToken);
                    //if (!inventoryResult.Succeeded)
                    //    return await Result.FailAsync(inventoryResult.Messages);

                    //var inventory = inventoryResult.Data;
                    //inventory.InStock -= item.Qty;
                    //var updateResult = await productService.UpdateProductInventorySettingsAsync(inventory, cancellationToken);
                    //if (!updateResult.Succeeded)
                    //    return await Result.FailAsync(updateResult.Messages);

                    repository.Delete(item);
                }

                var couponSpec = new LambdaSpec<ShoppingCartCoupon>(c => c.ShoppingCartId == shoppingCartId);
                var coupon = await couponRepository.FirstOrDefaultAsync(couponSpec, true, cancellationToken);
                if (coupon.Succeeded && coupon.Data is not null)
                {
                    couponRepository.Delete(coupon.Data);
                }

                await repository.SaveAsync(cancellationToken);
                return await Result.SuccessAsync("Shopping Cart successfully processed");
            }
            catch (Exception ex)
            {
                return await Result.FailAsync(ex.Message);
            }
        }

        /// <summary>
        /// Empties the specified shopping cart by removing all items and associated coupons.
        /// </summary>
        /// <remarks>This method removes all items and any associated coupon from the specified shopping
        /// cart.  Changes are persisted to the underlying data store. If an error occurs during the operation,  the
        /// returned result will indicate failure with the corresponding error message.</remarks>
        /// <param name="shoppingCartId">The unique identifier of the shopping cart to be emptied. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the success or failure of the operation. On success, the result includes a success message;  on
        /// failure, it includes an error message.</returns>
        public async Task<IBaseResult> EmptyAsync(string shoppingCartId, CancellationToken cancellationToken)
        {
            try
            {
                var spec = new LambdaSpec<ShoppingCartItem>(c => c.ShoppingCartId == shoppingCartId);

                var cartItems = await repository.ListAsync(spec, true, cancellationToken);
                foreach (var cartItem in cartItems.Data)
                {
                    repository.Delete(cartItem);
                }

                var couponSpec = new LambdaSpec<ShoppingCartItem>(c => c.ShoppingCartId == shoppingCartId);
                var coupon = await repository.FirstOrDefaultAsync(couponSpec, true, cancellationToken);
                if (coupon.Succeeded)
                {
                    repository.Delete(coupon.Data);
                }

                await repository.SaveAsync();

                return await Result.SuccessAsync("Shopping Cart Successfully Emptied");
            }
            catch (Exception ex)
            {
                return await Result.FailAsync(ex.Message); 
            }

        }

        /// <summary>
        /// Migrates the contents of a shopping cart to a new shopping cart identifier.
        /// </summary>
        /// <remarks>This method updates all items in the specified shopping cart to associate them with
        /// the new shopping cart identifier. If the migration is successful, the original shopping cart will no longer
        /// contain any items.</remarks>
        /// <param name="shoppingCartId">The identifier of the shopping cart to migrate from. Cannot be null or empty.</param>
        /// <param name="newCardId">The identifier of the new shopping cart to migrate to. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the migration. On success, the result includes a success message; on
        /// failure, the result includes an error message.</returns>
        public async Task<IBaseResult> MigrateAsync(string shoppingCartId, string newCardId, CancellationToken cancellationToken = default)
        {
            try
            {
                var spec = new LambdaSpec<ShoppingCartItem>(c => c.ShoppingCartId == shoppingCartId);
                
                var cartItems = await repository.ListAsync(spec, true, cancellationToken);
                foreach (var item in cartItems.Data)
                {
                    item.ShoppingCartId = newCardId;
                    repository.Update(item);
                }

                var couponSpec = new LambdaSpec<ShoppingCartCoupon>(c => c.ShoppingCartId == shoppingCartId);
                var couponResult = await couponRepository.FirstOrDefaultAsync(couponSpec, true, cancellationToken);

                if (couponResult.Data != null)
                    couponResult.Data.ShoppingCartId = newCardId;
                
                await repository.SaveAsync(cancellationToken);
                return await Result.SuccessAsync("Shopping Cart Successfully Migrated");
            }
            catch (Exception ex)
            {
                return await Result.FailAsync(ex.Message);
            }
        }
    }
}
