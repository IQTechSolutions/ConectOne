using System.Text;
using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Entities;
using IdentityModule.Application.ViewModels;
using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace EversdalPrimary.Blazor.ServerClient.Components.Account
{
    /// <summary>
    /// Represents a registration form component that handles user sign-up, role selection, and account creation
    /// workflows in the application.
    /// </summary>
    /// <remarks>The RegisterForm component manages the process of registering new users, including validating
    /// user input, associating users with specific roles (such as Parent, Learner, or Teacher), and sending email
    /// confirmation links. It integrates with various services to verify user information and ensure that only valid
    /// users are registered. This component is typically used as part of the application's user onboarding
    /// flow.</remarks>
    public partial class RegisterForm
    {
        private IEnumerable<IdentityError>? identityErrors;
        private List<string> _availableRoles = [];

        /// <summary>
        /// Gets or sets the data submitted by the user for email registration.
        /// </summary>
        [SupplyParameterFromForm] private EmailRegistrationViewModel Input { get; set; } = default!;

        /// <summary>
        /// Gets or sets the URL to which the user is redirected after a successful operation.
        /// </summary>
        /// <remarks>This property is typically used to preserve the user's intended destination when
        /// authentication or other intermediate steps are required. If not specified, the default redirect behavior is
        /// applied.</remarks>
        [SupplyParameterFromQuery] private string? ReturnUrl { get; set; }

        /// <summary>
        /// Gets a formatted error message that summarizes the identity errors, if any are present.
        /// </summary>
        private string? Message => identityErrors is null ? null : $"Error: {string.Join(", ", identityErrors.Select(error => error.Description))}";

        /// <summary>
        /// Asynchronously initializes the component and loads the list of available roles for registration selection.
        /// </summary>
        /// <remarks>This method retrieves all roles from the role service and filters them to include
        /// only those available for registration selection. The first available role named "Parent" is selected by
        /// default, if present.</remarks>
        /// <returns>A task that represents the asynchronous initialization operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            Input ??= new();

            var response = await RoleService.AllRoles();
            response.ProcessResponseForDisplay(SnackBar, () =>
            {
                _availableRoles = response.Data.Where(c => !c.NotAvailableForRegistrationSelection).Select(c => c.Name).ToList();
            });

            Input.SelectedOption = _availableRoles.FirstOrDefault(c => c == "Parent");
        }

        /// <summary>
        /// Registers a new user account using the information provided in the current input model and the specified
        /// edit context.
        /// </summary>
        /// <remarks>The registration process validates the user's role and email address against existing
        /// records for parents, learners, or teachers, and creates a new user account if validation succeeds. If email
        /// confirmation is required, the user is redirected to a confirmation page; otherwise, the user is signed in
        /// automatically. Error messages are displayed via the application's notification system if registration fails
        /// at any stage.</remarks>
        /// <param name="editContext">The edit context containing the state and validation information for the user registration form. Cannot be
        /// null.</param>
        /// <returns>A task that represents the asynchronous registration operation.</returns>
        public async Task RegisterUser(EditContext editContext)
        {
            // If the user selected "Parent", check if there's a parent record with the given email.
            if (Input.SelectedOption == "Parent")
            {
                var parentResponse = await ParentQueryService.ParentByEmailAsync(Input.Email);
                if (!parentResponse.Succeeded)
                {
                    var message = $"No parent matching email address '{Input.Email}'";
                    SnackBar.AddError(message);
                    return;
                }
                Input.UserId = parentResponse.Data.ParentId;
            }

            // If the user selected "Learner", check if there's a learner record with the given email.
            if (Input.SelectedOption == "Learner")
            {
                var learnerResponse = await LearnerQueryService.LearnerByEmailAsync(Input.Email);
                if (!learnerResponse.Succeeded)
                {
                    var message = $"No learner matching email address '{Input.Email}'";
                    SnackBar.AddError(message);
                    return;
                }
                Input.UserId = learnerResponse.Data.LearnerId;
            }

            // If the user selected "Teacher", check if there's a teacher record with the given email.
            if (Input.SelectedOption == "Teacher")
            {
                var teacherResponse = await TeacherService.TeacherAsync(Input.Email);
                if (!teacherResponse.Succeeded)
                {
                    var message = $"No teacher matching email address '{Input.Email}'";
                    SnackBar.AddError(message);
                    return;
                }
                Input.UserId = teacherResponse.Data.TeacherId;
            }

            Input.Role = Input.SelectedOption;

            var user = new ApplicationUser(Guid.NewGuid().ToString(), Input.Email, Input.FirstName, Input.LastName, Input.PhoneNumber, Input.Email, "", "");
            user.IsActive = true;
            await UserStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
            var result = await UserManager.CreateAsync(user, Input.Password);
            if (!result.Succeeded)
            {
                identityErrors = result.Errors;
                return;
            }

            if (!string.IsNullOrWhiteSpace(Input.SelectedOption))
            {
                await UserManager.AddToRolesAsync(user, new List<string> { Input.SelectedOption });
            }

            if (!result.Succeeded)
            {
                identityErrors = result.Errors;
                return;
            }

            var userInfo = new UserInfoDto()
            {
                UserId = user.Id,
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                PhoneNr = Input.PhoneNumber,
                EmailAddress = Input.Email
            };

            var userInfoResult = await UserService.UpdateUserInfoAsync(userInfo);

            if (!userInfoResult.Succeeded) SnackBar.AddErrors(userInfoResult.Messages);

            var userId = await UserManager.GetUserIdAsync(user);
            var code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = NavigationManager.GetUriWithQueryParameters(
                NavigationManager.ToAbsoluteUri("Account/ConfirmEmail").AbsoluteUri,
                new Dictionary<string, object?> { ["userId"] = userId, ["code"] = code, ["returnUrl"] = ReturnUrl });

            var _applicationConfiguration = Configuration.GetSection(typeof(ApplicationConfiguration).Name).Get<ApplicationConfiguration>();

            var fromEmailAddress = string.IsNullOrWhiteSpace(_applicationConfiguration.DoNotReplyEmailAddress)
                ? Configuration["EmailConfiguration:From"]
                : _applicationConfiguration.DoNotReplyEmailAddress;

            var fromName = string.IsNullOrWhiteSpace(_applicationConfiguration.AppliactionName)
                ? Configuration["EmailConfiguration:caption"]
                : _applicationConfiguration.AppliactionName;

            var emailResult = await EmailSender.SendUserVerificationEmailAsync(
                $"{Input.FirstName} {Input.LastName}".Trim(),
                user.Email!,
                fromName,
                fromEmailAddress,
                callbackUrl);

            Logger.LogInformation("User created a new account with password.");

            if (UserManager.Options.SignIn.RequireConfirmedAccount)
            {
                RedirectManager.RedirectTo(
                    "Account/RegisterConfirmation",
                    new() { ["email"] = Input.Email, ["returnUrl"] = ReturnUrl });
            }
            else
            {
                await SignInManager.SignInAsync(user, isPersistent: false);
                RedirectManager.RedirectTo(ReturnUrl);
            }
        }
    }
}
