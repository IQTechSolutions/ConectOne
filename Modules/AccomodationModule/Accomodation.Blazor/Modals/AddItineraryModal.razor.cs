using AccomodationModule.Application.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Accomodation.Blazor.Modals
{
    /// <summary>
    /// Represents a modal dialog for adding a golfer package to an itinerary.
    /// </summary>
    /// <remarks>
    /// This component allows users to create or edit an itinerary for a golfer package.
    /// It uses MudBlazor for UI components and supports cancellation and saving of the itinerary.
    /// </remarks>
	public partial class AddItineraryModal  
    {
        /// <summary>
        /// Gets the current instance of the dialog being managed by the component.
        /// </summary>
        /// <remarks>This property is set via cascading parameters and is typically used internally by the
        /// component to manage dialog behavior. It should not be null during normal usage.</remarks>
		[CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for the vacation interval.
        /// </summary>
		[Parameter] public string VacationIntervalId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the itinerary data for the current view.
        /// </summary>
        [Parameter] public ItineraryViewModel? Itinerary { get; set; }

        /// <summary>
        /// Saves the current itinerary and closes the dialog.
        /// </summary>
        /// <remarks>This method finalizes the itinerary and ensures the dialog is closed. It is intended
        /// to be used when the user has completed editing or reviewing the itinerary.</remarks>
		private void SaveAsync()
		{
            MudDialog.Close(Itinerary);
        }

        /// <summary>
        /// Cancels the current dialog operation.
        /// </summary>
        /// <remarks>This method terminates the dialog and triggers any cancellation logic associated with
        /// it. Use this method to programmatically close a dialog when a cancellation is required.</remarks>
		public void Cancel()
		{
			MudDialog.Cancel();
		}

        /// <summary>
        /// Asynchronously initializes the component and sets up the itinerary data.
        /// </summary>
        /// <remarks>This method ensures that the <see cref="Itinerary"/> property is initialized with a
        /// new instance of <see cref="ItineraryViewModel"/> if it is null. The new instance is assigned a unique
        /// identifier and the current vacation interval ID. After initialization, the base implementation of  <see
        /// cref="OnInitializedAsync"/> is invoked.</remarks>
        /// <returns></returns>
		protected override async Task OnInitializedAsync()
        {
            Itinerary ??= new ItineraryViewModel
            {
                ItineraryId = Guid.NewGuid().ToString(),
                VacationIntervalId = VacationIntervalId
            };


            await base.OnInitializedAsync();
        }
	}
}
