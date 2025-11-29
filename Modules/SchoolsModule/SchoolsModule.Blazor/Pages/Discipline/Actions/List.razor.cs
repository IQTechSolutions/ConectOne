using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using SchoolsModule.Application.ViewModels;
using SchoolsModule.Domain.Constants;
using SchoolsModule.Domain.Interfaces.Discipline;

namespace SchoolsModule.Blazor.Pages.Discipline.Actions;

/// <summary>
/// Represents a Blazor component for displaying and managing a list of disciplinary actions.
/// </summary>
/// <remarks>This component provides functionality for loading, displaying, and interacting with a table of
/// disciplinary actions. It supports server-side data loading and integrates with external services for fetching data
/// and displaying notifications.</remarks>
[Authorize]
public partial class List
{
    private bool _dense;
    private bool _striped = true;
    private bool _bordered;
    private bool _canCreate;
    private bool _canEdit;
    private bool _canDelete;
    private Dictionary<string, string> _scaleLookup = new();
    private MudTable<DisciplinaryActionViewModel> _table = null!;
    private readonly List<BreadcrumbItem> _items = new()
    {
        new("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
        new("Actions", href: null, disabled: true, icon: Icons.Material.Filled.Edit)
    };

    /// <summary>
    /// Gets or sets the task that provides the current authentication state.
    /// </summary>
    /// <remarks>This property is typically used in Blazor components to access the authentication
    /// state of the current user. The task should not be null and is expected to complete with a valid <see
    /// cref="AuthenticationState"/>.</remarks>
    [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

    /// <summary>
    /// Injects the authorization service for checking user permissions.
    /// </summary>
    [Inject] public IAuthorizationService AuthorizationService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the HTTP provider used for making HTTP requests.
    /// </summary>
    [Inject] public IDisciplinaryActionService DisciplinaryActionService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service used to display snack bar notifications.
    /// </summary>
    /// <remarks>This property is typically injected by the dependency injection framework. Ensure that the
    /// service is properly configured before use.</remarks>
    [Inject] public ISnackbar SnackBar { get; set; } = null!;

    /// <summary>
    /// Reloads the server-side data for the disciplinary actions table based on the specified table state.
    /// </summary>
    /// <remarks>This method retrieves the disciplinary actions data from the server, maps it to the view
    /// model, and updates the table data. If the server request fails, error messages are added to the snack
    /// bar.</remarks>
    /// <param name="state">The current state of the table, including sorting and pagination information.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="TableData{T}"/> object
    /// with the updated list of disciplinary actions and the total number of items.</returns>
    private async Task<TableData<DisciplinaryActionViewModel>> ServerReload(TableState state, CancellationToken cancellationToken)
    {
        var request = await DisciplinaryActionService.AllActionsAsync();
        List<DisciplinaryActionViewModel> data = new();
        if (request.Succeeded)
            data = request.Data.Select(a => new DisciplinaryActionViewModel(a)).ToList();
        SnackBar.AddErrors(request.Messages);
        return new TableData<DisciplinaryActionViewModel> { Items = data, TotalItems = data.Count };
    }

    /// <summary>
    /// Asynchronously initializes the component and retrieves severity scale data.
    /// </summary>
    /// <remarks>This method fetches severity scale data from the specified provider and populates a lookup
    /// dictionary  with the retrieved data. If the operation fails, error messages are displayed using the snack
    /// bar.</remarks>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateTask;

        _canCreate = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.SystemDataPermissions.Create)).Succeeded;
        _canEdit = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.SystemDataPermissions.Edit)).Succeeded;
        _canDelete = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.SystemDataPermissions.Delete)).Succeeded;

        var scales = await DisciplinaryActionService.AllSeverityScalesAsync();
        if (scales.Succeeded)
            _scaleLookup = scales.Data
                .Select(s => new SeverityScaleViewModel(s))
                .Where(s => s.SeverityScaleId != null)
                .ToDictionary(s => s.SeverityScaleId!, s => s.Name);
        SnackBar.AddErrors(scales.Messages);
        await base.OnInitializedAsync();
    }
}
