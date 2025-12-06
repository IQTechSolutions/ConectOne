using AccomodationModule.Domain.DataTransferObjects;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Vacations
{
    /// <summary>
    /// Component for displaying and managing itineraries associated with a vacation.
    /// </summary>
    public partial class Itinerary
    {
        /// <summary>
        /// Gets or sets the injected <see cref="ISnackbar"/> service used to display notifications or messages.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        #region Parameters

        /// <summary>
        /// The ID of the vacation to which the itineraries belong.
        /// </summary>
        [Parameter, EditorRequired] public string VacationId { get; set; } = null!;

        /// <summary>
        /// The URL of the banner image to display.
        /// </summary>
        [Parameter] public string BannerImageUrl { get; set; } = null!;

        #endregion

        #region Private Fields

        /// <summary>
        /// List to hold the itineraries associated with the vacation.
        /// </summary>
        private List<ItineraryDto> _itineraries = new();

        #endregion

        #region Overrides

        /// <summary>
        /// Initializes the component and loads the itineraries associated with the vacation.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            // Fetch the itineraries associated with the vacation
            //var inclusionResult = await Provider.GetAsync<IEnumerable<ItineraryDto>>($"vacations/itineraries/{VacationId}");
            //inclusionResult.ProcessResponseForDisplay(SnackBar, () =>
            //{
            //    // Populate the itineraries list with the fetched data
            //    _itineraries = inclusionResult.Data.OrderBy(c => c.Date).ToList();
            //});

            await base.OnInitializedAsync();
        }

        #endregion
    }
}