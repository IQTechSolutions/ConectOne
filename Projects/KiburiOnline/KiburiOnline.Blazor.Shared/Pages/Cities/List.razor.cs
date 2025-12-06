using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using LocationModule.Application.ViewModels;
using LocationModule.Domain.DataTransferObjects;
using LocationModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Cities
{
    /// <summary>
    /// Represents a component for managing a list of cities, including functionality for adding, updating, and deleting
    /// cities.
    /// </summary>
    /// <remarks>This component interacts with various services to perform CRUD operations on city data. It
    /// provides a user interface for managing cities,  including confirmation dialogs for destructive actions and
    /// modals for data entry. The component also handles navigation and displays  notifications for operation
    /// results.</remarks>
    public partial class List
    {
        private List<CityDto> _cities = [];
        private MudTable<CityDto> _table = null!;
        private bool _loaded;

        /// <summary>
        /// Gets or sets the service used to display dialogs in the application.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service for displaying snack bar notifications.
        /// </summary>
        /// <remarks>The <see cref="ISnackbar"/> service allows for displaying brief, non-intrusive
        /// notifications  to the user, typically used for status updates or feedback. This property must be injected 
        /// and should not be null.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used to manage navigation within the application.
        /// </summary>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing city-related operations.
        /// </summary>
        [Inject] public ICityService CityService { get; set; } = null!;

        /// <summary>
        /// Deletes a city from the list after user confirmation.
        /// </summary>
        /// <remarks>This method displays a confirmation dialog to the user before proceeding with the
        /// deletion. If the user confirms, the city is removed from the data source and the local list of cities. The
        /// table data is then reloaded to reflect the changes.</remarks>
        /// <param name="cityId">The unique identifier of the city to be deleted. Cannot be null or empty.</param>
        private async Task DeleteCity(string cityId)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this city from the list?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };
    
            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;
    
            if (!result!.Canceled)
            {
                var deleteResult = await CityService.RemoveCityAsync(cityId);
                if (deleteResult.Succeeded)
                {
                    _cities.RemoveAll(c => c.CityId == cityId);
                    await _table.ReloadServerData();
                }
            }
        }
    
        /// <summary>
        /// Opens a dialog to add a new city and, if confirmed, creates the city and updates the city list.
        /// </summary>
        /// <remarks>This method displays a modal dialog for adding a new city. If the user confirms the
        /// dialog,  the city data is sent to the city service for creation. Upon successful creation, the new city  is
        /// added to the local city list, and the data table is reloaded.</remarks>
        /// <returns></returns>
        private async Task AddCity()
        {
            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                FullWidth = true,
                MaxWidth = MaxWidth.Medium
            };
    
            var parameters = new DialogParameters<CityModal>
            {
                { x => x.City, new CityViewModel() }
            };
    
            var dialog = await DialogService.ShowAsync<CityModal>("Confirm", parameters, options);
            var result = await dialog.Result;
    
            if (!result!.Canceled)
            {
                var dto = ((CityViewModel)result.Data!).ToDto();
                var creationResult = await CityService.CreateCityAsync(dto);
                if (creationResult.Succeeded)
                {
                    _cities.Add(dto);
                    await _table.ReloadServerData();
                }
            }
        }
    
        /// <summary>
        /// Updates the specified city by displaying a modal dialog for user confirmation and editing.
        /// </summary>
        /// <remarks>This method opens a modal dialog to allow the user to edit the details of the
        /// specified city.  If the user confirms the changes, the updated city data is sent to the city service for
        /// persistence.  Upon a successful update, the local city collection is updated, and the data table is
        /// reloaded.</remarks>
        /// <param name="city">The city to be updated, represented as a <see cref="CityDto"/> object.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task UpdateCity(CityDto city)
        {
            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                FullWidth = true,
                MaxWidth = MaxWidth.Medium
            };
    
            var parameters = new DialogParameters<CityModal>
            {
                { x => x.City, new CityViewModel(city) }
            };
    
            var dialog = await DialogService.ShowAsync<CityModal>("Confirm", parameters, options);
            var result = await dialog.Result;
    
            if (!result!.Canceled)
            {
                var index = _cities.IndexOf(city);
                var dto = ((CityViewModel)result.Data!).ToDto();
                var updateResult = await CityService.UpdateCityAsync(dto);
                if (updateResult.Succeeded)
                {
                    _cities[index] = dto;
                    await _table.ReloadServerData();
                }
            }
        }
    
        /// <summary>
        /// Asynchronously initializes the component and loads the list of cities.
        /// </summary>
        /// <remarks>This method sets up the page metadata, including breadcrumbs, and retrieves the list
        /// of cities  from the city service. If the data retrieval fails, error messages are displayed using the 
        /// snackbar service, and the initialization process is halted. The method also ensures that the  component's
        /// state is marked as loaded upon successful data retrieval.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var request = await CityService.AllCitiesAsync();
            if (request.Succeeded)
            {
                _cities = request.Data.ToList();
            }
            else
            {
                SnackBar.AddErrors(request.Messages);
                return;
            }
    
            _loaded = true;
            await base.OnInitializedAsync();
        }
    }
}
