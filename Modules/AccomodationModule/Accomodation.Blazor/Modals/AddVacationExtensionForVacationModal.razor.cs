using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Accomodation.Blazor.Modals
{
    /// <summary>
    /// Modal component for adding a vacation destination.
    /// </summary>
    public partial class AddVacationExtensionForVacationModal
    {
        #region Parameters

        /// <summary>
        /// Instance of the MudBlazor dialog.
        /// </summary>
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

        /// <summary>
        /// The ID of the vacation.
        /// </summary>
        [Parameter] public string VacationId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing vacation-related operations.
        /// </summary>
        /// <remarks>This property is automatically injected by the dependency injection framework. Ensure
        /// that a valid  implementation of <see cref="IVacationService"/> is registered in the dependency
        /// container.</remarks>
        [Inject] public IVacationService VacationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display transient messages to the user.
        /// </summary>
        /// <remarks>This property is typically injected by the framework and provides methods for showing
        /// snackbars or toast notifications in the user interface. Assigning this property manually is not recommended
        /// outside of dependency injection scenarios.</remarks>
        [Inject] public ISnackbar Snackbar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for the vacation host.
        /// </summary>
        [Parameter] public string VacationHostId { get; set; } = null!;

        /// <summary>
        /// Indicates whether a new destination is being created.
        /// </summary>
        [Parameter] public bool Creating { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// The destination view model.
        /// </summary>
        private VacationDto VacationExtension { get; set; } = null!;

        /// <summary>
        /// List of available destinations.
        /// </summary>
        private List<VacationDto> _availableVacationExtensions = new();

        /// <summary>
        /// Function to convert a destination view model to its name.
        /// </summary>
        private readonly Func<VacationDto?, string> _vacationExtensionConverter = p => p?.VacationTitle?.VacationTitle is null ? "" : p.VacationTitle.VacationTitle;

        #endregion

        #region Actions

        /// <summary>
        /// Saves the destination asynchronously.
        /// </summary>
        private async Task SaveAsync()
        {
            MudDialog.Close(VacationExtension);
        }

        /// <summary>
        /// Cancels the operation and closes the dialog.
        /// </summary>
        public void Cancel()
        {
            MudDialog.Cancel();
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes the destination selections asynchronously.
        /// </summary>
        private async Task InitiateDestinationSelections()
        {
            var result = await VacationService.AllExtensionsAsync();

            result.ProcessResponseForDisplay(Snackbar, async () =>
            {
                _availableVacationExtensions = result.Data.ToList();
            });
        }

        /// <summary>
        /// Called when the component is initialized.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await InitiateDestinationSelections();
            await base.OnInitializedAsync();
        }

        #endregion
    }
}
