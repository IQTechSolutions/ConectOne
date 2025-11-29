using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SchoolsModule.Application.ViewModels;
using SchoolsModule.Domain.Interfaces;

namespace SchoolsModule.Blazor.Pages.AttendanceGroups;

/// <summary>
/// Provides functionality for creating and managing attendance groups within the application.
/// </summary>
/// <remarks>This class is designed to handle the creation of attendance groups, including user interactions  such
/// as form submission and navigation. It relies on dependency injection to access services  for HTTP communication,
/// dialog management, notifications, and navigation.</remarks>
public partial class Creator
{
    private readonly AttendanceGroupViewModel _group = new();
    private readonly List<BreadcrumbItem> _items = new()
    {
        new("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
        new("Attendance Groups", href: "/attendancegroups", icon: Icons.Material.Filled.People),
        new("Create Attendance Group", href: null, disabled: true, icon: Icons.Material.Filled.Edit)
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
    [Inject] public ISnackbar SnackBar { get; set; } = null!;

    /// <summary>
    /// Gets or sets the <see cref="NavigationManager"/> instance used for handling navigation and URI management in a
    /// Blazor application.
    /// </summary>
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>
    /// Creates a new attendance group asynchronously and processes the result.
    /// </summary>
    /// <remarks>This method sends a request to create an attendance group using the current group data.  If
    /// the operation is successful, a success message is displayed, and the user is redirected  to the attendance
    /// groups page. If the operation fails, the response is processed to display  an appropriate error
    /// message.</remarks>
    /// <returns></returns>
    private async Task CreateAsync()
    {
        var result = await AttendanceGroupService.CreateAsync(_group.ToDto());
        result.ProcessResponseForDisplay(SnackBar, () =>
        {
            SnackBar.AddSuccess($"{_group.Name} was created successfully");
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
}
