using System.Security.Claims;
using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Interfaces;
using IdentityModule.Application.ViewModels;
using IdentityModule.Domain.Constants;
using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using SchoolsModule.Domain.DataTransferObjects;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Profile
{
    /// <summary>
    /// The <c>Editor</c> component allows a logged-in user to view and modify their profile data,
    /// including whether they receive notifications, emails, and messages.
    /// It retrieves the user information from the server and then,
    /// depending on the user’s role (SuperUser, Learner, or Parent),
    /// calls the appropriate service methods to update the data.
    /// </summary>
    public partial class Editor
    {
        #region Injected Services and Parameters

        /// <summary>
        /// The user's authentication state is cascaded from a higher-level component.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Gets or sets the HTTP provider used for making HTTP requests.
        /// </summary>
        [Inject] public IBaseHttpProvider Provider { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications to the user.
        /// </summary>
        /// <remarks>This property is typically provided via dependency injection. Use the snack bar
        /// service to show transient messages or alerts within the application's user interface.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the navigation manager used to programmatically navigate and obtain information about the
        /// current URI.
        /// </summary>
        /// <remarks>This property is typically provided by dependency injection in Blazor applications.
        /// Use it to navigate to different pages or to access the current navigation state.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Optionally specifies which user ID to edit. If not provided, the code defaults to the current user's ID.
        /// </summary>
        [Parameter] public string? UserId { get; set; }

        #endregion

        #region Private Fields

        /// <summary>
        /// Indicates whether the initial loading of user data from the server is complete.
        /// </summary>
        public bool _loaded;

        /// <summary>
        /// Stores all editable info for the user's profile. Updated by the UI toggles and saved by UpdateAsync().
        /// </summary>
        private UserInfoViewModel _userInfo = new UserInfoViewModel();

        /// <summary>
        /// The current logged-in user (used to check roles, ID, etc.).
        /// </summary>
        private ClaimsPrincipal _user;

        #endregion

        #region Toggle Handlers

        /// <summary>
        /// Called when toggling "Receive Notifications" in the UI.
        /// </summary>
        private void ToggleRequireConsent(bool value) => _userInfo.RequireConsent = value;

        /// <summary>
        /// Called when toggling "Receive Notifications" in the UI.
        /// </summary>
        private void ToggleNotification(bool value) => _userInfo.ReceiveNotifications = value;

        /// <summary>
        /// Called when toggling "Receive Emails" in the UI.
        /// </summary>
        private void ToggleEmails(bool value) => _userInfo.RecieveEmails = value;

        /// <summary>
        /// Called when toggling "Receive Messages" in the UI.
        /// </summary>
        private void ToggleMessages(bool value) => _userInfo.ReceiveMessages = value;

        /// <summary>
        /// Called when toggling "Show Email Address" in the UI.
        /// </summary>
        private void ShowEmailAddress(bool value) => _userInfo.ShowEmailAddress = value;

        /// <summary>
        /// Called when toggling "Show Phone Number" in the UI.
        /// </summary>
        private void ShowPhoneNr(bool value) => _userInfo.ShowPhoneNr = value;

        #endregion

        #region Data Update Logic

        /// <summary>
        /// Saves any changed data in <c>_userInfo</c> to the server. 
        /// The logic differs based on the user's role (SuperUser, Learner, Parent).
        /// 
        /// 1. Update the base user info using the <see cref="AccountsProvider"/>.
        /// 2. If user is Learner, we also call the <see cref="ParentsProvider"/> to update the parent entity.
        /// 3. If user is Parent, we also call the <see cref="LearnersProvider"/> to update the learner entity.
        /// </summary>
        public async Task UpdateAsync()
        {
            //var userId = _user.GetUserId();

            //// Step 1: Update the general user info
            //var userInfoResult = await Provider.PostAsync("account/users/update", new UserInfoDto(_userInfo){UserId = userId});

            //// If updating user info succeeded, check the user's role
            //if (userInfoResult.Succeeded)
            //{
                // If user is the SuperUser, just navigate back to a main profile page
                if (_user.IsInRole(RoleConstants.SuperUser))
                {
                    var parentResponse = await Provider.PostAsync("account/users/update", _userInfo.ToDto());
                    parentResponse.ProcessResponseForDisplay(SnackBar, () =>
                    {
                        // On success, navigate back to the profile
                        NavigationManager.NavigateTo($"/profile");
                    });
                }
                // If the user is in a "Learner" role, call the ParentsProvider to update them as a parent entity
                else if (_user.IsInRole(RoleConstants.Learner))
                {
                    var parentResponse = await Provider.PostAsync("learners", new LearnerDto(_userInfo.ToDto()));
                    parentResponse.ProcessResponseForDisplay(SnackBar, () =>
                    {
                        // On success, navigate back to the profile
                        NavigationManager.NavigateTo($"/profile/{UserId}");
                    });
                }
                // If the user is in a "Parent" role, call the LearnersProvider to update them as a learner entity
                else if (_user.IsInRole(RoleConstants.Parent))
                {
                    var parentResponse = await Provider.PostAsync("parents/updateprofile", new ParentDto(_userInfo.ToDto())); 
                    parentResponse.ProcessResponseForDisplay(SnackBar, () =>
                    {
                        // On success, navigate back to the profile
                        NavigationManager.NavigateTo($"/profile");
                    });
                }
                else
                {
                var parentResponse = await Provider.PostAsync("account/users/update", _userInfo.ToDto());
                parentResponse.ProcessResponseForDisplay(SnackBar, () =>
                {
                    // On success, navigate back to the profile
                    NavigationManager.NavigateTo($"/profile");
                });
            }
            //}
            //else
            //{
            //    // If there was an error updating user info, show the server's error message
            //    SnackBar.AddErrors(userInfoResult.Messages);
            //}
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Executes after the component has rendered and handles the initial data load on the first render.
        /// </summary>
        /// <param name="firstRender">Indicates whether this is the first time the component is rendered.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            
            if (firstRender)
            {
                _loaded = false;

                var authState = await AuthenticationStateTask;
                _user = authState.User;
                var emailAddress = authState.User.GetUserEmail();

                if (authState.User.IsInRole(RoleConstants.Parent))
                {
                    var userInfoRequest = await Provider.GetAsync<ParentDto>($"parents/byemail/{emailAddress}");
                    if (userInfoRequest.Succeeded)
                    {
                        _userInfo = new UserInfoViewModel(userInfoRequest.Data.GetUserInfoDto());
                    }
                }
                else if (authState.User.IsInRole(RoleConstants.Learner))
                {
                    var userInfoRequest = await Provider.GetAsync<LearnerDto>($"learners/byemail/{emailAddress}");
                    if (userInfoRequest.Succeeded)
                    {
                        _userInfo = new UserInfoViewModel(userInfoRequest.Data.GetUserInfoDto());
                    }
                }
                else if (authState.User.IsInRole(RoleConstants.Teacher))
                {
                    var userInfoRequest = await Provider.GetAsync<TeacherDto>($"teachers/byemail/{emailAddress}");
                    if (userInfoRequest.Succeeded)
                    {
                        _userInfo = new UserInfoViewModel(userInfoRequest.Data.GetUserInfoDto());
                    }
                }
                else
                {
                    var userInfoRequest = await Provider.GetAsync<UserInfoDto>($"account/users/{_user.GetUserId()}");
                    if (userInfoRequest.Succeeded)
                    {
                        _userInfo = new UserInfoViewModel(userInfoRequest.Data);
                    }
                }

                _loaded = true;

                StateHasChanged();
            }
        }

        #endregion
    }
}
