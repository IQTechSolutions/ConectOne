using AdvertisingModule.Domain.DataTransferObjects;
using AdvertisingModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AdvertisingModule.Blazor.Components.Mobile
{
    /// <summary>
    /// Represents a sidebar component that displays a carousel of advertisements.
    /// </summary>
    /// <remarks>This component retrieves advertisements from an injected <see
    /// cref="IAdvertisementQueryService"/>  and displays them in a carousel format. The carousel automatically cycles
    /// through the advertisements  at a configurable interval. Users can navigate to a specific advertisement by
    /// clicking on it.</remarks>
    public partial class SideBarAddCarousel
    {
        private List<AdvertisementDto> _ads = new();
        private double _interval = 4000;
        private int _selectedIndex = 0;

        /// <summary>
        /// Gets or sets the service used to query advertisements.
        /// </summary>
        [Inject] public IAdvertisementQueryService AdvertisementQueryService { get; set; }

        /// <summary>
        /// Gets or sets the service used to display snackbars for user notifications.
        /// </summary>
        /// <remarks>The <see cref="ISnackbar"/> service is typically used to display transient messages
        /// or alerts to the user. Ensure that the service is properly initialized before attempting to use
        /// it.</remarks>
        [Inject] public ISnackbar Snackbar { get; set; }

        /// <summary>
        /// Navigates to the specified URL and forces the browser to reload the page.
        /// </summary>
        /// <remarks>The navigation will bypass client-side routing and trigger a full page
        /// reload.</remarks>
        /// <param name="url">The URL to navigate to. This must be a valid, absolute or relative URL.</param>
        private void NavigateToAdd(string url)
        {
            NavigationManager.NavigateTo(url, true);
        }

        /// <summary>
        /// Gets the number of groupings to add, calculated based on the total count of ads.
        /// </summary>
        private int AddGroupingCount => (int)Math.Ceiling((double)(_ads.Count / 4));

        /// <summary>
        /// Asynchronously initializes the component and retrieves advertisement data.
        /// </summary>
        /// <remarks>This method fetches all advertisements using the <see
        /// cref="AdvertisementQueryService"/>. If the operation succeeds, the advertisements are stored in the local
        /// collection. Otherwise, error messages are displayed using the <see cref="Snackbar"/> service.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var advertisementResult = await AdvertisementQueryService.AllAdvertisementsAsync();
            if (advertisementResult.Succeeded)
                _ads = advertisementResult.Data.ToList();
            else
                Snackbar.AddErrors(advertisementResult.Messages);

            await base.OnInitializedAsync();
        }
    }
}
