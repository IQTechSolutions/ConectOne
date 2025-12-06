using AccomodationModule.Application.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Countries
{
    /// <summary>
    /// Represents a modal dialog for managing country information.
    /// </summary>
    public partial class CountryModal
    {
        /// <summary>
        /// Gets or sets the current dialog instance for the component.
        /// </summary>
        /// <remarks>This property is typically set automatically via cascading parameters when the
        /// component is used within a MudBlazor dialog. It allows the component to interact with the dialog, such as
        /// closing it or returning a result.</remarks>
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

        /// <summary>
        /// Gets or sets the country information to be displayed or edited in the component.
        /// </summary>
        [Parameter] public CountryViewModel Country { get; set; } = new CountryViewModel();

        /// <summary>
        /// Closes the dialog and saves the selected country asynchronously.
        /// </summary>
        /// <remarks>This method should be called when the user confirms their selection. The dialog will
        /// be closed and the selected country will be returned to the caller. This method is intended for internal use
        /// within the dialog component.</remarks>
        private void SaveAsync()
        {
            MudDialog.Close(Country);
        }

        /// <summary>
        /// Cancels the current dialog operation and closes the dialog without returning a result.
        /// </summary>
        /// <remarks>Use this method to dismiss the dialog when the user chooses to cancel or exit without
        /// making a selection. No value will be returned to the caller upon cancellation.</remarks>
        public void Cancel()
        {
            MudDialog.Cancel();
        }
    }
}
