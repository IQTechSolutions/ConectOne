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

namespace Accomodation.Blazor.Components.Vacations
{
    /// <summary>
    /// Component for displaying and managing vacation packages in a table format.
    /// </summary>
    public partial class VacationsTable
    {
        private bool _dense;
        private bool _striped = true;
        private bool _bordered;

        private bool _canCreateVacation;
        private bool _canEditVacation;
        private bool _canDeleteVacation;
        private bool _canSearchVacation;

        private MudTable<VacationViewModel> _table = null!;
        private VacationPageParameters _pageParameters = new();

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
        /// Gets or sets the service used to display transient messages to the user.
        /// </summary>
        /// <remarks>This property is typically injected by the dependency injection framework. Use the
        /// <see cref="ISnackbar"/> service to show notifications such as alerts, confirmations, or status messages
        /// within the application's user interface.</remarks>
        [Inject] public ISnackbar Snackbar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display dialogs to the user.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application configuration settings.
        /// </summary>
        /// <remarks>The configuration is typically populated by the dependency injection system and
        /// provides access to key-value pairs for application settings. Use this property to retrieve configuration
        /// values such as connection strings, feature flags, or other environment-specific data.</remarks>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing vacation-related operations.
        /// </summary>
        /// <remarks>This property is automatically injected by the dependency injection framework. Ensure
        /// that a valid  implementation of <see cref="IVacationService"/> is registered in the service
        /// container.</remarks>
        [Inject] public IVacationService VacationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the NavigationManager used to manage URI navigation and location state within the application.
        /// </summary>
        /// <remarks>This property is typically set by the Blazor framework through dependency injection.
        /// It provides methods for programmatically navigating to different URIs and for responding to location
        /// changes.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        #endregion

        #region Parameters

        /// <summary>
        /// The ID of the vacation host associated with the vacation extensions.
        /// </summary>
        [Parameter] public string VacationHostId { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether the current item is an extension.
        /// </summary>
        [Parameter] public bool IsExtension { get; set; } 

        /// <summary>
        /// Gets or sets the base URL for fetching vacation data.
        /// </summary>
        [Parameter] public string StartUrl { get; set; } = "packages";

        #endregion

        #region Methods

        /// <summary>
        /// Clears all filters and reloads the table data.
        /// </summary>
        private void ClearFiltersAsync()
        {
            _pageParameters = new VacationPageParameters();
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
        /// Retrieves a paginated list of vacations based on the table state.
        /// </summary>
        /// <param name="state">The current state of the table.</param>
        /// <param name="token">Cancellation token for the async operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the table data.</returns>
        public async Task<TableData<VacationViewModel>> GetVacationsAsync(TableState state, CancellationToken token)
        {
            try
            {
                _pageParameters.PageNr = state.Page + 1;
                _pageParameters.PageSize = state.PageSize;

                _pageParameters.OrderBy = state.SortDirection switch
                {
                    SortDirection.Ascending => $"{state.SortLabel} desc",
                    SortDirection.Descending => $"{state.SortLabel} asc",
                    _ => null
                };

                var pagingResponse = await VacationService.PagedAsync(_pageParameters, token);

                return new TableData<VacationViewModel>()
                {
                    TotalItems = pagingResponse.TotalCount,
                    Items = pagingResponse.Data.Select(c => new VacationViewModel(c)).OrderBy(c => c.VacationTitle?.VacationTitle)
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Deletes a vacation package by its ID after user confirmation.
        /// </summary>
        /// <param name="vacationId">The ID of the vacation package to delete.</param>
        private async Task DeleteCategory(string vacationId)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this vacation package from this application?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var deletionResult = await VacationService.RemoveAsync(vacationId);
                if (!deletionResult.Succeeded) Snackbar.AddErrors(deletionResult.Messages);

                await _table.ReloadServerData();
            }
        }

        /// <summary>
        /// Deletes a vacation package by its ID after user confirmation.
        /// </summary>
        /// <param name="vacationId">The ID of the vacation package to delete.</param>
        private async Task DuplicateVacation(string vacationId)
        {
            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Large, FullWidth = true };
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to duplicate this vacation?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };
            
            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters, options);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var duplicationResult = await VacationService.DuplicateAsync(vacationId);
                if (!duplicationResult.Succeeded) Snackbar.AddErrors(duplicationResult.Messages);
                else await _table.ReloadServerData();
            }
        }
        
        #endregion

        /// <summary>
        /// Initializes the component by setting up the vacation host ID and loading initial data.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            _pageParameters.VacationHostId = VacationHostId;

            var authState = await AuthenticationStateTask;
            var currentUser = authState.User;

            _canCreateVacation = (await AuthorizationService.AuthorizeAsync(currentUser, Permissions.Vacation.Create)).Succeeded;
            _canEditVacation = (await AuthorizationService.AuthorizeAsync(currentUser, Permissions.Vacation.Edit)).Succeeded;
            _canDeleteVacation = (await AuthorizationService.AuthorizeAsync(currentUser, Permissions.Vacation.Delete)).Succeeded;
            _canSearchVacation = (await AuthorizationService.AuthorizeAsync(currentUser, Permissions.Vacation.Search)).Succeeded;

            await base.OnInitializedAsync();
        }
    }
}
