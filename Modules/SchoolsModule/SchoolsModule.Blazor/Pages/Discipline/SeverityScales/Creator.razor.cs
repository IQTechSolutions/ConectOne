using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SchoolsModule.Application.ViewModels;
using SchoolsModule.Domain.Interfaces.Discipline;

namespace SchoolsModule.Blazor.Pages.Discipline.SeverityScales;

/// <summary>
/// Represents a component for creating new severity scales within the application.
/// </summary>
/// <remarks>This component provides functionality for creating a new severity scale, including navigation and
/// user feedback. It is designed to be used within the context of the application's discipline management
/// system.</remarks>
[Authorize]
public partial class Creator
{
    private readonly SeverityScaleViewModel _scale = new();
    private readonly List<BreadcrumbItem> _items = new()
    {
        new("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
        new("Scales", href: "/discipline/scales", icon: Icons.Material.Filled.Edit),
        new("Create", href: null, disabled: true, icon: Icons.Material.Filled.Add)
    };

    /// <summary>
    /// Gets or sets the HTTP provider used for making HTTP requests.
    /// </summary>
    [Inject] public IDisciplinaryActionService DisciplinaryActionService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service used to display snack bar notifications.
    /// </summary>
    /// <remarks>This property is typically injected by the dependency injection framework. Ensure that a
    /// valid  implementation of <see cref="ISnackbar"/> is provided before using this property.</remarks>
    [Inject] public ISnackbar SnackBar { get; set; } = null!;

    /// <summary>
    /// Gets or sets the <see cref="NavigationManager"/> used for handling navigation and URI management in a Blazor
    /// application.
    /// </summary>
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>
    /// Creates a new severity scale asynchronously and processes the server response.
    /// </summary>
    /// <remarks>This method sends a request to create a new severity scale using the provided data. Upon a
    /// successful response, a success message is displayed, and the user is redirected to the scales overview
    /// page.</remarks>
    /// <returns></returns>
    private async Task CreateAsync()
    {
        var response = await DisciplinaryActionService.CreateSeverityScaleAsync(_scale.ToDto());
        response.ProcessResponseForDisplay(SnackBar, () =>
        {
            SnackBar.AddSuccess("Scale created");
            NavigationManager.NavigateTo("/discipline/scales", true);
        });
    }

    /// <summary>
    /// Cancels the current operation and navigates to the discipline scales page.
    /// </summary>
    /// <remarks>This method performs a forced navigation to the "/discipline/scales" route, bypassing any
    /// client-side caching.</remarks>
    private void Cancel() => NavigationManager.NavigateTo("/discipline/scales", true);
}
