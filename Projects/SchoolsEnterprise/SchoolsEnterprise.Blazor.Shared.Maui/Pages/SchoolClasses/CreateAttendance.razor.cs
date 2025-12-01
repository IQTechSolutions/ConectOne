using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SchoolsModule.Domain.DataTransferObjects;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.SchoolClasses;

/// <summary>
/// Represents the attendance management component for a specific school class.
/// </summary>
/// <remarks>This component is responsible for loading and displaying attendance-related data for a given school
/// class. The class ID is provided as a parameter, and the component fetches the corresponding data during
/// initialization.</remarks>
public partial class CreateAttendance
{
    private SchoolClassDto? _class;
    private bool _loaded;

    /// <summary>
    /// Gets or sets the HTTP provider used to perform HTTP operations.
    /// </summary>
    [Inject] public IBaseHttpProvider Provider { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service used to display snack bar notifications in the user interface.
    /// </summary>
    /// <remarks>This property is typically provided by dependency injection and allows components to show
    /// transient messages to users. The implementation of ISnackbar determines the appearance and behavior of the
    /// notifications.</remarks>
    [Inject] public ISnackbar SnackBar { get; set; } = null!;

    /// <summary>
    /// Gets or sets the unique identifier for the school class.
    /// </summary>
    [Parameter] public string SchoolClassId { get; set; } = null!;

    /// <summary>
    /// Asynchronously initializes the component and retrieves data for the specified school class.
    /// </summary>
    /// <remarks>This method fetches the details of a school class using the provided <see
    /// cref="SchoolClassId"/>  and processes the response for display. It also ensures that the base class
    /// initialization logic is executed.</remarks>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        var result = await Provider.GetAsync<SchoolClassDto>($"schoolClasses/{SchoolClassId}");
        result.ProcessResponseForDisplay(SnackBar, () =>
        {
            _class = result.Data;
        });
        _loaded = true;
        await base.OnInitializedAsync();
    }
}
