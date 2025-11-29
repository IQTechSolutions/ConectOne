using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using GroupingModule.Domain.DataTransferObjects;
using GroupingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using ProductsModule.Domain.Constants;
using ProductsModule.Domain.Interfaces;

namespace ProductsModule.Blazor.Components.ProductCategories;

/// <summary>
/// Represents a table component for managing and displaying product categories.
/// </summary>
/// <remarks>This component provides functionality for searching, navigating, and deleting product categories, as
/// well as handling pagination and sorting. It integrates with various services such as dialog, snackbar notifications,
/// and HTTP providers to perform these operations.</remarks>
public partial class ProductCategoriesTable
{
    private bool _dense = true;
    private bool _striped = true;
    private bool _bordered = false;

    private bool _canSearch = false;
    private bool _canCreate = false;
    private bool _canEdit = false;
    private bool _canDelete = false;

    private MudTable<CategoryDto> _table = null!;
    private readonly CategoryPageParameters _args = new();

    /// <summary>
    /// The cascading authentication state, used to determine 
    /// the current user's permissions (e.g., create/edit/delete learners).
    /// </summary>
    [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

    /// <summary>
    /// Injects the authorization service for checking user permissions.
    /// </summary>
    [Inject] public IAuthorizationService AuthorizationService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service used to display dialogs in the application.
    /// </summary>
    [Inject] public IDialogService DialogService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service used to display snack bar notifications in the application.
    /// </summary>
    /// <remarks>This property is typically injected by the dependency injection framework and should not be
    /// set manually in most cases.</remarks>
    [Inject] public ISnackbar SnackBar { get; set; } = null!;

    /// <summary>
    /// Gets or sets the <see cref="NavigationManager"/> instance used for handling navigation and URI management in the
    /// application.
    /// </summary>
    /// <remarks>This property is typically injected by the framework and should not be manually set in most
    /// scenarios.</remarks>
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    [Inject] public IProductCategoryService ProductCategoryService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the URL used to create a new product category.
    /// </summary>
    [Parameter] public string CreateUrl { get; set; } = $"/products/categories/create";

    /// <summary>
    /// Gets or sets the identifier of the parent entity.
    /// </summary>
    [Parameter] public string? ParentId { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered within the toolbar buttons area.
    /// </summary>
    /// <remarks>This property allows you to define custom content, such as buttons or other UI elements,  to
    /// be displayed in the toolbar. The content is specified as a <see cref="RenderFragment"/>.</remarks>
    [Parameter] public RenderFragment ToolbarButtons { get; set; } = null!;

    /// <summary>
    /// Initiates a search operation using the specified search text.
    /// </summary>
    /// <remarks>This method updates the search text and triggers a reload of the server data to reflect the
    /// new search criteria. Ensure that the <paramref name="text"/> parameter is valid before calling this
    /// method.</remarks>
    /// <param name="text">The search text to be used for filtering data. Cannot be null or empty.</param>
    /// <returns>A task that represents the asynchronous operation of reloading server data.</returns>
    private async Task OnSearch(string text)
    {
        _args.SearchText = text;
        await _table.ReloadServerData();
    }

    /// <summary>
    /// Navigates to the specified product category update page.
    /// </summary>
    /// <remarks>The method constructs a URL using the provided <paramref name="categoryId"/> and navigates to
    /// the  corresponding category update page. The navigation forces a reload of the page.</remarks>
    /// <param name="categoryId">The unique identifier of the category to navigate to. This value cannot be null or empty.</param>
    private void NavigateToCategory(string categoryId)
    {
        NavigationManager.NavigateTo($"/products/categories/update/{categoryId}", true);
    }

    /// <summary>
    /// Deletes a product category after user confirmation.
    /// </summary>
    /// <remarks>This method displays a confirmation dialog to the user before proceeding with the deletion. 
    /// If the user confirms, the category is deleted from the application, and the server data is reloaded.</remarks>
    /// <param name="categoryId">The unique identifier of the category to be deleted. Cannot be null or empty.</param>
    /// <returns></returns>
    private async Task DeleteCategory(string categoryId)
    {
        var parameters = new DialogParameters<ConformtaionModal>
        {
            { x => x.ContentText, "Are you sure you want to remove this product category from this application?" },
            { x => x.ButtonText, "Yes" },
            { x => x.Color, Color.Success }
        };

        var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
        var result = await dialog.Result;

        if (!result!.Canceled)
        {
            var deleteResult = await ProductCategoryService.DeleteCategoryAsync(categoryId);
            deleteResult.ProcessResponseForDisplay(SnackBar, async () => await _table.ReloadServerData());
        }
    }

    /// <summary>
    /// Configures the pagination and sorting parameters for the current page based on the provided table state.
    /// </summary>
    /// <remarks>This method updates the internal arguments used for data retrieval, such as the page number,
    /// page size, parent identifier, and sorting order. The sorting order is determined by the <see
    /// cref="SortDirection"/> and the associated sort label.</remarks>
    /// <param name="state">The current state of the table, including the page number, page size, sort label, and sort direction.</param>
    private void SetPageParameters(TableState state)
    {
        _args.PageNr = state.Page + 1;
        _args.PageSize = state.PageSize;
        _args.ParentId = ParentId;
        _args.OrderBy = state.SortDirection switch
        {
            SortDirection.Ascending => $"{state.SortLabel} desc",
            SortDirection.Descending => $"{state.SortLabel} asc",
            _ => _args.OrderBy
        };
    }

    /// <summary>
    /// Retrieves a paginated list of categories based on the specified table state.
    /// </summary>
    /// <remarks>If the operation fails, error messages are added to the application's notification
    /// system.</remarks>
    /// <param name="state">The state of the table, including pagination and sorting parameters.</param>
    /// <param name="token">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A <see cref="TableData{T}"/> object containing the total number of items and the list of categories for the
    /// current page.</returns>
    public async Task<TableData<CategoryDto>> GetCategoriesAsync(TableState state, CancellationToken token)
    {
        SetPageParameters(state);
        var pagingResponse = await ProductCategoryService.PagedCategoriesAsync(_args);
        if (!pagingResponse.Succeeded)
        {
            SnackBar.AddErrors(pagingResponse.Messages);
        }
        return new TableData<CategoryDto>() { TotalItems = pagingResponse.TotalCount, Items = pagingResponse.Data };
    }

    /// <summary>
    /// Asynchronously initializes the component and sets up the necessary state for authorization and data handling.
    /// </summary>
    /// <remarks>This method retrieves the current authentication state and evaluates the user's authorization
    /// for specific permissions. It also initializes query arguments with default values. Override this method to
    /// include additional initialization logic.</remarks>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateTask;

        _canSearch = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.ProductCategories.Search)).Succeeded;
        _canCreate = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.ProductCategories.Create)).Succeeded;
        _canEdit = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.ProductCategories.Edit)).Succeeded;
        _canDelete = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.ProductCategories.Delete)).Succeeded;

        _args.ParentId = ParentId;
        _args.OrderBy = "Name asc";

        await base.OnInitializedAsync();
    }
}
