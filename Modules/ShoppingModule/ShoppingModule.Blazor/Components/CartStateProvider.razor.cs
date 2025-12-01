using System.Security.Claims;
using AdvertisingModule.Domain.DataTransferObjects;
using Blazored.LocalStorage;
using BusinessModule.Domain.DataTransferObjects;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.Enums;
using IdentityModule.Domain.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ProductsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.DataTransferObjects;
using ShoppingModule.Application.ViewModels;
using ShoppingModule.Blazor.Models;
using ShoppingModule.Domain.DataTransferObjects;
using ShoppingModule.Domain.Enums;
using ShoppingModule.Domain.Interfaces;

namespace ShoppingModule.Blazor.Components
{
    /// <summary>
    /// Component for managing the state of the shopping cart.
    /// </summary>
    public partial class CartStateProvider
    {
        private string _shoppingCartId = "MyShoppingCart";
        private bool _hasLoaded;
        public event Action OnShoppingCartChanged = null!;
        private AuthenticationState _authentication = null!;

        #region Injectors and Parameters

        /// <summary>
        /// Gets or sets the content to be rendered inside the component.
        /// </summary>
        /// <remarks>Use this property to define the content that will be rendered within the component.
        /// Typically, this is set using Razor syntax in the parent component.</remarks>
        [Parameter] public RenderFragment ChildContent { get; set; } = null!;

        /// <summary>
        /// Gets or sets the HTTP provider used to perform HTTP requests.
        /// </summary>
        [Inject] public ICouponService CouponService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage the shopping cart for the current user.
        /// </summary>
        /// <remarks>This property is typically injected by the dependency injection framework. The
        /// service provides methods for adding, removing, and retrieving items in the shopping cart.</remarks>
        [Inject] public IShoppingCartService ShoppingCartService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to access protected local storage for the current user.
        /// </summary>
        /// <remarks>This property is typically set by dependency injection. Use this service to securely
        /// store and retrieve data in the browser's local storage, with data protection applied to prevent tampering or
        /// unauthorized access.</remarks>
        [Inject] public ILocalStorageService ProtectedLocalStorage { get; set; } = null!;

        /// <summary>
        /// Gets or sets the provider used to obtain authentication state information for the current user.
        /// </summary>
        /// <remarks>This property is typically set by the Blazor framework through dependency injection.
        /// Use this provider to query or subscribe to authentication state changes within the component.</remarks>
        [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the shopping cart associated with the current user session.
        /// </summary>
        public ShoppingCart? ShoppingCart { get; set; }

        #endregion

        #region Shipping & Voucher Functions

        /// <summary>
        /// Sets the shipping option for the shopping cart.
        /// </summary>
        /// <param name="shipping">The shipping option.</param>
        public void SetShipping(ShippingOption shipping)
        {
            ShoppingCart!.Shipping = shipping.Ammount;
            SaveChanges();
        }

        /// <summary>
        /// Sets the voucher for the shopping cart.
        /// </summary>
        /// <param name="couponCode">The coupon code.</param>
        public async Task SetCartVoucher(string couponCode)
        {
            var result = await CouponService.CouponByCodeAsync(couponCode);
            if (DateTime.Now.Date <= result.Data.DeActivationDate)
            {
                ShoppingCart!.Coupon = new CouponViewModel(result.Data);
            }
            SaveChanges();
        }

        #endregion

        #region Cart Functions

        /// <summary>
        /// Adds a product to the shopping cart asynchronously.
        /// </summary>
        /// <remarks>This method requires the user to be authenticated. If the user is not authenticated, the operation
        /// will fail. The method updates the shopping cart both in memory and in local storage upon successful addition of the
        /// product.</remarks>
        /// <param name="dto">The product details to be added to the shopping cart.</param>
        /// <param name="qty">The quantity of the product to add. Must be greater than zero.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>  indicating
        /// the success or failure of the operation.</returns>
        public async Task<IBaseResult> AddToCartAsync(ProductDto dto, int qty)
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            if (authState.User.Identity!.IsAuthenticated)
            {
                _shoppingCartId = authState.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            }

            var addModel = new ShoppingCartItemAddUpdateDto(_shoppingCartId, dto.ProductId, 
                dto.Images?.FirstOrDefault(c => c.ImageType == UploadType.Cover)?.RelativePath, dto.DisplayName,
                dto.ShortDescription, dto.Sku, dto.Pricing, qty);
            
            var response = await ShoppingCartService.AddShoppingCartItemAsync(addModel);
            if (!response.Succeeded)
                return await Result.FailAsync(response.Messages);

            ShoppingCart!.Add(response.Data);
            await ProtectedLocalStorage.SetItemAsync(_shoppingCartId, ShoppingCart);
            SaveChanges();

            return await Result.SuccessAsync();
        }

        /// <summary>
        /// Adds an item to the shopping cart asynchronously.
        /// </summary>
        /// <remarks>This method requires the user to be authenticated. If the user is not authenticated,
        /// the operation will not proceed. The method adds the specified item to the shopping cart and persists the
        /// updated cart to local storage.</remarks>
        /// <param name="dto">The advertisement tier data transfer object containing details about the item to add to the shopping cart.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
        public async Task<IBaseResult> AddToCartAsync(AdvertisementTierDto dto)
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            if (authState.User.Identity!.IsAuthenticated)
            {
                _shoppingCartId = authState.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            }

            var addModel = new ShoppingCartItemAddUpdateDto(_shoppingCartId, dto.Id, dto.Images?.FirstOrDefault(c => c.ImageType == UploadType.Cover)?.RelativePath, dto.Name,
                dto.Description, "", new PricingDto() { SellingPrice = dto.Price }, 1, ShoppingCartItemType.Advertising);

            addModel.Metadata.Add(new ShoppingCartItemMetadataDto("UserId", _shoppingCartId, addModel.ShoppingCartItemId));
            addModel.Metadata.Add(new ShoppingCartItemMetadataDto("AdvertisementId", Guid.NewGuid().ToString(), addModel.ShoppingCartItemId));

            var response = await ShoppingCartService.AddShoppingCartItemAsync(addModel);
            if (!response.Succeeded)
                return await Result.FailAsync(response.Messages);

            ShoppingCart!.Add(response.Data);
            await ProtectedLocalStorage.SetItemAsync(_shoppingCartId, ShoppingCart);
            SaveChanges();

            return await Result.SuccessAsync();
        }

        /// <summary>
        /// Adds a specified item to the shopping cart asynchronously.
        /// </summary>
        /// <remarks>This method requires the user to be authenticated. If the user is not authenticated,
        /// the operation will not proceed. The method updates the shopping cart both in memory and in local storage,
        /// ensuring the changes are persisted.</remarks>
        /// <param name="dto">The data transfer object containing details about the item to be added to the shopping cart. This includes
        /// the item's identifier, name, description, price, and associated images.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation. If the operation fails, the result includes error
        /// messages.</returns>
        public async Task<IBaseResult> AddToCartAsync(string listingId, ListingTierDto dto)
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            if (authState.User.Identity!.IsAuthenticated)
            {
                _shoppingCartId = authState.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            }

            var addModel = new ShoppingCartItemAddUpdateDto(_shoppingCartId, dto.Id, dto.Images?.FirstOrDefault(c => c.ImageType == UploadType.Cover)?.RelativePath, dto.Name,
                dto.Description, "", new PricingDto() { SellingPrice = dto.Price }, 1, ShoppingCartItemType.Service);
            
            addModel.Metadata.Add(new ShoppingCartItemMetadataDto("UserId", _shoppingCartId, addModel.ShoppingCartItemId));
            addModel.Metadata.Add(new ShoppingCartItemMetadataDto("BusinessListingId", dto.Id, addModel.ShoppingCartItemId));

            var response = await ShoppingCartService.AddShoppingCartItemAsync(addModel);
            if (!response.Succeeded)
                return await Result.FailAsync(response.Messages);

            ShoppingCart!.Add(response.Data);
            await ProtectedLocalStorage.SetItemAsync(_shoppingCartId, ShoppingCart);
            SaveChanges();

            return await Result.SuccessAsync();
        }

        /// <summary>
        /// Adds a service item to the shopping cart asynchronously.
        /// </summary>
        /// <remarks>This method requires the user to be authenticated. If the user is not authenticated,
        /// the operation will fail. The method updates the shopping cart both in memory and in local storage, ensuring
        /// the changes are persisted.</remarks>
        /// <param name="dto">The data transfer object representing the service to be added to the shopping cart.  This includes details
        /// such as the service ID, name, description, price, and associated images.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the success or failure of the operation, along with any relevant messages.</returns>
        public async Task<IBaseResult> AddToCartAsync(ServiceTierDto dto)
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            if (authState.User.Identity!.IsAuthenticated)
            {
                _shoppingCartId = authState.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            }

            var addModel = new ShoppingCartItemAddUpdateDto(_shoppingCartId, dto.Id, dto.Images?.FirstOrDefault(c => c.ImageType == UploadType.Cover)?.RelativePath, dto.Name,
                dto.Description, "", new PricingDto() { SellingPrice = dto.Price }, 1, ShoppingCartItemType.Service);

            addModel.Metadata.Add(new ShoppingCartItemMetadataDto("UserId", _shoppingCartId, addModel.ShoppingCartItemId));

            var response = await ShoppingCartService.AddShoppingCartItemAsync(addModel);
            if (!response.Succeeded)
                return await Result.FailAsync(response.Messages);

            ShoppingCart!.Add(response.Data);
            await ProtectedLocalStorage.SetItemAsync(_shoppingCartId, ShoppingCart);
            SaveChanges();

            return await Result.SuccessAsync();
        }

        /// <summary>
        /// Adds a ticket to the shopping cart asynchronously.
        /// </summary>
        /// <remarks>This method requires the user to be authenticated. The shopping cart is updated with
        /// the provided ticket details,  and the changes are persisted locally. If the operation fails, the returned
        /// result will include the failure messages.</remarks>
        /// <param name="dto">The ticket details to add to the shopping cart, including its ID, name, description, and price.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the success or failure of the operation.</returns>
        public async Task<IBaseResult> AddToCartAsync(SchoolEventTicketTypeDto dto)
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            if (authState.User.Identity!.IsAuthenticated)
            {
                _shoppingCartId = authState.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            }

            var addModel = new ShoppingCartItemAddUpdateDto(_shoppingCartId, dto.Id, "", dto.Name,
                dto.Description, "", new PricingDto() { SellingPrice = dto.Price }, 1, ShoppingCartItemType.Service);

            addModel.Metadata.Add(new ShoppingCartItemMetadataDto("UserId", _shoppingCartId, addModel.ShoppingCartItemId));

            var response = await ShoppingCartService.AddShoppingCartItemAsync(addModel);
            if (!response.Succeeded)
                return await Result.FailAsync(response.Messages);

            ShoppingCart!.Add(response.Data);
            await ProtectedLocalStorage.SetItemAsync(_shoppingCartId, ShoppingCart);
            SaveChanges();

            return await Result.SuccessAsync();
        }

        /// <summary>
        /// Removes a product from the shopping cart by product ID.
        /// </summary>
        /// <param name="productId">The product ID.</param>
        /// <returns>The result of the operation.</returns>
        public async Task<IBaseResult> RemoveFromCartAsync(string productId)
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            if (authState.User.Identity!.IsAuthenticated)
            {
                _shoppingCartId = authState.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            }

            var response = await ShoppingCartService.RemoveShoppingCartItemAsync(_shoppingCartId, productId);
            if (response.Succeeded)
            {
                ShoppingCart!.Remove(productId);
                await ProtectedLocalStorage.SetItemAsync(_shoppingCartId, ShoppingCart);

                return Result.Success();
            }
            else
            {
                return Result.Fail(response.Messages);
            }
        }

        /// <summary>
        /// Removes a product from the shopping cart by product view model.
        /// </summary>
        /// <param name="product">The product view model.</param>
        /// <returns>The result of the operation.</returns>
        public async Task<IBaseResult> RemoveFromCartAsync(CartItemViewModel product)
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            if (authState.User.Identity!.IsAuthenticated)
            {
                _shoppingCartId = authState.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            }

            var response = await ShoppingCartService.RemoveShoppingCartItemAsync(_shoppingCartId, product.ProductId);
            if (!response.Succeeded) return await Result.FailAsync(response.Messages);

            ShoppingCart!.Remove(product.ProductId);
            await ProtectedLocalStorage.RemoveItemAsync(_shoppingCartId);
            await ProtectedLocalStorage.SetItemAsync(_shoppingCartId, ShoppingCart);
            OnShoppingCartChanged.Invoke();

            return await Result.SuccessAsync();
        }

        /// <summary>
        /// Resets the shopping cart.
        /// </summary>
        public async Task ResetCartAsync()
        {
            await ProtectedLocalStorage.RemoveItemAsync(_shoppingCartId);

            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            if (authState.User.Identity!.IsAuthenticated)
            {
                _shoppingCartId = authState.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            }
            else
            {
                _shoppingCartId = "MyShoppingCart";
            }

            var localStorage = await ProtectedLocalStorage.GetItemAsync<ShoppingCart>(_shoppingCartId);

            if (localStorage == null)
            {
                ShoppingCart = new ShoppingCart(_shoppingCartId);
            }
            else
            {
                if (DateTime.Now > ShoppingCart!.LastAccessed.AddDays(ShoppingCart.TimeBeforeExpiryInDays))
                {
                    ShoppingCart = new ShoppingCart(_shoppingCartId);
                }
                else
                {
                    ShoppingCart = localStorage;
                    ShoppingCart.LastAccessed = DateTime.Now;
                }
            }

            await ProtectedLocalStorage.SetItemAsync(_shoppingCartId, ShoppingCart);
            _hasLoaded = true;

            SaveChanges();
        }

        /// <summary>
        /// Empties the shopping cart.
        /// </summary>
        /// <returns>The result of the operation.</returns>
        public async Task<IBaseResult> EmptyCartAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            if (authState.User.Identity!.IsAuthenticated)
            {
                var response = await ShoppingCartService.EmptyAsync(authState.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                if (!response.Succeeded)
                {
                    return await Result.FailAsync(response.Messages);
                }
            }

            if (ShoppingCart is not null)
            {
                ShoppingCart.CartItems?.Clear();
                ShoppingCart.Coupon = null;

                await ProtectedLocalStorage.SetItemAsync(_shoppingCartId, new ShoppingCart(authState.User.GetUserId()));
            }

            SaveChanges();

            return await Result.SuccessAsync();
        }

        /// <summary>
        /// Migrates the current shopping cart to the authenticated user's account.
        /// </summary>
        /// <remarks>This method associates the current shopping cart with the authenticated user by
        /// sending a migration request to the server. Upon successful migration, the local storage is updated to
        /// reflect the new association.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the migration. If the operation fails, the result includes error
        /// messages.</returns>
        public async Task<IBaseResult> MigrateCartAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var response = await ShoppingCartService.MigrateAsync(_shoppingCartId, authState.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            if (!response.Succeeded)
            {
                return await Result.FailAsync(response.Messages);
            }

            await ProtectedLocalStorage.RemoveItemAsync(_shoppingCartId);
            await ProtectedLocalStorage.SetItemAsync(authState.User.FindFirst(ClaimTypes.NameIdentifier)!.Value, ShoppingCart);

            SaveChanges();

            return await Result.SuccessAsync();
        }

        #endregion

        #region Lifetime Management

        /// <summary>
        /// Saves changes to the shopping cart.
        /// </summary>
        public void SaveChanges()
        {
            if (ShoppingCart is not null)
            {
                ShoppingCart.LastAccessed = DateTime.Now;
            }

            OnShoppingCartChanged?.Invoke();
        }

        /// <summary>
        /// Executes after the component has rendered.
        /// </summary>
        /// <param name="firstRender">Indicates whether this is the first render.</param>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                _authentication = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                if (_authentication.User.Identity!.IsAuthenticated)
                {
                    _shoppingCartId = _authentication.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                }

                var localStorage = await ProtectedLocalStorage.GetItemAsync<ShoppingCart>(_shoppingCartId);

                if (localStorage == null)
                {
                    if (_authentication.User.Identity!.IsAuthenticated)
                    {
                        var shoppingCartItems = await ShoppingCartService.GetShoppingCartAsync(_shoppingCartId);
                        if (shoppingCartItems.Data.Items.Any())
                        {
                            ShoppingCart = new ShoppingCart(shoppingCartItems.Data);
                        }
                        else
                        {
                            ShoppingCart = new ShoppingCart(_shoppingCartId);
                        }
                    }
                    else
                    {
                        ShoppingCart = new ShoppingCart(_shoppingCartId);
                    }
                }
                else
                {
                    if (!_authentication.User.Identity!.IsAuthenticated)
                    {
                        if (DateTime.Now > localStorage.LastAccessed.AddDays(localStorage.TimeBeforeExpiryInDays))
                        {
                            ShoppingCart = new ShoppingCart(_shoppingCartId);
                        }
                        else
                        {
                            ShoppingCart = localStorage;
                            ShoppingCart.LastAccessed = DateTime.Now;
                        }
                    }
                    else
                    {
                        var shoppingCartItems = await ShoppingCartService.GetShoppingCartAsync(_shoppingCartId);
                        if (shoppingCartItems.Data.Items.Any())
                        {
                            ShoppingCart = new ShoppingCart(shoppingCartItems.Data);
                        }
                        else
                        {
                            ShoppingCart = new ShoppingCart(_shoppingCartId);
                        }
                        await ProtectedLocalStorage.RemoveItemAsync("MyShoppingCart");
                    }
                }

                await ProtectedLocalStorage.SetItemAsync(_shoppingCartId, ShoppingCart);
                _hasLoaded = true;

                StateHasChanged();
            }
        }

        #endregion
    }
}
