using ConectOne.Domain.Interfaces;
using LocationModule.Application.ViewModels;
using LocationModule.Domain.DataTransferObjects;
using LocationModule.Domain.Entities;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Cities;

/// <summary>
/// Represents a modal dialog for managing city information, including its associated country.
/// </summary>
/// <remarks>This class is designed to be used within a Blazor application and provides functionality for
/// displaying and editing city details. It interacts with a dialog instance to handle user actions such as saving or
/// canceling changes. The modal also retrieves and displays a list of available countries for selection.</remarks>
public partial class CityModal
{
    private readonly Func<CountryDto?, string?> _countryConverter = c => c?.Name;

    /// <summary>
    /// Gets the current instance of the dialog, allowing interaction with the dialog's lifecycle.
    /// </summary>
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

    /// <summary>
    /// Gets or sets the city information represented by a <see cref="CityViewModel"/> instance.
    /// </summary>
    [Parameter] public CityViewModel City { get; set; } = new CityViewModel();

    /// <summary>
    /// Gets or sets the HTTP provider used for making HTTP requests.
    /// </summary>
    [Inject] public IBaseHttpProvider Provider { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service used to display snackbars for user notifications.
    /// </summary>
    [Inject] public ISnackbar Snackbar { get; set; } = null!;

    /// <summary>
    /// Gets or sets the collection of countries.
    /// </summary>
    public IEnumerable<CountryDto> Countries { get; set; } = new List<CountryDto>();

    /// <summary>
    /// Saves the current city data and closes the dialog if the operation is successful.
    /// </summary>
    /// <remarks>Ensures that the city's country information is set before saving. If the country is not
    /// specified,  a message is displayed to the user, and the operation is aborted. Otherwise, the dialog is closed 
    /// with the city data.</remarks>
    private void SaveAsync()
    {
        if (City.Country is not null)
        {
            City.CountryId = City.Country.CountryId;
        }

        if (string.IsNullOrEmpty(City.CountryId))
        {
            Snackbar.Add("Country is required");
            return;
        }

        MudDialog.Close(City);
    }

    /// <summary>
    /// Cancels the current dialog operation.
    /// </summary>
    /// <remarks>This method terminates the dialog and triggers the cancellation logic.  It is typically used
    /// to close the dialog without completing the operation  or returning a result.</remarks>
    public void Cancel()
    {
        MudDialog.Cancel();
    }

    /// <summary>
    /// Asynchronously initializes the component and loads the list of countries.
    /// </summary>
    /// <remarks>This method retrieves a collection of countries from the data provider and populates the 
    /// <see cref="Countries"/> property. If the <see cref="LocationModule.Domain.Entities.City.CountryId"/> property is set, the  corresponding
    /// country is assigned to <see cref="City.Country"/>. Finally, the base class  implementation of <see
    /// cref="OnInitializedAsync"/> is invoked.</remarks>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        var countriesResult = await Provider.GetAsync<IEnumerable<CountryDto>>("countries");
        if (countriesResult.Succeeded)
        {
            Countries = countriesResult.Data.ToList();
            if (City.CountryId is not null)
            {
                City.Country = Countries.FirstOrDefault(c => c.CountryId == City.CountryId);
            }
        }

        await base.OnInitializedAsync();
    }
}
