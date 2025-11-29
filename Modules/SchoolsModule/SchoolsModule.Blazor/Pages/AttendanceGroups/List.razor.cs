using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using SchoolsModule.Application.ViewModels;
using SchoolsModule.Domain.Constants;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Blazor.Pages.AttendanceGroups;

/// <summary>
/// Represents a component for managing and displaying a list of attendance groups.
/// </summary>
/// <remarks>This component provides functionality for displaying, creating, editing, and deleting attendance
/// groups. It supports server-side data loading, sorting, and pagination. The component also integrates with
/// authentication and authorization services to enforce permissions for various operations.</remarks>
public partial class List
{
    private int _totalItems;
    private int _currentPage;
    private bool _dense;
    private bool _striped = true;
    private bool _bordered;
    private bool _loaded;
    private bool _canCreate;
    private bool _canEdit;
    private bool _canDelete;

    private IEnumerable<AttendanceGroupViewModel> _groups = null!;
    private MudTable<AttendanceGroupViewModel> _table = null!;
    private readonly AttendanceGroupPageParameters _args = new();
    private readonly List<BreadcrumbItem> _items = new()
    {
        new("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
        new("Attendance Groups", href: null, disabled: true, icon: Icons.Material.Filled.Edit)
    };

    /// <summary>
    /// Gets or sets the task that provides the current authentication state.
    /// </summary>
    /// <remarks>This property is typically used in Blazor components to access the authentication state of
    /// the current user. The task should be awaited to retrieve the <see cref="AuthenticationState"/>
    /// instance.</remarks>
    [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

    /// <summary>
    /// Gets or sets the HTTP provider used for making HTTP requests.
    /// </summary>
    [Inject] public IAttendanceGroupService AttendanceGroupService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service used to display dialogs in the application.
    /// </summary>
    [Inject] public IDialogService DialogService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service used to display snack bar notifications.
    /// </summary>
    [Inject] public ISnackbar SnackBar { get; set; } = null!;

    /// <summary>
    /// Gets or sets the <see cref="NavigationManager"/> used to manage URI navigation and interaction in a Blazor
    /// application.
    /// </summary>
    /// <remarks>This property is typically injected by the Blazor framework and should not be set manually in
    /// most cases.</remarks>
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service responsible for handling authorization operations.
    /// </summary>
    [Inject] public IAuthorizationService AuthorizationService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the identifier of the parent group associated with this entity.
    /// </summary>
    [Parameter] public string ParentGroupId { get; set; }

    /// <summary>
    /// Asynchronously initializes the component, setting up authorization states and other required data.
    /// </summary>
    /// <remarks>This method determines the user's permissions for creating, editing, and deleting system data
    /// based on their authentication state. It also initializes the component's state and invokes the  base class
    /// implementation of <see cref="OnInitializedAsync"/>.</remarks>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateTask;

        _args.ParentGroupId = ParentGroupId;

        _canCreate = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.SystemDataPermissions.Create)).Succeeded;
        _canEdit = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.SystemDataPermissions.Edit)).Succeeded;
        _canDelete = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.SystemDataPermissions.Delete)).Succeeded;
        _loaded = true;
        await base.OnInitializedAsync();
    }

    /// <summary>
    /// Reloads the server-side data for the table based on the specified state.
    /// </summary>
    /// <remarks>This method retrieves the data for the table asynchronously, applying the specified
    /// pagination and sorting settings. The returned <see cref="TableData{T}"/> includes the total number of items and
    /// the items for the current page.</remarks>
    /// <param name="state">The current state of the table, including pagination and sorting information.</param>
    /// <param name="token">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A <see cref="TableData{T}"/> containing the total number of items and the current page of data.</returns>
    private async Task<TableData<AttendanceGroupViewModel>> ServerReload(TableState state, CancellationToken token)
    {
        await LoadData(state.Page, state.PageSize, state);
        return new TableData<AttendanceGroupViewModel> { TotalItems = _totalItems, Items = _groups };
    }

    /// <summary>
    /// Loads a paginated list of attendance groups based on the specified page number, page size, and table state.
    /// </summary>
    /// <remarks>This method retrieves data from the provider using the specified pagination and sorting
    /// parameters. The results are mapped to a collection of <see cref="AttendanceGroupViewModel"/> instances. If the
    /// request fails, error messages are added to the snack bar for user notification.</remarks>
    /// <param name="pageNumber">The zero-based index of the page to load.</param>
    /// <param name="pageSize">The number of items to include in each page.</param>
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

        var request = await AttendanceGroupService.PagedAttendanceGroupsAsync(_args);
        if (request.Succeeded)
        {
            _totalItems = request.TotalCount;
            _currentPage = request.CurrentPage;
            _groups = request.Data.Select(c => new AttendanceGroupViewModel(c));
        }
        SnackBar.AddErrors(request.Messages);
    }

    /// <summary>
    /// Resets the pagination and sorting parameters to their default values and reloads the server-side data.
    /// </summary>
    /// <remarks>This method sets the page number to 1, clears any sorting criteria, and sets the page size to
    /// 10  before triggering a reload of the server-side data. It is typically used to refresh the data table  to its
    /// initial state.</remarks>
    /// <returns>A task that represents the asynchronous operation of reloading the server-side data.</returns>
    private async Task Reload()
    {
        _args.PageNr = 1;
        _args.OrderBy = string.Empty;
        _args.PageSize = 10;
        await _table.ReloadServerData();
    }

    /// <summary>
    /// Deletes an attendance group after user confirmation.
    /// </summary>
    /// <remarks>This method displays a confirmation dialog to the user before proceeding with the deletion.
    /// If the user confirms, the attendance group is removed from the application, and the data table is
    /// reloaded.</remarks>
    /// <param name="attendanceGroupId">The unique identifier of the attendance group to be deleted. Cannot be null or empty.</param>
    /// <returns></returns>
    private async Task DeleteAttendanceGroup(string attendanceGroupId)
    {
        var parameters = new DialogParameters<ConformtaionModal>
        {
            { x => x.ContentText, "Are you sure you want to remove this attendance group from this application?" },
            { x => x.ButtonText, "Yes" },
            { x => x.Color, Color.Success }
        };
        var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
        var result = await dialog.Result;
        if (!result!.Canceled)
        {
            await AttendanceGroupService.DeleteAsync(attendanceGroupId);
            await _table.ReloadServerData();
        }
    }
}
