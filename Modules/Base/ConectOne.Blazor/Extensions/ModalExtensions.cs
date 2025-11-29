using ConectOne.Blazor.Modals;
using MudBlazor;

namespace ConectOne.Blazor.Extensions
{
    /// <summary>
    /// Provides extension methods for modal dialogs.
    /// </summary>
    public static class ModalExtensions
    {
        /// <summary>
        /// Shows a confirmation dialog with the specified message and returns the user's response.
        /// </summary>
        /// <param name="dialogService">The dialog service to use for showing the dialog.</param>
        /// <param name="message">The message to display in the confirmation dialog.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean value indicating whether the user confirmed the action.</returns>
        public static async Task<bool> ConfirmAction(this IDialogService dialogService, string message)
        {
            // Define the parameters for the confirmation dialog
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, message },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            // Show the confirmation dialog
            var dialog = await dialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            // Return true if the user confirmed the action, otherwise false
            return !result!.Canceled;
        }
    }
}
