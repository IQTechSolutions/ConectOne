using ConectOne.Blazor.Modals;
using ConectOne.Blazor.StateManagers;
using IdentityModule.Application.ViewModels;
using IdentityModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MudBlazor;

namespace EversdalPrimary.Blazor.Pwa.Pages.Authentication
{
    /// <summary>
    /// Represents the login component responsible for handling user authentication and related operations.
    /// </summary>
    /// <remarks>This class provides functionality for managing user login, including toggling password
    /// visibility, handling key-up events, and performing the login process. It relies on various injected services
    /// such as <see cref="IAccountsProvider"/>, <see cref="IJSRuntime"/>, and <see cref="NavigationManager"/> to
    /// perform its operations. Ensure that all required dependencies are properly configured and injected before using
    /// this component.</remarks>
    public partial class Login
    {
        private bool _passwordVisibility;
        private InputType _passwordInput = InputType.Password;
        private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
        private EmailAuthenticationViewModel _userAuthentication = new EmailAuthenticationViewModel();
        private string? _errorMessage;

        /// <summary>
        /// Gets or sets the return URL after successful login.
        /// </summary>
        [Parameter] public string? ReturnUrl { get; set; }

        /// <summary>
        /// Gets or sets the accounts provider for handling account-related operations.
        /// </summary>
        [Inject] public IAccountsProvider AccountsProvider { get; set; } = null!;

        /// <summary>
        /// Gets or sets the client preference manager responsible for managing user-specific preferences.
        /// </summary>
        /// <remarks>This property is typically injected and should not be null. Ensure that the
        /// dependency injection container is configured to provide an implementation of <see
        /// cref="IClientPreferenceManager"/>.</remarks>
        [Inject] public IClientPreferenceManager ClientPreferenceManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the JavaScript runtime instance used to invoke JavaScript functions from .NET.
        /// </summary>
        /// <remarks>This property is typically injected by the Blazor framework and should be used to
        /// perform JavaScript interop operations. Ensure that the instance is not null before invoking methods on
        /// it.</remarks>
        [Inject] public IJSRuntime JsRuntime { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used to manage navigation and URI manipulation in
        /// a Blazor application.
        /// </summary>
        /// <remarks>The <see cref="NavigationManager"/> is typically used to navigate to different pages
        /// or to retrieve the current URI. This property is automatically injected by the Blazor framework.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="ISnackbar"/> service used to display notifications and messages to the user.
        /// </summary>
        /// <remarks>The <see cref="ISnackbar"/> service must be injected and is required for displaying
        /// user notifications. Ensure that the dependency injection container is properly configured to provide an
        /// implementation of <see cref="ISnackbar"/>.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        /// <remarks>The configuration data can include settings from various sources such as
        /// appsettings.json,  environment variables, or user secrets. Ensure that the configuration is properly
        /// initialized  before accessing its values.</remarks>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display dialogs in the application.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Toggles the visibility of the password input field.
        /// </summary>
        private void TogglePasswordVisibility()
        {
            if (_passwordVisibility)
            {
                _passwordVisibility = false;
                _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
                _passwordInput = InputType.Password;
            }
            else
            {
                _passwordVisibility = true;
                _passwordInputIcon = Icons.Material.Filled.Visibility;
                _passwordInput = InputType.Text;
            }
        }

        /// <summary>
        /// Handles the key-up event triggered by the user.
        /// </summary>
        /// <param name="args">The event data associated with the key-up action.</param>
        private void OnKeyUpEvent(KeyboardEventArgs args)
        {
            _errorMessage = null;
            StateHasChanged();
        }

        /// <summary>
        /// Handles the user login process.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task 
            LoginNowAsync()
        {
            // Reset authentication error display
            _errorMessage = null;

            // Attempt to login using the provided user authentication data
            var result = await AccountsProvider.Login(_userAuthentication.ToAuthRequest());

            // Check if authentication was successful
            if (!result.IsSuccessfulAuth)
            {
                if (result.ErrorMessage == "Email Address not confirmed")
                {
                    var parameters = new DialogParameters<ConformtaionModal>
                    {
                        { x => x.ContentText, "You're email was not verified yet, would you like us to resend the conformation mail?" },
                        { x => x.ButtonText, "Yes" },
                        { x => x.Color, Color.Success }
                    };

                    var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
                    var conformationModalResult = await dialog.Result;
                }
                else
                {
                    // If authentication failed, display the error message
                    _errorMessage = string.IsNullOrEmpty(result.ErrorMessage) ? "Invalid Login Attempt" : result.ErrorMessage;
                }

                // Trigger UI update
                StateHasChanged();
            }
            else
            {
                //var subscription = await JsRuntime.InvokeAsync<NotificationSubscription>("blazorPushNotifications.requestSubscription");

                //if (subscription is not null)
                //{
                //    subscription.Id = Guid.NewGuid().ToString();
                //    subscription.UserId = result.UserId;
                //    subscription.RowVersion = Array.Empty<byte>();

                //    var createSubscriptionResult = await NotificationSubscriptionService.AddSubscription(subscription);
                //    if (!createSubscriptionResult.Succeeded)
                //    {
                //        SnackBar.Add("Failed to subscribe to notifications", Severity.Error);
                //    }
                //}

                if (result.PrivacyAndUsageTermsAcceptedTimeStamp is null && Configuration.GetSection("ApplicationConfiguration").GetValue<bool>("UsePrivacyNotice"))
                    NavigationManager.NavigateTo("privacyNotice");
                else
                {
                    // If authentication was successful, navigate to the return URL or default to the dashboard
                    if (string.IsNullOrWhiteSpace(ReturnUrl))
                    {
                        NavigationManager.NavigateTo("/");
                    }
                    else
                    {
                        NavigationManager.NavigateTo(ReturnUrl);
                    }
                }
            }
        }
    }
}