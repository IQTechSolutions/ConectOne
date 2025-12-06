using AccomodationModule.Domain.DataTransferObjects;
using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Vacations
{
    /// <summary>
    /// Component for displaying and managing destinations associated with a vacation.
    /// </summary>
    public partial class VacationDestinations
    {
        #region Injections

        /// <summary>
        /// Gets or sets the injected <see cref="ISnackbar"/> service used to display notifications or messages.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the HTTP provider used to perform HTTP operations.
        /// </summary>
        [Inject] public IBaseHttpProvider Provider { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        #endregion

        #region Parameters

        /// <summary>
        /// The ID of the vacation to which the destinations belong.
        /// </summary>
        [Parameter, EditorRequired] public string VacationId { get; set; } = null!;

        #endregion

        #region Private Fields

        /// <summary>
        /// List to hold the destinations associated with the vacation.
        /// </summary>
        private List<DestinationDto> _destinations = new();

        #endregion

        #region Overrides

        /// <summary>
        /// Initializes the component and loads the destinations associated with the vacation.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            // Fetch the destinations associated with the vacation
            var destinationsResult = await Provider.GetAsync<IEnumerable<DestinationDto>>($"vacations/vacationdestinations/{VacationId}");
            destinationsResult.ProcessResponseForDisplay(SnackBar, () =>
            {
                // Populate the destinations list with the fetched data
                _destinations = destinationsResult.Data.ToList();
            });

            await base.OnInitializedAsync();
        }

        #endregion
    }
}
