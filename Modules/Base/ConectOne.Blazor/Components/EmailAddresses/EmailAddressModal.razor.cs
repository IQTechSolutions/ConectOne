using ConectOne.Application.ViewModels;
using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ConectOne.Blazor.Components.EmailAddresses
{
    /// <summary>
    /// A modal dialog for creating or editing an <see cref="EmailAddressViewModel"/>.
    /// Displays configurable text (title, success message, button text) and 
    /// returns the resulting email address via the <see cref="MudDialog"/> mechanism.
    /// </summary>
    public partial class EmailAddressModal
    {
        /// <summary>
        /// The instance of the MudBlazor dialog, used for controlling its life cycle
        /// (e.g., canceling or closing with a result).
        /// </summary>
        [CascadingParameter] IMudDialogInstance MudDialog { get; set; } = null!;

        [Inject]public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// The email address data that the modal displays and allows the user to edit.
        /// </summary>
        [Parameter] public EmailAddressViewModel EmailAddress { get; set; } = null!;

        /// <summary>
        /// The title text displayed at the top of the modal.
        /// </summary>
        [Parameter] public string Title { get; set; } = null!;

        /// <summary>
        /// The success message shown when the user successfully completes the modal action.
        /// </summary>
        [Parameter] public string SuccessText { get; set; } = null!;

        /// <summary>
        /// The text displayed on the primary action button (e.g., "Save" or "Confirm").
        /// </summary>
        [Parameter] public string ButtonText { get; set; } = null!;

        /// <summary>
        /// Cancels the operation and closes the dialog without returning any value.
        /// </summary>
        private void Cancel()
        {
            MudDialog.Cancel();
        }

        /// <summary>
        /// Called when the user confirms the action (e.g., saving or updating an email address).
        /// Displays a success message and closes the dialog, returning the updated 
        /// <see cref="EmailAddressViewModel"/>.
        /// </summary>
        private void CreateCustomer()
        {
            if (string.IsNullOrEmpty(EmailAddress.EmailAddress))
            {
                SnackBar.AddError("Address is a required field");
                return;
            }

            SnackBar.Add(SuccessText, Severity.Success);
            MudDialog.Close(DialogResult.Ok(EmailAddress));
        }
    }
}
