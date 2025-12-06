using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.Constants;
using AccomodationModule.Domain.Interfaces;
using AccomodationModule.Domain.RequestFeatures;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Lodgings
{
    /// <summary>
    /// Represents a component that manages and displays a list of lodgings with filtering, sorting, and pagination
    /// capabilities.
    /// </summary>
    /// <remarks>The <see cref="List"/> class provides functionality for managing a paginated and sortable
    /// list of lodgings,  including operations such as filtering, searching, and deleting items. It integrates with
    /// various services  for navigation, dialog display, notifications, and HTTP operations. This component is designed
    /// for use in  Blazor applications and supports asynchronous data retrieval and manipulation.</remarks>
    public partial class List
    {
        private bool _showFilter;
        private bool _dense;
        private bool _striped = true;
        private bool _bordered;

        private bool _canSearchLodging;
        private bool _canCreateLodging;
        private bool _canEditLodging;
        private bool _canRemoveLodging;

        private LodgingParameters _pageParameters = new();
        private MudTable<LodgingViewModel> _table = null!;

        #region MyRegion

        /// <summary>
        /// Gets or sets the task that provides the current authentication state.
        /// </summary>
        /// <remarks>This property is typically used in Blazor components to access the user's
        /// authentication state. The <see cref="AuthenticationState"/> contains information about the user's identity
        /// and authentication status.</remarks>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for handling authorization operations.
        /// </summary>
        [Inject] public IAuthorizationService AuthorizationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used to manage navigation and URI manipulation in
        /// Blazor applications.
        /// </summary>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display dialogs in the application.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the injected <see cref="ISnackbar"/> service used to display notifications or messages.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing lodging-related operations.
        /// </summary>
        [Inject] public ILodgingService LodgingService { get; set; } = null!;

        #endregion

        #region Methods

        /// <summary>
        /// Retrieves a paginated list of lodgings based on the specified table state and sorting criteria.
        /// </summary>
        /// <remarks>This method uses the provided <paramref name="state"/> to determine the page number,
        /// page size,  and sorting direction for the lodgings query. The data is retrieved asynchronously from the 
        /// underlying provider and mapped to <see cref="LodgingViewModel"/> objects.</remarks>
        /// <param name="state">The current state of the table, including pagination and sorting information.</param>
        /// <param name="token">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A <see cref="TableData{T}"/> object containing the total number of lodgings and a collection of  <see
        /// cref="LodgingViewModel"/> instances representing the lodgings in the current page.</returns>
        public async Task<TableData<LodgingViewModel>> GetLodgingsAsync(TableState state, CancellationToken token)
        {
            _pageParameters.PageNr = state.Page + 1;
            _pageParameters.PageSize = state.PageSize;

            if (state.SortDirection == SortDirection.Ascending)
                _pageParameters.OrderBy = $"{state.SortLabel} desc";
            else if (state.SortDirection == SortDirection.Descending)
                _pageParameters.OrderBy = $"{state.SortLabel} asc";
            else _pageParameters.OrderBy = null;

            var pagingResponse = await LodgingService.PagedLodgingsAsync(_pageParameters);

            return new TableData<LodgingViewModel>() { TotalItems = pagingResponse.TotalCount, Items = pagingResponse.Data.Select(c => new LodgingViewModel(c)) };
        }

        /// <summary>
        /// Handles changes to the checked state and updates the associated data.
        /// </summary>
        /// <remarks>This method updates the active state of the associated page parameters and triggers a
        /// reload of server data.</remarks>
        /// <param name="value">The new checked state. <see langword="true"/> if the item is active; otherwise, <see langword="false"/>.</param>
        /// <returns></returns>
        private async Task CheckedChanged(bool value)
        {
            _pageParameters.Active = value;
            await _table.ReloadServerData();
        }

        /// <summary>
        /// Updates the featured status and reloads the server data for the table.
        /// </summary>
        /// <remarks>This method modifies the featured status in the page parameters and triggers a server
        /// data reload for the associated table. Ensure that the table is properly configured to handle the reload
        /// operation.</remarks>
        /// <param name="value">The new featured status to set. A value of <see langword="null"/> clears the featured status.</param>
        /// <returns></returns>
        private async Task FeaturedChanged(bool? value)
        {
            _pageParameters.Featured = value;
            await _table.ReloadServerData();
        }

        /// <summary>
        /// Clears all applied filters and reloads the server data.
        /// </summary>
        /// <remarks>This method resets the filter parameters to their default state and triggers a reload
        /// of the data from the server. It is typically used to reset the view to an unfiltered state.</remarks>
        private void ClearFiltersAsync()
        {
            _pageParameters = new LodgingParameters();
            _table.ReloadServerData();
        }

        /// <summary>
        /// Toggles the visibility of filters in the user interface.
        /// </summary>
        /// <param name="value">A <see langword="true"/> value makes the filters visible; <see langword="false"/> hides them.</param>
        public void ShowFilters(bool value)
        {
            _showFilter = value;
            StateHasChanged();
        }

        /// <summary>
        /// Deletes a lodging item from the application after user confirmation.
        /// </summary>
        /// <remarks>This method displays a confirmation dialog to the user before proceeding with the
        /// deletion. If the user confirms, the lodging is deleted using the specified provider. If the deletion is
        /// successful, the server data is reloaded; otherwise, error messages are displayed.</remarks>
        /// <param name="productId">The unique identifier of the lodging to be deleted. Can be <see langword="null"/>.</param>
        /// <returns></returns>
        private async Task DeleteLodging(string? productId)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this lodging from this application?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var deletionResult = await LodgingService.RemoveLodgingAsync(productId);
                if (deletionResult.Succeeded)
                {
                    await _table.ReloadServerData();
                }
                else
                {
                    SnackBar.AddErrors(deletionResult.Messages);
                }
            }
        }

        /// <summary>
        /// Initiates a search operation by reloading server data for the associated table.
        /// </summary>
        /// <remarks>This method asynchronously reloads data from the server to ensure the table reflects
        /// the latest state. It is intended to be used when updated or refreshed data is required.</remarks>
        /// <returns></returns>
        private async Task Search()
        {
            await _table.ReloadServerData();
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Performs initialization logic for the component when it is first rendered.
        /// </summary>
        /// <remarks>This method sets up page metadata, including the title, URL, and breadcrumbs,  to
        /// provide contextual information for the user interface. It also invokes the base  implementation to ensure
        /// any additional initialization logic defined in the parent class is executed.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            var currentUser = authState.User;

            _canSearchLodging = (await AuthorizationService.AuthorizeAsync(currentUser, Permissions.Lodging.Search)).Succeeded;
            _canCreateLodging = (await AuthorizationService.AuthorizeAsync(currentUser, Permissions.Lodging.Create)).Succeeded;
            _canEditLodging = (await AuthorizationService.AuthorizeAsync(currentUser, Permissions.Lodging.Edit)).Succeeded;
            _canRemoveLodging = (await AuthorizationService.AuthorizeAsync(currentUser, Permissions.Lodging.Delete)).Succeeded;
            
            await base.OnInitializedAsync();
        }

        #endregion
    }
}
