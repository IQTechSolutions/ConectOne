using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using AccomodationModule.Domain.RequestFeatures;
using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Accomodation.Blazor.Modals
{
    /// <summary>
    /// Represents a modal dialog for adding a vacation interval.
    /// This component allows users to input details for a vacation interval and save or cancel the operation.
    /// </summary>
    public partial class CopyGeneralInformationFromAnotherVacation
    {
        private VacationDto? _selectedVacation;
        private string _generalInformation;

        #region Parameters

        /// <summary>
        /// Gets or sets the MudBlazor dialog instance for managing the modal's state.
        /// </summary>
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing vacation-related operations.
        /// </summary>
        /// <remarks>This property is automatically injected by the dependency injection framework. Ensure
        /// that a valid  implementation of <see cref="IVacationService"/> is registered in the service
        /// container.</remarks>
        [Inject] public IVacationService VacationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display transient notification messages to the user.
        /// </summary>
        /// <remarks>Inject this property to provide access to snackbar notifications within the
        /// component. The service allows displaying brief messages, such as alerts or confirmations, typically at the
        /// bottom of the screen.</remarks>
        [Inject] public ISnackbar Snackbar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the ID of the vacation associated with the interval.
        /// </summary>
        [Parameter] public string? VacationId { get; set; } = null!;

        #endregion

        #region Fields

        /// <summary>
        /// A list of available vacations for selection in the modal.
        /// </summary>
        private readonly List<VacationDto> _availableVacations = [];

        /// <summary>
        /// A function to convert a vacation DTO to its name for display purposes.
        /// </summary>
        private readonly Func<VacationDto?, string?> _vacationConverter = p => p?.VacationTitle.VacationTitle;
        
        #endregion

        #region Methods

        /// <summary>
        /// Sets the currently selected vacation and updates the associated general information.
        /// </summary>
        /// <param name="vacation">The vacation to select. Cannot be null. The general information of this vacation will be used to update
        /// related state.</param>
        public void SetSelectedVacation(VacationDto vacation)
        {
            _selectedVacation = vacation;
            _generalInformation = vacation.GeneralInformation.Information;
        }

        /// <summary>
        /// Saves the vacation interval and closes the modal dialog.
        /// </summary>
        private void SaveAsync()
        {
            MudDialog.Close(_generalInformation);
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
            await base.OnInitializedAsync();
            var lodgingResult = await VacationService.AllVacationsAsync(new VacationPageParameters() { PageSize = 100 });
            if (lodgingResult.Succeeded)
            {
                var rangeToAdd = lodgingResult.Data.Where(c => c.VacationId != VacationId);
                if (rangeToAdd.Any())
                {
                    _availableVacations.AddRange(rangeToAdd);
                }
                else
                {
                    Snackbar.AddError("There are no other vacations to copy general information from");
                    MudDialog.Cancel();
                }
            }
        }

        #endregion
    }
}
