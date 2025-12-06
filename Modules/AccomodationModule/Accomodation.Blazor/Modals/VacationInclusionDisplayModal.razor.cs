using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.Enums;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Accomodation.Blazor.Modals
{
    /// <summary>
    /// Represents a modal dialog for adding a vacation interval.
    /// This component allows users to input details for a vacation interval and save or cancel the operation.
    /// </summary>
    public partial class VacationInclusionDisplayModal
    {
        #region Parameters

        /// <summary>
        /// Gets or sets the MudBlazor dialog instance for managing the modal's state.
        /// </summary>
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

        /// <summary>
        /// Gets or sets the vacation inclusion display type information for the current view model.
        /// </summary>
        public VacationInclusionDisplayTypeInformationViewModel VacationInclusionDisplayType { get; set; } = new();

        #endregion

        #region Fields

        /// <summary>
        /// A list of available Vacation Inclusion Display Types for selection in the modal.
        /// </summary>
        private List<VacationInclusionDisplayTypes> _availableVacationInclusionDisplayTypes = [];

        /// <summary>
        /// A list of available destinations for selection in the modal.
        /// </summary>
        private readonly List<string> _availableColumnNames = [ "One", "Two" ];

        #endregion

        #region Methods

        /// <summary>
        /// Saves the vacation interval and closes the modal dialog.
        /// </summary>
        private void SaveAsync()
        {
            MudDialog.Close(VacationInclusionDisplayType);
        }

        /// <summary>
        /// Cancels the operation and closes the modal dialog.
        /// </summary>
        private void Cancel()
        {
            MudDialog.Cancel();
        }

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Called when the component is initialized. Loads the available lodgings from the server.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            _availableVacationInclusionDisplayTypes = Enum.GetValues(typeof(VacationInclusionDisplayTypes)).Cast<VacationInclusionDisplayTypes>().ToList();
            await base.OnInitializedAsync();
        }

        #endregion
    }
}
