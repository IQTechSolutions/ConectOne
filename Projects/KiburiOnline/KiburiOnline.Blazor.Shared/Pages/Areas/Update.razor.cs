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
    /// Component for updating a golf course.
    /// </summary>
    public partial class Update
    {
        private string _imageSource = "_content/Accomodation.Blazor/images/NoImage.jpg";
        private AreaViewModel _area = new();

        public bool _canCreateArea;
        public bool _canUpdateArea;
        public bool _canDeleteArea;

        #region Parameters

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
        /// Gets or sets the <see cref="NavigationManager"/> instance used for managing navigation and URI manipulation
        /// in Blazor applications.
        /// </summary>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display dialogs in the application.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to calculate average temperatures.
        /// </summary>
        [Inject] public IAverageTemperatureService AverageTemperatureService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing area-related operations.
        /// </summary>
        /// <remarks>This property is automatically injected by the dependency injection framework. Ensure
        /// that the dependency is properly configured in the service container.</remarks>
        [Inject] public IAreaService AreaService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the injected <see cref="ISnackbar"/> service used to display notifications or messages.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the vacation host ID.
        /// </summary>
        [Parameter] public string AreaId { get; set; } = null!;

        #endregion

        #region Methods

        /// <summary>
        /// Displays a dialog for creating a new average temperature entry and, if confirmed, saves the entry to the
        /// data store.
        /// </summary>
        /// <remarks>This method opens a modal dialog to collect user input for a new average temperature
        /// entry.  If the user confirms the dialog, the input is validated and sent to the service for creation.  Upon
        /// successful creation, the new entry is added to the local collection of average temperatures.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task CreateAverageTemperatureAsync()
        {
            var parameters = new DialogParameters<AverageTempModal>
            {
                { x => x.AverageTemperature, new AverageTemperatureViewModel() { AreaId = AreaId} }
            };

            var dialog = await DialogService.ShowAsync<AverageTempModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var newItem = ((AverageTemperatureViewModel)result.Data).ToDto();

                var deletionResult = await AverageTemperatureService.CreateAverageTempratureAsync(newItem);
                if (!deletionResult.Succeeded) SnackBar.AddErrors(deletionResult.Messages);
                else _area.AverageTemperatures.Add(newItem);
            }
        }

        /// <summary>
        /// Updates the average temperature data asynchronously by displaying a confirmation dialog and applying the
        /// changes if confirmed.
        /// </summary>
        /// <remarks>This method displays a modal dialog to allow the user to confirm and edit the average
        /// temperature. If the user confirms the changes, the updated data is sent to the service for persistence. The
        /// method ensures that the local collection of average temperatures is updated only if the operation
        /// succeeds.</remarks>
        /// <param name="dto">The data transfer object representing the average temperature to be updated.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task UpdateAverageTemperatureAsync(AverageTemperatureDto dto)
        {
            var parameters = new DialogParameters<AverageTempModal>
            {
                { x => x.AverageTemperature, new AverageTemperatureViewModel(dto) }
            };

            var dialog = await DialogService.ShowAsync<AverageTempModal>("Confirm", parameters);
            var result = await dialog.Result;

            var index = _area.AverageTemperatures.IndexOf(dto);

            if (!result!.Canceled)
            {
                var newItem = ((AverageTemperatureViewModel)result.Data!).ToDto();

                var deletionResult = await AverageTemperatureService.UpdateAverageTempratureAsync(newItem);
                if (!deletionResult.Succeeded) SnackBar.AddErrors(deletionResult.Messages);
                else _area.AverageTemperatures[index] = newItem;
            }
        }

        /// <summary>
        /// Removes the specified average temperature from the area after user confirmation.
        /// </summary>
        /// <remarks>This method displays a confirmation dialog to the user before proceeding with the
        /// removal.  If the user confirms, the average temperature is removed from the area and the associated service
        /// is updated. If the removal operation fails, error messages are displayed in the snackbar.</remarks>
        /// <param name="dto">The data transfer object representing the average temperature to be removed. Must not be <see
        /// langword="null"/>.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task RemoveAverageTemperatureAsync(AverageTemperatureDto dto)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this average temperature from this area?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            var index = _area.AverageTemperatures.IndexOf(dto);

            if (!result!.Canceled)
            {
                var deletionResult = await AverageTemperatureService.RemoveAverageTempratureAsync(dto.Id);
                if (!deletionResult.Succeeded) SnackBar.AddErrors(deletionResult.Messages);
                else _area.AverageTemperatures.Remove(_area.AverageTemperatures[index]);
            }
        }

        /// <summary>
        /// Updates the golf course with the current data.
        /// </summary>
        private async Task UpdateAsync()
        {
            var updateResult = await AreaService.UpdateAreaAsync(_area.ToDto());
            updateResult.ProcessResponseForDisplay(SnackBar, () =>
            {
                SnackBar.Add($"Action Successful. Area \"{_area.Name}\" was successfully updated.", Severity.Success);
            });
        }

        /// <summary>
        /// Cancels the creation process and navigates back to the lodgings categories page.
        /// </summary>
        private void CancelCreation()
        {
            NavigationManager.NavigateTo("/areas");
        }

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Method invoked when the component is initialized.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            var currentUser = authState.User;

            _canCreateArea = (await AuthorizationService.AuthorizeAsync(currentUser, Permissions.Settings.Search)).Succeeded;
            _canUpdateArea = (await AuthorizationService.AuthorizeAsync(currentUser, Permissions.Settings.Create)).Succeeded;
            _canDeleteArea = (await AuthorizationService.AuthorizeAsync(currentUser, Permissions.Settings.Edit)).Succeeded;

            var result = await AreaService.AreaAsync(AreaId);
            _area = new AreaViewModel(result.Data);

            await base.OnInitializedAsync();
        }

        #endregion
    }
}
