using BusinessModule.Domain.Constants;
using BusinessModule.Domain.DataTransferObjects;
using BusinessModule.Domain.Interfaces;
using BusinessModule.Domain.RequestFeatures;
using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace BusinessModule.Blazor.Pages.BusinessListings;

/// <summary>
/// Represents a component that displays and manages a list of business listings with server-side data loading, sorting,
/// and pagination.
/// </summary>
/// <remarks>This component interacts with a backend service to fetch and display business listings in a table
/// format.  It supports server-side data operations such as sorting and pagination, and provides functionality for
/// deleting listings. The component also integrates with various services for HTTP requests, user notifications, and
/// navigation.</remarks>
public partial class List
{
    private IEnumerable<BusinessListingDto> _listings = new List<BusinessListingDto>();
    private MudTable<BusinessListingDto> _table = null!;
    private int _totalItems;
    private bool _dense;
    private bool _striped = true;
    private bool _bordered;
    private bool _loaded;
    private bool _canCreate;
    private bool _canEdit;
    private bool _canDelete;
    private readonly List<BreadcrumbItem> _items =
    [
        new("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
        new("Listings", href: null, disabled: true, icon: Icons.Material.Filled.List)
    ];

    /// <summary>
    /// Gets or sets the task that represents the asynchronous operation to retrieve the current authentication state.
    /// </summary>
    [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

    /// <summary>
    /// Injects the authorization service for checking user permissions.
    /// </summary>
    [Inject] public IAuthorizationService AuthorizationService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the HTTP provider used to make HTTP requests.
    /// </summary>
    [Inject] public IBusinessDirectoryQueryService Provider { get; set; } = null!;

    /// <summary>
    /// Gets or sets the application configuration settings.
    /// </summary>
    [Inject] public IConfiguration Configuration { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service responsible for executing commands related to the business directory.
    /// </summary>
    [Inject] public IBusinessDirectoryCommandService BusinessDirectoryCommandService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service used to display dialogs in the application.
    /// </summary>
    [Inject] public IDialogService DialogService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service used to display snack bar notifications in the application.
    /// </summary>
    /// <remarks>This property is typically injected by the dependency injection framework. Ensure that a
    /// valid  implementation of <see cref="ISnackbar"/> is provided before using this property.</remarks>
    [Inject] public ISnackbar SnackBar { get; set; } = null!;

    /// <summary>
    /// Gets or sets the <see cref="NavigationManager"/> instance used for handling navigation and URI management in the
    /// application.
    /// </summary>
    /// <remarks>This property is typically injected by the Blazor framework and should not be manually set in
    /// most scenarios.</remarks>
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>
    /// Invoked when the component is initialized. This method is called once during the component's lifecycle.
    /// </summary>
    /// <remarks>Use this method to perform any asynchronous initialization logic required for the component.
    /// The base implementation should be called to ensure proper initialization behavior.</remarks>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateTask;

        _canCreate = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.BusinessListingPermissions.Create)).Succeeded;
        _canEdit = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.BusinessListingPermissions.Edit)).Succeeded;
        _canDelete = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.BusinessListingPermissions.Delete)).Succeeded;

        _loaded = true;
        await base.OnInitializedAsync();
    }

    /// <summary>
    /// Retrieves and reloads a paginated, sorted list of active business listings from the server.
    /// </summary>
    /// <remarks>The method fetches active business listings from the server, applies sorting and pagination
    /// based on the provided <paramref name="state"/>, and updates the internal state of the component. If the server
    /// request fails, error messages are added to the snack bar.</remarks>
    /// <param name="state">The current table state, including pagination and sorting information.</param>
    /// <param name="token">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A <see cref="TableData{T}"/> object containing the total number of items and the current page of business
    /// listings.</returns>
    private async Task<TableData<BusinessListingDto>> ServerReload(TableState state, CancellationToken token)
    {
        var result = await Provider.PagedListingsAsync(new BusinessListingPageParameters() { PageSize = state.PageSize, PageNr = state.Page});
        if (result.Succeeded)
        {
            var data = result.Data.ToList();
            var sorted = data.AsQueryable();
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                sorted = state.SortDirection switch
                {
                    SortDirection.Ascending => sorted.OrderBy<BusinessListingDto, object>(e => state.SortLabel == nameof(BusinessListingDto.Heading) ? e.Heading : state.SortLabel == nameof(BusinessListingDto.Tier) ? e.Tier : e.Status),
                    SortDirection.Descending => sorted.OrderByDescending<BusinessListingDto, object>(e => state.SortLabel == nameof(BusinessListingDto.Heading) ? e.Heading : state.SortLabel == nameof(BusinessListingDto.Tier) ? e.Tier : e.Status),
                    _ => sorted
                };
            }
            _listings = sorted.Skip(state.Page * state.PageSize).Take(state.PageSize).ToList();
        }
        else
        {
            SnackBar.AddErrors(result.Messages);
        }
            
        return new TableData<BusinessListingDto> { TotalItems = result.TotalCount, Items = _listings };
    }

    /// <summary>
    /// Reloads the server data for the table asynchronously.
    /// </summary>
    /// <remarks>This method refreshes the data by invoking the server-side reload operation.  It is intended
    /// to ensure that the table reflects the most up-to-date state.</remarks>
    /// <returns></returns>
    private async Task Reload()
    {
        await _table.ReloadServerData();
    }

    /// <summary>
    /// Deletes a listing with the specified identifier after user confirmation.
    /// </summary>
    /// <remarks>This method prompts the user for confirmation before proceeding with the deletion.  If the
    /// user confirms, the listing is removed from the "businessdirectory" data source,  and the associated table data
    /// is reloaded to reflect the changes.</remarks>
    /// <param name="listingId">The unique identifier of the listing to be deleted. Cannot be null or empty.</param>
    /// <returns></returns>
    private async Task DeleteListing(string listingId)
    {
        if (await DialogService.ConfirmAction("Are you sure you want to remove this listing from this application?"))
        {
            await BusinessDirectoryCommandService.RemoveAsync(listingId);
            await _table.ReloadServerData();
        }
    }
}

