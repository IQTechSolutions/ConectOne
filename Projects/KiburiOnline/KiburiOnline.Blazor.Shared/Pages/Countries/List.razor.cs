using AccomodationModule.Application.ViewModels;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using LocationModule.Domain.DataTransferObjects;
using LocationModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Countries
{
    /// <summary>
    /// Represents a Blazor component for listing countries.
    /// </summary>
    public partial class List
    {
        private List<CountryDto> _countries = [];
        private MudTable<CountryDto> _table = null!;
        private bool _loaded;

        /// <summary>
        /// Gets or sets the service used to display dialogs to the user.
        /// </summary>
        /// <remarks>The <see cref="IDialogService"/> is typically used to show modal or non-modal
        /// dialogs, such as alerts, confirmations, or custom dialog components. This property is automatically injected
        /// by the dependency injection framework.</remarks>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used for handling navigation within the
        /// application.
        /// </summary>
        /// <remarks>This property is typically injected by the framework and should not be set manually
        /// in most scenarios.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing country-related operations.
        /// </summary>
        [Inject] public ICountryService CountryService { get; set; } = null!;

        /// <summary>
        /// Deletes a country from the list after user confirmation.
        /// </summary>
        /// <remarks>This method displays a confirmation dialog to the user before proceeding with the
        /// deletion.  If the user confirms, the country is removed from the data source and the UI is updated to
        /// reflect the change.</remarks>
        /// <param name="countryId">The unique identifier of the country to be deleted. Cannot be null or empty.</param>
        /// <returns></returns>
        private async Task DeleteCountry(string countryId)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this country from the list?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var deleteResult = await CountryService.RemoveCountryAsync(countryId);
                if (deleteResult.Succeeded)
                {
                    _countries.RemoveAll(c => c.CountryId == countryId);
                    await _table.ReloadServerData();
                }
            }
        }

        /// <summary>
        /// Displays a dialog for adding a new country and processes the result.
        /// </summary>
        /// <remarks>This method opens a modal dialog to collect information about a new country. If the
        /// user confirms the dialog,  the country data is sent to the country service for creation. Upon successful
        /// creation, the new country is  added to the internal collection and the data table is reloaded.</remarks>
        /// <returns></returns>
        private async Task AddCountry()
        {
            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                FullWidth = true,
                MaxWidth = MaxWidth.Medium
            };

            var parameters = new DialogParameters<CountryModal>
            {
                { x => x.Country, new CountryViewModel() }
            };

            var dialog = await DialogService.ShowAsync<CountryModal>("Confirm", options);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var dto = ((CountryViewModel)result.Data!).ToDto();
                var creationResult = await CountryService.CreateCountryAsync(dto);
                if (creationResult.Succeeded)
                {
                    _countries.Add(dto);
                    await _table.ReloadServerData();
                }
            }
        }

        /// <summary>
        /// Opens a dialog to update the details of a specified country. If the update is confirmed, the country data is
        /// updated and the table is reloaded.
        /// </summary>
        /// <remarks>This method displays a modal dialog allowing the user to edit the details of the
        /// specified country. If the user confirms the changes, the updated country data is sent to the service for
        /// persistence. The table displaying the list of countries is reloaded to reflect the changes.</remarks>
        /// <param name="country">The <see cref="CountryDto"/> object representing the country to be updated. Cannot be null.</param>
        /// <returns></returns>
        private async Task UpdateCountry(CountryDto country)
        {
            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                FullWidth = true,
                MaxWidth = MaxWidth.Medium
            };

            var parameters = new DialogParameters<CountryModal>
            {
                { x => x.Country, new CountryViewModel(country) }
            };

            var dialog = await DialogService.ShowAsync<CountryModal>("Confirm", parameters, options);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var index = _countries.IndexOf(country);
                var dto = ((CountryViewModel)result.Data!).ToDto();
                var updateResult = await CountryService.UpdateCountryAsync(dto);
                if (updateResult.Succeeded)
                {
                    _countries[index] = dto;
                    await _table.ReloadServerData();
                }
            }
        }

        /// <summary>
        /// Initializes the component asynchronously, setting up page metadata, retrieving the list of countries,  and
        /// updating the component's state based on the retrieved data.
        /// </summary>
        /// <remarks>This method configures the page details, including breadcrumbs, and fetches a list of
        /// countries  from the data provider. If the data retrieval is successful, the list of countries is stored
        /// locally  and the component is marked as loaded. If the data retrieval fails, error messages are displayed 
        /// using the snackbar service, and the initialization process is halted.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var request = await CountryService.AllCountriesAsync();
            if (request.Succeeded)
            {
                _countries = request.Data.ToList();
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
