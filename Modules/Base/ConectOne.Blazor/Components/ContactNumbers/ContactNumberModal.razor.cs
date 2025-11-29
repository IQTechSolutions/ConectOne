using ConectOne.Application.ViewModels;
using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ConectOne.Blazor.Components.ContactNumbers
{
    /// <summary>
    /// A modal component for displaying and editing a <see cref="ContactNumberViewModel"/>.
    /// Provides configurable UI text for the title, success message, and primary action button.
    /// </summary>
    public partial class ContactNumberModal
    {
        /// <summary>
        /// The dialog instance provided by MudBlazor's <see cref="MudDialog"/> framework.
        /// Used to control the lifecycle of this modal, such as closing or canceling.
        /// </summary>
        [CascadingParameter] IMudDialogInstance MudDialog { get; set; } = null!;

        [Inject] public ISnackbar SnackBar { get; set; }

        /// <summary>
        /// The <see cref="ContactNumberViewModel"/> holding phone number details (e.g., 
        /// international code, area code, number, whether it's default, etc.).
        /// </summary>
        [Parameter] public ContactNumberViewModel ContactNumber { get; set; } = null!;

        /// <summary>
        /// The title text displayed in the modal header.
        /// </summary>
        [Parameter] public string Title { get; set; } = null!;

        /// <summary>
        /// The success message shown via a snackbar upon successful completion of the modal action.
        /// </summary>
        [Parameter] public string SuccessText { get; set; } = null!;

        /// <summary>
        /// The text displayed on the primary action button (e.g., "Save" or "Confirm").
        /// </summary>
        [Parameter] public string ButtonText { get; set; } = null!;

        /// <summary>
        /// Cancels the modal operation. This action calls <see cref="MudDialog.Cancel"/>, 
        /// indicating that the user decided not to proceed.
        /// </summary>
        private void Cancel()
        {
            MudDialog.Cancel();
        }

        /// <summary>
        /// Invoked when the user confirms the modal action (e.g., to create or update the contact number).
        /// Displays a success message in a snackbar, then closes the modal and returns the 
        /// updated <see cref="ContactNumberViewModel"/> to the caller.
        /// </summary>
        private void CreateCustomer()
        {
            if (string.IsNullOrEmpty(ContactNumber.Number))
            {
                SnackBar.AddError("Number is a required field");
                return;
            }

            SnackBar.Add(SuccessText, Severity.Success);
            MudDialog.Close(DialogResult.Ok(ContactNumber));
        }
    }
}
