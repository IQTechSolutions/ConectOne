using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.DataTransferObjects;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Vacations.Modals
{
    /// <summary>
    /// Represents a modal dialog component for managing vacation gallery settings.
    /// </summary>
    /// <remarks>This class provides functionality for displaying and interacting with a modal dialog that
    /// allows users to configure vacation gallery settings. It includes methods for submitting or canceling the dialog,
    /// as well as injected services for HTTP operations and user notifications.</remarks>
    public partial class VacationGallerySetAsModal
    {
        private bool _loaded = false;
        private VacationGallerySetAsViewModel _model = new();
        private readonly string[] _imageTypes = ["Main Slider", "Summary Slider", "Banner Image 1", "Banner Image 1", "Banner Image 1", "Banner Image 1", "Map Image"];
        private ICollection<VacationDto> _availableVacations = [];
        private Func<VacationDto, string> vacationConverter = p => p?.VacationTitle.VacationTitle ?? string.Empty;

        /// <summary>
        /// Gets the current instance of the dialog being managed by the component.
        /// </summary>
        /// <remarks>This property is set via cascading parameters and is typically used internally by the
        /// component to manage dialog behavior. It should not be null during normal usage.</remarks>
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

        /// <summary>
        /// Gets or sets the snackbar service used to display notifications or messages to the user.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Submits the current dialog data and closes the dialog.
        /// </summary>
        /// <remarks>This method asynchronously closes the dialog using the provided model. Ensure that
        /// the dialog is in a valid state before calling this method.</remarks>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SubmitAsync()
        {
            MudDialog.Close(_model);
        }

        /// <summary>
        /// Cancels the current dialog operation.
        /// </summary>
        /// <remarks>This method terminates the dialog and triggers any cancellation logic associated with
        /// it. Use this method to programmatically close a dialog when a cancellation is required.</remarks>
        public void Cancel()
        {
            MudDialog.Cancel();
        }
    }
}
