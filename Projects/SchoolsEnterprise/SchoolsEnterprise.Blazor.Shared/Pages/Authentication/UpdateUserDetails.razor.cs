using System.Security.Claims;
using ConectOne.Blazor.Extensions;
using IdentityModule.Application.ViewModels;
using IdentityModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace SchoolsEnterprise.Blazor.Shared.Pages.Authentication
{
    /// <summary>
    /// The UpdateUserDetails component retrieves a user's details and allows 
    /// modifying and saving updated user information, such as basic info, social 
    /// settings, address, or other preferences. It uses:
    /// 1. MudTabs for a tab-based layout.
    /// 2. An IAccountsProvider to fetch and update user data.
    /// 3. A Snackbar for user feedback on save or error actions.
    /// </summary>
    public partial class UpdateUserDetails
    {
        private UserInfoViewModel _accountHolder = null!;
        private ClaimsPrincipal User = default!;
        private readonly MudTabs _tabs = null!;
        private MudTabPanel _detailsTab = null!;
        private MudTabPanel _socialSettingsTab = null!;
        private MudTabPanel _addressTab = null!;
        private MudTabPanel _settingsTab = null!;

        /// <summary>
        /// Provides the current authentication state (including user claims) from Blazor’s cascaded parameter.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// An injected account provider service used to retrieve and update user information.
        /// </summary>
        [Inject] public IUserService UserService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="ISnackbar"/> service used to display notifications or messages to the user.
        /// </summary>
        /// <remarks>The <see cref="ISnackbar"/> service is typically used to display brief, non-intrusive
        /// messages to the user,  such as notifications, warnings, or confirmations. Ensure that the service is
        /// properly injected before use.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used for handling navigation in the application.
        /// </summary>
        /// <remarks>This property is typically injected by the framework and should not be manually set
        /// in most scenarios.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// The user ID passed in via query string or URL route, representing 
        /// the user whose details we are editing.
        /// </summary>
        [Parameter] public string Id { get; set; } = null!;

        /// <summary>
        /// Activates a specific tab panel within the MudTabs.
        /// </summary>
        /// <param name="tabPanel">The panel to make active.</param>
        private void ToTab(MudTabPanel tabPanel)
        {
            _tabs.ActivatePanel(tabPanel);
        }

        /// <summary>
        /// Saves the current user details by calling the <see cref="AccountProvider"/>'s update method.
        /// Displays success or error messages in a SnackBar.
        /// </summary>
        public async Task Save()
        {
            var response = await UserService.UpdateUserInfoAsync(_accountHolder.ToDto());
            if (response.Succeeded)
            {
                // On success, show a success message
                SnackBar.Add($"Action Successful. User \"{_accountHolder.DisplayName}\" was successfully updated.");
            }
            else
            {
                // On error, show the same or a different message
                SnackBar.Add($"Action failed. Could not update \"{_accountHolder.DisplayName}\". Please try again.");
            }
        }

        /// <summary>
        /// Cancels the edit operation and navigates back to the home page ("/").
        /// </summary>
        public void Cancel()
        {
            NavigationManager.NavigateTo("/");
        }

        /// <summary>
        /// Called when the component initializes. 
        /// 1. Retrieves the currently authenticated user (claims).
        /// 2. Fetches the target user's info from the server.
        /// 3. Shows any errors in a SnackBar if retrieval fails.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            User = authState.User;

            // Fetch the user information by ID
            var response = await UserService.GetUserInfoAsync(Id);
            if (response.Succeeded)
            {
                // Map the returned data into the UserInfoViewModel for binding
                _accountHolder = new UserInfoViewModel(response.Data!);
            }
            else
            {
                // Show errors if the user fetch operation fails
                SnackBar.AddErrors(response.Messages);
            }
        }
    }
}
