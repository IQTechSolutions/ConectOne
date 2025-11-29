using ConectOne.Domain.ResultWrappers;
using MudBlazor;

namespace ConectOne.Blazor.Extensions
{
    /// <summary>
    /// Provides extension methods for processing operation results and displaying user interface feedback.
    /// </summary>
    public static class UIExtensions
    {
        /// <summary>
        /// Processes the result of an operation and displays appropriate feedback.
        /// </summary>
        /// <remarks>If the operation represented by <paramref name="response"/> is successful, the
        /// <paramref name="successAction"/> is invoked. Otherwise, error messages from <paramref name="response"/> are
        /// displayed using the provided <paramref name="snackbar"/>.</remarks>
        /// <param name="response">The result of the operation to process. Must implement <see cref="IBaseResult"/>.</param>
        /// <param name="snackbar">The snackbar instance used to display error messages.</param>
        /// <param name="successAction">The action to execute if the operation was successful.</param>
        public static void ProcessResponseForDisplay(this IBaseResult response, ISnackbar snackbar, Action successAction)
        {
            if (response.Succeeded)
            {
                successAction();
            }
            else
            {
                foreach (var error in response.Messages)
                {
                    snackbar.Add(error, Severity.Error);
                }
            }
        }
    }
}
