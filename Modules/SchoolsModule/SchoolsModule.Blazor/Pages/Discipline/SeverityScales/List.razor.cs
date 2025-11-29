using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using SchoolsModule.Application.ViewModels;
using SchoolsModule.Domain.Constants;
using SchoolsModule.Domain.Interfaces.Discipline;

namespace SchoolsModule.Blazor.Pages.Discipline.SeverityScales;

/// <summary>
/// Represents a component that displays and manages a list of severity scales within a table.
/// </summary>
/// <remarks>This component is designed to interact with a backend service to retrieve and display data in a
/// paginated table.  It also provides breadcrumb navigation for user context and integrates with a snackbar for
/// displaying notifications.</remarks>
[Authorize]
public partial class List
{
    private bool _canCreate;
    private bool _canEdit;
    private bool _canDelete;
    private MudTable<SeverityScaleViewModel> _table = null!;
    private readonly List<BreadcrumbItem> _items = new()
    {
        new("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
        new("Scales", href: null, disabled: true, icon: Icons.Material.Filled.Edit)
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
    [Inject] public IDisciplinaryActionService Provider2 { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service used to display snack bar notifications.
    /// </summary>
    /// <remarks>This property is typically injected by the dependency injection framework. Ensure that the
    /// service is properly configured before use.</remarks>
    [Inject] public ISnackbar SnackBar { get; set; } = null!;

    /// <summary>
    /// Reloads the server data and returns a table of severity scale view models based on the provided table state.
    /// </summary>
    /// <remarks>This method retrieves severity scale data from the server, maps it to view models, and
    /// returns it in a format suitable for table display. If the server request fails, error messages are added to the
    /// snack bar.</remarks>
    /// <param name="state">The current state of the table, including pagination and sorting information.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="TableData{T}"/> containing the list of severity scale view models and the total item count.</returns>
    private async Task<TableData<SeverityScaleViewModel>> ServerReload(TableState state, CancellationToken cancellationToken)
    {
        var authState = await AuthenticationStateTask;

        _canCreate = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.SystemDataPermissions.Create)).Succeeded;
        _canEdit = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.SystemDataPermissions.Edit)).Succeeded;
        _canDelete = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.SystemDataPermissions.Delete)).Succeeded;
        
        var request = await Provider2.AllSeverityScalesAsync();
        List<SeverityScaleViewModel> data = new();
        if (request.Succeeded)
            data = request.Data.Select(s => new SeverityScaleViewModel(s)).ToList();
        SnackBar.AddErrors(request.Messages);
        return new TableData<SeverityScaleViewModel> { Items = data, TotalItems = data.Count };
    }
}
