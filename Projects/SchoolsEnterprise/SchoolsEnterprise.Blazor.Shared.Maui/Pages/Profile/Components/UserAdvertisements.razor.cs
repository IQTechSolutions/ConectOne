using AdvertisingModule.Domain.DataTransferObjects;
using AdvertisingModule.Domain.Interfaces;
using AdvertisingModule.Domain.RequestFeatures;
using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Profile.Components
{
    /// <summary>
    /// Represents a component that displays advertisements associated with a specific user.
    /// </summary>
    /// <remarks>This component fetches and displays advertisements for the specified user. It supports an
    /// optional feature to display top-performing advertisements. The data is retrieved asynchronously from a remote
    /// service using the provided HTTP provider.</remarks>
    public partial class UserAdvertisements
    {
        private IEnumerable<AdvertisementDto> _adds = [];

        /// <summary>
        /// Gets or sets the HTTP provider used to perform HTTP requests.
        /// </summary>
        /// <remarks>The provider is typically injected and used to abstract HTTP communication, allowing
        /// for easier testing and flexibility. Ensure that a valid implementation of <see cref="IBaseHttpProvider"/> is
        /// provided before using this property.</remarks>
        [Inject] public IAdvertisementQueryService AdvertisementQueryService { get; set; }

        /// <summary>
        /// Gets or sets the service used to display transient messages to the user.
        /// </summary>
        /// <remarks>Use this property to show notifications, alerts, or status messages in the
        /// application's user interface. The implementation of ISnackbar determines how messages are presented and may
        /// provide options for message duration, styling, or actions.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; }

        /// <summary>
        /// Gets or sets the NavigationManager used to manage URI navigation and location state within the application.
        /// </summary>
        /// <remarks>The NavigationManager provides methods and properties for programmatic navigation and
        /// for retrieving information about the current URI. This property is typically set by dependency injection in
        /// Blazor components.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        [Parameter] public string UserId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the top performers should be displayed.
        /// </summary>
        [Parameter] public bool DisplayTopPerformers { get; set; } = false;

        /// <summary>
        /// Asynchronously initializes the component and retrieves a paginated list of advertisements for the specified
        /// user.
        /// </summary>
        /// <remarks>This method fetches advertisement data using the provided user ID and processes the
        /// response for display.  If the operation is successful, the retrieved advertisements are stored for
        /// rendering.  Additionally, this method invokes the base class's initialization logic.</remarks>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            var parameters = new AdvertisementListingPageParameters() { UserId = UserId };
            var result = await AdvertisementQueryService.PagedListingsAsync(parameters);
            result.ProcessResponseForDisplay(SnackBar, () =>
            {
                _adds = result.Data;
            });

            await base.OnInitializedAsync();
        }
    }
}
