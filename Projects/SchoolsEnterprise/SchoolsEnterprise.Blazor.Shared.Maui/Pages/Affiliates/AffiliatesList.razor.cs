using AdvertisingModule.Domain.DataTransferObjects;
using AdvertisingModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;
using Radzen.Blazor;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Affiliates
{
    /// <summary>
    /// Represents a component for adding a carousel widget to the application.
    /// </summary>
    /// <remarks>This component provides functionality to navigate to a specified URL for adding carousel
    /// widgets. It uses a sliding transition effect internally.</remarks>
    public partial class AffiliatesList
    {
        private List<AffiliateDto> _ads = new();
        private RadzenCarousel _carousel = null!;
        private RadzenCarousel _carousel2 = null!;
        private double _interval = 4000;
        private int _selectedIndex = 0;

        /// <summary>
        /// Gets the number of groupings to be added, calculated based on the total count of ads.
        /// </summary>
        private int AddGroupingCount => (int)Math.Ceiling((double)(_ads.Count / 8));

        /// <summary>
        /// Gets or sets the service used to query affiliate-related data.
        /// </summary>
        [Inject] private IAffiliateQueryService AffiliateQueryService { get; set; }

        /// <summary>
        /// Gets or sets the injected <see cref="ISnackbar"/> service used to display notifications or messages to the
        /// user.
        /// </summary>
        [Inject] private ISnackbar Snackbar { get; set; }

        /// <summary>
        /// Gets or sets the NavigationManager used to manage URI navigation and location state within the application.
        /// </summary>
        /// <remarks>This property is typically injected by the Blazor framework to enable programmatic
        /// navigation and to access the current URI. It should not be set manually outside of dependency injection
        /// scenarios.</remarks>
        [Inject] private NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        /// <remarks>The configuration provides access to key-value pairs and other settings used to
        /// control application behavior. This property is typically populated by dependency injection.</remarks>
        [Inject] private IConfiguration Configuration { get; set; }

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
        private async Task NavigateToAdd(string url)
        {
            NavigationManager.NavigateTo(url, true);
        }

        /// <summary>
        /// Asynchronously initializes the component and retrieves active advertisements.
        /// </summary>
        /// <remarks>This method fetches a list of active advertisements from the provider and updates the
        /// component's state accordingly.  If the retrieval fails, error messages are displayed using the Snackbar. The
        /// method also ensures the component's state  is refreshed after initialization.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var advertisementResult = await AffiliateQueryService.AllAffiliatesAsync();
            if (advertisementResult.Succeeded)
                _ads = advertisementResult.Data.ToList();
            else
                Snackbar.AddErrors(advertisementResult.Messages);
            
            await InvokeAsync(StateHasChanged);
            await base.OnInitializedAsync();
        }
    }
}
