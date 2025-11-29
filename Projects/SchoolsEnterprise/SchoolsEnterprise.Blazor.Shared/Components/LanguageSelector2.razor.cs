using ConectOne.Blazor.StateManagers;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using ConectOne.Blazor.Extensions;

namespace SchoolsEnterprise.Blazor.Shared.Components
{
    /// <summary>
    /// Represents a UI component that allows users to select and change the application's display language at runtime.
    /// </summary>
    /// <remarks>This component integrates with client preference management to persist language selection and
    /// updates the application's culture accordingly. It is typically used in Blazor applications to provide
    /// multilingual support. The component relies on dependency injection for accessing client preferences, displaying
    /// notifications, and handling navigation.</remarks>
    public partial class LanguageSelector2
    {
        private string _currentCulture = "en-ZA";

        /// <summary>
        /// Gets or sets the service responsible for managing client-specific preferences.
        /// </summary>
        [Inject] public IClientPreferenceManager ClientPreferenceManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display transient messages to the user.
        /// </summary>
        /// <remarks>This property is typically injected by the dependency injection framework. Use this
        /// service to show notifications, alerts, or other brief messages in the application's user
        /// interface.</remarks>
        [Inject] public ISnackbar Snackbar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the NavigationManager used to manage and interact with URI navigation in the application.
        /// </summary>
        /// <remarks>The NavigationManager provides methods and properties for programmatically navigating
        /// to different URIs, retrieving the current URI, and handling navigation events. This property is typically
        /// set by the Blazor framework via dependency injection.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Invoked after the component has rendered. Performs post-rendering logic, such as initializing culture
        /// settings on the first render.
        /// </summary>
        /// <remarks>Override this method to perform additional actions after the component has rendered.
        /// Use the <paramref name="firstRender"/> parameter to ensure initialization logic runs only once.</remarks>
        /// <param name="firstRender">Indicates whether this is the first time the component has been rendered. <see langword="true"/> if this is
        /// the first render; otherwise, <see langword="false"/>.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                _currentCulture = (await ClientPreferenceManager.GetPreference()).LanguageCode;
            }
        }

        /// <summary>
        /// Handles changes to the application's culture based on user selection.
        /// </summary>
        /// <remarks>If the selected culture is unchanged or invalid, the method performs no action. When
        /// the culture is changed, the application reloads to apply the new language preference.</remarks>
        /// <param name="args">The event arguments containing the newly selected culture value.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task OnCultureChanged(ChangeEventArgs args)
        {
            var selectedCulture = args.Value?.ToString();

            if (string.IsNullOrWhiteSpace(selectedCulture) || string.Equals(selectedCulture, _currentCulture, System.StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            _currentCulture = selectedCulture;

            var result = await ClientPreferenceManager.ChangeLanguageAsync(selectedCulture);
            result.ProcessResponseForDisplay(Snackbar, async () =>
            {
                NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
            });
        }
    }
}
