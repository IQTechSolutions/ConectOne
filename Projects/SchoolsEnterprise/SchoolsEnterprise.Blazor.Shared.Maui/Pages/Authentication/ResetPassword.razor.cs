using ConectOne.Blazor.Extensions;
using IdentityModule.Application.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Authentication
{
    /// <summary>
    /// The ResetPassword component handles the password reset functionality.
    /// It allows users to reset their password by providing a new password and confirming it.
    /// </summary>
    public partial class ResetPassword
    {
        #region Injected Services

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications in the user interface.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application configuration settings.
        /// </summary>
        /// <remarks>This property is typically populated by dependency injection and provides access to
        /// configuration values such as connection strings, application settings, and environment variables.</remarks>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        #endregion

        #region Parameters

        /// <summary>
        /// Gets or sets the email address associated with the account to reset the password for.
        /// </summary>
        [Parameter, SupplyParameterFromQuery] public string UserId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the reset code used to verify the password reset request.
        /// </summary>
        [Parameter, SupplyParameterFromQuery] public string ResetCode { get; set; } = null!;

        #endregion

        #region Private Fields

        /// <summary>
        /// Indicates whether the password input field is visible.
        /// </summary>
        private bool _passwordVisibility;

        /// <summary>
        /// The input type for the password field.
        /// </summary>
        private InputType _passwordInput = InputType.Password;

        /// <summary>
        /// The icon for toggling password visibility.
        /// </summary>
        private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

        /// <summary>
        /// Indicates whether the password confirmation input field is visible.
        /// </summary>
        private bool _passwordConformationVisibility;

        /// <summary>
        /// The input type for the password confirmation field.
        /// </summary>
        private InputType _passwordConformationInput = InputType.Password;

        /// <summary>
        /// The icon for toggling password confirmation visibility.
        /// </summary>
        private string _passwordConformationInputIcon = Icons.Material.Filled.VisibilityOff;

        /// <summary>
        /// The view model for resetting the password.
        /// </summary>
        private readonly ResetPasswordViewModel _resetPasswordViewModel = new ResetPasswordViewModel();

        #endregion

        #region Methods

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
        /// Toggles the visibility of the password confirmation input field.
        /// </summary>
        private void TogglePasswordConformationVisibility()
        {
            if (_passwordConformationVisibility)
            {
                _passwordConformationVisibility = false;
                _passwordConformationInputIcon = Icons.Material.Filled.VisibilityOff;
                _passwordConformationInput = InputType.Password;
            }
            else
            {
                _passwordConformationVisibility = true;
                _passwordConformationInputIcon = Icons.Material.Filled.Visibility;
                _passwordConformationInput = InputType.Text;
            }
        }

        /// <summary>
        /// Resets the user's password by sending a reset request to the server.
        /// </summary>
        private async Task ResetNowAsync()
        {
            //var resetPasswordResponse = await AccountsProvider.ResetPassword(new ResetPasswordRequest() { Password = _resetPasswordViewModel.Password, ResetCode = ResetCode, UserId = UserId });
            //resetPasswordResponse.ProcessResponseForDisplay(SnackBar, () =>
            //{
            //    NavigationManager.NavigateTo("/sign-in");
            //    SnackBar.AddSuccess("Password was successfully updated");
            //});

        }

        /// <summary>
        /// Handles validation failure by displaying an error message.
        /// </summary>
        private void ValidationFailed()
        {
            SnackBar.AddError("Your passwords do not match");
        }

        #endregion
    }
}