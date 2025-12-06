using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.Constants;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Areas
{
    /// <summary>
    /// Represents a Blazor component for managing a list of lodging types.
    /// </summary>
    /// <remarks>The <see cref="List"/> component provides functionality for displaying, creating,
    /// editing,  and deleting lodging types. It integrates with various services, such as authorization, HTTP
    /// communication,  and UI dialogs, to enable these operations. This component is designed to work with server-side
    /// data  and supports pagination, sorting, and user authentication checks.</remarks>
    public partial class List
    {
        private MudTable<AreaDto> _table = null!;

        private bool _canCreateArea;
        private bool _canEditArea;
        private bool _canDeleteArea;
        private bool _canViewRestaurants;

        private bool _dense;
        private bool _striped = true;
        private bool _bordered;

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
        /// Gets or sets the service used to display dialogs in the application.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="ISnackbar"/> service used to display notifications and messages to the user.
        /// </summary>
        /// <remarks>This property is typically injected by the dependency injection framework and should
        /// not be set manually in most cases.</remarks>
        [Inject] public ISnackbar Snackbar { get; set; } = null!;

        /// <summary>
        /// 
        /// </summary>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        [Inject] public IAreaService AreaService { get; set; } = null!;

        /// <summary>
        /// Retrieves a paginated list of lodging types based on the specified table state.
        /// </summary>
        /// <remarks>This method fetches lodging type data from the provider and maps it to a collection
        /// of <see cref="LodgingTypeDto"/> objects.</remarks>
        /// <param name="state">The table state that defines pagination and sorting parameters.</param>
        /// <param name="token">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A <see cref="TableData{T}"/> containing a collection of <see cref="LodgingTypeDto"/> objects and the total
        /// number of items.</returns>
        public async Task<TableData<AreaDto>> GetAreasAsync(TableState state, CancellationToken token)
        {
            var pagingResponse = await AreaService.AllAreasAsync(token);
            return new TableData<AreaDto>() { TotalItems = pagingResponse.Data.Count(), Items = pagingResponse.Data };
        }

        /// <summary>
        /// Displays a confirmation dialog and, if confirmed, deletes the specified area.
        /// </summary>
        /// <remarks>This method prompts the user with a confirmation dialog before attempting to delete
        /// the area.  If the user confirms, the area is deleted using the <see cref="AreaService"/>.  If the deletion
        /// fails, error messages are displayed using the <see cref="Snackbar"/>. After a successful deletion, the data
        /// table is reloaded to reflect the changes.</remarks>
        /// <param name="areaId">The unique identifier of the area to be deleted. Cannot be null or empty.</param>
        /// <returns></returns>
        private async Task Delete(string areaId)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this lodging type from this application?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var deletionResult = await AreaService.RemoveAreaAsync(areaId);
                if (!deletionResult.Succeeded) Snackbar.AddErrors(deletionResult.Messages);

                await _table.ReloadServerData();
            }
        }

        /// <summary>
        /// Displays a dialog for creating a new lodging type and processes the result.
        /// </summary>
        /// <remarks>This method opens a modal dialog to allow the user to create a new lodging type.  If
        /// the dialog is not canceled, the method sends the created lodging type data to the server  and reloads the
        /// table data to reflect the changes.</remarks>
        /// <returns></returns>
        private async Task Create()
        {
            var parameters = new DialogParameters<AreaModal>
            {
                { x => x.Area, new AreaViewModel() }
            };

            var dialog = await DialogService.ShowAsync<AreaModal>("Create Lodging Type", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var model = (AreaViewModel)result.Data!;

                var deletionResult = await AreaService.CreateAreaAsync(model.ToDto());
                if (!deletionResult.Succeeded) Snackbar.AddErrors(deletionResult.Messages);

                await _table.ReloadServerData();
            }
        }

        /// <summary>
        /// Navigates to the update page for the specified area.
        /// </summary>
        /// <param name="area">The area to be updated. The <see cref="AreaDto.Id"/> property is used to construct the navigation URL.</param>
        /// <returns></returns>
        private async Task Update(AreaDto area)
        {
            NavigationManager.NavigateTo($"areas/update/{area.Id}");
        }

        /// <summary>
        /// Asynchronously initializes the component and determines the user's authorization for lodging type
        /// operations.
        /// </summary>
        /// <remarks>This method retrieves the current user's authentication state and evaluates their
        /// permissions for searching, creating, editing, and deleting lodging types. The results are stored in internal
        /// fields to control the availability of related functionality in the component.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            var currentUser = authState.User;

            _canCreateArea = (await AuthorizationService.AuthorizeAsync(currentUser, Permissions.Settings.Create)).Succeeded;
            _canEditArea = (await AuthorizationService.AuthorizeAsync(currentUser, Permissions.Settings.Edit)).Succeeded;
            _canDeleteArea = (await AuthorizationService.AuthorizeAsync(currentUser, Permissions.Settings.Delete)).Succeeded;
            _canViewRestaurants = (await AuthorizationService.AuthorizeAsync(currentUser, Permissions.Restaurants.View)).Succeeded;


            await base.OnInitializedAsync();
        }

        /// <summary>
        /// Reloads the server data for the associated table.
        /// </summary>
        /// <remarks>This method asynchronously refreshes the data from the server, ensuring the table
        /// reflects the latest state. It is typically used to update the table's contents after changes have been made
        /// on the server.</remarks>
        /// <returns></returns>
        private async Task Reload()
        {
            await _table.ReloadServerData();
        }
    }
}
