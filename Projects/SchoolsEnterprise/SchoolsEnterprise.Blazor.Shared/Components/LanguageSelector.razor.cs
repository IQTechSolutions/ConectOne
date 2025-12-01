using ConectOne.Blazor.StateManagers;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace SchoolsEnterprise.Blazor.Shared.Components
{
    /// <summary>
    /// The LanguageSelector component is responsible for changing the user's 
    /// preferred language in the application. It uses a ClientPreferenceManager 
    /// to persist the choice, and reloads the current page to apply the new language setting.
    /// </summary>
    public partial class LanguageSelector
    {
        /// <summary>
        /// Gets or sets the <see cref="IClientPreferenceManager"/> instance used to manage client-specific preferences.
        /// </summary>
        [Inject] public IClientPreferenceManager ClientPreferenceManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used for handling navigation in the application.
        /// </summary>
        /// <remarks>This property is typically injected by the framework and should not be manually set
        /// in most scenarios.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="ISnackbar"/> service used to display notifications or messages to the user.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Changes the application's current language by calling 
        /// <c>ClientPreferenceManager.ChangeLanguageAsync(languageCode)</c>. 
        /// If successful, a success message is displayed and the page is reloaded 
        /// to apply the new language preference. Otherwise, any error messages 
        /// are displayed.
        /// </summary>
        /// <param name="languageCode">The language code (e.g., "en", "fr").</param>
        private async Task ChangeLanguageAsync(string languageCode)
        {
            // Request the ClientPreferenceManager to change the current language 
            var result = await ClientPreferenceManager.ChangeLanguageAsync(languageCode);

            // Check if the change was successful
            if (result.Succeeded)
            {
                // Inform the user that the language was changed successfully
                SnackBar.Add(result.Messages[0], Severity.Success);

                // Force a page reload to apply the new language
                NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
            }
            else
            {
                // If unsuccessful, display error messages
                foreach (var error in result.Messages)
                {
                    SnackBar.Add(error, Severity.Error);
                }
            }
        }
    }
}