using AdvertisingModule.Domain.DataTransferObjects;
using AdvertisingModule.Domain.Enums;
using AdvertisingModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Advertising
{
    /// <summary>
    /// The Categories component displays a loading state for a short period,
    /// then shows the category listings (or other content).
    /// It also allows navigation back to the commerce index or to a wallet page.
    /// </summary>
    public partial class AdvertisementTiers
    {
        public bool _loaded;
        private IEnumerable<AdvertisementTierDto> _tiers;

        /// <summary>
        /// Gets or sets the HTTP provider used to perform HTTP requests.
        /// </summary>
        /// <remarks>The HTTP provider is typically injected and used to manage HTTP communication within
        /// the application. Ensure that a valid implementation of <see cref="IBaseHttpProvider"/> is provided before
        /// using this property.</remarks>
        [Inject] public IAdvertisementTierQueryService AdvertisementTierQueryService { get; set; }

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications to the user.
        /// </summary>
        /// <remarks>This property is typically provided by dependency injection and allows components to
        /// show transient messages or alerts. The specific behavior and appearance of the snack bar depend on the
        /// implementation of the ISnackbar service.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; }

        /// <summary>
        /// Gets or sets the NavigationManager used to manage URI navigation and location state within the application.
        /// </summary>
        /// <remarks>This property is typically provided by dependency injection in Blazor applications.
        /// It enables programmatic navigation and access to the current URI.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Gets or sets the type of advertisement to be displayed.
        /// </summary>
        [Parameter] public AdvertisementType AdvertisementType { get; set; }

        /// <summary>
        /// Called by the Blazor runtime when the component initializes.
        /// Simulates a loading/data retrieval process via Task.Delay.
        /// </summary>
        protected override async Task OnInitializedAsync()
        { 
            var pagingResponse = await AdvertisementTierQueryService.AllAdvertisementTiersAsync(AdvertisementType);
            if (!pagingResponse.Succeeded)
            {
                SnackBar.AddErrors(pagingResponse.Messages);
            }
            else
            {
                _tiers = pagingResponse.Data;
            }
            _loaded = true;
        }

        /// <summary>
        /// Navigates the user back to the commerce index page.
        /// </summary>
        public void NavigateToCategoryPage(string tierId)
        {
            NavigationManager.NavigateTo($"/advertising/tiers/{tierId}");
        }

    }
}
