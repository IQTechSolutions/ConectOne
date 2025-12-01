using BloggingModule.Domain.Interfaces;
using BusinessModule.Domain.DataTransferObjects;
using BusinessModule.Domain.Interfaces;
using BusinessModule.Domain.RequestFeatures;
using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Enums;
using GroupingModule.Domain.DataTransferObjects;
using GroupingModule.Domain.RequestFeatures;
using IdentityModule.Domain.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MudBlazor;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages
{
    /// <summary>
    /// The Home component acts as a landing page or main screen for the application.
    /// It displays featured blog categories and provides quick navigation buttons.
    /// </summary>
    public partial class Home
    {
        private List<CategoryDto>? _featuredCategories = [];
        private List<BusinessListingDto> _userListings = [];
        private List<BusinessListingDto> _expiringListings = [];
        private static readonly TimeSpan ExpiringSoonThreshold = TimeSpan.FromDays(14);

        #region Injected Services

        /// <summary>
        /// The current authentication state, used to determine the logged-in user's identity.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;
        
        /// <summary>
        /// Gets or sets the service used to manage blog post categories.
        /// </summary>
        [Inject] public IBlogPostCategoryService BlogCategoryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to query business directory information.
        /// </summary>
        [Inject] public IBusinessDirectoryQueryService BusinessDirectoryQueryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to execute business directory commands.
        /// </summary>
        [Inject] public IBusinessDirectoryCommandService BusinessDirectoryCommandService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the NavigationManager used to manage and interact with URI navigation in the application.
        /// </summary>
        /// <remarks>This property is typically provided by dependency injection in Blazor applications.
        /// Use it to programmatically navigate to different URIs or to access information about the current navigation
        /// state.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Injected Logger for logging errors and information.
        /// </summary>
        [Inject] public ILogger<Home> Logger { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications to the user.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Provides localized strings for the Home page.
        /// </summary>
        [Inject] public IStringLocalizer<Home> Localizer { get; set; } = null!;

        #endregion

        #region Navigation Methods

        /// <summary>
        /// Navigates the user to the communication center page ("/commscenter").
        /// This typically might be used for messaging, announcements, or a chat area.
        /// </summary>
        public void NavigateToCommCenter()
        {
            NavigationManager.NavigateTo($"/commscenter");
        }

        /// <summary>
        /// Navigates the user to a calendar page ("/calendar"), presumably for viewing events,
        /// schedules, or date-based items.
        /// </summary>
        public void NavigateToCalendar()
        {
            NavigationManager.NavigateTo("/calendar");
        }

        /// <summary>
        /// Navigates the user to an external URL using a direct string. Here it points
        /// to a payment/market site. This approach leaves the Blazor app and opens
        /// a new page or tab (depending on the environment).
        /// </summary>
        public void NavigateToClickMall()
        {
            NavigationManager.NavigateTo(
                "https://affiliation.software/flysafair/c68198"
            );
        }

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Executes after the component has rendered and handles the initial data load on the first render.
        /// </summary>
        /// <param name="firstRender">Indicates whether this is the first time the component is rendered.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                try
                {
                    var authState = await AuthenticationStateTask;
                    var user = authState.User;
                    var featuredCategoriesResult = await BlogCategoryService.PagedCategoriesAsync(new CategoryPageParameters() { PageSize = 100, Featured = true });

                    // If the HTTP request or data fetch succeeded, store the categories.
                    if (featuredCategoriesResult.Succeeded)
                        _featuredCategories = featuredCategoriesResult.Data;

                    if (user.Identity?.IsAuthenticated == true)
                    {
                        var listingParameters = new BusinessListingPageParameters
                        {
                            UserId = user.GetUserId(),
                            Status = ReviewStatus.Approved,
                            PageSize = 100
                        };

                        var listingsResult = await BusinessDirectoryQueryService.PagedListingsAsync(listingParameters);
                        listingsResult.ProcessResponseForDisplay(SnackBar, () =>
                        {
                            _userListings = listingsResult.Data?.ToList() ?? [];
                            RefreshExpiringListings();
                        });
                    }

                    StateHasChanged();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "An error occurred while fetching featured category count.");
                    SnackBar.Add(Localizer["ErrorFetchingFeaturedCategories"]);
                }
            }
        }

        #endregion

        #region Listing Helpers

        /// <summary>
        /// Refreshes the list of user listings that are expiring soon.
        /// </summary>
        /// <remarks>This method filters the user's listings to include only those that are expiring
        /// within the  specified threshold, sorts them by their expiration date in ascending order, and updates  the
        /// internal collection of expiring listings.</remarks>
        private void RefreshExpiringListings()
        {
            _expiringListings = _userListings
                .Where(l => l.IsExpiringWithin(ExpiringSoonThreshold))
                .OrderBy(l => l.ActiveUntilUtc ?? DateTime.MaxValue)
                .ToList();
        }

        /// <summary>
        /// Renews the specified business listing by sending a renewal request to the server.
        /// </summary>
        /// <remarks>Upon successful renewal, the updated listing is either added to the user's listings
        /// or replaces the existing entry. This method also refreshes the list of expiring listings and displays a
        /// success message.</remarks>
        /// <param name="listing">The business listing to renew. The <paramref name="listing"/> must not be null and must have a valid
        /// <c>Id</c>.</param>
        private async Task RenewListingAsync(BusinessListingDto listing)
        {
            var result = await BusinessDirectoryCommandService.RenewAsync(listing.Id);
            result.ProcessResponseForDisplay(SnackBar, () =>
            {
                if (result.Data is null)
                    return;

                var index = _userListings.FindIndex(l => l.Id == result.Data.Id);
                if (index >= 0)
                    _userListings[index] = result.Data;
                else
                    _userListings.Add(result.Data);

                RefreshExpiringListings();
                SnackBar.Add($"Listing \"{result.Data.Heading}\" renewed successfully.", Severity.Success);
                StateHasChanged();
            });
        }

        #endregion
    }
}
