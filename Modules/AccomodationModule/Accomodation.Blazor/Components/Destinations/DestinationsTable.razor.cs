using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.Arguments;
using AccomodationModule.Domain.Constants;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace Accomodation.Blazor.Components.Destinations
{
    /// <summary>
    /// Represents a table component for managing vacation destinations.
    /// </summary>
    /// <remarks>The <see cref="DestinationsTable"/> class provides functionality for displaying, filtering, 
    /// searching, creating, editing, and deleting vacation destinations. It integrates with Blazor  components and
    /// services such as authentication, authorization, and navigation. This class  is designed to work with server-side
    /// data and supports pagination, sorting, and filtering  operations. <para> Use this component in a Blazor
    /// application to manage vacation destinations within a specific  vacation context. Ensure that required services,
    /// such as <see cref="IAuthorizationService"/>  and <see cref="AuthenticationStateTask"/>, are properly injected.
    /// </para></remarks>
	public partial class DestinationsTable
	{
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private bool _canSearchDestinations;
        private bool _canCreateDestination;
        private bool _canEditDestination;
        private bool _canDeleteDestination;

        private MudTable<DestinationViewModel> _table = null!;
        private DestinationPageParameters _pageParameters = new();

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
        /// Gets or sets the service responsible for handling destination-related operations.
        /// </summary>
        [Inject] public IDestinationService DestinationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display dialogs within the component.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display transient notification messages to the user.
        /// </summary>
        /// <remarks>Inject this property to provide access to snackbar notifications within the
        /// component. The service allows displaying brief messages, such as alerts or confirmations, typically at the
        /// bottom of the screen.</remarks>
        [Inject] public ISnackbar Snackbar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> used to manage and perform navigation within the
        /// application.
        /// </summary>
        /// <remarks>Use this property to programmatically navigate to different URIs or to access
        /// information about the current navigation state. This property is typically injected by the framework and
        /// should not be set manually in most scenarios.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        /// <remarks>Use this property to access configuration values such as connection strings,
        /// application settings, and environment variables. The configuration source may vary depending on how the
        /// application is set up (e.g., appsettings.json, environment variables, or other providers).</remarks>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        #endregion

        #region Parameters

        /// <summary>
        /// Gets or sets the unique identifier for a vacation.
        /// </summary>
        [Parameter] public string? VacationId { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the vacation extension.
        /// </summary>
        [Parameter] public string? VacationExtensionId { get; set; }

        /// <summary>
        /// Gets or sets the callback that is invoked when a destination is removed.
        /// </summary>
        /// <remarks>The callback receives the identifier of the removed destination as a <see
        /// cref="string"/>. Use this property to handle custom logic when a destination is removed.</remarks>
        [Parameter] public EventCallback<string> DestinationRemoved { get; set; }

        /// <summary>
        /// Gets or sets the callback that is invoked when a destination is successfully created.
        /// </summary>
        [Parameter] public EventCallback DestinationCreated { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Clears all filters applied to the current destination page and reloads the server data.
        /// </summary>
        /// <remarks>This method resets the page parameters to their default state and triggers a reload
        /// of the server data. It is intended to be used when the user wants to remove all filtering criteria and
        /// refresh the data.</remarks>
        private void ClearFiltersAsync()
        {
            _pageParameters = new DestinationPageParameters();
            _table.ReloadServerData();
        }

        /// <summary>
        /// Reloads the server data for the associated table asynchronously.
        /// </summary>
        /// <remarks>This method triggers a reload of server-side data for the table. It is useful for
        /// refreshing         the table's data when changes occur on the server or when updated information is
        /// required.</remarks>
        /// <returns></returns>
        private async Task Search()
        {
            await _table.ReloadServerData();
        }

        /// <summary>
        /// Retrieves a paginated list of destinations based on the specified table state.
        /// </summary>
        /// <remarks>This method uses the provided <paramref name="state"/> to determine the page number,
        /// page size, and sorting direction for the query. The results are returned as a paginated list of <see
        /// cref="DestinationViewModel"/>.</remarks>
        /// <param name="state">The state of the table, including pagination, sorting, and filtering information.</param>
        /// <param name="token">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A <see cref="TableData{DestinationViewModel}"/> object containing the paginated list of destinations and the
        /// total number of items.</returns>
        public async Task<TableData<DestinationViewModel>> GetDestinationsAsync(TableState state, CancellationToken token)
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

                var pagingResponse = await DestinationService.PagedDestinationsAsync(_pageParameters);

                return new TableData<DestinationViewModel>() { TotalItems = pagingResponse.Data.Count(), Items = pagingResponse.Data.Select(c => new DestinationViewModel(c)) };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        /// <summary>
        /// Deletes a vacation destination after user confirmation.
        /// </summary>
        /// <remarks>This method displays a confirmation dialog to the user before proceeding with the
        /// deletion. If the user confirms,  the destination is removed from the application. The method also triggers
        /// the <see cref="DestinationRemoved"/> event  upon successful deletion. <para> If the deletion fails, error
        /// messages are displayed using the application's notification system. </para></remarks>
        /// <param name="destinationId">The unique identifier of the vacation destination to be deleted. Cannot be null or empty.</param>
        /// <returns></returns>
        private async Task DeleteCategory(string destinationId)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this destination from this application?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                if (string.IsNullOrEmpty(VacationId) && string.IsNullOrEmpty(VacationExtensionId))
                {
                    var deletionResult = await DestinationService.RemoveAsync(destinationId);
                    if (!deletionResult.Succeeded) Snackbar.AddErrors(deletionResult.Messages);

                    await _table.ReloadServerData();
                }

                await DestinationRemoved.InvokeAsync(destinationId);
            }
        }

        /// <summary>
        /// Creates a new category based on the current vacation context and navigates to the appropriate destination
        /// creation page.
        /// </summary>
        /// <remarks>If both <see cref="VacationId"/> and <see cref="VacationExtensionId"/> are null or
        /// empty, the method redirects the user to the destination creation page. Otherwise, it triggers the <see
        /// cref="DestinationCreated"/> event asynchronously.</remarks>
        /// <returns></returns>
        private async Task CreateCategory()
        {
            if (string.IsNullOrEmpty(VacationId) && string.IsNullOrEmpty(VacationExtensionId))
            {
                NavigationManager.NavigateTo("/destinations/create");
            }

            await DestinationCreated.InvokeAsync();
        }

        /// <summary>
        /// Asynchronously initializes the component and sets up authorization and page parameters.
        /// </summary>
        /// <remarks>This method retrieves the current user's authentication state and determines their
        /// authorization  for various destination-related permissions. It also initializes page parameters based on the
        /// provided vacation and vacation extension IDs.</remarks>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            var currentUser = authState.User;

            _canSearchDestinations = (await AuthorizationService.AuthorizeAsync(currentUser, Permissions.Destination.Search)).Succeeded;
            _canCreateDestination = (await AuthorizationService.AuthorizeAsync(currentUser, Permissions.Destination.Create)).Succeeded;
            _canEditDestination = (await AuthorizationService.AuthorizeAsync(currentUser, Permissions.Destination.Edit)).Succeeded;
            _canDeleteDestination = (await AuthorizationService.AuthorizeAsync(currentUser, Permissions.Destination.Delete)).Succeeded;

            _pageParameters.VacationId = VacationId;
            _pageParameters.VacationExtensionId = VacationExtensionId;

            await base.OnInitializedAsync();
        }

        #endregion
    }
}