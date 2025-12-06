using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Restaurants
{
    /// <summary>
    /// Represents a Blazor component for listing cancellation terms.
    /// This component allows users to view, add, update, and delete cancellation terms.
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
        /// Gets or sets the service used to perform restaurant-related operations.
        /// </summary>
        [Inject] public IRestaurantService RestaurantService { get; set; } = null!;

        /// <summary>
        /// A list to hold the cancellation terms data.
        /// </summary>
        private List<RestaurantDto> _restaurants = [];

        /// <summary>
        /// A MudTable instance for displaying cancellation terms in a table format.
        /// </summary>
        private MudTable<RestaurantDto> _table = null!;

        /// <summary>
        /// A boolean flag to indicate whether the component has loaded its data.
        /// </summary>
        private bool _loaded;

        /// <summary>
        /// Deletes a restaurant from the availability list after user confirmation.
        /// </summary>
        /// <remarks>This method displays a confirmation dialog to the user before proceeding with the
        /// deletion.  If the user confirms, the restaurant is removed from the availability list and the data is
        /// reloaded.</remarks>
        /// <param name="restaurantId">The unique identifier of the restaurant to be deleted. Cannot be <see langword="null"/> or empty.</param>
        /// <returns></returns>
        private async Task DeleteRestaurant(string restaurantId)
        {
            // Configure the parameters for the confirmation dialog.
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this restaurant from the availability list?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            // Show the confirmation dialog.
            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            // If the dialog was not canceled, proceed with deletion.
            if (!result!.Canceled)
            {
                // Call the API to delete the cancellation term.
                var deleteResult = await RestaurantService.RemoveRestaurantAsync(restaurantId);
                // If the deletion was successful, reload the table data.
                if (deleteResult.Succeeded)
                {
                    _restaurants.Remove(_restaurants.FirstOrDefault(c => c.Id == restaurantId));
                    await _table.ReloadServerData();
                }
            }
        }

        /// <summary>
        /// Adds a new cancellation term.
        /// </summary>
        private async Task AddRestaurant()
        {
            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                FullWidth = true,
                MaxWidth = MaxWidth.Medium
            };

            var parameters = new DialogParameters<RestaurantModal>
            {
                { x => x.Restaurant, new RestaurantViewModel() }
            };

            // Show the cancellation term modal dialog.
            var dialog = await DialogService.ShowAsync<RestaurantModal>("Confirm", options);
            var result = await dialog.Result;

            // If the dialog was not canceled, proceed with adding the cancellation term.
            if (!result!.Canceled)
            {
                // Cast the dialog result data to a BookingTermViewModel.
                var cancellationTerm = ((RestaurantViewModel)result.Data!).ToDto();

                // Call the API to create the new cancellation term.
                var creationResult = await RestaurantService.CreateRestaurantAsync(cancellationTerm);
                // If the creation was successful, add the new term to the list and reload the table data.
                if (creationResult.Succeeded)
                {
                    _restaurants.Add(cancellationTerm);
                    await _table.ReloadServerData();
                }
            }
        }

        /// <summary>
        /// Updates an existing cancellation term.
        /// </summary>
        /// <param name="cancellationTermId">The ID of the cancellation term to update.</param>
        private async Task UpdateRestaurant(RestaurantDto restaurant)
        {
            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                FullWidth = true,
                MaxWidth = MaxWidth.Medium
            };
            
            // Configure the parameters for the cancellation term modal dialog.
            var parameters = new DialogParameters<RestaurantModal>
            {
                { x => x.Restaurant, new RestaurantViewModel(restaurant) }
            };

            // Show the cancellation term modal dialog.
            var dialog = await DialogService.ShowAsync<RestaurantModal>("Confirm", parameters, options);
            var result = await dialog.Result;

            // If the dialog was not canceled, proceed with updating the cancellation term.
            if (!result!.Canceled)
            {
                var index = _restaurants.IndexOf(restaurant);

                // Cast the dialog result data to a BookingTermViewModel.
                var cancellationTerm = ((RestaurantViewModel)result.Data).ToDto();

                // Call the API to update the cancellation term.
                var updateResult = await RestaurantService.UpdateRestaurantAsync(cancellationTerm);
                // If the update was successful, update the term in the list and reload the table data.
                if (updateResult.Succeeded)
                {
                    _restaurants[index] = cancellationTerm;
                    await _table.ReloadServerData();
                }
            }
        }

        /// <summary>
        /// Overrides the OnInitializedAsync method to perform initialization logic when the component is initialized.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            // Call the API to get the list of cancellation terms.
            var request = await RestaurantService.AllRestaurantsAsync();
                
            // If the request was successful, populate the list of cancellation terms.
            if (request.Succeeded)
            {
                _restaurants = request.Data.ToList();
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