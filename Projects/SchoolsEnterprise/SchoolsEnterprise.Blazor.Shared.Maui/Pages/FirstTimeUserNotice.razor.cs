using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Settings;
using ConectOne.Domain.Interfaces;
using IdentityModule.Domain.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using MudBlazor;
using MudBlazor.ThemeManager;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages
{
    /// <summary>
    /// Represents a component that manages the display and acceptance of privacy and usage terms for first-time users.
    /// </summary>
    /// <remarks>This class is typically used in Blazor applications to handle the process of presenting
    /// privacy and usage terms to first-time users and recording their acceptance. It integrates with the
    /// authentication state and an HTTP provider to perform the necessary operations.</remarks>
    public partial class FirstTimeUserNotice
    {
        private ThemeManagerTheme _themeManager = new ThemeManagerTheme() { Theme = ApplciationTheme.LightTheme };
        private bool _themeManagerOpen;

        /// <summary>
        /// Gets or sets the task that provides the current authentication state.
        /// </summary>
        /// <remarks>This property is typically used in Blazor components to access the authentication
        /// state  provided by the cascading parameter. Ensure that the task is awaited to retrieve the  authentication
        /// state.</remarks>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Gets or sets the HTTP provider used for making HTTP requests.
        /// </summary>
        [Inject] public IBaseHttpProvider Provider { get; set; }

        /// <summary>
        /// Gets or sets the navigation manager used to programmatically navigate and query the current URI within the
        /// application.
        /// </summary>
        /// <remarks>The navigation manager provides methods for navigating to different URIs and for
        /// retrieving information about the current navigation state. This property is typically set by the framework
        /// through dependency injection.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Gets or sets the service used to display transient messages to the user.
        /// </summary>
        /// <remarks>Typically used to show notifications, alerts, or feedback messages within the
        /// application's user interface. The implementation of ISnackbar determines how messages are presented and
        /// managed.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; }

        /// <summary>
        /// Gets or sets the application configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; }

        /// <summary>
        /// Accepts the privacy and usage terms on behalf of the current user.
        /// </summary>
        /// <remarks>This method retrieves the current user's authentication state and sends a request to
        /// update the  acceptance timestamp for the privacy and usage terms. If the operation succeeds, the user is 
        /// redirected to the application's home page. If it fails, error messages are displayed in the UI.</remarks>
        public async Task AcceptNotice()
        {
            var authState = await AuthenticationStateTask;
            var userId = authState.User.GetUserId();

            var result = await Provider.PostAsync($"account/setPrivacyAndUsageTermsAcceptedTimeStamp/{userId}");
            if (!result.Succeeded) SnackBar.AddErrors(result.Messages);
            else NavigationManager.NavigateTo("/");
        }
    }
}
