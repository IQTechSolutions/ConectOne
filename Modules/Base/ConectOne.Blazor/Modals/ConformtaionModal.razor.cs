using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ConectOne.Blazor.Modals
{
    /// <summary>
    /// Represents a modal dialog component that displays a confirmation message with customizable content and action
    /// buttons.
    /// </summary>
    /// <remarks>Use this component to prompt users for confirmation before proceeding with an action. The
    /// modal supports customizable text for the main content, confirmation button, and optional cancel button. The
    /// appearance of the confirmation button can be adjusted using the specified color.</remarks>
    public partial class ConformtaionModal
    {
        /// <summary>
        /// Gets or sets the dialog instance for interacting with the current MudBlazor dialog.
        /// </summary>
        /// <remarks>This property is typically provided as a cascading parameter within a MudBlazor
        /// dialog component. Use it to close the dialog, pass results, or perform other dialog-related
        /// actions.</remarks>
        [CascadingParameter] IMudDialogInstance MudDialog { get; set; }

        /// <summary>
        /// Gets or sets the text content to be displayed within the component.
        /// </summary>
        [Parameter] public string ContentText { get; set; }

        /// <summary>
        /// Gets or sets the text displayed on the button.
        /// </summary>
        [Parameter] public string ButtonText { get; set; }

        /// <summary>
        /// Gets or sets the text displayed on the cancel button.
        /// </summary>
        [Parameter] public string CancelButtonText { get; set; } = "Cancel";

        /// <summary>
        /// Gets or sets a value indicating whether the Cancel button is displayed.
        /// </summary>
        [Parameter] public bool ShowCancelButton { get; set; } = true;

        /// <summary>
        /// Gets or sets the color to use for the component.
        /// </summary>
        [Parameter] public Color Color { get; set; }

        /// <summary>
        /// Closes the dialog and indicates a successful result.
        /// </summary>
        /// <remarks>Use this method to programmatically close the dialog and signal that the operation
        /// was completed successfully. This is typically called in response to a user action, such as clicking a submit
        /// or confirm button.</remarks>
        void Submit() => MudDialog.Close(DialogResult.Ok(true));

        /// <summary>
        /// Cancels the dialog and closes it without returning a result to the caller.
        /// </summary>
        /// <remarks>Use this method to dismiss the dialog when the user cancels the operation or chooses
        /// not to proceed. No data will be returned to the dialog's caller.</remarks>
        void Cancel() => MudDialog.Cancel();
    }
}
