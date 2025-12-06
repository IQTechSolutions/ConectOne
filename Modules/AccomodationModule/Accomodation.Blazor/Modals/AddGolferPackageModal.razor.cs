using AccomodationModule.Application.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Accomodation.Blazor.Modals
{
    /// <summary>
    /// Represents a modal dialog for adding a golfer package to an itinerary.
    /// </summary>
    /// <remarks>This component is designed to be used within a Blazor application and provides functionality
    /// for creating or modifying an itinerary associated with a vacation interval.</remarks>
	public partial class AddGolferPackageModal
    {
        /// <summary>
        /// Gets the current instance of the dialog being managed by the component.
        /// </summary>
        /// <remarks>This property is typically used internally by the component to interact with the
        /// dialog. It is set via cascading parameters and should not be modified directly.</remarks>
		[CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

        /// <summary>
        /// Gets or sets the ID of the vacation interval associated with the itinerary.
        /// </summary>
        /// <remarks>This parameter is required for the component to function correctly and should be provided
        /// when the component is instantiated.</remarks>
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
        /// it. Use this method to programmatically close a dialog when a cancellation action is required.</remarks>
		public void Cancel()
		{
			MudDialog.Cancel();
		}

        /// <summary>
        /// Performs asynchronous initialization when the component is first rendered.
        /// </summary>
        /// <remarks>This method initializes the <see cref="Itinerary"/> property if it is null, assigning
        /// it a new instance of  <see cref="ItineraryViewModel"/> with a unique identifier and the associated vacation
        /// interval ID.  It also invokes the base class's asynchronous initialization logic.</remarks>
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
