using AdvertisingModule.Domain.DataTransferObjects;
using AdvertisingModule.Domain.Interfaces;
using AdvertisingModule.Domain.RequestFeatures;
using ConectOne.Blazor.Extensions;
using IdentityModule.Domain.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Advertising
{
    /// <summary>
    /// Represents a business directory component that manages navigation and category data retrieval.
    /// </summary>
    /// <remarks>This class is designed to interact with a backend service to retrieve paginated category data
    /// and provides functionality for navigating to specific pages within the application.</remarks>
    public partial class AdvertisementDirectory
    {
        private ICollection<AdvertisementDto> _advertisements = new List<AdvertisementDto>();

        /// <summary>
        /// The user's authentication state is cascaded from a higher-level component.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to query the business directory via REST API.
        /// </summary>
        [Inject] public IAdvertisementQueryService AdvertisementQueryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application configuration settings.
        /// </summary>
        /// <remarks>The configuration provides access to key-value pairs and other settings used to
        /// control application behavior. This property is typically populated by the dependency injection
        /// system.</remarks>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications to the user.
        /// </summary>
        /// <remarks>This property is typically provided via dependency injection. Use the snack bar
        /// service to show transient messages or alerts within the application's user interface.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Asynchronously initializes the component by retrieving a paged list of categories and handling the response.
        /// </summary>
        /// <remarks>This method fetches a paged list of categories from the specified provider and
        /// updates the local category data.  If the operation fails, error messages are displayed using the configured
        /// snackbar.</remarks>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            var _user = authState.User;

            var parameters = new AdvertisementListingPageParameters(){ UserId = _user.GetUserId(), PageSize = 100};
            
            var pagingResponse = await AdvertisementQueryService.PagedListingsAsync(parameters);
            if (!pagingResponse.Succeeded)
            {
                SnackBar.AddErrors(pagingResponse.Messages);
            }
            _advertisements = pagingResponse.Data;

            await base.OnInitializedAsync();
        }
    }
}
