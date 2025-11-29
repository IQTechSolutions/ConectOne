using BusinessModule.Domain.Constants;
using BusinessModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using GroupingModule.Domain.DataTransferObjects;
using GroupingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace BusinessModule.Blazor.Components.BusinessCategories;

/// <summary>
/// Represents a table component for managing and displaying business categories.
/// </summary>
/// <remarks>This component provides functionality for searching, navigating, and managing business categories. It
/// supports server-side data loading, pagination, sorting, and custom toolbar buttons.</remarks>
public partial class BusinessCategoriesTable
{
    private bool _dense = true;
    private bool _striped = true;
    private bool _bordered;

    private bool _canCreate;
    private bool _canEdit;
    private bool _canDelete;
    private bool _canSearch;
    private bool _canCreateSubCategories;

    private MudTable<CategoryDto> _table = null!;
    private readonly CategoryPageParameters _args = new();

    /// <summary>
    /// Gets or sets the task that represents the asynchronous operation to retrieve the current authentication state.
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
    /// Gets or sets the service used to display snack bar notifications.
    /// </summary>
    /// <remarks>This property is typically injected by the dependency injection framework. Ensure that the
    /// service is properly configured before use.</remarks>
    [Inject] public ISnackbar SnackBar { get; set; } = null!;

    /// <summary>
    /// Gets or sets the <see cref="NavigationManager"/> used to manage URI navigation and interaction in a Blazor
    /// application.
    /// </summary>
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service responsible for managing business directory categories.
    /// </summary>
    [Inject] public IBusinessDirectoryCategoryService BusinessDirectoryCategoryService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the URL used to create a new category in the business directory.
    /// </summary>
    [Parameter] public string CreateUrl { get; set; } = "/businessdirectory/categories/create";

    /// <summary>
    /// Gets or sets the identifier of the parent entity.
    /// </summary>
    [Parameter] public string? ParentId { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered as toolbar buttons.
    /// </summary>
    /// <remarks>This property allows you to define custom toolbar buttons by providing a <see
    /// cref="RenderFragment"/>. The specified content will be rendered in the toolbar area of the component.</remarks>
    [Parameter] public RenderFragment ToolbarButtons { get; set; } = null!;

    /// <summary>
    /// Initiates a search operation with the specified search text and reloads the server data.
    /// </summary>
    /// <param name="text">The search text to use for filtering data. Cannot be null or empty.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnSearch(string text)
    {
        _args.SearchText = text;
        await _table.ReloadServerData();
    }

    /// <summary>
    /// Navigates to the specified category's update page in the business directory.
    /// </summary>
    /// <remarks>This method performs a navigation operation to the update page for the given category. The
    /// navigation is forced to reload the page, ensuring the latest data is displayed.</remarks>
    /// <param name="categoryId">The unique identifier of the category to navigate to. Cannot be null or empty.</param>
    private void NavigateToCategory(string categoryId)
    {
        NavigationManager.NavigateTo($"/businessdirectory/categories/update/{categoryId}", true);
    }

    /// <summary>
    /// Deletes a business category from the application after user confirmation.
    /// </summary>
    /// <remarks>This method displays a confirmation dialog to the user before proceeding with the deletion. 
    /// If the user confirms, the category is deleted from the server, and the associated data table is
    /// reloaded.</remarks>
    /// <param name="categoryId">The unique identifier of the category to be deleted. Cannot be null or empty.</param>
    /// <returns></returns>
    private async Task DeleteCategory(string categoryId)
    {
        var parameters = new DialogParameters<ConformtaionModal>
        {
            { x => x.ContentText, "Are you sure you want to remove this business category from this application?" },
            { x => x.ButtonText, "Yes" },
            { x => x.Color, Color.Success }
        };

        var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
        var result = await dialog.Result;

        if (!result!.Canceled)
        {
            var deleteResult = await BusinessDirectoryCategoryService.DeleteCategoryAsync(categoryId);
            deleteResult.ProcessResponseForDisplay(SnackBar, async () => await _table.ReloadServerData());
        }
    }

    /// <summary>
    /// Configures the pagination and sorting parameters for the current page based on the specified table state.
    /// </summary>
    /// <remarks>This method updates the internal arguments used for data retrieval, including the page
    /// number, page size,  parent identifier, and sorting order. The sorting direction is inverted to align with the
    /// expected behavior  for the next page load.</remarks>
    /// <param name="state">The current state of the table, including page number, page size, and sorting information.</param>
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
    /// <remarks>If the operation fails, error messages are added to the application's snack bar for user
    /// notification.</remarks>
    /// <param name="state">The table state that defines pagination and sorting parameters.</param>
    /// <param name="token">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A <see cref="TableData{T}"/> containing the total number of items and the list of categories for the current
    /// page.</returns>
    public async Task<TableData<CategoryDto>> GetCategoriesAsync(TableState state, CancellationToken token)
    {
        SetPageParameters(state);
        var pagingResponse = await BusinessDirectoryCategoryService.PagedCategoriesAsync(_args);
        if (!pagingResponse.Succeeded)
        {
            SnackBar.AddErrors(pagingResponse.Messages);
        }
        return new TableData<CategoryDto>() { TotalItems = pagingResponse.TotalCount, Items = pagingResponse.Data };
    }

    /// <summary>
    /// Asynchronously initializes the component and sets up authorization and query parameters.
    /// </summary>
    /// <remarks>This method retrieves the current authentication state and configures the component's query
    /// parameters and authorization flags based on the user's permissions. It also calls the base implementation of
    /// <see cref="OnInitializedAsync"/>.</remarks>
    /// <returns></returns>
    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateTask;

        _args.ParentId = ParentId;
        _args.OrderBy = "Name asc";

        _canSearch = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.BusinessCategoryPermissions.Search)).Succeeded;
        _canCreate = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.BusinessCategoryPermissions.Create)).Succeeded;
        _canEdit = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.BusinessCategoryPermissions.Edit)).Succeeded;
        _canDelete = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.BusinessCategoryPermissions.Delete)).Succeeded;
        _canCreateSubCategories = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.BusinessCategoryPermissions.SubCategory)).Succeeded;

        await base.OnInitializedAsync();
    }
}
