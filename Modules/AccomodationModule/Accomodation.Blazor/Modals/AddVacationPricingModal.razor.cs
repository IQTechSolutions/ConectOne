using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Accomodation.Blazor.Modals
{
    /// <summary>
    /// Represents a modal for adding vacation pricing items.
    /// </summary>
    public partial class AddVacationPricingModal
    {
        #region Parameters

        /// <summary>
        /// Gets or sets the MudDialog instance for this modal.
        /// </summary>
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

        [Inject] public IVacationPricingService VacationPricingService { get; set; } = null!;

        #endregion

        #region Private Fields

        /// <summary>
        /// An array of price categories.
        /// </summary>
        [Parameter] public List<string>? PriceCategories { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for a vacation.
        /// </summary>
        [Parameter] public string? VacationId { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the VacationPricingItem for this modal.
        /// </summary>
        [Parameter] public VacationPricingItemViewModel VacationPricingItem { get; set; } = new VacationPricingItemViewModel() { VacationPriceItemId = Guid.NewGuid().ToString() };

        #endregion

        #region Methods

        /// <summary>
        /// Saves the vacation pricing item and closes the modal.
        /// </summary>
        private void SaveAsync()
        {
            MudDialog.Close(VacationPricingItem);
        }

        /// <summary>
        /// Cancels the modal.
        /// </summary>
        public void Cancel()
        {
            MudDialog.Cancel();
        }
        
        /// <summary>
        /// Asynchronously initializes the component and retrieves vacation price categories.
        /// </summary>
        /// <remarks>This method fetches price categories for the specified vacation using the <see
        /// cref="Provider"/> service.  If no categories are retrieved or the operation fails, default categories
        /// ("Golfer" and "Non-Golfer") are used.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            if (PriceCategories == null)
            {
                var result = await VacationPricingService.VacationPricingGroupsAsync(VacationId);
                if(result == null || !result.Succeeded || !result.Data.Any())  PriceCategories = ["Golfer", "Non-Golfer"];
                else PriceCategories = result.Data.Select(c => c.Name).ToList();
            }

            await base.OnInitializedAsync();
        }

        #endregion
    }
}