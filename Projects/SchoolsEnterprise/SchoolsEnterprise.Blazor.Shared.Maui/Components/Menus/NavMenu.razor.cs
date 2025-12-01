using BusinessModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using System.Security.Claims;
using AdvertisingModule.Domain.Interfaces;
using AdvertisingModule.Domain.RequestFeatures;
using BusinessModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using IdentityModule.Domain.Extensions;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Components.Menus
{
    /// <summary>
    /// Represents a navigation menu component within the Maui/Blazor hybrid application.
    /// This menu provides navigation actions to different parts of the app and includes user-related functionality,
    /// such as signing out and viewing the user profile.
    /// </summary>
    public partial class NavMenu
    {
        private ClaimsPrincipal _user;
        private int listingCount;
        private int advertisementCount;
        private string _appVersion = string.Empty;

        /// <summary>
        /// Performs additional initialization tasks related to the application version.
        /// </summary>
        /// <remarks>This method is intended to be implemented in a partial class to provide custom logic
        /// for initializing application version-specific settings or behaviors. The implementation is optional and
        /// depends on the application's requirements.</remarks>
        partial void InitializeAppVersion();

        #region Cascading Parameters

        /// <summary>
        /// Provides access to the current authentication state, including the user’s identity and claims.
        /// Marked with <see cref="CascadingParameterAttribute"/> to receive the data from a higher-level component.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Event callback to notify when the user logs out.
        /// </summary>
        [Parameter] public EventCallback OnLogOut { get; set; }

        #endregion

        #region Injected Services

        /// <summary>
        /// Provides localized strings for the navigation menu component.
        /// </summary>
        [Inject] public IStringLocalizer<NavMenu> Localizer { get; set; } = null!;

        /// <summary>
        /// Gets or sets the NavigationManager used to manage and interact with URI navigation in the application.
        /// </summary>
        /// <remarks>The NavigationManager provides methods and properties for programmatically navigating
        /// to different URIs and for responding to navigation events. This property is typically set by the Blazor
        /// framework via dependency injection.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications to the user.
        /// </summary>
        [Inject] ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application configuration settings.
        /// </summary>
        /// <remarks>This property is typically populated by dependency injection and provides access to
        /// configuration values such as connection strings, application settings, and environment variables. Modifying
        /// this property at runtime may affect how the application retrieves configuration data.</remarks>
        [Inject] IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to query business directory information.
        /// </summary>
        [Inject] public IBusinessDirectoryQueryService BusinessDirectoryQueryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to query advertisements.
        /// </summary>
        [Inject] public IAdvertisementQueryService AdvertisementQueryService { get; set; } = null!;

        #endregion

        #region Navigation and User Actions

        /// <summary>
        /// Signs out the current user by removing any stored device token from preferences and then calling the 
        /// <see cref="AccountProvider.Logout"/> method to end the user’s session. Finally, navigates to the sign-in page.
        /// </summary>
        private async Task SignOut()
        {
            await OnLogOut.InvokeAsync();
        }

        /// <summary>
        /// Navigates to the user’s profile page.
        /// </summary>
        public void NavigateToProfilePage()
        {
            NavigationManager.NavigateTo("/profile", true);
        }

        /// <summary>
        /// Navigates the user to the application's dashboard.
        /// </summary>
        /// <remarks>This method redirects the user to the root URL ("/") of the application and forces a
        /// reload of the page.</remarks>
        public void NavigateToDashboard()
        {
            NavigationManager.NavigateTo("/", true);
        }

        /// <summary>
        /// Navigates the user to the "Make a Donation" page.
        /// </summary>
        /// <remarks>This method redirects the user to the root URL ("/") and forces a reload of the
        /// page.</remarks>
        public void NavigateToMakeADonation()
        {
            NavigationManager.NavigateTo("/donations/create", true);
        }

        /// <summary>
        /// Navigates to the communications center page, often used for messaging or announcements.
        /// </summary>
        public void NavigateToCommsCenterPage()
        {
            NavigationManager.NavigateTo("/commscenter", true);
        }

        /// <summary>
        /// Navigates to the communications center page, often used for messaging or announcements.
        /// </summary>
        public void NavigateToChatGroupPage()
        {
            NavigationManager.NavigateTo("/chatgroups", true);
        }

        /// <summary>
        /// Navigates to the main events page, displaying an overview of school or organizational events.
        /// </summary>
        public void NavigateToEventsPage()
        {
            NavigationManager.NavigateTo("/events", true);
        }

        /// <summary>
        /// Navigates the user to the event tickets page.
        /// </summary>
        /// <remarks>This method redirects the user to the "/events/tickets" URL and forces a reload of
        /// the page.</remarks>
        public void NavigateToEventTicketsPage()
        {
            NavigationManager.NavigateTo("/events/tickets", true);
        }

        #endregion

        /// <summary>
        /// Asynchronously initializes the component and loads data required for the page.
        /// </summary>
        /// <remarks>This method retrieves the current user's authentication state and uses it to fetch
        /// paginated data  for business listings and advertisements. The retrieved data is processed and displayed, and
        /// the  counts of listings and advertisements are updated accordingly.</remarks>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            InitializeAppVersion();

            var authState = await AuthenticationStateTask;
            _user = authState.User;

            if (authState.User.Identity.IsAuthenticated)
            {
                var parameters = new BusinessListingPageParameters() { UserId = _user.GetUserId(), PageSize = 100 };
                var result = await BusinessDirectoryQueryService.PagedListingsAsync(parameters);
                result.ProcessResponseForDisplay(SnackBar, () =>
                {
                    listingCount = result.Data.Count();
                });

                var addParameters = new AdvertisementListingPageParameters() { UserId = _user.GetUserId(), PageSize = 100 };
                var addResult = await AdvertisementQueryService.PagedListingsAsync(addParameters);

                addResult.ProcessResponseForDisplay(SnackBar, () =>
                {
                    advertisementCount = addResult.Data.Count();
                });
            }


            

            
        }
    }
}