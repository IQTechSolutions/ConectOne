using AccomodationModule.Application.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Areas
{
    /// <summary>
    /// Represents a modal dialog for displaying and editing average temperature data.
    /// </summary>
    /// <remarks>This class is designed to be used within a Blazor application and interacts with a dialog
    /// instance to handle user actions such as submitting or canceling the dialog.</remarks>
    public partial class AverageTempModal
    {
        /// <summary>
        /// Represents the current dialog instance within the cascading MudBlazor component hierarchy.
        /// </summary>
        /// <remarks>This property is automatically set via cascading parameters in the MudBlazor
        /// framework. It should not be manually assigned or modified by the user.</remarks>
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

        /// <summary>
        /// Gets or sets the view model representing the average temperature data.
        /// </summary>
        [Parameter] public AverageTemperatureViewModel AverageTemperature { get; set; } = new();

        /// <summary>
        /// Submits the current average temperature value and closes the dialog.
        /// </summary>
        /// <remarks>This method is typically used to finalize the input and dismiss the dialog. The <see
        /// cref="AverageTemperature"/> value is passed to the dialog's close operation. Ensure that <see
        /// cref="AverageTemperature"/> is set to a valid value before calling this method.</remarks>
        public void Submit()
        {
            MudDialog.Close(AverageTemperature);
        }

        /// <summary>
        /// Cancels the current dialog operation.
        /// </summary>
        /// <remarks>This method terminates the dialog and triggers any cancellation logic associated with
        /// it. Use this method to programmatically close a dialog when a cancellation action is required.</remarks>
        public void Cancel()
        {
            MudDialog.Cancel();
        }

    }
}
