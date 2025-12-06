using AccomodationModule.Application.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Accomodation.Blazor.Modals
{
    /// <summary>
    /// Modal component for adding or editing an itinerary item.
    /// </summary>
    public partial class AddItineraryItemModal
    {
        #region Parameters

        /// <summary>
        /// The instance of the MudBlazor dialog.
        /// </summary>
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

        /// <summary>
        /// The ID of the itinerary to which the item belongs.
        /// </summary>
        [Parameter] public string? ItineraryId { get; set; }

        /// <summary>
        /// The ID of the itinerary item being edited (if applicable).
        /// </summary>
        [Parameter] public string? ItineraryItemId { get; set; }

        /// <summary>
        /// The view model for the itinerary item.
        /// </summary>
        [Parameter] public ItineraryItemViewModel? ItineraryItem { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Saves the itinerary item and closes the modal.
        /// </summary>
        private void SaveAsync()
        {
            MudDialog.Close(ItineraryItem);
        }

        /// <summary>
        /// Cancels the operation and closes the modal.
        /// </summary>
        public void Cancel()
        {
            MudDialog.Cancel();
        }

        /// <summary>
        /// Initializes the component and sets up the itinerary item view model.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            if (ItineraryItem == null)
            {
                ItineraryItem = new ItineraryItemViewModel() { ItineraryItemId = Guid.NewGuid().ToString() };

                ItineraryItem.ItineraryId = ItineraryId!;
                ItineraryItem.ItineraryItemId = ItineraryItemId!;
            }
            await base.OnInitializedAsync();
        }

        #endregion
    }
}
