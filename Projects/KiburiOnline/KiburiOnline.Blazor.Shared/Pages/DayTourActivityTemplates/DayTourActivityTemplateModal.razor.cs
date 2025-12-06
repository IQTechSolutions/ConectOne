using AccomodationModule.Application.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.DayTourActivityTemplates;

/// <summary>
/// Represents a modal dialog for editing a day tour activity template.
/// </summary>
/// <remarks>This class provides functionality to save changes to the activity template or cancel the
/// operation.</remarks>
public partial class DayTourActivityTemplateModal
{
    /// <summary>
    /// Gets or sets the current instance of the dialog.
    /// </summary>
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

    /// <summary>
    /// Gets or sets the template for a day tour activity.
    /// </summary>
    [Parameter] public DayTourActivityTemplateViewModel Template { get; set; } = new DayTourActivityTemplateViewModel();

    /// <summary>
    /// Asynchronously saves the current template and closes the dialog.
    /// </summary>
    /// <remarks>This method closes the dialog using the specified template. Ensure that the template is
    /// correctly set before calling this method.</remarks>
    private void SaveAsync()
    {
        MudDialog.Close(Template);
    }

    /// <summary>
    /// Cancels the current dialog operation.
    /// </summary>
    /// <remarks>This method terminates the dialog and triggers any cancellation logic associated with it. It
    /// is typically used to close the dialog without applying any changes.</remarks>
    public void Cancel()
    {
        MudDialog.Cancel();
    }
}