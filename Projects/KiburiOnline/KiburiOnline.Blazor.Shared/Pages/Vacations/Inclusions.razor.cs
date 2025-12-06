using AccomodationModule.Domain.DataTransferObjects;
using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Vacations
{
    /// <summary>
    /// Component for displaying and managing inclusions associated with a vacation.
    /// </summary>
    public partial class Inclusions
    {
        #region Private Fields

        /// <summary>
        /// Gets or sets the HTTP provider used to perform HTTP operations.
        /// </summary>
        [Inject] public IBaseHttpProvider Provider { get; set; } = null!;

        /// <summary>
        /// Gets or sets the injected <see cref="ISnackbar"/> service used to display notifications or messages.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        #endregion

        #region Parameters

        /// <summary>
        /// The ID of the vacation to which the inclusions belong.
        /// </summary>
        [Parameter] public string? VacationId { get; set; } 

        /// <summary>
        /// Gets or sets the unique identifier for the vacation extension.
        /// </summary>
        [Parameter] public string? VacationExtensionId { get; set; } 

        #endregion

        #region Private Fields

        /// <summary>
        /// List to hold the inclusions associated with the vacation.
        /// </summary>
        private IEnumerable<VacationInclusionDto> _inclusions = new List<VacationInclusionDto>();

        #endregion

        #region Overrides

        /// <summary>
        /// Initializes the component and loads the inclusions associated with the vacation.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrEmpty(VacationId))
            {
                var inclusionResult = await Provider.GetAsync<IEnumerable<VacationInclusionDto>>($"vacations/inclusions/{VacationId}");
                inclusionResult.ProcessResponseForDisplay(SnackBar, () =>
                {
                    // Populate the inclusions list with the fetched data
                    _inclusions = inclusionResult.Data;
                });
            }

            if (!string.IsNullOrEmpty(VacationExtensionId))
            {
                var inclusionResult = await Provider.GetAsync<IEnumerable<VacationInclusionDto>>($"vacations/extensions/inclusions/{VacationExtensionId}");
                inclusionResult.ProcessResponseForDisplay(SnackBar, () =>
                {
                    // Populate the inclusions list with the fetched data
                    _inclusions = inclusionResult.Data;
                });
            }
            

            await base.OnInitializedAsync();
        }

        #endregion
    }
}