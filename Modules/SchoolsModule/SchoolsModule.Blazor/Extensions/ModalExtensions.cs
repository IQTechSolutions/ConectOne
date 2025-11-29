using ConectOne.Blazor.Modals;
using MudBlazor;

namespace SchoolsModule.Blazor.Extensions
{
    /// <summary>
    /// Provides extension methods for displaying modal dialogs and working with table state in UI components.
    /// </summary>
    /// <remarks>This static class contains helper methods that simplify common modal dialog interactions and
    /// table state manipulations, such as showing confirmation dialogs and formatting sort order strings. These methods
    /// are intended to be used as extensions in UI development scenarios.</remarks>
    public static class ModalExtensions
    {
        /// <summary>
        /// Displays a confirmation dialog with the specified content and button text, and returns a value indicating
        /// whether the user confirmed the action.
        /// </summary>
        /// <param name="dialogService">The dialog service used to display the confirmation dialog. Cannot be null.</param>
        /// <param name="contentText">The message or question to display in the confirmation dialog.</param>
        /// <param name="buttonText">The text to display on the confirmation button. Defaults to "Yes" if not specified.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the user
        /// confirmed the action; otherwise, <see langword="false"/>.</returns>
        public static async Task<bool> ShowConfirmationDialog(DialogService dialogService, string contentText, string buttonText = "Yes")
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, contentText },
                { x => x.ButtonText, buttonText },
                { x => x.Color, Color.Success }
            };

            var dialog = await dialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;
            return !result.Canceled;
        }

        /// <summary>
        /// Returns a string representing the sort order for the specified table state, or null if no sort is applied.
        /// </summary>
        /// <param name="state">The table state containing the sort label and direction to evaluate. Cannot be null.</param>
        /// <returns>A string in the format "{SortLabel} asc" or "{SortLabel} desc" if sorting is applied; otherwise, null.</returns>
        public static string? GetSortOrder(this TableState state)
        {
            if (string.IsNullOrEmpty(state.SortLabel) || state.SortDirection == SortDirection.None)
                return null;

            var direction = state.SortDirection == SortDirection.Ascending ? "asc" : "desc";
            return $"{state.SortLabel} {direction}";
        }
    }
}
