using AccomodationModule.Application.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Areas
{
    /// <summary>
    /// Represents a modal dialog for managing area-related data.
    /// </summary>
    /// <remarks>This class is designed to be used within a Blazor application and provides functionality for
    /// submitting or canceling changes to an area. It interacts with a dialog instance to handle user
    /// actions.</remarks>
    public partial class AreaModal
    {
        /// <summary>
        /// Represents the current dialog instance within the cascading MudBlazor component hierarchy.
        /// </summary>
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

        /// <summary>
        /// Gets or sets the area information represented by an <see cref="AreaViewModel"/>.
        /// </summary>
        [Parameter] public AreaViewModel Area { get; set; } = new();

        /// <summary>
        /// Closes the current dialog within the specified area.
        /// </summary>
        /// <remarks>This method triggers the closure of a dialog using the provided area context. Ensure
        /// that the dialog is open before calling this method.</remarks>
        public void Submit()
        {
            MudDialog.Close(Area);
        }

        /// <summary>
        /// Cancels the current dialog operation.
        /// </summary>
        /// <remarks>This method terminates the dialog and triggers any cancellation logic associated with
        /// it. It is typically used to close a dialog without completing its intended action.</remarks>
        public void Cancel()
        {
            MudDialog.Cancel();
        }

    }
}
