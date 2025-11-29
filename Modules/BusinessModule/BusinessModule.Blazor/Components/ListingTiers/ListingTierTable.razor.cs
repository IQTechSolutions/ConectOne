using BusinessModule.Domain.Constants;
using BusinessModule.Domain.DataTransferObjects;
using BusinessModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace BusinessModule.Blazor.Components.ListingTiers
{
    /// <summary>
    /// Represents a component for managing and displaying a table of listing tiers.
    /// </summary>
    /// <remarks>This component provides functionality for loading, displaying, and interacting with a table
    /// of listing tiers. It supports server-side data loading, deletion of listing tiers, and integration with various
    /// services such as HTTP providers, dialog services, and snack bar notifications. The table can be customized with
    /// options such as dense, striped, and bordered styles.</remarks>
    public partial class ListingTierTable
    {
        private IEnumerable<ListingTierDto> _listingTiers = null!;
        private MudTable<ListingTierDto> _table = null!;

        private int _totalItems;
        private bool _dense;
        private bool _striped = true;
        private bool _bordered;
        private bool _loaded;

        private bool _canCreate;
        private bool _canEdit;
        private bool _canDelete;

        /// <summary>
        /// Gets or sets the service used to display dialogs in the application.
        /// </summary>
        /// <remarks>This property is typically injected by the dependency injection framework. Ensure
        /// that the service is properly configured before using it to display dialogs.</remarks>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications.
        /// </summary>
        /// <remarks>This property is typically injected by the dependency injection framework. Ensure
        /// that a valid implementation of <see cref="ISnackbar"/> is provided before using this property.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used for handling navigation within the
        /// application.
        /// </summary>
        /// <remarks>This property is typically injected by the framework and should not be set manually
        /// in most cases.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application configuration settings.
        /// </summary>
        /// <remarks>The configuration data can include settings from various sources, such as
        /// appsettings.json,  environment variables, or user secrets. Ensure that the configuration is properly
        /// initialized  before accessing its values.</remarks>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to query listing tier information.
        /// </summary>
        /// <remarks>This property is automatically injected and must not be null. Ensure the service is
        /// properly configured in the dependency injection container.</remarks>
        [Inject] public IListingTierQueryService ListingTierQueryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for executing commands related to listing tiers.
        /// </summary>
        [Inject] public IListingTierCommandService ListingTierCommandService { get; set; } = null!;

        /// <summary>
        /// Injects the authorization service for checking user permissions.
        /// </summary>
        [Inject] public IAuthorizationService AuthorizationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the task that provides the current authentication state.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Asynchronously initializes the component and sets its initial state.
        /// </summary>
        /// <remarks>This method is called by the Blazor framework during the component's initialization
        /// lifecycle. It sets the component's loaded state and ensures that any base class initialization logic is
        /// executed.</remarks>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;

            _canCreate = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.BusinessTierPermissions.Create)).Succeeded;
            _canEdit = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.BusinessTierPermissions.Edit)).Succeeded;
            _canDelete = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.BusinessTierPermissions.Delete)).Succeeded;

            _loaded = true;
            await base.OnInitializedAsync();
        }

        /// <summary>
        /// Reloads the server-side data and returns a paginated table of listing tiers.
        /// </summary>
        /// <remarks>This method retrieves the latest data from the server and updates the table with the
        /// current state. The operation respects the provided <paramref name="token"/> for cancellation.</remarks>
        /// <param name="state">The current state of the table, including pagination and sorting information.</param>
        /// <param name="token">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A <see cref="TableData{T}"/> containing the total number of items and the current page of listing tiers.</returns>
        private async Task<TableData<ListingTierDto>> ServerReload(TableState state, CancellationToken token)
        {
            await LoadData();
            return new TableData<ListingTierDto> { TotalItems = _totalItems, Items = _listingTiers };
        }

        /// <summary>
        /// Asynchronously loads listing tier data and updates the internal state with the retrieved information.
        /// </summary>
        /// <remarks>This method retrieves a collection of listing tiers from the data provider and
        /// updates the total item count  and listing tier collection if the request is successful. If the request
        /// fails, error messages are added to the snack bar.</remarks>
        /// <returns></returns>
        private async Task LoadData()
        {
            var request = await ListingTierQueryService.AllListingTiersAsync();
            if (request.Succeeded && request.Data is not null)
            {
                _totalItems = request.Data.Count();
                _listingTiers = request.Data;
            }
            SnackBar.AddErrors(request.Messages);
        }

        /// <summary>
        /// Reloads the server data for the table asynchronously.
        /// </summary>
        /// <remarks>This method triggers a reload of the server-side data associated with the table.  It
        /// is an asynchronous operation and should be awaited to ensure the reload completes before
        /// proceeding.</remarks>
        /// <returns></returns>
        private async Task Reload()
        {
            await _table.ReloadServerData();
        }

        /// <summary>
        /// Displays a confirmation dialog and, if confirmed, deletes the specified listing tier associated with the
        /// given affiliate ID.
        /// </summary>
        /// <remarks>This method prompts the user with a confirmation dialog before proceeding with the
        /// deletion.  If the user cancels the operation, no changes are made. After a successful deletion, the
        /// server-side data is reloaded.</remarks>
        /// <param name="affiliateId">The unique identifier of the affiliate whose listing tier is to be deleted. Cannot be null or empty.</param>
        /// <returns></returns>
        private async Task Delete(string affiliateId)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this listing tier from this application?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                await ListingTierCommandService.RemoveAsync(affiliateId);
                await _table.ReloadServerData();
            }
        }
    }
}
