using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.Constants;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using ConectOne.Domain.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace Accomodation.Blazor.Components.VacationHosts
{
    /// <summary>
    /// Component for displaying and managing vacation hosts in a table format.
    /// </summary>
    public partial class VacationHostsTable
    {
        #region Private Fields

        private bool _dense;
        private bool _striped = true;
        private bool _bordered;

        private MudTable<VacationHostViewModel> _table = null!;
        private RequestParameters _pageParameters = new();

        private bool _canCreateVacationHost;
        private bool _canEditVacationHost;
        private bool _canRemoveVacationHost;
        private bool _canSearchVacationHosts;
        private bool _canCreateVacation;

        #endregion

        #region Injections

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
        /// Gets or sets the service responsible for managing vacation host operations.
        /// </summary>
        /// <remarks>This property is typically injected via dependency injection and must be set before
        /// using any functionality that depends on it.</remarks>
        [Inject] public IVacationHostService VacationHostService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display transient messages to the user.
        /// </summary>
        /// <remarks>This property is typically provided by dependency injection and allows components to
        /// show notifications such as snackbars or toasts. Assigning this property manually is not recommended outside
        /// of a dependency injection context.</remarks>
        [Inject] public ISnackbar Snackbar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display dialogs to the user.
        /// </summary>
        /// <remarks>This property is typically set by the dependency injection framework. Assigning a
        /// custom implementation allows for customized dialog presentation or testing scenarios.</remarks>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        #endregion

        #region Methods

        /// <summary>
        /// Clears all filters and reloads the table data.
        /// </summary>
        private void ClearFiltersAsync()
        {
            _pageParameters = new RequestParameters();
            _table.ReloadServerData();
        }

        /// <summary>
        /// Triggers a search and reloads the table data.
        /// </summary>
        private async Task Search()
        {
            await _table.ReloadServerData();
        }

        /// <summary>
        /// Retrieves a paginated list of vacation hosts based on the table state.
        /// </summary>
        /// <param name="state">The current state of the table.</param>
        /// <param name="token">Cancellation token for the async operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the table data.</returns>
        public async Task<TableData<VacationHostViewModel>> GetVacationHostsAsync(TableState state, CancellationToken token)
        {
            _pageParameters.PageNr = state.Page + 1;
            _pageParameters.PageSize = state.PageSize;

            if (state.SortDirection == SortDirection.Ascending)
                _pageParameters.OrderBy = $"{state.SortLabel} desc";
            else if (state.SortDirection == SortDirection.Descending)
                _pageParameters.OrderBy = $"{state.SortLabel} asc";
            else _pageParameters.OrderBy = null;

            var pagingResponse = await VacationHostService.PagedVacationHostsAsync(_pageParameters, token);

            return new TableData<VacationHostViewModel>()
            {
                TotalItems = pagingResponse.Data.Count(),
                Items = pagingResponse.Data.Select(c => new VacationHostViewModel(c))
            };
        }

        /// <summary>
        /// Deletes a vacation host by its ID after user confirmation.
        /// </summary>
        /// <param name="categoryId">The ID of the vacation host to delete.</param>
        private async Task DeleteCategory(string categoryId)
        {
            var parameters = new DialogParameters<ConformtaionModal>();
            parameters.Add(x => x.ContentText, "Are you sure you want to remove this vacation host from this application?");
            parameters.Add(x => x.ButtonText, "Yes");
            parameters.Add(x => x.Color, Color.Success);

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var deletionResult = await VacationHostService.RemoveAsync(categoryId);
                if (!deletionResult.Succeeded) Snackbar.AddErrors(deletionResult.Messages);

                await _table.ReloadServerData();
            }
        }

        /// <summary>
        /// Asynchronously initializes the component and determines the current user's authorization  for various
        /// vacation-related permissions.
        /// </summary>
        /// <remarks>This method checks the current user's authentication and authorization state to
        /// determine  their permissions for creating, editing, deleting, and searching vacation hosts, as well as 
        /// creating vacations. These permissions are stored in private fields for use within the component.</remarks>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            var currentUser = authState.User;

            _canCreateVacationHost = (await AuthorizationService.AuthorizeAsync(currentUser, Permissions.VacationHost.Create)).Succeeded;
            _canEditVacationHost = (await AuthorizationService.AuthorizeAsync(currentUser, Permissions.VacationHost.Edit)).Succeeded;
            _canRemoveVacationHost = (await AuthorizationService.AuthorizeAsync(currentUser, Permissions.VacationHost.Delete)).Succeeded;
            _canSearchVacationHosts = (await AuthorizationService.AuthorizeAsync(currentUser, Permissions.VacationHost.Search)).Succeeded;
            _canCreateVacation = (await AuthorizationService.AuthorizeAsync(currentUser, Permissions.Vacation.Create)).Succeeded;

            await base.OnInitializedAsync();
        }

        #endregion
    }
}
