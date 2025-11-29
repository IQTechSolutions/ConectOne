using System.Security.Claims;
using IdentityModule.Application.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace SchoolsEnterprise.Blazor.Shared.Pages.Authentication
{  

    /// <summary>
    /// The Security component handles password change functionality for a given user. 
    /// It provides:
    /// 1. A form to input the current password, a new password, and a confirmation.
    /// 2. Password visibility toggling for both the current and new passwords.
    /// 3. An interaction with an <see cref="IAccountsProvider"/> to perform the password update.
    /// 
    /// If no <see cref="UserId"/> is explicitly provided as a parameter, the component 
    /// retrieves it from the currently authenticated user.
    /// </summary>
    public partial class Security
    {
        private readonly ChangePasswordViewModel _passwordModel = new();
        private bool _currentPasswordVisibility;
        private bool _newPasswordVisibility;
        private InputType _currentPasswordInput = InputType.Password;
        private InputType _newPasswordInput = InputType.Password;
        private string _currentPasswordInputIcon = Icons.Material.Filled.VisibilityOff;
        private string _newPasswordInputIcon = Icons.Material.Filled.VisibilityOff;
        
        /// <summary>
        /// Gets or sets the <see cref="ISnackbar"/> service used to display notifications or messages to the user.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Initiates a password change request through AccountsProvider. 
        /// If successful, it resets the password fields and shows a success Snackbar message.
        /// Otherwise, it displays an error message.
        /// </summary>
        private async Task ChangePasswordAsync()
        {
            //var response = await AccountsProvider
            //    .ChangePasswordAsync(new ChangePasswordRequest(UserId, _passwordModel.CurrentPassword, _passwordModel.NewPassword));
            //if (response.Successful)
            //{
            //    SnackBar.Add("Password Changed Successfully!", Severity.Success);
            //    _passwordModel.CurrentPassword = string.Empty;
            //    _passwordModel.NewPassword = string.Empty;
            //    _passwordModel.ConfirmNewPassword = string.Empty;
            //}
            //else
            //{
            //    SnackBar.Add(response.ErrorMessage, Severity.Error);
            //}
        }               

        /// <summary>
        /// Toggles the visibility (masking) of the current or new password fields, 
        /// updating the input type and icon accordingly.
        /// </summary>
        /// <param name="newPassword">
        /// If true, toggles the new password field; if false, toggles the current password field.
        /// </param>
        private void TogglePasswordVisibility(bool newPassword)
        {
            if (newPassword)
            {
                if (_newPasswordVisibility)
                {
                    _newPasswordVisibility = false;
                    _newPasswordInputIcon = Icons.Material.Filled.VisibilityOff;
                    _newPasswordInput = InputType.Password;
                }
                else
                {
                    _newPasswordVisibility = true;
                    _newPasswordInputIcon = Icons.Material.Filled.Visibility;
                    _newPasswordInput = InputType.Text;
                }
            }
            else
            {
                if (_currentPasswordVisibility)
                {
                    _currentPasswordVisibility = false;
                    _currentPasswordInputIcon = Icons.Material.Filled.VisibilityOff;
                    _currentPasswordInput = InputType.Password;
                }
                else
                {
                    _currentPasswordVisibility = true;
                    _currentPasswordInputIcon = Icons.Material.Filled.Visibility;
                    _currentPasswordInput = InputType.Text;
                }
            }
        }

        /// <summary>
        /// Provides the AuthenticationState for the current user context, 
        /// used if no explicit UserId is supplied as a parameter.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = default!;

        /// <summary>
        /// The user ID for whom the password is being changed. 
        /// If null, the ID is fetched from the current authenticated user.
        /// </summary>
        [Parameter] public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Initializes the component, ensuring a valid UserId is set.
        /// If one is not passed in, the system will look up the current 
        /// authenticated user’s ID from the authentication context.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            // If no explicit UserId parameter is passed, 
            // retrieve the logged-in user's ID from claims.
            if (string.IsNullOrEmpty(UserId))
            {
                var authState = await AuthenticationStateTask;
                UserId = authState.User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
        }
    }
}
