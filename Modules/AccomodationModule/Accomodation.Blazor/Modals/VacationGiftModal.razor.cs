using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Accomodation.Blazor.Modals
{
    /// <summary>
    /// Represents a modal dialog for adding a vacation interval.
    /// This component allows users to input details for a vacation interval and save or cancel the operation.
    /// </summary>
    public partial class VacationGiftModal
    {
        private List<GiftDto> _availableGiftTemplates = [];
        private readonly Func<GiftDto?, string?> _giftTemplateConverter = p => p?.Name;

        #region Parameters

        /// <summary>
        /// Gets or sets the MudBlazor dialog instance for managing the modal's state.
        /// </summary>
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing gift-related operations.
        /// </summary>
        [Inject] public IGiftService GiftService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the ID of the vacation associated with the interval.
        /// </summary>
        [Parameter] public string VacationId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the vacation interval view model containing the interval details.
        /// </summary>
        [Parameter] public RoomGiftViewModel? Gift { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Saves the vacation interval and closes the modal dialog.
        /// </summary>
        private void SaveAsync()
        {
            MudDialog.Close(Gift);
        }

        /// <summary>
        /// Cancels the operation and closes the modal dialog.
        /// </summary>
        private void Cancel()
        {
            MudDialog.Cancel();
        }

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Called when the component is initialized. Loads the available lodgings from the server.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            Gift ??= new RoomGiftViewModel() { VacationId = VacationId };

            var giftTemplateListResult = await GiftService.GiftListAsync();
            if (giftTemplateListResult.Succeeded)
            {
                _availableGiftTemplates = giftTemplateListResult.Data.ToList();
            }

            await base.OnInitializedAsync();
        }

        #endregion
    }
}
