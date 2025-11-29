using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using SchoolsModule.Domain.DataTransferObjects;
using System.ComponentModel.DataAnnotations;
using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Interfaces;
using IdentityModule.Application.ViewModels;
using IdentityModule.Domain.DataTransferObjects;
using Microsoft.Extensions.Configuration;

namespace SchoolsEnterprise.Blazor.Shared.Pages.Authentication
{
    /// <summary>
    /// The Register component allows a new user (Parent, Learner, or Teacher) to register
    /// by providing their email, name, and password. It verifies whether the email matches 
    /// a known record in the system (e.g., a Parent, Learner, or Teacher) before proceeding 
    /// with registration.
    /// 
    /// Key Highlights:
    /// 1. Dynamically checks against ParentsProvider, LearnersProvider, or TeachersProvider 
    ///    to ensure the email is valid for the chosen user type.
    /// 2. Shows error messages in a Snackbar and on the form if validation fails.
    /// 3. Optionally toggles password visibility in the UI.
    /// 4. Navigates to the login page upon successful registration.
    /// </summary>
    public partial class Register
    {
        private EmailRegistrationViewModel userRegistration = new EmailRegistrationViewModel();
        public List<string> _errors = [];
        private bool _passwordVisibility;
        private InputType _passwordInput = InputType.Password;
        private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

        private List<RoleDto> _availableRoles = [];
        private readonly Func<RoleDto?, string> _roleConverter = p => p?.Name ?? "";

        private string PhoneNumber
        {
            get => userRegistration.PhoneNumber;
            set
            {
                var digitsOnly = new string((value ?? string.Empty).Where(char.IsDigit).ToArray());
                var hasChanged = !string.Equals(userRegistration.PhoneNumber, digitsOnly, StringComparison.Ordinal);

                userRegistration.PhoneNumber = digitsOnly;

                if (hasChanged)
                {
                    StateHasChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the HTTP provider used for making HTTP requests.
        /// </summary>
        /// <remarks>This property is typically injected via dependency injection and must be set before
        /// use.</remarks>
        [Inject] public IBaseHttpProvider Provider { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used for handling navigation in the application.
        /// </summary>
        /// <remarks>This property is typically injected by the framework and should not be manually set
        /// in most scenarios.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        /// <remarks>The configuration data may include settings from various sources such as
        /// appsettings.json, environment variables, or user secrets. Ensure that the property is properly initialized
        /// before accessing its values.</remarks>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="ISnackbar"/> service used to display notifications or messages to the user.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        // The selected role type (Parent, Learner, or Teacher). Used to determine which 
        // provider (ParentsProvider, LearnersProvider, TeachersProvider) to call.
        public RoleDto SelectedOption { get; set; }

        /// <summary>
        /// Provides a static instance of <see cref="EmailAddressAttribute"/> for validating email addresses.
        /// </summary>
        /// <remarks>This instance can be used to validate whether a given string conforms to the format
        /// of a valid email address.</remarks>
        private static readonly EmailAddressAttribute EmailValidator = new();

        /// <summary>
        /// Handles the key-up event triggered by the user.
        /// </summary>
        /// <param name="args">The <see cref="KeyboardEventArgs"/> containing information about the key-up event, such as the key pressed
        /// and any modifier keys.</param>
        private void OnKeyUpEvent(KeyboardEventArgs args)
        {
            _errors = [];
            StateHasChanged();
        }

        /// <summary>
        /// Handles changes to the state of a radio button.
        /// </summary>
        /// <param name="args">A string representing the arguments or state associated with the radio button change. Cannot be null.</param>
        private void OnRadioButtonChanged(string args)
        {
            _errors = [];
            StateHasChanged();
        }

        /// <summary>
        /// Validates the specified phone number and returns any validation errors.
        /// </summary>
        /// <remarks>A valid phone number must contain only digit characters. If the input is <see
        /// langword="null"/>, empty, or consists solely of whitespace, no validation errors are returned.</remarks>
        /// <param name="value">The phone number to validate. Can be <see langword="null"/> or empty.</param>
        /// <returns>An enumerable collection of validation error messages. Returns an empty collection if the phone number is
        /// valid or not provided.</returns>
        private IEnumerable<string> ValidatePhoneNumber(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                yield break;
            }

            if (!value.All(char.IsDigit))
            {
                yield return "Phone Number must contain only digits.";
            }
        }

        private IEnumerable<string> ValidateEmail(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                yield break;
            }

            if (!EmailValidator.IsValid(value))
            {
                yield return "Please enter a valid email address.";
            }
        }

        /// <summary>
        /// Attempts to register a user based on the selected role (Parent, Learner, Teacher). 
        /// Validates that the provided email corresponds to a known record in the system. 
        /// If successful, redirects to the login page.
        /// </summary>
        public async Task RegisterAsync()
        {
            // Generate a new user ID (though it may be overridden if a matching record is found).
            var userId = Guid.NewGuid().ToString();

            // If the user selected "Parent", check if there's a parent record with the given email.
            if (SelectedOption.Name == "Parent")
            {
                var parentResponse = await Provider.GetAsync<ParentDto>($"parents/exist/{userRegistration.Email}");
                if (!parentResponse.Succeeded)
                {
                    var message = $"No parent matching email address '{userRegistration.Email}'";
                    _errors = [message];
                    StateHasChanged();
                    return;
                }
                userRegistration.UserId = parentResponse.Data.ParentId;
            }

            // If the user selected "Learner", check if there's a learner record with the given email.
            if (SelectedOption.Name == "Learner")
            {
                var learnerResponse = await Provider.GetAsync<LearnerDto>($"learners/exist/{userRegistration.Email}");
                if (!learnerResponse.Succeeded)
                {
                    var message = $"No learner matching email address '{userRegistration.Email}'";
                    _errors = new List<string> { message };
                    StateHasChanged();
                    return;
                }
                userRegistration.UserId = learnerResponse.Data.LearnerId;
            }

            // If the user selected "Teacher", check if there's a teacher record with the given email.
            if (SelectedOption.Name == "Teacher")
            {
                var teacherResponse = await Provider.GetAsync<TeacherDto>($"teachers/exist/{userRegistration.Email}");
                if (!teacherResponse.Succeeded)
                {
                    var message = $"No teacher matching email address '{userRegistration.Email}'";
                    _errors = new List<string> { message }; StateHasChanged();
                    return;
                }
                userRegistration.UserId = teacherResponse.Data.TeacherId;
            }

            // Set the Role property on the user registration to the selected option (Parent, Learner, or Teacher).
            userRegistration.Role = SelectedOption.Name;

            // Package the registration data into a RegistrationRequest object.
            var content = userRegistration.ToRegistrationRequest(Configuration.GetSection("ApplicationConfiguration:").GetValue<bool>("ConfirmEmailBeforeLogin"));
            
            // Call the AccountsProvider to register the user.
            var result = await Provider.PostAsync("account/register", content); ;

            // If registration fails, set the errors to be displayed in the UI.
            if (!result.Succeeded)
            {
                _errors = result.Messages;
                StateHasChanged();
            }
            else
            {
                // On success, notify the user and navigate to the login page.
                SnackBar.Add(
                    $"{userRegistration.FirstName} {userRegistration.LastName} Successfully Registered",
                    Severity.Success
                );
                NavigationManager.NavigateTo("/login");

                // Reset the user registration form.
                userRegistration = new EmailRegistrationViewModel();
            }
        }

        /// <summary>
        /// Toggles whether the password field is displayed in plain text or masked.
        /// Updates the icon accordingly.
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
        /// Asynchronously initializes the component and retrieves the list of available roles.
        /// </summary>
        /// <remarks>This method fetches the roles from the server and filters out roles that are not
        /// available  for registration selection. It also sets the default selected role to "Parent" if
        /// available.</remarks>
        /// <returns></returns>
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
