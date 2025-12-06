using Accomodation.Blazor.Modals;
using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Utilities;

namespace KiburiOnline.Blazor.Shared.Pages.Vacations
{
    /// <summary>
    /// Component for managing vacation pricing items.
    /// </summary>
    public partial class VacationPricingTable
    {
        private List<VacationPricingItemDto> _vacationPrices = new();
        private MudDropContainer<VacationPricingItemDto> _inclusionsDropBox;

        #region Injections

        /// <summary>
        /// Gets or sets the dialog service used to display dialogs and manage user interactions.
        /// </summary>
        [Inject] private IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the injected instance of <see cref="ISnackbar"/> used to display notifications or messages.
        /// </summary>
        [Inject] private ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for calculating vacation pricing details.
        /// </summary>
        /// <remarks>This property is automatically injected and should be configured in the dependency
        /// injection container.</remarks>
        [Inject] public IVacationPricingService VacationPricingService { get; set; } = null!;

        #endregion

        #region Parameters

        /// <summary>
        /// The ID of the vacation to which the pricing items belong.
        /// </summary>
        [Parameter, EditorRequired] public string VacationId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for vacation pricing.
        /// </summary>
        [Parameter] public string VacationPricingId { get; set; } = null!;

        #endregion

        #region Methods

        /// <summary>
        /// Updates the selector and order of a dropped vacation pricing item based on its drop zone identifier.
        /// </summary>
        /// <remarks>This method modifies the <see cref="VacationPricingItemDto.Selector"/> property of
        /// the dropped item to match the drop zone identifier. It also updates the order of the item within the
        /// collection of vacation pricing items, applying an offset based on the drop zone identifier.</remarks>
        /// <param name="dropItem">The dropped item containing information about the vacation pricing item and its drop zone.</param>
        private void ItemUpdated(MudItemDropInfo<VacationPricingItemDto> dropItem)
        {
            dropItem.Item.Selector = dropItem.DropzoneIdentifier;

            var indexOffset = dropItem.DropzoneIdentifier switch
            {
                "2" => _vacationPrices.Count(x => x.Selector == "1"),
                _ => 0
            };

            _vacationPrices.UpdateOrder(dropItem, item => item.Order, indexOffset);
        }

        /// <summary>
        /// Creates a new vacation pricing item.
        /// </summary>
        private async Task CreateVacationPricingItem()
        {
            var dialog = await DialogService.ShowAsync<AddVacationPricingModal>("Confirm");
            var result = await dialog.Result;
            if (!result!.Canceled)
            {
                var createdItem = ((VacationPricingItemViewModel)result.Data!).ToDto();
                
                createdItem.Order = _vacationPrices.Count + 1;
                var creationResult = await VacationPricingService.CreateVacationPriceAsync(createdItem);
                
                creationResult.ProcessResponseForDisplay(SnackBar, () =>
                {
                    _vacationPrices.Add(createdItem);
                    _inclusionsDropBox.Refresh();
                });
            }
        }

        /// <summary>
        /// Update a vacation price item.
        /// </summary>
        private async Task UpdateVacationPricingItem(VacationPricingItemDto vacationPricingItem)
        {
            var parameters = new DialogParameters<AddVacationPricingModal>
            {
                { x => x.VacationPricingItem, new VacationPricingItemViewModel(vacationPricingItem) }
            };

            var dialog = await DialogService.ShowAsync<AddVacationPricingModal>("Confirm", parameters);
            var result = await dialog.Result;
            if (!result!.Canceled)
            {
                var createdItem = ((VacationPricingItemViewModel)result.Data!).ToDto();

                createdItem.Order = _vacationPrices.Count + 1;
                var creationResult = await VacationPricingService.UpdateVacationPriceAsync(createdItem);

                creationResult.ProcessResponseForDisplay(SnackBar, () =>
                {

                    var modalResult = (VacationPricingItemViewModel)result.Data!;
                    var index = _vacationPrices.IndexOf(vacationPricingItem);
                    _vacationPrices[index] = modalResult.ToDto();
                    _inclusionsDropBox.Refresh();
                });
            }
        }

        /// <summary>
        /// Removes a vacation pricing item.
        /// </summary>
        /// <param name="vacationPricingItemId">The ID of the vacation pricing item to remove.</param>
        private async Task RemoveVacationPricingItem(string vacationPricingItemId)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this pricing item from this application?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var removalResult = await VacationPricingService.RemoveVacationPriceAsync(vacationPricingItemId);
                removalResult.ProcessResponseForDisplay(SnackBar, () =>
                {
                    _vacationPrices.Remove(_vacationPrices.FirstOrDefault(c => c.VacationPriceItemId == vacationPricingItemId)!);
                    _inclusionsDropBox.Refresh();
                });
            }
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Initializes the component and loads the vacation pricing items.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var vacationPricesResult = await VacationPricingService.VacationPricesAsync(VacationId);
            vacationPricesResult.ProcessResponseForDisplay(SnackBar, () =>
            {
                _vacationPrices = vacationPricesResult.Data.ToList();
                StateHasChanged();
            });

            await base.OnInitializedAsync();
        }

        #endregion
    }
}
