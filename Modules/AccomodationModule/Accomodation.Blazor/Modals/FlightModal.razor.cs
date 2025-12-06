using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace Accomodation.Blazor.Modals
{
    /// <summary>
    /// Represents a modal dialog for adding a vacation interval.
    /// This component allows users to input details for a vacation interval and save or cancel the operation.
    /// </summary>
    public partial class FlightModal
    {
        private List<AirportDto> _availableAirports = [];
        private readonly Func<AirportDto?, string?> _airportConverter = p => p?.Name;

        #region Parameters

        /// <summary>
        /// Gets or sets the MudBlazor dialog instance for managing the modal's state.
        /// </summary>
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

        /// <summary>
        /// Gets or sets the ID of the vacation associated with the interval.
        /// </summary>
        [Parameter] public VacationDto Vacation { get; set; } = null!;

        /// <summary>
        /// Gets or sets the vacation interval view model containing the interval details.
        /// </summary>
        [Parameter] public FlightViewModel? Flight { get; set; }

        /// <summary>
        /// Gets or sets the flight service used to manage flight-related operations.
        /// </summary>
        [Inject] public IAirportService AirportService { get; set; }

        /// <summary>
        /// Gets or sets the application configuration settings used to retrieve key-value pairs and other configuration
        /// data.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// Flag to indicate if this is an arrival movement
        /// </summary>
        public bool VacationArrival { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Saves the vacation interval and closes the modal dialog.
        /// </summary>
        private void SaveAsync()
        {
            MudDialog.Close(Flight);
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
        /// Initializes the component.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            if (Flight == null)
            {
                Flight = new FlightViewModel() { VacationId = Vacation.VacationId };
            }

            var airportListResult = await AirportService.AllAirportsAsync();
            if (airportListResult.Succeeded)
            {
                _availableAirports = airportListResult.Data.ToList();
            }

            if (Flight.DepartureAirport == null && Flight.ArrivalAirport != null)
                VacationArrival = true;

            await base.OnInitializedAsync();
        }
    }
}
