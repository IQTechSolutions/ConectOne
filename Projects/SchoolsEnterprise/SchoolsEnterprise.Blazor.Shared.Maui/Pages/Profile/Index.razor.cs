using System.Security.Claims;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using ConectOne.Domain.Interfaces;
using IdentityModule.Domain.Constants;
using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Extensions;
using IdentityModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using MudBlazor;
using SchoolsModule.Domain.DataTransferObjects;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Profile
{
    /// <summary>
    /// Represents a profile page in a Blazor application that displays 
    /// user-related data, such as their main user info and 
    /// learner- or parent-specific data, depending on the user's role.
    /// </summary>
    public partial class Index
    {
        private bool isLoading = true;
        private ClaimsPrincipal? _user;

        #region Cascading Parameters & Services

        /// <summary>
        /// Provides the current authentication state for the user.
        /// This is cascaded from a parent component (e.g., MainLayout) 
        /// to give access to identity claims.
        /// </summary>
        [CascadingParameter]  public Task<AuthenticationState> AuthenticationTask { get; set; } = null!;

        /// <summary>
        /// Injected service for making HTTP requests.
        /// </summary>
        [Inject] public IBaseHttpProvider Provider { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications to the user.
        /// </summary>
        /// <remarks>This property is typically provided via dependency injection. Use this service to
        /// show transient messages or alerts in the application's user interface.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application configuration settings.
        /// </summary>
        /// <remarks>This property is typically populated by dependency injection and provides access to
        /// configuration values such as connection strings, application settings, and environment variables.</remarks>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display dialogs to the user.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the NavigationManager used to manage and interact with URI navigation in the application.
        /// </summary>
        /// <remarks>The NavigationManager provides methods and properties for programmatically navigating
        /// to different URIs and for responding to navigation events. This property is typically set by the Blazor
        /// framework via dependency injection.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the provider used to access account-related operations.
        /// </summary>
        [Inject] public IAccountsProvider AccountsProvider { get; set; } = null!;

        #endregion

        #region Private Fields

        /// <summary>
        /// If the user is a Learner, this property holds their learner-specific data. 
        /// This data is fetched from <see cref="IBaseHttpProvider"/>.
        /// </summary>
        private LearnerDto? _learner;

        /// <summary>
        /// Holds general user information for the profile (e.g., name, display name, email).
        /// Fetched from <see cref="IBaseHttpProvider"/>.
        /// </summary>
        private UserInfoDto? _userInfo;

        /// <summary>
        /// If the current user is a parent, this collection stores all learners 
        /// associated with that parent. For example, a parent with multiple children
        /// in the system would see all their learners here.
        /// </summary>
        private ICollection<LearnerDto> _learners = [];

        /// <summary>
        /// If the current user is a learner, this collection stores the parents 
        /// (if any) linked to them. For instance, a learner might have multiple 
        /// guardians/parents in the system.
        /// </summary>
        private ICollection<ParentDto> _parents = [];

        /// <summary>
        /// A placeholder or demonstration path/URL for the user's profile image.
        /// In a real implementation, this might come from <c>_userInfo</c> 
        /// or from an uploaded resource.
        /// </summary>
        private string _profileImage = "images/pictures/faces/3s.png";

        /// <summary>
        /// Handles the removal of a user profile, including confirmation and subsequent actions.
        /// </summary>
        /// <remarks>This method displays a confirmation dialog to the user before proceeding with the
        /// profile removal. If the user confirms, it attempts to remove the user from the application. If the user is a
        /// parent, additional cleanup is performed to remove the parent-specific data. Upon successful removal, the
        /// user is logged out and redirected to the application's home page.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task OnProfileRemove()
        {

            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this parent from this application?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };
            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var removalResult = await Provider.DeleteAsync($"account/users/removeUser", _user.GetUserId());
                removalResult.ProcessResponseForDisplay(SnackBar, async () =>
                {
                    if (_user.IsInRole(RoleConstants.Parent))
                    {
                        var deleteResult = await Provider.DeleteAsync("parents", _userInfo.UserId);
                        if (deleteResult.Succeeded)
                        {
                            SnackBar.AddSuccess("Parent removed successfully");
                        }
                    }
                    else
                    {
                        SnackBar.AddSuccess("User info removed successfully");
                        
                    }
                    await AccountsProvider.Logout();
                    NavigationManager.NavigateTo("/");
                });
            }
        }

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Called when the component is being initialized. Fetches data 
        /// based on the user's role:
        /// <list type="bullet">
        ///   <item><description>User Info: Basic account details (e.g., name, email).</description></item>
        ///   <item><description>If Learner: Also fetch learner object and parent(s) data.</description></item>
        ///   <item><description>Otherwise: Assumed to be Parent or another role, so fetch learners data.</description></item>
        /// </list>
        /// This method is awaited, ensuring data is loaded before rendering completes.
        /// </summary>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                // Retrieve the user's claims principal from the cascading parameter.
                var auth = await AuthenticationTask;
                _user = auth.User;
                var emailAddress = auth.User.GetUserEmail();

                if (auth.User.IsInRole(RoleConstants.Parent))
                {
                    var userInfoRequest = await Provider.GetAsync<ParentDto>($"parents/byemail/{emailAddress}");
                    if (userInfoRequest.Succeeded)
                    {
                        _userInfo = userInfoRequest.Data.GetUserInfoDto();

                        var learnersResult = await Provider.GetAsync<List<LearnerDto>>($"parents/parentlearners/{_user.GetUserId()}");
                        if (learnersResult.Succeeded)
                            _learners = learnersResult.Data;
                    }
                }
                else if (auth.User.IsInRole(RoleConstants.Learner))
                {
                    var userInfoRequest = await Provider.GetAsync<LearnerDto>($"learners/byemail/{emailAddress}");
                    if (userInfoRequest.Succeeded)
                    {
                        _userInfo = userInfoRequest.Data.GetUserInfoDto();

                        var learnersResult = await Provider.GetAsync<List<ParentDto>>($"learners/learnerparents/{_user.GetUserId()}");
                        if (learnersResult.Succeeded)
                            _parents = learnersResult.Data;
                    }
                }
                else if (auth.User.IsInRole(RoleConstants.Teacher))
                {
                    var userInfoRequest = await Provider.GetAsync<TeacherDto>($"teachers/byemail/{emailAddress}");
                    if (userInfoRequest.Succeeded)
                    {
                        _userInfo = userInfoRequest.Data.GetUserInfoDto();
                    }
                }
                else
                {
                    var userInfoRequest = await Provider.GetAsync<UserInfoDto>($"account/users/{_user.GetUserId()}");
                    if (userInfoRequest.Succeeded)
                    {
                        _userInfo = userInfoRequest.Data;
                    }
                }

                isLoading = false;

                // Complete the base initialization after data retrieval.
                await base.OnInitializedAsync();
                StateHasChanged();
            }
        }

        #endregion

        #region Navigation/Interaction Methods

        /// <summary>
        /// Navigates to a detailed page or view for a specific learner, 
        /// typically used when the user clicks a learner in the UI.
        /// </summary>
        /// <param name="learnerId">The unique identifier for the learner to navigate to.</param>
        public void NavigateToLearnerDetails(string learnerId)
        {
           // NavigationManager.NavigateTo($"/learners/{learnerId}");
        }

        /// <summary>
        /// An example method for navigating to parent details. 
        /// Currently implemented the same as <see cref="NavigateToLearnerDetails"/>,
        /// but can be customized for a parent-based route.
        /// </summary>
        /// <param name="learnerId">The learner's ID used in the route to display parent details.</param>
        public void NavigateToParentDetails(string learnerId)
        {
            NavigationManager.NavigateTo($"/learners/{learnerId}");
        }

        #endregion
    }
}
