using ConectOne.Domain.Enums;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace SchoolsEnterprise.Blazor.Shared.Pages.Authentication.Modals
{
    /// <summary>
    /// Represents a modal dialog component for restoring or recreating a role within the application.
    /// </summary>
    /// <remarks>This component is typically used within a MudBlazor dialog context to prompt users to either
    /// restore a previously deleted or modified role, or to restart the role creation process. It provides options to
    /// confirm restoration, recreate the role, or cancel the operation. The component exposes parameters to control its
    /// appearance and behavior, such as displaying a Cancel button or setting the dialog color.</remarks>
    public partial class RoleRestorationModal
    {
        /// <summary>
        /// Gets or sets the current dialog instance for this component.
        /// </summary>
        /// <remarks>This property is typically provided by the MudBlazor dialog infrastructure via
        /// cascading parameters. It allows the component to interact with the dialog, such as closing it or returning a
        /// result. This property is usually set automatically and should not be set manually.</remarks>
        [CascadingParameter] IMudDialogInstance MudDialog { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a Cancel button is displayed in the component.
        /// </summary>
        [Parameter] public bool ShowCancelButton { get; set; } = true;

        /// <summary>
        /// Gets or sets the color to use for the component.
        /// </summary>
        [Parameter] public Color Color { get; set; }

        /// <summary>
        /// Closes the dialog and indicates that the role creation process should be restarted.
        /// </summary>
        /// <remarks>Call this method to signal that the user wishes to recreate the role. The dialog will
        /// close with a result indicating a 'recreate' action, which can be handled by the calling code to initiate the
        /// role creation workflow again.</remarks>
        void ReCreate() => MudDialog.Close(DialogResult.Ok(RoleCreationAction.Recreate));

        /// <summary>
        /// Closes the dialog and indicates that the restore action was selected.
        /// </summary>
        /// <remarks>Call this method to signal that the user has chosen to perform a restore operation.
        /// The dialog will close with a result indicating the restore action.</remarks>
        void Restore() => MudDialog.Close(DialogResult.Ok(RoleCreationAction.Restore));

        /// <summary>
        /// Cancels the dialog and closes it without returning a result.
        /// </summary>
        /// <remarks>Use this method to dismiss the dialog when the user chooses to cancel or exit without
        /// completing the intended action. No result will be passed back to the caller.</remarks>
        void Cancel() => MudDialog.Cancel();
    }
}
