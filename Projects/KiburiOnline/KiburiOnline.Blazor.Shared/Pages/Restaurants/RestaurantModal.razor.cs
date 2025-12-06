using AccomodationModule.Application.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Restaurants
{
    /// <summary>
    /// Represents a modal dialog for managing restaurant information.
    /// </summary>
    /// <remarks>This class is designed to be used within a Blazor application and provides functionality for
    /// saving or canceling changes to a restaurant's details. It interacts with a dialog instance to handle user
    /// actions and relies on dependency injection for HTTP operations.</remarks>
    public partial class RestaurantModal
    {
        /// <summary>
        /// Gets the current instance of the dialog being managed by the component.
        /// </summary>
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

        /// <summary>
        /// Gets or sets the restaurant information to be displayed or managed.
        /// </summary>
        [Parameter] public RestaurantViewModel Restaurant { get; set; } = new RestaurantViewModel() { Id = Guid.NewGuid().ToString() };

        /// <summary>
        /// Saves the current restaurant data and closes the dialog.
        /// </summary>
        /// <remarks>This method closes the dialog and passes the current restaurant data to the caller.
        /// Ensure that the <c>Restaurant</c> object contains valid data before invoking this method.</remarks>
        private void SaveAsync()
        {
            MudDialog.Close(Restaurant);
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
    }
}