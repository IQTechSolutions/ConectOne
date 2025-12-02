using ConectOne.Blazor.Extensions;
using IdentityModule.Application.ViewModels;
using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using MudBlazor;
using NelspruitHigh.Blazor.Maui.Localization;
using Plugin.Firebase.CloudMessaging;

namespace NelspruitHigh.Blazor.Maui.Pages.Authentication
{
    /// <summary>
    /// The SignIn component handles user authentication for the application.
    /// </summary>
    public partial class SignIn
    {
        private bool _passwordVisibility;
        private InputType _passwordInput = InputType.Password;
        private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
        private EmailAuthenticationViewModel _userAuthentication = new();
        public string _errorMessage;

        /// <summary>
        /// Gets or sets the accounts provider for handling account-related operations.
        /// </summary>
        [Inject] public IAccountsProvider AccountsProvider { get; set; } = null!;

        /// <summary>
        /// Gets or sets the base HTTP provider for making API requests.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the base HTTP provider for making API requests.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the navigation manager for handling navigation within the application.
        /// </summary>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the string localizer for shared resources.
        /// </summary>
        [Inject] public IStringLocalizer<SharedResource> Localizer { get; set; } = null!;
        
        /// <summary>
        /// Toggles the visibility of the password input field.
        /// </summary>
        void TogglePasswordVisibility()
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
        /// Handles the key up event for the input field.
        /// </summary>
        /// <param name="args">The keyboard event args from the button push</param>
        private void OnKeyUpEvent(KeyboardEventArgs args)
        {
            _errorMessage = null;
            StateHasChanged();
        }

        /// <summary>
        /// Handles the user login process.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task LoginNowAsync()
        {
            _errorMessage = null;
            var result = await AccountsProvider.Login(_userAuthentication.ToAuthRequest());

            if (!result.IsSuccessfulAuth)
            {
                _errorMessage = string.IsNullOrEmpty(result.ErrorMessage) ? "Invalid Login Attempt" : result.ErrorMessage;
                StateHasChanged();
            }
            else
            {
                await CrossFirebaseCloudMessaging.Current.CheckIfValidAsync();
                var token = await CrossFirebaseCloudMessaging.Current.GetTokenAsync();
                var deviceToken = new DeviceTokenDto(result.UserId, Preferences.Get("DeviceToken", ""));
                var deviceTokenResult = await AccountsProvider.SetDeviceToken(deviceToken);
                if (!deviceTokenResult.Succeeded)
                {
                    SnackBar.AddErrors(deviceTokenResult.Messages);
                }

                if (result.PrivacyAndUsageTermsAcceptedTimeStamp is null && Configuration.GetSection("ApplicationConfiguration").GetValue<bool>("UsePrivacyNotice"))
                    NavigationManager.NavigateTo("privacyNotice");
                else
                    NavigationManager.NavigateTo("/", true);
            }
        }
    }
}
