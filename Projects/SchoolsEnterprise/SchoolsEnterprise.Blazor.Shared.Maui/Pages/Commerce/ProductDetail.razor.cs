using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System.Security.Claims;
using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using ProductsModule.Domain.DataTransferObjects;
using ShoppingModule.Blazor.Components;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Commerce
{
    /// <summary>
    /// Represents the detail view for a specific product in the shopping flow.
    /// It displays breadcrumb navigation for the shopping area and 
    /// provides quick navigation to cart and checkout pages.
    /// </summary>
    public partial class ProductDetail
    {
        private ProductFullInfoDto _product;
        private int _qty = 1;
        private ClaimsPrincipal _user;

        // A list of breadcrumb items to help users understand their location 
        // within the shopping flow and navigate back if needed.
        private List<BreadcrumbItem> _items =
        [
            new("Shopping", href: "/shopping"),
            new("Product Details", href: null, disabled: true)
        ];

        /// <summary>
        /// Gets or sets the <see cref="CartStateProvider"/> instance used to manage the state of the shopping cart.
        /// </summary>
        [CascadingParameter] public CartStateProvider ShoppingCartService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the task that provides the current authentication state.
        /// </summary>
        /// <remarks>The <see cref="AuthenticationStateTask"/> is a cascading parameter, meaning it is
        /// provided by an ancestor component, such as the <c>AuthenticationStateProvider</c>. Ensure that the ancestor
        /// component supplies this value for proper functionality.</remarks>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Gets or sets the HTTP provider used to perform HTTP requests.
        /// </summary>
        /// <remarks>The HTTP provider is typically injected and used to manage HTTP communication within
        /// the application. Ensure that a valid implementation of <see cref="IBaseHttpProvider"/> is provided before
        /// using this property.</remarks>
        [Inject] public IBaseHttpProvider Provider { get; set; }

        /// <summary>
        /// Gets or sets the NavigationManager used to manage URI navigation and location state within the application.
        /// </summary>
        /// <remarks>This property is typically provided by dependency injection in Blazor applications.
        /// Use it to programmatically navigate to different URIs or to access information about the current navigation
        /// state.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Gets or sets the service used to display transient messages to the user.
        /// </summary>
        /// <remarks>This property is typically injected by the framework and provides methods for showing
        /// snack bar notifications, such as alerts or status messages, in the user interface.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; }

        /// <summary>
        /// Gets or sets the application configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the product.
        /// </summary>
        [Parameter] public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the currently selected product option.
        /// </summary>
        public ProductDto SelectedOption { get; set; }

        /// <summary>
        /// Resets the state of the object by clearing the selected option.
        /// </summary>
        /// <remarks>This method sets the <see cref="SelectedOption"/> property to <see langword="null"/>,
        /// effectively clearing any previously selected value.</remarks>
        private void Reset()
        {
            SelectedOption = null;
        }

        /// <summary>
        /// Navigates to the product detail page for the specified product.
        /// </summary>
        /// <remarks>This method constructs the URL for the product detail page using the provided
        /// <paramref name="productId"/>  and navigates to it using the <see cref="NavigationManager"/>.</remarks>
        /// <param name="productId">The unique identifier of the product to display. This value cannot be null or empty.</param>
        public void NavigateToProductDetail(string productId)
        {
            NavigationManager.NavigateTo($"/shopping/productdetail/{productId}");
        }

        /// <summary>
        /// Called by Blazor when initializing the component.
        /// Currently empty, but may include product retrieval or other 
        /// initialization logic in the future.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            _user = authState.User;

            var result = await Provider.GetAsync<ProductFullInfoDto>($"products/{ProductId}");
            if (result.Succeeded)
            {
                _product = result.Data;
                SelectedOption = CheckAndSetVariants(new ProductDto(_product));
            }
            else
                SnackBar.AddErrors(result.Messages);

        }

        /// <summary>
        /// Ensures that the product's variants are properly set and returns the first variant if available; otherwise,
        /// returns the original product.
        /// </summary>
        /// <param name="dto">The product data transfer object to check and process.</param>
        /// <returns>The first variant of the product if variants are present; otherwise, the original product.</returns>
        private ProductDto CheckAndSetVariants(ProductDto dto)
        {
            if (dto.Variants.Any())
            {
                return CheckAndSetVariants(dto.Variants.FirstOrDefault());
            }
            return dto;
            
        }

        /// <summary>
        /// Navigates the user to the checkout page (shopping/cart/checkout).
        /// </summary>
        public void NavigateToCheckout()
        {
            NavigationManager.NavigateTo("/shopping/cart/checkout");
        }

        /// <summary>
        /// Navigates the user to the cart detail page (shopping/cart).
        /// </summary>
        public void NavigateToCartDetail()
        {
            NavigationManager.NavigateTo("/shopping/cart");
        }

        /// <summary>
        /// Adds the specified product to the shopping cart asynchronously.
        /// </summary>
        /// <remarks>If the operation fails, error messages are displayed using the snack bar.  On
        /// success, a confirmation message is displayed.</remarks>
        /// <param name="dto">The product to add, represented as a <see cref="ProductDto"/> object. Must not be <see langword="null"/>.</param>
        /// <param name="qty">The quantity of the product to add. Must be greater than zero.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task AddToCartAsync(int qty)
        {
            var result = await ShoppingCartService.AddToCartAsync(SelectedOption with { Images = _product.Images }, qty);
            if (!result.Succeeded)
            {
                SnackBar.AddErrors(result.Messages);
            }
            else
            {
                SnackBar.AddSuccess($"Product '{SelectedOption.DisplayName} successfully added to cart'");
            }
        }
    }
}
