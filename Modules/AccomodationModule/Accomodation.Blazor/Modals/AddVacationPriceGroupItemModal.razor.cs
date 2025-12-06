using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Accomodation.Blazor.Modals
{
    /// <summary>
    /// Modal component for adding a vacation price group item.
    /// </summary>
    public partial class AddVacationPriceGroupItemModal
    {
        /// <summary>
        /// Gets or sets the current dialog instance for the component.
        /// </summary>
        /// <remarks>This property is typically provided as a cascading parameter by the MudBlazor dialog
        /// infrastructure. Use this instance to interact with the dialog, such as closing it or returning a result.
        /// This property is intended for use within dialog components and should not be set manually.</remarks>
		[CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Closes the current dialog asynchronously.
        /// </summary>
        private void SaveAsync()
		{
            MudDialog.Close(Name);
        }

        /// <summary>
        /// Cancels the current dialog operation and closes the dialog without applying any changes.
        /// </summary>
        /// <remarks>Use this method to dismiss the dialog when the user chooses to cancel or exit without
        /// saving. After calling this method, any pending changes in the dialog are discarded.</remarks>
		public void Cancel()
		{
			MudDialog.Cancel();
		}
	}
}
