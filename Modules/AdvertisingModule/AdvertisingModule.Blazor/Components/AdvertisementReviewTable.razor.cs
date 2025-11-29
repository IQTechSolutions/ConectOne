using AdvertisingModule.Application.ViewModels;
using AdvertisingModule.Domain.Constants;
using AdvertisingModule.Domain.Interfaces;
using AdvertisingModule.Domain.RequestFeatures;
using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace AdvertisingModule.Blazor.Components
{
    /// <summary>
    /// Represents a table component for reviewing advertisements, providing functionality to load, approve, and reject
    /// advertisements.
    /// </summary>
    /// <remarks>This component is designed to display a paginated table of advertisements for review. It
    /// supports server-side data loading and provides actions for approving or rejecting advertisements. The table's
    /// appearance can be customized using properties such as <see cref="Dense"/>, <see cref="Striped"/>, and <see
    /// cref="Bordered"/>.</remarks>
    public partial class AdvertisementReviewTable
    {
        private IEnumerable<AdvertisementViewModel> _advertisements = null!;
        private MudTable<AdvertisementViewModel> _table = null!;
        private int _totalItems;
        private bool _dense;
        private bool _striped = true;
        private bool _bordered;
        private bool _loaded;

        private bool _canCreate;
        private bool _canReject;
        private bool _canApprove;

        /// <summary>
        /// Gets or sets the task that represents the asynchronous operation to retrieve the current authentication state.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Injects the authorization service for checking user permissions.
        /// </summary>
        [Inject] public IAuthorizationService AuthorizationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications in the application.
        /// </summary>
        /// <remarks>The <see cref="ISnackbar"/> service must be injected and configured in the
        /// application's dependency injection container. Use this property to display transient notifications to the
        /// user, such as status messages or alerts.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; }

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; }

        /// <summary>
        /// Gets or sets the service used to query advertisements.
        /// </summary>
        /// <remarks>This property is typically injected via dependency injection. Ensure that a valid
        /// implementation of <see cref="IAdvertisementQueryService"/> is provided before using this property.</remarks>
        [Inject] public IAdvertisementQueryService AdvertisementQueryService { get; set; }

        /// <summary>
        /// Gets or sets the service responsible for executing advertisement-related commands.
        /// </summary>
        [Inject] public IAdvertisementCommandService AdvertisementCommandService { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        [Parameter] public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the review status of the current item.
        /// </summary>
        [Parameter] public ReviewStatus ReviewStatus { get; set; }

        /// <summary>
        /// Reloads the server-side data for the table based on the specified state.
        /// </summary>
        /// <remarks>This method retrieves and updates the data displayed in the table by reloading it
        /// from the server.  Ensure that the <paramref name="token"/> is monitored to handle cancellation
        /// appropriately.</remarks>
        /// <param name="state">The current state of the table, including sorting, filtering, and pagination settings.</param>
        /// <param name="token">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A <see cref="TableData{T}"/> object containing the total number of items and the current page of data.</returns>
        private async Task<TableData<AdvertisementViewModel>> ServerReload(TableState state, CancellationToken token)
        {
            await LoadData();
            return new TableData<AdvertisementViewModel> { TotalItems = _totalItems, Items = _advertisements };
        }

        /// <summary>
        /// Approves the advertisement with the specified identifier.
        /// </summary>
        /// <remarks>This method sends a request to approve the advertisement and processes the response. 
        /// If the operation is successful, the advertisement data is reloaded, and a success message is
        /// displayed.</remarks>
        /// <param name="advertisementId">The unique identifier of the advertisement to approve. Cannot be <see langword="null"/> or empty.</param>
        /// <returns></returns>
        private async Task ApproveAdvertisement(string advertisementId)
        {
            var request = await AdvertisementCommandService.ApproveAsync(advertisementId);
            request.ProcessResponseForDisplay(SnackBar, async () =>
            {
                await LoadData();
                SnackBar.AddSuccess("Advertisement Approved");
            });
        }

        /// <summary>
        /// Rejects the advertisement with the specified identifier.
        /// </summary>
        /// <remarks>This method sends a request to reject the advertisement and processes the response. 
        /// If the operation is successful, the advertisement data is reloaded, and a success message is
        /// displayed.</remarks>
        /// <param name="advertisementId">The unique identifier of the advertisement to reject. Cannot be <see langword="null"/> or empty.</param>
        /// <returns></returns>
        private async Task RejectAdvertisement(string advertisementId)
        {
            var request = await AdvertisementCommandService.RejectAsync(advertisementId);
            request.ProcessResponseForDisplay(SnackBar, async () =>
            {
                await LoadData();
                SnackBar.AddSuccess("Advertisement Rejected");
            });
        }

        /// <summary>
        /// Loads advertisement data asynchronously based on the specified review status and user ID.
        /// </summary>
        /// <remarks>This method retrieves a paginated list of advertisements using the provided review
        /// status and user ID. The retrieved data is processed and displayed, updating the total item count and the
        /// list of advertisements.</remarks>
        /// <returns></returns>
        private async Task LoadData()
        {
            var args = new AdvertisementListingPageParameters() { Status = ReviewStatus, UserId = UserId};

            var request = await AdvertisementQueryService.PagedListingsAsync(args);
            request.ProcessResponseForDisplay(SnackBar, () =>
            {
                var data = request.Data.ToList();
                _totalItems = data.Count;
                _advertisements = data.Select(c => new AdvertisementViewModel(c));
            });
        }

        /// <summary>
        /// Asynchronously reloads the server data for the table.
        /// </summary>
        /// <remarks>This method refreshes the data by invoking the server-side reload operation. It should be
        /// called when the table's data needs to be updated to reflect the latest state.</remarks>
        private async Task Reload()
        {
            await _table.ReloadServerData();
        }

        /// <summary>
        /// Asynchronously initializes the component and performs any required setup logic.
        /// </summary>
        /// <remarks>This method is called by the Blazor framework during the component's initialization
        /// phase. It invokes the base implementation to ensure that any inherited initialization logic is executed.
        /// Override this method to include additional asynchronous initialization logic specific to the
        /// component.</remarks>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;

            _canCreate = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.AdvertisementReview.Create)).Succeeded;
            _canReject = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.AdvertisementReview.Reject)).Succeeded;
            _canApprove = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.AdvertisementReview.Approve)).Succeeded;

            await base.OnInitializedAsync();
        }
    }
}
