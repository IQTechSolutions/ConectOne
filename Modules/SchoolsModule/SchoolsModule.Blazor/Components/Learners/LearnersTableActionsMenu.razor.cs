using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using SchoolsModule.Domain.Constants;

namespace SchoolsModule.Blazor.Components.Learners
{
    /// <summary>
    /// Represents the action menu for managing learners in a table.
    /// </summary>
    public partial class LearnersTableActionsMenu
    {
        private bool _canEdit;
        private bool _canDelete;
        private bool _canSendMessage;
        private bool _cancreateChat;
        private UserInfoDto? _userInfo;

        #region Parameters & Injections

        /// <summary>
        /// Cascading parameter for the authentication state task.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Injected provider for account-related operations.
        /// </summary>
        [Inject] public IUserService? UserService { get; set; } = null!;

        /// <summary>
        /// Injects the authorization service for checking user permissions.
        /// </summary>
        [Inject] public IAuthorizationService AuthorizationService { get; set; } = null!;

        /// <summary>
        /// Event callback for removing a learner.
        /// </summary>
        [Parameter] public EventCallback<string> RemoveLearner { get; set; }

        /// <summary>
        /// The ID of the learner.
        /// </summary>
        [Parameter, EditorRequired] public string LearnerId { get; set; } = null!;

        /// <summary>
        /// Indicates whether to ignore registration.
        /// </summary>
        [Parameter] public bool IgnoreRegistration { get; set; } = false;

        /// <summary>
        /// Indicates whether to display the delete button.
        /// </summary>
        [Parameter] public bool DisplayDeleteButton { get; set; } = true;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether the RemoveLearner event callback is set.
        /// </summary>
        private bool IsRemoveLearnerSet => RemoveLearner.HasDelegate;

        #endregion

        #region Methods

        /// <summary>
        /// Invokes the RemoveLearner event callback.
        /// </summary>
        private async Task OnRemoveLearner()
        {
            await RemoveLearner.InvokeAsync(LearnerId);
        }

        /// <summary>
        /// Initializes the component asynchronously.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;

            // Check if the user has permission to edit or delete learners
            _canEdit = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.LearnerPermissions.Edit)).Succeeded;
            _canDelete = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.LearnerPermissions.Delete)).Succeeded;
            _canSendMessage = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.LearnerPermissions.SendMessage)).Succeeded;
            _cancreateChat = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.LearnerPermissions.CreateChat)).Succeeded;
            

            // Retrieve user information for the learner
            var userInfoResult = await UserService.GetUserInfoAsync(LearnerId);
            if (userInfoResult is { Succeeded: true, Data: not null })
            {
                _userInfo = userInfoResult.Data;
            }

            await base.OnInitializedAsync();
        }

        #endregion
    }
}
