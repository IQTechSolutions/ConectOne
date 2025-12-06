using AccomodationModule.Application.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Gifts;

/// <summary>
/// Represents a modal dialog for managing gift-related data.
/// </summary>
/// <remarks>This class is designed to be used within a Blazor application and provides functionality  for saving
/// or canceling changes to a gift. It interacts with a dialog instance to handle  user actions.</remarks>
public partial class GiftModal
{
    /// <summary>
    /// Gets the current instance of the dialog, allowing interaction with the dialog's lifecycle.
    /// </summary>
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

    /// <summary>
    /// Gets or sets the gift details to be displayed or processed.
    /// </summary>
    [Parameter] public GiftViewModel Gift { get; set; } = new GiftViewModel();

    /// <summary>
    /// Saves the current gift and closes the dialog.
    /// </summary>
    /// <remarks>This method finalizes the current operation by closing the dialog and returning the gift
    /// object.  Ensure that the gift object is properly initialized before calling this method.</remarks>
    private void SaveAsync()
    {
        MudDialog.Close(Gift);
    }

    /// <summary>
    /// Cancels the current dialog operation.
    /// </summary>
    /// <remarks>This method terminates the dialog and triggers the cancellation logic.  It is typically used
    /// to close the dialog without completing the operation  or returning a result.</remarks>
    public void Cancel()
    {
        MudDialog.Cancel();
    }
}

