using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces;

namespace SchoolsModule.Blazor.Pages.SchoolClasses;

/// <summary>
/// Represents the attendance management component for a specific school class.
/// </summary>
/// <remarks>This component is responsible for loading and displaying attendance-related data for a given school
/// class. The class ID is provided as a parameter, and the component fetches the corresponding data during
/// initialization.</remarks>
public partial class Attendance
{
    private SchoolClassDto? _class;
    private bool _loaded;

    /// <summary>
    /// Gets or sets the HTTP provider used to perform HTTP operations.
    /// </summary>
    [Inject] public ISchoolClassService SchoolClassService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the injected service for displaying snack bar notifications.
    /// </summary>
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
        var result = await SchoolClassService.SchoolClassAsync(SchoolClassId);
        result.ProcessResponseForDisplay(SnackBar, () =>
        {
            _class = result.Data;
        });
        _loaded = true;
        await base.OnInitializedAsync();
    }
}
