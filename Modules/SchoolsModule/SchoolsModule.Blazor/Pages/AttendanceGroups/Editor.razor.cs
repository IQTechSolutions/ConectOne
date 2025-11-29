using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SchoolsModule.Application.ViewModels;
using SchoolsModule.Domain.Interfaces;

namespace SchoolsModule.Blazor.Pages.AttendanceGroups;

/// <summary>
/// Represents a component for editing and updating attendance group information.
/// </summary>
/// <remarks>This component provides functionality to update an existing attendance group by interacting with
/// backend services. It includes dependency-injected services for HTTP communication, dialog management, notifications,
/// and navigation.</remarks>
public partial class Editor
{
    private AttendanceGroupViewModel _group = new();
    private readonly List<BreadcrumbItem> _items = new()
    {
        new("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
        new("Attendance Groups", href: "/attendancegroups", icon: Icons.Material.Filled.People),
        new("Update Attendance Group", href: null, disabled: true, icon: Icons.Material.Filled.Edit)
    };

    /// <summary>
    /// Gets or sets the HTTP provider used for making HTTP requests.
    /// </summary>
    [Inject] public IAttendanceGroupService AttendanceGroupService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service used to display dialogs in the application.
    /// </summary>
    /// <remarks>This property is typically injected by the dependency injection framework. Ensure that a
    /// valid implementation of <see cref="IDialogService"/> is provided before using this property.</remarks>
    [Inject] public IDialogService DialogService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service used to display snack bar notifications.
    /// </summary>
    /// <remarks>This property is typically injected by the dependency injection framework. Ensure that the
    /// service is properly configured before use.</remarks>
    [Inject] public ISnackbar SnackBar { get; set; } = null!;

    /// <summary>
    /// Gets or sets the <see cref="NavigationManager"/> instance used for handling navigation within the application.
    /// </summary>
    /// <remarks>This property is automatically injected by the dependency injection framework in Blazor
    /// applications. Ensure that the property is properly initialized before use.</remarks>
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>
    /// Gets or sets the identifier for the attendance group.
    /// </summary>
    [Parameter] public string AttendanceGroupId { get; set; } = null!;
    
    /// <summary>
    /// Updates the attendance group by sending the updated data to the server asynchronously.
    /// </summary>
    /// <remarks>This method sends a POST request to the server with the updated attendance group data. If the
    /// operation is successful, a success message is displayed, and the user is redirected to the attendance groups
    /// page.</remarks>
    /// <returns></returns>
    private async Task UpdateAsync()
    {
        var result = await AttendanceGroupService.UpdateAsync(_group.ToDto());
        result.ProcessResponseForDisplay(SnackBar, () =>
        {
            SnackBar.AddSuccess("Attendance group was updated successfully");
            NavigationManager.NavigateTo("attendancegroups");
        });
    }

    /// <summary>
    /// Cancels the current operation and navigates to the attendance groups page.
    /// </summary>
    /// <remarks>This method redirects the user to the "/attendancegroups" route. Ensure that the  <see
    /// cref="NavigationManager"/> is properly initialized before calling this method.</remarks>
    private void Cancel()
    {
        NavigationManager.NavigateTo("/attendancegroups");
    }

    /// <summary>
    /// Asynchronously initializes the component and retrieves data for the specified attendance group.
    /// </summary>
    /// <remarks>This method fetches the attendance group data using the provided <see cref="Provider"/>
    /// service  and initializes the local view model if the request is successful. Any error messages from the  request
    /// are displayed using the <see cref="SnackBar"/> service.</remarks>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        var request = await AttendanceGroupService.AttendanceGroupAsync(AttendanceGroupId);
        if (request.Succeeded && request.Data != null)
        {
            _group = new AttendanceGroupViewModel(request.Data);
        }
        SnackBar.AddErrors(request.Messages);
        await base.OnInitializedAsync();
    }
}
