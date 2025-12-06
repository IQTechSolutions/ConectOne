using ConectOne.Domain.Interfaces;
using IdentityModule.Application.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Authentication
{
    /// <summary>
    /// The SignUp component is responsible for handling user registration.
    /// It provides a form for user input and handles the registration process.
    /// </summary>
    public partial class SignUp
    {
        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used for managing navigation and URI manipulation.
        /// </summary>
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;

        /// <summary>
        /// Gets or sets the injected instance of <see cref="ISnackbar"/> used to display notifications or messages.
        /// </summary>
        [Inject] private ISnackbar SnackBar { get; set; } = default!;

        /// <summary>
        /// Gets or sets the HTTP provider used to perform HTTP operations.
        /// </summary>
        [Inject] private IBaseHttpProvider Provider { get; set; } = default!;

        /// <summary>
        /// Indicates whether the password is visible.
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
        /// Toggles the visibility of the password field.
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
        /// Indicates whether the password confirmation is visible.
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
        /// Toggles the visibility of the password confirmation field.
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
        /// ViewModel for user registration.
        /// </summary>
        private EmailRegistrationViewModel _userRegistration = new EmailRegistrationViewModel();

        /// <summary>
        /// Collection of registration error messages.
        /// </summary>
        public List<string> Errors = [];

        /// <summary>
        /// The selected role for the user.
        /// </summary>
        public string SelectedOption { get; set; } = "Basic";

        /// <summary>
        /// Handles the key up event in the input fields.
        /// Clears the error messages and updates the state.
        /// </summary>
        /// <param name="args">The keyboard event arguments.</param>
        private void OnKeyUpEvent(KeyboardEventArgs args)
        {
            Errors = [];
            StateHasChanged();
        }

        /// <summary>
        /// Handles the user registration process.
        /// </summary>
        public async Task RegisterAsync()
        {
            _userRegistration.Role = SelectedOption;
            
            var result = await Provider.PostAsync("account/register", _userRegistration.ToRegistrationRequest());

            if (!result.Succeeded)
            {
                Errors = result.Messages;
                StateHasChanged();
            }
            else
            {
                SnackBar.Add($"{_userRegistration.FirstName} {_userRegistration.LastName} Successfully Registered", Severity.Success);
                NavigationManager.NavigateTo("/");
                _userRegistration = new EmailRegistrationViewModel();
            }
        }
    }
}
