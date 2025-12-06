using AccomodationModule.Application.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Lodgings
{
    /// <summary>
    /// Represents a modal component for creating a new room within the application.
    /// </summary>
    /// <remarks>This component provides functionality to manage the creation of a new room, including
    /// submitting the room data or canceling the operation. It interacts with a dialog instance to handle user
    /// interactions and state management.</remarks>
    public partial class RoomModal
    {
        /// <summary>
        /// Represents the current state and data of a room in the application.
        /// </summary>
        /// <remarks>This field is used internally to manage and update the room's view model.</remarks>
        [Parameter] public RoomViewModel Room { get; set; } = new();

        /// <summary>
        /// Gets the current instance of the dialog being managed by the component.
        /// </summary>
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

        /// <summary>
        /// Closes the dialog associated with the current room.
        /// </summary>
        /// <remarks>This method triggers the closure of the dialog using the specified room context.
        /// Ensure that the room context is valid before calling this method to avoid unexpected behavior.</remarks>
        public void Submit()
        {
            MudDialog.Close(Room);
        }

        /// <summary>
        /// Cancels the current dialog operation.
        /// </summary>
        /// <remarks>This method terminates the dialog and triggers the cancellation behavior. It is
        /// typically used to close a dialog without confirming any changes or actions.</remarks>
        public void Cancel()
        {
            MudDialog.Cancel();
        }
    }
}
