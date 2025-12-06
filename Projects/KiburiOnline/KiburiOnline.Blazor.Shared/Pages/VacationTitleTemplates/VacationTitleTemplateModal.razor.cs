using AccomodationModule.Application.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.VacationTitleTemplates;

/// <summary>
/// Represents a modal dialog for editing a vacation title template.
/// </summary>
/// <remarks>This class provides functionality to save or cancel changes made to a vacation title
/// template.</remarks>
public partial class VacationTitleTemplateModal
{
    /// <summary>
    /// Gets or sets the current instance of the dialog.
    /// </summary>
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

    /// <summary>
    /// Gets or sets the template used for displaying vacation titles.
    /// </summary>
    [Parameter] public VacationTitleTemplateViewModel Template { get; set; } = new VacationTitleTemplateViewModel();

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
    /// should be called when the dialog needs to be closed without completing the intended operation.</remarks>
    public void Cancel()
    {
        MudDialog.Cancel();
    }
}
