using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using ProductsModule.Domain.Constants;
using ProductsModule.Domain.DataTransferObjects;
using ProductsModule.Domain.Interfaces;
using ProductsModule.Domain.RequestFeatures;

namespace ProductsModule.Blazor.Components.Products
{
    public partial class ProductsTable
    {
        private MudTable<ProductDto> _table = null!;
        private IEnumerable<ProductDto> _products = null!;
        private readonly ProductsParameters _args = new();
        private int _totalItems;
        private int _currentPage;
        private bool _canCreate;
        private bool _canEdit;
        private bool _canDelete;
        private bool _loaded;
        private bool _dense;
        private bool _striped = true;
        private bool _bordered;

        /// <summary>
        /// Gets or sets the task that represents the asynchronous operation to retrieve the current authentication state.
        /// </summary>
        /// <remarks>This property is typically provided by the Blazor framework and is used to access the
        /// authentication state  within a component. The task should be awaited to retrieve the <see
        /// cref="AuthenticationState"/> instance.</remarks>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to handle authorization operations.
        /// </summary>
        /// <remarks>This property is typically injected by the dependency injection framework and should not be
        /// set manually in most cases. Ensure that a valid implementation of <see cref="IAuthorizationService"/> is
        /// configured in the dependency injection container.</remarks>
        [Inject] public IAuthorizationService AuthorizationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing product-related operations.
        /// </summary>
        [Inject] public IProductService ProductService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="ISnackbar"/> service used to display snack bar notifications.
        /// </summary>
        /// <remarks>This property is typically injected by the dependency injection framework. Ensure that a
        /// valid  implementation of <see cref="ISnackbar"/> is configured in the service container before using this
        /// property.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the NavigationManager used to manage and interact with URI navigation in the application.
        /// </summary>
        /// <remarks>The NavigationManager provides methods and properties for programmatically navigating
        /// to different URIs and for responding to navigation events. This property is typically set by the Blazor
        /// framework via dependency injection.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display dialogs in the application.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier of the shop owner associated with this component.
        /// </summary>
        [Parameter] public string? ShopOwnerId { get; set; }

        /// <summary>
        /// Reloads the server-side data for the table based on the specified state.
        /// </summary>
        /// <remarks>This method retrieves the data for the table asynchronously, applying the specified state
        /// (e.g., page number, page size, and sorting) to determine the subset of data to load.</remarks>
        /// <param name="state">The current state of the table, including pagination and sorting information.</param>
        /// <param name="token">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A <see cref="TableData{T}"/> containing the total number of items and the current page of data as a collection
        /// of <see cref="ProductViewModel"/> objects.</returns>
        private async Task<TableData<ProductDto>> ServerReload(TableState state, CancellationToken token)
        {
            await LoadData(state.Page, state.PageSize, state);
            return new TableData<ProductDto> { TotalItems = _totalItems, Items = _products };
        }

        /// <summary>
        /// Asynchronously loads a paginated list of products based on the specified page number, page size, and table
        /// state.
        /// </summary>
        /// <remarks>This method retrieves a paginated and optionally sorted list of products from the data
        /// provider. The sorting is determined by the <paramref name="state"/> parameter, which specifies the sort label
        /// and direction. If the request is successful, the method updates the total item count, current page, and product
        /// list. Any error messages from the request are added to the snack bar for user feedback.</remarks>
        /// <param name="pageNumber">The zero-based index of the page to load.</param>
        /// <param name="pageSize">The number of items to include in each page. Must be greater than zero.</param>
        /// <param name="state">The current state of the table, including sorting information.</param>
        /// <returns></returns>
        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {
            string sortOrder = string.Empty;
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                var sortDirection = "";
                if (state.SortDirection == SortDirection.Ascending)
                    sortDirection = "asc";
                if (state.SortDirection == SortDirection.Descending)
                    sortDirection = "desc";

                sortOrder = state.SortDirection != SortDirection.None ? $"{state.SortLabel} {sortDirection}" : string.Empty;
            }

            _args.OrderBy = sortOrder;
            _args.PageSize = pageSize;
            _args.PageNr = pageNumber + 1;

            var request = await ProductService.PagedPricedProductsAsync(_args);
            if (request.Succeeded)
            {
                _totalItems = request.TotalCount;
                _currentPage = request.CurrentPage;
                _products = request.Data;
            }
            SnackBar.AddErrors(request.Messages);
        }

        /// <summary>
        /// Resets the query parameters to their default values and reloads the server data asynchronously.
        /// </summary>
        /// <remarks>This method clears any sorting applied by setting the <c>OrderBy</c> parameter to an empty
        /// string, resets the page size to 10, and sets the page number to 1 before reloading the data from the
        /// server.</remarks>
        /// <returns>A task that represents the asynchronous operation of reloading the server data.</returns>
        private async Task Reload()
        {
            _args.OrderBy = "";
            _args.PageSize = 10;
            _args.PageNr = 1;
            await _table.ReloadServerData();
        }

        /// <summary>
        /// Deletes a product with the specified identifier after user confirmation.
        /// </summary>
        /// <remarks>This method displays a confirmation dialog to the user before proceeding with the deletion. 
        /// If the user confirms, the product is deleted from the data source, and the associated table is
        /// reloaded.</remarks>
        /// <param name="productId">The unique identifier of the product to be deleted. Cannot be null or empty.</param>
        /// <returns></returns>
        private async Task DeleteProduct(string productId)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this product from this application?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                await ProductService.DeleteAsync(productId);
                await _table.ReloadServerData();
            }
        }

        /// <summary>
        /// Asynchronously initializes the component and determines the user's authorization for specific actions.
        /// </summary>
        /// <remarks>This method checks the user's authorization for create, edit, and delete permissions using
        /// the provided  <see cref="AuthorizationService"/> and updates the component's state accordingly. It also ensures
        /// that  the component is marked as loaded after initialization.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;

            _args.ShopOwnerId = ShopOwnerId;

            _canCreate = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.Products.Create)).Succeeded;
            _canEdit = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.Products.Edit)).Succeeded;
            _canDelete = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.Products.Delete)).Succeeded;

            _loaded = true;

            await base.OnInitializedAsync();
        }
    }
}
