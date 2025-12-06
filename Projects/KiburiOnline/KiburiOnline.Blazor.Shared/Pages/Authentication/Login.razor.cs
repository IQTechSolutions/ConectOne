using IdentityModule.Application.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Authentication
{
    /// <summary>
    /// Represents the login component responsible for handling user authentication and navigation.
    /// </summary>
    /// <remarks>This class provides functionality for managing user login, including toggling password
    /// visibility, handling input events, and performing the login process. It integrates with the application's
    /// navigation system and account provider to facilitate authentication and redirection.</remarks>
    public partial class Login
    {
        private bool _passwordVisibility;
        private InputType _passwordInput = InputType.Password;
        private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
        private EmailAuthenticationViewModel _userAuthentication = new EmailAuthenticationViewModel();
        private string _errorMessage = null!;

        /// <summary>
        /// Gets or sets the return URL after successful login.
        /// </summary>
        [Parameter] public string? ReturnUrl { get; set; }

        /// <summary>
        /// Gets or sets the accounts provider for handling account-related operations.
        /// </summary>
      //  [Inject] public IAccountsProvider AccountsProvider { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used for handling navigation in the application.
        /// </summary>
        /// <remarks>This property is typically injected by the framework and should not be manually set
        /// in most scenarios.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

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
        /// Handles the key up event for the input fields.
        /// </summary>
        /// <param name="args">The keyboard event argument</param>
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
            try
            {
                //// Reset authentication error display
                //_errorMessage = null;

                //// Attempt to login using the provided user authentication data
                //var result = await AccountsProvider.Login(_userAuthentication.ToAuthRequest());

                //// Check if authentication was successful
                //if (!result.IsSuccessfulAuth)
                //{
                //    // If authentication failed, display the error message
                //    _errorMessage = string.IsNullOrEmpty(result.ErrorMessage) ? "Invalid Login Attempt" : result.ErrorMessage;

                //    // Trigger UI update
                //    StateHasChanged();
                //}
                //else
                //{
                //    if (result.PrivacyAndUsageTermsAcceptedTimeStamp is null && Configuration.GetSection("ApplicationConfiguration").GetValue<bool>("UsePrivacyNotice"))
                //        NavigationManager.NavigateTo("privacyNotice");
                //    else
                //    {
                //        // If authentication was successful, navigate to the return URL or default to the dashboard
                //        if (string.IsNullOrWhiteSpace(ReturnUrl))
                //        {
                //            NavigationManager.NavigateTo("/");
                //        }
                //        else
                //        {
                //            NavigationManager.NavigateTo(ReturnUrl);
                //        }
                //    }
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}