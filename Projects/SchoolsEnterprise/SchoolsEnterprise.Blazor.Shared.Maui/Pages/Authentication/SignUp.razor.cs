using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Interfaces;
using IdentityModule.Application.ViewModels;
using IdentityModule.Domain.DataTransferObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Configuration;
using MudBlazor;
using SchoolsModule.Domain.DataTransferObjects;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Authentication
{
    /// <summary>
    /// The SignUp component is responsible for handling user registration.
    /// It provides a form for user input and handles the registration process.
    /// </summary>
    public partial class SignUp
    {
        private List<RoleDto> _availableRoles = [];
        private readonly Func<RoleDto?, string> _roleConverter = p => p?.Name ?? "";

        /// <summary>
        /// Injected HTTP provider for making API requests.
        /// </summary>
        [Inject] public IBaseHttpProvider Provider { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display transient messages to the user.
        /// </summary>
        /// <remarks>This property is typically injected by the framework and provides methods for showing
        /// snackbars or notifications within the user interface.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the NavigationManager used to manage and interact with URI navigation in the application.
        /// </summary>
        /// <remarks>The NavigationManager provides methods and properties for programmatically navigating
        /// to different URIs, retrieving the current URI, and handling navigation events. This property is typically
        /// injected by the Blazor framework.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

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
        public List<string> _errors = [];

        /// <summary>
        /// The selected role for the user.
        /// </summary>
        public RoleDto SelectedOption { get; set; }

        /// <summary>
        /// Handles the key-up event triggered by the keyboard.
        /// </summary>
        /// <param name="args">The <see cref="KeyboardEventArgs"/> containing details about the key-up event, such as the key pressed and
        /// any modifier keys.</param>
        private void OnKeyUpEvent(KeyboardEventArgs args)
        {
            _errors = [];
            StateHasChanged();
        }

        /// <summary>
        /// Handles the event triggered when the selected role changes.
        /// </summary>
        /// <remarks>This method updates the selected role, clears any existing errors, and refreshes the
        /// component's state.</remarks>
        /// <param name="role">The newly selected role. Cannot be null.</param>
        private void OnRoleSelectionChanged(RoleDto role)
        {
            _errors = [];
            SelectedOption = role;
            StateHasChanged();
        }

        /// <summary>
        /// Handles the user registration process.
        /// </summary>
        public async Task RegisterAsync()
        {
            _userRegistration.Role = SelectedOption.Name;

            if (SelectedOption.Name == "Parent")
            {
                var parentResult = await Provider.GetAsync<ParentDto>($"parents/exist/{_userRegistration.Email}");
                if (parentResult.Succeeded && parentResult.Data is not null)
                {
                    _userRegistration.UserId = parentResult.Data.ParentId;
                }
                else
                {
                    _errors.Add("Parent does not exist");
                    StateHasChanged();
                    return;
                }
            }
            if (SelectedOption.Name == "Learner")
            {
                var learnerResult = await Provider.GetAsync<string>($"learners/exist/{_userRegistration.Email}");
                if (learnerResult.Succeeded && !string.IsNullOrEmpty(learnerResult.Data))
                {
                    _userRegistration.UserId = learnerResult.Data!;
                }
                else
                {
                    _errors.Add("Learner does not exist");
                    StateHasChanged();
                    return;
                }
            }
            if (SelectedOption.Name == "Teacher")
            {
                var teacherResult = await Provider.GetAsync<string>($"teachers/exist/{_userRegistration.Email}");
                if (teacherResult.Succeeded && !string.IsNullOrEmpty(teacherResult.Data))
                {
                    _userRegistration.UserId = teacherResult.Data;
                }
                else
                {
                    _errors.Add("Teacher does not exist");
                    StateHasChanged();
                    return;
                }
            }

            var result = await Provider.PostAsync("account/register", _userRegistration.ToRegistrationRequest(Configuration.GetSection("ApplicationConfiguration:").GetValue<bool>("ConfirmEmailBeforeLogin")));

            if (!result.Succeeded)
            {
                _errors = result.Messages;
                StateHasChanged();
            }
            else
            {
                SnackBar.Add($"{_userRegistration.FirstName} {_userRegistration.LastName} Successfully Registered", Severity.Success);
                NavigationManager.NavigateTo("/");
                _userRegistration = new EmailRegistrationViewModel();
            }
        }

        /// <summary>
        /// Asynchronously initializes the component and retrieves the list of available roles.
        /// </summary>
        /// <remarks>This method fetches role data from the server and filters out roles that are not
        /// available  for registration selection. It also sets the default selected role to "Parent" if
        /// available.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var response = await Provider.GetAsync<IEnumerable<RoleDto>>($"account/roles");
            response.ProcessResponseForDisplay(SnackBar, () =>
            {
                _availableRoles = response.Data.Where(c => !c.NotAvailableForRegistrationSelection).ToList();
            });

            SelectedOption = _availableRoles.FirstOrDefault(c => c.Name == "Parent");
            await base.OnInitializedAsync();
        }
    }
}
