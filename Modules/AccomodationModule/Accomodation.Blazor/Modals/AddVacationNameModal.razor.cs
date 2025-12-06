using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Accomodation.Blazor.Modals
{
    /// <summary>
    /// Modal component for adding a vacation name.
    /// </summary>
    public partial class AddVacationNameModal
    {
        /// <summary>
        /// Gets or sets the dialog instance associated with the current component context.
        /// </summary>
        /// <remarks>This property is typically provided as a cascading parameter within MudBlazor dialog
        /// components. It enables interaction with the dialog, such as closing or updating its state. Accessing this
        /// property outside of a dialog context may result in a null value.</remarks>
		[CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Closes the current dialog asynchronously.
        /// </summary>
        /// <remarks>This method triggers the closure of the dialog associated with the current context.
        /// It should be called when the dialog's operation is complete and the UI should be dismissed. If the dialog is
        /// already closed, calling this method has no effect.</remarks>
        private void SaveAsync()
		{
            MudDialog.Close(Name);
        }

        /// <summary>
        /// Cancels the current dialog operation and closes the dialog without returning a result.
        /// </summary>
        /// <remarks>Use this method to dismiss the dialog when the user chooses to cancel or exit without
        /// making a selection. No result will be passed back to the caller.</remarks>
		public void Cancel()
		{
			MudDialog.Cancel();
		}
	}
}
