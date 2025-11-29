using AdvertisingModule.Domain.DataTransferObjects;
using AdvertisingModule.Domain.Enums;
using AdvertisingModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;
using Radzen.Blazor;

namespace AdvertisingModule.Blazor.Components.Mobile
{
    /// <summary>
    /// Represents a component for adding a carousel widget to the application.
    /// </summary>
    /// <remarks>This component provides functionality to navigate to a specified URL for adding carousel
    /// widgets. It uses a sliding transition effect internally.</remarks>
    public partial class AddCarouselWidget
    {
        private List<AdvertisementDto> _ads = new();
        private RadzenCarousel _carousel = null!;
        private RadzenCarousel _carousel2 = null!;
        private double _interval = 4000;
        private int _selectedIndex = 0;

        /// <summary>
        /// Gets the number of groupings to add, based on the current count of ads.
        /// </summary>
        private int AddGroupingCount => (int)Math.Ceiling((double)(_ads.Count / 8));

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used for managing navigation and URI manipulation.
        /// </summary>
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        [Inject] private IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to query advertisements.
        /// </summary>
        [Inject] private IAdvertisementQueryService AdvertisementQueryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the injected <see cref="ISnackbar"/> service used to display notifications or messages to the
        /// user.
        /// </summary>
        [Inject] private ISnackbar Snackbar { get; set; } = null!;

        /// <summary>
        /// Represents the default transition type used for animations.
        /// </summary>
        /// <remarks>The transition type is set to <see cref="Transition.Slide"/> by default. This field
        /// is read-only and cannot be modified after initialization.</remarks>
        private readonly Transition _transition = Transition.Slide;

        /// <summary>
        /// Navigates to the specified URL and forces a reload of the page.
        /// </summary>
        /// <remarks>This method uses the <see cref="NavigationManager"/> to perform the navigation. The
        /// <paramref name="url"/> parameter should represent a valid absolute or relative URL. Passing an invalid or
        /// null URL will result in undefined behavior.</remarks>
        /// <param name="url">The URL to navigate to. Must be a valid, non-null string.</param>
        private async Task NavigateToAdvertiseWithUs()
        {
            NavigationManager.NavigateTo($"/advertising/tiers/{(int)AdvertisementType.Website}");
        }

        /// <summary>
        /// Asynchronously initializes the component and retrieves active advertisements.
        /// </summary>
        /// <remarks>This method fetches a list of active advertisements from the provider and updates the
        /// component's state accordingly.  If the retrieval fails, error messages are displayed using the Snackbar
        /// service.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var advertisementResult = await AdvertisementQueryService.ActiveAdvertisementsAsync();
            if (advertisementResult.Succeeded)
                _ads = advertisementResult.Data.ToList();
            else
                Snackbar.AddErrors(advertisementResult.Messages);
            
            await InvokeAsync(StateHasChanged);
            await base.OnInitializedAsync();
        }
    }
}
