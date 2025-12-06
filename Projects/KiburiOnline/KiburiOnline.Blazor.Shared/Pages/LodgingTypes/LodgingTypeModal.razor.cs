using AccomodationModule.Application.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.LodgingTypes
{
    /// <summary>
    /// Represents a modal dialog for managing lodging type information.
    /// </summary>
    /// <remarks>This class is used to display and handle user interactions for lodging type data within a
    /// modal dialog. It provides functionality to submit or cancel the dialog, passing the lodging type data back to
    /// the caller.</remarks>
    public partial class LodgingTypeModal
    {
        /// <summary>
        /// Gets the current instance of the dialog being managed by this component.
        /// </summary>
        /// <remarks>This property is used to interact with the dialog instance, such as closing the
        /// dialog or retrieving its state.</remarks>
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

        /// <summary>
        /// Gets or sets the lodging type information for the current context.
        /// </summary>
        [Parameter] public LodgingTypeViewModel LodgingType { get; set; } = new();

        /// <summary>
        /// Closes the dialog and submits the selected lodging type.
        /// </summary>
        /// <remarks>This method finalizes the user's selection by closing the dialog and passing the
        /// selected lodging type. Ensure that <see cref="LodgingType"/> is set to the desired value before calling this
        /// method.</remarks>
        public void Submit()
        {
            MudDialog.Close(LodgingType);
        }

        /// <summary>
        /// Cancels the current dialog operation.
        /// </summary>
        /// <remarks>This method triggers the cancellation of the dialog by invoking the underlying  <see
        /// cref="MudDialog.Cancel"/> method. Use this to programmatically close the dialog  and signal a cancellation
        /// to the caller.</remarks>
        public void Cancel()
        {
            MudDialog.Cancel();
        }

    }
}
