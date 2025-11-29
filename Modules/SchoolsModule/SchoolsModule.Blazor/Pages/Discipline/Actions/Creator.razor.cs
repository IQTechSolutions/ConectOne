using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SchoolsModule.Application.ViewModels;
using SchoolsModule.Domain.Interfaces.Discipline;

namespace SchoolsModule.Blazor.Pages.Discipline.Actions;

/// <summary>
/// Represents a component for creating disciplinary actions within the application.
/// </summary>
/// <remarks>This component provides functionality to create new disciplinary actions, including initializing
/// severity scales and handling user interactions for creating or canceling actions. It interacts with backend services
/// to retrieve data and submit new actions, and provides feedback to the user via notifications.</remarks>
[Authorize]
public partial class Creator
{
    private readonly DisciplinaryActionViewModel _action = new();
    private List<SeverityScaleViewModel> _scales = new();
    private readonly List<BreadcrumbItem> _items = new()
    {
        new("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
        new("Actions", href: "/discipline/actions", icon: Icons.Material.Filled.Edit),
        new("Create", href: null, disabled: true, icon: Icons.Material.Filled.Add)
    };

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
    /// Gets or sets the <see cref="NavigationManager"/> instance used for handling navigation and URI management in a
    /// Blazor application.
    /// </summary>
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;
    
    /// <summary>
    /// Asynchronously initializes the component and retrieves the severity scales data.
    /// </summary>
    /// <remarks>This method fetches a collection of severity scales from the specified data provider.  If the
    /// operation succeeds, the retrieved data is transformed into view models and stored locally.  Any error messages
    /// from the operation are displayed using the snack bar.</remarks>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        var result = await DisciplinaryActionService.AllSeverityScalesAsync();
        if (result.Succeeded)
            _scales = result.Data.Select(s => new SeverityScaleViewModel(s)).ToList();
        SnackBar.AddErrors(result.Messages);
        await base.OnInitializedAsync();
    }

    /// <summary>
    /// Creates a new disciplinary action asynchronously and processes the response.
    /// </summary>
    /// <remarks>This method sends a request to create a disciplinary action using the provided data. Upon a
    /// successful response, it displays a success message and navigates to the disciplinary actions page.</remarks>
    /// <returns></returns>
    private async Task CreateAsync()
    {
        var response = await DisciplinaryActionService.CreateActionAsync(_action.ToDto());
        response.ProcessResponseForDisplay(SnackBar, () =>
        {
            SnackBar.AddSuccess("Action created");
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
