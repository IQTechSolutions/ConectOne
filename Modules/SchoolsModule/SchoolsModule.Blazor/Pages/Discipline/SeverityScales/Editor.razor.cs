using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SchoolsModule.Application.ViewModels;
using SchoolsModule.Domain.Interfaces.Discipline;

namespace SchoolsModule.Blazor.Pages.Discipline.SeverityScales;

/// <summary>
/// Represents a component for editing and updating severity scales within the application.
/// </summary>
/// <remarks>This component is responsible for retrieving, displaying, and updating severity scale data. It
/// interacts with backend services to fetch and persist data, and provides user feedback through notifications.
/// Navigation and breadcrumb management are also handled to ensure a seamless user experience.</remarks>
[Authorize]
public partial class Editor
{
    private SeverityScaleViewModel _scale = new();
    private readonly List<BreadcrumbItem> _items = new()
    {
        new("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
        new("Scales", href: "/discipline/scales", icon: Icons.Material.Filled.Edit),
        new("Update", href: null, disabled: true, icon: Icons.Material.Filled.Edit)
    };

    /// <summary>
    /// Gets or sets the HTTP provider used for making HTTP requests.
    /// </summary>
    [Inject] public IDisciplinaryActionService DisciplinaryActionService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service used to display snack bar notifications.
    /// </summary>
    [Inject] public ISnackbar SnackBar { get; set; } = null!;

    /// <summary>
    /// Gets or sets the <see cref="NavigationManager"/> used for handling navigation and URI management in a Blazor
    /// application.
    /// </summary>
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>
    /// Gets or sets the identifier for the scale. 
    /// </summary>
    [Parameter] public string ScaleId { get; set; } = null!;

    /// <summary>
    /// Asynchronously initializes the component and retrieves the severity scale data for the specified scale ID.
    /// </summary>
    /// <remarks>This method fetches the severity scale data from the provider using the specified scale ID. 
    /// If the operation succeeds and data is returned, the scale is initialized with the retrieved data.  Any error
    /// messages from the operation are added to the snack bar for user notification.</remarks>
    /// <returns></returns>
    protected override async Task OnInitializedAsync()
    {
        var result = await DisciplinaryActionService.SeverityScaleAsync(ScaleId);
        if (result.Succeeded && result.Data != null)
            _scale = new SeverityScaleViewModel(result.Data);
        SnackBar.AddErrors(result.Messages);
        await base.OnInitializedAsync();
    }

    /// <summary>
    /// Updates the severity scale by sending the current scale data to the server.
    /// </summary>
    /// <remarks>This method sends a POST request to the server to update the severity scale.  If the
    /// operation is successful, a success message is displayed, and the user is redirected to the scales
    /// page.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task UpdateAsync()
    {
        var response = await DisciplinaryActionService.UpdateSeverityScaleAsync(_scale.ToDto());
        response.ProcessResponseForDisplay(SnackBar, () =>
        {
            SnackBar.AddSuccess("Scale updated");
            NavigationManager.NavigateTo("/discipline/scales", true);
        });
    }

    /// <summary>
    /// Cancels the current operation and navigates to the discipline scales page.
    /// </summary>
    /// <remarks>This method performs a navigation to the "/discipline/scales" route and forces a reload of
    /// the page.</remarks>
    private void Cancel() => NavigationManager.NavigateTo("/discipline/scales", true);
}
