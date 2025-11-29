using MudBlazor;

namespace ConectOne.Blazor.Extensions
{
    /// <summary>
    /// Provides extension methods for the ISnackbar interface to simplify adding success and error messages.
    /// </summary>
    /// <remarks>These extension methods offer a convenient way to display standardized success and error
    /// notifications using an ISnackbar implementation. They help ensure consistent message severity and reduce
    /// repetitive code when working with snackbars.</remarks>
    public static class SnackbarExtensions
    {
        /// <summary>
        /// Displays a success message in the specified snackbar using the standard success severity.
        /// </summary>
        /// <param name="snackbar">The snackbar instance in which to display the message. Cannot be null.</param>
        /// <param name="message">The message text to display as a success notification. Cannot be null or empty.</param>
        public static void AddSuccess(this ISnackbar snackbar, string message)
        {
            snackbar.Add(message, Severity.Success);
        }

        /// <summary>
        /// Displays an error message in the specified snackbar with error severity.
        /// </summary>
        /// <param name="snackbar">The snackbar instance in which to display the error message. Cannot be null.</param>
        /// <param name="message">The error message to display. Cannot be null or empty.</param>
        public static void AddError(this ISnackbar snackbar, string message)
        {
            snackbar.Add(message, Severity.Error);
        }

        /// <summary>
        /// Adds multiple error messages to the specified snackbar for display.
        /// </summary>
        /// <param name="snackbar">The snackbar instance to which the error messages will be added. Cannot be null.</param>
        /// <param name="messages">A list of error messages to add to the snackbar. Each message in the list will be displayed as a separate
        /// error. Cannot be null.</param>
        public static void AddErrors(this ISnackbar snackbar, List<string> messages)
        {
            foreach (var message in messages)
            {
                snackbar.AddError(message);
            }
        }
    }
}
