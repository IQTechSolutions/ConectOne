using AdvertisingModule.Domain.DataTransferObjects;
using AdvertisingModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;
using Radzen.Blazor;

namespace AdvertisingModule.Blazor.Components.Mobile
{
    /// <summary>
    /// Represents a UI component for displaying and managing affiliate advertisements.
    /// </summary>
    /// <remarks>The <see cref="AffiliateWidget"/> component is designed to display a carousel of
    /// advertisements and provide navigation functionality. It integrates with various services, such as <see
    /// cref="IAdvertisementQueryService"/> for fetching advertisements, <see cref="ISnackbar"/> for displaying
    /// notifications, and <see cref="NavigationManager"/> for handling navigation.</remarks>
    public partial class AffiliateWidget
    {
        private RadzenCarousel _carousel = null!;
        private RadzenCarousel _carousel2 = null!;
        private int _selectedIndex;
        private int AddGroupingCount => (int)Math.Ceiling((double)(Affiliates.Count / 8));

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
        [Inject] private IAffiliateQueryService AffiliateQueryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the injected <see cref="ISnackbar"/> service used to display notifications or messages to the
        /// user.
        /// </summary>
        [Inject] private ISnackbar Snackbar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the number of items to display.
        /// </summary>
        [Parameter] public int DisplayRowCount { get; set; } = 6;

        /// <summary>
        /// Gets or sets the interval, in milliseconds, used for timing operations.
        /// </summary>
        [Parameter] public int Interval { get; set; } = 4000;

        /// <summary>
        /// Gets or sets the list of affiliates associated with the current entity.
        /// </summary>
        [Parameter] public List<AffiliateDto> Affiliates { get; set; } = [];

        /// <summary>
        /// Represents the default transition type used for animations.
        /// </summary>
        /// <remarks>The transition type is set to <see cref="Transition.Slide"/> by default. This field
        /// is read-only and cannot be modified after initialization.</remarks>
        private readonly Transition _transition = Transition.Slide;

        /// <summary>
        /// Asynchronously initializes the component and retrieves active advertisements.
        /// </summary>
        /// <remarks>This method fetches a list of active advertisements from the provider and updates the
        /// component's state accordingly.  If the retrieval fails, error messages are displayed using the Snackbar
        /// service.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var advertisementResult = await AffiliateQueryService.AllAffiliatesAsync();
            if (advertisementResult.Succeeded)
                Affiliates = advertisementResult.Data.ToList();
            else
                Snackbar.AddErrors(advertisementResult.Messages);
            
            await InvokeAsync(StateHasChanged);
            await base.OnInitializedAsync();
        }
    }
}
