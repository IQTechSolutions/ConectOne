using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SchoolsModule.Application.ViewModels;
using SchoolsModule.Domain.Interfaces.Discipline;

namespace SchoolsModule.Blazor.Pages.Discipline.Actions;

/// <summary>
/// Represents a Blazor component for editing disciplinary actions.
/// </summary>
/// <remarks>The <see cref="Editor"/> component provides functionality for managing disciplinary actions, 
/// including loading severity scales, retrieving action details, and updating actions.  It relies on dependency
/// injection for HTTP requests, navigation, and user notifications.</remarks>
[Authorize]
public partial class Editor
{
    private DisciplinaryActionViewModel _action = new();
    private List<SeverityScaleViewModel> _scales = new();
    private readonly List<BreadcrumbItem> _items = new()
    {
        new("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
        new("Actions", href: "/discipline/actions", icon: Icons.Material.Filled.Edit),
        new("Update", href: null, disabled: true, icon: Icons.Material.Filled.Edit)
    };

    /// <summary>
    /// Gets or sets the service used to manage disciplinary actions within the application.
    /// </summary>
    [Inject] public IDisciplinaryActionService DisciplinaryActionService { get; set; } = null!;

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
    /// <remarks>This property is typically injected by the Blazor framework and should not be set manually in
    /// most scenarios.</remarks>
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>
    /// Gets or sets the unique identifier for the action.
    /// </summary>
    [Parameter] public string ActionId { get; set; } = null!;

    /// <summary>
    /// Asynchronously initializes the component by loading severity scales and disciplinary action data.
    /// </summary>
    /// <remarks>This method retrieves severity scales and disciplinary action details from the data provider
    /// and initializes the corresponding view models. Any error messages encountered during the data retrieval process
    /// are added to the snack bar for user notification.</remarks>
    /// <returns></returns>
    protected override async Task OnInitializedAsync()
    {
        var scales = await DisciplinaryActionService.AllSeverityScalesAsync();
        if (scales.Succeeded)
            _scales = scales.Data.Select(s => new SeverityScaleViewModel(s)).ToList();
        SnackBar.AddErrors(scales.Messages);

        var result = await DisciplinaryActionService.ActionAsync(ActionId);
        if (result.Succeeded && result.Data != null)
            _action = new DisciplinaryActionViewModel(result.Data);
        SnackBar.AddErrors(result.Messages);
        await base.OnInitializedAsync();
    }

    /// <summary>
    /// Updates the disciplinary action by sending the current action data to the server.
    /// </summary>
    /// <remarks>This method sends a POST request to the server to update the disciplinary action.  If the
    /// update is successful, a success message is displayed, and the user is redirected  to the disciplinary actions
    /// page. If the update fails, the response is processed to  display an appropriate error message.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task UpdateAsync()
    {
        var response = await DisciplinaryActionService.UpdateActionAsync(_action.ToDto());
        response.ProcessResponseForDisplay(SnackBar, () =>
        {
            SnackBar.AddSuccess("Action updated");
            NavigationManager.NavigateTo("/discipline/actions", true);
        });
    }

    /// <summary>
    /// Cancels the current operation and navigates to the "Discipline Actions" page.
    /// </summary>
    /// <remarks>This method performs a forced navigation to the specified page, bypassing any client-side
    /// caching.</remarks>
    private void Cancel() => NavigationManager.NavigateTo("/discipline/actions", true);
}
