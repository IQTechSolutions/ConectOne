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

namespace KiburiOnline.Blazor.Shared.Pages.LodgingTypes
{
    /// <summary>
    /// Represents a Blazor component for managing a list of lodging types.
    /// </summary>
    /// <remarks>The <see cref="LodgingTypeList"/> component provides functionality for displaying, creating,
    /// editing,  and deleting lodging types. It integrates with various services, such as authorization, HTTP
    /// communication,  and UI dialogs, to enable these operations. This component is designed to work with server-side
    /// data  and supports pagination, sorting, and user authentication checks.</remarks>
    public partial class LodgingTypeList
    {
        private MudTable<LodgingTypeDto> _table = null!;

        private bool _canCreateLodgingTypes;
        private bool _canEditLodgingTypes;
        private bool _canDeleteLodgingTypes;

        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

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
        /// Gets or sets the HTTP provider used for making HTTP requests.
        /// </summary>
        /// <remarks>This property is typically injected via dependency injection and must be set to a
        /// non-null value before use. Ensure that the provided implementation satisfies the application's HTTP
        /// communication requirements.</remarks>
        [Inject] public ILodgingTypeService Provider { get; set; } = null!;

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
        /// Retrieves a paginated list of lodging types based on the specified table state.
        /// </summary>
        /// <remarks>This method fetches lodging type data from the provider and maps it to a collection
        /// of <see cref="LodgingTypeDto"/> objects.</remarks>
        /// <param name="state">The table state that defines pagination and sorting parameters.</param>
        /// <param name="token">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A <see cref="TableData{T}"/> containing a collection of <see cref="LodgingTypeDto"/> objects and the total
        /// number of items.</returns>
        public async Task<TableData<LodgingTypeDto>> GetLodgingTypesAsync(TableState state, CancellationToken token)
        {
            var pagingResponse = await Provider.AllLodgingTypesAsync();
            return new TableData<LodgingTypeDto>() { TotalItems = pagingResponse.Data.Count(), Items = pagingResponse.Data.Select(c => new LodgingTypeDto(c.Id, c.Name, c.Description)) };
        }

        /// <summary>
        /// Deletes a golf course with the specified identifier after user confirmation.
        /// </summary>
        /// <remarks>This method displays a confirmation dialog to the user before proceeding with the
        /// deletion.  If the user confirms, the golf course is deleted using the underlying data provider.  If the
        /// deletion fails, error messages are displayed in the UI.</remarks>
        /// <param name="lodgingTypeId">The unique identifier of the lodging type to delete. Cannot be null or empty.</param>
        /// <returns></returns>
        private async Task Delete(string lodgingTypeId)
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
                var deletionResult = await Provider.RemoveLodgingTypeAsync(lodgingTypeId);
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
            var parameters = new DialogParameters<LodgingTypeModal>
            {
                { x => x.LodgingType, new LodgingTypeViewModel() }
            };

            var dialog = await DialogService.ShowAsync<LodgingTypeModal>("Create Lodging Type", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var model = (LodgingTypeViewModel)result.Data;

                var deletionResult = await Provider.CreateLodgingTypeAsync(new LodgingTypeDto(model.Id, model.Name, model.Description));
                if (!deletionResult.Succeeded) Snackbar.AddErrors(deletionResult.Messages);

                await _table.ReloadServerData();
            }
        }

        /// <summary>
        /// Updates the lodging type information by displaying a confirmation dialog and processing the user's input.
        /// </summary>
        /// <remarks>This method opens a modal dialog to confirm and edit the lodging type details. If the
        /// user confirms the changes,  the updated data is sent to the server for processing. If the operation fails,
        /// error messages are displayed in a snackbar.</remarks>
        /// <param name="lodging">The lodging type data to be updated. Cannot be null.</param>
        /// <returns></returns>
        private async Task Update(LodgingTypeDto lodging)
        {
            var parameters = new DialogParameters<LodgingTypeModal>
            {
                { x => x.LodgingType, new LodgingTypeViewModel(lodging) }
            };

            var dialog = await DialogService.ShowAsync<LodgingTypeModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var model = (LodgingTypeViewModel)result.Data;

                var deletionResult = await Provider.UpdateLodgingTypeAsync(new LodgingTypeDto(model.Id, model.Name, model.Description));
                if (!deletionResult.Succeeded) Snackbar.AddErrors(deletionResult.Messages);

                await _table.ReloadServerData();
            }
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

            _canCreateLodgingTypes = (await AuthorizationService.AuthorizeAsync(currentUser, Permissions.LodgingTypes.Create)).Succeeded;
            _canEditLodgingTypes = (await AuthorizationService.AuthorizeAsync(currentUser, Permissions.LodgingTypes.Edit)).Succeeded;
            _canDeleteLodgingTypes = (await AuthorizationService.AuthorizeAsync(currentUser, Permissions.LodgingTypes.Delete)).Succeeded;


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
