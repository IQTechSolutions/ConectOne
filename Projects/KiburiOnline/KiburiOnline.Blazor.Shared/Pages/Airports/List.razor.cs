using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Airports
{
    /// <summary>
    /// Represents a Blazor component for listing airports.
    /// This component allows users to view, add, update, and delete airports.
    /// </summary>
    public partial class List
    {
        /// <summary>
        /// Gets or sets the dialog service used to display dialogs in the application.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used for managing navigation and URI manipulation
        /// in Blazor applications.
        /// </summary>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing airport-related operations.
        /// </summary>
        [Inject] public IAirportService AirportService { get; set; } = null!;

        /// <summary>
        /// A list to hold the airports data.
        /// </summary>
        private List<AirportDto> _airports = [];

        /// <summary>
        /// A MudTable instance for displaying airports in a table format.
        /// </summary>
        private MudTable<AirportDto> _table = null!;

        /// <summary>
        /// A boolean flag to indicate whether the component has loaded its data.
        /// </summary>
        private bool _loaded;

        /// <summary>
        /// Deletes an airport by its ID.
        /// </summary>
        /// <param name="airportId">The ID of the airport to delete.</param>
        private async Task DeleteAirport(string airportId)
        {
            // Configure the parameters for the confirmation dialog.
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this airport from the availability list?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            // Show the confirmation dialog.
            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            // If the dialog was not canceled, proceed with deletion.
            if (!result!.Canceled)
            {
                // Call the API to delete the airport.
                var deleteResult = await AirportService.RemoveAirportAsync(airportId);
                // If the deletion was successful, reload the table data.
                if (deleteResult.Succeeded)
                {
                    _airports.Remove(_airports.FirstOrDefault(c => c.Id == airportId));
                    await _table.ReloadServerData();
                }
            }
        }

        /// <summary>
        /// Adds a new airport.
        /// </summary>
        private async Task AddAirport()
        {
            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                FullWidth = true,
                MaxWidth = MaxWidth.Medium
            };

            var parameters = new DialogParameters<AirportModal>
            {
                { x => x.Airport, new AirportViewModel() }
            };

            // Show the airport modal dialog.
            var dialog = await DialogService.ShowAsync<AirportModal>("Confirm", parameters, options);
            var result = await dialog.Result;

            // If the dialog was not canceled, proceed with adding the airport.
            if (!result!.Canceled)
            {
                // Cast the dialog result data to a AirportViewModel.
                var airportModel = ((AirportViewModel)result.Data!).ToDto();

                // Call the API to create the new airport.
                var creationResult = await AirportService.CreateAirportAsync(airportModel);
                // If the creation was successful, add the new item to the list and reload the table data.
                if (creationResult.Succeeded)
                {
                    _airports.Add(airportModel);
                    await _table.ReloadServerData();
                }
            }
        }

        /// <summary>
        /// Updates an existing airport.
        /// </summary>
        /// <param name="airport">The airport to update.</param>
        private async Task UpdateAirport(AirportDto airport)
        {
            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                FullWidth = true,
                MaxWidth = MaxWidth.Medium
            };

            // Configure the parameters for the airport modal dialog.
            var parameters = new DialogParameters<AirportModal>
            {
                { x => x.Airport, new AirportViewModel(airport) }
            };

            // Show the airport modal dialog.
            var dialog = await DialogService.ShowAsync<AirportModal>("Confirm", parameters, options);
            var result = await dialog.Result;

            // If the dialog was not canceled, proceed with updating the airport.
            if (!result!.Canceled)
            {
                var index = _airports.IndexOf(airport);

                // Cast the dialog result data to a AirportViewModel.
                var airportModel = ((AirportViewModel)result.Data!).ToDto();

                // Call the API to update the airport.
                var updateResult = await AirportService.UpdateAirportAsync(airportModel);
                // If the update was successful, update the item in the list and reload the table data.
                if (updateResult.Succeeded)
                {
                    _airports[index] = airportModel;
                    await _table.ReloadServerData();
                }
            }
        }

        /// <summary>
        /// Overrides the OnInitializedAsync method to perform initialization logic when the component is initialized.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            // Call the API to get the list of airports.
            var request = await AirportService.AllAirportsAsync();

            // If the request was successful, populate the list of airports.
            if (request.Succeeded)
            {
                _airports = request.Data.ToList();
            }
            else
            {
                // If the request failed, add the error messages to the SnackBar.
                SnackBar.AddErrors(request.Messages);
                return;
            }

            // Set the loaded flag to true.
            _loaded = true;

            await base.OnInitializedAsync();
        }
    }
}
