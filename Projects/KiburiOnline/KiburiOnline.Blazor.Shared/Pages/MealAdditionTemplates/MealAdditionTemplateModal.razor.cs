using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.MealAdditionTemplates;

/// <summary>
/// Modal dialog for creating or editing meal addition templates.
/// </summary>
public partial class MealAdditionTemplateModal
{
    /// <summary>
    /// Gets the current instance of the dialog being managed by this component.
    /// </summary>
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

    /// <summary>
    /// Gets or sets the template used to define the structure and details of a meal addition.
    /// </summary>
    [Parameter] public MealAdditionTemplateViewModel Template { get; set; } = new MealAdditionTemplateViewModel();

    /// <summary>
    /// Gets or sets the service used to perform restaurant-related operations.
    /// </summary>
    [Inject] public IRestaurantService RestaurantService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the injected instance of the <see cref="ISnackbar"/> service used to display notifications or
    /// messages to the user.
    /// </summary>
    [Inject] public ISnackbar SnackBar { get; set; } = null!;

    /// <summary>
    /// Gets or sets the collection of restaurants.
    /// </summary>
    private List<RestaurantDto> Restaurants { get; set; } = [];

    /// <summary>
    /// Converts a <see cref="RestaurantDto"/> object to its string representation, typically the restaurant's name.
    /// </summary>
    /// <remarks>If the provided <see cref="RestaurantDto"/> is null, the converter returns an empty
    /// string.</remarks>
    private readonly Func<RestaurantDto?, string> _restaurantDtoConverter = p => p?.Name ?? "";

    /// <summary>
    /// Saves the current template and closes the dialog.
    /// </summary>
    /// <remarks>This method finalizes the current operation by saving the template and closing the associated
    /// dialog. Ensure that the template is properly initialized before calling this method.</remarks>
    private void SaveAsync()
    {
        MudDialog.Close(Template);
    }

    /// <summary>
    /// Cancels the current dialog operation.
    /// </summary>
    /// <remarks>This method terminates the dialog and triggers any cancellation logic associated with it. Use
    /// this method to programmatically close a dialog when a cancellation is required.</remarks>
    public void Cancel()
    {
        MudDialog.Cancel();
    }

    /// <summary>
    /// Asynchronously initializes the component and retrieves a list of restaurants from the data provider.
    /// </summary>
    /// <remarks>This method fetches restaurant data from the specified endpoint and updates the <see
    /// cref="Restaurants"/> collection  if the operation succeeds. If the operation fails, error messages are displayed
    /// using the <see cref="SnackBar"/> service.</remarks>
    /// <returns>A task that represents the asynchronous initialization operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        var response = await RestaurantService.AllRestaurantsAsync();
        if (response.Succeeded)
        {
            Restaurants = response.Data.ToList();
        }
        else
        {
            SnackBar.AddErrors(response.Messages);
        }
        await base.OnInitializedAsync();
    }
}
