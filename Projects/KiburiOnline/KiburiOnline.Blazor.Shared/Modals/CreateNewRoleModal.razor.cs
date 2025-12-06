using IdentityModule.Application.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Modals
{
    /// <summary>
    /// Represents a modal dialog for assigning a new role to a user.
    /// </summary>
    /// <remarks>This component is designed to facilitate the assignment of roles to users within the system.
    /// It displays the user's name, a list of available roles, and provides options to submit or cancel the role
    /// assignment. The modal interacts with injected services to retrieve user and role data, submit role assignments,
    /// and display notifications.</remarks>
    public partial class CreateNewRoleModal
    {
        private UserRoleViewModel _userRole = new();

        /// <summary>
        /// Gets the current instance of the dialog being managed by the component.
        /// </summary>
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

        /// <summary>
        /// Submits the current user role and closes the dialog.
        /// </summary>
        /// <remarks>This method closes the dialog by passing the current user role to the dialog's close
        /// mechanism. Ensure that the user role is properly set before calling this method.</remarks>
        public void Submit()
        {
            MudDialog.Close(_userRole);
        }

        /// <summary>
        /// Cancels the current dialog operation.
        /// </summary>
        /// <remarks>This method terminates the dialog and triggers the cancellation behavior. It is
        /// typically used to close a dialog without confirming any changes or actions.</remarks>
        public void Cancel()
        {
            MudDialog.Cancel();
        }
    }
}
