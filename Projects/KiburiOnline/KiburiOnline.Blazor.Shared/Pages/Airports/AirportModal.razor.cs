using AccomodationModule.Application.ViewModels;
using LocationModule.Domain.DataTransferObjects;
using LocationModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Airports
{
    /// <summary>
    /// Represents a modal dialog for managing airport information.
    /// </summary>
    /// <remarks>This class is designed to be used within a Blazor application and provides functionality for
    /// saving or canceling changes to an airport's details. It interacts with a dialog instance to handle user
    /// actions and relies on dependency injection for HTTP operations.</remarks>
    public partial class AirportModal
    {
        private readonly Func<CityDto?, string?> _cityConverter = p => p?.Name;

        /// <summary>
        /// Gets the current instance of the dialog being managed by the component.
        /// </summary>
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

        /// <summary>
        /// Gets or sets the airport information to be displayed or managed.
        /// </summary>
        [Parameter] public AirportViewModel Airport { get; set; } = new AirportViewModel() { Id = Guid.NewGuid().ToString() };
        
        /// <summary>
        /// Gets or sets the <see cref="ISnackbar"/> service used to display notifications and messages to the user.
        /// </summary>
        /// <remarks>This property is typically injected by the dependency injection framework and should
        /// not be set manually in most cases.</remarks>
        [Inject] public ISnackbar Snackbar { get; set; } = null!;

        [Inject] public ICityService CityService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the collection of cities.
        /// </summary>
        public IEnumerable<CityDto> Cities { get; set; } = new List<CityDto>();

        /// <summary>
        /// Saves the current airport data and closes the dialog.
        /// </summary>
        /// <remarks>This method closes the dialog and passes the current airport data to the caller.
        /// Ensure that the <c>Airport</c> object contains valid data before invoking this method.</remarks>
        private void SaveAsync()
        {
            if (Airport.City.CityId == null)
            {
                Snackbar.Add("City is required");
                return;
            }
                

            MudDialog.Close(Airport);
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

        /// <summary>
        /// Asynchronously initializes the component and retrieves a list of cities from the data provider.
        /// </summary>
        /// <remarks>This method fetches city data from the provider using the "cities" endpoint.  If the
        /// operation succeeds, the retrieved data is stored in the <see cref="Cities"/> property.</remarks>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var citiesResult = await CityService.AllCitiesAsync();
            if (citiesResult.Succeeded)
            {
                Cities = citiesResult.Data.ToList();
            }

            await base.OnInitializedAsync();
        }
    }
}
