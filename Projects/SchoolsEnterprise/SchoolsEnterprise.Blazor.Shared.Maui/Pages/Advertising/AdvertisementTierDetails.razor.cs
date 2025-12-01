using AdvertisingModule.Domain.DataTransferObjects;
using AdvertisingModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;
using ProductsModule.Domain.DataTransferObjects;
using ShoppingModule.Blazor.Components;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Advertising
{
    /// <summary>
    /// Represents the details of an advertisement tier, including functionality for loading tier data and managing user
    /// interactions.
    /// </summary>
    /// <remarks>This class is a Blazor component that interacts with an HTTP provider to retrieve
    /// advertisement tier details and provides methods for user actions such as adding the tier to a cart. Ensure that
    /// the <see cref="Provider"/> property is set before invoking methods that depend on HTTP communication.</remarks>
    public partial class AdvertisementTierDetails
    {
        public bool _loaded;
        private AdvertisementTierDto _tier;

        /// <summary>
        /// Gets or sets the <see cref="CartStateProvider"/> instance used to manage the state of the shopping cart.
        /// </summary>
        [CascadingParameter] public CartStateProvider ShoppingCartService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to query advertisement tiers.
        /// </summary>
        [Inject] public IAdvertisementTierQueryService AdvertisementTierQueryService { get; set; }

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications in the user interface.
        /// </summary>
        /// <remarks>This property is typically provided by dependency injection and allows components to
        /// show transient messages to users. The specific behavior and appearance of the snack bar depend on the
        /// implementation of the ISnackbar service.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; }

        /// <summary>
        /// Gets or sets the application configuration settings.
        /// </summary>
        /// <remarks>The configuration provides access to key-value pairs and other settings used to
        /// configure the application's behavior. This property is typically populated by dependency
        /// injection.</remarks>
        [Inject] public IConfiguration Configuration { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the tier.
        /// </summary>
        [Parameter] public string TierId { get; set; }

        /// <summary>
        /// Called by the Blazor runtime when the component initializes.
        /// Simulates a loading/data retrieval process via Task.Delay.
        /// </summary>
        protected override async Task OnInitializedAsync()
        { 
            var pagingResponse = await AdvertisementTierQueryService.AdvertisementTierAsync(TierId);
            if (!pagingResponse.Succeeded)
            {
                SnackBar.AddErrors(pagingResponse.Messages);
            }
            else
            {
                _tier = pagingResponse.Data;
            }
            _loaded = true;
        }

        /// <summary>
        /// Adds the specified product to the shopping cart asynchronously.
        /// </summary>
        /// <remarks>If the operation fails, error messages are displayed using the snack bar.  On
        /// success, a confirmation message is displayed.</remarks>
        /// <param name="dto">The product to add, represented as a <see cref="ProductDto"/> object. Must not be <see langword="null"/>.</param>
        /// <param name="qty">The quantity of the product to add. Must be greater than zero.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task AddToCartAsync()
        {
            var result = await ShoppingCartService.AddToCartAsync(_tier);
            result.ProcessResponseForDisplay(SnackBar, () =>
            {
                SnackBar.AddSuccess($"Package '{_tier.Name} successfully added to cart'");
            });
        }

    }
}
