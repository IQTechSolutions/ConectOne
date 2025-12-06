// ReSharper disable MustUseReturnValue

using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using KiburiOnline.Blazor.Shared.Pages.Vacations.Modals;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Vacations
{
    /// <summary>
    /// Component for creating a new vacation package.
    /// </summary>
    public partial class Create
    {
        private VacationViewModel _vacation = new();
        private int _selectedTab;

        #region Injected Services

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
        /// Gets or sets the service used to display snack bar notifications.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing vacation-related operations.
        /// </summary>
        /// <remarks>This property is automatically injected by the dependency injection framework. Ensure
        /// that a valid  implementation of <see cref="IVacationService"/> is registered in the service
        /// container.</remarks>
        [Inject] public IVacationService VacationService { get; set; } = null!;

        #endregion

        #region Parameters

        /// <summary>
        /// The ID of the vacation host associated with the vacation package.
        /// </summary>
        [Parameter] public string? VacationHostId { get; set; }

        #endregion

        #region Itinerary Entry Item Templates

        /// <summary>
        /// Displays a dialog to create a new vacation pricing item, processes the user's input,  and adds the created
        /// item to the vacation's pricing list if the operation succeeds.
        /// </summary>
        /// <remarks>This method opens a modal dialog for the user to input details for a new vacation
        /// pricing item.  If the user confirms the operation, the item is created and sent to the server for
        /// persistence.  Upon successful creation, the item is added to the local vacation pricing list and associated 
        /// dropdown items. If the operation fails, error messages are displayed to the user.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task CreateItineraryEntryItemTemplate()
        {
            var parameters = new DialogParameters<ItineraryItemTemplateModal>
            {
                { x => x.Template, new ItineraryEntryItemTemplateViewModel() { Id = Guid.NewGuid().ToString(), DayNr = _vacation.ItineraryEntryItemTemplates.Count() + 1, VacationId = _vacation.VacationId }}
            };
            var dialog = await DialogService.ShowAsync<ItineraryItemTemplateModal>("Create Itinerary Item", parameters);
            var result = await dialog.Result;
            if (!result!.Canceled)
            {
                var createdItem = ((ItineraryEntryItemTemplateViewModel)result.Data).ToDto();
                _vacation.ItineraryEntryItemTemplates.Add(createdItem);
                _vacation.Nights = _vacation.ItineraryEntryItemTemplates.Count - 1;
            }
        }

        /// <summary>
        /// Updates a vacation price group item by displaying a confirmation dialog, creating a new item based on user
        /// input,  and adding it to the vacation pricing list if the operation succeeds.
        /// </summary>
        /// <remarks>This method displays a modal dialog to confirm the addition of a new vacation price
        /// group item. If the user confirms,  the method creates a new item, assigns it an order, and sends it to the
        /// server for persistence. Upon successful creation,  the item is added to the local vacation pricing list and
        /// associated dropdown items. If the operation fails, error messages  are displayed to the user.</remarks>
        /// <param name="dto">The data transfer object containing information about the vacation price group to be updated.</param>
        private async Task UpdateItineraryEntryItemTemplate(ItineraryEntryItemTemplateDto dto)
        {
            var parameters = new DialogParameters<ItineraryItemTemplateModal>
            {
                { x => x.Template, new ItineraryEntryItemTemplateViewModel(dto) }
            };
            var dialog = await DialogService.ShowAsync<ItineraryItemTemplateModal>("Update Itinerary Item", parameters);

            var result = await dialog.Result;
            if (!result!.Canceled)
            {
                var createdItem = ((ItineraryEntryItemTemplateViewModel)result.Data!).ToDto();

                var index = _vacation.ItineraryEntryItemTemplates.IndexOf(dto);
                _vacation.ItineraryEntryItemTemplates[index] = createdItem;
            }
        }

        /// <summary>
        /// Removes a vacation price group item based on the provided data transfer object (DTO).
        /// </summary>
        /// <remarks>This method displays a confirmation dialog before proceeding with the removal. If the
        /// operation is confirmed,  the item is removed from the vacation price group and the associated data
        /// structures are updated accordingly.</remarks>
        /// <param name="dto">The <see cref="AccomodationModule.Domain.DataTransferObjects.VacationPriceGroupDto"/> representing the vacation price group item to be removed.</param>
        private async Task RemoveItineraryEntryItemTemplate(ItineraryEntryItemTemplateDto dto)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this template from this vacation?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                _vacation.ItineraryEntryItemTemplates.Remove(dto);
            }
        }

        #endregion

        #region Create and Cancel Methods

        /// <summary>
        /// Creates the vacation package.
        /// </summary>
        private async Task CreateAsync(int? tabIndex = null)
        {
            if(_vacation.VacationTitle is null)
            {
                SnackBar.AddError("Vacation title is a required field.");
                return;
            }

            var creationResult = await VacationService.CreateAsync(_vacation.ToDto(), "");
            if (!creationResult.Succeeded) SnackBar.AddErrors(creationResult.Messages);

            SnackBar.Add($"Action Successful. Vacation package \"{_vacation.VacationTitle.VacationTitle}\" was successfully created.", Severity.Success);

            if (tabIndex is not null)
                NavigationManager.NavigateTo($"/packages/update/{_vacation.VacationId}/{tabIndex}");
            else
                NavigationManager.NavigateTo($"/packages/update/{_vacation.VacationId}");

        }

        /// <summary>
        /// Cancel the creation of the vacation package.
        /// </summary>
        private void CancelCreation()
        {
            NavigationManager.NavigateTo("/packages");
        }

        /// <summary>
        /// Initializes the component.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        #endregion
    }
}
