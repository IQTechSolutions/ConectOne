using AccomodationModule.Application.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.CustomVariableTags;

/// <summary>
/// Represents a modal dialog for managing a custom variable tag, providing options to save or cancel the operation.
/// </summary>
/// <remarks>This class is designed to be used within a Blazor application and interacts with the dialog's
/// lifecycle  through the <see cref="IMudDialogInstance"/> interface. It allows users to modify a <see
/// cref="CustomVariableTagViewModel"/>  and either save the changes or cancel the operation.</remarks>
public partial class CustomVariableTagModal
{
    /// <summary>
    /// Gets the current instance of the dialog, allowing interaction with the dialog's lifecycle.
    /// </summary>
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

    /// <summary>
    /// Gets or sets the tag associated with the custom variable.
    /// </summary>
    [Parameter] public CustomVariableTagViewModel Tag { get; set; } = new CustomVariableTagViewModel();

    /// <summary>
    /// Closes the dialog and returns the specified result.
    /// </summary>
    /// <remarks>This method is typically used to close a dialog and pass a result value back to the caller.
    /// Ensure that the <see cref="Tag"/> property contains the desired result before invoking this method.</remarks>
    private void SaveAsync()
    {
        MudDialog.Close(Tag);
    }

    /// <summary>
    /// Cancels the current dialog operation.
    /// </summary>
    /// <remarks>This method terminates the dialog and triggers the cancellation logic.  It is typically used
    /// to close the dialog without completing the operation.</remarks>
    public void Cancel()
    {
        MudDialog.Cancel();
    }
}
