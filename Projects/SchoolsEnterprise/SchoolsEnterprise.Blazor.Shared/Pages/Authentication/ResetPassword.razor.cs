using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Interfaces;
using IdentityModule.Application.ViewModels;
using IdentityModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace SchoolsEnterprise.Blazor.Shared.Pages.Authentication
{
    /// <summary>
    /// The ResetPassword component handles the password reset functionality.
    /// It allows users to reset their password by providing a new password and confirming it.
    /// </summary>
    public partial class ResetPassword
    {
        #region Private Fields

        private bool _passwordVisibility;
        private InputType _passwordInput = InputType.Password;
        private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

        private bool _passwordConformationVisibility;
        private InputType _passwordConformationInput = InputType.Password;
        private string _passwordConformationInputIcon = Icons.Material.Filled.VisibilityOff;

        private ResetPasswordViewModel _resetPasswordViewModel = new ResetPasswordViewModel();

        #endregion

        #region Injected Services

        /// <summary>
        /// Gets or sets the accounts provider for handling account-related operations.
        /// </summary>
        [Inject] public IBaseHttpProvider Provider { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used for handling navigation and URI management in
        /// the application.
        /// </summary>
        /// <remarks>This property is typically injected by the dependency injection framework and should
        /// not be manually set in most cases.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="ISnackbar"/> service used to display notifications or messages to the user.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

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
            var resetPasswordResponse = await Provider.PostAsync<string, ResetPasswordRequest>("account/reset", new ResetPasswordRequest() { Password = _resetPasswordViewModel.Password, ResetCode = ResetCode, UserId = UserId });
            resetPasswordResponse.ProcessResponseForDisplay(SnackBar, () =>
            {
                NavigationManager.NavigateTo("/login");
                SnackBar.AddSuccess("Password was successfully updated");
            });
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
