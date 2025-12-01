using BusinessModule.Domain.DataTransferObjects;
using BusinessModule.Domain.Interfaces;
using BusinessModule.Domain.RequestFeatures;
using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Profile.Components
{
    /// <summary>
    /// Represents a component that displays a collection of business listings associated with a specific user.
    /// </summary>
    /// <remarks>This component fetches and displays business listings for the specified user. It supports an
    /// optional feature to highlight top-performing listings. The data is retrieved asynchronously using the provided
    /// HTTP provider.</remarks>
    public partial class UserListings
    {
        private List<BusinessListingDto> _listings = [];
        private static readonly TimeSpan ExpiringSoonThreshold = TimeSpan.FromDays(14);

        /// <summary>
        /// Gets or sets the HTTP provider used to perform HTTP requests.
        /// </summary>
        /// <remarks>The provider is typically injected and used to abstract HTTP communication, allowing
        /// for easier testing and dependency management.</remarks>
        [Inject] public IBusinessDirectoryQueryService Provider { get; set; }

        /// <summary>
        /// Gets or sets the command service used to execute business directory operations.
        /// </summary>
        [Inject] public IBusinessDirectoryCommandService CommandProvider { get; set; }

        /// <summary>
        /// Gets or sets the service used to display transient messages to the user.
        /// </summary>
        /// <remarks>Use this property to show notifications, alerts, or status messages in the
        /// application's user interface. The implementation of ISnackbar determines how messages are displayed and
        /// managed.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; }

        /// <summary>
        /// Gets or sets the application configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; }

        /// <summary>
        /// Gets or sets the NavigationManager used to manage URI navigation and location state within the application.
        /// </summary>
        /// <remarks>This property is typically provided by dependency injection in Blazor applications.
        /// Use it to programmatically navigate to different URIs or to access information about the current navigation
        /// state.</remarks>
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
        /// Gets or sets a value indicating whether the profile entry feature is enabled.
        /// </summary>
        [Parameter] public bool ProfileEntry { get; set; }

        /// <summary>
        /// Asynchronously initializes the component and retrieves a paged list of business listings for the current
        /// user.
        /// </summary>
        /// <remarks>This method fetches data from the "businessdirectory/paged" endpoint using the
        /// current user's ID  and processes the response to populate the business listings. If the response contains
        /// data,  it is displayed in the component. Any errors encountered during the operation are displayed  using
        /// the provided snack bar.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var parameters = new BusinessListingPageParameters()
            {
                UserId = UserId,
                Status = ReviewStatus.Approved
            };
            var result = await Provider.PagedListingsAsync(parameters);
            result.ProcessResponseForDisplay(SnackBar, () =>
            {
                _listings = result.Data?.ToList() ?? [];
            });

            await base.OnInitializedAsync();
        }

        /// <summary>
        /// Determines the appropriate color to represent the remaining time status of a business listing.
        /// </summary>
        /// <remarks>Use this method to visually indicate the urgency or status of a business listing
        /// based on its remaining time. The specific color returned corresponds to standard status categories: primary
        /// (normal), warning (expiring soon), error (expired), and secondary (unknown or unavailable).</remarks>
        /// <param name="listing">The business listing for which to evaluate the remaining time status. Cannot be null.</param>
        /// <returns>A color indicating the remaining time status: returns Color.Secondary if the remaining time is unavailable,
        /// Color.Error if the listing has expired, Color.Warning if the listing is expiring soon, or Color.Primary
        /// otherwise.</returns>
        private Color GetRemainingColor(BusinessListingDto listing)
        {
            var remaining = listing.TimeRemaining();
            if (!remaining.HasValue)
                return Color.Secondary;

            if (remaining.Value <= TimeSpan.Zero)
                return Color.Error;

            if (listing.IsExpiringWithin(ExpiringSoonThreshold))
                return Color.Warning;

            return Color.Primary;
        }

        /// <summary>
        /// Determines the appropriate color to represent the progress status of the specified business listing.
        /// </summary>
        /// <remarks>Use this method to obtain a visual indicator for the listing's progress or expiration
        /// status, suitable for UI elements such as progress bars or status badges.</remarks>
        /// <param name="listing">The business listing for which to determine the progress color. Cannot be null.</param>
        /// <returns>A color value indicating the progress status: success, warning, error, or secondary, depending on the
        /// listing's remaining time and expiration state.</returns>
        private Color GetProgressColor(BusinessListingDto listing)
        {
            var remaining = listing.TimeRemaining();
            if (!remaining.HasValue)
                return Color.Secondary;

            if (remaining.Value <= TimeSpan.Zero)
                return Color.Error;

            if (listing.IsExpiringWithin(ExpiringSoonThreshold))
                return Color.Warning;

            return Color.Success;
        }

        /// <summary>
        /// Renews the specified business listing asynchronously and updates the local listing collection upon success.
        /// </summary>
        /// <remarks>If the renewal is successful, the local collection of listings is updated and a
        /// success notification is displayed. If the listing does not exist in the collection, it is added.</remarks>
        /// <param name="listing">The business listing to renew. Must not be null.</param>
        /// <returns>A task that represents the asynchronous renew operation.</returns>
        private async Task RenewListingAsync(BusinessListingDto listing)
        {
            var result = await CommandProvider.RenewAsync(listing.Id);
            result.ProcessResponseForDisplay(SnackBar, () =>
            {
                if (result.Data is null)
                    return;

                var items = _listings.ToList();
                var index = items.FindIndex(l => l.Id == listing.Id);
                if (index >= 0)
                    items[index] = result.Data;
                else
                    items.Add(result.Data);

                _listings = items;
                SnackBar.Add($"Listing \"{result.Data.Heading}\" renewed successfully.", Severity.Success);
                StateHasChanged();
            });
        }
    }
}
