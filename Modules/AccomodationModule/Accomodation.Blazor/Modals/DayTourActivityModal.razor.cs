using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Accomodation.Blazor.Modals
{
    /// <summary>
    /// Represents a modal dialog for adding a vacation interval.
    /// This component allows users to input details for a vacation interval and save or cancel the operation.
    /// </summary>
    public partial class DayTourActivityModal
    {
        #region Parameters

        /// <summary>
        /// Gets or sets the MudBlazor dialog instance for managing the modal's state.
        /// </summary>
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

        /// <summary>
        /// Gets or sets the ID of the vacation associated with the interval.
        /// </summary>
        [Parameter] public string VacationId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for the associated extension.
        /// </summary>
        public string? ExtensionId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the vacation interval view model containing the interval details.
        /// </summary>
        [Parameter] public DayTourActivityViewModel? DayTour { get; set; }

        [Inject] public IDayTourActivityTemplateService DayTourActivityTemplateService { get; set; } = null!;

        #endregion

        #region Methods

        private readonly Func<DayTourActivityTemplateDto?, string?> _daytourActivityTemplateConverter = p => p?.Name;
        public IEnumerable<DayTourActivityTemplateDto> DayTourActivityTemplates { get; set; } = new List<DayTourActivityTemplateDto>();

        /// <summary>
        /// Saves the vacation interval and closes the modal dialog.
        /// </summary>
        private void SaveAsync()
        {
            MudDialog.Close(DayTour);
        }

        /// <summary>
        /// Cancels the operation and closes the modal dialog.
        /// </summary>
        private void Cancel()
        {
            MudDialog.Cancel();
        }

        #endregion

        /// <summary>
        /// Lifecycle method invoked when the component is first initialized.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            if (DayTour == null)
            {
                DayTour = new DayTourActivityViewModel() { VacationId = VacationId, VacationExtensionId = ExtensionId};
            }

            var templatesResult = await DayTourActivityTemplateService.GetAllAsync();
            if (templatesResult.Succeeded)
            {
                DayTourActivityTemplates = templatesResult.Data.ToList();
            }

            await base.OnInitializedAsync();
        }
    }
}
