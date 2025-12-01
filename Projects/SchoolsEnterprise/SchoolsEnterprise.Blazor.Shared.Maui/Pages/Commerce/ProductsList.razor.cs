using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Interfaces;
using GroupingModule.Domain.DataTransferObjects;
using GroupingModule.Domain.RequestFeatures;
using Microsoft.Extensions.Configuration;
using MudBlazor;
using ProductsModule.Domain.DataTransferObjects;
using ProductsModule.Domain.RequestFeatures;
using ShoppingModule.Blazor.Components;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Commerce
{
    /// <summary>
    /// Represents a component that manages and displays a list of products and their associated categories.
    /// </summary>
    /// <remarks>This class provides functionality for interacting with product data, including loading
    /// product and category information, handling category selection, and navigating to product details. It also
    /// integrates with authentication and shopping cart services to enable user-specific operations, such as adding
    /// products to the cart.</remarks>
    public partial class ProductsList
    {
        public bool _loaded;
        private List<ProductDto> _products;
        private List<CategoryDto> _categories;
        private readonly Func<CategoryDto?, string?> _categoryConverter = p => p?.Name;
        private CategoryDto _selectedCategory;
        private ClaimsPrincipal _user;

        /// <summary>
        /// Gets or sets the task that provides the current authentication state.
        /// </summary>
        /// <remarks>This property is typically used in Blazor components to access the authentication
        /// state of the current user. The task should be awaited to retrieve the <see
        /// cref="AuthenticationState"/>.</remarks>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing the state of the shopping cart.
        /// </summary>
        [CascadingParameter] public CartStateProvider ShoppingCartService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier for the category.
        /// </summary>
        [Parameter] public string CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the HTTP provider used for making HTTP requests.
        /// </summary>
        [Inject] public IBaseHttpProvider Provider { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications to the user.
        /// </summary>
        /// <remarks>This property is typically set by dependency injection. Use the provided ISnackbar
        /// instance to show transient messages or alerts within the application's user interface.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the NavigationManager used to manage and interact with URI navigation in the application.
        /// </summary>
        /// <remarks>This property is typically provided by dependency injection in Blazor applications.
        /// Use it to programmatically navigate to different URIs or to access information about the current navigation
        /// state.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the navigation manager used to programmatically navigate and obtain information about the
        /// current URI in the application.
        /// </summary>
        /// <remarks>The navigation manager provides methods for navigating to different URIs and for
        /// handling navigation events within a Blazor application. This property is typically injected by the
        /// framework.</remarks>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Handles the event triggered when the category selection changes.
        /// </summary>
        /// <remarks>This method retrieves a paged list of categories from the data provider and updates
        /// the internal category list. If the operation fails, error messages are displayed using the
        /// snackbar.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task OnCategorySelectionChanged()
        {
            var pagingResponse = await Provider.GetPagedAsync<CategoryDto, CategoryPageParameters>("products/categories/paged", new CategoryPageParameters() { PageSize = 100 });
            if (!pagingResponse.Succeeded)
            {
                SnackbarExtensions.AddErrors(SnackBar, pagingResponse.Messages);
            }
            else
            {
                _categories = pagingResponse.Data;
                _selectedCategory = pagingResponse.Data.FirstOrDefault(c => c.CategoryId == CategoryId);
            }
        }

        /// <summary>
        /// Adds the specified product to the shopping cart asynchronously.
        /// </summary>
        /// <remarks>If the operation fails, error messages are displayed using the snack bar.  On
        /// success, a confirmation message is displayed.</remarks>
        /// <param name="dto">The product to add, represented as a <see cref="ProductDto"/> object. Must not be <see langword="null"/>.</param>
        /// <param name="qty">The quantity of the product to add. Must be greater than zero.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task AddToCartAsync(ProductDto dto, int qty)
        {
            var result = await ShoppingCartService.AddToCartAsync(dto, qty);
            if (!result.Succeeded)
            {
                SnackBar.AddErrors(result.Messages);
            }
            else
            {
                SnackBar.AddSuccess($"Product '{dto.DisplayName} successfully added to cart'");
            }
        }

        /// <summary>
        /// Asynchronously initializes the component by loading authentication state, categories, and products.
        /// </summary>
        /// <remarks>This method retrieves the current authentication state and uses it to initialize the
        /// user context.  It then fetches a paginated list of categories and products from the respective data
        /// providers.  If the data retrieval fails, error messages are displayed using the configured
        /// snackbar.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            _user = authState.User;

            var pagingResponse = await Provider.GetPagedAsync<CategoryDto, CategoryPageParameters>("products/categories/paged", new CategoryPageParameters() { PageSize = 100 });
            if (!pagingResponse.Succeeded)
            {
                SnackbarExtensions.AddErrors(SnackBar, pagingResponse.Messages);
            }
            else
            {
                _categories = pagingResponse.Data;
                _selectedCategory = pagingResponse.Data.FirstOrDefault(c => c.CategoryId == CategoryId);
            }

            var request = await Provider.GetPagedAsync<ProductDto, ProductsParameters>("products", new ProductsParameters() { PageSize = 100, CategoryIdFilter = CategoryId});
            if (request.Succeeded)
            {
                _products = request.Data;
            }
            else
            {
                SnackbarExtensions.AddErrors(SnackBar, request.Messages);
            }

            _loaded = true;
        }

        /// <summary>
        /// Navigates to the product detail page for the specified product.
        /// </summary>
        /// <remarks>The method constructs the URL for the product detail page using the provided
        /// <paramref name="productId"/>  and navigates to it. Ensure that <paramref name="productId"/> corresponds to a
        /// valid product in the system.</remarks>
        /// <param name="productId">The unique identifier of the product to display. This value cannot be null or empty.</param>
        public void NavigateToProductDetail(string productId)
        {
            NavigationManager.NavigateTo($"/shopping/productdetail/{productId}");
        }

        /// <summary>
        /// Represents a menu with a name.
        /// </summary>
        /// <remarks>This class can be used to define and manage menus in an application.  The <see
        /// cref="Name"/> property specifies the name of the menu.</remarks>
        public class Menu
        {
            public string Name { get; set; }
        }

        /// <summary>
        /// A delegate that converts a <see cref="Menu"/> object to its string representation.
        /// </summary>
        /// <remarks>The conversion is performed by accessing the <see cref="Menu.Name"/> property.  If
        /// the <paramref name="Menu"/> object is <see langword="null"/>, the delegate returns <see
        /// langword="null"/>.</remarks>
        Func<Menu, string> converter = p => p?.Name;
    }
}
