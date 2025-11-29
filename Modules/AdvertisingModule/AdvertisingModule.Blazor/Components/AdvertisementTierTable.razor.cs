using AdvertisingModule.Domain.Constants;
using AdvertisingModule.Domain.DataTransferObjects;
using AdvertisingModule.Domain.Enums;
using AdvertisingModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace AdvertisingModule.Blazor.Components
{
    /// <summary>
    /// Represents a component that displays and manages advertisement tiers in a tabular format.
    /// </summary>
    /// <remarks>This component provides functionality for loading, displaying, and managing advertisement
    /// tiers. It supports server-side data loading, deletion of advertisement tiers, and integration with external
    /// services such as HTTP providers, dialog services, and notifications. The component is designed to work with
    /// Blazor's cascading parameters and dependency injection.</remarks>
    public partial class AdvertisementTierTable
    {
        private IEnumerable<AdvertisementTierDto> _advertisementTiers = null!;
        private MudTable<AdvertisementTierDto> _table = null!;

        private int _totalItems;
        private bool _dense;
        private bool _striped = true;
        private bool _bordered;
        private bool _loaded;
        private bool _canCreateAdvertisement;
        private bool _canCreate;
        private bool _canEdit;
        private bool _canDelete;

        /// <summary>
        /// Gets or sets the service used to query advertisement tier information.
        /// </summary>
        /// <remarks>This property is typically injected via dependency injection and must be set before
        /// using any functionality that depends on advertisement tier queries.</remarks>
        [Inject] public IAdvertisementTierQueryService AdvertisementTierQueryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for executing commands related to advertisement tiers.
        /// </summary>
        [Inject] public IAdvertisementTierCommandService AdvertisementTierCommandService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display dialogs in the application.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications in the application.
        /// </summary>
        /// <remarks>The <see cref="ISnackbar"/> service must be injected and is typically used to provide
        /// user feedback  through transient, non-intrusive messages.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used to manage navigation within the application.
        /// </summary>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        /// <remarks>This property is typically populated via dependency injection and provides access to
        /// configuration values such as app settings, connection strings, and other configuration sources.</remarks>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Injects the authorization service for checking user permissions.
        /// </summary>
        [Inject] public IAuthorizationService AuthorizationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the task that represents the authentication state of the current user.
        /// </summary>
        /// <remarks>This property is typically provided as a cascading parameter in Blazor applications
        /// to enable  components to access the authentication state asynchronously. Ensure that the task is not null 
        /// and resolves to a valid <see cref="AuthenticationState"/>.</remarks>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;
        
        /// <summary>
        /// Gets or sets the type of advertisement to be displayed.
        /// </summary>
        [Parameter, EditorRequired] public AdvertisementType AdvertisementType { get; set; }

        /// <summary>
        /// Asynchronously initializes the component and sets its initial state.
        /// </summary>
        /// <remarks>This method is called by the Blazor framework during the component's initialization
        /// lifecycle. It sets the component's loaded state and ensures that any base class initialization logic is
        /// executed.</remarks>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;

            _canCreateAdvertisement = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.Advertisement.Create)).Succeeded;
            _canCreate = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.AdvertisementTier.Create)).Succeeded;
            _canEdit = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.AdvertisementTier.Edit)).Succeeded;
            _canDelete = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.AdvertisementTier.Delete)).Succeeded;

            _loaded = true;
            await base.OnInitializedAsync();
        }

        /// <summary>
        /// Reloads the server-side data for the table based on the specified state.
        /// </summary>
        /// <remarks>This method retrieves the latest data for the table and applies the specified state,
        /// such as sorting or pagination. The operation respects the provided <paramref name="token"/> to allow for
        /// cancellation.</remarks>
        /// <param name="state">The current state of the table, including sorting and pagination information.</param>
        /// <param name="token">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A <see cref="TableData{T}"/> containing the total number of items and the current page of advertisement tier
        /// data.</returns>
        private async Task<TableData<AdvertisementTierDto>> ServerReload(TableState state, CancellationToken token)
        {
            await LoadData();
            return new TableData<AdvertisementTierDto> { TotalItems = _totalItems, Items = _advertisementTiers };
        }

        /// <summary>
        /// Asynchronously loads advertisement tier data from the provider and updates the internal state.
        /// </summary>
        /// <remarks>This method retrieves a collection of advertisement tiers from the provider and
        /// updates the total item count  and the internal advertisement tier list if the request is successful. If the
        /// request fails, error messages  are added to the snack bar.</remarks>
        /// <returns></returns>
        private async Task LoadData()
        {
            var request = await AdvertisementTierQueryService.AllAdvertisementTiersAsync(AdvertisementType);
            if (request.Succeeded && request.Data is not null)
            {
                _totalItems = request.Data.Count();
                _advertisementTiers = request.Data;
            }
            SnackBar.AddErrors(request.Messages);
        }

        /// <summary>
        /// Reloads the server data for the table asynchronously.
        /// </summary>
        /// <remarks>This method triggers a reload of the server-side data associated with the table.  It
        /// is an asynchronous operation and should be awaited to ensure completion.</remarks>
        /// <returns></returns>
        private async Task Reload()
        {
            await _table.ReloadServerData();
        }

        /// <summary>
        /// Displays a confirmation dialog and, if confirmed, deletes the specified advertisement tier associated with
        /// the given affiliate ID.
        /// </summary>
        /// <remarks>This method prompts the user with a confirmation dialog before proceeding with the
        /// deletion. If the user cancels the operation, no changes are made. After a successful deletion, the
        /// server-side data is reloaded to reflect the updated state.</remarks>
        /// <param name="affiliateId">The unique identifier of the affiliate whose advertisement tier is to be deleted. Cannot be null or empty.</param>
        /// <returns></returns>
        private async Task Delete(string affiliateId)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this advertisement tier from this application?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                await AdvertisementTierCommandService.RemoveAsync(affiliateId);
                await _table.ReloadServerData();
            }
        }
    }
}
