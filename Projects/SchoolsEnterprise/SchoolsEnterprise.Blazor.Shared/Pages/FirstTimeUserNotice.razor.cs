using ConectOne.Blazor.Extensions;
using IdentityModule.Domain.Extensions;
using IdentityModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace SchoolsEnterprise.Blazor.Shared.Pages
{
    /// <summary>
    /// Represents a component that manages the display and acceptance of privacy and usage terms for first-time users.
    /// </summary>
    /// <remarks>This class is typically used in Blazor applications to handle the workflow for first-time
    /// users accepting privacy and usage terms. It integrates with authentication, HTTP services, and UI notifications
    /// to facilitate the process. The component relies on dependency injection for its services and is designed to
    /// operate within an authentication context.</remarks>
    public partial class FirstTimeUserNotice
    {
        /// <summary>
        /// Gets or sets the task that provides the current authentication state.
        /// </summary>
        /// <remarks>This property is typically used in Blazor components to access the user's
        /// authentication state. The value is provided by the <see cref="CascadingParameterAttribute"/> and is
        /// automatically populated when the component is within an authentication context.</remarks>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Gets or sets the HTTP provider used for making HTTP requests.
        /// </summary>
        [Inject] public IUserService UserService { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ISnackbar"/> service used to display notifications or messages to the user.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used for handling navigation and URI management in
        /// the application.
        /// </summary>
        [Inject] public NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Accepts the privacy and usage terms on behalf of the current user.
        /// </summary>
        /// <remarks>This method retrieves the current user's authentication state and sends a request to
        /// update the  privacy and usage terms acceptance timestamp. If the operation succeeds, the user is redirected 
        /// to the home page. If it fails, error messages are displayed in the UI.</remarks>
        public async Task AcceptNotice()
        {
            var authState = await AuthenticationStateTask;
            var userId = authState.User.GetUserId();

            var result = await UserService.SetPrivacyAndUsageTermsAcceptedTimeStamp(userId);
            if (!result.Succeeded) SnackBar.AddErrors(result.Messages);
            else NavigationManager.NavigateTo("/");
        }
    }
}
